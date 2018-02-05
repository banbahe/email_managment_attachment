
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace EmailManagmentAttachment.Controllers
{
    public class GmailController : IGmail
    {
        string pathinit = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.Create), "emailmanagmentattachment", "tmp");

        protected IAttach attach;
        public GmailController(IAttach _attach)
        {
            this.attach = _attach;
        }
        public bool GetAttachments(GmailService service, string userId, string messageId)
        {
            bool flag = false;
            //string outputDir = "temp";
            try
            {
                Message message = service.Users.Messages.Get(userId, messageId).Execute();
                if (message.Payload.Parts != null)
                {
                    IList<MessagePart> parts = message.Payload.Parts;
                    foreach (MessagePart part in parts)
                    {
                        if (!String.IsNullOrEmpty(part.Filename))
                        {
                            String attId = part.Body.AttachmentId;
                            MessagePartBody attachPart = service.Users.Messages.Attachments.Get(userId, messageId, attId).Execute();
                            String attachData = attachPart.Data.Replace('-', '+');
                            attachData = attachData.Replace('_', '/');

                            byte[] data = Convert.FromBase64String(attachData);
                            string filetmp = Path.Combine(pathinit, part.Filename);
                            File.WriteAllBytes(filetmp, data);
                            attach.file = part.Filename;
                            attach.source = filetmp;
                            attach.CheckPath();
                            flag = attach.Move();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
            return flag;
        }

        public Message GetMessage(GmailService service, string userId, string messageId)
        {
            try
            {
                return service.Users.Messages.Get(userId, messageId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
            return null;
        }

        public List<Message> ListMessages(GmailService service, string userId, string query)
        {
            List<Message> result = new List<Message>();
            UsersResource.MessagesResource.ListRequest request = service.Users.Messages.List(userId);
            request.Q = query;

            do
            {
                try
                {
                    ListMessagesResponse response = request.Execute();
                    result.AddRange(response.Messages);
                    request.PageToken = response.NextPageToken;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                }
            } while (!String.IsNullOrEmpty(request.PageToken));

            return result;
        }
    }
}



