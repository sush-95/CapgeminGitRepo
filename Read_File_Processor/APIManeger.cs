using DataAccess_Utility;
using Newtonsoft.Json;
using Read_File_Processor;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Read_File_Processor
{
    public class APIManeger
    {
        string apiBaseUri = ConfigurationManager.AppSettings["ApiBaseUri"];
        string TokenID = ConfigurationManager.AppSettings["TokenID"];
        public DataTable GetSuspect()
        {
            string apiPath = "Suspect/GetSuspect";
            //SuspectMasterListResponse SuspectMasterListResponse = new SuspectMasterListResponse(); ;
            var result = "";
            DataTable dt = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("TokenID", TokenID);

                // client.DefaultRequestHeaders.Add("Authorization", "Bearer " + request.Token);

                var client1 = new HttpClient();
                var response = client.GetAsync(apiBaseUri + apiPath).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                    //SuspectMasterListResponse = (new JavaScriptSerializer()).Deserialize<SuspectMasterListResponse>(result);
                    //string output = JsonConvert.SerializeObject(SuspectMasterListResponse.SuspectList);
                    var dataSet = JsonConvert.DeserializeObject<DataSet>(result);
                    dt = dataSet.Tables[0];
                    //DataTable dt = (DataTable)JsonConvert.DeserializeObject(result, (typeof(DataTable)));
                }
            }
            return dt;
        }
        public async Task<string> PostSuspect1(PackageRequestModel request)
        {
            string apiPath = "";// "Suspect/PostSuspect";
            DML_Utility objDML = new DML_Utility();

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(apiBaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromMinutes(100);

                client.DefaultRequestHeaders.Add("TokenID", TokenID);
                // client.DefaultRequestHeaders.Add("Authorization", "Bearer " + request.Token);
                string json = JsonConvert.SerializeObject(request);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                //var client1 = new HttpClient();
                //client1.Timeout = TimeSpan.FromMinutes(100);
                var response = await client.PostAsync(apiBaseUri + apiPath, stringContent).ConfigureAwait(false);
                objDML.Add_Exception_Log(json, "");
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;

            }
        }

        public string PostSuspect(PackageRequestModel request)
        {
            DML_Utility objDML = new DML_Utility();
            var client1 = new RestSharp.RestClient(apiBaseUri);
            client1.Timeout =100000000;
            var request1 = new RestRequest(Method.POST);
            request1.AddHeader("TokenID", TokenID);
            request1.AddHeader("Accept", "*/*");
            request1.AddHeader("Content-Type", "application/json");
            string json = JsonConvert.SerializeObject(request);
            objDML.Add_Exception_Log(json, "Package Request");
            request1.AddParameter("undefined", json, RestSharp.ParameterType.RequestBody);
            IRestResponse response1 = client1.Execute(request1);
            var Outputresponse = response1.Content;
            objDML.Add_Exception_Log(Outputresponse, "Package Response");

            return Outputresponse;
        }
        public string PostWiproPackageRequest(WiproPackageRequestModel request)
        {
            DML_Utility objDML = new DML_Utility();
            var client1 = new RestSharp.RestClient(apiBaseUri);
            client1.Timeout = 100000000;
            var request1 = new RestRequest(Method.POST);
            request1.AddHeader("TokenID", TokenID);
            request1.AddHeader("Accept", "*/*");
            request1.AddHeader("Content-Type", "application/json");
            string json = JsonConvert.SerializeObject(request);
            objDML.Add_Exception_Log(json, "Package Request");
            request1.AddParameter("undefined", json, RestSharp.ParameterType.RequestBody);
            IRestResponse response1 = client1.Execute(request1);
            var Outputresponse = response1.Content;
            objDML.Add_Exception_Log(Outputresponse, "Package Response");

            return Outputresponse;
        }

    }
}
