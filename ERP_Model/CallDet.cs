using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Model
{
    class CallDet
    { 
        public string Mobile  { get; set; }
        public string Remark  { get; set; }
        public int  Status	  { get; set; }
        public int  Ratio	  { get; set; }
        public int  UserID	  { get; set; }
        public int  ProductID { get; set; }
        public int  SubCateID { get; set; }
        public int  ItemID	  { get; set; }
        public int  CallID { get; set; }
        public DateTime FollowUpDt  { get; set; }
        public bool isLead  { get; set; }
         
    }
}
