using DataAccess_Utility;
using Read_File_Processor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fadv_Download_Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            //while(true)
            //{
            //    Execute();
            //}
            //Test_DownloadResponse();
            Execute();
        }

        static void Execute()
        {
            DML_Utility objDML = new DML_Utility();

            ExecuteProcess obj = new ExecuteProcess();
            CaseCreationProcessor obj1 = new CaseCreationProcessor();

            //objDML.Insert_Json_in_requesStateInstanse(Convert.ToInt64(338200), 1, "REQ-0002", 165, "Case Creation by Touchless", 1, 5, 0);
            //long NewRequestID = 23548;
            //string fileupload = "C:\\Users\\Grid\\Downloads\\My Received Files\\My Received Files";
            //string destinationPath = fileupload + "\\" + NewRequestID;
            //List<string> copiedFiles = FileUtility.FileUpload(fileupload, destinationPath);
            //objDML.Insert_FilePathIndocument_upload(copiedFiles, Convert.ToInt64(NewRequestID));


            string error = obj.Write_JSON_TO_Download();
            System.Threading.Thread.Sleep(2000);

            string output1 = obj.Execute_Excel_YetToStart_Process_Download();
            System.Threading.Thread.Sleep(2000);
            obj.Read_Response();
            //string output = obj1.Create_Case_Creation_Json_For_FreshCase();

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
                string MessageId = "a7520c5f-7245-4f91-aefc-6063785ccabe";
                string ServiceId = "DOWNLOAD";
                string jsonDownload = JsonCreater.getDownload_Response(MessageId, ServiceId, "Y");
                bool ret = objQueue.Rabbit_Send_Response_Queue(jsonDownload, "Response", "localhost", out error);
            }
            catch (Exception ex)
            {
                int iException = objDML.Add_Exception_Log(ex.Message, "");
                iException = objDML.Add_Exception_Log("Wipro exception : " + ex.Message, "Main Function");
            }
        }
    }
}
