using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompxERP.Models
{
    public class Citydet
    {
        public int cityType { get; set; }
        public int citycode { get; set; }
        public string cityname { get; set; }
        public string cityalia { get; set; }//170803
        public string cityrute { get; set; }//170803
        public string cityexti { get; set; }//170803
        public string cityhindi { get; set; }//170803
    }

    public class clsPayFollowUp
    { 
    public int FID { get; set; }
    public int BillID { get; set; }
    public int PartyID { get; set; }
    public int StatusID { get; set; }
    public int UseCode { get; set; }
        public string  BillNo { get; set; }
        public string Remark  { get; set; }
        public DateTime FDate  { get; set; }
        public DateTime NextDt  { get; set; }
        public decimal  CommitPay { get; set; }
        public string Acctname { get; set; }
          
    }

}