using DataAccess_Utility;
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
using System.Timers;

namespace CapGemini_Service
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        
        public Service1()
        {
            InitializeComponent();
        }
        public bool isEngineExecuting;
        protected override void OnStart(string[] args)
        {
            try
            {
                //DebugMode();
                //Servicelog();
                timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
                timer.Interval = 5000; //number in milisecinds  
                timer.Enabled = true;
                //while (true)
                //{
                //    ExecuteProcess obj = new ExecuteProcess();
                //    string error = obj.Write_JSON_TO_Download();
                //    System.Threading.Thread.Sleep(5000);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wipro exception : " + ex.Message.ToString());
                throw ex;

            }
        }
        //[Conditional("DEBUG_SERVICE")]
        //private  void DebugMode()
        //{
        //    Servicelog();
        //    Debugger.Break();
        //}
        public void Servicelog()
        {
            DML_Utility objDML = new DML_Utility();

            try
            {
                //objDML.Add_Exception_Log("Service Start", "");
                ExecuteProcess obj = new ExecuteProcess();
                Get_Data_Utility objGet = new Get_Data_Utility();
               string error = obj.Write_JSON_TO_Download();
                //objDML.Add_Exception_Log("Download Complete", "");

                System.Threading.Thread.Sleep(2000);

                CaseCreationProcessor obj1 = new CaseCreationProcessor();
                    obj.Execute_Json_YetToStart_Process_Download();
                //objDML.Add_Exception_Log("FreshCase Request Sent", "");

                System.Threading.Thread.Sleep(2000);
                obj.Read_Response();
                string output = obj1.Create_Case_Creation_Json_For_FreshCase();
                //objDML.Add_Exception_Log("Service End", "");

            }
            catch (Exception ex)
            {
                objDML.Add_Exception_Log("Wipro exception : " + ex.Message, "");
                Console.WriteLine(ex.Message.ToString());
                throw ex;

            }
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            if (!isEngineExecuting)
            {
                try
                {
                    isEngineExecuting = true;
                    Servicelog();
                }
                catch (Exception ex)
                {
                    //appLogger.Error("Verification engine execution failure : " + ex.Message);
                    //TO-DO write code to send email.
                }
                finally
                {
                    isEngineExecuting = false;
                }

                //appLogger.Error("Service is recall at " + DateTime.Now);
            }
        }
        
        protected override void OnStop()
        {
        }
    }
}
