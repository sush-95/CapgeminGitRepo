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
using DataAccess_Utility;
using StateProcessor;

namespace ws_download_process
{
    public partial class CallServiceFADV : ServiceBase
    {

        Timer timer = new Timer();
        public CallServiceFADV()
        {
            InitializeComponent();
        }
        public bool isEngineExecuting;
        protected override void OnStart(string[] args)
        {
            try
            {
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

        public void Servicelog()
        {
            DML_Utility objDML = new DML_Utility();

            try
            {
                ExecuteProcess obj = new ExecuteProcess();
                Get_Data_Utility objGet = new Get_Data_Utility();
                string error = obj.Write_JSON_TO_Download();
                System.Threading.Thread.Sleep(2000);

                CaseCreationProcessor obj1 = new CaseCreationProcessor();
                string output1 = obj.Execute_Excel_YetToStart_Process_Download();
                System.Threading.Thread.Sleep(2000);
                obj.Read_Response();
                string output = obj1.Create_Case_Creation_Json_For_FreshCase();

                //ExecuteProcess obj2 = new ExecuteProcess();
                //clsState state = new clsState();
                //List<tbl_request_state_instance> OpenStates = objGet.GetAllOpenState();
                //foreach (var item in OpenStates)
                //{
                //    state.SwitchAllOpenStates(item);
                //}

            }
            catch (Exception ex)
            {
                objDML.Add_Exception_Log(ex.Message, "");
                Console.WriteLine("Wipro exception : " + ex.Message.ToString());
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
