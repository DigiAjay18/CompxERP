using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompxERP.Models
{
    public class clsOrdeMst
    {

        public string mstchno { get; set; }
        public string mstrema { get; set; }
        public int mstcode  { get; set; }
        public DateTime mstdate  { get; set; }
        public  decimal msttota  { get; set; }

        public List<clsOrdeMst> lstOrder { get; set; }
    }
     

}