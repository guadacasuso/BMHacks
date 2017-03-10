using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Configuration;
using System.Threading.Tasks;
using AwardBot.Models;
using System.Net.Http;
using AwardBot.Services;

namespace AwardBot.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

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

            #region Set Access Token & Refresh Token in Azue Storage Table
            IStorageService storageService = new AzureStorageService();
            storageService.SaveAccessToken(token);
            storageService.SaveRefreshToken(token);
            #endregion

            ViewBag.Message = "Plase input group alias for using award bot";

            return View();
        }

        [HttpPost]
        public ActionResult Authorize(FormCollection collection)
        {
            string form = collection.GetValue("upn").AttemptedValue;
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}