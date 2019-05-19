using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Discord_Bot
{
    class Config
    {
        private const string configFolder = "Resources";
        private const string configFile = "config.json";
        private const string channelFile = "channels.json";
        private const string userFile = "users.json";
        private const string gameFile = "gameRoles.json";

        public static BotConfig bot;
        public static BotIds channels;
        public static BotIds users;
        public static Bot_otm_IDs gameRoles;

        static Config()
        {
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);
            if (!File.Exists(configFolder + "/" + configFile))
            {
                bot = new BotConfig();
                string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
                File.WriteAllText(configFolder + "/" + configFile, json);
            }
            else
            {
                string json = File.ReadAllText(configFolder + "/" + configFile);
                bot = JsonConvert.DeserializeObject<BotConfig>(json);
            }
            if (!File.Exists(configFolder + "/" + channelFile))
            {
                channels = new BotIds();
                string json = JsonConvert.SerializeObject(channels, Formatting.Indented);
                File.WriteAllText(configFolder + "/" + channelFile, json);
            }
            else
            {
                string json = File.ReadAllText(configFolder + "/" + channelFile);
                channels = JsonConvert.DeserializeObject<BotIds>(json);
            }
            if (!File.Exists(configFolder + "/" + userFile))
            {
                users = new BotIds();
                string json = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllText(configFolder + "/" + userFile, json);
            }
            else
            {
                string json = File.ReadAllText(configFolder + "/" + userFile);
                users = JsonConvert.DeserializeObject<BotIds>(json);
            }
            if (!File.Exists(configFolder + "/" + gameFile))
            {
                gameRoles = new Bot_otm_IDs();
                string json = JsonConvert.SerializeObject(gameRoles, Formatting.Indented);
                File.WriteAllText(configFolder + "/" + gameFile, json);
            }
            else
            {
                string json = File.ReadAllText(configFolder + "/" + gameFile);
                gameRoles = JsonConvert.DeserializeObject<Bot_otm_IDs>(json);
            }
        }
        public static bool AddChannelAdmin(ulong id)
        {
            if (channels.admin.Contains(id)) return false;
            channels.admin.Add(id);
            return true;
        }
        public static bool AddChannelUser(ulong id)
        {
            if (channels.user.Contains(id)) return false;
            channels.user.Add(id);
            return true;
        }

        public static bool AddUserAdmin(ulong id)
        {
            if (users.admin.Contains(id)) return false;
            users.admin.Add(id);
            return true;
        }
        public static bool AddUserUser(ulong id)
        {
            if (users.user.Contains(id)) return false;
            users.user.Add(id);
            return true;
        }
        public static bool AddGamingRole(ulong id)
        {
            if (gameRoles.many.Contains(id)) return false;
            gameRoles.many.Add(id);
            return true;
        }
        public static void Save()
        {
            string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
            File.WriteAllText(configFolder + "/" + configFile, json);
            json = JsonConvert.SerializeObject(channels, Formatting.Indented);
            File.WriteAllText(configFolder + "/" + channelFile, json);
            json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(configFolder + "/" + userFile, json);
            json = JsonConvert.SerializeObject(gameRoles, Formatting.Indented);
            File.WriteAllText(configFolder + "/" + gameFile, json);
        }
    }

    public struct BotConfig
    {
        public string token;
        public string cmdPrefix;
    }

    public struct BotIds
    {
        public List<ulong> admin;
        public List<ulong> user;
    }
    public struct Bot_otm_IDs
    {
        public ulong one;
        public List<ulong> many;
    }
}
