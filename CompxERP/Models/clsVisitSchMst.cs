using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompxERP.Models
{
    /*Start 181012 */
    public class clsVisitSchMst
    {
        public int vsCompCode { get; set; }
        public string vsDate { get; set; }
        public string vsCode { get; set; }
[Required(ErrorMessage = "Please Select Employee Name...")]
        public string vsEmpID { get; set; }
        [Required(ErrorMessage = "Please Feed Schedule Date...")]
        public string vsSchDate { get; set; }
        public string vtPartyName { get; set; }
        public string vtArea { get; set; }
        public string sTrnDet { get; set; }

        public int aCode { get; set; }
        public string aType { get; set; }
        public string aMsg { get; set; }

        public List<SelectListItem> lstSchEmp { get; set; }
        public List<SelectListItem> lstPartyName { get; set; }
        public List<SelectListItem> lstArea { get; set; }
    }
    public class clsVisitSchTrn
    {
        public int vtCompCode { get; set; }
        public int vtCode { get; set; }
        public int vsID { get; set; }
        public string vtTypeName { get; set; }
        public string vtPartyName { get; set; }
        public string vtArea { get; set; }
        public string vtMobile { get; set; }
        public string vtAddress { get; set; }
        public string vtModelNo { get; set; }
        public string vtMachineNo { get; set; }
        public string vtWeightName { get; set; }
        public int vtValidFor { get; set; }
        public string vtMachineType { get; set; }
        public int vtDueYear { get; set; }
        public int vtDueMonths { get; set; }
        public DateTime vtValidUpToDate { get; set; }
        public string vtVcType { get; set; }

        public int aCode { get; set; }
        public string aType { get; set; }
        public string aMsg { get; set; }

        public List<clsVisitSchTrn> LstItem { get; set; }
    }
	   /*End 181012  */
	public class clsDailyVisitEntry/*181016 */
    {
        public int dvCompCode { get; set; }
        public Nullable<DateTime> dvDate { get; set; }
        public int dvCode { get; set; }
        public string dvPartyName { get; set; }
        public string dvMobile { get; set; }
        public string dvVisitDetail { get; set; }
        public decimal dvEstCost { get; set; }
        public Nullable<DateTime> dvNextFollowUp { get; set; }
        public string dvRemark { get; set; }
        
        public int aCode { get; set; }
        public string aType { get; set; }
        public string aMsg { get; set; }

        public List<SelectListItem> lstSchEmp { get; set; }
        public int dvQty { get; set; }/*181017 %temp%*/
    }
 
}