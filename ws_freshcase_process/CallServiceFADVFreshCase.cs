using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Read_File_Processor;
using System.Timers;

namespace ws_freshcase_process
{
    public partial class CallServiceFADVFreshCase : ServiceBase
    {
        Timer timer = new Timer();
        public CallServiceFADVFreshCase()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Servicelog();
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000; //number in milisecinds  
            timer.Enabled = true;
        }
        public void Servicelog()
        {
            try
            {
                CaseCreationProcessor obj = new CaseCreationProcessor();
                //string output = obj.Create_Case_Creation_Json_For_FreshCase();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Infosys exception : " + ex.Message.ToString());
                throw ex;

            }
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            Servicelog();
        }

        protected override void OnStop()
        {
        }
    }
}
