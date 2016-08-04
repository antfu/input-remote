using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace KeyboardRemote.Tools
{
    public class SettingHelper
    {
        private static Dictionary<string, SettingHelper> cache = new Dictionary<string, SettingHelper>();
        private dynamic settings_obj;

        public string FilePath { get; private set; }
        public string Json
        {
            get
            {
                return JsonConvert.SerializeObject(this.settings_obj);
            }
        }
   
        /// <summary>
        /// SettingHelper Factory 
        /// Insure there is only one instance of a setting file
        /// </summary>
        /// <param name="_filepath"></param>
        /// <returns></returns>
        public static SettingHelper GetSetting(string _filepath)
        {
            var filepath = Path.GetFullPath(_filepath);
            // if cached
            if (cache.ContainsKey(filepath))
                return cache[filepath];
            // otherwise
            var setting = new SettingHelper(filepath);
            cache[filepath] = setting;
            return setting;
        }
        private SettingHelper(string _filepath)
        {
            this.FilePath = _filepath;
            if (!File.Exists(this.FilePath))
                File.CreateText(this.FilePath).Close();
            var json = File.ReadAllText(this.FilePath);
            // empty file
            if (json == "")
                json = "{}";
            this.settings_obj = JsonConvert.DeserializeObject(json);
        }
        public dynamic this[string key]
        {
            get
            {
                try
                {
                    return this.settings_obj[key];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                if (this.settings_obj[key] != value)
                {
                    this.settings_obj[key] = value;
                    File.WriteAllText(this.FilePath, this.Json);
                }
            }
        }
    }
}
