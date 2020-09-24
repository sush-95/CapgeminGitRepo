using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess_Utility;
using System.Configuration;
using MessageService;
using System.IO;

namespace Read_File_Processor
{
    public class FileUtility
    {
        public List<string> FileUpload(string SourcePath, string DestinationPath)
        {
            List<string> CopiedFiles = new List<string>();
            DML_Utility objDML = new DML_Utility();

            try
            {
                //Now Create all of the directories
                if (!Directory.Exists(DestinationPath))
                    Directory.CreateDirectory(DestinationPath);
                if (!Directory.Exists(DestinationPath + "\\RPA"))
                    Directory.CreateDirectory(DestinationPath + "\\RPA");
                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
                {
                    //System.Threading.Thread.Sleep(2000);
                    string fileName = newPath.Replace(SourcePath, DestinationPath);
                    File.Copy(newPath, fileName, true);
                    //System.Threading.Thread.Sleep(2000);
                    fileName = newPath.Replace(SourcePath, DestinationPath + "\\RPA");
                    File.Copy(newPath, fileName, true);
                    CopiedFiles.Add(fileName);
                }
                //SourcePath = Path.GetFullPath(Path.Combine(SourcePath, @"..\")) + "\\ScreenShot";
                //objDML.Add_Exception_Log(SourcePath, "ScreenshotPath");
                //foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
                //{
                //    //System.Threading.Thread.Sleep(2000);
                //    string fileName = newPath.Replace(SourcePath, DestinationPath);
                //    File.Copy(newPath, fileName, true);
                //    fileName = newPath.Replace(SourcePath, DestinationPath + "\\RPA");
                //    File.Copy(newPath, fileName, true);
                //    //System.Threading.Thread.Sleep(2000);
                //    CopiedFiles.Add(fileName);
                //}
                return CopiedFiles;
            }
            catch (Exception ex)
            {
                objDML.Add_Exception_Log("Wipro exception : " + ex.Message, "FileUpload");
                return CopiedFiles;
            }
        }
    }
}
