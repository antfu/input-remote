using InputRemote.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InputRemote.Client.Components
{
    public class AddressHelper
    {
        public static string GetWsAddress(bool input = false)
        {
            var settings = SettingHelper.GetSetting("setting.json");

            if (!input)
            {
                if (settings["ws_url"] != null)
                {
                    return settings["ws_url"];
                }
                else if (settings["http_url"] != null)
                {
                    settings["ws_url"] = WebsocketAgent.HttpToWsUrl(settings["http_url"]);
                    return settings["ws_url"];
                }
            }

            var url_input = new AddressInput();
            url_input.Url = settings["ws_url"] ?? "";

            if (url_input.ShowDialog() == true)
            {
                settings["http_url"] = url_input.Url;
                settings["ws_url"] = WebsocketAgent.HttpToWsUrl(url_input.Url);
                return settings["ws_url"];
            }
            else
                // Input canceled
                return null;
        }
    }
}
