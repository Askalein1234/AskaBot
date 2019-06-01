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
        private const string oneConf = "config.json";

        private static Bot config;

        private struct Bot
        {
            public BotGeneral general;
            public Dictionary<ulong, BotServer> servers;
        }

        private struct BotGeneral
        {
            public BotConfig bot;
            public BotIds users;
        }

        private struct BotServer
        {
            public string name;
            public BotConfig bot;
            public BotIds channels;
            public BotIds users;
            public List<Bot_otm_IDs> dependencies;
        }

        private struct BotConfig
        {
            public string token;
            public string cmdPrefix;
        }

        private struct BotIds
        {
            public List<ulong> admin;
            public List<ulong> user;
        }

        private struct Bot_otm_IDs
        {
            public ulong one;
            public List<ulong> many;
        }

        static Config()
        {
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);
            if (!File.Exists(configFolder + "/" + oneConf))
            {
                config = new Bot();
                string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(configFolder + "/" + oneConf, json);
            }
            else
            {
                string json = File.ReadAllText(configFolder + "/" + oneConf);
                config = JsonConvert.DeserializeObject<Bot>(json);
            }
        }

        public static void Save()
        {
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(configFolder + "/" + oneConf, json);
        }

        public static string GetGlobalPrefix()
        {
            return config.general.bot.cmdPrefix;
        }

        public static string GetToken()
        {
            return config.general.bot.token;
        }

        public static bool KnowsServer(ulong id)
        {
            return config.servers.TryGetValue(id, out BotServer server);
        }

        private static bool KnowsServer(ulong id, out BotServer server)
        {
            return config.servers.TryGetValue(id, out server);
        }

        public static string GetServerPrefix(ulong id)
        {
            if (!config.servers.TryGetValue(id, out BotServer serverConfig)) return config.general.bot.cmdPrefix;
            return serverConfig.bot.cmdPrefix ?? config.general.bot.cmdPrefix;
        }

        public static List<ulong> GetDependencies(ulong serverId, ulong roleId)
        {
            List<ulong> dependencies = new List<ulong>();
            if (!config.servers.TryGetValue(serverId, out BotServer server)) return dependencies;
            IEnumerable<Bot_otm_IDs> deps = server.dependencies.Where(x => x.many.Contains(roleId));
            foreach(Bot_otm_IDs dep in deps)
            {
                dependencies.Add(dep.one);
            }
            return dependencies;
        }

        public static bool AddServer(ulong id, string name)
        {
            UpdateServerName(id, name);
            if (KnowsServer(id)) return false;
            BotServer newServer = new BotServer
            {
                name = name
            };
            newServer.channels.admin = new List<ulong>();
            newServer.channels.user = new List<ulong>();
            newServer.users.admin = new List<ulong>();
            newServer.users.user = new List<ulong>();
            newServer.dependencies = new List<Bot_otm_IDs>();
            config.servers.Add(id, newServer);
            Save();
            return true;
        }

        public static string UpdateServerName(ulong id, string name)
        {
            if (!config.servers.TryGetValue(id, out BotServer server)) return null;
            string oldName = server.name;
            server.name = name;
            return oldName;
        }
    }
}
