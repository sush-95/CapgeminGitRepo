using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.IO;
using DataAccess_Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Business_Entities;

namespace Read_File_Processor
{
    public class ExecuteProcess
    {


        public int Execute_Excel_InitialTracker()
        {
            int Value = 0;
            DML_Utility objDML = new DML_Utility();
            try
            {
                string strFilePath = "";
                string strFileName = "";
                // Get HostName //
                Get_Data_Utility objGet = new Get_Data_Utility();
                List<tbl_config_value> lstConfig = objGet.Get_Cofig_Details("MISDATA");
                foreach (var ob in lstConfig)
                {
                    strFilePath = ob.configstring;
                    strFileName = ob.FileName;
                }

                string path = strFilePath + strFileName; // 
                //FilePath_Container.FilePath + FilePath_Container.FileName_InitationTracker;
                if (File.Exists(path))
                {
                    DataTable dt = Read_Excel("Sheet1", path);
                    if (dt.Rows.Count > 0)
                    {
                        // Add Rows Count into table //
                        long rowcount = dt.Rows.Count;
                        objDML.Add_Rows_Count_Data(path, rowcount);
                        Value = addData(dt);
                    }
                    else
                    {
                        Value = 9; // No data exist
                    }
                }
            }
            catch (Exception ex)
            {
                Value = -1;
                ////// Exception Log ///
                int iException = objDML.Add_Exception_Log("Wipro exception : " + ex.Message, "");

            }
            return Value;
        }

        public string Read_Response()
        {
            DML_Utility objDML = new DML_Utility();
            string output = "";
            // Read RABITMQ for Response //
            string Response_Data = "";
            string Response_Error = "";
            try
            {
                RabbitMQ_Utility objQueue = new RabbitMQ_Utility();
                bool retResponse = objQueue.Receive(RabbitMQ_Utility.RabbitMQResponseQueue, out Response_Data, out Response_Error);
                //objDML.Add_Exception_Log(Response_Data, "");

                //objDML.Add_Exception_Log(Response_Data, "");
                if (!string.IsNullOrEmpty(Response_Data))
                {
                    string responce_MessageId = "";
                    string responce_ServiceId = "";

                    responce_MessageId = Read_Json_TagWise(Response_Data, "metadata", "requestId");
                    responce_ServiceId = Read_Json_TagWise(Response_Data, "metadata", "task");
                    //responce_ServiceId = Read_Json_Data_TagWise(Response_Data, "metadata", "task");
                    //long reqId = Convert.ToInt64(Read_Json_Data_TagWise(Response_Data, "data", "taskSerialNo"));
                    // Add Response into Database //
                    Get_Data_Utility obj = new Get_Data_Utility();
                    //DML_Utility objDML = new DML_Utility();
                    long reqId = obj.Get_Request_Id(responce_MessageId, responce_ServiceId);
                    //objDML.Add_Exception_Log("select * from tbl_request_details where messageid='" + responce_MessageId + "'", "");
                    //objDML.Add_Exception_Log(reqId.ToString(), "");

                    if (reqId > 0)
                    {
                        objDML.Add_Response_Json(reqId, Response_Data, responce_MessageId, responce_ServiceId);
                    }
                }
            }
            catch (Exception ex)
            {
                output = "exception : " + ex.Message.ToString();
                ////// Exception Log ///

                int iException = objDML.Add_Exception_Log("Wipro exception : " + ex.Message, "Read_Response");
            }
            return output;
        }
        public string ConvertToXLSX(string filePath)
        {
            // logger.Info("Convert to XLSX started.");
            DML_Utility objDML = new DML_Utility();

            try
            {
                string directoryName = Path.GetDirectoryName(filePath);
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string[] files = new string[1];
                string[] newFile = new string[1];
                files = Directory.GetFiles(directoryName, fileName + ".xls");
                var app = new Microsoft.Office.Interop.Excel.Application();
                //foreach (string file in files)
                //{
                var wb = app.Workbooks.Open(files[0].ToString());
                wb.SaveAs(Filename: files[0].ToString() + "x", FileFormat: Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
                wb.Close();
                app.Quit();
                File.Delete(filePath);
                newFile = Directory.GetFiles(directoryName, fileName + ".xlsx");
                filePath = Path.GetFullPath(newFile[0]).ToString();
                // }
                //logger.Info("Convert to XLSX completed.");
                return filePath;
            }
            catch (Exception ex)
            {
                objDML.Add_Exception_Log("Wipro exception : " + "Before conversion", "");
                //logger.Error("Error occured while converting xls to xslx. Error message: " + ex.Message.ToString());
                throw ex;
                //return null;
            }

        }

        public void Execute_Json_YetToStart_Process_Download()
        {
            Get_Data_Utility obj = new Get_Data_Utility();
            DML_Utility objDML = new DML_Utility();
            GetCapGeminiParameters_Freshcase paramsObj = new GetCapGeminiParameters_Freshcase();
            //objDML.Add_Exception_Log("", "Execute_Excel_YetToStart_Process_Download");
            JsonCreater JsonCreater = new Read_File_Processor.JsonCreater();

            try
            {
                string strDateKey = System.DateTime.Now.ToString("yyyyMMddHHmmss"), output = string.Empty;
                // GET RESONSE JSON TO BE PROCESSED 
                List<tbl_response_detail> lstResponse = obj.Get_Response_Data_ToBe_Process(FilePath_Container.ServiceId_WiproDownload);
                if (lstResponse.Count > 0)
                {
                    foreach (tbl_response_detail res in lstResponse)
                    {
                        JObject DownloadJboj = JObject.Parse(res.response_json);
                        string BoitId = DownloadJboj["metadata"]["botId"].ToString();
                        Email_Processor mailobj = new Email_Processor();
                        JArray DataArray = JArray.Parse(DownloadJboj["data"][0]["result"]["downloadDetails"].ToString());
                        foreach (var item in DataArray)
                        {
                            string candidateId = item["candidateId"].ToString();
                            string assigedTo = item["assigedTo"].ToString();
                            string Activity = item["Activity"].ToString();
                            string dueDate = item["dueDate"].ToString();
                            string startDate = item["startDate"].ToString();
                            string firstName = item["firstName"].ToString();
                            string lastName = item["lastName"].ToString();
                            string activityCreationDate = item["activityCreationDate"].ToString();
                            string placeOfPosting = item["placeOfPosting"].ToString();
                            string sbu = item["sbu"].ToString();
                            string bgvAgency = item["bvgAgency"].ToString();
                           int checkstat=objDML.InsertCapgeminiTable(candidateId, assigedTo, Activity, dueDate, startDate, firstName, lastName, activityCreationDate, placeOfPosting, sbu, bgvAgency, BoitId);

                            if (checkstat == 2)
                            {

                                mailobj.SendMail("", "", "");
                            }
                        }
                        //  objDML.Update_Response_Status(res.id);
                    }


                }
                List<tbl_cap_gemini> CapGeminiList = obj.Get_UnProcessedRequestsCapGemini();
                foreach (var item in CapGeminiList)
                {
                    if (obj.IsCapgeminiDuplicate(item.Candidate_ID))
                    {
                        if (CheckIfRequestSentOrnot(item.Candidate_ID))
                        {
                            string queueMessageId = Guid.NewGuid().ToString();
                            string queueServiceId = FilePath_Container.FreshCase;
                            output = JsonCreater.getDetails_FreshCase(paramsObj.GetCapgeminiParameter(item.Candidate_ID, item.First_Name, item.Last_Name), queueMessageId, queueServiceId);
                            int iDML = objDML.Add_Request_Json_Detail(queueMessageId, queueServiceId, output);
                            if (iDML == 1)
                            {
                                bool ret = Write_JSON_TO_RABBIT_MQ(output);
                                objDML.Add_Exception_Log("Capgemini: 1st attempt for CandidateId : " + item.Candidate_ID + " Sub- Login : Capgemini has been sent to rabbitMQ ", item.Candidate_ID);
                                output = ret ? "Success" : "Failed";
                            }
                        }                       
                    }
                }

            }
            catch(Exception ex)
            {
                
            }
        }
        public bool CheckIfRequestSentOrnot(string cadidateId)
        {
            bool res = false;
            try
            {
                fadv_touchlessEntities fadv = new fadv_touchlessEntities();
                string matchstring = "\"candidateId\":\"" + cadidateId.Replace("\"", "") + "\"";
                List<tbl_request_details> request = fadv.tbl_request_details.Where(x => x.json_text.Replace(" ", "").Contains(matchstring)).ToList();
                if (request.Count == 0)
                {
                    res = true;
                }
            }
            catch { }
            return res;
        }

        public void SendCaseDataToRabitMQ(string sublogin, string resumeid, string empname)
        {
            //try
            //{

            //    DML_Utility objDML = new DML_Utility();
            //    string queueMessageId = Guid.NewGuid().ToString();
            //    string queueServiceId = FilePath_Container.FreshCase;
            //    JsonCreater objjson = new JsonCreater();
            //    string output = objjson.getDetails_FreshCase(resumeid, empname, queueMessageId, sublogin, queueServiceId);
            //    int iDML = objDML.Add_Request_Json_Detail(queueMessageId, queueServiceId, output);
            //    if (iDML == 1)
            //    {
            //        bool ret = Write_JSON_TO_RABBIT_MQ(output);
            //        output = ret ? "Success" : "Failed";
            //    }
            //}
            //catch (Exception ex)
            //{

            //}

        }
        public string Process_Excel_File()
        {
            string output = "";

            Get_Data_Utility obj = new Get_Data_Utility();
            DML_Utility objDML = new DML_Utility();
            JsonCreater JsonCreater = new Read_File_Processor.JsonCreater();
            try
            {

            }
            catch (Exception ex)
            {
                output = "exception : " + ex.Message.ToString();
                ////// Exception Log ///
                //DML_Utility objDML = new DML_Utility();
                int iException = objDML.Add_Exception_Log("Wipro exception : " + ex.Message, "");
            }
            return output;

        }

        //public string Execute_Excel_YetToStart()
        //{
        //    string output = "";

        //    Get_Data_Utility obj = new Get_Data_Utility();
        //    DML_Utility objDML = new DML_Utility();
        //    JsonCreater JsonCreater = new Read_File_Processor.JsonCreater();
        //    try
        //    {
        //        //Get_Data_Utility obj = new Get_Data_Utility();
        //        //DML_Utility objDML = new DML_Utility();
        //        string strDateKey = System.DateTime.Now.ToString("yyyyMMddHHmmss");

        //        // Read RABITMQ for Response //
        //        string Response_Data = "";
        //        string Response_Error = "";
        //        RabbitMQ_Utility objQueue = new RabbitMQ_Utility();

        //        //bool retResponse = objQueue.Receive("Response", out Response_Data, out Response_Error);
        //        bool retResponse = objQueue.Receive(RabbitMQ_Utility.RabbitMQResponseQueue, out Response_Data, out Response_Error);

        //        if (!string.IsNullOrEmpty(Response_Data))
        //        {
        //            string responce_MessageId = "";
        //            string responce_Status = "";
        //            string responce_Module = "";
        //            //string RequestType = "DOWNLOAD";
        //            string path = Read_Json(Response_Data, out responce_MessageId, out responce_Status, out responce_Module);

        //            if (responce_Status.ToLower() == "y" || responce_Status.ToLower() == "yes")
        //            {
        //                // Add Response into Database //
        //                long reqId = obj.Get_Request_Id(responce_MessageId, "DOWNLOAD");
        //                if (reqId > 0)
        //                {
        //                    // Add Response to Database Table //
        //                    if (objDML.Add_Response_Json(reqId, Response_Data) > 0)
        //                    {
        //                        if (!string.IsNullOrEmpty(path))
        //                        {
        //                            if (File.Exists(path))
        //                            {
        //                                //string path = FilePath_Container.FilePath + FilePath_Container.FileName_YetToStart;
        //                                DataTable dt = Read_Excel("Sheet1", path);
        //                                if (dt.Rows.Count > 0)
        //                                {
        //                                    output = addData_YetToStart(dt, strDateKey);
        //                                    if (output == "OK")
        //                                    {
        //                                        // Get Data after compare for New Request Id

        //                                        List<tbl_input_request_data> lst = new List<tbl_input_request_data>();
        //                                        List<YetToStart> lstJson = new List<YetToStart>();
        //                                        YetToStart objJson = new YetToStart();
        //                                        lst = obj.Get_New_Request_Id_List(strDateKey);

        //                                        foreach (var objList in lst)
        //                                        {
        //                                            //lstJson = new List<YetToStart>();
        //                                            //objJson.Request_ID = objList.Request_ID;
        //                                            //objJson.Candidate_ID = objList.Candidate_ID;
        //                                            //objJson.Associate_Id = objList.Associate_Id;
        //                                            //lstJson.Add(objJson);

        //                                            //var json = JsonConvert.SerializeObject(lstJson);
        //                                            string MessageId = Guid.NewGuid().ToString();
        //                                            output = JsonCreater.getDetails_FreshCase(objList.Request_ID, objList.Associate_Id, objList.Candidate_ID, objList.DOJ, MessageId);
        //                                            /////////////////////////// Add Json request into database //////////////
        //                                            int iDML = objDML.Add_Request_Json_Detail(MessageId, "YET2START", output);
        //                                            if (iDML == 1)
        //                                            {
        //                                                bool ret = Write_JSON_TO_RABBIT_MQ(output);
        //                                                output = ret ? "Success" : "Failed";
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    output = "not exist";
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        output = "exception : " + ex.Message.ToString();
        //        ////// Exception Log ///
        //        //DML_Utility objDML = new DML_Utility();
        //        int iException = objDML.Add_Exception_Log(ex.Message, "");

        //    }
        //    return output;
        //}

        public string Read_Json_TagWise(string json, string TagName, string SubTagName)
        {
            string TagValue = "";
            try
            {
                JObject rss = JObject.Parse(json);
                TagValue = (string)rss[TagName][SubTagName];
            }
            catch (Exception ex)
            {
                TagValue = "";
                throw ex;
            }
            return TagValue;
        }
        public string Read_Json_Data_TagWise(string json, string TagName, string SubTagName)
        {
            string TagValue = "";
            try
            {
                JObject rss = JObject.Parse(json);
                JArray ja = JArray.Parse(rss[TagName].ToString());
                JObject rss1 = JObject.Parse(ja[0].ToString());
                TagValue = rss1[SubTagName].ToString();
            }
            catch (Exception ex)
            {
                TagValue = "";
                throw ex;
            }
            return TagValue;
        }
        public string Read_Json_Case_Creation_TagValue(string json, string Tag, string TagValue)
        {
            string retValue = "";
            try
            {
                JObject rss = JObject.Parse(json);
                retValue = (string)rss[Tag][TagValue];
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return retValue;
        }

        public string Read_Json(string json, out string MessageId, out string rssStatus, out string rssModule)
        {
            string FilePath = "";
            try
            {
                JObject rss = JObject.Parse(json);
                MessageId = (string)rss["Header"]["MessageId"];
                rssModule = (string)rss["Header"]["Module"];
                rssStatus = (string)rss["Data"]["Status"];
                string rssFilePath = (string)rss["Data"]["FilePath"];
                string rssFileName = (string)rss["Data"]["FileName"];

                FilePath = rssFilePath;// + rssFileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return FilePath;
        }

        public DataTable ConvertToCSV(string filePath)
        {
            //Will hold all rows of table as CSV
            string contents = File.ReadAllText(filePath);
            contents = contents.Replace("<table>", "");
            contents = contents.Replace("<tr>", "");
            contents = contents.Replace("</table>", "");
            string[] allrow = contents.Split(new string[] { "</tr>" }, StringSplitOptions.None);
            DataTable dt = new DataTable();
            foreach (var row in allrow)
            {
                if (row.Contains("<th>"))
                {
                    string[] allcols = row.Split(new string[] { "</th>" }, StringSplitOptions.None);
                    foreach (var col in allcols)
                    {
                        dt.Columns.Add(col.Replace(Environment.NewLine, "").Replace("<th>", "").Trim());
                    }
                }
                else
                {
                    string[] allcols = row.Split(new string[] { "</td>" }, StringSplitOptions.None);
                    DataRow dr = dt.NewRow();
                    int i = 0;
                    foreach (var col in allcols)
                    {
                        string val = col.Replace(Environment.NewLine, "").Replace("<td>", "").Replace("<text>", "").Replace("</text>", "").Trim();
                        dr[i] = val;
                        i++;
                    }
                    dt.Rows.Add(dr);

                }
            }
            return dt;
        }
        public DataTable Read_Excel(string sheetName, string path)
        {
            DataTable dt = new DataTable();
            try
            {
                using (OleDbConnection conn = new OleDbConnection())
                {
                    string Import_FileName = path;
                    string fileExtension = Path.GetExtension(Import_FileName);
                    if (fileExtension.ToLower() == ".xls")
                        conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;IMEX=1;'";
                    else if (fileExtension == ".xlsx")
                        conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1;'";
                    using (OleDbCommand comm = new OleDbCommand())
                    {
                        comm.CommandText = "Select * from [" + sheetName + "$]";

                        comm.Connection = conn;

                        using (OleDbDataAdapter da = new OleDbDataAdapter())
                        {
                            da.SelectCommand = comm;
                            da.Fill(dt);
                            //return dt;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                dt = null;
                throw ex;
            }
            return dt;
        }

        public DataTable Read_CSV(string path)
        {
            try
            {


                string contents = File.ReadAllText(path + ".csv");

                string[] allrow = contents.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                DataTable dt = new DataTable();
                bool IsHeader = true;
                foreach (var row in allrow)
                {
                    string[] allcols = row.Split(new string[] { "," }, StringSplitOptions.None);
                    DataRow dr = dt.NewRow();
                    int i = 0;
                    foreach (var col in allcols)
                    {
                        if (IsHeader)
                            dt.Columns.Add(col.Trim());
                        else
                        {
                            if (i < dt.Columns.Count)
                                dr[i] = col;
                            i++;
                        }
                    }
                    if (!string.IsNullOrEmpty(dr[0].ToString()))
                        dt.Rows.Add(dr);
                    IsHeader = false;
                }
                return dt;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public DataTable Import(String path)
        {
            DML_Utility objDML = new DML_Utility();

            try
            {


                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.ActiveSheet;

                int index = 0;
                object rowIndex = 2;
                int colindex = 1;

                DataTable dt = new DataTable();
                //dt.Columns.Add("FirstName");
                //dt.Columns.Add("LastName");
                //dt.Columns.Add("Mobile");
                //dt.Columns.Add("Landline");
                //dt.Columns.Add("Email");
                //dt.Columns.Add("ID");

                DataRow row;
                while (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[1, colindex]).Value2 != null)
                {
                    ++colindex;
                }
                for (int i = 1; i < colindex; i++)
                {
                    dt.Columns.Add(Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[1, i]).Value2));
                    //dt.Columns.Add();
                }

                while (((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2 != null)
                {
                    rowIndex = 2 + index;
                    row = dt.NewRow();
                    for (int i = 1; i < colindex; i++)
                    {
                        row[i - 1] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, i]).Value2);
                    }


                    //row[0] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1]).Value2);
                    //row[1] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 2]).Value2);
                    //row[2] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 3]).Value2);
                    //row[3] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 4]).Value2);
                    //row[4] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 5]).Value2);
                    index++;
                    dt.Rows.Add(row);
                }
                app.Workbooks.Close();
                return dt;
            }
            catch (Exception ex)
            {

                objDML.Add_Exception_Log("Wipro exception : " + ex.Message, "");
                throw ex;
            }
        }

        public int addData(DataTable dt)
        {
            int intValue = 0;
            fadv_touchlessEntities entit = new fadv_touchlessEntities();
            tbl_initiation_tracker tbl = new tbl_initiation_tracker();
            List<tbl_initiation_tracker> lst = new List<tbl_initiation_tracker>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    tbl = new tbl_initiation_tracker();
                    tbl.request_id = dr["Request ID"].ToString().Trim();
                    tbl.req_date = dr["Date"].ToString().Trim();
                    tbl.candidate_id = dr["Candidate ID"].ToString().Trim();
                    tbl.associate_id = dr["AssociateId"].ToString().Trim();
                    tbl.bgv_type = dr["BGV Type"].ToString().Trim();
                    tbl.package = dr["Package"].ToString().Trim();
                    tbl.account = dr["Account"].ToString().Trim();
                    tbl.name = dr["Name"].ToString().Trim();
                    tbl.allocated_to = dr["Allocated To"].ToString().Trim();
                    tbl.docs_download = dr["Docs Downloaded"].ToString().Trim();
                    tbl.status = dr["Status"].ToString().Trim();
                    tbl.crn = dr["CRN"].ToString().Trim();
                    tbl.creation_date = dr["Creation Date"].ToString().Trim();
                    tbl.drug_panel = dr["drug panel"].ToString().Trim();
                    tbl.req_date1 = dr["Date1"].ToString().Trim();
                    lst.Add(tbl);
                }
                entit.tbl_initiation_tracker.AddRange(lst);
                entit.SaveChanges();
                intValue = 1;
            }
            catch (Exception ex)
            {
                intValue = -1;
                throw ex;
            }
            return intValue;
        }

        public string addData_YetToStart(DataTable dt, string strDateKey)
        {
            string Msg = "";
            DML_Utility objDML = new DML_Utility();

            fadv_touchlessEntities entit = new fadv_touchlessEntities();
            tbl_input_request_data tbl = new tbl_input_request_data();
            List<tbl_input_request_data> lst = new List<tbl_input_request_data>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    tbl = new tbl_input_request_data();
                    tbl.Account = dr["Account"].ToString().Trim();
                    tbl.Account_Group = dr["Account Group"].ToString().Trim();
                    tbl.Additional_Payment_Status = dr["Additional Payment Status"].ToString().Trim();
                    tbl.Associate_Id = dr["AssociateId"].ToString().Trim();
                    tbl.BGV_Type = dr["BGV Type"].ToString().Trim();
                    tbl.BU = dr["BU"].ToString().Trim();
                    tbl.Candidate_ID = dr["CandidateId"].ToString().Trim();
                    tbl.CE_Available = dr["CE Available(Yes/No)"].ToString().Trim();
                    tbl.CE_BGV_Initiated_Date = dr["CE-BGV Initiated date"].ToString().Trim();
                    tbl.Components = dr["Components"].ToString().Trim();
                    tbl.Department = dr["Department"].ToString().Trim();
                    tbl.DOJ = dr["DOJ"].ToString().Trim();
                    tbl.HR_POC = dr["HR POC"].ToString().Trim();
                    tbl.Last_Updated_On = dr["Last Updated On"].ToString().Trim();
                    tbl.Name = dr["Name"].ToString().Trim();
                    tbl.Pre_BGV_Initiated_Date = dr["Pre-BGV Initiated Date"].ToString().Trim();
                    tbl.Project = dr["Project"].ToString().Trim();
                    tbl.Report_uploaded_date = dr["Report uploaded date"].ToString().Trim();
                    //tbl.Request_Date = dr["Date1"].ToString().Trim();
                    tbl.Request_ID = dr["RequestId"].ToString().Trim();
                    tbl.Vendor_Status = dr["Vendor Status"].ToString().Trim();
                    tbl.Work_Location = dr["Work Location"].ToString().Trim();
                    tbl.ImportKey = Convert.ToInt64(strDateKey.ToString());

                    lst.Add(tbl);
                }
                entit.tbl_input_request_data.AddRange(lst);
                entit.SaveChanges();
                Msg = "OK";
            }
            catch (Exception ex)
            {
                Msg = "ex";
                objDML.Add_Exception_Log("Wipro exception : " + ex.Message, "");
                throw ex;
            }
            return Msg;
        }


        #region Add JSON into RABBITMQ
        public bool Write_JSON_TO_RABBIT_MQ(string json)
        {
            bool ret = false;
            string error = "";
            try
            {
                RabbitMQ_Utility objQueue = new RabbitMQ_Utility();
                //ret = objQueue.Rabbit_Send(json, "Request", "localhost", out error);
                ret = objQueue.Rabbit_Send(json, RabbitMQ_Utility.RabbitMQRequestQueue, RabbitMQ_Utility.RabbitMQUrl, out error);
            }
            catch (Exception ex)
            {
                error = "ex";
                ////// Exception Log ///
                throw ex;
            }
            return ret;
        }

        public bool Write_JSON_TO_ServerRABBIT_MQ(string json)
        {
            bool ret = false;
            string error = "";
            try
            {
                RabbitMQ_Utility objQueue = new RabbitMQ_Utility();
                //ret = objQueue.Rabbit_Send(json, "Request", "localhost", out error);
                ret = objQueue.ServerRabbit_Send(json, RabbitMQ_Utility.ServerQueue, RabbitMQ_Utility.RabbitMQUrl, out error);
            }
            catch (Exception ex)
            {
                error = "ex";
                ////// Exception Log ///
                throw ex;
            }
            return ret;
        }


        public string Write_JSON_TO_Download()
        {
            bool ret = false;
            string output = "";
            string error = "";
            DML_Utility objDML = new DML_Utility();
            Get_Data_Utility objGet = new Get_Data_Utility();
            try
            {
                //objDML.Add_Exception_Log(ts.TotalMinutes.ToString(), "");
                // Check Last Request //
                int Flag = 1;
                List<tbl_request_details> lstObj = new List<tbl_request_details>();
                lstObj = objGet.Get_Last_Request(FilePath_Container.ServiceId_WiproDownload);

                if (lstObj.Count > 0)
                {
                    double timeMinutes = 0;
                    DateTime dtReq = (DateTime)lstObj[0].createdOn;
                    //DateTime dt = System.DateTime.Now;
                    DateTime dt = System.DateTime.Now.AddMinutes(0);
                    TimeSpan ts = dt - dtReq;
                    List<tbl_config_value> lstConfig = objGet.Get_Cofig_Details("CapgeminiDOWNLOADREQUEST");
                    foreach (var ob in lstConfig)
                    {
                        double.TryParse(ob.configstring, out timeMinutes);
                    }
                    timeMinutes = timeMinutes <= 0 ? 59 : timeMinutes;
                    //int iException1 = objDML.Add_Exception_Log(ts.TotalMinutes.ToString(), dt.ToString());
                    if (ts.TotalMinutes > timeMinutes)
                    {
                        //objDML.Add_Exception_Log("Wipro log : " + (ts.TotalMinutes).ToString(), "Download request sent");
                        Flag = 1;
                    }
                    else
                    {
                        Flag = 0;
                    }
                }

                if (Flag > 0)
                {
                    string MessageId = Guid.NewGuid().ToString();
                    string ServiceId = FilePath_Container.ServiceId_WiproDownload.ToString();// "DOWNLOAD";// Guid.NewGuid().ToString();
                                                                                             ///////////////////////// Add Json request into database //////////////
                    JsonCreater JsonCreater = new Read_File_Processor.JsonCreater();
                    string json = JsonCreater.getDownload(MessageId, ServiceId);
                    int iDML = objDML.Add_Request_Json_Detail(MessageId, ServiceId, json);  // Add_Request_Json(MessageId, "DOWNLOAD", json);
                                                                                            ///////////////////////// Add Json request into Rabbit MQ //////////////
                    if (iDML == 1)
                    {
                        ret = Write_JSON_TO_RABBIT_MQ(json);
                        output = ret ? "Success" : "Failed";
                    }
                }
                else
                {
                    // READ RESPONSE QUEUE //
                    Read_Response();
                }
            }
            catch (Exception ex)
            {

                output = "CapGemini : " + ex.Message.ToString();
                ////// Exception Log ///
                //DML_Utility objDML = new DML_Utility();
                //int iException = objDML.Add_Exception_Log(ex.Message, "Write_JSON_TO_Download");
            }
            return output;
        }

        #endregion
    }
}

