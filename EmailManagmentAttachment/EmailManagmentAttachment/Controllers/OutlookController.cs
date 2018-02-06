using System;
using System.IO;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using MimeKit;

namespace EmailManagmentAttachment.Controllers
{
    public class OutlookController
    {
        private readonly string _pathinit = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.Create), "emailmanagmentattachment", "iblanquelhotmailcom", "tmp");

        private IAttach attach;

        public OutlookController(IAttach _attach)
        {
            this.attach = _attach;
        }
        public void Read()
        {
            using (var client = new ImapClient())
            {
                // For demo-purposes, accept all SSL certificates
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                //client.Connect("imap-mail.outlook.com", 993, true);
                //client.Connect("imap.mail.yahoo.com", 993, true);

                //client.Authenticate("@outlook.com", "*****");
                //client.Authenticate("@hotmail.com", ""*****");
                //client.Authenticate("account", ""*****");

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                DateTime dateTime = new DateTime(1994, 1, 1);

                var personal = client.GetFolder(client.PersonalNamespaces[0]);

                foreach (var item in personal.GetSubfolders(StatusItems.None, false))
                {
                    ListUid(item);
                }

                //foreach (var item in personal.GetSubfolders(StatusItems.None, false))
                //{

                //    ListUid(item);
                //    //IEnumerable<IMailFolder> test = item.GetSubfolders(StatusItems.None, false);
                //    // item.Open(FolderAccess.ReadOnly);
                //    //IList<UniqueId> uids2 = item.Search(SearchQuery.All);
                //    //foreach (UniqueId item2 in uids2)
                //    //{
                //    //    MimeMessage message = item.GetMessage(item2);
                //    //    Console.WriteLine("Folder{1} Subject: {0}", message.Subject, item.Name);
                //    //}

                //}


                IList<UniqueId> uids = client.Inbox.Search(SearchQuery.SentAfter(dateTime));

                foreach (UniqueId item in uids)
                {
                    MimeMessage message = client.Inbox.GetMessage(item);
                    Console.WriteLine("Subject: {0}", message.Subject);

                }

                //Console.WriteLine("Total messages: {0}", inbox.Count);
                //Console.WriteLine("Recent messages: {0}", inbox.Recent);

                //for (int i = 0; i < inbox.Count; i++)
                //{
                //    var message = inbox.GetMessage(i);
                //    Console.WriteLine("Subject: {0}", message.Subject);
                //}
                // start




                // end
                client.Disconnect(true);
                Console.Read();
            }
        }

        private int indice;
        private IList<UniqueId> ListUid(IMailFolder mailFolder)
        {
            mailFolder.Open(FolderAccess.ReadOnly);
            IList<UniqueId> list = mailFolder.Search(SearchQuery.All);
            Console.WriteLine("Folder{0} ", mailFolder.Name);

            foreach (UniqueId item2 in list)
            {
                try
                {


                    MimeMessage message = mailFolder.GetMessage(item2);

                    foreach (var attachment in message.Attachments)
                    {

                        if (attachment is MessagePart)
                        {
                            var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
                            var rfc822 = (MessagePart)attachment;
                            rfc822.Message.WriteTo(Path.Combine(this._pathinit, fileName));
                            attach.file = fileName;
                            attach.source = Path.Combine(this._pathinit, fileName);
                            attach.CheckPath();
                            attach.Move();
                        }
                        else
                        {
                            var part = (MimePart)attachment;
                            var fileName = part.FileName;

                            using (var stream = File.Create(Path.Combine(this._pathinit, fileName)))
                            {
                                part.Content.DecodeTo(stream);
                            }
                            attach.file = fileName;
                            attach.source = Path.Combine(this._pathinit, fileName);
                            attach.CheckPath();
                            attach.Move();

                        }
                    }

                    var getChilds = mailFolder.GetSubfolders(StatusItems.None, false);
                    if (getChilds.Count() > 0)
                    {
                        foreach (var item in getChilds)
                        {
                            if (item.Name == ".before attachs" || item.Name == ".before heart")
                                continue;
                            else
                                this.ListUid(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }




            return list;
        }
    }
}
