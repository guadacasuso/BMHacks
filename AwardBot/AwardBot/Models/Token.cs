using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AwardBot.Models
{
    //Data model for Token table in Azure Storage Table
    public class TokenEntity : TableEntity
    {
        public TokenEntity(string tokenType)
        {
            this.PartitionKey = "token";
            this.RowKey = tokenType;
        }

        public TokenEntity() { }

        public string Token { get; set; }
    }

    public class Token
    {
        public string token_type { get; set; }
        public string scope { get; set; }
        public string expires_in { get; set; }
        public string ext_expires_in { get; set; }
        public string expires_on { get; set; }
        public string not_before { get; set; }
        public string resource { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string id_token { get; set; }
    }
}