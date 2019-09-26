using JCZPT.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCZPT.Util
{
    public class ConfigManageUtil
    {
        #region 字段
        private static ConfigManageUtil instance;
        private Dictionary<String, String> configDic = new Dictionary<string, string>();

        #endregion
        #region 属性

        public Dictionary<String, String> ConfigDic
        {
            get { return configDic; }
            set { configDic = value; }
        }
        #endregion
        #region 公共方法
        public static ConfigManageUtil GetInstance()
        {
            if(instance==null)
            {
                instance = new ConfigManageUtil();
            }
            return instance;
        }
        public void ReadConfig()
        {
            
            String configValue = String.Empty;
            foreach (String key in ConfigurationManager.AppSettings.Keys)
            {
                configValue = ConfigurationManager.AppSettings[key];
                this.ConfigDic.Add(key, configValue);
            }
            
        }
        public void WriteConfig(String key,String newValue)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = newValue;
            config.AppSettings.SectionInformation.ForceSave = true;
            config.Save();
            //刷新，否则程序读取的还是之前的值（可能已装入内存）
            ConfigurationManager.RefreshSection("appSettings");
        }
        public void AddConfig(String newKey, String value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add(newKey,value);
            config.AppSettings.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);
            //刷新，否则程序读取的还是之前的值（可能已装入内存）
           ConfigurationManager.RefreshSection("appSettings");
        }
        #endregion
        #region 私有方法
        private ConfigManageUtil()
        {

        }
        #endregion
    }
}
