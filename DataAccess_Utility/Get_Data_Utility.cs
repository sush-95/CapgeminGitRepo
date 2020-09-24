using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.Entity;

namespace DataAccess_Utility
{
    public class Get_Data_Utility
    {
        public List<tbl_cap_gemini> Get_UnProcessedRequestsCapGemini()
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey=" + ImportKey + " and Request_ID not in (select request_id from tbl_initiation_tracker)").ToList<tbl_input_request_data>();
            string Query = "Select Candidate_ID,First_Name,Last_Name from tbl_cap_gemini where Is_Processed=0;";
            List<tbl_cap_gemini> list = entity.Database.SqlQuery<tbl_cap_gemini>(Query).ToList<tbl_cap_gemini>(); 
            return list;
        }
        public bool IsCapgeminiDuplicate(string CandidateID)
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey=" + ImportKey + " and Request_ID not in (select request_id from tbl_initiation_tracker)").ToList<tbl_input_request_data>();
            string Query = "select * from tbl_cap_gemini where Candidate_ID='" + CandidateID + "'";
            List<tbl_cap_gemini> list = entity.Database.SqlQuery<tbl_cap_gemini>(Query).ToList<tbl_cap_gemini>(); ;
            return (list.Count > 1) ? true : false;
        }
        public List<tbl_cap_gemini> GetListOnCandiadteID(string CandidateID)
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey=" + ImportKey + " and Request_ID not in (select request_id from tbl_initiation_tracker)").ToList<tbl_input_request_data>();
            string Query = "select Candidate_ID,First_Name,Last_Name,Entry_Date from tbl_cap_gemini where Candidate_ID='" + CandidateID + "'";
            List<tbl_cap_gemini> list = entity.Database.SqlQuery<tbl_cap_gemini>(Query).ToList<tbl_cap_gemini>(); ;
            return list;
        }
        public List<tbl_wipro_campus_data> Get_UnProcessedRequests()
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey=" + ImportKey + " and Request_ID not in (select request_id from tbl_initiation_tracker)").ToList<tbl_input_request_data>();
            string Query = "select `Resume ID` as ResumeID, `Emp ID` as EmpID, `Emp Name` as EmpName from tbl_wipro_campus_data where IspProcessed=0;";
            List<tbl_wipro_campus_data> list = entity.Database.SqlQuery<tbl_wipro_campus_data>(Query).ToList<tbl_wipro_campus_data>(); ;
            return list;
        }
        public bool IsCampusDuplicate(string ResumeID)
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey=" + ImportKey + " and Request_ID not in (select request_id from tbl_initiation_tracker)").ToList<tbl_input_request_data>();
            string Query = "select * from tbl_wipro_campus_data where `Resume ID`='" + ResumeID + "'";
            List<tbl_wipro_campus_data> list = entity.Database.SqlQuery<tbl_wipro_campus_data>(Query).ToList<tbl_wipro_campus_data>(); ;
            return (list.Count > 1) ? true : false;
        }
        public List<tbl_wipro_gsh_data> Get_UnProcessedGSHRequests()
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey=" + ImportKey + " and Request_ID not in (select request_id from tbl_initiation_tracker)").ToList<tbl_input_request_data>();
            string Query = "select `Resume Number` as ResumeNumber,  `Candidate Name` as CandidateName from tbl_wipro_gsh_data where IsProcessed=0;";
            List<tbl_wipro_gsh_data> list = entity.Database.SqlQuery<tbl_wipro_gsh_data>(Query).ToList<tbl_wipro_gsh_data>(); ;
            return list;
        }
        public bool IsGSHDuplicate(string ResumeID)
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey=" + ImportKey + " and Request_ID not in (select request_id from tbl_initiation_tracker)").ToList<tbl_input_request_data>();
            string Query = "select * from tbl_wipro_gsh_data where `Resume Number`='" + ResumeID + "'";
            List<tbl_wipro_gsh_data> list = entity.Database.SqlQuery<tbl_wipro_gsh_data>(Query).ToList<tbl_wipro_gsh_data>(); ;
            return (list.Count > 1) ? true : false;
        }
        public List<tbl_wipro_internal_data> Get_UnProcessedInternalRequests()
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey=" + ImportKey + " and Request_ID not in (select request_id from tbl_initiation_tracker)").ToList<tbl_input_request_data>();
            string Query = "select `Employee Number` as EmployeeNumber,  `Employee Name` as EmployeeName from tbl_wipro_internal_data where IsProcessed=0;";
            List<tbl_wipro_internal_data> list = entity.Database.SqlQuery<tbl_wipro_internal_data>(Query).ToList<tbl_wipro_internal_data>(); ;
            return list;
        }
        public bool IsInternalDuplicate(string ResumeID)
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey=" + ImportKey + " and Request_ID not in (select request_id from tbl_initiation_tracker)").ToList<tbl_input_request_data>();
            string Query = "select * from tbl_wipro_internal_data where `Employee Number`='" + ResumeID + "'";
            List<tbl_wipro_internal_data> list = entity.Database.SqlQuery<tbl_wipro_internal_data>(Query).ToList<tbl_wipro_internal_data>(); ;
            return (list.Count > 1) ? true : false;
        }
        public List<tbl_wipro_lateral_data> Get_UnProcessedLateralRequests()
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey=" + ImportKey + " and Request_ID not in (select request_id from tbl_initiation_tracker)").ToList<tbl_input_request_data>();
            string Query = "select `Resume Number` as ResumeNumber,  `Candidate Name` as CandidateName from tbl_wipro_lateral_data where IsProcessed=0;";
            List<tbl_wipro_lateral_data> list = entity.Database.SqlQuery<tbl_wipro_lateral_data>(Query).ToList<tbl_wipro_lateral_data>(); ;
            return list;
        }
        public bool IsLateralDuplicate(string ResumeID)
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey=" + ImportKey + " and Request_ID not in (select request_id from tbl_initiation_tracker)").ToList<tbl_input_request_data>();
            string Query = "select * from tbl_wipro_lateral_data where `Resume Number`='" + ResumeID + "'";
            List<tbl_wipro_lateral_data> list = entity.Database.SqlQuery<tbl_wipro_lateral_data>(Query).ToList<tbl_wipro_lateral_data>(); ;
            return (list.Count > 1) ? true : false;
        }
        public List<tbl_wipro_contract_data> Get_UnProcessedContractRequests()
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey=" + ImportKey + " and Request_ID not in (select request_id from tbl_initiation_tracker)").ToList<tbl_input_request_data>();
            string Query = "select `Resume Number` as ResumeNumber,  `Candidate Name` as CandidateName from tbl_wipro_contract_data where IsProcessed=0;";
            List<tbl_wipro_contract_data> list = entity.Database.SqlQuery<tbl_wipro_contract_data>(Query).ToList<tbl_wipro_contract_data>(); ;
            return list;
        }
        public bool IscontractDuplicate(string ResumeID)
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey=" + ImportKey + " and Request_ID not in (select request_id from tbl_initiation_tracker)").ToList<tbl_input_request_data>();
            string Query = "select * from tbl_wipro_contract_data where `Resume Number`='" + ResumeID + "'";
            List<tbl_wipro_contract_data> list = entity.Database.SqlQuery<tbl_wipro_contract_data>(Query).ToList<tbl_wipro_contract_data>(); ;
            return (list.Count > 1) ? true : false;
        }

        public tbl_Wipro_Details Get_WiproCandiateDetails(string Candidte_Id, string Check_Initiated)
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey=" + ImportKey + " and Request_ID not in (select request_id from tbl_initiation_tracker)").ToList<tbl_input_request_data>();
            string Query = "select * from tbl_Wipro_details where Candidte_Id='" + Candidte_Id + "' and Check_Initiated='" + Check_Initiated + "'";
            List<tbl_Wipro_Details> list = entity.Database.SqlQuery<tbl_Wipro_Details>(Query).ToList<tbl_Wipro_Details>(); ;
            return list[0];
        }
        public bool IsDuplicate(string Candidte_Id, string Check_Initiated)
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey=" + ImportKey + " and Request_ID not in (select request_id from tbl_initiation_tracker)").ToList<tbl_input_request_data>();
            string Query = "select * from tbl_Wipro_details where Candidte_Id=" + Candidte_Id + " and Check_Initiated='" + Check_Initiated + "'";
            List<tbl_Wipro_Details> list = entity.Database.SqlQuery<tbl_Wipro_Details>(Query).ToList<tbl_Wipro_Details>(); ;
            return (list.Count > 1) ? true : false;
        }

        public List<tbl_yettostart_casecreation_data> Get_tbl_yettostart_casecreation_data(long ExpressReqID)
        {
            List<tbl_yettostart_casecreation_data> lstObj = new List<tbl_yettostart_casecreation_data>();
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            lstObj = entity.tbl_yettostart_casecreation_data.Where(x => x.ExpressRequestID == ExpressReqID).ToList<tbl_yettostart_casecreation_data>();
            return lstObj;
        }

        public List<tbl_response_detail> Get_Response_Data_ToBe_Process(string ServiceId)
        {
            List<tbl_response_detail> lstObj = new List<tbl_response_detail>();
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_response_detail.SqlQuery("SELECT * FROM tbl_response_detail where status=1 and service_id='"+ ServiceId + "' order by CreatedOn desc limit 1;").ToList<tbl_response_detail>();
            lstObj = entity.tbl_response_detail.SqlQuery("SELECT * FROM tbl_response_detail where status=1 and service_id={0} order by CreatedOn desc limit 1;", ServiceId).ToList<tbl_response_detail>();

            return lstObj;
        }

        public List<tbl_input_request_data> Get_New_Request_Id_List(string ImportKey) //Get_FreshCase_YetToStart(string ImportKey)
        {
            List<tbl_initiation_tracker> lstObjMISData = new List<tbl_initiation_tracker>();
            List<tbl_input_request_data> lstObj = new List<tbl_input_request_data>();
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            tbl_initiation_tracker tblMISData = new tbl_initiation_tracker();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey={0} and Request_ID not in (select request_id from tbl_initiation_tracker)", ImportKey).ToList<tbl_input_request_data>();
            lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey={0} and Request_ID not in (select request_id from tbl_initiation_tracker where Active is null Or Active!=0)", ImportKey).ToList<tbl_input_request_data>();
            foreach (var obj in lstObj)
            {
                long Id = long.Parse(obj.Id.ToString());
                // Update Request Status
                using (fadv_touchlessEntities entities = new fadv_touchlessEntities())
                {
                    tbl_input_request_data processData = entities.tbl_input_request_data.Where(x => x.Id == Id).First();

                    processData.Active = 9;
                    entities.SaveChanges();
                }
            }
            lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey={0} and Active=9", ImportKey).ToList<tbl_input_request_data>();
            foreach (var objInsert in lstObj)
            {
                tblMISData = new tbl_initiation_tracker();
                //request_id, candidate_id, associate_id, bgv_type, name, account
                tblMISData.request_id = objInsert.Request_ID;
                tblMISData.candidate_id = objInsert.Candidate_ID;
                tblMISData.associate_id = objInsert.Associate_Id;
                tblMISData.bgv_type = objInsert.BGV_Type;
                tblMISData.name = objInsert.Name;
                tblMISData.account = objInsert.Account;
                lstObjMISData.Add(tblMISData);
            }
            entity.tbl_initiation_tracker.AddRange(lstObjMISData);
            entity.SaveChanges();

            return lstObj;
        }

        public long Get_Request_Id(string strMessageId, string RequestType)
        {
            List<tbl_request_details> lstObj = new List<tbl_request_details>();
            string Query = "select * from tbl_request_details where json_text like'%" + strMessageId + "%' ";
            //Query = "select * from tbl_request_details where messageid='" + strMessageId + "'";
            //Query = "select * from tbl_request_details where messageid like {0}";
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            try
            {
                lstObj = entity.tbl_request_details.SqlQuery(Query).ToList<tbl_request_details>();
                return lstObj.Count > 0 ? string.IsNullOrEmpty(lstObj[0].id.ToString()) ? 0 : long.Parse(lstObj[0].id.ToString()) : 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long Get_FreshCase_Response_Id(string strRequestId)
        {
            List<tbl_yettostart_casecreation_data> lstObj = new List<tbl_yettostart_casecreation_data>();
            tbl_yettostart_casecreation_data tbl = new tbl_yettostart_casecreation_data();
            //string Query = "select * from tbl_yettostart_casecreation_data where queue_request_id='" + strRequestId + "'";
            string Query = "select * from tbl_yettostart_casecreation_data where queue_request_id={0}";
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            try
            {
                lstObj = entity.tbl_yettostart_casecreation_data.SqlQuery(Query, strRequestId).ToList<tbl_yettostart_casecreation_data>();
                return lstObj.Count > 0 ? string.IsNullOrEmpty(lstObj[0].id.ToString()) ? 0 : long.Parse(lstObj[0].id.ToString()) : 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long Get_Request_Id_new(string strMessageId, string RequestType)
        {
            List<tbl_request_details> lstObj = new List<tbl_request_details>();
            //string Query = "select * from tbl_request_details where messageid='" + strMessageId + "' and request_type = '" + RequestType + "'";
            string Query = "select * from tbl_request_details where messageid={0} and request_type = {1}";
            //Query = "select * from tbl_request_details where messageid='" + strMessageId + "'";
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            try
            {
                lstObj = entity.tbl_request_details.SqlQuery(Query, strMessageId, RequestType).ToList<tbl_request_details>();
                return lstObj.Count > 0 ? string.IsNullOrEmpty(lstObj[0].id.ToString()) ? 0 : long.Parse(lstObj[0].id.ToString()) : 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<tbl_request_details> Get_Last_Request(string serviceID)
        {
            List<tbl_request_details> lstObj = new List<tbl_request_details>();
            string Query = "select * from tbl_request_details where request_type='" + serviceID + "' order by CreatedOn desc limit 1";
            fadv_touchlessEntities entity = new fadv_touchlessEntities();

            try
            {
                lstObj = entity.tbl_request_details.SqlQuery(Query).ToList<tbl_request_details>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstObj;
        }
        public List<tbl_college_details> Get_College_Details(long reqid, long resid, string college, string degree)
        {
            List<tbl_college_details> lstObj = new List<tbl_college_details>();
            //string Query = "select * from tbl_college_details where active=1 and reqid="+ reqid + " and resid=" + resid + " and college='" + college + "' and degree='" + degree + "' order by field_source";
            string Query = "select * from tbl_college_details where active=1 and reqid={0} and resid={1} and college={2} and degree={3} order by field_source";
            fadv_touchlessEntities entity = new fadv_touchlessEntities();

            try
            {
                lstObj = entity.tbl_college_details.SqlQuery(Query, reqid, resid, college, degree).ToList<tbl_college_details>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstObj;
        }

        public List<tbl_config_value> Get_Cofig_Details(string configType)
        {
            List<tbl_config_value> lstObj = new List<tbl_config_value>();
            fadv_touchlessEntities entity = new fadv_touchlessEntities();

            try
            {
                lstObj = entity.tbl_config_value.Where(x => x.configtype == configType).ToList<tbl_config_value>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstObj;
        }
        // koushik code

        public List<tbl_requests> Get_RequestID()
        {
            List<tbl_requests> lstObj = new List<tbl_requests>();
            string Query = "select * from tbl_requests  order by RequestID desc limit 1";
            fadv_touchlessEntities entity = new fadv_touchlessEntities();

            try
            {
                lstObj = entity.tbl_requests.SqlQuery(Query).ToList<tbl_requests>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstObj;
        }
        public List<tbl_request_state_instance> GetAllOpenState()
        {
            List<tbl_request_state_instance> lstObj = new List<tbl_request_state_instance>();
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            string Query = "select * from tbl_request_state_instance where Is_Current=1 and requestID in(select requestID from tbl_request_state_instance where Comments= 'Case Created by Touchless');";
            try
            {
                lstObj = entity.tbl_request_state_instance.SqlQuery(Query).ToList<tbl_request_state_instance>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstObj;
        }

        public List<tbl_document_data> Get_DataEntry(string requesID)
        {
            List<tbl_document_data> lstObj = new List<tbl_document_data>();
            string Query = "select * from tbl_document_data Where RequestID=" + requesID;
            fadv_touchlessEntities entity = new fadv_touchlessEntities();

            try
            {
                lstObj = entity.tbl_document_data.SqlQuery(Query).ToList<tbl_document_data>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstObj;
        }

        public List<string> Get_AutoDataCaseCreationJson(string sql)
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            DataTable retVal = new DataTable();
            retVal = entity.Database.SqlQuery<DataTable>(sql).FirstOrDefault();
            var sequenceQueryResult = entity.Database.SqlQuery<string>(sql).ToList();
            return sequenceQueryResult;
        }
        public List<DatabaseCheckRecords> getDatabaseCheckRecords(string requestid)
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            string query = @"select First_Name,Middle_Name,Last_Name,CAST(DATE_FORMAT(Date_Of_Birth, '%d/%m/%Y') AS char) As Date_Of_Birth,JSON_Data,a.ClientID,Is_Bulk from tbl_requests a inner join 
       tbl_client_master b on a.ClientID = b.ClientID and b.is_active = 1 where a.RequestID =  " + requestid;
            var dt = entity.Database.SqlQuery<DatabaseCheckRecords>(query, requestid).ToList();
            return dt;
        }
        public int insertAutoDataentryDocumentJson(string requestid, string Jsondata)
        {

            int count = Convert.ToInt32(GetDocumentDataCount(requestid)[0]);
            string sql = "";
            if (count > 0)
            {
                sql = "update tbl_document_data set DocumentID='160000',Document_Version='1',Data_Sequence='1',JSON_Data=@in_Json_data,Date_Created=now(),Date_Modified=now(),PartitionKey='0' where RequestID=@in_reqid";
            }
            else
            {
                sql = " insert into tbl_document_data (RequestID,DocumentID,Document_Version,Data_Sequence,JSON_Data,Date_Created,Date_Modified,PartitionKey) values (@in_reqid,'160000','1','1',@in_Json_data,now(),now(),'0'); ";

            }
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            MySqlParameter[] queryParams = new MySqlParameter[] {
                                        new MySqlParameter("in_reqid", requestid),
                                        new MySqlParameter("in_Json_data", Jsondata),
            };
            var sequenceQueryResult = entity.Database.ExecuteSqlCommand(sql, queryParams);

            return sequenceQueryResult;
        }
        public List<string> GetDocumentDataCount(string rquestID)
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            DataTable retVal = new DataTable();
            string sql = "select count(*) from tbl_document_data where RequestID=" + rquestID + ";";
            retVal = entity.Database.SqlQuery<DataTable>(sql).FirstOrDefault();
            var sequenceQueryResult = entity.Database.SqlQuery<string>(sql).ToList();
            return sequenceQueryResult;
        }
        public DataTable updateStateID(string requestid)
        {

            fadv_touchlessEntities entity = new fadv_touchlessEntities();

            string sql = @"select p.PackageID from tbl_packages p inner join tbl_request_packages rp on p.PackageID = rp.PackageID and LCASE(p.DataEntry_Type)='cde' 
       inner join tbl_requests r on r.RequestID = rp.RequestID and r.Is_Active = 1
       where rp.RequestID = " + requestid + "";
            string sql2 = @"select 1 as isAuto from tbl_packages p inner
                                         join tbl_request_packages rp on
     p.PackageID = rp.PackageID and LCASE(p.DataEntry_Type) = 'auto'
    
                                    inner join tbl_requests r on r.RequestID = rp.RequestID and r.Is_Active = 1
    
                                where rp.RequestID = " + requestid + "";
            if (getIsExist(sql) > 0)
            {
                execcute_usp_insert_case_history(requestid, "TL-0003", "0", DateTime.Now.ToString(), "Task Awaiting for CDE");
            }
            else if (getIsExist(sql2) > 0)
            {
                execcute_usp_insert_case_history(requestid, "TL-0010", "0", DateTime.Now.ToString(), "Task Allocated by Robo");
            }
            else
            {
                execcute_usp_insert_case_history(requestid, "TL-0004", "0", DateTime.Now.ToString(), "Task Allocated by Robo");
            }

            return new DataTable();
        }

        private void execcute_usp_insert_case_history(string p_requestid, string p_state_masterid, string p_userid, string p_date, string p_message)
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            int in_Sequence = 0;
            string sql = "select 1 from tbl_request_state_instance where RequestID=" + p_requestid + " and Is_Current=1 limit 1";

            if (getIsExist(sql) > 0)
            {
                sql = " select Sequence from tbl_request_state_instance where RequestID=" + p_requestid + " and Is_Current=1 order by Date_Created desc limit 1;";
                string seq = entity.Database.SqlQuery<string>(sql).FirstOrDefault();
                in_Sequence = Convert.ToInt32(seq) + 1;
            }
            else
            {
                in_Sequence = 1;
            }

            sql = @"update tbl_request_state_instance set `Is_Current`=0 where `requestid`=@p_requestid;
     insert into tbl_request_state_instance (`RequestID`,`Sequence`,`StateID`,`UserID`,`Date_Created`,`Comments`,`Is_Current`,`IS_Review`,`PartitionKey`) 
     values (@p_requestid,@in_Sequence,@p_state_masterid,@p_userid,Now(),@p_message,1,0,0); ";
            MySqlParameter[] queryParams = new MySqlParameter[] {
                                        new MySqlParameter("p_requestid", p_requestid),
                                        new MySqlParameter("in_Sequence", in_Sequence),
                                        new MySqlParameter("p_state_masterid", p_state_masterid),
                                        new MySqlParameter("p_userid", p_userid),
                                        new MySqlParameter("p_date", p_date),
                                        new MySqlParameter("p_message", p_message)

            };
            var sequenceQueryResult = entity.Database.ExecuteSqlCommand(sql, queryParams);

        }

        public int getIsExist(string sql)
        {
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            DataTable retVal = new DataTable();
            //string sql = "select count(*) from tbl_document_data where RequestID=" + rquestID + ";";
            //retVal = entity.Database.SqlQuery<DataTable>(sql).FirstOrDefault();
            var sequenceQueryResult = entity.Database.SqlQuery<string>(sql).ToList();
            return (sequenceQueryResult.Count == 0) ? 0 : Convert.ToInt32(sequenceQueryResult[0]);
        }
        public string getPackageID(string package, string SBUID)
        {
            DML_Utility objDML = new DML_Utility();
            try
            {
                fadv_touchlessEntities entity = new fadv_touchlessEntities();
                DataTable retVal = new DataTable();
                string sql = "select PackageID from tbl_packages where Package_name='" + package + "' and SBUID=" + SBUID + ";";
                retVal = entity.Database.SqlQuery<DataTable>(sql).FirstOrDefault();
                var sequenceQueryResult = entity.Database.SqlQuery<string>(sql).ToList();
                return sequenceQueryResult[0];
            }
            catch (Exception ex)
            {
                objDML.Add_Exception_Log("Wipro exception : " + "Package is not found in CSPI Express for" + package + "And" + SBUID, ex.Message);
                throw;
            }
        }
    }
    public class tbl_Wipro_Details
    {
        public string Candidte_Id { get; set; }
        public string Employee_No { get; set; }
        public string Employee_Name { get; set; }
        public string Posting_Location { get; set; }
        public string Date_Of_Joining { get; set; }
        public string Fathers_Name { get; set; }
        public string Date_Of_Birth { get; set; }
        public string Employment_Type { get; set; }
        public string Check_Status { get; set; }
        public string Mapping_Date { get; set; }
        public string Check_Initiated { get; set; }
        public string POB_Location { get; set; }
    }
    public class tbl_wipro_campus_data
    {

        public string ResumeID { get; set; }
        public string EmpID { get; set; }
        public string EmpName { get; set; }
        //public string DateOfJoining { get; set; }
        //public string FatherName { get; set; }
        //public string DateofBirth { get; set; }
        //public string ContactNumber { get; set; }
        //public string EmailId { get; set; }
        //public string Country { get; set; }
        //public string Geo { get; set; }
        //public string Qualification { get; set; }
        //public string UniversityName { get; set; }
        //public string CollegeName { get; set; }
        //public string ModeOfEducation { get; set; }
        //public string YearOfPassing { get; set; }
        //public string AccountName { get; set; }
        //public string AgencyNumber { get; set; }
        //public string BGVInitiatedDate { get; set; }
        //public string BGVInsuffRaisedDate { get; set; }
        //public string InsuffComponent { get; set; }
        //public string InsuffRaisedRemarks { get; set; }
        //public string InsuffClearedRemarks { get; set; }
        //public string BGVInsuffClearedDate { get; set; }
        //public string BGVAssignedDate { get; set; }
        //public string BGVQCRejectedDate { get; set; }
        //public string BGVQCRejectedRemarks { get; set; }
        //public string BGVSPOC { get; set; }
        //public string AG_REPORTED_STATUS_BGV { get; set; }
        //public string AG_REPORTED_DATE_BGV { get; set; }
        //public string SLA { get; set; }
        //public string AgencytoreverifyDate { get; set; }
        //public string Agencytoreverifyremarks { get; set; }
        //public string AgencyPlannedClosureDate { get; set; }
        //public string PriorityFlag { get; set; }
        //public string Division { get; set; }
        //public string BGVStatus { get; set; }
        //public string ActiveFlag { get; set; }
        //public string BGV_Final_status { get; set; }
    }

    public class tbl_wipro_gsh_data
    {
        public string ResumeNumber { get; set; }
        public string CandidateName { get; set; }
    }
    public class tbl_wipro_internal_data
    {
        public string EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
    }
    public class tbl_wipro_lateral_data
    {
        public string ResumeNumber { get; set; }
        public string CandidateName { get; set; }
    }
    public class tbl_wipro_contract_data
    {
        public string ResumeNumber { get; set; }
        public string CandidateName { get; set; }
    }
    public class tbl_cap_gemini
    {
        public string Candidate_ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Entry_Date { get; set; }
    }

}
