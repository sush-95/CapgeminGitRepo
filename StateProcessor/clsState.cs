using DataAccess_Utility;
using Read_File_Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateProcessor
{
    public class clsState
    {
        public void SwitchAllOpenStates(tbl_request_state_instance instance)
        {
            switch (instance.StateID)
            {
                case clsConstants.AssistedQC:
                    AssistedQC(instance);
                    break;
                case clsConstants.ManualScoping:
                    AssistedQC(instance);
                    break;
                case clsConstants.MRLState:
                    ExecuteMRL(instance);
                    break;
                case clsConstants.FinalState:
                    SendFinalStatusToBOT(instance);
                    break;
                default:
                    break;
            }
        }
        public void AssistedQC(tbl_request_state_instance instance)
        {
            //JsonCreater objJson = new JsonCreater();
            //List<tbl_document_data> lstDataEntry = objGet.Get_DataEntry(instance.RequestID.ToString());
            //objJson.GetAssitedQCJson(lstDataEntry);
        }
        public void ExecuteMRL(tbl_request_state_instance instance)
        {

        }
        public void SendFinalStatusToBOT(tbl_request_state_instance instance)
        {

        }
    }
}
