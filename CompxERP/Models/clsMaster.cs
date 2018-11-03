using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompxERP.Models
{
    public class clsMaster
    {
    }

    public class clsCallDet
    {
        public int ID  { get; set;  }
        public int CallID  { get; set;  }
        public int Status	  { get; set;  }
        public int Ratio	  { get; set;  }
        public int isLead	  { get; set;  }
        public int UserID	  { get; set;  }
        public int ProductID  { get; set;  }
        public string Mobile  { get; set;  }
        public string Remark  { get; set;  }
        public string StudName  { get; set;  }

    }
    public class JsonItem
    {
        public string Desc { get; set; }
        public string Unit { get; set; }
        public string Packing { get; set; }
        public string Avg { get; set; }
        public string Alias { get; set; }
        public string Extra { get; set; }
        public string Vat { get; set; }
        public string Hindi { get; set; } public string ItemRate { get; set; } public string itemcode { get; set; }
    }

    public class clsOrderData
    {
        public string mstchno { get; set; }
        public string mstDrawNo { get; set; } 

        public string mstContactPerson { get; set; } 
        public string mstrema { get; set; } 
        public int mstcode   { get; set; } 
        public int compcode   { get; set; } 
        public decimal msttota    { get; set; } 

        public string City { get; set; } 
        public string Dealer { get; set; } 
        public string Company { get; set; }
        public DateTime mstdate { get; set; }
         
    }
    
    public class clsOrderFollowUp
    {
        public string acctname { get; set; }
        public string Remark { get; set; }
        public int acctcode { get; set; }
        public string acctphon { get; set; }
        public DateTime FDate { get; set; }
        public DateTime NextDt { get; set; }
         
    }

    public class clsPF_Receipt
    {
        public string acctname { get; set; } 
        public string ChqNo  { get; set; }
        public string Remark { get; set; }
        public string Bank { get; set; }
        
        public int ID  { get; set; }
        public int DealerID  { get; set; }
        public int UserID  { get; set; }
        public int ModeID  { get; set; }
        public int BankID  { get; set; }
        public int TypeID  { get; set; }
      
        public DateTime MstDate  { get; set; }
        public DateTime CreatedOn  { get; set; }
        public DateTime ChqDate  { get; set; }

        public string sChqDate { get; set; }
        public string sMstDate { get; set; }
        public string sMode { get; set; }
        public string sType { get; set; }
        public string sStatus { get; set; }

        public decimal Amount { get; set; }
        public decimal AccessAmt { get; set; }
        public decimal DemandAmt { get; set; }
    }
}
 