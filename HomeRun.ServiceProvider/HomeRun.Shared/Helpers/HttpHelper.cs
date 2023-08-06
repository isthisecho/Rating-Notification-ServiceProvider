using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeRun.Shared.Helpers
{
    public class HttpHelper
    {
        public static StringContent GetJsonHttpContent(object items)
        {
            return new StringContent(JsonConvert.SerializeObject(items), Encoding.UTF8, "application/json");
        }

        public static class Urls
        {
            public readonly static string SubmitRating = "/Rating";
            public readonly static string GetNewNotifications = "/Notification/";

        }
    }
}