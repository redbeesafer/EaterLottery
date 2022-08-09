using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EaterLottery
{
    public partial class EaterLotteryFrm : Form
    {
        public EaterLotteryFrm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 取得AppSettings
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        private static string GetAppSetting(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings.AllKeys.Contains(key) ? appSettings[key].ToString() : "";
        }

        /// <summary>
        /// 設定AppSettings
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        private static void SetAppSetting(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            config.AppSettings.Settings[key].Value = value;
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 開抽囉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            string[] stores = GetAppSetting("store").Split(',').OrderBy(x => r.Next()).ToArray();
            string store; //今天抽的
            List<string> oldStores = GetAppSetting("oldStore").Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(); //昨天抽的
            do
            {
                //至少抽一次....
                store = stores[r.Next(0, stores.Length)];
            }
            while (oldStores.Contains(store));

            //5家就重置....
            if (oldStores.Count >= 5)
                oldStores.Clear();

            //加入設定檔
            oldStores.Add(store);

            //存檔
            SetAppSetting("oldStore", string.Join(",", oldStores));

            MessageBox.Show(store, "恭喜抽中");
        }
    }
}
