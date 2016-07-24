using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Antnf.KeyboardRemote.Tools
{
    public class SettingHelper
    {
        private dynamic settings_obj;
        private string filepath;
        private string json;
        public SettingHelper(string _filepath)
        {
            this.filepath = _filepath;
            if (!File.Exists(this.filepath))
                File.CreateText(this.filepath).Close();
            this.json = File.ReadAllText(this.filepath);
            if (this.json == "")
                this.json = "{}";
            this.settings_obj = JsonConvert.DeserializeObject(this.json);
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
                this.settings_obj[key] = value;
                this.json = JsonConvert.SerializeObject(this.settings_obj);
                File.WriteAllText(this.filepath, this.json);
            }
        }
    }
}
