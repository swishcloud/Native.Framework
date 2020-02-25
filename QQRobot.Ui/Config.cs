using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQRobot.Ui
{
    public class Config
    {
        public static Config Instance { get; set; }
        private const string path_root = ".qqrobot";
        private const string path_forbidden_words_regex_config_file = path_root+ "/forbidden_words_regex_config";
        static Config()
        {
            Instance = new Config();
            Directory.CreateDirectory(path_root);
        }
        public void SaveRegexsConfig(string[] regexs)
        {
            File.WriteAllText(path_forbidden_words_regex_config_file,string.Join("\r\n",regexs));
        }
        public string[] ReadRegexsConfig()
        {
            if (!File.Exists(path_forbidden_words_regex_config_file))
            {
                return new List<string>().ToArray();
            }
            var str = File.ReadAllText(path_forbidden_words_regex_config_file);
            return str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
