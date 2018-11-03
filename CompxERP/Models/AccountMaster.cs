//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Web;
//using System.Web.WebPages.Html;
////using System.Web.Optimization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CompxERP.Models
{

    public class AccountMaster : usermenust
    {
		public List<SelectListItem> acctcateData { get; set; } 
        public List<SelectListItem> acctjmblData { get; set; } 
        public List<SelectListItem> acctitrdData { get; set; } 
        public List<SelectListItem> acctagenData { get; set; }
        public List<SelectListItem> acformreqData { get; set; }
        public List<SelectListItem> acctgstaData { get; set; }//170825
        public decimal? acctjmbl { get; set; }
        public decimal? acctrate { get; set; }
        public string acformreq { get; set; }
        public string groumain { get; set; }
        public string grouaddr { get; set; }
        public string grourepo { get; set; }
        public string isEdit { get; set; }
        [Required(ErrorMessage = "*")]
        public int acctgrou { get; set; }
        [Required(ErrorMessage = "*")]
        public string acctname { get; set; }
        public int? acctcode { get; set; }
        public string acctalia { get; set; }
        public string acctrema { get; set; }
        public string acctsort { get; set; }
        //public string acctconp { get; set; } ECC
        public int? acctdsa { get; set; }
        public string TinNo { get; set; }
        [RegularExpression(@"([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?", ErrorMessage = "Enter Valid PAN")]
        public string PAN { get; set; }
        public string CSTNo { get; set; }
        public string acctPhone { get; set; }
        [RegularExpression(@"[0]?[789]\d{9}", ErrorMessage = "Enter Valid Mobile No.")]
        public string acctMob2 { get; set; }
        [RegularExpression(@"[0]?[789]\d{9}", ErrorMessage = "Enter Valid Mobile No.")]
        public string acctphon { get; set; }
        //[DataType(DataType.acctphon)] Ajay on 20052017
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")]
        //[RegularExpression(@"^([0]|\+91[\-\s]?)?[789]\d{9}$")]
        //[RegularExpression("[^0-9]", ErrorMessage = "number")]
        //public string acctphon { get; set; }
        public string accthind { get; set; }
        public string acctaddr { get; set; }
        public int? acctarea { get; set; }
        public int? acctcity { get; set; }
        public int? acctstat { get; set; }
        public int compcode { get; set; }
        public int acctledg { get; set; }
        public string PIN { get; set; }
        public string acctconp { get; set; }
        public int? acctcurr { get; set; }
        public int? acctcate { get; set; }
        public string acctfax { get; set; }
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Enter Valid Email ID")]
        public string acctmail { get; set; }

        [Required(ErrorMessage = "Please Select Group Type")]
        [Display(Name = "Please Select Type")]
        public string acctgroup { get; set; }
        public string acctareaname { get; set; }
        public string acctledgname { get; set; }
        public string Cityname { get; set; }
        public string Statename { get; set; }
        public string acctpinc { get; set; }
        public int? acctcform { get; set; }
        public int? acctCntry { get; set; }
        public int? acctcldr { get; set; }
        public int? acctclcr { get; set; }
		//public int? acctjmbl { get; set; }
        public int? acctagen { get; set; }
        public int? acwithbl { get; set; }
        public int? acctitrd { get; set; }

        //public int? acctrate { get; set; }
        public int? acctprty { get; set; }
        public int? acctprbl { get; set; }
        public int? acFarmerDC { get; set; }
        //[RegularExpression(@"([0-9]){2}([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?([0-9]){2}([a-zA-Z]){2}", ErrorMessage = "Enter Valid GSTIN")]
		public string acctgstin { get; set; }//170817
        public int? acctgsta{ get; set; }//170825
        public string acctOwner { get; set; }//170817

        public string acctpinc_Hearder { get; set; }

    }
}