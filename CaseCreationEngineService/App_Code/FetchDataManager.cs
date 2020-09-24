using Common;
using Common.DBConstants;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CaseCreationEngineService.App_Code
{
    class FetchDataManager : IDisposable
    {
        public string getData()
        {
            try
             {
                AppLog.Debug("Service started");
                string username = Convert.ToString(ConfigurationManager.AppSettings[Constants.Token.username]);
                string password = Convert.ToString(ConfigurationManager.AppSettings[Constants.Token.password]);

                CSPiHelper helper = new CSPiHelper(username, password);
                AppLog.Debug("Step 0");
                var dt = getDatacaseCreation(helper);
                AppLog.Debug("Step 1");
                return null;
            }
            catch (Exception ex)
            {
                AppLog.Debug("Error:" + ex.StackTrace);
                return null;
            }

        }
        public void Dispose()
        {
            //TO-DO Free managed and unmanaged resources
            getData();
        }
        public OracleParameter CreateCursor(string name, ParameterDirection direction = ParameterDirection.Output)
        {
            return new OracleParameter(name, OracleDbType.RefCursor, direction);
        }
        string Json_Format = "", JsonCaseCreationData = "";
        string country = "", EmailId = "", stateId = "", generatedRequestId = "";
        List<string> ClentSpecific = new List<string>();
        List<string> document = new List<string>();
        public DataTable getDatacaseCreation(CSPiHelper Helper)
        {

            string connectionStr = ConfigurationManager.ConnectionStrings["CSPiConnection"].ConnectionString;
            string fadvEntityId = Convert.ToString(ConfigurationManager.AppSettings["Fadv_EntityId"]);
            AppLog.Debug("Step 2");
            using (OracleConnection con = new OracleConnection())
            {
                con.ConnectionString = connectionStr;
                con.Open();
               AppLog.Debug("Step 3");
                Console.WriteLine("Connected to Oracle Database {0}", con.ServerVersion);
                using (OracleCommand cmd = new OracleCommand())
                {
                    AppLog.Debug("Step 4");
                    string dateNow = DateTime.Today.ToShortDateString();
                    //UAT date time
                    string b = Convert.ToDateTime(dateNow).ToString("MM/dd/yyyy");
                    dateNow = b.Replace("-", "/");

                    string dateTommorow = DateTime.Today.AddDays(1).ToShortDateString();
                    //production date time
                    string c = Convert.ToDateTime(dateTommorow).ToString("MM/dd/yyyy");
                    dateTommorow = c.Replace("-", "/");

                    cmd.Connection = con;
                    cmd.CommandText = "pkg_cspi_rpt_magik_data_apac.fn_get_clienthr_cases_data";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(CreateCursor("return_cursor", ParameterDirection.ReturnValue));
                    cmd.Parameters.Add("pi_entity_id", fadvEntityId);
                    cmd.Parameters.Add("pi_Start_date", dateNow);
                    cmd.Parameters.Add("pi_End_date", dateTommorow);
                    AppLog.Debug("Today :" + dateNow + "Tommorow :" + dateTommorow);
                    cmd.Parameters.Add(CreateCursor("po_case_clienthr_cases_data"));
                    cmd.Parameters.Add(CreateCursor("po_client_field_data"));
                    DataSet ds = new DataSet();
                    AppLog.Debug("Step 5");
                    OracleDataAdapter MyDa = new OracleDataAdapter(cmd);
                    MyDa.Fill(ds);
                   AppLog.Debug("Data fetch sucess from Oracle");
                    CSPiModelRequest obj = new CSPiModelRequest();
                    con.Close();
                    var countT = 0;
                    foreach (DataRow dr1 in ds.Tables[1].Rows)
                    {
                        countT++;
                        Json_Format = "";
                        string CRN = Regex.Replace(dr1["CASE_REF_NUMBER"].ToString(), @"[^\u0000-\u007F]+", string.Empty);
                    // if (CRN != "R233-6112490-WEST-2019") continue;
                        AppLog.Debug("Working On CRNNumber :" + CRN);
                        //   DataTable crnNO = CRNDetails(Regex.Replace(dr1["CASE_REF_NUMBER"].ToString(), @"[^\u0000-\u007F]+", string.Empty));
                            DBParameters param = new DBParameters(TBL_Requests.TableName);
                            param.AddParameter(TBL_Requests.CRN_Number, CRN);
                            List<DBTBL_Requests> TaskDestiny = Helper.DBRead_TBL_Requests(param, false);

                            string birthDateStr = "";
                            string is_dob = "0";
                            string crnCreatedDate = null;
                            if (TaskDestiny.Count == 0)
                            {
                                
                                try
                                {
                                    country = Regex.Replace(dr1["COUNTRY_NAME"].ToString(), @"[^\u0000-\u007F]+", string.Empty);
                                    EmailId = Regex.Replace(dr1["EMAIL_ADDRESS"].ToString(), @"[^\u0000-\u007F]+", string.Empty);
                                    DateTime birthDate = DateTime.MinValue;
                                    DateTime CreatedCse = DateTime.MinValue;
                                    is_dob = "0";


                                    if (!string.IsNullOrEmpty(Regex.Replace(dr1["BIRTH_DT"].ToString(), @"[^\u0000-\u007F]+", string.Empty)))
                                    {
                                       // birthDate = DateTime.ParseExact(form.dateOfBirth, "dd/MM/yyyy", null);
                                        string[] s2 = (Regex.Replace(dr1["BIRTH_DT"].ToString(), @"[^\u0000-\u007F]+", string.Empty)).Split(' ');
                                        string dateOfBirth = s2[0];
                                       birthDate = DateTime.Parse(dateOfBirth);
                                        // birthDate = DateTime.ParseExact(dateOfBirth,"dd/MM/yyyy", null);
                                      
                                         is_dob = "1";
                                      
                                    }

                                    if (!string.IsNullOrEmpty(Regex.Replace(dr1["Case Date"].ToString(), @"[^\u0000-\u007F]+", string.Empty)))
                                    {
                                        string[] s1 = (Regex.Replace(dr1["Case Date"].ToString(), @"[^\u0000-\u007F]+", string.Empty)).Split(' ');
                                        string caseDate = s1[0];
                                        // crnCreatedDate = (DateTime.ParseExact(CreatedCseDate, "dd/mm/yyyy", null)).ToString();
                                        // String result = String.Join("-", CreatedCseDate.Split('/').Reverse());
                                        //crnCreatedDate = result+" "+s1[1];
                                        CreatedCse = DateTime.Parse(caseDate);
                                        crnCreatedDate = CreatedCse.ToString("yyyy-MM-dd");

                                    }

                                    birthDateStr = birthDate.ToString("yyyy-MM-dd");

                                }
                                catch (Exception ex)
                                {

                                }
                                try
                                {
                                    stateId = "REQ-0002";
                                    obj.OperationName = "usp_save_request_data_engine";
                                    obj.OperationType = "procedure";
                                    //Json_Format = getJsonclientSpecificFields(country, EmailId);
                                    DataTable ClientCode = ClientDetails(Regex.Replace(dr1["CLIENT_CODE"].ToString(), @"[^\u0000-\u007F]+", string.Empty));

                                    if (ClientCode.Rows.Count > 0)
                                    {
                                        DataTable SBUID = SBUDetails((Regex.Replace(dr1["SBU Name"].ToString(), @"[^\u0000-\u007F]+", string.Empty)), ClientCode.Rows[0]["ClientID"].ToString());
                                        if (SBUID.Rows.Count > 0)
                                        {
                                            DataTable PackageID = PackageDetails(Regex.Replace(dr1["PACKAGE_NAME"].ToString(), @"[^\u0000-\u007F]+", string.Empty), SBUID.Rows[0]["SBUID"].ToString(), ClientCode.Rows[0]["ClientID"].ToString());

                                            if (PackageID.Rows.Count > 0 && SBUID.Rows.Count > 0 && ClientCode.Rows.Count > 0)
                                            {
                                               // DataTable PackageIDList = PackckageDetailsNew(PackageID.Rows[0]["PackageID"].ToString());
                                               // if (PackageIDList.Rows.Count > 0)
                                              //  {
                                                    
                                                    Json_Format += "{";
                                                    Json_Format += "\"requestoremail\":{\"request_field_id\": \"131\",\"data\":\"" + Regex.Replace(dr1["EMAIL_ADDRESS"].ToString(), @"[^\u0000-\u007F]+", string.Empty) + "\",\"text\": \"" + Regex.Replace(dr1["EMAIL_ADDRESS"].ToString(), @"[^\u0000-\u007F]+", string.Empty) + "\"},";
                                                    foreach (DataRow dr2 in ds.Tables[2].Rows)
                                                    {
                                                        string CSPCCase_ID = Regex.Replace(dr2["CASE_ID"].ToString(), @"[^\u0000-\u007F]+", string.Empty);
                                                        string Case_ID = Regex.Replace(dr1["CASE_ID"].ToString(), @"[^\u0000-\u007F]+", string.Empty);
                                                        string Clent_Field = Regex.Replace(dr2["Client_Field"].ToString(), @"[^\u0000-\u007F]+", string.Empty);
                                                        string ANSWERS = Regex.Replace(dr2["ANSWERS"].ToString(), @"[^\u0000-\u007F]+", string.Empty);
                                                        if (Case_ID == CSPCCase_ID)
                                                        {
                                                            DataTable CLIENTSpeccific = PackageDetailsNew(Clent_Field, ClientCode.Rows[0]["ClientID"].ToString());
                                                            if (Clent_Field != "requestoremail")
                                                            {
                                                                if (CLIENTSpeccific.Rows.Count > 0)
                                                                {
                                                                    Json_Format += "\"" + CLIENTSpeccific.Rows[0]["field_json_tag"].ToString() + "\": {\"request_field_id\": \"" + CLIENTSpeccific.Rows[0]["request_field_id"].ToString() + "\",\"data\":\"" + ANSWERS + "\",\"text\": \"" + ANSWERS + "\"},";
                                                                }
                                                            }
                                                        }
                                                    }
                                                    // }
                                                    Json_Format = Json_Format.TrimEnd(',');
                                                    Json_Format += "}";

                                                    //Json_Format = getJsonclientSpecificFields(country, EmailId,ClientCode.Rows[0]["ClientID"].ToString());
                                                    AppLog.Debug("Case Strated for CRN :" + CRN);
                                                    obj.ParameterList = new List<OperationParameter>();
                                                    obj.ParameterList.Add(new OperationParameter("@p_request_id", "0"));
                                                    obj.ParameterList.Add(new OperationParameter("@p_client_id", ClientCode.Rows[0]["ClientID"].ToString()));
                                                    obj.ParameterList.Add(new OperationParameter("@p_crn_no", Regex.Replace(dr1["CASE_REF_NUMBER"].ToString(), @"[^\u0000-\u007F]+", string.Empty)));
                                                    obj.ParameterList.Add(new OperationParameter("@p_crn_created_date", crnCreatedDate));

                                                    obj.ParameterList.Add(new OperationParameter("@p_first_name", Regex.Replace(dr1["FIRST_NAME"].ToString(), @"[^\u0000-\u007F]+", string.Empty)));
                                                    obj.ParameterList.Add(new OperationParameter("@p_middle_name", Regex.Replace(dr1["MIDDLE_NAME"].ToString(), @"[^\u0000-\u007F]+", string.Empty)));
                                                    obj.ParameterList.Add(new OperationParameter("@p_last_name", Regex.Replace(dr1["LAST_NAME"].ToString(), @"[^\u0000-\u007F]+", string.Empty)));
                                                    obj.ParameterList.Add(new OperationParameter("@p_client_ref_no", Regex.Replace(dr1["CLIENT_REFERNCE_NO"].ToString(), @"[^\u0000-\u007F]+", string.Empty)));
                                                    obj.ParameterList.Add(new OperationParameter("@p_subject_detail", Regex.Replace(dr1["SUBJECT_DETAIL"].ToString(), @"[^\u0000-\u007F]+", string.Empty)));
                                                    obj.ParameterList.Add(new OperationParameter("@p_subject_type", Regex.Replace(dr1["SUBJECT_TYPE"].ToString(), @"[^\u0000-\u007F]+", string.Empty)));
                                                    obj.ParameterList.Add(new OperationParameter("@p_is_date_of_birth", is_dob));
                                                    obj.ParameterList.Add(new OperationParameter("@p_date_of_birth", birthDateStr));
                                                    obj.ParameterList.Add(new OperationParameter("@p_type_of_check", Regex.Replace(dr1["TYPE_OF_CHECK"].ToString(), @"[^\u0000-\u007F]+", string.Empty)));
                                                    obj.ParameterList.Add(new OperationParameter("@p_candidateauthorizationletter", Regex.Replace(dr1["CANDIDATE_AUTHORIZATION_LETTER"].ToString(), @"[^\u0000-\u007F]+", string.Empty)));
                                                    obj.ParameterList.Add(new OperationParameter("@p_package_type", "Soft Copy"));
                                                    obj.ParameterList.Add(new OperationParameter("@p_srt_data", "SRT"));
                                                    obj.ParameterList.Add(new OperationParameter("@p_sbu_entitiesid", SBUID.Rows[0]["SBUID"].ToString()));
                                                    obj.ParameterList.Add(new OperationParameter("@p_loa_submitted", "Yes"));

                                                    obj.ParameterList.Add(new OperationParameter("@p_bvf_submitted", "Yes"));

                                                    obj.ParameterList.Add(new OperationParameter("@p_json_data", Json_Format));
                                                    obj.ParameterList.Add(new OperationParameter("@p_userid", "165"));

                                                    obj.ParameterList.Add(new OperationParameter("@p_comment", "Imported from CSPI"));
                                                    obj.ParameterList.Add(new OperationParameter("@p_state_id", stateId));
                                                    // obj.ParameterList.Add(new OperationParameter("@p_out_request_id", "", a)); s
                                                    obj.ParameterList.Add(new OperationParameter("@p_out_request_id", "p_out_request_id"));

                                                    CSPiHelper ctr = new CSPiHelper(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);
                                                    DataTable retv = ctr.ExecuteStoredProcedure(obj).Result;
                                                    DataRow dr = retv.Rows[0];
                                                    try
                                                    {

                                                        String columnValue = dr[0].ToString();
                                                        if (retv.Rows.Count > 0)
                                                        {
                                                            // dr = retv.Rows[1];
                                                            // generatedRequestId = dr[0].ToString();
                                                            generatedRequestId = columnValue;
                                                            retv.Dispose();
                                                            AppLog.Debug("Case creted" + CRN + "RequestId" + generatedRequestId);
                                                            //DELETEING EXISTING PACKAGE FROM CASE
                                                            obj.OperationName = "DELETE FROM tbl_request_packages WHERE RequestID=@p_request_id";
                                                            obj.OperationType = "query";
                                                            obj.ParameterList = new List<OperationParameter>();
                                                            obj.ParameterList.Add(new OperationParameter("@p_request_id", generatedRequestId));

                                                            DataTable retv1 = ctr.BotTaskExecuteQuery(obj).Result;

                                                            retv.Dispose();
                                                            //  END - DELETEING EXISTING PACKAGE FROM CASE

                                                            // ADDING NEWLY SELECTED PACKAGE INTO CASE
                                                            obj.OperationName = "INSERT INTO tbl_request_packages (`RequestID`,`PackageID`) VALUES (@p_request_id,@p_package_id)";
                                                            obj.OperationType = "query";
                                                            obj.ParameterList = new List<OperationParameter>();
                                                            obj.ParameterList.Add(new OperationParameter("@p_request_id", generatedRequestId));
                                                            obj.ParameterList.Add(new OperationParameter("@p_package_id", PackageID.Rows[0]["PackageID"].ToString()));
                                                            DataTable retv2 = ctr.BotTaskExecuteQuery(obj).Result;
                                                            //  END - ADDING NEWLY SELECTED PACKAGE INTO CASE
                                                            updateStatus(generatedRequestId);

                                                            JsonCaseCreationData = "{\"Header\": {\"Type\": \"Request\",\"ServiceID\": \"CaseCreation\",\"MessageId\": \"" + Guid.NewGuid().ToString() + "\",\"MessageDate\": \"" + DateTime.Now.ToShortDateString() + "\",\"RequestID\": \"" + generatedRequestId + "\",";
                                                            JsonCaseCreationData += "\"Module\": \"" + "CaseCreation" + "\",\"RetrySequecne\": \"0\",\"BOTID\": \"\",\"CheckPoint\": \"\"},\"Data\":{\"CaseCreation\":{\"" + "requestId" + "\":  \"" + generatedRequestId + "\", \"" + "clientId" + "\":  \"" + ClientCode.Rows[0]["ClientID"].ToString() + "\",";
                                                            JsonCaseCreationData += "\"" + "crnNo" + "\":  \"" + Regex.Replace(dr1["CASE_REF_NUMBER"].ToString(), @"[^\u0000-\u007F]+", string.Empty) + "\",";
                                                            JsonCaseCreationData += "\"" + "crnCreatedDate" + "\":  \"" + Regex.Replace(dr1["Case Date"].ToString(), @"[^\u0000-\u007F]+", string.Empty) + "\",";
                                                            JsonCaseCreationData += "\"" + "firstName" + "\":  \"" + Regex.Replace(dr1["FIRST_NAME"].ToString(), @"[^\u0000-\u007F]+", string.Empty) + "\",\"" + "middleName" + "\":  \"" + Regex.Replace(dr1["MIDDLE_NAME"].ToString(), @"[^\u0000-\u007F]+", string.Empty) + "\",";
                                                            JsonCaseCreationData += "\"" + "lastName" + "\":  \"" + Regex.Replace(dr1["LAST_NAME"].ToString(), @"[^\u0000-\u007F]+", string.Empty) + "\",\"" + "dateOfBirth" + "\":  \"" + birthDateStr + "\",";
                                                            JsonCaseCreationData += "\"" + "sbuEntitiesId" + "\":  \"" + SBUID.Rows[0]["SBUID"].ToString() + "\",\"" + "clientReferenceNo" + "\":  \"" + Regex.Replace(dr1["CASE_REF_NUMBER"].ToString(), @"[^\u0000-\u007F]+", string.Empty) + "\",\"" + "subjectDetails" + "\":  \"" + Regex.Replace(dr1["SUBJECT_DETAIL"].ToString(), @"[^\u0000-\u007F]+", string.Empty) + "\",";
                                                            JsonCaseCreationData += "\"" + "subjectType" + "\":  \"" + Regex.Replace(dr1["SUBJECT_TYPE"].ToString(), @"[^\u0000-\u007F]+", string.Empty) + "\",\"" + "typeOfCheck" + "\":  \"" + Regex.Replace(dr1["TYPE_OF_CHECK"].ToString(), @"[^\u0000-\u007F]+", string.Empty) + "\",";
                                                            JsonCaseCreationData += "\"" + "authorizationLetter" + "\":  \"" + Regex.Replace(dr1["CANDIDATE_AUTHORIZATION_LETTER"].ToString(), @"[^\u0000-\u007F]+", string.Empty) + "\",\"" + "packageType" + "\":  \"" + "Soft Copy" + "\",";
                                                            JsonCaseCreationData += "\"" + "srtData" + "\":  \"" + "SRT" + "\",\"" + "lOASubmited" + "\":  \"" + "Yes" + "\",\"" + "bVFSubmitted" + "\":  \"" + "Yes" + "\",";
                                                            JsonCaseCreationData += "\"" + "isClientSpecificField" + "\":  \"" + "true" + "\",\"" + "clientSpecificFields" + "\":  " + Json_Format + ",";
                                                            JsonCaseCreationData += "\"" + "comment" + "\":  \"" + null + "\",\"" + "request_status" + "\":  \"" + "Complete" + "\",";
                                                            JsonCaseCreationData += "\"" + "packageList" + "\":[  \"" + PackageID.Rows[0]["PackageID"].ToString() + "\"],\"" + "clientName" + "\":  \"" + Regex.Replace(dr1["Client Name"].ToString(), @"[^\u0000-\u007F]+", string.Empty) + "\",";
                                                            JsonCaseCreationData += "\"" + "sbuEntitiesName" + "\":  \"" + Regex.Replace(dr1["SBU Name"].ToString(), @"[^\u0000-\u007F]+", string.Empty) + "\",\"" + "packageName" + "\":  \"" + Regex.Replace(dr1["PACKAGE_NAME"].ToString(), @"[^\u0000-\u007F]+", string.Empty) + "\"";
                                                            JsonCaseCreationData += "}},\"" + "Status" + "\":{  \"" + "Value" + "\":  \"" + "" + "\",";
                                                            JsonCaseCreationData += "\"" + "Description" + "\":  \"" + "" + "\",\"" + "Details" + "\":  \"" + null + "\"}}";
                                                            //SaveJsonData(generatedRequestId, JsonCaseCreationData, "CaseCreation", "1");
                                                            AppLog.Debug("Case Success CRNNumber :" + CRN);

                                                    //For document fetch 
                                                    DataTable CheckPackegeCDEMDE = CheckPackegeCDEOrMDE(generatedRequestId);
                                                    if (CheckPackegeCDEMDE.Rows.Count == 0)
                                                    {
                                                        AppLog.Debug("MDE Case :" + CRN);
                                                        DataTable dtDocument = ds.Tables[0];
                                                        foreach (DataRow objDataRow in dtDocument.Rows)
                                                        {
                                                            string CSPCCase_ID = Regex.Replace(objDataRow["CASE_ID"].ToString(), @"[^\u0000-\u007F]+", string.Empty);
                                                            string Case_ID = Regex.Replace(dr1["CASE_ID"].ToString(), @"[^\u0000-\u007F]+", string.Empty);
                                                            string documentPath = objDataRow["DOCUMENT_PATH"].ToString();
                                                            if (CSPCCase_ID == Case_ID)
                                                            {
                                                                document.Add(Regex.Replace(objDataRow["DOCUMENT_PATH"].ToString(), @"[^\u0000-\u007F]+", string.Empty));
                                                            }
                                                        }
                                                        AppLog.Debug("Document Upload started :" + CRN);
                                                        Savedocument(generatedRequestId);
                                                    }
                                                }  
                                                }
                                                catch (Exception ex)
                                                {

                                                }
                                              }

                                            }
                                            else
                                            {
                                                AppLog.Debug("Not Match CRNNumber :" + CRN);
                                            }
                                        }
                                    
                                }
                                catch(Exception ex)
                                { }
                            }
                      //  }

                    }
                    return null;

                }
            }
        }
        public string Savedocument(string generatedRequestId)
        {

            string sql = "";
            string destinationLocation = Convert.ToString(ConfigurationManager.AppSettings["CaseDocumentLocation"]);
            string cdeDocLocation = Convert.ToString(ConfigurationManager.AppSettings["CDEDocumentLocation"]);
            AppLog.Debug("doc save at " + destinationLocation + DateTime.Now);
            bool bdocumentuploaded = false;
            destinationLocation = destinationLocation + "Documents";
            if (!Directory.Exists(destinationLocation))
            {
                Directory.CreateDirectory(destinationLocation);
            }
            destinationLocation = destinationLocation + "\\NewCase";
            if (!Directory.Exists(destinationLocation))
            {
                Directory.CreateDirectory(destinationLocation);
            }
            destinationLocation = destinationLocation + "\\" + generatedRequestId + "\\";
            if (!Directory.Exists(destinationLocation))
            {
                Directory.CreateDirectory(destinationLocation);
            }
            foreach (String documentPath in document)
            {

                string destFile = "";
                try
                {
                    AppLog.Debug("Document Processing");
                    // string documentPath1 = @"Backup\doc\cde_file\2.pdf";
                    string fileName = System.IO.Path.GetFileName(cdeDocLocation + documentPath);
                    destFile = System.IO.Path.Combine(destinationLocation, fileName);
                    string repVal = @"\";
                    string currectDoCPath = documentPath.Replace("/", repVal);
                    AppLog.Debug("destFile & fileName $ currectDoCPath" + destFile + " " + fileName + "" + cdeDocLocation + currectDoCPath);
                    System.IO.File.Copy(cdeDocLocation + currectDoCPath, destFile, true);
                    AppLog.Debug("data " + cdeDocLocation + " " + currectDoCPath + " " + destinationLocation);
                }
                catch (Exception ex)
                {
                  
                    continue;
                }
               
                bdocumentuploaded = true;
                UploadDocument(destFile, generatedRequestId);

            }
            document.Clear();
            return null;
        }
        public DataTable CheckPackegeCDEOrMDE(string generatedRequestId)
        {

                CSPiModelRequest obj = new CSPiModelRequest();
                obj.OperationName = "sp_UpdateRequestStatus";
                List<OperationParameter> oprList = new List<OperationParameter>();
                oprList.Add(new OperationParameter("@reqid", generatedRequestId));

                obj.ParameterList = new List<OperationParameter>();
                obj.ParameterList = oprList;
                CSPiHelper ctrl = new CSPiHelper(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);
                DataTable dtResponse = ctrl.BotTaskExecuteQuery(obj).Result;
                return dtResponse;

        }
        public bool UploadDocument(string document_path, string request_id)
        {
            
            bool bSuccess = false;
            try
            {
                CSPiModelRequest obj = new CSPiModelRequest();
                obj.OperationName = "usp_upload_document";
                obj.OperationType = "procedure";
                obj.ParameterList = new List<OperationParameter>();
                obj.ParameterList.Add(new OperationParameter("@p_document_path", document_path));
                obj.ParameterList.Add(new OperationParameter("@p_request_id", request_id));
  
                CSPiHelper ctrl = new CSPiHelper(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);
                DataTable dtResponse = ctrl.BotTaskExecuteQuery(obj).Result;

                bSuccess = true;
                AppLog.Debug("Document Saved rquestID"+ request_id);
                

            }

            catch (Exception ex)
            {
                bSuccess = false;
              
            }
            return bSuccess;
        }

        public string SaveJsonData(string RequestId, string JasonData, string Type, string Status)
        {

            CSPiModelRequest obj = new CSPiModelRequest();

            obj.OperationName = "usp_request_tracker";
            obj.OperationType = "procedure";
            obj.ParameterList = new List<OperationParameter>();
            obj.ParameterList.Add(new OperationParameter("@p_requestid", RequestId));
            obj.ParameterList.Add(new OperationParameter("@p_jasondata", JasonData));
            obj.ParameterList.Add(new OperationParameter("@p_type", Type));
            obj.ParameterList.Add(new OperationParameter("@p_status", Status));
            CSPiHelper ctrl = new CSPiHelper(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);
            DataTable dtResponse = ctrl.BotTaskExecuteQuery(obj).Result;
            return null;

        }

        //public string getJsonclientSpecificFields(string Country, string emailID, string ClientID)
        //{
        //    DataTable PackageID = PackageDetails(ClientID);

        //    //if()
        //    Json_Format = "{\"Country\": {\"request_field_id\": \"131\",\"data\":\"" + country + "\",\"text\": \"" + country + "\"";
        //    Json_Format += "},\"requestoremail\":{\"request_field_id\": \"131\",\"data\":\"" + EmailId + "\",\"text\": \"" + EmailId + "\"";
        //    Json_Format += "}}";
        //    return Json_Format;
        //}
        public DataTable PackageDetailsNew(string SBUName, string clientID)
        {
            CSPiModelRequest obj = new CSPiModelRequest();
            obj.OperationName = "usp_get_client_request_additional_fields_CseCreationNew2";
            List<OperationParameter> oprList = new List<OperationParameter>();
            oprList.Add(new OperationParameter("@P_SBUName", SBUName));
            oprList.Add(new OperationParameter("@in_cspi_clientcode", clientID));
            obj.ParameterList = new List<OperationParameter>();
            obj.ParameterList = oprList;
            CSPiHelper ctrl = new CSPiHelper(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);
            DataTable dtResponse = ctrl.BotTaskExecuteQuery(obj).Result;
            return dtResponse;
        }


        public DataTable PackageDetails(string clientId,string Clent_Field)
        {

            CSPiModelRequest obj = new CSPiModelRequest();
            obj.OperationName = "usp_get_client_request_additional_fields_CseCreationNew1";
            List<OperationParameter> oprList = new List<OperationParameter>();
            obj.ParameterList.Add(new OperationParameter("@pRequestId", clientId));
            obj.ParameterList.Add(new OperationParameter("@pCRN_Number", Clent_Field));
            obj.ParameterList = new List<OperationParameter>();
            obj.ParameterList = oprList;
            CSPiHelper ctrl = new CSPiHelper(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);
            DataTable dtResponse = ctrl.BotTaskExecuteQuery(obj).Result;
            return dtResponse;

        }
        public void updateStatus(string requestId)
        {
            try
            {

                CSPiModelRequest obj = new CSPiModelRequest();
                obj.OperationName = "sp_UpdateRequestStatus";
                List<OperationParameter> oprList = new List<OperationParameter>();
                oprList.Add(new OperationParameter("@reqid", requestId));

                obj.ParameterList = new List<OperationParameter>();
                obj.ParameterList = oprList;
                CSPiHelper ctrl = new CSPiHelper(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);
                DataTable dtResponse = ctrl.BotTaskExecuteQuery(obj).Result;
            }
            catch (Exception ex)
            {
            }
        }

        public DataTable PackageDetails(string PackageName, string SUBID, string clientID)  
        {

            CSPiModelRequest obj = new CSPiModelRequest();
            obj.OperationName = "usp_PackageDetailsData";
            List<OperationParameter> oprList = new List<OperationParameter>();
            oprList.Add(new OperationParameter("@P_Package_NAME", PackageName));
            oprList.Add(new OperationParameter("@in_SBUID", SUBID));
            oprList.Add(new OperationParameter("@in_cspi_clientcode", clientID));

            obj.ParameterList = new List<OperationParameter>();
            obj.ParameterList = oprList;
            CSPiHelper ctrl = new CSPiHelper(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);
            DataTable dtResponse = ctrl.BotTaskExecuteQuery(obj).Result;
            return dtResponse;

        }

        public DataTable SBUDetails(string SBUName, string clientID)
        {
            CSPiModelRequest obj = new CSPiModelRequest();
            obj.OperationName = "usp_SBUDetails";
            List<OperationParameter> oprList = new List<OperationParameter>();
            oprList.Add(new OperationParameter("@P_SBUName", SBUName));
            oprList.Add(new OperationParameter("@in_cspi_clientcode", clientID));
            obj.ParameterList = new List<OperationParameter>();
            obj.ParameterList = oprList;
            CSPiHelper ctrl = new CSPiHelper(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);
            DataTable dtResponse = ctrl.BotTaskExecuteQuery(obj).Result;
            return dtResponse;
        }
        public DataTable ClientDetails(string ClientCode)
        {
            CSPiModelRequest obj = new CSPiModelRequest();
            obj.OperationName = "usp_ClientDetails";
            List<OperationParameter> oprList = new List<OperationParameter>();
            oprList.Add(new OperationParameter("@P_ClientCode", ClientCode));

            obj.ParameterList = new List<OperationParameter>();
            obj.ParameterList = oprList;
            CSPiHelper ctrl = new CSPiHelper(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);
            DataTable dtResponse = ctrl.BotTaskExecuteQuery(obj).Result;
            return dtResponse;
        }
        public DataTable PackckageDetailsNew(string PackageId)  
        {
            CSPiModelRequest obj = new CSPiModelRequest();
            obj.OperationName = "usp_getPacageID";
            List<OperationParameter> oprList = new List<OperationParameter>();
            oprList.Add(new OperationParameter("@P_ClientCode", PackageId));

            obj.ParameterList = new List<OperationParameter>();
            obj.ParameterList = oprList;
            CSPiHelper ctrl = new CSPiHelper(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"]);
            DataTable dtResponse = ctrl.BotTaskExecuteQuery(obj).Result;
            return dtResponse;
        }

    }

}
