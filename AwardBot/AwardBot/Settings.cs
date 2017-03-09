using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AwardBot
{
    public class Settings
    {
        //Endpoints
        public static string microsoftLoginUrl = "https://login.microsoftonline.com/common";
        public static string tokenEndpoint = "https://login.microsoftonline.com/common/oauth2/token";
        public static string graphApiEndpoint = "https://graph.microsoft.com";
    }
}