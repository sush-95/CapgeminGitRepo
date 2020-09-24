using Newtonsoft.Json;
using Read_File_Processor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ws_download_process
{
    public class APIManeger
    {
        string apiBaseUri = ConfigurationManager.AppSettings["ApiBaseUri"];
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
        public async Task<string> PostSuspect(PackageRequestModel request)
        {
            string apiPath = "Suspect/PostSuspect";
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(apiBaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // client.DefaultRequestHeaders.Add("Authorization", "Bearer " + request.Token);
                string json = JsonConvert.SerializeObject(request);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                var client1 = new HttpClient();
                var response = await client.PostAsync(apiBaseUri + apiPath, stringContent).ConfigureAwait(false);

                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;

            }
        }


    }
}
