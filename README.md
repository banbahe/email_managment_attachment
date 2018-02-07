# Email Managment Controller

[![N|Solid](https://farm5.staticflickr.com/4674/39226324365_3ab01e1c5c_o.png)](https://nodesource.com/products/nsolid)

Read all your attachs and store per whatever type  .


# Save and read any ahead mail provider

  - Support Yahoo, Outlook and Gmail
  - Requires your authorization and create setup )


Yahoo :
  - Server: imap.mail.yahoo.com
  - Port: 993
  - Requires SSL: Yes
 
Outlook :
  - Server:  imap-mail.outlook.com
  - Port: 993
  - Requires SSL: Yes

Gmail :
  - settings main account in https://developers.google.com/gmail/api/
  
### Installation

Install the dependencies requires NuGet 2.12 or higher.

```sh
$ Install-Package MailKit -Version 2.0.1	
$ Install-Package Google.Apis -Version 1.32.1	
```
