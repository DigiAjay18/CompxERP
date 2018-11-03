using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompxERP.Models
{
    public class clsReportMaster
    {
    }
public class DueList
    {

        public string acctdesc { get; set; }
        public string voucrefn { get; set; }
        public string CompName { get; set; }
        public int vouccond { get; set; }
        public int voucdays { get; set; }
    public int trnledg { get; set; }
        public int rowidnum { get; set; }
    public int vouctype { get; set; }
    public int vouccode { get; set; }
    public int compcode { get; set; }
    public int FollowUp { get; set; }
  public int acctcode { get; set; }

  public int StatusID { get; set; }

  public int InvDue { get; set; }
  public Int32 DueDays { get; set; }
     
        public DateTime voucdate { get; set; }
        public DateTime voucdued { get; set; }

        public decimal voucdrvl { get; set; }
        public decimal voucblvl { get; set; }
        public decimal balancev { get; set; }
        public decimal sumbala { get; set; }
    public decimal Dues { get; set; }
        public decimal OrdValue { get; set; }
        public decimal Commited { get; set; }

        public DateTime FDate  { get; set; }
        public DateTime NextDt  { get; set; }
        public decimal CommitPay  { get; set; }
        public string Remark { get; set; }
        public string Mobile { get; set; }
        public decimal MaxDues { get; set; }
    }

}