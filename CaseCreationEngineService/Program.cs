using CaseCreationEngineService.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CaseCreationEngineService
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
            //    new Service1()
            //};
            //ServiceBase.Run(ServicesToRun);

            while (true)
            {
                using (FetchDataManager caseCreation = new FetchDataManager())
                {
                    caseCreation.getData();
                }
            }
        }
    }
}
