using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Read_File_Processor;
using DataAccess_Utility;

namespace ws_initialtracker_process
{
    public partial class CallServiceFADVInitialTracker : ServiceBase
    {
        //private System.Timers.Timer timer;
        public CallServiceFADVInitialTracker()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                //instantiate timer
                //Thread t = new Thread(new ThreadStart(this.InitTimer));
                //t.Start();
                Servicelog();
            }
            catch(Exception ex)
            {

            }
        }
        //private void InitTimer()
        //{
        //    timer = new System.Timers.Timer();
        //    //wire up the timer event 
        //    timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        //    //set timer interval   
        //    //var timeInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["TimerIntervalInSeconds"]); 
        //    double timeInSeconds = 3.0;
        //    timer.Interval = (timeInSeconds * 1000);
        //    // timer.Interval is in milliseconds, so times above by 1000 
        //    timer.Enabled = true;
        //}

        //protected void timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    int timer_fired = 0;
        //    Servicelog();
        //    timer.Enabled = false;
        //    timer.Close();
        //    timer.Dispose();

        //    ServiceController service = new ServiceController("CallServiceFADVInitialTracker");

        //    if ((service.Status.Equals(ServiceControllerStatus.Stopped)) ||

        //        (service.Status.Equals(ServiceControllerStatus.StopPending)))

        //        service.Start();

        //    else service.Stop();

        //}
        public void Servicelog()
        {
            DML_Utility objDML = new DML_Utility();

            try
            {
                ExecuteProcess obj = new ExecuteProcess();
                int Value = obj.Execute_Excel_InitialTracker();
            }
            catch (Exception ex)
            {
                int iException = objDML.Add_Exception_Log(ex.Message, "");

                Console.WriteLine("Wipro exception : " + ex.Message.ToString());
                throw ex;
            }
        }
        protected override void OnStop()
        {
            //timer.Enabled = false;
        }
    }
}
