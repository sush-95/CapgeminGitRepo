using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Entities;
using System.Globalization;
using Read_File_Processor;

namespace DataAccess_Utility
{
    public class DML_Utility
    {
        public int Add_Exception_Log(string Exception, string FunctionName = "")
        {
            int Value = 0;
            fadv_touchlessEntities entit = new fadv_touchlessEntities();
            tbl_exception_log tbl = new tbl_exception_log();
            List<tbl_exception_log> lst = new List<tbl_exception_log>();
            try
            {

                tbl.exception_log = Exception;
                tbl.function_name = FunctionName;

                lst.Add(tbl);
                entit.tbl_exception_log.AddRange(lst);
                entit.SaveChanges();
                Value = 1;
            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }

        public int Add_Request_Json_Detail(string message_id, string request_type, string json = "")
        {
            int Value = 0;
            fadv_touchlessEntities entit = new fadv_touchlessEntities();
            tbl_request_details tbl = new tbl_request_details();
            List<tbl_request_details> lst = new List<tbl_request_details>();
            try
            {
                if (string.IsNullOrEmpty(json.Trim()))
                {

                }

                tbl.messageid = message_id;
                tbl.json_text = json;
                tbl.request_type = request_type;

                lst.Add(tbl);
                entit.tbl_request_details.AddRange(lst);
                entit.SaveChanges();
                Value = 1;
            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }

        public int Add_Rows_Count_Data(string filepath, long rowcount)
        {
            int Value = 0;
            fadv_touchlessEntities entit = new fadv_touchlessEntities();
            tbl_mislog_count tbl = new tbl_mislog_count();
            List<tbl_mislog_count> lst = new List<tbl_mislog_count>();
            try
            {

                tbl.filePath = filepath;
                tbl.rowscount = rowcount;

                lst.Add(tbl);
                entit.tbl_mislog_count.AddRange(lst);
                entit.SaveChanges();
                Value = 1;
            }
            catch (Exception ex)
            {
                Value = 0;
                //throw ex;
            }
            return Value;
        }

        public int Add_Response_Json(long request_id, string json = "", string message_id = "", string service_id = "")
        {
            int Value = 0;
            fadv_touchlessEntities entit = new fadv_touchlessEntities();
            tbl_response_detail tbl = new tbl_response_detail();
            List<tbl_response_detail> lst = new List<tbl_response_detail>();

            tbl_request_details tblReq = new tbl_request_details();
            List<tbl_request_details> lstReq = new List<tbl_request_details>();
            try
            {

                tbl.request_id = request_id; // Int64.Parse(request_id);
                tbl.response_json = json;
                tbl.message_id = message_id;
                tbl.service_id = service_id;
                tbl.status = 1;

                lst.Add(tbl);
                entit.tbl_response_detail.AddRange(lst);
                entit.SaveChanges();
                // Update Request Status
                using (fadv_touchlessEntities entities = new fadv_touchlessEntities())
                {
                    tbl_request_details processData = entities.tbl_request_details.Where(x => x.id == request_id).First();

                    processData.Status = 1;
                    entities.SaveChanges();
                }
                Value = 1;
            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }

        public int Add_Response_Json(string message_id, string json = "")
        {
            int Value = 0;
            fadv_touchlessEntities entit = new fadv_touchlessEntities();
            tbl_response_detail tbl = new tbl_response_detail();
            List<tbl_response_detail> lst = new List<tbl_response_detail>();
            try
            {

                tbl.request_id = 1;
                tbl.response_json = json;

                lst.Add(tbl);
                entit.tbl_response_detail.AddRange(lst);
                entit.SaveChanges();
                Value = 1;
            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }

        public int Add_Response_Json_FreshCase_Update(long request_id, long response_id, List<tbl_yettostart_casecreation_data> lst, string json = "")
        {
            int Value = 0;
            try
            {
                if (lst.Count > 0)
                {
                    using (fadv_touchlessEntities entit = new fadv_touchlessEntities())
                    {
                        entit.tbl_yettostart_casecreation_data.AddRange(lst);
                        entit.SaveChanges();
                    }
                }
                Value = 1;
            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }

        public int Add_Response_Json(long request_id, List<tbl_yettostart_casecreation_data> lst, string json = "")
        {
            int Value = 0;
            try
            {
                if (lst.Count > 0)
                {
                    using (fadv_touchlessEntities entit = new fadv_touchlessEntities())
                    {
                        entit.tbl_yettostart_casecreation_data.AddRange(lst);
                        entit.SaveChanges();
                    }
                }
                tbl_response_detail tbl = new tbl_response_detail();
                List<tbl_response_detail> lstResponse = new List<tbl_response_detail>();
                tbl.request_id = request_id; // Int64.Parse(request_id);
                tbl.response_json = json;
                tbl.status = 1;

                lstResponse.Add(tbl);
                using (fadv_touchlessEntities entit = new fadv_touchlessEntities())
                {
                    entit.tbl_response_detail.AddRange(lstResponse);
                    entit.SaveChanges();

                    tbl_request_details processData = entit.tbl_request_details.Where(x => x.id == request_id).First();

                    processData.Status = 1;
                    entit.SaveChanges();
                }
                Value = 1;
            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }

        public int Add_Response_Json(long request_id, long response_id, List<Yet2Start_College> lst)
        {
            int Value = 0;
            try
            {
                if (lst.Count > 0)
                {
                    tbl_college_details tbl = new tbl_college_details();
                    List<tbl_college_details> lstResponse = new List<tbl_college_details>();
                    foreach (var obj in lst)
                    {
                        string college = obj.college.Trim();
                        string degree = obj.degree.Trim();
                        sbyte active = 1;

                        tbl = new tbl_college_details();
                        tbl.college = college.Trim();
                        tbl.degree = degree.Trim();
                        tbl.field_source = obj.field_source.Trim();
                        tbl.reqid = request_id;
                        tbl.resid = response_id;

                        tbl.active = active;
                        lstResponse.Add(tbl);
                    }
                    using (fadv_touchlessEntities entit = new fadv_touchlessEntities())
                    {
                        entit.tbl_college_details.AddRange(lstResponse);
                        entit.SaveChanges();
                    }
                    // Check for Check_Rule_Engine_CVT_UT //
                    Check_Rule_Engine_CVT_UT_New(request_id, response_id, lst);
                    //Check_Rule_Engine_CVT_UT(request_id, response_id, lst);
                }
                Value = 1;
            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }

        public int Check_Rule_Engine_CVT_UT_New(long request_id, long response_id, List<Yet2Start_College> lst)
        {
            int Value = 0;
            try
            {
                List<tbl_college_details> lstObj = new List<tbl_college_details>();
                fadv_touchlessEntities entity = new fadv_touchlessEntities();
                string Query = "set SQL_SAFE_UPDATES = 0;";
                Value = entity.Database.ExecuteSqlCommand(Query);
                //Query = "update fadv_touchless.tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM fadv_touchless.tbl_college_details where resId = " + response_id + " and reqId = " + request_id + " group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";
                Query = "update tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM tbl_college_details where resId = {0} and reqId = {1} group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";

                Value = entity.Database.ExecuteSqlCommand(Query, response_id, request_id);
                Value = 1;
            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }
        public int Check_Rule_Engine_CVT_UT(long request_id, long response_id, List<Yet2Start_College> lst)
        {
            int Value = 0;
            try
            {
                Get_Data_Utility objGet = new Get_Data_Utility();
                foreach (var obj in lst)
                {
                    int cvt = 0;
                    int ut = 0;
                    string college = obj.college.Trim();
                    string degree = obj.degree.Trim();
                    sbyte active = 1;
                    List<tbl_college_details> lstResponse = new List<tbl_college_details>();
                    lstResponse = objGet.Get_College_Details(request_id, response_id, college, degree);
                    foreach (var cd in lstResponse)
                    {
                        long cdid = cd.id;
                        string fieldsource = cd.field_source.Trim();
                        if (cd.field_source.ToLower() == "cvt")
                        {
                            cvt = 1;
                        }
                        else if (cd.field_source.ToLower() == "ut")
                        {
                            ut = 1;
                        }
                        if (cvt > 0 & ut > 0)
                        {
                            using (fadv_touchlessEntities entit = new fadv_touchlessEntities())
                            {
                                List<tbl_college_details> processData = entit.tbl_college_details.Where(x => x.field_source == "UT" && x.reqid == request_id && x.resid == response_id && x.college == college && x.degree == degree).ToList<tbl_college_details>();
                                foreach (var uddt in processData)
                                {
                                    tbl_college_details updatedata = entit.tbl_college_details.Where(x => x.id == uddt.id).First();
                                    updatedata.active = 9;
                                    entit.SaveChanges();
                                }
                            }
                        }
                    }
                }
                Value = 1;
            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }

        public int Update_Response_Status(long resId)
        {
            int Value = 0;
            try
            {
                using (fadv_touchlessEntities entities = new fadv_touchlessEntities())
                {
                    tbl_response_detail processData = entities.tbl_response_detail.Where(x => x.id == resId).First();

                    processData.status = 2;
                    entities.SaveChanges();
                }
                Value = 1;
            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }
        //koushik code
        public int Insert_FilePathIndocument_upload(List<string> Files, long requestID)
        {
            int Value = 0;
            fadv_touchlessEntities entit = new fadv_touchlessEntities();
            tbl_document_upload tbl = new tbl_document_upload();
            List<tbl_document_upload> lst = new List<tbl_document_upload>();
            DML_Utility objDML = new DML_Utility();

            try
            {
                foreach (var file in Files)
                {
                    tbl = new tbl_document_upload();
                    tbl.Document_Path = file;
                    tbl.Date_Created = DateTime.Now;
                    tbl.Date_Modified = DateTime.Now;
                    tbl.PartitionKey = 0;
                    tbl.Requestid = requestID;
                    lst.Add(tbl);
                }
                entit.tbl_document_upload.AddRange(lst);
                entit.SaveChanges();
                Value = 1;
            }
            catch (Exception ex)
            {
                objDML.Add_Exception_Log("Wipro exception : " + ex.Message, "Insert_FilePathIndocument_upload");
                Value = 0;
                throw ex;
            }
            return Value;
        }

        public int Insert_Json_in_requestracker(string RequestID, string json = "")
        {
            int Value = 0;
            fadv_touchlessEntities entit = new fadv_touchlessEntities();
            tbl_request_tracker tbl = new tbl_request_tracker();
            //List<tbl_request_tracker> lst = new List<tbl_request_tracker>();
            try
            {

                tbl.RequestID = Convert.ToUInt32(RequestID);
                tbl.Type = "CaseCreation";
                tbl.Json_Data = json;
                tbl.Operation_Date = DateTime.Now;
                tbl.Queue_Flag = 1;
                //lst.Add(tbl);
                entit.tbl_request_tracker.Add(tbl);
                entit.SaveChanges();
                Value = 1;
            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }
        public int Insert_Json_in_requesStateInstanse(long RequestID, int sequence, string state, long userID, string comment, sbyte isCurrent, sbyte IsReview, sbyte PartitionKey)
        {
            int Value = 0;
            fadv_touchlessEntities entit = new fadv_touchlessEntities();
            tbl_request_state_instance tbl = new tbl_request_state_instance();
            try
            {

                tbl.RequestID = RequestID;
                tbl.Sequence = sequence;// 1;
                tbl.StateID = state;// "REQ-0002";
                tbl.UserID = userID;// 165;
                tbl.Date_Created = DateTime.Now;
                tbl.Comments = comment;// "Case Creation by Touchless";
                tbl.Is_Current = isCurrent;// 1;
                tbl.IS_Review = IsReview;
                tbl.PartitionKey = PartitionKey;
                entit.tbl_request_state_instance.Add(tbl);
                entit.SaveChanges();
                Value = 1;
            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }

        public int Insert_data_in_requests(ref string RequestID, tbl_Wipro_Details list, string JsonDatRequests, string clientID, string SBUID)
        {
            int Value = 0;
            fadv_touchlessEntities entit = new fadv_touchlessEntities();
            Get_Data_Utility objGet = new Get_Data_Utility();

            tbl_requests tbl = new tbl_requests();
            //List<tbl_requests> lst = new List<tbl_requests>();
            sbyte Dateofbirth = 0;
            DML_Utility objDML = new DML_Utility();
            List<tbl_requests> ListRequestID = objGet.Get_RequestID();
            //objDML.Add_Exception_Log("Before", "");

            decimal NewRequestID = ListRequestID[0].RequestID + 1;
            RequestID = NewRequestID.ToString();
            int intRequestID = Convert.ToInt32(RequestID);
            try
            {
                //foreach (tbl_yettostart_casecreation_data item in list)
                //if (list.Count > 0)
                {
                    //tbl_yettostart_casecreation_data item = list[0];
                    // if (item.first_name != string.Empty) Dateofbirth = 1;
                    tbl.RequestID = Convert.ToUInt32(intRequestID);
                    tbl.ClientID = Convert.ToInt32(clientID);
                    // tbl.ClientID = 0;
                    tbl.First_Name = list.Employee_Name;
                    tbl.Last_Name = "";
                    tbl.Middle_Name = "";
                    tbl.Client_Ref_No = list.Candidte_Id;
                    tbl.Case_Date = DateTime.Now;
                    tbl.Subject_Detail = "FADV";
                    tbl.Subject_Type = "Candidate";
                    tbl.Is_Date_of_Birth = Dateofbirth;
                    tbl.Date_Of_Birth = Convert.ToDateTime(list.Date_Of_Birth, CultureInfo.InvariantCulture);// DateTime.ParseExact(CandidateDetails.Date_Of_Birth, "MM/dd/yyyy",CultureInfo.InvariantCulture);
                                                                                                             //Convert.ToDateTime(list.Date_Of_Birth);
                    tbl.Type_Of_Check = "Both";//Pre employment or Post employment or Both
                    tbl.Candidate_Authorization_Letter = "Yes";// Yes or No

                    tbl.Package_Type = "Soft Copy"; // Soft Copy or Hard copy
                    tbl.Srt_Data = "SRT";
                    tbl.Date_Created = DateTime.Now;
                    tbl.Date_Modified = DateTime.Now;
                    tbl.Is_Active = true;
                    //  tbl.SBUID =0;
                    tbl.LOA_Submitted = "Yes";
                    tbl.BVF_Submitted = "Yes";
                    tbl.PartitionKey = 0;
                    tbl.JSON_Data = JsonDatRequests;
                    tbl.SBUID = Convert.ToInt64(SBUID);
                    //lst.Add(tbl);

                    Value = 1;
                    //intRequestID++;
                    entit.tbl_requests.Add(tbl);
                    entit.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                objDML.Add_Exception_Log("Wipro exception : " + ex.Message, "Insert_data_in_requests");
                objDML.Add_Exception_Log("Wipro exception : " + ex.InnerException.Message, "Insert_data_in_requests");
                Value = 0;
                throw ex;
            }
            return Value;
        }


        public int updateYetTostart(long resId, long ExpresReqID)
        {
            int Value = 0;
            try
            {
                //fadv_touchlessEntities entit = new fadv_touchlessEntities();
                //tbl_yettostart_casecreation_data processData = entit.tbl_yettostart_casecreation_data.Where(x => x.queue_request_id == resId).First();
                //processData.ExpressRequestID = ExpresReqID;
                //entit.SaveChanges();

                //Value = 1;


                List<tbl_college_details> lstObj = new List<tbl_college_details>();
                fadv_touchlessEntities entity = new fadv_touchlessEntities();
                string Query = "set SQL_SAFE_UPDATES = 0;";
                Value = entity.Database.ExecuteSqlCommand(Query);
                //Query = "update fadv_touchless.tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM fadv_touchless.tbl_college_details where resId = " + response_id + " and reqId = " + request_id + " group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";
                Query = "update tbl_yettostart_casecreation_data set ExpressRequestID=" + ExpresReqID + " where queue_request_id=" + resId;

                Value = entity.Database.ExecuteSqlCommand(Query, ExpresReqID, resId);
                Value = 1;


            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }
        public int updateWiproDetails(string candidateID, string checkInitiated, long ExpresReqID)
        {
            int Value = 0;
            try
            {
                //fadv_touchlessEntities entit = new fadv_touchlessEntities();
                //tbl_yettostart_casecreation_data processData = entit.tbl_yettostart_casecreation_data.Where(x => x.queue_request_id == resId).First();
                //processData.ExpressRequestID = ExpresReqID;
                //entit.SaveChanges();

                //Value = 1;


                List<tbl_college_details> lstObj = new List<tbl_college_details>();
                fadv_touchlessEntities entity = new fadv_touchlessEntities();
                string Query = "set SQL_SAFE_UPDATES = 0;";
                Value = entity.Database.ExecuteSqlCommand(Query);
                //Query = "update fadv_touchless.tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM fadv_touchless.tbl_college_details where resId = " + response_id + " and reqId = " + request_id + " group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";
                Query = "update tbl_Wipro_details set ExpressRequestID=" + ExpresReqID + "  where Candidte_ID=" + candidateID + " and Check_Initiated='" + checkInitiated + "'";

                Value = entity.Database.ExecuteSqlCommand(Query, ExpresReqID, candidateID, checkInitiated);
                Value = 1;


            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }

        public int InsertPackageDetails(long ExpresReqID, string PackageID)
        {
            int Value = 0;
            try
            {
                List<tbl_college_details> lstObj = new List<tbl_college_details>();
                fadv_touchlessEntities entity = new fadv_touchlessEntities();
                string Query = "set SQL_SAFE_UPDATES = 0;";
                Value = entity.Database.ExecuteSqlCommand(Query);
                //Query = "update fadv_touchless.tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM fadv_touchless.tbl_college_details where resId = " + response_id + " and reqId = " + request_id + " group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";
                Query = "insert into tbl_request_packages (RequestID,PackageID) Values (" + ExpresReqID + "," + PackageID + ")";

                Value = entity.Database.ExecuteSqlCommand(Query);
                Value = 1;


            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }
        public int InsertCapgeminiTable(string Candidte_Id, string assigedTo, string Activity, string dueDate, string startDate, string firstName, string lastName, string activityCreationDate, string placeOfPosting, string sbu, string bgvAgency, string BotId)
        {
            int Value = 0;
            try
            {
              
                Get_Data_Utility getObj = new Get_Data_Utility();
                // string sql = "select count(*) from tbl_cap_gemini where Candidte_Id='" + Candidte_Id + "' and Check_Initiated='" + Check_Initiated + "' ;";
                string sql = "select * from tbl_cap_gemini where Candidate_ID='" + Candidte_Id.Trim() + "';";
                if ((getObj.GetListOnCandiadteID(Candidte_Id.Trim())).Count == 0)
                {
                    List<tbl_college_details> lstObj = new List<tbl_college_details>();
                    fadv_touchlessEntities entity = new fadv_touchlessEntities();
                    string Query = "set SQL_SAFE_UPDATES = 0;";
                    Value = entity.Database.ExecuteSqlCommand(Query);
                    Query = "Insert into `tbl_cap_gemini`(`Candidate_ID`,`Assigned_To`,`Activity`,`Due_date`,`Start_Date`,`First_Name`,`Last_Name`,`Activity_Creation_Date`,`Place_of_Posting`,`SBU`,`BGV_Agency`,`Entry_Date`,`Last_updated_time`,`Bot_Name`) values" +
                                      "('" + Candidte_Id.Trim() + "','" + assigedTo.Trim() + "','" + Activity.Trim() + "','" + dueDate.Trim() + "','" + startDate.Trim() + "','" + firstName.Trim() + "','" + lastName.Trim() + "','" + activityCreationDate.Trim() + "','" + placeOfPosting.Trim() + "','" + sbu.Trim() + "','" + bgvAgency.Trim() + "','" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + BotId + "')";

                    Value = entity.Database.ExecuteSqlCommand(Query);
                    Value = 1;
                }
                else
                {
                    List<tbl_cap_gemini> capList = getObj.GetListOnCandiadteID(Candidte_Id.Trim());
                    if (capList.Count != 0)
                    {
                        DateTime entrydate;
                        DateTime.TryParse(capList[0].EntryDate, out entrydate);                     
                        if ((DateTime.Now - entrydate).Days > 0&& (DateTime.Now - entrydate).Hours>6)
                        {
                            Value = 2;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }

        public int InsertWiproDetails(string Candidte_Id, string Employee_No, string Employee_Name, string Posting_Location, string Date_Of_Joining, string Fathers_Name, string Date_Of_Birth, string Employment_Type, string Check_Status, string Mapping_Date, string Check_Initiated, string POB_Location)
        {
            int Value = 0;
            try
            {
                Get_Data_Utility getObj = new Get_Data_Utility();
                string sql = "select count(*) from tbl_Wipro_details where Candidte_Id='" + Candidte_Id + "' and Check_Initiated='" + Check_Initiated + "' ;";
                if (getObj.getIsExist(sql) == 0)
                {
                    List<tbl_college_details> lstObj = new List<tbl_college_details>();
                    fadv_touchlessEntities entity = new fadv_touchlessEntities();
                    string Query = "set SQL_SAFE_UPDATES = 0;";
                    Value = entity.Database.ExecuteSqlCommand(Query);
                    //Query = "update fadv_touchless.tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM fadv_touchless.tbl_college_details where resId = " + response_id + " and reqId = " + request_id + " group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";
                    Query = "insert into tbl_Wipro_details (Candidte_Id,Employee_No,Employee_Name,Posting_Location,Date_Of_Joining,Fathers_Name,Date_Of_Birth,Employment_Type,Check_Status,Mapping_Date,Check_Initiated,POB_Location,IsProcessed)" +
                        " Values ('" + Candidte_Id + "','" + Employee_No + "','" + Employee_Name + "','" + Posting_Location + "','" + Date_Of_Joining + "','" + Fathers_Name + "','" + Date_Of_Birth + "','" + Employment_Type + "','" + Check_Status + "','" + Mapping_Date + "','" + Check_Initiated + "','" + POB_Location + "',0)";

                    Value = entity.Database.ExecuteSqlCommand(Query);
                    Value = 1;
                }

            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }

        public int InsertWiproCampusDetails(string ResumeID, string EmpID, string EmpName, string DateOfJoining, string FatherName, string DateofBirth, string ContactNumber, string EmailId, string Country, string Geo, string Qualification, string UniversityName, string CollegeName, string ModeOfEducation, string YearOfPassing, string AccountName, string AgencyNumber, string BGVInitiatedDate, string BGVInsuffRaisedDate, string InsuffComponent, string InsuffRaisedRemarks, string InsuffClearedRemarks, string BGVInsuffClearedDate, string BGVAssignedDate, string BGVQCRejectedDate, string BGVQCRejectedRemarks, string BGVSPOC, string AG_REPORTED_STATUS_BGV, string AG_REPORTED_DATE_BGV, string SLA, string AgencytoreverifyDate, string Agencytoreverifyremarks, string AgencyPlannedClosureDate, string PriorityFlag, string Division, string BGVStatus, string ActiveFlag, string BGV_Final_status)
        {
            int Value = 0;
            try
            {
                Get_Data_Utility getObj = new Get_Data_Utility();

                string sql = "select count(*) from tbl_wipro_campus_data where `Resume ID` like '%" + ResumeID + "%';";
                if (getObj.getIsExist(sql) == 0)
                {
                    List<tbl_college_details> lstObj = new List<tbl_college_details>();
                    fadv_touchlessEntities entity = new fadv_touchlessEntities();
                    string Query = "set SQL_SAFE_UPDATES = 0;";
                    Value = entity.Database.ExecuteSqlCommand(Query);
                    //Query = "update fadv_touchless.tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM fadv_touchless.tbl_college_details where resId = " + response_id + " and reqId = " + request_id + " group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";
                    Query = "INSERT INTO `tbl_wipro_campus_data`(`Resume ID`,`Emp ID`,`Emp Name`,`Date Of Joining`,`Father Name`,`Date of Birth`,`Contact Number`,`Email Id`,`Country`,`Geo`,`Qualification`,`University Name`,`College Name`,`Mode Of Education`,`Year Of Passing`,`Account Name`,`Agency Number`,`BGV Initiated Date`,`BGV Insuff Raised Date`,`Insuff Component`,`Insuff Raised Remarks`,`Insuff Cleared Remarks`,`BGV Insuff Cleared Date`,`BGV Assigned Date`,`BGV QC Rejected Date`,`BGV QC Rejected Remarks`,`BGV SPOC`,`AG_REPORTED_STATUS_BGV`,`AG_REPORTED_DATE_BGV`,`SLA`,`Agency to re-verify Date`,`Agency to re-verify remarks`,`Agency Planned Closure Date`,`Priority Flag`,`Division`,`BGV Status`,`Active Flag`,`BGV_Final_status`,`IspProcessed`)" +
                        " Values ('" + ResumeID + "','" + EmpID + "','" + EmpName + "','" + DateOfJoining + "','" + FatherName + "','" + DateofBirth + "','" + ContactNumber + "','" + EmailId + "','" + Country + "','" + Geo + "','" + Qualification + "','" + UniversityName + "','" + CollegeName + "','" + ModeOfEducation + "','" + YearOfPassing + "','" + AccountName + "','" + AgencyNumber + "','" + BGVInitiatedDate + "','" + BGVInsuffRaisedDate + "','" + InsuffComponent + "','" + InsuffRaisedRemarks + "','" + InsuffClearedRemarks + "','" + BGVInsuffClearedDate + "','" + BGVAssignedDate + "','" + BGVQCRejectedDate + "','" + BGVQCRejectedRemarks + "','" + BGVSPOC + "','" + AG_REPORTED_STATUS_BGV + "','" + AG_REPORTED_DATE_BGV + "','" + SLA + "','" + AgencytoreverifyDate + "','" + Agencytoreverifyremarks + "','" + AgencyPlannedClosureDate + "','" + PriorityFlag + "','" + Division + "','" + BGVStatus + "','" + ActiveFlag + "','" + BGV_Final_status + "',0)";

                    Value = entity.Database.ExecuteSqlCommand(Query);
                    Value = 1;
                }
            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }


        public int UpdateWiproDetails(string resumeID, string Check_Initiated)
        {
            int Value = 0;
            try
            {
                List<tbl_college_details> lstObj = new List<tbl_college_details>();
                fadv_touchlessEntities entity = new fadv_touchlessEntities();
                string Query = "set SQL_SAFE_UPDATES = 0;";
                Value = entity.Database.ExecuteSqlCommand(Query);
                //Query = "update fadv_touchless.tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM fadv_touchless.tbl_college_details where resId = " + response_id + " and reqId = " + request_id + " group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";
                Query = "update tbl_wipro_campus_data set IspProcessed=1 where `Resume ID`='" + resumeID + "'";

                Value = entity.Database.ExecuteSqlCommand(Query);
                Value = 1;
            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }
        public int InsertWiproGSHDetails(string ResumeNumber, string CandidateName, string OverallStatus, string FatherName, string DateofBirth, string ContractNumber, string EmailId, string AccountName, string AgencyNumber, string BGVInitiatedDate, string BGV1AssignedDate, string BGV2AssignedDate, string BGV1QCRejectedDate, string BGV2QCRejectedDate, string Qualification, string UniversityName, string CollegeName, string ModeOfEducation, string YearOfPassing, string DateOfJoining, string CurrentAddress, string PermanentAddress, string BGV1InsufRaisedDate, string BGV2InsufRaisedDate, string BGV1InsufClearedDate, string BGV2InsufClearedDate, string InsuffComponent, string BGVSPOC, string BGV1_status, string BGV2_status, string BGV1_Final_status, string BGV2_Final_status, string Insuff_Raised_Remarks)
        {
            int Value = 0;
            try
            {
                Get_Data_Utility getObj = new Get_Data_Utility();
                string sql = "select count(*) from tbl_wipro_gsh_data where `Resume Number` like '%" + ResumeNumber + "%' ;";
                if (getObj.getIsExist(sql) == 0)
                {
                    List<tbl_college_details> lstObj = new List<tbl_college_details>();
                    fadv_touchlessEntities entity = new fadv_touchlessEntities();
                    string Query = "set SQL_SAFE_UPDATES = 0;";
                    Value = entity.Database.ExecuteSqlCommand(Query);
                    //Query = "update fadv_touchless.tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM fadv_touchless.tbl_college_details where resId = " + response_id + " and reqId = " + request_id + " group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";
                    Query = "INSERT INTO `tbl_wipro_gsh_data`(`Resume Number`,`Candidate Name`,`Overall Status`,`Father Name`,`Date of Birth`,`Contract Number`,`Email Id`,`Account Name`,`Agency Number`,`BGV Initiated Date`,`BGV1 Assigned Date`,`BGV2 Assigned Date`,`BGV1 QC Rejected Date`,`BGV2 QC Rejected Date`,`Qualification`,`University Name`,`College Name`,`Mode Of Education`,`Year Of Passing`,`Date Of Joining`,`Current Address`,`Permanent Address`,`BGV1 Insuff Raised Date`,`BGV2 Insuff Raised Date`,`BGV1 Insuff Cleared Date`,`BGV2 Insuff Cleared Date`,`Insuff Component`,`BGV SPOC`,`BGV1_status`,`BGV2_status`,`BGV1_Final_status`,`BGV2_Final_status`,`Insuff_Raised_Remarks`,`IsProcessed`)" +
                        " Values ('" + ResumeNumber + "','" + CandidateName + "','" + OverallStatus + "','" + FatherName + "','" + DateofBirth + "','" + ContractNumber + "','" + EmailId + "','" + AccountName + "','" + AgencyNumber + "','" + BGVInitiatedDate + "','" + BGV1AssignedDate + "','" + BGV2AssignedDate + "','" + BGV1QCRejectedDate + "','" + BGV2QCRejectedDate + "','" + Qualification + "','" + UniversityName + "','" + CollegeName + "','" + ModeOfEducation + "','" + YearOfPassing + "','" + DateOfJoining + "','" + CurrentAddress + "','" + PermanentAddress + "','" + BGV1InsufRaisedDate + "','" + BGV2InsufRaisedDate + "','" + BGV1InsufClearedDate + "','" + BGV2InsufClearedDate + "','" + InsuffComponent + "','" + BGVSPOC + "','" + BGV1_status + "','" + BGV2_status + "','" + BGV1_Final_status + "','" + BGV2_Final_status + "','" + Insuff_Raised_Remarks + "',0)";

                    Value = entity.Database.ExecuteSqlCommand(Query);
                    Value = 1;
                }


            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }
        public int UpdateWiproGSHDetails(string resumeID, string Check_Initiated)
        {
            int Value = 0;
            try
            {
                List<tbl_college_details> lstObj = new List<tbl_college_details>();
                fadv_touchlessEntities entity = new fadv_touchlessEntities();
                string Query = "set SQL_SAFE_UPDATES = 0;";
                Value = entity.Database.ExecuteSqlCommand(Query);
                //Query = "update fadv_touchless.tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM fadv_touchless.tbl_college_details where resId = " + response_id + " and reqId = " + request_id + " group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";
                Query = "update tbl_wipro_gsh_data set IsProcessed=1 where `Resume Number`='" + resumeID + "'";

                Value = entity.Database.ExecuteSqlCommand(Query);
                Value = 1;


            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }

        public int InsertWiproInternalDetails(string EmployeeNumber, string EmployeeName, string DateOfJoining, string FatherName, string DateofBirth, string ContratNumber, string EmailId, string Country, string Geo, string Qualification, string UniversityName, string CollegeName, string ModeOfEducation, string YearOfPassing, string AccountName, string AgencyNumber, string BGVInitiatedDate, string BGVInsuffRaisedDate, string BGVInsuffClearedDate, string BGVAssignedDate, string BGVQCRejectedDate, string BGVSPOC, string AG_REPORTED_STATUS_BGV, string AG_REPORTED_DATE_BGV, string SLA, string AgencytoreverifyDate, string AgencytoreverifyRemarks, string AgencyPlannedClosureDate, string PriorityFlag, string ActiveFlag, string BGV_STATUS, string BGV_FINAL_STATUS)
        {
            int Value = 0;
            try
            {
                Get_Data_Utility getObj = new Get_Data_Utility();
                string sql = "select count(*) from tbl_wipro_internal_data where `Employee Number` like '%" + EmployeeNumber + "%' ;";
                if (getObj.getIsExist(sql) == 0)
                {
                    List<tbl_college_details> lstObj = new List<tbl_college_details>();
                    fadv_touchlessEntities entity = new fadv_touchlessEntities();
                    string Query = "set SQL_SAFE_UPDATES = 0;";
                    Value = entity.Database.ExecuteSqlCommand(Query);
                    //Query = "update fadv_touchless.tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM fadv_touchless.tbl_college_details where resId = " + response_id + " and reqId = " + request_id + " group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";
                    Query = "INSERT INTO `tbl_wipro_internal_data`(`Employee Number`,`Employee Name`,`Date Of Joining`,`Father Name`,`Date Of Birth`,`Contact Number`,`Email Id`,`Country`,`Geo`,`Qualification`,`University Name`,`College Name`,`Mode Of Education`,`Year Of Passing`,`Account Name`,`Agency Number`,`BGV Initiated Date`,`BGV Insuff Raised Date`,`BGV Insuff Cleared Date`,`BGV Assigned Date`,`BGV QC Rejected Date`,`BGV SPOC`,`AG_REPORTED_STATUS_BGV`,`AG_REPORTED_DATE_BGV`,`SLA`,`Agency to re-verify Date`,`Agency to re-verify Remarks`,`Agency Planned Closure Date`,`Priority Flag`,`Active Flag`,`BGV_STATUS`,`BGV_Final_status`,`IsProcessed`)" +
                        " Values ('" + EmployeeNumber + "','" + EmployeeName + "','" + DateOfJoining + "','" + FatherName + "','" + DateofBirth + "','" + ContratNumber + "','" + EmailId + "','" + Country + "','" + Geo + "','" + Qualification + "','" + UniversityName + "','" + CollegeName + "','" + ModeOfEducation + "','" + YearOfPassing + "','" + AccountName + "','" + AgencyNumber + "','" + BGVInitiatedDate + "','" + BGVInsuffRaisedDate + "','" + BGVInsuffClearedDate + "','" + BGVAssignedDate + "','" + BGVQCRejectedDate + "','" + BGVSPOC + "','" + AG_REPORTED_STATUS_BGV + "','" + AG_REPORTED_DATE_BGV + "','" + SLA + "','" + AgencytoreverifyDate + "','" + AgencytoreverifyRemarks + "','" + AgencyPlannedClosureDate + "','" + PriorityFlag + "','" + ActiveFlag + "','" + BGV_STATUS + "','" + BGV_FINAL_STATUS + "',0)";

                    Value = entity.Database.ExecuteSqlCommand(Query);
                    Value = 1;
                }
                else
                {
                    sql = "select  count(*)  from tbl_wipro_internal_data where `Employee Number` like '%" + EmployeeNumber + "%' and `Account Name` like '%LLOYDS TSB BANK PLC Database%' and BGV_STATUS in ('\"Insufficient Info\"','\"Agency to Verify\"','\"Insufficient Info Cleared to Agency\"','\"Insufficient Info  L2 Cleared to Agency\"');";
                    if (getObj.getIsExist(sql) > 0)
                    {
                        Console.WriteLine("Wipro exception : Sending the record to process agian after checking account name and Status for : " + EmployeeNumber.ToString());
                        Value = 2;
                    }
                    //else
                    //{
                    //    sql = "select * from tbl_wipro_internal_data where `Employee Number` like '%" + EmployeeNumber + "%' and `Account Name` like '%LLOYDS TSB BANK PLC Database%' and BGV_STATUS='\"Active\"';";
                    //    if (getObj.getIsExist(sql) >0)
                    //    {
                    //        sql = "select * from tbl_wipro_internal_data where `Employee Number` like '%" + EmployeeNumber + "%' and `Account Name` like '%LLOYDS TSB BANK PLC Database%' and BGV_Final_status in ('\"Insufficient Info\"','\"Agency to Verify\"','\"Insufficient Info Cleared to Agency\"','\"Insufficient Info  L2 Cleared to Agency\"');";
                    //        if (getObj.getIsExist(sql) >0)
                    //        {
                    //            Value = 2;
                    //        }
                    //    }
                    //}
                }


            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }
        public int UpdateWiproInternalDetails(string resumeID, string Check_Initiated)
        {
            int Value = 0;
            try
            {
                List<tbl_college_details> lstObj = new List<tbl_college_details>();
                fadv_touchlessEntities entity = new fadv_touchlessEntities();
                string Query = "set SQL_SAFE_UPDATES = 0;";
                Value = entity.Database.ExecuteSqlCommand(Query);
                //Query = "update fadv_touchless.tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM fadv_touchless.tbl_college_details where resId = " + response_id + " and reqId = " + request_id + " group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";
                Query = "update tbl_wipro_internal_data set IsProcessed=1 where `Employee Number`='" + resumeID + "'";

                Value = entity.Database.ExecuteSqlCommand(Query);
                Value = 1;


            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }

        public int InsertWiproLateralDetails(string ResumeNumber, string CandidateName, string FatherName, string DateofBirth, string ContractNumber,
                                  string EmailId, string Qualification, string UniversityName, string CollegeName, string ModeOfEducation, string YearOfPassing, string AccountName,
                                  string AgencyNumber, string BGVInitiatedDate, string BGV1InsufRaisedDate, string InsuffComponent, string InsuffRaisedRemarks,
                                  string InsuffClearedRemarks, string BGV1InsufClearedDate, string BGV1AssignedDate, string BGV1QCRejectedDate, string BGV2AssignedDate,
                                  string BGV2InsufRaisedDate, string BGV2InsuffComponent, string BGV2InsuffRaisedRemarks, string BGV2InsuffClearedRemarks,
                                  string BGV2InsufClearedDate, string BGV2QCRejectedDate, string DateOfJoining, string BGVSPOC, string AG_REPORTED_Status_BV1, string AG_REPORTED_Status_BV2,
                                  string AG_REPORT_date_BV1, string AG_REPORT_date_BV2, string priorityflag, string Divison, string BGV1_status,
                                  string BGV2_status, string BGV1_Final_status, string BGV2_Final_status)
        {
            int Value = 0;
            try
            {
                Get_Data_Utility getObj = new Get_Data_Utility();
                string sql = "select count(*) from tbl_wipro_lateral_data where `Resume Number` like '%" + ResumeNumber + "%' ;";
                if (getObj.getIsExist(sql) == 0)
                {
                    List<tbl_college_details> lstObj = new List<tbl_college_details>();
                    fadv_touchlessEntities entity = new fadv_touchlessEntities();
                    string Query = "set SQL_SAFE_UPDATES = 0;";
                    Value = entity.Database.ExecuteSqlCommand(Query);
                    //Query = "update fadv_touchless.tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM fadv_touchless.tbl_college_details where resId = " + response_id + " and reqId = " + request_id + " group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";
                    Query = "INSERT INTO `tbl_wipro_lateral_data`(`Resume Number`,`Candidate Name`,`Father Name`,`Date of Birth`,`Contact Number`,`Email Id`,`Qualification`,`University Name`,`College Name`,`Mode Of Education`,`Year Of Passing`,`Account Name`,`Agency Number`,`BGV Initiated Date`,`BGV1 Insuff Raised Date`,`Insuff Component`,`Insuff Raised Remarks`,`Insuff Cleared Remarks`,`BGV1 Insuff Cleared Date`,`BGV1 Assigned Date`,`BGV1 QC Rejected Date`,`BGV2 Assigned Date`,`BGV2 Insuff Raised Date`,`BGV2 Insuff Component`,`BGV2 Insuff Raised Remarks`,`BGV2 Insuff Cleared Remarks`,`BGV2 Insuff Cleared Date`,`BGV2 QC Rejected Date`,`Date Of Joining`,`BGV SPOC`,`AG_REPORTED_Status_BV1`,`AG_REPORTED_Status_BV2`,`AG_REPORT_date_BV1`,`AG_REPORT_date_BV2`,`priority flag`,`Divison`,`BGV1_status`,`BGV2_status`,`BGV1_Final_status`,`BGV2_Final_status`,`IsProcessed`)" +
                        " Values ('" + ResumeNumber + "','" + CandidateName + "','" + FatherName + "','" + DateofBirth + "','" + ContractNumber
                        + "','" + EmailId + "','" + Qualification + "','" + UniversityName + "','" + CollegeName + "','" + ModeOfEducation + "','" + YearOfPassing + "','" + AccountName
                        + "','" + AgencyNumber + "','" + BGVInitiatedDate + "','" + BGV1InsufRaisedDate + "','" + InsuffComponent + "','" + InsuffRaisedRemarks
                        + "','" + InsuffClearedRemarks + "','" + BGV1InsufClearedDate + "','" + BGV1AssignedDate + "','" + BGV1QCRejectedDate + "','" + BGV2AssignedDate
                        + "','" + BGV2InsufRaisedDate + "','" + BGV2InsuffComponent + "','" + BGV2InsuffRaisedRemarks + "','" + BGV2InsuffClearedRemarks
                        + "','" + BGV2InsufClearedDate + "','" + BGV2QCRejectedDate + "','" + DateOfJoining + "','" + BGVSPOC + "','" + AG_REPORTED_Status_BV1 + "','" + AG_REPORTED_Status_BV2
                        + "','" + AG_REPORT_date_BV1 + "','" + AG_REPORT_date_BV2 + "','" + priorityflag + "','" + Divison + "','" + BGV1_status
                        + "','" + BGV2_status + "','" + BGV1_Final_status + "','" + BGV2_Final_status + "',0)";

                    Value = entity.Database.ExecuteSqlCommand(Query);
                    Value = 1;
                }


            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }
        public int UpdateWiproLateralDetails(string resumeID, string Check_Initiated)
        {
            int Value = 0;
            try
            {
                List<tbl_college_details> lstObj = new List<tbl_college_details>();
                fadv_touchlessEntities entity = new fadv_touchlessEntities();
                string Query = "set SQL_SAFE_UPDATES = 0;";
                Value = entity.Database.ExecuteSqlCommand(Query);
                //Query = "update fadv_touchless.tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM fadv_touchless.tbl_college_details where resId = " + response_id + " and reqId = " + request_id + " group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";
                Query = "update tbl_wipro_lateral_data set IsProcessed=1 where `Resume Number`='" + resumeID + "'";

                Value = entity.Database.ExecuteSqlCommand(Query);
                Value = 1;


            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }

        public int InsertWiproContractDetails(string ResumeNumber, string CandidateName, string OverallStatus, string FatherName, string DateofBirth, string ContractNumber, string EmailId, string AccountName, string AgencyNumber, string BGVInitiatedDate, string BGV1AssignedDate, string BGV2AssignedDate, string BGV1QCRejectedDate, string BGV2QCRejectedDate, string Qualification, string UniversityName, string CollegeName, string ModeOfEducation, string YearOfPassing, string DateOfJoining, string CurrentAddress, string PermanentAddress, string BGV1InsufRaisedDate, string BGV2InsufRaisedDate, string BGV1InsufClearedDate, string BGV2InsufClearedDate, string InsuffComponent, string InsuffRaisedRemarks, string InsuffClearedRemarks, string BGVSPOC, string BGV1_status, string BGV2_status, string BGV1_Final_status, string BGV2_Final_status)
        {
            int Value = 0;
            try
            {
                Get_Data_Utility getObj = new Get_Data_Utility();
                string sql = "select count(*) from tbl_wipro_contract_data where `Resume Number` like '%" + ResumeNumber + "%' ;";
                if (getObj.getIsExist(sql) == 0)
                {
                    List<tbl_college_details> lstObj = new List<tbl_college_details>();
                    fadv_touchlessEntities entity = new fadv_touchlessEntities();
                    string Query = "set SQL_SAFE_UPDATES = 0;";
                    Value = entity.Database.ExecuteSqlCommand(Query);
                    //Query = "update fadv_touchless.tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM fadv_touchless.tbl_college_details where resId = " + response_id + " and reqId = " + request_id + " group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";
                    Query = "INSERT INTO `tbl_wipro_contract_data`(`Resume Number`,`Candidate Name`,`Overall Status`,`Father Name`,`Date of Birth`,`Contract Number`,`Email Id`,`Account Name`,`Agency Number`,`BGV Initiated Date`,`BGV1 Assigned Date`,`BGV2 Assigned Date`,`BGV1 QC Rejected Date`,`BGV2 QC Rejected Date`,`Qualification`,`University Name`,`College Name`,`Mode Of Education`,`Year Of Passing`,`Date Of Joining`,`Current Address`,`Permanent Address`,`BGV1 Insuff Raised Date`,`BGV2 Insuff Raised Date`,`BGV1 Insuff Cleared Date`,`BGV2 Insuff Cleared Date`,`Insuff Component`,`Insuff Raised Remarks`,`Insuff Cleared Remarks`,`BGV SPOC`,`BGV1_status`,`BGV2_status`,`BGV1_Final_status`,`BGV2_Final_status`,`IsProcessed`)" +
                        " Values ('" + ResumeNumber + "','" + CandidateName + "','" + OverallStatus + "','" + FatherName + "','" + DateofBirth + "','" + ContractNumber + "','" + EmailId + "','" + AccountName + "','" + AgencyNumber + "','" + BGVInitiatedDate + "','" + BGV1AssignedDate + "','" + BGV2AssignedDate + "','" + BGV1QCRejectedDate + "','" + BGV2QCRejectedDate + "','" + Qualification + "','" + UniversityName + "','" + CollegeName + "','" + ModeOfEducation + "','" + YearOfPassing + "','" + DateOfJoining + "','" + CurrentAddress + "','" + PermanentAddress + "','" + BGV1InsufRaisedDate + "','" + BGV2InsufRaisedDate + "','" + BGV1InsufClearedDate + "','" + BGV2InsufClearedDate + "','" + InsuffComponent + "','" + InsuffRaisedRemarks + "','" + InsuffClearedRemarks + "','" + BGVSPOC + "','" + BGV1_status + "','" + BGV2_status + "','" + BGV1_Final_status + "','" + BGV2_Final_status + "',0)";

                    Value = entity.Database.ExecuteSqlCommand(Query);
                    Value = 1;
                }


            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }
        public int UpdateWiproContractDetails(string resumeID, string Check_Initiated)
        {
            int Value = 0;
            try
            {
                List<tbl_college_details> lstObj = new List<tbl_college_details>();
                fadv_touchlessEntities entity = new fadv_touchlessEntities();
                string Query = "set SQL_SAFE_UPDATES = 0;";
                Value = entity.Database.ExecuteSqlCommand(Query);
                //Query = "update fadv_touchless.tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM fadv_touchless.tbl_college_details where resId = " + response_id + " and reqId = " + request_id + " group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";
                Query = "update tbl_wipro_contract_data set IsProcessed=1 where `Resume Number`='" + resumeID + "'";

                Value = entity.Database.ExecuteSqlCommand(Query);
                Value = 1;


            }
            catch (Exception ex)
            {
                Value = 0;
                throw ex;
            }
            return Value;
        }


    }
}
