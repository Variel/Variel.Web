using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Variel.Web.Common
{
    public class SettingsProvider
    {
        readonly ISettingsDatabaseContext _database;
        readonly IConfiguration _config;

        public SettingsProvider(ISettingsDatabaseContext database, IConfiguration config)
        {
            _database = database;
            _config = config;
        }

        public string GetStatic(string key)
            => _config.GetValue<string>(key);

        public T GetStatic<T>(string key)
            => _config.GetValue<T>(key);

        public string GetConnectionString(string name)
            => _config.GetConnectionString(name);

        public string GetString(string key)
            => _database.AppSettings.Find(key)?.Content;

        public object GetObject(string key)
            => _database.AppSettings.Find(key)?.AsObject();

        public async Task<string> GetStringAsync(string key)
            => (await _database.AppSettings.FindAsync(key))?.Content;

        public async Task<object> GetObjectAsync(string key)
            => (await _database.AppSettings.FindAsync(key))?.AsObject();

        public string this[string key]
            => GetString(key);

        public void SetString(string key, string value)
        {
            var setting = _database.AppSettings.Find(key);
            if (setting == null)
            {
                setting = new AppSetting { Content = value, IsJson = false, Key = key };
                _database.AppSettings.Add(setting);
            }
            else
            {
                setting.Content = value;
            }

            _database.SaveChangesAsync().Wait();
        }
    }
}
