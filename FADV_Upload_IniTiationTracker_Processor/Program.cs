using Read_File_Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADV_Upload_IniTiationTracker_Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecuteUpload();
        }

        static void ExecuteUpload()
        {
            ExecuteProcess obj = new ExecuteProcess();
            int Value = obj.Execute_Excel_InitialTracker();

        }
    }
}
