using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Discord_Bot
{
    internal static class Config
    {
        private const string ConfigFolder = "Resources";
        private const string ConfigFile = "config.json";
        private const string ChannelFile = "channels.json";
        private const string UserFile = "users.json";

        public static BotConfig Bot;
        public static BotIds Channels;
        public static BotIds Users;

        static Config()
        {
            if (!Directory.Exists(ConfigFolder))
                Directory.CreateDirectory(ConfigFolder);
            if (!File.Exists(ConfigFolder + "/" + ConfigFile))
            {
                Bot = new BotConfig();
                string json = JsonConvert.SerializeObject(Bot, Formatting.Indented);
                File.WriteAllText(ConfigFolder + "/" + ConfigFile, json);
            }
            else
            {
                string json = File.ReadAllText(ConfigFolder + "/" + ConfigFile);
                Bot = JsonConvert.DeserializeObject<BotConfig>(json);
            }
            if (!File.Exists(ConfigFolder + "/" + ChannelFile))
            {
                Channels = new BotIds();
                string json = JsonConvert.SerializeObject(Channels, Formatting.Indented);
                File.WriteAllText(ConfigFolder + "/" + ChannelFile, json);
            }
            else
            {
                string json = File.ReadAllText(ConfigFolder + "/" + ChannelFile);
                Channels = JsonConvert.DeserializeObject<BotIds>(json);
            }
            if (!File.Exists(ConfigFolder + "/" + UserFile))
            {
                Users = new BotIds();
                string json = JsonConvert.SerializeObject(Users, Formatting.Indented);
                File.WriteAllText(ConfigFolder + "/" + UserFile, json);
            }
            else
            {
                string json = File.ReadAllText(ConfigFolder + "/" + UserFile);
                Users = JsonConvert.DeserializeObject<BotIds>(json);
            }
        }
        public static bool AddChannelAdmin(ulong id)
        {
            if (Channels.Admin.Contains(id)) return false;
            Channels.Admin.Add(id);
            return true;
        }
        public static bool AddChannelUser(ulong id)
        {
            if (Channels.User.Contains(id)) return false;
            Channels.User.Add(id);
            return true;
        }

        public static bool AddUserAdmin(ulong id)
        {
            if (Users.Admin.Contains(id)) return false;
            Users.Admin.Add(id);
            return true;
        }
        public static bool AddUserUser(ulong id)
        {
            if (Users.User.Contains(id)) return false;
            Users.User.Add(id);
            return true;
        }
        public static void Save()
        {
            var json = JsonConvert.SerializeObject(Bot, Formatting.Indented);
            File.WriteAllText(ConfigFolder + "/" + ConfigFile, json);
            json = JsonConvert.SerializeObject(Channels, Formatting.Indented);
            File.WriteAllText(ConfigFolder + "/" + ChannelFile, json);
            json = JsonConvert.SerializeObject(Users, Formatting.Indented);
            File.WriteAllText(ConfigFolder + "/" + UserFile, json);
        }
    }

    public struct BotConfig
    {
        public string Token;
        public string CmdPrefix;
    }

    public struct BotIds
    {
        public List<ulong> Admin;
        public List<ulong> User;
    }
}
