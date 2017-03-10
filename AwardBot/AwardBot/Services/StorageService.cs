using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using AwardBot.Models;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using System.Diagnostics;

namespace AwardBot.Services
{
    public interface IStorageService
    {
        void SaveAccessToken(Token accessToken);
        void SaveRefreshToken(Token refreshToken);
        void InitializeStorage();
        string GetAccessToken();
    }

    public class AzureStorageService : IStorageService
    {
        CloudStorageAccount storageAccount;
        CloudTableClient tableClient;
        CloudTable tokenTable;

        public AzureStorageService()
        {
            storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());
            tableClient = storageAccount.CreateCloudTableClient();
            tokenTable = tableClient.GetTableReference(Settings.tokenTableName);
        }

        public void SaveAccessToken(Token token)
        {
            TableOperation accessTokenQuery = TableOperation.Retrieve<TokenEntity>(Settings.partitionKeyName, Settings.accessTokenRowName);
            TokenEntity accessTokenEntity = (TokenEntity)tokenTable.Execute(accessTokenQuery).Result;
            accessTokenEntity.Token = token.access_token;
            tokenTable.Execute(TableOperation.Replace(accessTokenEntity));
        }

        public string GetAccessToken()
        {
            TableOperation accessTokenQuery = TableOperation.Retrieve<TokenEntity>(Settings.partitionKeyName, Settings.accessTokenRowName);
            TokenEntity accessTokenEntity = (TokenEntity)tokenTable.Execute(accessTokenQuery).Result;
            //accessTokenEntity.Token = token.access_token;
            //tokenTable.Execute(TableOperation.Replace(accessTokenEntity));
            return accessTokenEntity.Token;
        }

        public void SaveRefreshToken(Token token)
        {
            TableOperation refreshTokenQuery = TableOperation.Retrieve<TokenEntity>(Settings.partitionKeyName, Settings.refreshTokenRowname);
            TokenEntity refreshTokenEntity = (TokenEntity)tokenTable.Execute(refreshTokenQuery).Result;
            refreshTokenEntity.Token = token.refresh_token;
            tokenTable.Execute(TableOperation.Replace(refreshTokenEntity));

        }

        public void InitializeStorage()
        {
            if (tokenTable.CreateIfNotExists())
            {
                TokenEntity accessTokenEntity = new TokenEntity(Settings.accessTokenRowName);
                TokenEntity refreshTokenEntity = new TokenEntity(Settings.refreshTokenRowname);
                TableBatchOperation batchOperation = new TableBatchOperation();
                batchOperation.Insert(accessTokenEntity);
                batchOperation.Insert(refreshTokenEntity);
                tokenTable.ExecuteBatch(batchOperation);
                Trace.TraceInformation("Storage initialized");
            }
            else
            {
                Trace.TraceInformation("Storage has been already initialized");
            }
        }
    }
}