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
                            tbl.client_ref_no = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "client_ref_no");
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
                                        Create_Case_Creation_Json(reqId, resId, "", "", "", "");
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
                int iException = objDML.Add_Exception_Log(ex.Message, "");
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
                    //objDML.Add_Exception_Log(lstResponse.Count.ToString(), "");

                    string MessageId = "";
                    //MessageId.Split('a').Distinct;
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
                    // objDML.Add_Exception_Log("Before API Call", "");

                    string responce_MessageId = "";
                    string responce_ServiceId = "";
                    string Response_Data = "";
                    string path = Read_Json_Case_Creation(ResJson, out responce_MessageId, out responce_ServiceId);
                    //objDML.Add_Exception_Log(responce_MessageId, responce_ServiceId);

                    string status = Read_Json_Case_Creation_TagValue(ResJson, "Status", "Value");
                    //objDML.Add_Exception_Log(responce_MessageId, responce_ServiceId);

                    //objDML.Add_Exception_Log(status, "");
                    //objDML.Add_Exception_Log(responce_ServiceId, "");
                    fadv_touchlessEntities entit = new fadv_touchlessEntities();


                    if (status == "0000")
                    {
                        if (responce_ServiceId.ToLower() == "freshcase")
                        {
                            //objDML.Add_Exception_Log("Before API Call", ResJson);

                            string reqID = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "request_id");
                            List<tbl_input_request_data> InputData = entit.tbl_input_request_data.Where(x => x.Request_ID == reqID).ToList<tbl_input_request_data>();
                            //List<tbl_input_request_data> InputData = entit.tbl_input_request_data.Where(x => x.Request_ID == reqID).ToList<tbl_input_request_data>();



                            List<tbl_yettostart_casecreation_data> lst = new List<tbl_yettostart_casecreation_data>();
                            tbl_yettostart_casecreation_data tbl = new tbl_yettostart_casecreation_data();
                            string College_CVT = "";
                            string College_UT = "";
                            string Degree_CVT = "";
                            string Degree_UT = "";
                            Response_Data = ResJson;

                            //objDML.Add_Exception_Log("Before API Call", InputData.Count.ToString());

                            string Account = (Read_Json_Case_Creation_TagValue(Response_Data, "Data", "account_name") == "") ? InputData[0].Account : Read_Json_Case_Creation_TagValue(Response_Data, "Data", "account_name");
                            string AccountGroup = InputData[0].Account_Group;
                            string vbgType = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "bvg_type");
                            string bvgsubtype = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "bvg_sub_type");
                            string bu = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "BU");
                            string labelName = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "candidate_name");
                            string sbu = "";
                            string outputParametrs = ConfigurationManager.AppSettings["outputParametrs"];
                            Dictionary<string, string> paravalues = getPackageName(Account, vbgType, bu, bvgsubtype, outputParametrs, labelName);
                            string PackageName = (paravalues.Keys.Contains("package")) ? paravalues["package"] : "";
                            sbu = (paravalues.Keys.Contains("sbu")) ? paravalues["sbu"] : "";
                            objDML.Add_Exception_Log(PackageName, "");

                            tbl.queue_request_id = ReqId;
                            tbl.responseId = ResId;
                            tbl.cognizent_tech_solution = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "cognizent_tech_solution");
                            tbl.clientcode = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "clientcode");
                            tbl.candidate_name = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "candidate_name");
                            tbl.client_ref_no = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "client_ref_no");
                            tbl.bvg_type = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "bvg_type");
                            tbl.package = PackageName;// Read_Json_Case_Creation_TagValue(Response_Data, "Data", "package");
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

                            tbl.reference_type_cvt = "";// Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Reference_Type_CVT");
                            tbl.reference_type_ut = "";//Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Reference_Type_UT");
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
                            string FilePath = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "FilePath").Replace(@"\", @"\\");
                            string LOA_Present = Read_Json_Case_Creation_TagValue(Response_Data, "Data", "LOA_Present");
                            string clientID = (Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Client_ID") == "") ? "1400000003" : Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Client_ID");
                            string clientName = (Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Client_Name") == "") ? "Cognizant Technology Solutions (IT)" : Read_Json_Case_Creation_TagValue(Response_Data, "Data", "Client_Name");

                            string SBUIID = (Read_Json_Case_Creation_TagValue(Response_Data, "Data", "SBU_ID") == "") ? "Cognizant Technology Solutions (IT)" : Read_Json_Case_Creation_TagValue(Response_Data, "Data", "SBU_ID"); ;
                            tbl.active = 1;
                            lst.Add(tbl);

                            int intValue = objDML.Add_Response_Json_FreshCase_Update(ReqId, ResId, lst, Response_Data);
                            //objDML.Add_Exception_Log("After Yet", "");
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
                                //objDML.Add_Exception_Log("After strCollegeCVT", "");

                                for (int i = 0; i < strCollegeUT.Length; i++)
                                {
                                    objCollege = new Yet2Start_College();
                                    objCollege.college = strCollegeUT[i].ToString().Trim();
                                    objCollege.degree = strDegreeUT[i].ToString().Trim();
                                    objCollege.field_source = "UT";
                                    lstCollege.Add(objCollege);
                                }
                                //objDML.Add_Exception_Log("After strCollegeUT", "");

                                // Add this into database Table //
                                intValue = objDML.Add_Response_Json(ReqId, ResId, lstCollege);
                                if (intValue > 0)
                                {
                                    //////////////////////////////// CASE CREATION JSON FINAL ////////////////////
                                    output = Create_Case_Creation_Json(ReqId, ResId, FilePath, clientID, SBUIID, clientName);
                                    if (output.ToLower() == "success")
                                    {
                                        // Update Status in Response Table //
                                        objDML.Update_Response_Status(ResId);
                                    }
                                }
                                //objDML.Add_Exception_Log("After success", "");

                            }
                        }
                    }
                    else if (status == "0001")
                    {
                        using (fadv_touchlessEntities entities = new fadv_touchlessEntities())
                        {
                            string ReqMsg = Read_Json_Case_Creation_TagValue(ResJson, "Data", "Request_Id");
                            //objDML.Add_Exception_Log(ReqMsg, "");

                            tbl_initiation_tracker processData = entities.tbl_initiation_tracker.Where(x => x.request_id == ReqMsg.Trim()).First();
                            processData.active = 0;
                            entities.SaveChanges();
                            //objDML.Add_Exception_Log("After FailedJson", "");

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
                int iException = objDML.Add_Exception_Log(ex.Message, "Create_Case_Creation_Json_For_FreshCase");
            }

            return output;
        }

        public Dictionary<string, string> getPackageName(string Account, string vbgType, string bu, string vbgSubType, string outputParametr, string labelName)
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
                newobj.requestId = Guid.NewGuid().ToString();
                newobj.requestDate = DateTime.Now.ToString();
                newobj.sourceApp = "Touchless";
                newobj.sourceAppModule = "Package Manager";
                newobj.requestLabel = labelName;
                PkgObj.metadata = newobj;
                List<Data> lstdata = new List<Data>();
                Data data = new Data();
                data.account = Account;
                data.bgvtype = vbgType;
                //data.bgvsubtype = vbgSubType;
                data.bu = bu;
                lstdata.Add(data);
                PkgObj.data = lstdata;

                APIManeger manager = new APIManeger();
                string response = manager.PostSuspect(PkgObj).Result;
                objDML.Add_Exception_Log(response, "");
                string[] para = outputParametr.Split(',');
                Dictionary<string, string> returnvalues = new Dictionary<string, string>();
                foreach (var item in para)
                {
                    string paravalue = Read_Json_PackageName(response, "response", "data", item);
                    if (item == "package")
                    {
                        if (paravalue == "" || paravalue == "Not Available")
                            paravalue = "Lateral - Amex - BPO 20-Sep-19 CRTC212RPA";
                    }
                    returnvalues.Add(item, paravalue);
                }
                //string package = Read_Json_PackageName(response, "response", "data", "package");
                ////////////////////////////////////////////////////////
                //if (package == "" || package == "Not Available")
                //    package = "Lateral - Amex - BPO 20-Sep-19 CRTC212RPA";
                return returnvalues;
            }
            catch (Exception ex)
            {
                int iException = objDML.Add_Exception_Log(ex.InnerException.Message, "");

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
                string jsonFinal = objFinal.Final_Create_Case_Json(MessageId, lstFreshCase, lstCollege, lstYet2StartData, lstMISLog, ServiceId, "", "", "");

                /////////////////////////// Add Json request into database //////////////

                int iDML = objDML.Add_Request_Json_Detail(MessageId, "CaseCreation", jsonFinal);
                if (iDML == 1)
                {
                    bool ret = objExe.Write_JSON_TO_RABBIT_MQ(jsonFinal);
                    output = ret ? "Success" : "Failed";
                }

                //bool ret = objExe.Write_JSON_TO_RABBIT_MQ(jsonFinal); 
                //output = ret ? "Success" : "Failed";
            }
            catch (Exception ex)
            {
                output = "ex";
                //throw ex;
                int iException = objDML.Add_Exception_Log(ex.Message, "");
            }

            return output;
        }

        public string Create_Case_Creation_Json(long reqId, long resId, string fileupload, string clientID, string SBUID, string clientName)
        {
            string output = "";
            DML_Utility objDML = new DML_Utility();
            Get_Data_Utility objGet = new Get_Data_Utility();
            try
            {
                string strRequest_Id = "";
                string strCandidate_Id = "";
                string strAssociate_Id = "";
                List<tbl_requests> RequestID = objGet.Get_RequestID();
                //objDML.Add_Exception_Log("Before", "");

                decimal NewRequestID = RequestID[0].RequestID + 1;
                //objDML.Add_Exception_Log("After", "");

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
                //Code Commented as we need only till download documents
                List<tbl_input_request_data> lstYet2StartData = entit.tbl_input_request_data.Where(x => x.Request_ID == strRequest_Id && x.Associate_Id == strAssociate_Id && x.Candidate_ID == strCandidate_Id).ToList<tbl_input_request_data>();
                List<tbl_initiation_tracker> lstMISLog = entit.tbl_initiation_tracker.Where(x => x.request_id == strRequest_Id && x.associate_id == strAssociate_Id && x.candidate_id == strCandidate_Id).ToList<tbl_initiation_tracker>();

                JsonCreater objFinal = new JsonCreater();
                string MessageId = Guid.NewGuid().ToString();
                string ServiceId = FilePath_Container.CaseCreation;
                string jsonFinal = objFinal.Final_Create_Case_Json(MessageId, lstFreshCase, lstCollege, lstYet2StartData, lstMISLog, fileupload, clientID, SBUID, clientName, ServiceId);
                objDML.Add_Exception_Log(jsonFinal, "Json_Data");
                string JsonDatRequests = Read_Json_DataForCaseCreation(jsonFinal, "Data", "CaseCreation", "clientSpecificFields");
                objDML.Add_Exception_Log(JsonDatRequests, "Json_Data");
                /////////////////////////// Add Json request into database //////////////
                //objDML.Add_Exception_Log(lstFreshCase.Count().ToString(), Convert.ToString(NewRequestID));
                //int ADDRequests = objDML.Insert_data_in_requests(Convert.ToString(NewRequestID), lstFreshCase, JsonDatRequests, clientID, SBUID);
                jsonFinal = objFinal.Final_Create_Case_Json(MessageId, lstFreshCase, lstCollege, lstYet2StartData, lstMISLog, fileupload, clientID, SBUID, clientName, ServiceId, Convert.ToString(NewRequestID));
                objDML.Insert_Json_in_requestracker(Convert.ToString(NewRequestID), jsonFinal);
                int iDML = objDML.Add_Request_Json_Detail(MessageId, "CaseCreation", jsonFinal);
                if (iDML == 1)
                {
                    bool ret = objExe.Write_JSON_TO_ServerRABBIT_MQ(jsonFinal);
                    output = ret ? "Success" : "Failed";
                    objDML.Add_Exception_Log(output, "");
                }

                //bool ret = objExe.Write_JSON_TO_RABBIT_MQ(jsonFinal); 
                //output = ret ? "Success" : "Failed";
            }
            catch (Exception ex)
            {
                output = "ex";
                //throw ex;
                int iException = objDML.Add_Exception_Log(ex.Message, "");
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
