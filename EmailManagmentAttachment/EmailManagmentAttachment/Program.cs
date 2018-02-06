using EmailManagmentAttachment.Controllers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
namespace EmailManagmentAttachment
{
    class Program
    {
        static string[] Scopes = { GmailService.Scope.GmailReadonly };
        static string ApplicationName = "Gmail.API.NET.Quickstart";
        static string pathinit = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.Create), "emailmanagmentattachment", "config");


        static void Main(string[] args)
        {
            IAttach attach = new AttachController();
            OutlookController outlookController = new OutlookController(attach);
            outlookController.Read();
            throw new Exception("");
            //
            IGmail gmail = new GmailController(attach);

            UserCredential credential;

            using (var stream = new FileStream(@pathinit + @"\client_secret.json", FileMode.Open, FileAccess.Read))
            {
                try
                {

                    //  string credPath = System.Environment.GetFolderPath(
                    //System.Environment.SpecialFolder.Personal);
                    string credPath = pathinit;
                    credPath = Path.Combine(credPath, ".credentials/gmail-dotnet-quickstart.json");

                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                    throw;
                }
            }

            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            UsersResource.LabelsResource.ListRequest request = service.Users.Labels.List("me");
            // start read mail
            //UsersResource.MessagesResource.ListRequest requestMessage = service.Users.Messages.List("me");
            //IList<Message> listMessage = requestMessage.Execute().Messages;

            IList<Message> listMessage = gmail.ListMessages(service, "me", "has:attachment");
            foreach (var item in listMessage)
            {
                var res = gmail.GetMessage(service, "me", item.Id);
                gmail.GetAttachments(service, "me", res.Id);
            }

            Console.Read();
        }


        ///// <summary>
        ///// Retrieve a Message by ID.
        ///// </summary>
        ///// <param name="service">Gmail API service instance.</param>
        ///// <param name="userId">User's email address. The special value "me"
        ///// can be used to indicate the authenticated user.</param>
        ///// <param name="messageId">ID of Message to retrieve.</param>
        //public static Message GetMessage(GmailService service, String userId, String messageId)
        //{
        //    try
        //    {
        //        return service.Users.Messages.Get(userId, messageId).Execute();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("An error occurred: " + e.Message);
        //    }

        //    return null;
        //}

        //public static void GetAttachments(GmailService service, String userId, String messageId, String outputDir)
        //{
        //    try
        //    {
        //        Message message = service.Users.Messages.Get(userId, messageId).Execute();
        //        if (message.Payload.Parts != null)
        //        {
        //            IList<MessagePart> parts = message.Payload.Parts;
        //            foreach (MessagePart part in parts)
        //            {
        //                if (!String.IsNullOrEmpty(part.Filename))
        //                {
        //                    String attId = part.Body.AttachmentId;
        //                    MessagePartBody attachPart = service.Users.Messages.Attachments.Get(userId, messageId, attId).Execute();

        //                    // Converting from RFC 4648 base64 to base64url encoding
        //                    // see http://en.wikipedia.org/wiki/Base64#Implementations_and_history
        //                    String attachData = attachPart.Data.Replace('-', '+');
        //                    attachData = attachData.Replace('_', '/');

        //                    byte[] data = Convert.FromBase64String(attachData);

        //                    File.WriteAllBytes(Path.Combine(outputDir, part.Filename), data);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("An error occurred: " + e.Message);
        //    }
        //}

        //public static List<Message> ListMessages(GmailService service, String userId, String query)
        //{
        //    List<Message> result = new List<Message>();
        //    UsersResource.MessagesResource.ListRequest request = service.Users.Messages.List(userId);
        //    request.Q = query;

        //    do
        //    {
        //        try
        //        {
        //            ListMessagesResponse response = request.Execute();
        //            result.AddRange(response.Messages);
        //            request.PageToken = response.NextPageToken;
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine("An error occurred: " + e.Message);
        //        }
        //    } while (!String.IsNullOrEmpty(request.PageToken));

        //    return result;
        //}



    }
}
