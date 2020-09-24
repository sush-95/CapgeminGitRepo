using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataAccess_Utility
{
    public class Get_Data_Utility
    {
        public List<tbl_input_request_data> Get_New_Request_Id_List1(string ImportKey)
        {
            List<tbl_input_request_data> lstObj = new List<tbl_input_request_data>();
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            //lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey=" + ImportKey + " and Request_ID not in (select request_id from tbl_initiation_tracker)").ToList<tbl_input_request_data>();
            lstObj = entity.tbl_input_request_data.SqlQuery("select * from tbl_input_request_data where ImportKey={0} and Request_ID not in (select request_id from tbl_initiation_tracker)", ImportKey).ToList<tbl_input_request_data>();

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
            string Query = "select id from tbl_request_details where messageid='" + strMessageId + "' and request_type = '" + RequestType + "'";
            //Query = "select * from tbl_request_details where messageid='" + strMessageId + "'";
            Query = "select * from tbl_request_details where messageid={0}";
            fadv_touchlessEntities entity = new fadv_touchlessEntities();
            try
            {
                lstObj = entity.tbl_request_details.SqlQuery(Query, strMessageId).ToList<tbl_request_details>();
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

        public List<tbl_request_details> Get_Last_Request()
        {
            List<tbl_request_details> lstObj = new List<tbl_request_details>();
            string Query = "select * from tbl_request_details where request_type='download' order by CreatedOn desc limit 1";
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


    }
}
