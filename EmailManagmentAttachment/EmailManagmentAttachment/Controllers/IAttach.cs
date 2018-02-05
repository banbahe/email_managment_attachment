using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailManagmentAttachment.Controllers
{
    public interface IAttach
    {
        string file { get; set; }
        string source { get; set; }


        bool Move();

        bool CheckPath();


    }
}
