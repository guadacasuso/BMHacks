﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Graph;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace AwardBot.Services
{
    public class MessageService
    {
        // Send an email message from the current user.
        //public async Task<string> SendEmail(string accessToken, MessageRequest email)
        //{
        //    string endpoint = "https://graph.microsoft.com/v1.0/me/sendMail";
        //    using (var client = new HttpClient())
        //    {
        //        using (var request = new HttpRequestMessage(HttpMethod.Post, endpoint))
        //        {
        //            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //            // This header has been added to identify our sample in the Microsoft Graph service. If extracting this code for your project please remove.
        //            request.Headers.Add("SampleID", "aspnet-connect-rest-sample");
        //            request.Content = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");
        //            using (HttpResponseMessage response = await client.SendAsync(request))
        //            {
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    return Resource.Graph_SendMail_Success_Result;
        //                }
        //                return response.ReasonPhrase;
        //            }
        //        }
        //    }
        //}

        //// Create the email message.
        //public MessageRequest BuildEmailMessage(string recipients, string subject)
        //{

        //    // Prepare the recipient list.
        //    string[] splitter = { ";" };
        //    string[] splitRecipientsString = recipients.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
        //    List<Recipient> recipientList = new List<Recipient>();
        //    foreach (string recipient in splitRecipientsString)
        //    {
        //        recipientList.Add(new Recipient
        //        {
        //            EmailAddress = new UserInfo
        //            {
        //                Address = recipient.Trim()
        //            }
        //        });
        //    }

        //    // Build the email message.
        //    Message message = new Message
        //    {
        //        Body = new ItemBody
        //        {
        //            Content = Resource.Graph_SendMail_Body_Content,
        //            ContentType = "HTML"
        //        },
        //        Subject = subject,
        //        ToRecipients = recipientList
        //    };

        //    return new MessageRequest
        //    {
        //        Message = message,
        //        SaveToSentItems = true
        //    };
        //}
        //}
    }
}