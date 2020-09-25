using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Business_Entities;
using DataAccess_Utility;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace Read_File_Processor
{
    public class CaseCreationProcessor
    {
        private object objDML;

        public string Execute_Case_Creation()
        {
            string output = "";
            DML_Utility objDML = new DML_Utility();
            try
            {

                // Read RABITMQ for Response //
                string Response_Data = "";
                string Response_Error = "";
                RabbitMQ_Utility objQueue = new RabbitMQ_Utility();
                //bool retResponse = objQueue.Receive("Response", out Response_Data, out Response_Error);
                bool retResponse = objQueue.Receive(RabbitMQ_Utility.RabbitMQResponseQueue, out Response_Data, out Response_Error);

                if (!string.IsNullOrEmpty(Response_Data))
                {
                    List<tbl_yettostart_casecreation_data> lst = new List<tbl_yettostart_casecreation_data>();
                    tbl_yettostart_casecreation_data tbl = new tbl_yettostart_casecreation_data();
                    string responce_MessageId = "";
                    string responce_ServiceId = "";
                    string path = Read_Json_Case_Creation(Response_Data, out responce_MessageId, out responce_ServiceId);
                    if (responce_ServiceId.ToLower() == "freshcase")
                    {
                        string College_CVT = "";
                        string College_UT = "";
                        string Degree_CVT = "";
                        string Degree_UT = "";

                        // get RequestId column from table //
                        Get_Data_Utility obj = new Get_Data_Utility();
                        long reqId = obj.Get_Request_Id_new(responce_MessageId, "YET2START");
                        if (reqId > 0)
                        {
                            tbl.queue_request_id = reqId;
                            tbl.cognizent_tech_solution = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "cognizent_tech_solution");
                            tbl.clientcode = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "clientcode");
                            tbl.candidate_name = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "candidate_name");

                            string candidateID = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "candidate_id");
                            string associate_id = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "associate_id");
                            tbl.client_ref_no = ((candidateID != "0") ? candidateID : associate_id);// Read_Json_Case_Creation_TagValue(Response_Data, "Data", "client_ref_no");
                            tbl.bvg_type = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "bvg_type");
                            tbl.package = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "package");
                            tbl.specification = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "specification");
                            tbl.project_id = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "project_id");
                            tbl.project_name = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "project_name");
                            tbl.doj = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "doj");
                            tbl.request_id = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "request_id");
                            tbl.associate_id = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "associate_id");
                            tbl.candidate_id = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "candidate_id");
                            tbl.employee_id = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "employee_id");
                            tbl.account_name = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "account_name");
                            tbl.tensse = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "tensse");
                            tbl.actual_case_created = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "actual_case_created");
                            tbl.first_name = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "First_Name");
                            tbl.last_name = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Last_Name");
                            tbl.date_of_birth = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Date_Of_Birth");
                            tbl.father_name = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Father_Name");
                            tbl.nationality = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Nationality");
                            tbl.mobile_number = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Mobile_Number");
                            tbl.current_address = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Current_Address");
                            tbl.permanent_address = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Permanent_Address");
                            tbl.longest_stay_address = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Longest_Stay_Address");
                            tbl.LOA_Status = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "LOA_Status");

                            College_CVT = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "College_CVT_UG");
                            College_UT = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "College_UT_UG");
                            Degree_CVT = "";// Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Degree_Type_CVT");
                            Degree_UT = "";// Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Degree_Type_UT");
                            tbl.college_cvt = College_CVT;
                            tbl.college_ut = College_UT;
                            tbl.degree_type_cvt = Degree_CVT;
                            tbl.degree_type_ut = Degree_UT;

                            tbl.reference_type_cvt = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Reference_Type_CVT");
                            tbl.reference_type_ut = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Reference_Type_UT");
                            tbl.company_name_cvt = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Company_Name_CVT_Previous");
                            tbl.company_name_ut = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Company_Name_UT_Previous");
                            tbl.id_cvt = "";// Read_Json_Case_Creation_TagValue(Response_Data, "Data", "ID_CVT");
                            tbl.id_ut = "";//Read_Json_Case_Creation_TagValue(Response_Data, "Data", "ID_UT");
                            tbl.employment_type_cvt = "";// Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Employment_Type_CVT");
                            tbl.employment_type_ut = "";// Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Employment_Type_UT");

                            string College_UT_PG = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "College_UT_PG");
                            string College_CVT_PG = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "College_CVT_PG");
                            string BU = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "BU");
                            string Company_Name_UT_Current = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Company_Name_UT_Current");
                            string Company_Name_CVT_Current = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Company_Name_CVT_Current");
                            string FilePath = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "FilePath");
                            string LOA_Present = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "LOA_Present");


                            lst.Add(tbl);
                            int intValue = objDML.Add_Response_Json(reqId, lst, Response_Data);
                            if (intValue > 0)
                            {
                                // Update college detail //
                                List<Yet2Start_College> lstCollege = new List<Yet2Start_College>();
                                Yet2Start_College objCollege = new Yet2Start_College();

                                string[] strCollegeCVT = College_CVT.Split(new char[] { '+' });
                                string[] strCollegeUT = College_UT.Split(new char[] { '+' });
                                string[] strDegreeCVT = Degree_CVT.Split(new char[] { '+' });
                                string[] strDegreeUT = Degree_UT.Split(new char[] { '+' });

                                for (int i = 0; i < strCollegeCVT.Length; i++)
                                {
                                    objCollege = new Yet2Start_College();
                                    objCollege.college = strCollegeCVT[i].ToString().Trim();
                                    objCollege.degree = strDegreeCVT[i].ToString().Trim();
                                    objCollege.field_source = "CVT";
                                    lstCollege.Add(objCollege);
                                }
                                for (int i = 0; i < strCollegeCVT.Length; i++)
                                {
                                    objCollege = new Yet2Start_College();
                                    objCollege.college = strCollegeUT[i].ToString().Trim();
                                    objCollege.degree = strDegreeUT[i].ToString().Trim();
                                    objCollege.field_source = "UT";
                                    lstCollege.Add(objCollege);
                                }

                                // Add this into database Table //
                                long resId = obj.Get_FreshCase_Response_Id(reqId.ToString());
                                if (resId > 0)
                                {

                                    intValue = objDML.Add_Response_Json(reqId, resId, lstCollege);
                                    if (intValue > 0)
                                    {
                                        //////////////////////////////// CASE CREATION JSON FINAL ////////////////////
                                        //Create_Case_Creation_Json(reqId, resId, "", "", "", "", "");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                output = "ex";
                int iException = objDML.Add_Exception_Log("Wipro exception : " + ex.Message, "");
            }
            return output;
        }

        public string Create_Case_Creation_Json_For_FreshCase()
        {
            string output = "";
            long ResId = 0;
            long ReqId = 0;
            DML_Utility objDML = new DML_Utility();
            Get_Data_Utility obj = new Get_Data_Utility();
            try
            {

                List<tbl_response_detail> lstResponse = obj.Get_Response_Data_ToBe_Process(FilePath_Container.FreshCase);
                if (lstResponse.Count > 0)
                {
                    string MessageId = "";
                    string ServiceId = "";

                    string ResJson = "";
                    foreach (var res in lstResponse)
                    {
                        MessageId = res.message_id;
                        ServiceId = res.service_id;
                        ResId = res.id;
                        ReqId = (long)res.request_id;
                        ResJson = res.response_json;
                    }
                    string result = Read_Json_DataForCaseCreation(ResJson, "metadata", "status", "success");
                    string requestId = Read_Json_TagWise(ResJson, "metadata", "requestId");
                    fadv_touchlessEntities entit = new fadv_touchlessEntities();
                    if (result.ToLower() == "true")
                    {
                        JObject resjsonobj = JObject.Parse(ResJson);
                        string candidateID = resjsonobj["data"][0]["taskSpecs"]["candidateId"].ToString().Trim();
                        string BotId = Read_Json_TagWise(ResJson, "metadata", "botId");
                        objDML.UpdateCapgeminiRecord(candidateID,BotId);
                    }
                    else
                    {
                        using (fadv_touchlessEntities entities = new fadv_touchlessEntities())
                        {
                            tbl_request_details processData = entities.tbl_request_details.Where(x => x.json_text.Contains(requestId)).First();
                            string json = processData.json_text;
                            string attempt = Read_Json_TagWise(json, "metadata", "attempt");
                            string BotId = Read_Json_TagWise(ResJson, "metadata", "botId");
                            if (attempt == "1")
                            {
                                JObject rss = JObject.Parse(json);
                                string queueMessageId = Guid.NewGuid().ToString();
                                rss["metadata"]["attempt"] = "2";
                                rss["metadata"]["requestId"] = queueMessageId;
                                string updatedJson = rss.ToString();
                                string queueServiceId = FilePath_Container.FreshCase;
                                output = updatedJson;
                                int iDML = objDML.Add_Request_Json_Detail(queueMessageId, queueServiceId, output);
                                if (iDML == 1)
                                {
                                    string resumeno = (string)rss["data"][0]["taskSpecs"]["candidateId"];
                                    RabbitMQ_Utility objQueue = new RabbitMQ_Utility();
                                    objDML.UpdateCapgeminiRecord(resumeno, BotId);
                                    string error;
                                    objDML.Add_Exception_Log("Capgemini: 2nd attempt for Candidate Id : : " + resumeno + " has been sent to rabbitMQ ", resumeno);
                                    objQueue.Rabbit_Send(updatedJson, RabbitMQ_Utility.RabbitMQRequestQueue, RabbitMQ_Utility.RabbitMQUrl, out error);
                                }
                            }
                        }
                    }
                    objDML.Update_Response_Status(ResId);

                }
            }
            catch (Exception ex)
            {
                output = "ex";
                objDML.Update_Response_Status(ResId);
                //throw ex;
                int iException = objDML.Add_Exception_Log("Wipro exception : " + ex.Message, "Create_Case_Creation_Json_For_FreshCase");
            }

            return output;
        }

        public Dictionary<string, string> getPackageName(string Account, string vbgType, string bu, string vbgSubType, string outputParametr, string labelName, string CustomerRequestID, string accountGroup, string dataScrappedValue)
        {
            DML_Utility objDML = new DML_Utility();
            try
            {
                //Find PackageName
                string engineID = ConfigurationManager.AppSettings["engineID"];
                string engineLicenseId = ConfigurationManager.AppSettings["engineLicenseId"];

                PackageRequestModel PkgObj = new PackageRequestModel();
                metadata newobj = new metadata();
                newobj.engineName = "Package Identification Engine";
                newobj.engineID = engineID;//"999286bf-c25b-4120-bbfa-5d34dda6994a";
                newobj.engineLicenseId = engineLicenseId;// "8565b92f-fba5-48d5-94ca-1e864ef98787";
                newobj.engineType = "1";
                newobj.requestId = CustomerRequestID;// Guid.NewGuid().ToString();
                newobj.requestDate = DateTime.Now.ToString();
                newobj.sourceApp = "Touchless";
                newobj.sourceAppModule = "Package Manager";
                newobj.requestLabel = labelName;
                PkgObj.metadata = newobj;
                List<Data> lstdata = new List<Data>();
                Data data = new Data();
                data.account = Account;
                data.accountgroup = accountGroup;
                data.bgvtype = vbgType;
                data.datascrappedvalue = dataScrappedValue;
                //data.bgvsubtype = vbgSubType;
                data.bu = bu;
                lstdata.Add(data);
                PkgObj.data = lstdata;

                APIManeger manager = new APIManeger();

                string response = manager.PostSuspect(PkgObj);
                //objDML.Add_Exception_Log(response, "");
                string[] para = outputParametr.Split(',');
                Dictionary<string, string> returnvalues = new Dictionary<string, string>();
                foreach (var item in para)
                {
                    string paravalue = Read_Json_PackageName(response, "response", "data", item);
                    returnvalues.Add(item, paravalue);
                }
                return returnvalues;
            }
            catch (Exception ex)
            {
                int iException = objDML.Add_Exception_Log("Wipro exception : " + ex.InnerException.Message, "");

                throw;
            }

        }
        public Dictionary<string, string> getWiproPackageName(string Check_Initiated, string CustomerRequestID, string labelName, string outputParametr)
        {
            DML_Utility objDML = new DML_Utility();
            try
            {
                //Find PackageName
                string engineID = ConfigurationManager.AppSettings["engineID"];
                string engineLicenseId = ConfigurationManager.AppSettings["engineLicenseId"];

                WiproPackageRequestModel PkgObj = new WiproPackageRequestModel();
                metadata newobj = new metadata();
                newobj.engineName = "Package Identification Engine";
                newobj.engineID = engineID;//"999286bf-c25b-4120-bbfa-5d34dda6994a";
                newobj.engineLicenseId = engineLicenseId;// "8565b92f-fba5-48d5-94ca-1e864ef98787";
                newobj.engineType = "1";
                newobj.requestId = CustomerRequestID;// Guid.NewGuid().ToString();
                newobj.requestDate = DateTime.Now.ToString();
                newobj.sourceApp = "Touchless";
                newobj.sourceAppModule = "Package Manager";
                newobj.requestLabel = labelName;
                PkgObj.metadata = newobj;
                List<WiproData> lstdata = new List<WiproData>();
                WiproData data = new WiproData();
                data.checkinitiated = Check_Initiated;
                lstdata.Add(data);
                PkgObj.data = lstdata;

                APIManeger manager = new APIManeger();

                string response = manager.PostWiproPackageRequest(PkgObj);
                //objDML.Add_Exception_Log(response, "");
                string[] para = outputParametr.Split(',');
                Dictionary<string, string> returnvalues = new Dictionary<string, string>();
                foreach (var item in para)
                {
                    string paravalue = Read_Json_PackageName(response, "response", "data", item);
                    returnvalues.Add(item, paravalue);
                }
                return returnvalues;
            }
            catch (Exception ex)
            {
                int iException = objDML.Add_Exception_Log("Wipro exception : " + ex.InnerException.Message, "");

                throw;
            }

        }

        public string Create_Case_Creation_JsonOld(long reqId, long resId)
        {
            string output = "";
            DML_Utility objDML = new DML_Utility();
            try
            {
                string strRequest_Id = "";
                string strCandidate_Id = "";
                string strAssociate_Id = "";
                // Get Data
                ExecuteProcess objExe = new ExecuteProcess();
                fadv_touchlessEntities entit = new fadv_touchlessEntities();
                //tbl_yettostart_casecreation_data tbl = new tbl_yettostart_casecreation_data();
                List<tbl_yettostart_casecreation_data> lstFreshCase = entit.tbl_yettostart_casecreation_data.Where(x => x.responseId == resId && x.queue_request_id == reqId).ToList<tbl_yettostart_casecreation_data>();
                List<tbl_college_details> lstCollege = entit.tbl_college_details.Where(x => x.active == 1 && x.resid == resId && x.reqid == reqId).ToList<tbl_college_details>();
                foreach (var obj in lstFreshCase)
                {
                    strRequest_Id = obj.request_id;
                    strAssociate_Id = obj.associate_id;
                    strCandidate_Id = obj.candidate_id;
                }
                List<tbl_input_request_data> lstYet2StartData = entit.tbl_input_request_data.Where(x => x.Request_ID == strRequest_Id && x.Associate_Id == strAssociate_Id && x.Candidate_ID == strCandidate_Id).ToList<tbl_input_request_data>();
                List<tbl_initiation_tracker> lstMISLog = entit.tbl_initiation_tracker.Where(x => x.request_id == strRequest_Id && x.associate_id == strAssociate_Id && x.candidate_id == strCandidate_Id).ToList<tbl_initiation_tracker>();

                JsonCreater objFinal = new JsonCreater();
                string MessageId = Guid.NewGuid().ToString();
                string ServiceId = FilePath_Container.CaseCreation;
                //string jsonFinal = objFinal.Final_Create_Case_Json(MessageId, lstFreshCase, lstCollege, lstYet2StartData, lstMISLog, ServiceId, "", "", "");

                /////////////////////////// Add Json request into database //////////////

                //int iDML = objDML.Add_Request_Json_Detail(MessageId, "CaseCreation", jsonFinal);
                //if (iDML == 1)
                //{
                //    bool ret = objExe.Write_JSON_TO_RABBIT_MQ(jsonFinal);
                //    output = ret ? "Success" : "Failed";
                //}

                //bool ret = objExe.Write_JSON_TO_RABBIT_MQ(jsonFinal); 
                //output = ret ? "Success" : "Failed";
            }
            catch (Exception ex)
            {
                output = "ex";
                //throw ex;
                int iException = objDML.Add_Exception_Log("Wipro exception : " + ex.Message, "");
            }

            return output;
        }

        public string Create_Case_Creation_Json(long reqId, long resId, tbl_Wipro_Details CandidateDetails, string package, string Database_Sent, string PONumber, string fileupload, string clientID, string SBUID, string clientName, string sbuName, string PackageName)
        {
            string output = "";
            DML_Utility objDML = new DML_Utility();
            Get_Data_Utility objGet = new Get_Data_Utility();
            try
            {
                string strRequest_Id = "";
                string strCandidate_Id = "";
                string strAssociate_Id = "";
                // List<tbl_requests> RequestID = objGet.Get_RequestID();
                //objDML.Add_Exception_Log("Before", "");

                //decimal NewRequestID = RequestID[0].RequestID + 1;
                //objDML.Add_Exception_Log("After", "");

                // Get Data

                JsonCreater objFinal = new JsonCreater();
                string MessageId = Guid.NewGuid().ToString();
                string ServiceId = "CaseCreation";// FilePath_Container.CaseCreation;
                //fileupload = "";
                string jsonFinal = objFinal.Final_Create_Case_Json(MessageId, CandidateDetails, package, Database_Sent, PONumber, fileupload, clientID, SBUID, clientName, ServiceId, sbuName: sbuName);
                string jsonFinalForRequests = objFinal.Final_Create_Case_JsonForRequests(MessageId, CandidateDetails, package, Database_Sent, PONumber, fileupload, clientID, SBUID, clientName, ServiceId, sbuName: sbuName);
                objDML.Add_Exception_Log("Wipro log : " + jsonFinal, "Json_Data");
                string JsonDatRequests = Read_Json_DataForCaseCreation(jsonFinalForRequests, "Data", "CaseCreation", "clientSpecificFields");
                objDML.Add_Exception_Log("Wipro log : " + JsonDatRequests, "Json_Data");
                /////////////////////////// Add Json request into database //////////////
                //objDML.Add_Exception_Log(lstFreshCase.Count().ToString(), Convert.ToString(NewRequestID));
                string NewRequestID = "0";
                int ADDRequests = objDML.Insert_data_in_requests(ref NewRequestID, CandidateDetails, JsonDatRequests, clientID, SBUID);
                jsonFinal = objFinal.Final_Create_Case_Json(MessageId, CandidateDetails, package, Database_Sent, PONumber, fileupload, clientID, SBUID, clientName, ServiceId, RequestID: NewRequestID, sbuName: sbuName);
                objDML.Insert_Json_in_requestracker(Convert.ToString(NewRequestID), jsonFinal);
                objDML.Insert_Json_in_requesStateInstanse(Convert.ToInt64(NewRequestID), 1, "REQ-0002", 165, "Case Created by Touchless", 1, 5, 0);



                //Uploading The downloaded files
                string destinationPath = ConfigurationManager.AppSettings["destinationPath"] + "\\" + NewRequestID;
                FileUtility fileUtility = new FileUtility();
                List<string> copiedFiles = fileUtility.FileUpload(fileupload, destinationPath);
                objDML.Add_Exception_Log("Wipro log : " + copiedFiles.Count + "Files Uploaded", "");
                objDML.Insert_FilePathIndocument_upload(copiedFiles, Convert.ToInt64(NewRequestID));
                objDML.Add_Exception_Log("Wipro log : " + "Files Path Inserted in database", NewRequestID.ToString());


                objDML.Add_Exception_Log("Wipro log : " + "RequestInstance Created", NewRequestID.ToString());
                objDML.updateWiproDetails(CandidateDetails.Candidte_Id, CandidateDetails.Check_Initiated, Convert.ToInt64(NewRequestID));

                string packageID = objGet.getPackageID(PackageName, SBUID);
                objDML.InsertPackageDetails(Convert.ToInt64(NewRequestID), packageID);



                int iDML = objDML.Add_Request_Json_Detail(MessageId, "CaseCreation", jsonFinal);
                ExecuteProcess objExe = new ExecuteProcess();
                if (iDML == 1)
                {
                    bool ret = objExe.Write_JSON_TO_ServerRABBIT_MQ(jsonFinal);
                    output = ret ? "Success" : "Failed";
                    objDML.Add_Exception_Log("Wipro log : " + output, "");
                }

                //bool ret = objExe.Write_JSON_TO_RABBIT_MQ(jsonFinal); 
                //output = ret ? "Success" : "Failed";
            }
            catch (Exception ex)
            {
                output = "ex";
                //throw ex;
                while (ex != null)
                {
                    int iException = objDML.Add_Exception_Log("Wipro exception : " + ex.Message, "Create_Case_Creation_Json");
                    ex = ex.InnerException;
                }
            }

            return output;
        }
        public string Read_Json_DataForCaseCreation(string json, string Tag, string TagValue, string key)
        {
            string retValue = "";
            try
            {
                JObject rss = JObject.Parse(json);
                retValue = rss[Tag][TagValue][key].ToString();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return retValue;
        }
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

        public string Read_Json_Case_Creation(string json, out string MessageId, out string rssServiceID)
        {
            string FilePath = "";
            try
            {
                JObject rss = JObject.Parse(json);
                MessageId = (string)rss["Header"]["MessageID"];
                rssServiceID = (string)rss["Header"]["ServiceId"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return FilePath;
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

        public string Read_Json_PackageName(string json, string Tag, string TagValue, string key)
        {
            string retValue = "";
            try
            {
                JObject rss = JObject.Parse(json);
                retValue = (string)rss[Tag][TagValue][0][key];
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return retValue;
        }

    }
}
