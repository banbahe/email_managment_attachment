using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace EmailManagmentAttachment.Controllers
{
    public class AttachController : IAttach
    {
        // string pathinit = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.Create), "emailmanagmentattachment", "dist");

        private readonly string _pathinit = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.Create), "emailmanagmentattachment", "dist");
        public string pathinit => _pathinit;

        public string file { get; set; }
        public string source { get; set; }

        public bool CheckPath()
        {
            try
            {
                string tmp = pathinit + @"\" + Path.GetExtension(file).ToLower();
                if (Directory.Exists(tmp))
                    return true;
                else
                {
                    Directory.CreateDirectory(tmp);
                    return true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public bool Move()
        {
            try
            {
                string tmp = Path.GetExtension(file).ToLower();
                tmp = Path.Combine(pathinit, tmp, name());
                File.Copy(source, tmp, true);
                File.Delete(source);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private string name()
        {
            return string.Concat(DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(), Path.GetExtension(file));
        }
    }
}
