using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Entities;

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
                Query = "update fadv_touchless.tbl_college_details a inner join (SELECT college, degree, count(*) as c FROM fadv_touchless.tbl_college_details where resId = " + response_id + " and reqId = " + request_id + " group by college, degree, active) b  on  a.college = b.college and a.degree = b.degree and field_source = 'ut' and b.c > 1 set a.active = 9;";

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

        public int Insert_Json_in_requestracker(string RequestID, string json = "")
        {
            int Value = 0;
            fadv_touchlessEntities entit = new fadv_touchlessEntities();
            tbl_request_tracker tbl = new tbl_request_tracker();
            List<tbl_request_tracker> lst = new List<tbl_request_tracker>();
            try
            {

                //tbl.RequestID = Convert.ToUInt32(RequestID);
                tbl.Type = "";
                tbl.Json_Data = json;
                tbl.Operation_Date = DateTime.Now;

                lst.Add(tbl);
                entit.tbl_request_tracker.AddRange(lst);
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
        public int Insert_data_in_requests(string RequestID, List<tbl_yettostart_casecreation_data> list)
        {
            int Value = 0;
            fadv_touchlessEntities entit = new fadv_touchlessEntities();
            tbl_requests tbl = new tbl_requests();
            List<tbl_requests> lst = new List<tbl_requests>();
            sbyte Dateofbirth = 0;
            int intRequestID= Convert.ToInt32(RequestID);
            try
            {
                foreach (tbl_yettostart_casecreation_data item in list)
                {
                    if (item.first_name != string.Empty) Dateofbirth = 1;
                    tbl.RequestID = Convert.ToUInt32(intRequestID);
                    // tbl.ClientID = 0;
                    tbl.First_Name = item.first_name;
                    tbl.Last_Name = item.last_name;
                    tbl.Middle_Name = "";
                    tbl.Client_Ref_No = item.client_ref_no;
                    tbl.Case_Date = DateTime.Now;
                    tbl.Subject_Detail = "";
                    tbl.Subject_Type = "";
                    tbl.Is_Date_of_Birth = Dateofbirth;
                    tbl.Date_Of_Birth = Convert.ToDateTime(item.date_of_birth);
                    tbl.Type_Of_Check = "";//Pre employment or Post employment or Both
                    tbl.Candidate_Authorization_Letter = "";// Yes or No

                    tbl.Package_Type = ""; // Soft Copy or Hard copy
                    tbl.Srt_Data = "SRT";
                    tbl.Date_Created = DateTime.Now;
                    tbl.Date_Modified = DateTime.Now;
                    tbl.Is_Active = true;
                    //  tbl.SBUID =0;
                    tbl.LOA_Submitted = "Yes";
                    tbl.BVF_Submitted = "Yes";
                    tbl.PartitionKey = 0;
                    tbl.JSON_Data = "";

                    lst.Add(tbl);
                   
                    Value = 1;
                    intRequestID++;
                }

                entit.tbl_requests.AddRange(lst);
                entit.SaveChanges();

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
