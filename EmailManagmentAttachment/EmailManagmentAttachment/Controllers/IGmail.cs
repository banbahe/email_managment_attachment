using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Data;

namespace EmailManagmentAttachment.Controllers
{
    interface IGmail
    {
        Message GetMessage(GmailService service, String userId, String messageId);

        bool GetAttachments(GmailService service, String userId, String messageId);

        List<Message> ListMessages(GmailService service, String userId, String query);
    }
}
