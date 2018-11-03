using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompxERP.Models
{
    public class clsTicketMst
    {

         
        public int tFor {get ; set;}
        public int tTopic {get ; set;}
        public int tType {get ; set;}
        public int tUserID {get ; set;}
        public int tStatus {get ; set;}

        public string tNo  {get ; set;}
        public string tDesc  {get ; set;}
        public string tPath  {get ; set;}

        public DateTime tDate { get; set; }
        public string sDate { get; set; }
       
       public HttpPostedFileWrapper ImageFile { get; set; }
          
    }
}