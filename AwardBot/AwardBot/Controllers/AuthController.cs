using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Configuration;
using System.Threading.Tasks;

namespace AwardBot.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Login()
        {
            AuthenticationContext authContext = new AuthenticationContext(Settings.microsoftLoginUrl);
            var authUri = authContext.GetAuthorizationRequestUrlAsync(
                Settings.graphApiEndpoint,
                ConfigurationManager.AppSettings["ClientId"],
                new Uri($"{ConfigurationManager.AppSettings["AppUrl"]}/Auth/Authorize"),
                UserIdentifier.AnyUser,
                null
                );
            return Redirect(authUri.Result.ToString());
        }

        /*
        public async Task<ActionResult> Authorize(string code)
        {
            //Get access token
            var authContext = new AuthenticationContext(Settings.microsoftLoginUrl);
            var authResult = await authContext.AcquireTokenByAuthorizationCodeAsync(
                code,
                new Uri($"{ConfigurationManager.AppSettings["AppUrl"]}/Auth/Authorize"),
                new ClientCredential(
                    ConfigurationManager.AppSettings["ClientId"],
                    ConfigurationManager.AppSettings["ClientSecret"]
                    )
                );

            #region Get access token and refresh token without ADAL. Because we can't get refresh token from ADAL authContext.<= refresh token is non-public members...
            Token token;
            using (var httpClient = new HttpClient())
            {
                HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type","authorization_code" },
                    {"client_id",ConfigurationManager.AppSettings["ClientId"] },
                    { "client_secret",ConfigurationManager.AppSettings["ClientSecret"]},
                    {"redirect_uri", $"{ConfigurationManager.AppSettings["AppUrl"]}/Auth/Authorize"},
                    { "code",code }
                });
                //content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                HttpResponseMessage response = await httpClient.PostAsync(Settings.tokenEndpoint, content);
                token = await response.Content.ReadAsAsync<Token>();
            }
            #endregion

            if (authResult.UserInfo.DisplayableId.Contains(Settings.domain))
            {
                //Initialize Storage Account and Initizalize Clients
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                #region Set Access Token & Refresh Token in Azue Storage Table
                //Get Table reference
                CloudTable tokenTable = tableClient.GetTableReference(Settings.tokenTableName);

                //Update Access Token
                TableOperation accessTokenQuery = TableOperation.Retrieve<TokenEntity>(Settings.partitionKeyName, Settings.accessTokenRowName);
                TokenEntity accessTokenEntity = (TokenEntity)tokenTable.Execute(accessTokenQuery).Result;
                accessTokenEntity.Token = token.access_token;
                tokenTable.Execute(TableOperation.Replace(accessTokenEntity));

                //Update Refresh Token
                TableOperation refreshTokenQuery = TableOperation.Retrieve<TokenEntity>(Settings.partitionKeyName, Settings.refreshTokenRowname);
                TokenEntity refreshTokenEntity = (TokenEntity)tokenTable.Execute(refreshTokenQuery).Result;
                refreshTokenEntity.Token = token.refresh_token;
                tokenTable.Execute(TableOperation.Replace(refreshTokenEntity));
                #endregion

                //Send queueMessage for fetching 
                await fetchingUserprofileQueue.AddMessageAsync(new CloudQueueMessage("users"));

                ViewBag.Message = "認証/認可完了しました。データ収集を開始します。";
            }
            else
            {
                ViewBag.Message = "あなたは対象ドメインではありません。データ収集を行いませんでした。";
            }
            return View();
        }
        */
    }
}