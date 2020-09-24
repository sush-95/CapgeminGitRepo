using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Read_File_Processor;


namespace ws_yettostart_process
{
    public partial class CallServiceFADVYetToStart : ServiceBase
    {
        Timer timer = new Timer();
        public CallServiceFADVYetToStart()
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
                ExecuteProcess obj = new ExecuteProcess();
                string output = obj.Execute_Excel_YetToStart_Process_Download();
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
