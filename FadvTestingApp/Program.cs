using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FadvTestingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Email_Processor mailobj = new Email_Processor();
            mailobj.SendMail("", "sushil.beura@gridinfocom.com");
        }
    }
}
