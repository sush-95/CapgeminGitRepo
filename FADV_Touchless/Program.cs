using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.IO;
using DataAccess_Utility;
using Read_File_Processor;
using Newtonsoft.Json.Linq;

namespace FADV_Touchless
{
    class Program
    {
        static void Main(string[] args)
        {
            //JsonCreater JsonCreater = new Read_File_Processor.JsonCreater();
            //string jsonDownload = JsonCreater.getData_FreshCaseResponse();

            //SendResponse();

            //Test_DownloadResponse();
            while (true)
            {
                ExecuteFile();
                System.Threading.Thread.Sleep(2000);
            }
        }


        static void ExecuteFile()
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
                //System.Threading.Thread.Sleep(2000);
                ////obj.Read_Response();
                string output = obj1.Create_Case_Creation_Json_For_FreshCase();



            }
            catch (Exception ex)
            {
                objDML.Add_Exception_Log("Wipro exception : " + ex.Message, "");
                Console.WriteLine(ex.Message.ToString());
                throw ex;

            }
        }

        static void Test_DownloadResponse()
        {

            //Get_Data_Utility objGet = new Get_Data_Utility();
            DML_Utility objDML = new DML_Utility();
            ExecuteProcess obj = new ExecuteProcess();
            JsonCreater JsonCreater = new Read_File_Processor.JsonCreater();
            RabbitMQ_Utility objQueue = new RabbitMQ_Utility();
            try
            {
                string output = "";
                string error = "";
                string MessageId = "2eb1d289-f9e5-4d80-9834-3550fcc0da48";
                string jsonDownload = JsonCreater.getDownload_Response(MessageId, "Y");
                bool ret = objQueue.Rabbit_Send_Response_Queue(jsonDownload, "Response", "localhost", out error);
            }
            catch (Exception ex)
            {
                int iException = objDML.Add_Exception_Log("Wipro exception : " + ex.Message, "");
                iException = objDML.Add_Exception_Log(ex.Message, "Main Function");
            }
        }

    }
}
