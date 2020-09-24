using Read_File_Processor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FADV_Service_download
{
    public partial class Service1 : ServiceBase
    {

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            while (true)
            {
                //ExecuteProcess obj = new ExecuteProcess();
                //string error = obj.Write_JSON_TO_Download();
                System.Threading.Thread.Sleep(5000);
            }
        }

        protected override void OnStop()
        {
        }
    }
}
