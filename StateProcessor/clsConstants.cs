using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateProcessor
{
    static class clsConstants
    {
        public const string MRLState = "REQ-0006";
        public const string FinalState = "REQ-0008";

        public const string Created = "REQ-0002";
        public const string AwaitingDEAllocation = "REQ-0004";
        public const string DataEntry = "REQ-0006";
        public const string AwaitingQCAllocation = "REQ-0007";
        public const string AssistedQC = "REQ-0009";
        public const string ValidationofScope = "REQ-0010";
        public const string AllocateQCVerification = "REQ-0012";
        public const string QCAuthorization = "REQ-0014";
        public const string AllocateManualScoping = "REQ-0015";
        public const string ManualScoping = "REQ-0016";
        public const string DocumentSetRequestCreated = "REQ-0018";
        public const string Scopingrunfailed = "REQ-0099";
        public const string EscalatedDEExceeded = "REQ-7000";
        public const string EscalatedQCExceeded = "REQ-7002";
        public const string EscalatedScopingTimeExceeded = "REQ-7004";
        public const string OnHoldDEStage = "REQ-8000";
        public const string Cancelled = "REQ-9999";
    }
}
