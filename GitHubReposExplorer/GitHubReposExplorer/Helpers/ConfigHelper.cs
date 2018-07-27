using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace GitHubReposExplorer.Helpers
{
    public class ConfigHelper
    {
        private const string username = "username";
        private const string accessToken = "access-token";
        private ConfigHelper()
        { }

        private static readonly ConfigHelper instance = new ConfigHelper();

        public static ConfigHelper Instance
        {
            get
            {
                return instance;
            }
        }

        public string UserName
        {
            get
            {
                return GetConfigValueForKey(username);
            }
        }

        public string AccessToken
        {
            get
            {
                return GetConfigValueForKey(accessToken);
            }
        }

        private string GetConfigValueForKey(string key)
        {
            var configFile = "GitHubReposExplorer.Config.Config.xml";
            string[] a = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            using (var stream = this.GetType().Assembly.GetManifestResourceStream(configFile))
            using (var reader = new StreamReader(stream))
            {
                var doc = XDocument.Parse(reader.ReadToEnd());
                return doc.Element("config").Element(key).Value;
            }
        }
    }
}
