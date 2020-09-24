using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Read_File_Processor
{
    public class PackageRequestModel
    {
        
        public metadata metadata { get; set; }

        private List<Data> _data;
        public List<Data> data
        {
            get
            {
                if (_data == null)
                {
                    _data = new List<Data>();
                }
                return _data;
            }
            set
            {
                _data = value;
            }
        }
    }
    public class Data
    {
        public string bgvtype { get; set; }
        //public string bgvsubtype { get; set; }
        public string account { get; set; }
        public string bu { get; set; }
        
        //public string previousemployment { get; set; }
        //public string accountgrouppreferencefirst { get; set; }
        //public string accountgrouppreferencesecond { get; set; }


    }
    public class metadata
    {
        public string engineName { get; set; }
        public string engineID { get; set; }
        public string engineLicenseId { get; set; }
        public string engineType { get; set; }
        public string requestId { get; set; }
        public string requestDate { get; set; }
        public string sourceApp { get; set; }
        public string sourceAppModule { get; set; }
        public string requestLabel { get; set; }
    }
}
