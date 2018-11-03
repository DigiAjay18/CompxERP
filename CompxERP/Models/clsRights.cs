using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CompxERP.Models
{
    public class clsRights
    {
        public Int32 mencode { get; set; }
        public Int32 menuser { get; set; }
        public Int32 GrpCode { get; set; }
        public Int32 UseHeadID{ get; set; }

        public string Menname { get; set; }

        public bool menview { get; set; }
        public bool menaddi { get; set; }
        public bool menedit { get; set; }
        public bool mendele { get; set; }
        public bool menacce { get; set; }

        public List<SelectListItem> lstGrpMenu { get; set; }
        public List<SelectListItem> lstUser { get; set; }
        public List<SelectListItem> lstGrpUser{ get; set; }

    }
    public class clsHierarchyUser
    {
        public string usename { get; set; }
        public Int32 usecode { get; set; }
        public string usernm { get; set; }
        public Int32 isExistsUser { get; set; }
    }
    public class JsonItemFormData
    {
        public string itemcode { get; set; }
        public string itemsrno { get; set; }
        public string target { get; set; }
        public string itgpnumb { get; set; }
        public string itgpalia { get; set; }
        public string itgpsort { get; set; }
        public string itgpcode { get; set; }
        public string itgpcode1 { get; set; }
        public string dDate { get; set; }

        public string target1 { get; set; }
        public string itgpalia1 { get; set; }
        public string itgpsort1 { get; set; }

        public string itgpcodesc { get; set; }
        public string unitcode { get; set; }
        public string UnitName { get; set; }
        public string itgpbcqt { get; set; }
        public string itgptype { get; set; }
        public string itgprefn { get; set; } 
        public string itgpcart { get; set; } 
        public string itgprefn1 { get; set; } 
        public string itgpcart1 { get; set; } 
        public JsonItem[] Data { get; set; }


    }
     
}