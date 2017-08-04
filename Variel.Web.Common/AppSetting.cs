using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace Variel.Web.Common
{
    public class AppSetting
    {
        [Key, MaxLength(150)]
        public string Key { get; set; }
        public string Content { get; set; }
        public bool IsJson { get; set; }

        public object AsObject() => IsJson ? JsonConvert.DeserializeObject(Content) : Content;


        public AppSetting() { }

        public AppSetting(string key, string content)
        {
            Key = key;
            Content = content;
            IsJson = false;
        }

        public AppSetting(string key, object content)
        {
            Key = key;
            Content = JsonConvert.SerializeObject(content);
            IsJson = true;
        }
    }
}
