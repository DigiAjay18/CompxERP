using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace CompxERP.Models
{
    public class clsUser
    {

        public  int useshowtr	  { get; set; }
        public  int usestatus	  { get; set; }
        public  int Compcode	  { get; set; }
        public  int usecode  { get; set; }
        public  int usetype   { get; set; }

        public  int Tot_Dist   { get; set; }
        public  int Tot_Deal   { get; set; }
        public int Tot_Emp { get; set; }
         
        [Required(ErrorMessage = "Please Enter User Name")]
        [Display(Name = "Enter User Name")]
        public  string   usepass    { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        [Display(Name = "Enter Password")]
        public  string usename      { get; set; }
        public  DateTime   usecrdt    { get; set; }
        public DateTime usemodt { get; set; }
        public string UserType { get; set; }
        public string PassWord { get; set; }
        public string UserNM { get; set; }

    }
    public class clsLedger_
    {

        public int id { get; set; }
public string HeadDes     { get; set; }
public string VchNo       { get; set; }
public DateTime VchDt { get; set; }
public string VchTp       { get; set; }
public string Particulars { get; set; }
public decimal DrAmt { get; set; }
public decimal CrAmt { get; set; }
public string Narration   { get; set; }
public string GSTType     { get; set; }
public DateTime CreatedOn { get; set; }
         
    }
    public class clsLedger
    {

        public string grouname { get; set; }
        public int? acctcode { get; set; }
        public string acctname { get; set; }
        public string cityname { get; set; }
        public decimal? opbl { get; set; }
        public decimal dr { get; set; }
        public decimal cr { get; set; }
        public decimal? crdrbl { get; set; }
        public string grourepo { get; set; }
        public DateTime? mstdate { get; set; }
        public int? mstcode { get; set; }
        public int? msttype { get; set; }
        public string VcNO { get; set; }
        public string Particular { get; set; }
        public int? trnsrno { get; set; }
        public string trnrema { get; set; }
        public string mstrema { get; set; }
        public string sMstdate { get; set; }
        public string sType { get; set; }
        public string FromDt { get; set; }
        public string ToDt { get; set; }
        public List<clsLedger> LstItem { get; set; }
        public int? PartyID { get; set; }
 public int? Compcode { get; set; }


    }
    public class clsFilter
    {
        public clsFilter() { }

        public int ItemID { get; set; }
        public string ItemNm { get; set; }

        public int PartyID { get; set; }
        public string PartyName { get; set; }

        public string unitname { get; set; }
        public int unitcode { get; set; }

        public string dFrom { get; set; }
        public string dTo { get; set; }

        public bool IsSelected { get; set; }

        public List<clsFilter> lstFilter { get; set; }
        public List<clsFilter> lstAgent { get; set; }
        public List<clsFilter> lstEmployee { get; set; }
        public List<clsFilter> lstCity { get; set; }
          public List<clsFilter> lstBrand { get; set; }
    }

    public class clsDayWork
    {
        public int trnCode { get; set; }
        public string Acctname { get; set; }
        public string EType { get; set; }
        public decimal CR { get; set; }
        public decimal DR { get; set; }
        public string Remark { get; set; }
        public DateTime WorDate { get; set; }
    }

    public class SaleBook
    {
        public int mstcode { get; set; }
        public int compcode { get; set; }
        public int msttype { get; set; }

        public string partyname { get; set; }
        public DateTime mstdate { get; set; }

        public string Acctname { get; set; }
        public string mstchno { get; set; }

        public decimal sumitdamou { get; set; }
        public decimal sumcgstamou { get; set; }
        public decimal sumsgstamou { get; set; }
        public decimal sumigstamou { get; set; }
        public decimal mstneta { get; set; }

    }
}



	 
	 
	  	 
             