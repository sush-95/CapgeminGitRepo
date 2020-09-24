using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ws_download_process
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new CallServiceFADV()
            //};
            //ServiceBase.Run(ServicesToRun);

#if (!DEBUG)
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new CallServiceFADV()
            };
            ServiceBase.Run(ServicesToRun);
#else
            while (true)
                using (CallServiceFADV _services = new CallServiceFADV())
                {
                    _services.Servicelog();
                }
#endif
        }
    }
}
