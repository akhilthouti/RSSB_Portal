using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INDO_FIN_NET.Models
{
    public class ClsFormFlag
    {
        public long PersonalId { get; set; }
        public Nullable<bool> IsLogin { get; set; }

        public Nullable<bool> IsQuickEnrollSubmit { get; set; }

        public Nullable<bool> IsDocumentSubmit { get; set; }

        public Nullable<bool> IsIPVSubmit { get; set; }

        public Nullable<bool> RekycCustomer { get; set; }
        public Nullable<bool> isIPVSkip { get; set; }

        public Nullable<bool> isCAFPDF { get; set; }
        public Nullable<bool> isSavingAcc { get; set; }
        public Nullable<bool> IsSignUpDone { get; set; }

        public Nullable<bool> IssummarysheetSubmit { get; set; }
        public string proceedwithOCR { get; set; }

        public string shareAadharNumber { get; set; }

        public string KYCverificationType { get; set; }

        public string AccountType { get; set; }
    }

}
