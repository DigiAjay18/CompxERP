using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompxERP.Models
{
    public class Jourmaster : clsPoItem
    {

        public int compcode { get; set; }
        //   public int MstType { get; set; }
        public int msttype { get; set; }
        public int trntype { get; set; }
        public int trncode { get; set; }
        public int mstcode { get; set; }
        public int IsPrint { get; set; }
        public int mstcode_Print { get; set; }
        //[Display(Name = "Tanggal Lahir")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //public string TanggalLahir { get; set; }

        //[Display(Name = "Date")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //public System.DateTime mstdate { get; set; }
        public DateTime mstdate { get; set; }
        public string sMstdate { get; set; }

        public string  ChallanDate { get; set; }
        public string MSG { get; set; }public string Header { get; set; }
        public decimal msttota { get; set; }

        public string mstrema { get; set; }

        public string mstrefc { get; set; }

        public string mstchno { get; set; }

        public System.Nullable<int> mstptcode { get; set; }

        public System.Nullable<int> mstdrcode { get; set; }
        public System.Nullable<int> PartyID { get; set; }
        public string mstgncd { get; set; }

        public string mstexti { get; set; }

        public System.Nullable<int> mstAppr { get; set; }

        public System.Nullable<int> mstsamen { get; set; }

        public System.Nullable<double> oldwht { get; set; }

        public System.Nullable<decimal> _oldamt { get; set; }

        public System.Nullable<double> mstpay { get; set; }

        public string mstcfno { get; set; }

        public System.Nullable<System.DateTime> mstcfdt { get; set; }

        public System.Nullable<System.DateTime> StDate { get; set; }
        public System.Nullable<System.DateTime> LastDate { get; set; }

        public System.Nullable<int> mststat { get; set; }
        public System.Nullable<int> msttime { get; set; }

        public System.Nullable<decimal> mstbala { get; set; }

        public System.Nullable<decimal> mstneta { get; set; }
        public System.Nullable<decimal> DisPer { get; set; }
        public System.Nullable<decimal> DisAmt { get; set; }
        public System.Nullable<decimal> Exp1_Per { get; set; }

        public string mstblno { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}")]
        public DateTime? HireDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public System.Nullable<System.DateTime> mstbldt { get; set; }

        public string mstclno { get; set; }
       
        public System.Nullable<System.DateTime> mstcldt { get; set; }

        public string msttimR { get; set; }

        public string msttimI { get; set; }

        public string mstchnm { get; set; }

        public System.Nullable<int> mstsite { get; set; }

        public System.Nullable<decimal> mstpaid { get; set; }

        public System.Nullable<int> mstctyp { get; set; }

        public System.Nullable<decimal> mstexca { get; set; }

        public System.Nullable<decimal> msteces { get; set; }

        public System.Nullable<decimal> mstsurc { get; set; }

        public System.Nullable<decimal> mstqtyd { get; set; }

        public System.Nullable<decimal> mstfina { get; set; }

        public string mstaddr { get; set; }

        public System.Nullable<int> mstbrok { get; set; }

        public System.Nullable<int> mstbrnc { get; set; }

        public System.Nullable<int> mstsubt { get; set; }

        public System.Nullable<decimal> msttopay { get; set; }

        public System.Nullable<decimal> msttobil { get; set; }

        public string mstconnm { get; set; }

        public string mstdodr { get; set; }

        public string mstvtype { get; set; }

        public System.Nullable<int> msttaxby { get; set; }

        public System.Nullable<decimal> mstyldp { get; set; }

        public System.Nullable<decimal> mstadde { get; set; }

        public System.Nullable<decimal> mstlate { get; set; }

        public System.Nullable<int> mstcust { get; set; }

        public System.Nullable<decimal> mstminqty { get; set; }

        public System.Nullable<decimal> mstExAmt { get; set; }

        public string mstser1 { get; set; }

        public string mstser2 { get; set; }

        public string mstHindiNrr { get; set; }

        public string mstbrhd { get; set; }

        public string msttrhd { get; set; }

        public string msthdcd { get; set; }

        public System.Nullable<int> mstcomp { get; set; }

        public System.Nullable<int> mstcurrcd { get; set; }

        public System.Nullable<decimal> mstcurrrat { get; set; }

        public System.Nullable<int> mstpdc { get; set; }
        public bool IsPostDt { get; set; }
        public bool IsEdit { get; set; }

        public System.Nullable<decimal> mstdifq { get; set; }

        public System.Nullable<int> mstincome { get; set; }

        public bool IsAcctPay { get; set; }
        public System.Nullable<int> mstactpay { get; set; }

        public System.Nullable<int> mstbuyerc { get; set; }

        public System.Nullable<decimal> mstperd { get; set; }

        public System.Nullable<int> mstchsisno { get; set; }

        public System.Nullable<decimal> mstexon { get; set; }

        public System.Nullable<decimal> mstscon { get; set; }

        public string mstContactPerson { get; set; }

        public string mstContactType { get; set; }

        public System.Nullable<System.DateTime> mstDueDate { get; set; }

        public string mstreftag { get; set; }

        public System.Nullable<int> mstempo { get; set; }

        public System.Nullable<int> mstdepa { get; set; }

        public string mstindno { get; set; }

        public System.Nullable<System.DateTime> mstinddat { get; set; }

        public string mstpono { get; set; }

        public System.Nullable<System.DateTime> mstpodate { get; set; }

        public string mstqno { get; set; }

        public System.Nullable<System.DateTime> mstqdt { get; set; }

        public string mstinvno { get; set; }

        public System.Nullable<System.DateTime> mstinvdt { get; set; }

        public string mststatus { get; set; }

        public string mstPerm { get; set; }

        public string msttrfr { get; set; }

        public System.Nullable<int> mstschno { get; set; }

        public System.Nullable<int> mstgpno { get; set; }

        public string mstexcDes { get; set; }

        public string msttaxDes { get; set; }
        public System.Nullable<decimal> msttaxper { get; set; }
        public System.Nullable<decimal> BalAmt { get; set; }
        public string mstfrghtDes { get; set; }

        public System.Nullable<decimal> mstfrghtper { get; set; }

        public string mstdeliDes { get; set; }

        public string mstpayDes { get; set; }

        public string mstpayMode { get; set; }

        public string mstvaliDes { get; set; }

        public string MstPriceQuoted { get; set; }

        public System.Nullable<System.DateTime> mstgpdt { get; set; }

        public System.Nullable<int> mstbsrc { get; set; }

        public System.Nullable<int> msttvcn { get; set; }

        public System.Nullable<decimal> mstbsrt { get; set; }

        public string mstchnH { get; set; }

        public System.Nullable<int> mstchnV { get; set; }

        public System.Nullable<decimal> mstvat1 { get; set; }

        public System.Nullable<decimal> mstvat2 { get; set; }

        public System.Nullable<decimal> mstvat3 { get; set; }

        public System.Nullable<int> mstautcd { get; set; }

        public System.Nullable<System.DateTime> mstautdt { get; set; }

        public System.Nullable<int> mstautpr { get; set; }

        public System.Nullable<System.DateTime> mstauttm { get; set; }

        public System.Nullable<decimal> mstincv { get; set; }

        public string mstack1 { get; set; }

        public string mstack2 { get; set; }

        public string mstack3 { get; set; }

        public string mstack4 { get; set; }

        public System.Nullable<int> mstchqadj { get; set; }
        public bool IsChqAdj { get; set; }

        public System.Nullable<int> mstfinc { get; set; }

        public System.Nullable<int> mstcnfst { get; set; }

        public System.Nullable<decimal> mstincv7 { get; set; }

        public System.Nullable<int> mstvincl { get; set; }

        public System.Nullable<int> mstuser { get; set; }

        public System.Nullable<int> lotcode { get; set; }

        public int mstrowc { get; set; }

        public System.Nullable<int> grprowc { get; set; }

        public System.Nullable<int> mstoldc { get; set; }

        public System.Nullable<int> mstsale { get; set; }

        public string mstrefno { get; set; }

        public System.Nullable<System.DateTime> mstCmpdt { get; set; }

        public string mstJobNo { get; set; }

        public string mstDrawNo { get; set; }

        public System.Nullable<int> mstjobEnd { get; set; }

        public string mstlotno { get; set; }

        public string mstsection { get; set; }

        public string mstminno { get; set; }

        public System.Nullable<int> mstHour { get; set; }

        public System.Nullable<int> mstElUn { get; set; }

        public System.Nullable<int> msttrat { get; set; }

        public System.Nullable<decimal> mstvonw { get; set; }

        public System.Nullable<decimal> mstvval { get; set; }

        public System.Nullable<decimal> mstincvz { get; set; }

        public System.Nullable<decimal> mstvat0 { get; set; }

        public System.Nullable<int> mstrfvc { get; set; }

        public System.Nullable<int> mstrfvt { get; set; }

        public System.Nullable<int> mstdsptoc { get; set; }

        public string itmload { get; set; }

        public string totalw { get; set; }

        public string packnos { get; set; }

        public System.Nullable<int> mstexec { get; set; }
        //  [Required(ErrorMessage = "*")]
        public string partyname { get; set; }

        public string Remark { get; set; }
        public System.Nullable<decimal> tpDrAmt { get; set; }
        public System.Nullable<decimal> tpCrAmt { get; set; }
        public string Narration { get; set; }
        public int tpPartyID { get; set; }
        public int type { get; set; }
        public int UserID { get; set; }
        public string msttimes { get; set; }
        public string sItemDet { get; set; }
        public List<Jourmaster> ListArea { get; set; }
        public List<Jourmaster> LstItem { get; set; }

        public System.Nullable<decimal> totDrAmt { get; set; }
        public System.Nullable<decimal> totCrAmt { get; set; }

        public string acctname { get; set; }
        public string sPurSalType { get; set; }

        public System.Nullable<decimal> trndram { get; set; }
        public System.Nullable<decimal> trncram { get; set; }

        public string trnrema { get; set; }

        public string sJourTrn { get; set; }

        public System.Nullable<int> acctgrou { get; set; }




        public int ItemID { get; set; }
        public int Srno { get; set; }
        public int? Qty { get; set; }
        public int? Rate { get; set; }
        public decimal Amt { get; set; }

        public string ItemRemark { get; set; }
        public string ItemNm { get; set; }

        public string Broker { get; set; }
        public string BillType { get; set; }

        public decimal Interest { get; set; }
        public decimal Comm { get; set; }
        public decimal CommAmt { get; set; }
        public int DueDay { get; set; }
        public decimal ExcisePer { get; set; }
        public decimal Excise { get; set; }
        public decimal? TaxPer { get; set; }
        public decimal Tax_S { get; set; }
        public decimal Tax_C { get; set; }
        public decimal Tax_I { get; set; }
        public decimal Tax_Amt { get; set; }
        public decimal ItemNetAmt { get; set; }

        public string sTrnDet { get; set; }
        public decimal VATPer { get; set; }
        public decimal VAT { get; set; }

        public decimal Exp1_Amt { get; set; }
        public decimal Exp2_Amt { get; set; }
        public decimal Exp3_Amt { get; set; }
        public decimal Exp4_Amt { get; set; }
        public decimal Exp2_Per { get; set; }
        public decimal Exp3_Per { get; set; }
        public decimal Exp4_Per { get; set; }

        public System.Nullable<decimal> TotDis { get; set; }
        public System.Nullable<decimal> TotTax { get; set; }

        public List<SelectListItem> lstUnitMst { get; set; }
        //public List<SelectListItem> lstBillType { get; set; }
        public SelectList StateList { get; set; }


        public decimal? Cases { get; set; }
        public decimal PU_Qty2 { get; set; }
        public string UnitID { get; set; }
        public int? acctcode { get; set; }
        public decimal ItemTax { get; set; }


        public string ItemNm_Fini { get; set; }
        public int ItemID_Fini { get; set; }
        public int Qty_Fini { get; set; }
        public int Rate_Fini { get; set; }
        public decimal Amt_Fini { get; set; }
        public string UnitID_Fini { get; set; }
        public decimal Cases_Fini { get; set; }

        public string ItemNm_Pack { get; set; }
        public int ItemID_Pack { get; set; }
        public int Qty_Pack { get; set; }
        public int Rate_Pack { get; set; }
        public decimal Amt_Pack { get; set; }
        public string UnitID_Pack { get; set; }
        public decimal Cases_Pack { get; set; }

        public string ItemNm_Exp { get; set; }
        public int ItemID_Exp { get; set; }
        public int Qty_Exp { get; set; }
        public int Rate_Exp { get; set; }
        public decimal Amt_Exp { get; set; }
        public string UnitID_Exp { get; set; }
        public decimal Cases_Exp { get; set; }

        public decimal TotQtyRaw { get; set; }
        public decimal TotQtyFini { get; set; }
        public decimal TotQtyPack  { get; set; }
        public decimal TotQtyExp  { get; set; } 

        public decimal TotalRaw { get; set; }
        public decimal TotalPack { get; set; }
        public decimal TotalExp  { get; set; }

        public string sItemRaw { get; set; }
        public string sItemFinish { get; set; }
        public string sItemPacking { get; set; }
        public string sItemExp { get; set; }

        public int SchemeID { get; set; }
        public List<clsPoItem> lstParty { get; set; }
        public decimal Ratio { get; set; }
        public decimal Ratio_Org { get; set; }
        public int CateID { get; set; }
        public int GrouID{ get; set; }
        public string CateNm { get; set; }
        public string GrouNm { get; set; }
        public int RatioType { get; set; }

        public string CateNm_Fini { get; set; }
        public string GrouNm_Fini { get; set; }
        public string CateNm_Pack { get; set; }
        public string GrouNm_Pack { get; set; }
		        public string acctledg { get; set; }//170830
        public int? mstprtc { get; set; }//170830
        public string msternv { get; set; }//170830
        public string mstpofs { get; set; }//170830
        public decimal? mstintr { get; set; }//170830
        public int? mstdued { get; set; }//170830
        public int? mstrvsc { get; set; }//170830
        public string mstIorL { get; set; }//170830

        public string NewName { get; set; }//170830
        public string NewAddress { get; set; }//170830
        public string NewGSTIN { get; set; }//170830
         [RegularExpression(@"[0]?[789]\d{9}", ErrorMessage = "Enter Valid Mobile No.")]
        public string NewMobile { get; set; }//170830
        public int? SubCate { get; set; }//170830

        public string NewItemNm { get; set; }//170830
        public string NewItemHSN { get; set; }//170830
        public string NewItemGST { get; set; }//170830
        public int?  itemnumb { get; set; }//170830
        public decimal? NewItemVat { get; set; }//170830
        public decimal? NewItemEcee { get; set; }//170830
        public int? unitcode { get; set; }//170830

        public string Cityname { get; set; }//2/10/2017
        public string Statename { get; set; }//2/10/2017
        public int? acctcity { get; set; }//2/10/2017
        public int? acctstat { get; set; }//2/10/2017

        public int? mstappl { get; set; }

        public int hdnAcGroup { get; set; }
        public int TaxHead_I { get; set; }

          [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Enter Valid Email ID")]
        public string EMail { get; set; }
        
             public IEnumerable<clsPoItem> LstOrdItd2 { get; set; }
             public List<clsPoItem> LstOrdItd { get; set; }

             public List<SelectListItem> lstFTerm { get; set; }
             public List<SelectListItem> lstOTerm { get; set; }


           
     

    }
}