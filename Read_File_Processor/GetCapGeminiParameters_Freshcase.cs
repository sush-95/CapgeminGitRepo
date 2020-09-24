using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Read_File_Processor
{
    public class GetCapGeminiParameters_Freshcase
    {
        public Dictionary<string, string> GetCapgeminiParameter(string CandidateId, string firstName, string lastName)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("customerName", "Capgemini Technologies Services India Limited");
            dict.Add("candidateId", CandidateId);
            dict.Add("firstName", firstName);
            dict.Add("lastName", lastName);
            return dict;
        }
    }
}
