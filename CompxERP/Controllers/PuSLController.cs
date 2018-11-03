//using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompxERP.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using CompxERP.Filters;
 

namespace Compx.Controllers
{
    [UserSessionfilter]
    public class PuSLController : Controller// CultureController  
    { 
        //
        // GET: /PuSL/
        ERPDataContext db = new ERPDataContext();
        public ActionResult Index(int MstType=0 ,int mencode = 0)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;
            if (Convert.ToInt16(SessionMaster.CompCode) == 0) { return View("LogOff","Home" ); }

            //dt = oSubmit.GetVoucher(Convert.ToInt32(Request.QueryString["MstType"]), Convert.ToInt32(SessionMaster.CompCode));
            dt = oSubmit.GetVoucher(Convert.ToInt32(Request.QueryString["MstType"]), 0);
            ViewBag.MstType = Request.QueryString["MstType"];
            ViewBag.MenCode = Request.QueryString["mencode"];

            Jourmaster oModel = new Jourmaster();
            List<Jourmaster> oList = new List<Jourmaster>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                oModel = new Jourmaster();
                oModel.mstcode = Convert.ToInt32(dt.Rows[i]["mstcode"]);
                oModel.mstdate = Convert.ToDateTime(dt.Rows[i]["mstdate"]);
                oModel.msttota = Convert.ToDecimal(dt.Rows[i]["msttota"]);
                oModel.mstchno = dt.Rows[i]["mstChno"].ToString();
                oModel.mstrema = dt.Rows[i]["mstrema"].ToString();
                oModel.mstclno = dt.Rows[i]["mstclno"].ToString();
                oModel.mstchnm = dt.Rows[i]["mstchnm"].ToString();
                oModel.compcode = Convert.ToInt32(dt.Rows[i]["compcode"]);
                oModel.msttype = Convert.ToInt32(dt.Rows[i]["msttype"]);
                oModel.partyname =  dt.Rows[i]["partyname"].ToString();
                oList.Add(oModel);
            }
            oModel.ListArea = oList;
            return View(oModel);
        } 
        public ActionResult PuSLList(int MstType=0 ,int mencode = 0)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;
            if (Convert.ToInt16(SessionMaster.CompCode) == 0) { return View("LogOff","Home" ); } 
             
            dt = oSubmit.GetVoucher(Convert.ToInt32(Request.QueryString["MstType"]), Convert.ToInt32(SessionMaster.CompCode));
            ViewBag.MstType = Request.QueryString["MstType"];
            ViewBag.MenCode = Request.QueryString["mencode"];

            Jourmaster oModel = new Jourmaster();
            List<Jourmaster> oList = new List<Jourmaster>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                oModel = new Jourmaster();
                oModel.mstcode = Convert.ToInt32(dt.Rows[i]["mstcode"]);
                oModel.mstdate = Convert.ToDateTime(dt.Rows[i]["mstdate"]);
                oModel.msttota = Convert.ToDecimal(dt.Rows[i]["msttota"]);
                oModel.mstchno = dt.Rows[i]["mstChno"].ToString();
                oModel.mstrema = dt.Rows[i]["mstrema"].ToString();
                oModel.mstclno = dt.Rows[i]["mstclno"].ToString();
                oModel.mstchnm = dt.Rows[i]["mstchnm"].ToString();
                oModel.compcode = Convert.ToInt32(dt.Rows[i]["compcode"]);
                oModel.msttype = Convert.ToInt32(dt.Rows[i]["msttype"]);
                oModel.partyname =  dt.Rows[i]["partyname"].ToString();
                oList.Add(oModel);
            }
            oModel.ListArea = oList;
            return View("PuSLList", oModel);
        }
 //       [HttpGet]
 //       public JsonResult DelPuSL(int CompCode, int MstType, int MstCode, int MenCode)
 //       {
 //clsSubmitModel oSubmit = new clsSubmitModel();
 //string sMsg = "Delete Successfully...";
 //           try
 //           {  
 //               oSubmit.BeginTran(); 
 //               oSubmit.insertData("delete from jourMst  where compcode = " + CompCode + " and Msttype = " + MstType + " and  Mstcode = " + MstCode ); 
 //               oSubmit.insertData("delete from jourtrn  where compcode = " + CompCode + " and trntype = " + MstType + " and  trncode = " + MstCode ); 
 //               oSubmit.insertData("delete b from itpursal a inner join GathDet b on a.itdgath = b.gathCode and a.Itdtype =b.Itdtype where a.Itdtype = " + MstType + " and Itdcode = " + MstCode); 
 //               oSubmit.insertData("Delete from itpursal where CompCode = " + CompCode + " and Itdtype = " + MstType + " and Itdcode = " + MstCode); 
 //               oSubmit.Commit();
 //           }
 //           catch (Exception ex)
 //           {
 //               oSubmit.RollBack();
 //               sMsg = ex.Message;  
 //           }
             
 //           return Json(sMsg, JsonRequestBehavior.AllowGet); 
 //       }

        [HttpGet]
        public ActionResult Create(int comp = 0,   int mstCode = 0, int MstType = 0, int mstcode_Print = 0 ,int mencode =0)
        {
            if (mstcode_Print == 0)
            {
                TempData["message"] = null;
            }

            if (Convert.ToInt16(SessionMaster.CompCode) == 0) { return View("LogOff", "Home"); } 

            try{
            //DBEntity oDB = new DBEntity();
            comp = SessionMaster.CompCode;
            if (comp == 0)
            {
                TempData["message"] = "Please Select Company ...";
            }
                
            clsSubmitModel oSubmit = new clsSubmitModel();
            ViewBag.MstType = Request.QueryString["MstType"];
            Jourmaster frm = new Jourmaster();
                 
            //if (Convert.ToBoolean((from a in db.invesetts where a.optcode == 125 select a.optYN).First()) == true) frm.IsPrint = 1; else frm.IsPrint = 0;
             
            //************************** Rights ****************************
            DataTable dt;
            dt = oSubmit.GetData("select * from usermenust where mencode =" + Request.QueryString["MenCode"] + " and menuser =" + SessionMaster.UserID,true);
            //if (dt.Rows.Count > 0)
            //{
            //    if (Convert.ToBoolean(dt.Rows[0]["menview"]) == false) Response.Redirect("../home/menuNew");
            //    frm.menaddi = Convert.ToBoolean(dt.Rows[0]["menaddi"]);
            //    frm.menedit = Convert.ToBoolean(dt.Rows[0]["menedit"]);
            //    frm.menview = Convert.ToBoolean(dt.Rows[0]["menview"]);

            //    frm.mencode = Convert.ToInt32(dt.Rows[0]["menCode"]);       // Add new variable  menu code "Rupesh"
            //    ViewBag.MenCode = frm.mencode;                       //==//===
            //}
            //**************************************************************  

            if (MstType == 42)
                ViewBag.mstsale = new SelectList(from res in db.accounts where (res.acctgrou == 40 || res.acctgrou == 9) && res.compcode == comp orderby res.acctname select new { res.acctcode, res.acctname }, "acctcode", "acctname");
            else
                ViewBag.mstsale = new SelectList(from res in db.accounts where (res.acctgrou == 41 || res.acctgrou == 10) && res.compcode == comp orderby res.acctname select new { res.acctcode, res.acctname }, "acctcode", "acctname");


            ViewBag.Exp1 = new SelectList(from res in db.accounts where (res.acctgrou == ConstVariable.TypeCode_IndirectExpenses || res.acctgrou == ConstVariable.TypeCode_Taxes || res.acctgrou == ConstVariable.TypeCode_DirectExpenses) && (res.acctgsta == null || res.acctgsta == 0) && res.compcode == comp orderby res.acctname select new { res.acctcode, res.acctname }, "acctcode", "acctname");
           
                ViewBag.TaxHead_I = new SelectList(from res in db.accounts where (res.acctgrou == ConstVariable.TypeCode_Taxes) && res.compcode == comp select new { res.acctcode, res.acctname }, "acctcode", "acctname");

                DataTable dtUnit;

                List<clsPoItem> lstUnit = new List<clsPoItem>();
                //dtUnit = oSubmit.GetUnit(SessionMaster.CompCode); 
                //clsPoItem sm = new clsPoItem(); 
                //for (int i = 0; i < dtUnit.Rows.Count; i++)
                //{
                //    sm = new clsPoItem();
                //    sm.unitname = dtUnit.Rows[i]["unitname"].ToString();
                //    sm.unitcode = dtUnit.Rows[i]["unitcode"].ToString();
                //    lstUnit.Add(sm);
                //} 
                ViewBag.UnitID = new SelectList(from a in db.unitdets where a.compcode == SessionMaster.CompCode select new {  a.unitcode ,a.unitname }, "unitcode", "unitname");
                 
               ViewBag.mstctyp = new SelectList(from res in db.studdets where res.studType == 387 select new { res.studCode, res.studName }, "studcode", "studname");
                

            //===== code by rupesh
            if (mstCode <= 0)
            { 
                TempData["FrsRec"] = "This is First record!!!"; 
            }


            if (mstCode > 0)
            {
                #region  "Edit" 
                dt = oSubmit.GetVoucher(MstType, comp, mstCode);

                if (dt.Rows.Count > 0)
                {

                    //frm.msttimes = dt.Rows[0][""];

                    frm.msttota = Convert.ToDecimal(dt.Rows[0]["msttota"].ToString());
                    frm.mstpofs =  dt.Rows[0]["mstpofs"].ToString() ;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["mstbala"].ToString())) frm.mstbala = Convert.ToDecimal(dt.Rows[0]["mstbala"].ToString());
                    if (!string.IsNullOrEmpty(dt.Rows[0]["mstpaid"].ToString())) frm.mstpaid = Convert.ToDecimal(dt.Rows[0]["mstpaid"].ToString());
                    if (!string.IsNullOrEmpty(dt.Rows[0]["mstneta"].ToString())) frm.mstneta = Convert.ToDecimal(dt.Rows[0]["mstneta"].ToString());
                    frm.Remark = dt.Rows[0]["Remark"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[0]["mstblno"].ToString())) frm.mstblno = dt.Rows[0]["mstblno"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[0]["mstclno"].ToString())) frm.mstclno = dt.Rows[0]["mstclno"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[0]["mstbldt"].ToString())) if (dt.Rows[0]["mstbldt"].ToString() != "") frm.mstbldt = Convert.ToDateTime(dt.Rows[0]["mstbldt"].ToString());
                    if (!string.IsNullOrEmpty(dt.Rows[0]["mstcldt"].ToString())) if (dt.Rows[0]["mstcldt"].ToString() != "") frm.mstbldt = Convert.ToDateTime(dt.Rows[0]["mstcldt"].ToString());
                    //if (!string.IsNullOrEmpty(dt.Rows[0]["CommAmt"].ToString())) frm.CommAmt = Convert.ToDecimal(dt.Rows[0]["CommAmt"].ToString());
                    if (!string.IsNullOrEmpty(dt.Rows[0]["Comm"].ToString())) frm.Comm = Convert.ToDecimal(dt.Rows[0]["Comm"].ToString());
                    if (!string.IsNullOrEmpty(dt.Rows[0]["mstchnV"].ToString())) frm.mstchnV = Convert.ToInt32(dt.Rows[0]["mstchnV"].ToString());
                    if (!string.IsNullOrEmpty(dt.Rows[0]["mstchnH"].ToString())) frm.mstchnH = dt.Rows[0]["mstchnH"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[0]["mstchno"].ToString())) frm.mstchno = dt.Rows[0]["mstchno"].ToString();

                    frm.acctname = dt.Rows[0]["acctname"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[0]["Broker"].ToString())) frm.Broker = dt.Rows[0]["Broker"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[0]["mstPrtc"].ToString())) frm.PartyID = Convert.ToInt32(dt.Rows[0]["mstPrtc"].ToString()); //PartyID
                    if (!string.IsNullOrEmpty(dt.Rows[0]["mstbrok"].ToString())) frm.mstbrok = Convert.ToInt32(dt.Rows[0]["mstbrok"].ToString());

                    //if (dt.Rows[0]["msttime"].ToString().Length == 6)
                    //    frm.msttimes = dt.Rows[0]["msttime"].ToString().Substring(0, 2) + ":" + dt.Rows[0]["msttime"].ToString().Substring(2, 2) + ":" + dt.Rows[0]["msttime"].ToString().Substring(4, 2);
                    //else 
                    //    frm.msttimes = dt.Rows[0]["msttime"].ToString();

                    frm.mstrema = dt.Rows[0]["mstRema"].ToString();
                    ViewBag.MstCode = frm.mstcode = Convert.ToInt32(dt.Rows[0]["mstcode"]);
                    frm.compcode = Convert.ToInt32(SessionMaster.CompCode);
                    frm.msttype = frm.type = MstType;
                    frm.mstdate = Convert.ToDateTime(dt.Rows[0]["mstDate"]);
                    frm.sMstdate = Convert.ToDateTime(dt.Rows[0]["mstdate"]).ToString("dd/MM/yyyy");

                    //frm.mstsale = Convert.ToInt32(dt.Rows[0]["mstsale"]);
                 if (dt.Rows[0]["mstctyp"].ToString() !="")   frm.mstctyp = Convert.ToInt32(dt.Rows[0]["mstctyp"].ToString());

                 if (MstType == 42)
                     ViewBag.mstsale = new SelectList(from res in db.accounts where (res.acctgrou == 40 || res.acctgrou == 9) && res.compcode == comp orderby res.acctsort select new { res.acctcode, res.acctname }, "acctcode", "acctname", dt.Rows[0]["mstsale"]);
                 else
                     ViewBag.mstsale = new SelectList(from res in db.accounts where (res.acctgrou == 41 || res.acctgrou == 10) && res.compcode == comp orderby res.acctsort select new { res.acctcode, res.acctname }, "acctcode", "acctname", dt.Rows[0]["mstsale"]);

                     
                    //ViewBag.Exp1 = new SelectList(from res in db.accounts where (res.acctgrou == ConstVariable.TypeCode_IndirectExpenses || res.acctgrou == ConstVariable.TypeCode_Taxes || res.acctgrou == ConstVariable.TypeCode_DirectExpenses) && res.compcode == comp orderby res.acctname select new { res.acctcode, res.acctname }, "acctcode", "acctname");


                    //DataTable dtUnit;

                    //List<clsPoItem> lstUnit = new List<clsPoItem>();
                    //dtUnit = oSubmit.GetUnit(SessionMaster.CompCode);

                    //clsPoItem sm = new clsPoItem();

                    //for (int i = 0; i < dtUnit.Rows.Count; i++)
                    //{
                    //    sm = new clsPoItem();
                    //    sm.unitname = dtUnit.Rows[i]["unitname"].ToString();
                    //    sm.unitcode = dtUnit.Rows[i]["unitcode"].ToString();
                    //    lstUnit.Add(sm);
                    //}

                    //ViewBag.UnitID = new SelectList(lstUnit, "unitcode", "unitname");

                    //if (dt.Rows[0]["mstctyp"].ToString() != "")
                    //    ViewBag.mstctyp = new SelectList(from res in db.studdet1s where res.studType == 387 select new { res.studCode, res.studName }, "studcode", "studname", dt.Rows[0]["mstctyp"].ToString());
                    //else
                    //    ViewBag.mstctyp = new SelectList(from res in db.studdet1s where res.studType == 387 select new { res.studCode, res.studName }, "studcode", "studname");

                    //ViewBag.SubCate = new SelectList(from res in db.itgroups where res.compcode == SessionMaster.CompCode orderby res.itgpname select new { res.itgpcode, res.itgpname }, "itgpcode", "itgpname");

                    //ViewBag.itemnumb = new SelectList(from res in db.studdet1s where res.studType == 61 select new { res.studCode, res.studName }, "studcode", "studname");

                   // ViewBag.TaxHead_I = new SelectList(from res in db.accounts where (res.acctgrou == ConstVariable.TypeCode_Taxes) && res.compcode == comp select new { res.acctcode, res.acctname }, "acctcode", "acctname");

                    return View("Create", frm);
                }
                else
                {
                    var MaxMstCode = 0;
                    MaxMstCode = (db.jourmsts.Where(x => x.compcode == SessionMaster.CompCode).Where(x => x.msttype == MstType).Max(x => x.mstcode));
                    if (mstCode > MaxMstCode)
                    {
                        TempData["lstRec"] = "This is last record!!!";
                        return RedirectToAction("Create", "PuSL", new { comp = comp,   MstType = MstType, mstcode_Print = mstcode_Print, mencode = mencode });
                    }
                    else
                    {
                        TempData["lstRec"] = "Record Not Found ...";

                        //frm.msttimes = DateTime.Now.ToString("HH:mm:ss");
                        ViewBag.MstCode =frm.mstcode = mstCode;
                        frm.compcode =  Convert.ToInt32(SessionMaster.CompCode);
                        frm.msttype = frm.type = MstType;
                        frm.mstdate = DateTime.Now.Date; 
                        return View("Create", frm);
                        //return RedirectToAction("Create", "PuSL", new { comp = comp, MstCode = mstCode  , MstType = MstType, mstcode_Print = mstcode_Print, mencode = mencode });
                    }
                     
                }
                #endregion
            }
            else
            {
                #region "Create"
                
                //frm.msttimes = DateTime.Now.ToString("HH:mm:ss");
                DataTable dt2;

                dt2 = oSubmit.GetData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from jourmst where compcode ='" + comp + "' and msttype ='" + MstType + "'", true);
                ViewBag.MstCode = frm.mstcode = Convert.ToInt32(dt2.Rows[0]["mstcode"]);
                frm.compcode = Convert.ToInt32(SessionMaster.CompCode);
              frm.msttype =  frm.type = MstType;
                frm.mstdate = DateTime.Now.Date;

                //if (MstType == 42)
                //    ViewBag.mstsale = new SelectList(from res in db.accounts where (res.acctgrou == 40 || res.acctgrou == 9) && res.compcode == comp orderby res.acctsort select new { res.acctcode, res.acctname }, "acctcode", "acctname");
                //else
                //    ViewBag.mstsale = new SelectList(from res in db.accounts where (res.acctgrou == 41 || res.acctgrou == 10) && res.compcode == comp orderby res.acctsort select new { res.acctcode, res.acctname }, "acctcode", "acctname");

                 
                   // ViewBag.Exp1 = new SelectList(from res in db.accounts where (res.acctgrou == ConstVariable.TypeCode_IndirectExpenses || res.acctgrou == ConstVariable.TypeCode_Taxes || res.acctgrou == ConstVariable.TypeCode_DirectExpenses) && res.compcode == comp orderby res.acctname select new { res.acctcode, res.acctname }, "acctcode", "acctname");

                //ViewBag.TaxHead_I = new SelectList(from res in db.accounts where (res.acctgrou == ConstVariable.TypeCode_Taxes) && res.compcode == comp select new { res.acctcode, res.acctname }, "acctcode", "acctname");

                //DataTable dtUnit;

                //List<clsPoItem> lstUnit = new List<clsPoItem>();
                //dtUnit = oSubmit.GetUnit(SessionMaster.CompCode);

                //clsPoItem sm = new clsPoItem();
                //// sm.unitname = "--Select--"; sm.unitcode = "0~0"; lstUnit.Add(sm);

                //for (int i = 0; i < dtUnit.Rows.Count; i++)
                //{
                //    sm = new clsPoItem();
                //    sm.unitname = dtUnit.Rows[i]["unitname"].ToString();
                //    sm.unitcode = dtUnit.Rows[i]["unitcode"].ToString();
                //    lstUnit.Add(sm);
                //}
                 
                frm.mstcode_Print = mstcode_Print;
               // ViewBag.UnitID = new SelectList(lstUnit, "unitcode", "unitname");

                //ViewBag.mstctyp = new SelectList(from res in db.studdet1s where res.studType == 387 select new { res.studCode, res.studName }, "studcode", "studname");
                //ViewBag.mstctyp = new SelectList(from res in db.studdets where res.studType == 387 select new { (int?)res.studCode, res.studName }, "studcode", "studname");
                //ViewBag.SubCate = new SelectList(from res in db.itgroups where res.compcode == SessionMaster.CompCode orderby res.itgpname select new { res.itgpcode, res.itgpname }, "itgpcode", "itgpname");


                //ViewBag.itemnumb = new SelectList(from res in db.studdet1s where res.studType == 61 select new { res.studCode, res.studName }, "studcode", "studname");
                 

                #endregion
            }
            return View("Create", frm);
            }
            catch
            {
                return View();
            }
        }
        //[HttpPost]
        //public ActionResult Create(Jourmaster oCls)
        //{
        //    if (Convert.ToInt16(SessionMaster.CompCode) == 0) { return View("LogOff", "Home"); } 

        //    clsSubmitModel oSubmit = new clsSubmitModel();
        //    commFunction oCom = new commFunction();
        //    try
        //    {
        //        jourtrn oTrn = new jourtrn();
        //        sapuitd oSapu = new sapuitd();
        //        gathdet oGath = new gathdet();

        //        string sGathCode = "";
        //        string sItmTbl = "itpursal"; // jab delivery challan se stock update hota he toh sales bill me sapuitd nahi toh itpursal 28082017

        //        DataTable dt2;

        //        //dt2 = oSubmit.GetData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from jourmst where compcode ='" + comp + "' and msttype ='" + MstType + "'");
                 
        //        if (Request.QueryString["MstCode"] != null && Convert.ToInt32(Request.QueryString["MstCode"]) > 0) { } else
        //        {
        //            oCls.mstcode = Convert.ToInt32( oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from jourmst where compcode ='" + SessionMaster.CompCode + "' and msttype ='" + oCls.msttype + "'" ,"0" ,true));
        //        }
 
        //        var json = oCls.sItemDet;
        //        clsPoItem ItemDet = JsonConvert.DeserializeObject<clsPoItem>(json);

        //        ViewBag.MstType = oCls.msttype;
        //        int iDays = 0;
        //        decimal iComm = 0, iInterest = 0, mstperd = 0;

        //        oCls.mstdate = oSubmit.GetDateFormat(oCls.sMstdate);

        //        if (oCls.mstdued > 0) iDays = (int)oCls.mstdued;//DueDay 170831
        //        if (oCls.Comm > 0) iComm = oCls.Comm;
        //        if (oCls.Interest > 0) iInterest = oCls.Interest;

        //        if (oCls.mstchnH == null ) oCls.mstchnH = "";
        //        oCls.mststat = 0;
        //        //oCls.msttime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
        //        oCls.mstrefc = getParty(Convert.ToInt32(oCls.PartyID)) + "~" + iInterest + "~" + iDays + "~" + iComm;
        //        if (oCls.msttype == 42)
        //        {
        //            oCls.mstchno = oCls.mstchnH + oCls.mstchnV;//170830
        //            oCls.mstexti = "~~#0";//170830
        //        }
        //        else
        //        {
        //            oCls.mstexti = "~";//170830
        //            oCls.mstchnV=0;
        //        }
        //        oCls.mstgncd = "0~0~" + oCls.acctledg + "~0";//170830

                

        //        oCls.mstAppr = 0;
        //        oCls.mstqtyd = 0;
        //        oCls.mstvat1 = 0;
        //        oCls.mstvat2 = 0;
        //        oCls.mstvat3 = 0;
        //        oCls.mstchnm = "";
        //        oCls.oldwht = 0;
        //        oCls.mstsite = 0;
        //        oCls.mstbrnc = 0;
        //        oCls.mstsubt = 0;
        //        oCls.mstcust = 0;
        //        oCls.mstprtc = oCls.PartyID;
        //        oCls.msternv = "";
        //        oCls.mstrvsc = 0;
        //        oCls.mstcurrcd = 1;
        //        oCls.mstcurrrat = 1;
        //        oCls.mstintr = 0;

        //        oCls.mstDueDate = oCls.mstdate.AddDays(iDays);//170831
        //        oCls.mstbuyerc = 0;
        //       // oCls.mstperd = 0;
        //        oCls.mstdsptoc = 0;
        //        if (ItemDet.LstItem.Count>0 && ItemDet.LstItem[0].GSTStateVal == "1")
        //            oCls.mstIorL = "L";//I // 2 By Ajay On 20032018
        //        else
        //            oCls.mstIorL = "I";//L // 2 By Ajay On 20032018

        //        oSubmit.BeginTran();
        //        //oSubmit.InsJourmst(oCls);

        //        oSubmit.insertData("delete from jourtrn  where compcode = " + oCls.compcode + " and trntype = " + oCls.msttype + " and  trncode = " + oCls.mstcode + " and  trnDate = '" + oCls.mstdate.ToString("yyyy-MM-dd") + "'");

        //        oSubmit.insertData("delete b from " + sItmTbl + " a inner join GathDet b on a.itdgath = b.gathCode and a.Itdtype =b.Itdtype where a.Itdtype = " + oCls.msttype + " and Itdcode = " + oCls.mstcode);

        //        oSubmit.insertData("Delete from " + sItmTbl + " where CompCode = " + oCls.compcode + " and Itdtype = " + oCls.msttype + " and Itdcode = " + oCls.mstcode);

        //        for (int i = 0; i < ItemDet.LstItem.Count; i++)
        //        {

        //            //var q = from a in db.GetTable<charcodege>() select a; 
        //            //var w = from res in db.charcodeges select new { res.gatcode };

        //            sGathCode = oSubmit.GetUsWoCode();

        //            oSapu.compcode = Convert.ToInt16(oCls.compcode);
        //            oSapu.itdtype = Convert.ToInt32(oCls.msttype);
        //            oSapu.itdcode = Convert.ToInt32(oCls.mstcode);
        //            oSapu.itdtime = Convert.ToInt32(oCls.msttime);
        //            oSapu.itdsrno = Convert.ToInt16(i + 1);
        //            oGath.gathpuri = oGath.gathstat = Convert.ToString(oSapu.itddate = Convert.ToDateTime(oCls.mstdate));
        //            //oSapu.itdtime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
        //            oSapu.itditem = Convert.ToInt32(ItemDet.LstItem[i].ItemID);
        //            oGath.gathdesc = oSapu.itdrema = "";// ItemDet.LstItem[i].Remark.ToString();// ItemRemark.ToString();
        //            //oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].Qty);
        //            if (ItemDet.LstItem[i].UnitID.ToString() != "") oSapu.itdunit = Convert.ToInt32(ItemDet.LstItem[i].UnitID); else oSapu.itdunit = 0;
        //            oSapu.itddime = ItemDet.LstItem[i].Rate.ToString();
                     
        //            oSapu.itdvate = oSapu.itdrate = Convert.ToDecimal(ItemDet.LstItem[i].Rate);
                    
        //             Convert.ToDecimal(oSapu.itdvate);
        //            oSapu.itdamou = Convert.ToDecimal(ItemDet.LstItem[i].Amt);
        //            //oSapu.itdtowt = 0;//Convert.ToInt32(ItemDet.LstItem[i].Qty);//170830
        //            if (oCls.msttype == 42) oSapu.itdpenq = oSapu.itdquan = -Convert.ToInt32(ItemDet.LstItem[i].Qty);
        //            else oSapu.itdpenq = oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].Qty);
        //            //oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].Qty);//170830
        //            oSapu.itdpkin = 0;
        //            oSapu.itdmill = 1;
        //            oSapu.itdgodo = 1;
                    
        //            oSapu.itdlswt= 0;//170831
                     
                  
        //            oSapu.itdlabonw	= 0;//170831
        //            oSapu.itdvatinc	= 0;//170831
        //           // oSapu.itdcasert= 0;//170831

        //            if (ItemDet.LstItem[i].Rate.ToString() != "") oSapu.itdactprc = oGath.gathwtdi = Convert.ToDecimal(ItemDet.LstItem[i].Rate.ToString());//TRate.ToString()); 
        //            if (ItemDet.LstItem[i].DisPer.ToString() != "") oSapu.itddscp = oGath.gathqtdi = Convert.ToDecimal(ItemDet.LstItem[i].DisPer);
        //            if (ItemDet.LstItem[i].DisAmt.ToString() != "") oGath.gathlabp = Convert.ToDouble(ItemDet.LstItem[i].DisAmt);
        //            if (ItemDet.LstItem[i].DisWT.ToString() != "") oGath.gathdion = Convert.ToDecimal(ItemDet.LstItem[i].DisWT);
        //            if (ItemDet.LstItem[i].DisQtyAmt.ToString() != "") oGath.gathcdam = Convert.ToDecimal(ItemDet.LstItem[i].DisQtyAmt);
        //            if (ItemDet.LstItem[i].PerDisTota.ToString() != "") oGath.gathdscv = oSapu.itdperd = Convert.ToDecimal(ItemDet.LstItem[i].PerDisTota);
        //            if (ItemDet.LstItem[i].QtyDis.ToString() != "") oSapu.itdqtyd = Convert.ToDecimal(ItemDet.LstItem[i].QtyDis);

        //            if (ItemDet.LstItem[i].FQty.ToString() != "") oGath.gatlabg = Convert.ToDouble(ItemDet.LstItem[i].FQty);
        //            if (ItemDet.LstItem[i].BilledQty.ToString() != "") oGath.gathwtdf = Convert.ToDecimal(ItemDet.LstItem[i].BilledQty);
        //            if (ItemDet.LstItem[i].itemdisc.ToString() != "")  oSapu.itdtowt =  Convert.ToDecimal(ItemDet.LstItem[i].itemdisc)*oSapu.itdquan ;

        //            oCls.mstperd += oSapu.itdperd;

        //            if (ItemDet.LstItem[i].acctcode.ToString() != "") oSapu.itdinde = Convert.ToInt32(ItemDet.LstItem[i].acctcode);
        //            else oSapu.itdinde = 0; // Ajay on 29012018 For get proper Gst Value in Trn(Output) List in Edit Time HTML Code >> (if (itm[i].acctcode == trn[ii].ItemID))

        //            oGath.gathcode = oSapu.itdgath = sGathCode;
        //            oSapu.itdempo = SessionMaster.UserID;//0;170831 //Subodh Sir Se Puchana hai...
        //            oSapu.itdlabamt = 0;
        //            oGath.gathwast = ItemDet.LstItem[i].TaxPer.ToString();

        //            //{170830
        //            oSapu.itdgstrtv = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer);
        //            if (ItemDet.LstItem[i].GSTStateVal == "1") // 2 By Ajay On 20032018
        //            {
        //                if (Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal) > 0) oSapu.itdcgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal);
        //                else oSapu.itdcgstrt = 0;

        //                oSapu.itdcgstvl = Convert.ToDecimal(ItemDet.LstItem[i].Amt) * oSapu.itdcgstrt / 100;
        //                if (Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal) > 0) oSapu.itdsgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal);
        //                else oSapu.itdsgstrt = 0;

        //                oSapu.itdsgstvl = Convert.ToDecimal(ItemDet.LstItem[i].Amt) * oSapu.itdsgstrt / 100;
        //                oSapu.itdigstrt = 0;
        //                oSapu.itdigstvl = 0;
        //            }
        //            else
        //            {
        //                oSapu.itdcgstrt = 0;
        //                oSapu.itdcgstvl = 0;
        //                oSapu.itdsgstrt = 0;
        //                oSapu.itdsgstvl = 0;
        //                //if (Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal) > 0) oSapu.itdigstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal); else oSapu.itdigstrt = 0;   By Ajay On 21032018

        //                if (Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal) > 0) oSapu.itdigstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer);
        //                else oSapu.itdigstrt = 0;

        //                oSapu.itdigstvl = Convert.ToDecimal(ItemDet.LstItem[i].Amt) * oSapu.itdigstrt / 100;

        //            }
        //            oSapu.itdcessrt = Convert.ToDecimal(0);
        //            oSapu.itdcessvl = Convert.ToDecimal(0);
        //            oSapu.itdugstrt = Convert.ToDecimal(0);
        //            oSapu.itdugstvl = Convert.ToDecimal(0);
        //            //}170830
        //            oSubmit.insSapuItd(oSapu, sItmTbl);
        //            oSubmit.insGathDet(oGath);

        //        }

        //        oSubmit.InsJourmst(oCls);

        //        ItemDet = JsonConvert.DeserializeObject<clsPoItem>(oCls.sTrnDet);
        //        for (int i = 0; i < ItemDet.LstItem.Count; i++)
        //        {
        //            if (ItemDet.LstItem[i].ItemID > 0)
        //            {
        //                oTrn.compcode = Convert.ToInt16(oCls.compcode);
        //                oTrn.trntype = oCls.msttype;
        //                oTrn.trncode = oCls.mstcode;
        //                oTrn.trntime = oCls.msttime;
        //                oTrn.trnsrno = Convert.ToInt16(i + 1);
        //                oTrn.trndate = oCls.mstdate;
        //                oTrn.trnitem = ItemDet.LstItem[i].ItemID;

        //                if (ItemDet.LstItem[i].trnledg.ToString() != "") oTrn.trnledg = ItemDet.LstItem[i].trnledg; else oTrn.trnledg = 0;

        //                if (ItemDet.LstItem[i].AdjuAmt.ToString() != "") oTrn.trnadju = Convert.ToDecimal(ItemDet.LstItem[i].AdjuAmt); else oTrn.trnadju = 0;

        //                if (oCls.msttype == 42)
        //                {
        //                    oTrn.trndram = ItemDet.LstItem[i].tpDrAmt;
        //                    oTrn.trncram = ItemDet.LstItem[i].tpCrAmt;
        //                }
        //                else//Purchase
        //                {
        //                    oTrn.trndram = ItemDet.LstItem[i].tpCrAmt;
        //                    oTrn.trncram = ItemDet.LstItem[i].tpDrAmt;
        //                }
        //                oTrn.trninde = ItemDet.LstItem[i].trninde;//170829
        //                oTrn.trnexpa = ItemDet.LstItem[i].trnexpa;//170830
        //                oTrn.trntagv = ItemDet.LstItem[i].trntagv;//170830
        //                if (ItemDet.LstItem[i].trnaddv.ToString() != "") oTrn.trnaddv = Convert.ToDecimal(ItemDet.LstItem[i].trnaddv);//170830

        //                oSubmit.InsJourTrn(oTrn);
        //            }
        //        }

        //        int iMode = 1;

        //        if (oCls.IsEdit != true)
        //        {
        //            iMode = 0;
        //            oCls.StDate = Convert.ToDateTime(oCom.getOpenDate(DateTime.Now.Date));
        //            oCls.LastDate = Convert.ToDateTime(oCom.getClosDate(DateTime.Now.Date));
        //            oSubmit.UpdCodeGen(oCls);
        //        }

        //        //********************************* User Work ********************************* 
        //        clsUserWork oUser = new clsUserWork();
        //        oUser.woruser = SessionMaster.UserID;
        //        oUser.wormode = iMode;
        //        oUser.worcomp = SessionMaster.CompCode;
        //        oUser.wortype = oCls.msttype;
        //        oUser.worcode = oCls.mstcode;
        //        oUser.worsrno = oSubmit.GetUsWoCode(); 
        //        oUser.worrema = "D-" + oCls.acctname + "#" + oCls.sMstdate + "#" + oCls.mstrema;
        //        oUser.wordate = oCls.mstdate;
        //        oUser.worrfsr = "";
        //        oUser.worsysn = Environment.MachineName;
        //        oUser.IP_Add = this.Request.UserHostAddress;
        //        oUser.WorChNo = oCls.mstchno;
        //        oUser.WorNarr = oCls.mstrema;
        //        oSubmit.InsUserWork(oUser);
        //        //********************************* User Work ********************************* 

        //        oCls.mstcode_Print = oCls.mstcode;
        //        oSubmit.Commit();

        //        oCls.IsPrint = 1;

        //        TempData["message"] = "Saved Successfully ...";
        //        oCls.mstcode = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        oSubmit.RollBack();
        //        TempData["message"] = ex.Message;
        //        //return View();
        //        return RedirectToAction("Create", new { MstType = oCls.msttype, MstCode = 0, comp = oCls.compcode, MenCode = Request.QueryString["MenCode"] });
        //    }
        //    return RedirectToAction("Create", new { MstType = oCls.msttype, MstCode = oCls.mstcode, comp = oCls.compcode, mstcode_Print = oCls.mstcode_Print, MenCode = Request.QueryString["MenCode"] });
        //}

        private string getParty(int iParty)
        {
            if (iParty.ToString().Length == 1)
                return "00000" + iParty;
            else if (iParty.ToString().Length == 2)
                return "0000" + iParty;
            else if (iParty.ToString().Length == 3)
                return "000" + iParty;
            else if (iParty.ToString().Length == 4)
                return "00" + iParty;
            else if (iParty.ToString().Length == 5)
                return "0" + iParty;
            else
                return iParty.ToString();
        }
        public ActionResult GetPuSLDet(int iMstcode, int iCompcode, int iType)
        {

            clsSubmitModel oSubmit = new clsSubmitModel();

            DataTable dt;
            dt = oSubmit.GetVoucher(iType, iCompcode, iMstcode);

            List<clsPoItem> oList = new List<clsPoItem>();
            List<clsPoItem> oExpList = new List<clsPoItem>();
            List<clsPoItem> oItemList = new List<clsPoItem>();
            clsPoItem oOrder1 = new clsPoItem();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                oOrder1 = new clsPoItem();
                oOrder1.ItemID = Convert.ToInt32(dt.Rows[i]["tpPartyID"]);
                oOrder1.trnItem = dt.Rows[i]["partyname"].ToString();
                oOrder1.AdjuAmt = Convert.ToDecimal(dt.Rows[i]["trnAdju"]);
                oOrder1.tpDrAmt = Convert.ToDecimal(dt.Rows[i]["tpDrAmt"]);
                oOrder1.tpCrAmt = Convert.ToDecimal(dt.Rows[i]["tpCrAmt"]);

                oOrder1.trninde = Convert.ToInt32(dt.Rows[i]["trninde"]);
                oOrder1.trnexpa = Convert.ToDecimal(dt.Rows[i]["trnexpa"]);
                oOrder1.trntagv = Convert.ToInt32(dt.Rows[i]["trntagv"]);
                oOrder1.trnaddv = Convert.ToDecimal(dt.Rows[i]["trnaddv"]);
                  
                oList.Add(oOrder1);

                if (dt.Rows[i]["trntagv"].ToString() == "1")
                { 
                    oExpList.Add(oOrder1);
                }

            }

            DataTable dtExp;
            dt.DefaultView.RowFilter = "trntagv = 1";
            dtExp = dt.DefaultView.ToTable();


            var ItmSapu = from J in db.jourmsts
                          join a in db.itpursals on new { _Comp = J.compcode, _Type = (int)J.msttype, _Code = J.mstcode } equals new { _Comp = a.compcode, _Type = a.itdtype, _Code = a.itdcode }
                          join b in db.itemains on new { Sapu_Comp = a.compcode, Sapu_Item = a.itditem } equals new { Sapu_Comp = b.compcode, Sapu_Item = b.itemcode }
                          join e in db.itgroups on new { _Comp = b.compcode, _grp = (int)b.itemgrou } equals new { _Comp = e.compcode, _grp = e.itgpcode }
                          join c in db.unitdets on new { Sapu_Comp = e.compcode, Sapu_Item = (int)e.itgpbasi } equals new { Sapu_Comp = (int)c.compcode, Sapu_Item = (int)c.unitcode } into tmpUnit
                          from c in tmpUnit.DefaultIfEmpty()
                          join G in db.gathdets on a.itdgath equals G.gathcode into tmpGath
                          from G in tmpGath.DefaultIfEmpty()
                          join d in db.accounts on b.itemname equals d.acctname into tmpAcct
                          from d in tmpAcct.DefaultIfEmpty()

                          where a.itdcode == iMstcode && a.itdtype == iType && a.compcode == iCompcode
                          select new
                          {
                              ItemID = a.itditem,
                              ddlItem = b.itemname,
                              Qty = Math.Abs(a.itdquan),
                              Rate = a.itdrate,
                              Amt = a.itdamou,
                              Remark = a.itdrema,
                              Cases = 0,
                              UnitID = a.itdunit,
                              unitname = c.unitname == null ? "" : c.unitname,
                              TaxPer = a.itdgstrtv == null ? 0 : a.itdgstrtv,
                              Tax_Amt = (a.itdamou * (a.itdgstrtv == null ? 0 : a.itdgstrtv)) / 100,
                              //ItemNetAmt: $('#ItemNetAmt').val(),
                              acctcode = a.itdinde,// (int?) d.acctcode, 
                              ItemRemark = a.itdrema,
                              ItemNetAmt = a.itdamou + (a.itdamou * (a.itdgstrtv == null ? 0 : a.itdgstrtv)) / 100,
                              GSTStateVal = J.mstIorL == "I" ? 2 : 1,
                              TRate = G.gathwtdi == null ? 0 : G.gathwtdi,
                              DisPer = G.gathqtdi == null ? 0 : G.gathwtdi,

                              DisAmt = G.gathlabp == null ? 0 : G.gathlabp,
                              DisWT = G.gathdion == null ? 0 : G.gathdion,
                              DisQtyAmt = G.gathcdam == null ? 0 : G.gathcdam,
                              PerDisTota = G.gathdscv == null ? 0 : G.gathdscv,
                              QtyDis = a.itdqtyd == null ? 0 : a.itdqtyd,
                              FQty = G.gatlabg == null ? 0 : G.gatlabg,
                              BilledQty = G.gathwtdf == null ? 0 : G.gathwtdf
                              //ListArea= null,  
                          };  
                          
            //  select unitCode ,unitname ,* from unitdet

            var sData = new { oList = oList , ExpList = oExpList, oItemList = ItmSapu }; 
            return Json(Json(sData).Data, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AddAccount(string Name, string Address, string Mobile, string GSTIN, int AcGroup, int State, int City)
        {  
            var max = 0;
            try
            {
                clsSubmitModel oSubmit = new clsSubmitModel();
                if (Convert.ToInt16(SessionMaster.CompCode) == 0) { Response.Redirect("~/Index/Index"); } 
                max = db.accounts.Max(i => i.acctcode);
                max = max + 1;

                oSubmit.insertData("Insert into account (acctcode , compcode ,Acctname ,AcctAddr ,acctgstin ,acctphon ,acctgrou,acctstat ,acctcity )Values (" + max + "," + SessionMaster.CompCode + ",'" + Name + "','" + Address + "','" + GSTIN + "','" + Mobile + "' , " + AcGroup + " ," + State + " ," + City + " ) ");
            } 
            catch (Exception ex)
            {
                return Json(Json(0).Data, JsonRequestBehavior.AllowGet);
            }

            return Json(Json(max).Data, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ChkGST(int acctCode , int SaleType   )
        {
            DataTable dt = new DataTable();
            if (Convert.ToInt16(SessionMaster.CompCode) == 0) { Response.Redirect("~/Index/Index"); } 

                clsSubmitModel oSubmit = new clsSubmitModel();
                dt = oSubmit.GetData("exec SP_ChkGST @acctCode=" + acctCode + ",@CompCode=" + SessionMaster.CompCode + ",@SaleType=" + SaleType + ", @acctComp=" + SessionMaster.compunde, true);
                                           
                  var  ITEMS = new { HeadingV = dt.Rows[0]["HeadingV"].ToString(), ErrorVal = dt.Rows[0]["ErrorVal"].ToString()};
            
            return Json(ITEMS, JsonRequestBehavior.AllowGet); 
        }
        public ActionResult AddItem(string Name, int itemgrou, int itemnumb, decimal Vat, string HSN, decimal itemgstr, string itemcess)
        {
            var itemcode = 0; var itemsrno = 0;
            try
            {
                clsSubmitModel oSubmit = new clsSubmitModel();
 
                itemcode = (db.itemains.Where(i => i.compcode == SessionMaster.CompCode).Max(i => (int?)i.itemcode) ?? 0) + 1; 

                itemsrno = (db.itemains.Where(x => x.compcode == SessionMaster.CompCode).Where(x => x.itemgrou == itemgrou).Max(x => x.itemsrno) ?? 0)+1;
              
                oSubmit.insertData(" Insert into itemain(compcode ,itemcode ,itemname ,itemgrou ,itemnumb ,itemsrno ,itemmini ,itemmaxi, itemsort ,itemdisc ,itemalia , itemtext ,itemvatr,itemhind,itemtype,itemhsnc,itemgstr,itemcess)      VALUES(" + SessionMaster.CompCode + "," + itemcode + ",'" + Name + "'," + itemgrou + "," + itemnumb + "," + itemsrno + " ,0," + itemnumb + ",0 ,0,'',''," + Vat + ",'','General','" + HSN + "'," + itemgstr + "," + itemcess + ")   ");

                oSubmit.insertData(" insert into iteminfo(compcode,itemcode,itemdesc,itemalia ,itgpbcqt,igs2 ,itemhsnc ,itemgstr ,itemdisc,igc1,igc2) values(" + SessionMaster.CompCode + "," + itemcode + ",'" + Name + "','',1,0,'" + HSN + "'," + itemgstr + ",0,0," + itemgrou + ")");
            }
            catch (Exception ex)
            {
                return Json(Json(0).Data, JsonRequestBehavior.AllowGet);
            }

            return Json(Json(itemcode).Data, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Getqty(int partyid, int iType, int Qtyid)
        {
            Jourmaster oProp = new Jourmaster();
            var ITEMS = 0;
            int iTypes = Convert.ToInt32(113);
            clsSubmitModel oSubmit = new clsSubmitModel();
            //DataTable dt = oSubmit.GetData("SELECT isnull(sum(itdquan),0) as qty  FROM itpursal Where compcode ='" + Convert.ToInt32(SessionMaster.CompCode) + "' AND itditem ='" + partyid + "' and itdtype ='" + iTypes + "'");
            //if (Convert.ToInt32(dt.Rows[0]["qty"]) >= Qtyid)
            //{
            //    ITEMS = Convert.ToInt32(dt.Rows[0]["qty"]);
            //}
            //else
            //{
            //    if (iType == 42)
            //        ITEMS = 1;
            //    else
            //        ITEMS = 0;

            ITEMS =Convert.ToInt32( oSubmit.GetSingleData("SELECT isnull(sum(itdquan),0) as qty  FROM itpursal Where compcode =" + Convert.ToInt32(SessionMaster.CompCode) + " AND itditem =" + partyid ,"0",true )) ;
             
            return Json(ITEMS , JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Getpartid(int partyid)
        {
            //Jourmaster oProp = new Jourmaster();
            //var Items1 = ""; var Items2 = "";
            //clsSubmitModel oSubmit = new clsSubmitModel();
            //DataTable dt = oSubmit.GetData("select  isnull( max(acctpinc) ,Upper(substring(Max(Acctname) ,0,3))) Head, isnull(  Max(b.mstchnV),0)+1 Value from Account a Left join JourMst b on a.acctpinc = b.mstchnH   Where a.compcode ='" + Session["CompCode"].ToString() + "' AND  acctcode='" + partyid + "'");
            //if (dt.Rows.Count > 0)
            //{
            //    string head = dt.Rows[0]["Head"].ToString();
            //    string Value = dt.Rows[0]["Value"].ToString();
            //    Items1 = head;
            //    Items2 = Value;
            //}
            //else
            //{
            //    //  ITEMS = 0;
            //}
            if (Convert.ToInt16(SessionMaster.CompCode) == 0) { Response.Redirect("~/Index/Index"); } 

clsSubmitModel oSubmit = new clsSubmitModel();
            string sHead = ""; int iValue = 0;

            sHead = oSubmit.GetSingleData("select top 1 case when b.acctpinc <> '' and b.acctpinc is not null then b.acctpinc else mstchnH end Head from Jourmst  a  left Join Account b on b.acctcode = a.MstSale and a.compcode =b.compcode  Where a.compcode = " + SessionMaster.CompCode + "  and MstSale="+ partyid ,"" ,true).ToString(); 

            iValue = (db.jourmsts.Where(x => x.compcode == SessionMaster.CompCode).Where(x => x.mstchnH  == sHead).Max(x => x.mstchnV) ?? 0) + 1;
             
            var ITEMS = new { Items11 = sHead, Items12 = iValue };
            return Json(ITEMS, JsonRequestBehavior.AllowGet);
        }     

        public JsonResult getNextPrevRec(int CompCode, int MstType, int MstCode )
        {
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Create", "PuSL", new { MstType = MstType, MstCode = MstCode, comp = CompCode, mstcode_Print = 0 });
            return Json(redirectUrl, JsonRequestBehavior.AllowGet); 
        }
         
        public ActionResult GSTApply(int SaleType)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;

            dt = oSubmit.GetData("select acctcform from account where Compcode = " + SessionMaster.CompCode + " and acctcode = " + SaleType , true);
            return Json(dt.Rows[0]["acctcform"], JsonRequestBehavior.AllowGet);

        }

        //public JsonResult getTaxRate(decimal Rate)
        //{
        //    var iTax = from a in db.getTaxRateOnPrice(Rate) select new { a.taxRate };
        //    return Json(iTax, JsonRequestBehavior.AllowGet);
             
        //}

//        #region "Porduction"


//        public ActionResult PorductionList()
//        {
//            clsSubmitModel oSubmit = new clsSubmitModel();
//            DataTable dt;
//            int comp = Convert.ToInt32(SessionMaster.CompCode);
//            if (comp == 0)
//            {
//                TempData["message"] = "Please Select Company ...";
//            }

//            ViewBag.MstType = Request.QueryString["MstType"];

//            if (Request.QueryString["MstType"].ToString() == "74")
//                dt = oSubmit.GetProduction(Convert.ToInt32(Request.QueryString["MstType"]), Convert.ToInt32(SessionMaster.CompCode));
//            else
//                dt = oSubmit.GetVoucher(Convert.ToInt32(Request.QueryString["MstType"]), Convert.ToInt32(SessionMaster.CompCode));

//            var ItmSapu = from a in db.ordemsts
//                          where a.msttype == Convert.ToInt32(Request.QueryString["MstType"]) && a.compcode == SessionMaster.CompCode
//                          select new
//                          {
//                              mstcode = a.mstcode,
//                              mstdate = a.mstdate,
//                              msttota = a.msttota,
//                              mstChno = a.mstchno,
//                              compcode = a.compcode,
//                              msttype = a.msttype,
//                          };

//            Jourmaster oModel = new Jourmaster();
//            List<Jourmaster> oList = new List<Jourmaster>();
//            for (int i = 0; i < dt.Rows.Count; i++)
//            {
//                oModel = new Jourmaster();
//                oModel.mstcode = Convert.ToInt32(dt.Rows[i]["mstcode"]);
//                oModel.mstdate = Convert.ToDateTime(dt.Rows[i]["mstdate"]);
//                oModel.msttota = Convert.ToDecimal(dt.Rows[i]["msttota"]);
//                oModel.mstchno = dt.Rows[i]["mstChno"].ToString();
//                oModel.mstrema = dt.Rows[i]["mstrema"].ToString();
//                oModel.mstclno = dt.Rows[i]["mstclno"].ToString();
//                oModel.mstchnm = dt.Rows[i]["mstchnm"].ToString();
//                oModel.compcode = Convert.ToInt32(dt.Rows[i]["compcode"]);
//                oModel.msttype = Convert.ToInt32(dt.Rows[i]["msttype"]);

//                oList.Add(oModel);
//            }

//            oModel.ListArea = oList;
//            return View(oModel);
//        }

//        [HttpGet]
//        public ActionResult Production(int comp = 0, int id = 0, int mstCode = 0, int MstType = 0)
//        {
//            SessionMaster.CompCode = 6;

//            DBEntity oDB = new DBEntity();
//            comp = SessionMaster.CompCode;
//            if (comp == 0)
//            {
//                TempData["message"] = "Please Select Company ...";
//            }

//            clsSubmitModel oSubmit = new clsSubmitModel();
//            ViewBag.MstType = Request.QueryString["MstType"];
//            Jourmaster frm = new Jourmaster();

//            if (mstCode > 0)
//            {
//                #region  "Edit"
//                DataTable dt;
//                if (MstType == 74)
//                    dt = oSubmit.GetData("select * from ordemst where compcode = " + comp + " and msttype = 74 and mstcode =" + mstCode, true);
//                else
//                    dt = oSubmit.GetVoucher(MstType, comp, mstCode);

//                if (dt.Rows.Count > 0)
//                {
//                    if (dt.Rows[0]["mstyldp"].ToString() != "") frm.Ratio_Org = Convert.ToInt32(dt.Rows[0]["mstyldp"]);
//                    //frm.msttimes = dt.Rows[0]["msttime"].ToString();
//                    frm.mstchnm = dt.Rows[0]["mstchnm"].ToString();
//                    frm.mstchno = dt.Rows[0]["mstchno"].ToString();
//                    if (dt.Rows[0]["mstchnV"].ToString() != "") frm.mstchnV = Convert.ToInt32(dt.Rows[0]["mstchnV"].ToString());
//                    if (dt.Rows[0]["msttota"].ToString() != "") frm.msttota = Convert.ToDecimal(dt.Rows[0]["msttota"].ToString());
//                    frm.mstrema = dt.Rows[0]["mstrema"].ToString();

//                    if (dt.Rows[0]["mstpdc"].ToString() != "") frm.RatioType = Convert.ToInt32(dt.Rows[0]["mstpdc"].ToString());

//                    ViewBag.MstCode = frm.mstcode = Convert.ToInt32(dt.Rows[0]["mstcode"]);
//                    frm.compcode = Convert.ToInt32(dt.Rows[0]["compcode"]);
//                    frm.type = Convert.ToInt32(dt.Rows[0]["msttype"]);
//                    frm.mstdate = Convert.ToDateTime(dt.Rows[0]["mstDate"]);
//                    frm.ChallanDate = Convert.ToDateTime(dt.Rows[0]["mstDate"]).ToString("dd/MM/yyyy");

//                    DataTable dtUnit;

//                    List<clsPoItem> lstUnit = new List<clsPoItem>();
//                    dtUnit = oSubmit.GetUnit(SessionMaster.CompCode);

//                    clsPoItem sm = new clsPoItem();

//                    for (int i = 0; i < dtUnit.Rows.Count; i++)
//                    {
//                        sm = new clsPoItem();
//                        sm.unitname = dtUnit.Rows[i]["unitname"].ToString();
//                        sm.unitcode = dtUnit.Rows[i]["unitcode"].ToString();
//                        lstUnit.Add(sm);
//                    }

//                    ViewBag.UnitID_Exp = ViewBag.UnitID_Pack = ViewBag.UnitID_Fini = ViewBag.UnitID = new SelectList(lstUnit, "unitcode", "unitname");

//                    return View("Production", frm);
//                }
//                #endregion
//            }
//            else
//            {
//                #region "Create"

//                //frm.msttimes = DateTime.Now.ToString("HH:mm:ss");
//                DataTable dt2;
//                string sTable = "";
//                if (MstType == 74)
//                    sTable = "ordemst";
//                else
//                    sTable = "Jourmst";

//                dt2 = oSubmit.GetData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from " + sTable + " where compcode ='" + comp + "' and msttype ='" + MstType + "'", true);
//                ViewBag.MstCode = frm.mstcode = Convert.ToInt32(dt2.Rows[0]["mstcode"]);
//                frm.compcode = Convert.ToInt32(SessionMaster.CompCode);
//                frm.type = MstType;
//                frm.mstdate = DateTime.Now.Date;

//                DataTable dtUnit;

//                List<clsPoItem> lstUnit = new List<clsPoItem>();
//                dtUnit = oSubmit.GetUnit(SessionMaster.CompCode);

//                clsPoItem sm = new clsPoItem();

//                for (int i = 0; i < dtUnit.Rows.Count; i++)
//                {
//                    sm = new clsPoItem();
//                    sm.unitname = dtUnit.Rows[i]["unitname"].ToString();
//                    sm.unitcode = dtUnit.Rows[i]["unitcode"].ToString();
//                    lstUnit.Add(sm);
//                }

//                ViewBag.UnitID_Exp = ViewBag.UnitID_Pack = ViewBag.UnitID_Fini = ViewBag.UnitID = new SelectList(lstUnit, "unitcode", "unitname");

//                #endregion
//            }
//            return View("Production", frm);
//        }

//        [HttpPost]
//        public ActionResult Production(Jourmaster oCls)
//        {
//            clsSubmitModel oSubmit = new clsSubmitModel();
//            commFunction oCom = new commFunction();
//            try
//            {
//                clsPoItem ItemDet = null;
//                jourtrn oTrn = new jourtrn();
//                sapuitd oSapu = new sapuitd();
//                string sTblMst = "";
//                string sTblItd = "";
//                if (oCls.msttype == 74)
//                {
//                    sTblMst = "OrdeMst";
//                    sTblItd = "OrdeItd";
//                }
//                else
//                {
//                    sTblMst = "JourMst";
//                    sTblItd = "ITPurSal";
//                }

//                string sGathCode = "";

//                //var json = oCls.sItemDet;

//                oCls.mstdate = oSubmit.GetDateFormat(oCls.ChallanDate);
//                ViewBag.MstType = oCls.msttype;
//                int iDays = 0;
//                decimal iComm = 0, iInterest = 0;

//                if (oCls.mstdued > 0) iDays = (int)oCls.mstdued;// DueDay;170831
//                if (oCls.Comm > 0) iComm = oCls.Comm;
//                if (oCls.Interest > 0) iInterest = oCls.Interest;

//                if (oCls.Ratio.ToString() != "") oCls.mstyldp = oCls.Ratio;

//                oCls.mststat = 0;
//                //oCls.msttime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
//                oCls.mstrefc = getParty(Convert.ToInt32(oCls.mstcode)) + "~" + iInterest + "~" + iDays + "~" + iComm;
//                oCls.mstpdc = Convert.ToInt32(oCls.RatioType);
//                oSubmit.BeginTran();

//                oSubmit.InsJourmst(oCls, sTblMst);


//                oSubmit.insertData("delete from Ordetrn  where compcode = " + oCls.compcode + " and trntype = " + oCls.msttype + " and  trncode = " + oCls.mstcode);
//                oSubmit.insertData("delete from " + sTblItd + "  where compcode = " + oCls.compcode + " and Itdtype = " + oCls.msttype + " and  itdcode = " + oCls.mstcode);


//                if (oCls.sItemRaw != null)
//                {
//                    ItemDet = JsonConvert.DeserializeObject<clsPoItem>(oCls.sItemRaw);
//                    for (int i = 0; i < ItemDet.LstItem.Count; i++)
//                    {
//                        oSapu.compcode = Convert.ToInt16(oCls.compcode);
//                        oSapu.itdtype = Convert.ToInt32(oCls.msttype);
//                        oSapu.itdcode = Convert.ToInt32(oCls.mstcode);
//                        oSapu.itdtime = Convert.ToInt32(oCls.msttime);
//                        oSapu.itdsrno = Convert.ToInt16(i + 1);
//                        oSapu.itddate = Convert.ToDateTime(oCls.mstdate);
//                        //oSapu.itdtime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
//                        oSapu.itditem = Convert.ToInt32(ItemDet.LstItem[i].ItemID);
//                        oSapu.itdtowt = oSapu.itdpenq = oSapu.itdquan = Convert.ToInt32(-ItemDet.LstItem[i].Qty);
//                        oSapu.itdunit = Convert.ToInt32(ItemDet.LstItem[i].UnitID);
//                        oSapu.itddime = ItemDet.LstItem[i].Rate.ToString();//170830 remove convert
//                        oSapu.itdvate = oSapu.itdrate = Convert.ToDecimal(ItemDet.LstItem[i].Rate);

//                        oSapu.itdamou = Convert.ToInt32(ItemDet.LstItem[i].Amt);

//                        oSapu.itdinde = 0;
//                        if (ItemDet.LstItem[i].Cases.ToString() != "") oSapu.itdpkin = Convert.ToDecimal(ItemDet.LstItem[i].Cases);
//                        oSapu.itdmill = 1; oSapu.itdgodo = 1; oSapu.itdgath = sGathCode;
//                        oSapu.itdempo = 0; oSapu.itdlabamt = 0;
//                        ItemDet.LstItem[i].TaxPer.ToString();
//                        oSubmit.insSapuItd(oSapu, sTblItd);

//                    }
//                }

//                if (oCls.sItemFinish != null)
//                {
//                    ItemDet = JsonConvert.DeserializeObject<clsPoItem>(oCls.sItemFinish);
//                    for (int i = 0; i < ItemDet.LstItem.Count; i++)
//                    {
//                        oSapu.compcode = Convert.ToInt16(oCls.compcode);
//                        oSapu.itdtype = Convert.ToInt32(oCls.msttype);
//                        oSapu.itdcode = Convert.ToInt32(oCls.mstcode);
//                        oSapu.itdtime = Convert.ToInt32(oCls.msttime);
//                        oSapu.itdsrno = Convert.ToInt16(i + 1);
//                        oSapu.itddate = Convert.ToDateTime(oCls.mstdate);
//                        //oSapu.itdtime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
//                        oSapu.itditem = Convert.ToInt32(ItemDet.LstItem[i].ItemID);
//                        oSapu.itdtowt = oSapu.itdpenq = oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].Qty);
//                        oSapu.itdunit = Convert.ToInt32(ItemDet.LstItem[i].UnitID);
//                        oSapu.itddime = ItemDet.LstItem[i].Rate.ToString();// Convert.ToInt32(ItemDet.LstItem[i].Rate);170830
//                        oSapu.itdvate = oSapu.itdrate = Convert.ToDecimal(ItemDet.LstItem[i].Rate);
//                        oSapu.itdinde = 1;
//                        oSapu.itdamou = Convert.ToInt32(ItemDet.LstItem[i].Amt);

//                        if (ItemDet.LstItem[i].Cases.ToString() != "") oSapu.itdpkin = Convert.ToDecimal(ItemDet.LstItem[i].Cases);
//                        oSapu.itdmill = 1; oSapu.itdgodo = 1; oSapu.itdgath = sGathCode;
//                        oSapu.itdpkin = 1;
//                        oSapu.itdempo = 0; oSapu.itdlabamt = 0;
//                        ItemDet.LstItem[i].TaxPer.ToString();
//                        oSubmit.insSapuItd(oSapu, sTblItd);

//                    }
//                }
//                if (oCls.sItemPacking != null)
//                {
//                    ItemDet = JsonConvert.DeserializeObject<clsPoItem>(oCls.sItemPacking);
//                    for (int i = 0; i < ItemDet.LstItem.Count; i++)
//                    {
//                        oSapu.compcode = Convert.ToInt16(oCls.compcode);
//                        oSapu.itdtype = Convert.ToInt32(oCls.msttype);
//                        oSapu.itdcode = Convert.ToInt32(oCls.mstcode);
//                        oSapu.itdtime = Convert.ToInt32(oCls.msttime);
//                        oSapu.itdsrno = Convert.ToInt16(i + 1);
//                        oSapu.itddate = Convert.ToDateTime(oCls.mstdate);
//                        oSapu.itdtime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
//                        oSapu.itditem = Convert.ToInt32(ItemDet.LstItem[i].ItemID);
//                        oSapu.itdtowt = oSapu.itdpenq = oSapu.itdquan = Convert.ToInt32(-ItemDet.LstItem[i].Qty);
//                        oSapu.itdunit = Convert.ToInt32(ItemDet.LstItem[i].UnitID);
//                        oSapu.itddime = ItemDet.LstItem[i].Rate.ToString();// Convert.ToInt32(ItemDet.LstItem[i].Rate);170830
//                        oSapu.itdvate = oSapu.itdrate = Convert.ToDecimal(ItemDet.LstItem[i].Rate);
//                        oSapu.itdinde = 2;
//                        oSapu.itdamou = Convert.ToInt32(ItemDet.LstItem[i].Amt);

//                        if (ItemDet.LstItem[i].Cases.ToString() != "") oSapu.itdpkin = Convert.ToDecimal(ItemDet.LstItem[i].Cases);
//                        oSapu.itdmill = 1; oSapu.itdgodo = 1; oSapu.itdgath = sGathCode;
//                        oSapu.itdempo = 0; oSapu.itdlabamt = 0;
//                        ItemDet.LstItem[i].TaxPer.ToString();
//                        oSubmit.insSapuItd(oSapu, sTblItd);
//                    }
//                }
//                if (oCls.sItemExp != null)
//                {
//                    ItemDet = JsonConvert.DeserializeObject<clsPoItem>(oCls.sItemExp);
//                    for (int i = 0; i < ItemDet.LstItem.Count; i++)
//                    {
//                        if (ItemDet.LstItem[i].ItemID > 0)
//                        {
//                            oTrn.trnsrno = Convert.ToInt16(i + 1);
//                            oTrn.compcode = Convert.ToInt16(oCls.compcode);
//                            oTrn.trntype = Convert.ToInt32(oCls.msttype);
//                            oTrn.trncode = Convert.ToInt32(oCls.mstcode);
//                            oTrn.trntime = Convert.ToInt32(oCls.msttime);
//                            oTrn.trnsrno += 1;
//                            oTrn.trndate = Convert.ToDateTime(oCls.mstdate);

//                            if (ItemDet.LstItem[i].ItemID.ToString() != "") oTrn.trnledg = ItemDet.LstItem[i].trnledg; else oTrn.trnledg = 0;

//                           // oTrn.trnledg = oTrn.trnitem = ItemDet.LstItem[i].ItemID;
//                            oTrn.trnadju = Convert.ToInt32(ItemDet.LstItem[i].Rate);
//                            oTrn.trndram = Convert.ToInt32(ItemDet.LstItem[i].Amt);
//                            oTrn.trncram = 0;
//                            oTrn.trnexpr = ItemDet.LstItem[i].UnitID;
//                            oTrn.trninde = 3;
//                            oSubmit.InsJourTrn(oTrn, true);
//                        }
//                    }
//                }

//                if (oCls.IsEdit != true)
//                {
//                    oCls.StDate = Convert.ToDateTime(oCom.getOpenDate(DateTime.Now.Date));
//                    oCls.LastDate = Convert.ToDateTime(oCom.getClosDate(DateTime.Now.Date));
//                    oSubmit.UpdCodeGen(oCls);
//                }

//                TempData["message"] = "Saved Successfully ...";

//                oSubmit.Commit();
//                oCls.mstcode = 0;

//            }
//            catch (Exception ex)
//            {
//                oSubmit.RollBack();
//                TempData["message"] = ex.Message;
//                return View("Production");
//            }
//            return RedirectToAction("Production", new { MstType = oCls.msttype, MstCode = oCls.mstcode, comp = oCls.compcode });
//        }

//        [HttpGet]
//        public JsonResult GetProduction_a(int iMstcode, int iCompcode, int iType)
//        {
//            var itmExp = from a in db.ordetrns
//                         join b in db.accounts on
//                            new { _Comp = a.compcode, _Code = a.trnitem } equals
//                            new { _Comp = (Int16)b.compcode, _Code = (int)b.acctcode }
//                         where a.trncode == iMstcode && a.trntype == iType && a.compcode == iCompcode
//                         select new
//                         {
//                             ItemID = a.trnitem,
//                             ddlItem = b.acctname,
//                             Rate = a.trnadju,
//                             Amt = a.trndram,
//                             UnitID = a.trnexpr,
//                             unitname = a.trnexpr == 1 ? "%" :
//                             a.trnexpr == 2 ? "/Pack" :
//                             a.trnexpr == 3 ? "Fix" :
//                             a.trnexpr == 4 ? "/Pcs" : ""
//                         };
//            return Json(new { itmExp }, JsonRequestBehavior.AllowGet);
//        }

//        public ActionResult GetProduction(int iMstcode, int iCompcode, int iType)
//        {

//            var x = from a in db.itpursals
//                    where a.itdcode == iMstcode && a.itdtype == iType && a.compcode == iCompcode
//                    select new
//                    {
//                        a.itditem,
//                        a.compcode,
//                        a.itdunit,
//                        a.itdcode,
//                        a.itdtype,
//                        a.itdinde,
//                        a.itdpkin,
//                        a.itdquan,
//                        a.itdrate,
//                        a.itdamou
//                    };


//            if (iType == 74)
//                x = from a in db.ordeitds
//                    where a.itdcode == iMstcode && a.itdtype == iType && a.compcode == iCompcode
//                    select new
//                 {
//                     a.itditem,
//                     a.compcode,
//                     a.itdunit,
//                     a.itdcode,
//                     a.itdtype,
//                     a.itdinde,
//                     a.itdpkin,
//                     a.itdquan,
//                     a.itdrate,
//                     a.itdamou
//                 };

//            //new { _Comp = a.compcode, _Code = a.itditem } equals
//            //new { _Comp = b.compcode, _Code = b.itemcode }

//            var itmRaw = from a in x
//                         join b in db.itemains on
//                            new { _Code = a.itditem } equals
//                            new { _Code = b.itemcode }
//                         join c in db.uniconvs on
//                            new { Sapu_Item = a.itdunit } equals
//                            new { Sapu_Item = c.unitcode } into tmpConv
//                         from c in tmpConv.DefaultIfEmpty()
//                         where a.itdinde == 0
//                         select new
//                         {
//                             ItemID = a.itditem,
//                             ddlItem = b.itemname,
//                             Cases = a.itdpkin,
//                             Qty = Math.Abs(a.itdquan),
//                             Rate = a.itdrate,
//                             Amt = a.itdamou,
//                             UnitID = a.itdunit.ToString(),
//                             unitname = c.unitdesc.ToString()
//                         };
//            var itmFini = from a in x
//                          join b in db.itemains on
//                             new { _Code = a.itditem } equals
//                             new { _Code = b.itemcode }
//                          join c in db.uniconvs on
//                             new { Sapu_Comp = a.compcode, Sapu_Item = a.itdunit } equals
//                             new { Sapu_Comp = c.compcode, Sapu_Item = c.unitcode } into tmpConv
//                          from c in tmpConv.DefaultIfEmpty()
//                          where a.itdinde == 1
//                          select new
//                          {
//                              ItemID = a.itditem,
//                              ddlItem = b.itemname,
//                              Cases = a.itdpkin,
//                              Qty = a.itdquan,
//                              Rate = a.itdrate,
//                              Amt = a.itdamou,
//                              UnitID = a.itdunit.ToString(),
//                              unitname = c.unitdesc.ToString()
//                          };

//            var itmPack = from a in x
//                          join b in db.itemains on
//                             new { _Code = a.itditem } equals
//                             new { _Code = b.itemcode }
//                          join c in db.uniconvs on
//                             new { Sapu_Comp = a.compcode, Sapu_Item = a.itdunit } equals
//                             new { Sapu_Comp = c.compcode, Sapu_Item = c.unitcode } into tmpConv
//                          from c in tmpConv.DefaultIfEmpty()
//                          where a.itdinde == 2
//                          select new
//                          {
//                              ItemID = a.itditem,
//                              ddlItem = b.itemname,
//                              Cases = a.itdpkin,
//                              Qty = Math.Abs(a.itdquan),
//                              Rate = a.itdrate,
//                              Amt = a.itdamou,
//                              UnitID = a.itdunit.ToString(),
//                              unitname = c.unitdesc.ToString()
//                          };


//            var itmExp = from a in db.ordetrns
//                         join b in db.accounts on
//                            new { _Comp = a.compcode, _Code = a.trnitem } equals
//                            new { _Comp = (Int16)b.compcode, _Code = (int)b.acctcode }
//                         where a.trncode == iMstcode && a.trntype == iType && a.compcode == iCompcode
//                         select new
//                         {
//                             ItemID = a.trnitem,
//                             ddlItem = b.acctname,
//                             Rate = a.trnadju,
//                             Amt = a.trndram,
//                             UnitID = a.trnexpr,
//                             unitname = a.trnexpr == 1 ? "%" :
//                             a.trnexpr == 2 ? "/Pack" :
//                             a.trnexpr == 3 ? "Fix" :
//                             a.trnexpr == 4 ? "/Pcs" : ""
//                         };

//            var sData = new { oItmRaw = itmRaw, oItmFini = itmFini, oItmPack = itmPack, oItmExp = itmExp };
//            return Json(Json(sData).Data, JsonRequestBehavior.AllowGet);

//        }

//        public ActionResult GetProduction_Scm(string sSchmNm)
//        {
//            clsSubmitModel oSubmit = new clsSubmitModel();
//            DataTable dt;
//            int iMstcode = 0;
//            decimal iRatio = 0;
//            int iType = 74; int iRatioType = 0;
//            dt = oSubmit.GetData("select mstcode,mstyldp ,mstpdc from ordemst where compcode = " + SessionMaster.CompCode + " and msttype = 74 and mstchnm = '" + sSchmNm + "'", true);
//            if (dt.Rows.Count > 0)
//            {
//                iMstcode = Convert.ToInt32(dt.Rows[0]["mstcode"].ToString());
//                if (dt.Rows[0]["mstyldp"].ToString() != "") iRatio = Convert.ToDecimal(dt.Rows[0]["mstyldp"].ToString());
//                if (dt.Rows[0]["mstpdc"].ToString() != "") iRatioType = Convert.ToInt16(dt.Rows[0]["mstpdc"].ToString());
//            }
//            int iCompcode = SessionMaster.CompCode;
//            var x = from a in db.itpursals
//                    where a.itdcode == iMstcode && a.itdtype == 74 && a.compcode == iCompcode
//                    select new
//                    {
//                        a.itditem,
//                        a.compcode,
//                        a.itdunit,
//                        a.itdcode,
//                        a.itdtype,
//                        a.itdinde,
//                        a.itdpkin,
//                        a.itdquan,
//                        a.itdrate,
//                        a.itdamou,
//                        OrgQty = a.itdquan
//                    };

//            if (iType == 74)
//                x = from a in db.ordeitds
//                    where a.itdcode == iMstcode && a.itdtype == 74 && a.compcode == iCompcode
//                    select new
//                    {
//                        a.itditem,
//                        a.compcode,
//                        a.itdunit,
//                        a.itdcode,
//                        a.itdtype,
//                        a.itdinde,
//                        a.itdpkin,
//                        a.itdquan,
//                        a.itdrate,
//                        a.itdamou,
//                        OrgQty = a.itdquan //OrgQty = (decimal)(0)  
//                    };



//            var itmRaw = from a in x
//                         join b in db.itemains on
//                            new { _Code = a.itditem } equals
//                            new { _Code = b.itemcode }
//                         join c in db.uniconvs on
//                            new { Sapu_Comp = a.compcode, Sapu_Item = a.itdunit } equals
//                            new { Sapu_Comp = c.compcode, Sapu_Item = c.unitcode } into tmpConv
//                         from c in tmpConv.DefaultIfEmpty()
//                         where a.itdinde == 0
//                         select new
//                         {
//                             ItemID = a.itditem,
//                             ddlItem = b.itemname,
//                             Cases = a.itdpkin,
//                             Qty = Math.Abs(a.itdquan),
//                             Rate = a.itdrate,
//                             Amt = a.itdamou,
//                             UnitID = a.itdunit,
//                             unitname = c.unitdesc,
//                             OrgQty = Math.Abs(a.OrgQty)
//                         };
//            var itmFini = from a in x
//                          join b in db.itemains on
//                             new { _Code = a.itditem } equals
//                             new { _Code = b.itemcode }
//                          join c in db.uniconvs on
//                             new { Sapu_Comp = a.compcode, Sapu_Item = a.itdunit } equals
//                             new { Sapu_Comp = c.compcode, Sapu_Item = c.unitcode } into tmpConv
//                          from c in tmpConv.DefaultIfEmpty()
//                          where a.itdinde == 1
//                          select new
//                          {
//                              ItemID = a.itditem,
//                              ddlItem = b.itemname,
//                              Cases = a.itdpkin,
//                              Qty = Math.Abs(a.itdquan),
//                              Rate = a.itdrate,
//                              Amt = a.itdamou,
//                              UnitID = a.itdunit,
//                              unitname = c.unitdesc,
//                              OrgQty = Math.Abs(a.OrgQty)
//                          };

//            var itmPack = from a in x
//                          join b in db.itemains on
//                             new { _Code = a.itditem } equals
//                             new { _Code = b.itemcode }
//                          join c in db.uniconvs on
//                             new { Sapu_Comp = a.compcode, Sapu_Item = a.itdunit } equals
//                             new { Sapu_Comp = c.compcode, Sapu_Item = c.unitcode } into tmpConv
//                          from c in tmpConv.DefaultIfEmpty()
//                          where a.itdinde == 2
//                          select new
//                          {
//                              ItemID = a.itditem,
//                              ddlItem = b.itemname,
//                              Cases = a.itdpkin,
//                              Qty = Math.Abs(a.itdquan),
//                              Rate = a.itdrate,
//                              Amt = a.itdamou,
//                              UnitID = a.itdunit,
//                              unitname = c.unitdesc,
//                              OrgQty = Math.Abs(a.OrgQty)
//                          };


//            var itmExp = from a in db.ordetrns
//                         join b in db.accounts on
//                            new { _Code = a.trnitem } equals
//                            new { _Code = (int)b.acctcode }
//                         where a.trncode == iMstcode && a.trntype == 74 && a.compcode == iCompcode
//                         select new
//                         {
//                             ItemID = a.trnitem,
//                             ddlItem = b.acctname,
//                             Rate = a.trnadju,
//                             Amt = a.trndram,
//                             UnitID = a.trnexpr,
//                             unitname = a.trnexpr == 1 ? "%" :
//                             a.trnexpr == 2 ? "/Pack" :
//                             a.trnexpr == 3 ? "Fix" :
//                             a.trnexpr == 4 ? "/Pcs" : ""
//                         };

//            var sData = new { oItmRaw = itmRaw, oItmFini = itmFini, oItmPack = itmPack, oItmExp = itmExp, iRatio = iRatio, iRatioType = iRatioType };
//            return Json(Json(sData).Data, JsonRequestBehavior.AllowGet);

//        }

//        [AcceptVerbs(HttpVerbs.Post)]
//        public JsonResult GetScheme(string Schm)
//        {

//            var Scheme = from a in db.ordemsts
//                         where a.msttype == 74 && a.compcode == SessionMaster.CompCode && a.mstchnm.Contains(Schm)
//                         select new { a.mstchnm, a.mstcode };
//            return Json(Scheme, JsonRequestBehavior.AllowGet);

//        }


//        public ActionResult Print(int comp = 0, int mstCode = 0, int msttype = 0)//170826
//        {
//            try
//            {
//                clsSubmitModel oSubmit = new clsSubmitModel();
              
//                DataSet ds = oSubmit.GetDataSet("sp_GetSalesBill @mstcode=" + mstCode + ",@compcode=" + comp + ",@msttype=" + msttype);
//                Session["Sauda"] = ds;

//                if (comp == 60) Session["ImgPath"] = "~/img/digi-logo.png"; else Session["ImgPath"] = "~/img/footer_logo.png";
                 
//            }
//            catch {  }

//return View("~/Views/Shared/Report.aspx");

//        }

//        public ActionResult PendingVehicleReport(int comp = 0, int mstCode = 0)
//        {
//            //    string reportName = "productionvouch.rpt";
//            //    SetReportData(reportName, "E");
//            //objModel = new ReportData();
//            clsSubmitModel oSubmit = new clsSubmitModel();
//            DataTable dt = oSubmit.GetData("select * from tmpprodtbl",true);
//            Session["PODetail"] = dt;
//            return View("Print");
//        }



//        #endregion
//        #region "inquiry"

//        //public ActionResult Inquiry()
//        //{
//        //    clsSubmitModel oSubmit = new clsSubmitModel();
//        //    DataTable dt;
//        //    int comp = Convert.ToInt32(SessionMaster.CompCode);
//        //    if (comp == 0)
//        //    {
//        //        TempData["message"] = "Please Select Company ...";
//        //    }

//        //    ViewBag.MstType = Request.QueryString["MstType"];

//        //    if (Request.QueryString["MstType"].ToString() == "74")
//        //        dt = oSubmit.GetProduction(Convert.ToInt32(Request.QueryString["MstType"]), Convert.ToInt32(SessionMaster.CompCode));
//        //    else
//        //        dt = oSubmit.GetVoucher(Convert.ToInt32(Request.QueryString["MstType"]), Convert.ToInt32(SessionMaster.CompCode));

//        //    var ItmSapu = from a in db.ordemsts
//        //                  where a.msttype == Convert.ToInt32(Request.QueryString["MstType"]) && a.compcode == SessionMaster.CompCode
//        //                  select new
//        //                  {
//        //                      mstcode = a.mstcode,
//        //                      mstdate = a.mstdate,
//        //                      msttota = a.msttota,
//        //                      mstChno = a.mstchno,
//        //                      compcode = a.compcode,
//        //                      msttype = a.msttype,
//        //                  };

//        //    Jourmaster oModel = new Jourmaster();
//        //    List<Jourmaster> oList = new List<Jourmaster>();
//        //    for (int i = 0; i < dt.Rows.Count; i++)
//        //    {
//        //        oModel = new Jourmaster();
//        //        oModel.mstcode = Convert.ToInt32(dt.Rows[i]["mstcode"]);
//        //        oModel.mstdate = Convert.ToDateTime(dt.Rows[i]["mstdate"]);
//        //        oModel.msttota = Convert.ToDecimal(dt.Rows[i]["msttota"]);
//        //        oModel.mstchno = dt.Rows[i]["mstChno"].ToString();
//        //        oModel.mstrema = dt.Rows[i]["mstrema"].ToString();
//        //        oModel.mstclno = dt.Rows[i]["mstclno"].ToString();
//        //        oModel.mstchnm = dt.Rows[i]["mstchnm"].ToString();
//        //        oModel.compcode = Convert.ToInt32(dt.Rows[i]["compcode"]);
//        //        oModel.msttype = Convert.ToInt32(dt.Rows[i]["msttype"]);

//        //        oList.Add(oModel);
//        //    }

//        //    oModel.ListArea = oList;

//        //    List<clsPoItem> lstParty = oSubmit.GetAllParty(SessionMaster.CompCode);

//        //    oModel.lstParty = lstParty;

//        //    return View(oModel);
//        //}

//        [HttpGet]
//        public ActionResult Inquiry(int comp = 0, int id = 0, int mstCode = 0, int MstType = 0)
//        {
//            SessionMaster.CompCode = 6;

//            DBEntity oDB = new DBEntity();
//            comp = SessionMaster.CompCode;
//            if (comp == 0)
//            {
//                TempData["message"] = "Please Select Company ...";
//            }

//            clsSubmitModel oSubmit = new clsSubmitModel();
//            ViewBag.MstType = Request.QueryString["MstType"];
//            Jourmaster frm = new Jourmaster();

//            if (mstCode > 0)
//            {
//                #region  "Edit"
//                DataTable dt;
//                if (MstType == 74)
//                    dt = oSubmit.GetData("select * from ordemst where compcode = " + comp + " and msttype = 74 and mstcode =" + mstCode, true);
//                else
//                    dt = oSubmit.GetVoucher(MstType, comp, mstCode);

//                if (dt.Rows.Count > 0)
//                {

//                    frm.msttimes = dt.Rows[0]["msttime"].ToString();
//                    frm.mstchnm = dt.Rows[0]["mstchnm"].ToString();
//                    frm.mstchno = dt.Rows[0]["mstchno"].ToString();
//                    if (dt.Rows[0]["mstchnV"].ToString() != "") frm.mstchnV = Convert.ToInt32(dt.Rows[0]["mstchnV"].ToString());
//                    if (dt.Rows[0]["msttota"].ToString() != "") frm.msttota = Convert.ToDecimal(dt.Rows[0]["msttota"].ToString());
//                    frm.mstrema = dt.Rows[0]["mstrema"].ToString();

//                    ViewBag.MstCode = frm.mstcode = Convert.ToInt32(dt.Rows[0]["mstcode"]);
//                    frm.compcode = Convert.ToInt32(dt.Rows[0]["compcode"]);
//                    frm.type = Convert.ToInt32(dt.Rows[0]["msttype"]);
//                    frm.mstdate = Convert.ToDateTime(dt.Rows[0]["mstDate"]);

//                    DataTable dtUnit;

//                    List<clsPoItem> lstUnit = new List<clsPoItem>();
//                    dtUnit = oSubmit.GetUnit(SessionMaster.CompCode);

//                    clsPoItem sm = new clsPoItem();

//                    for (int i = 0; i < dtUnit.Rows.Count; i++)
//                    {
//                        sm = new clsPoItem();
//                        sm.unitname = dtUnit.Rows[i]["unitname"].ToString();
//                        sm.unitcode = dtUnit.Rows[i]["unitcode"].ToString();
//                        lstUnit.Add(sm);
//                    }

//                    ViewBag.UnitID_Exp = ViewBag.UnitID_Pack = ViewBag.UnitID_Fini = ViewBag.UnitID = new SelectList(lstUnit, "unitcode", "unitname");

//                    return View("Inquiry", frm);
//                }
//                #endregion
//            }
//            else
//            {
//                #region "Create"

//                frm.msttimes = DateTime.Now.ToString("HH:mm:ss");
//                DataTable dt2;
//                string sTable = "";
//                if (MstType == 74)
//                    sTable = "ordemst";
//                else
//                    sTable = "Jourmst";

//                dt2 = oSubmit.GetData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from " + sTable + " where compcode ='" + comp + "' and msttype ='" + MstType + "'", true);
//                ViewBag.MstCode = frm.mstcode = Convert.ToInt32(dt2.Rows[0]["mstcode"]);
//                frm.compcode = Convert.ToInt32(SessionMaster.CompCode);
//                frm.type = MstType;
//                frm.mstdate = DateTime.Now.Date;

//                DataTable dtUnit;

//                List<clsPoItem> lstUnit = new List<clsPoItem>();
//                dtUnit = oSubmit.GetUnit(SessionMaster.CompCode);

//                clsPoItem sm = new clsPoItem();

//                for (int i = 0; i < dtUnit.Rows.Count; i++)
//                {
//                    sm = new clsPoItem();
//                    sm.unitname = dtUnit.Rows[i]["unitname"].ToString();
//                    sm.unitcode = dtUnit.Rows[i]["unitcode"].ToString();
//                    lstUnit.Add(sm);
//                }

//                ViewBag.UnitID_Exp = ViewBag.UnitID_Pack = ViewBag.UnitID_Fini = ViewBag.UnitID = new SelectList(lstUnit, "unitcode", "unitname");

//                List<clsPoItem> lstParty = oSubmit.GetAllParty(SessionMaster.CompCode);

//                frm.lstParty = lstParty;

//                #endregion
//            }
//            return View("Inquiry", frm);
//        }

//        [HttpPost]
//        public ActionResult Inquiry(Jourmaster oCls)
//        {
//            clsSubmitModel oSubmit = new clsSubmitModel();
//            commFunction oCom = new commFunction();
//            gathdet oGath = new gathdet();

//            try
//            {
//                clsPoItem ItemDet = null;
//                jourtrn oTrn = new jourtrn();
//                sapuitd oSapu = new sapuitd();
//                string sTblMst = "";
//                string sTblItd = "";
//                if (oCls.msttype == 1162)
//                {
//                    sTblMst = "OrdeMst";
//                    sTblItd = "OrdeItd";
//                }
//                else
//                {
//                    sTblMst = "JourMst";
//                    sTblItd = "ITPurSal";
//                }

//                string sGathCode = "";

//                //var json = oCls.sItemDet;


//                ViewBag.MstType = oCls.msttype;
//                int iDays = 0;
//                decimal iComm = 0, iInterest = 0;

//                if (oCls.mstdued > 0) iDays = (int)oCls.mstdued;// DueDay;170831
//                if (oCls.Comm > 0) iComm = oCls.Comm;
//                if (oCls.Interest > 0) iInterest = oCls.Interest;

//                oCls.mststat = 0;
//                oCls.msttime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));

//                if (ModelState.IsValid)
//                {
//                    oSubmit.BeginTran();

//                    DataTable dt2;
//                    for (int iIndex = 0; iIndex < oCls.lstParty.Count; iIndex++)
//                    {
//                        if (oCls.lstParty[iIndex].IsSelected == false) continue;

//                        dt2 = oSubmit.GetData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from " + sTblMst + " where compcode ='" + oCls.compcode + "' and msttype ='" + oCls.msttype + "'");
//                        oCls.mstcode = Convert.ToInt32(dt2.Rows[0]["mstcode"]);

//                        oCls.mstcust = oCls.lstParty[iIndex].tpPartyID;
//                        oCls.mstrefc = getParty(Convert.ToInt32(oCls.mstcust)) + "~" + iInterest + "~" + iDays + "~" + iComm;

//                        oSubmit.InsJourmst(oCls, sTblMst);

//                        sGathCode = oSubmit.GetUsWoCode();

//                        oSubmit.insertData("delete from Ordetrn  where compcode = " + oCls.compcode + " and trntype = " + oCls.msttype + " and  trncode = " + oCls.mstcode);
//                        oSubmit.insertData("delete from " + sTblItd + "  where compcode = " + oCls.compcode + " and Itdtype = " + oCls.msttype + " and  itdcode = " + oCls.mstcode);


//                        if (oCls.sItemRaw != null)
//                        {
//                            ItemDet = JsonConvert.DeserializeObject<clsPoItem>(oCls.sItemRaw);
//                            for (int i = 0; i < ItemDet.LstItem.Count; i++)
//                            {
//                                oSapu.compcode = Convert.ToInt16(oCls.compcode);
//                                oGath.itdtype = oSapu.itdtype = Convert.ToInt32(oCls.msttype);
//                                oSapu.itdcode = Convert.ToInt32(oCls.mstcode);
//                                oSapu.itdtime = Convert.ToInt32(oCls.msttime);
//                                oSapu.itdsrno = Convert.ToInt16(i + 1);
//                                oSapu.itddate = Convert.ToDateTime(oCls.mstdate);
//                                oSapu.itdtime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
//                                oSapu.itditem = Convert.ToInt32(ItemDet.LstItem[i].ItemID);
//                                oSapu.itdtowt = oSapu.itdpenq = oSapu.itdquan = Convert.ToInt32(-ItemDet.LstItem[i].Qty);
//                                oSapu.itdunit = Convert.ToInt32(ItemDet.LstItem[i].UnitID);
//                                oSapu.itddime = ItemDet.LstItem[i].Rate.ToString();// Convert.ToInt32(ItemDet.LstItem[i].Rate);170830
//                                oSapu.itdvate = oSapu.itdrate = Convert.ToDecimal(ItemDet.LstItem[i].Rate);

//                                oSapu.itdthickness = Convert.ToDecimal(ItemDet.LstItem[i].Cases);
//                                oSapu.itdlength = Convert.ToDecimal(ItemDet.LstItem[i].Tax_S);
//                                oSapu.itdwidth = Convert.ToDecimal(ItemDet.LstItem[i].Tax_C);
//                                oSapu.itdweight = Convert.ToDecimal(ItemDet.LstItem[i].Tax_I);
//                                oSapu.itdtowt = Convert.ToDecimal(ItemDet.LstItem[i].Amt);


//                                oSapu.itdpkin = Convert.ToInt32(ItemDet.LstItem[i].Qty);

//                                oSapu.itdinde = 0;

//                                oSapu.itdmill = 1; oSapu.itdgodo = 1;
//                                oGath.gathcode = oSapu.itdgath = sGathCode;
//                                oSapu.itdempo = 0; oSapu.itdlabamt = 0;
//                                ItemDet.LstItem[i].TaxPer.ToString();
//                                oSubmit.insSapuItd(oSapu, sTblItd);
//                                oSubmit.insGathDet(oGath);
//                            }
//                        }


//                        //if (oCls.sItemExp != null)
//                        //{
//                        //    ItemDet = JsonConvert.DeserializeObject<clsPoItem>(oCls.sItemExp);
//                        //    for (int i = 0; i < ItemDet.LstItem.Count; i++)
//                        //    {
//                        //        if (ItemDet.LstItem[i].ItemID > 0)
//                        //        {
//                        //            oTrn.trnsrno = Convert.ToInt16(i + 1);
//                        //            oTrn.compcode = Convert.ToInt16(oCls.compcode);
//                        //            oTrn.trntype = Convert.ToInt32(oCls.msttype);
//                        //            oTrn.trncode = Convert.ToInt32(oCls.mstcode);
//                        //            oTrn.trntime = Convert.ToInt32(oCls.msttime);
//                        //            oTrn.trnsrno += 1;
//                        //            oTrn.trndate = Convert.ToDateTime(oCls.mstdate);
//                        //            oTrn.trnledg = oTrn.trnitem = ItemDet.LstItem[i].ItemID;
//                        //            oTrn.trnadju = Convert.ToInt32(ItemDet.LstItem[i].Rate);
//                        //            oTrn.trndram = Convert.ToInt32(ItemDet.LstItem[i].Amt);
//                        //            oTrn.trncram = 0;
//                        //            oTrn.trnexpr = ItemDet.LstItem[i].UnitID;
//                        //            oTrn.trninde = 3;
//                        //            oSubmit.InsJourTrn(oTrn, true);
//                        //        }
//                        //    }
//                        //}

//                        if (oCls.IsEdit != true)
//                        {
//                            oCls.StDate = Convert.ToDateTime(oCom.getOpenDate(DateTime.Now.Date));
//                            oCls.LastDate = Convert.ToDateTime(oCom.getClosDate(DateTime.Now.Date));
//                            oSubmit.UpdCodeGen(oCls);
//                        }

//                    }
//                    TempData["message"] = "Saved Successfully ...";

//                    oSubmit.Commit();
//                    oCls.mstcode = 0;

//                }
//            }
//            catch (Exception ex)
//            {
//                oSubmit.RollBack();
//                TempData["message"] = ex.Message;
//                return View("Inquiry");
//            }
//            return RedirectToAction("Inquiry", new { MstType = oCls.msttype, MstCode = oCls.mstcode, comp = oCls.compcode });
//        }

//        [HttpPost]
//        public ActionResult DispatchBillSummary(int comp = 0, int id = 0, int mstCode = 0, int MstType = 0)
//        {
//            string reportName = "DispBillSumm.rpt";
//            SetReportData(reportName, "E");
//            return View("Print");
//        }

//        public void SetReportData(string reportName, string format)
//        {
//            clsSubmitModel oSubmit = new clsSubmitModel();
//            DataTable dt = oSubmit.GetData("select * from tmpprodtbl", true);

//            ReportDocument rd = new ReportDocument();
//            rd.Load(System.IO.Path.Combine(Server.MapPath("~/Report"), reportName));
//            // rd.Load(System.IO.Path.Combine(Server.MapPath(@"../Report"), reportName));


//            rd.SetDataSource(dt);

//            rd.Dispose();
//            //objModel.fillModel();            
//        }

//        #endregion

//        #region "Service Input"
//        [HttpGet]
//        public ActionResult ServiceInput(int comp = 0, int mstCode = 0, int MstType = 0, int mstcode_Print = 0, int mencode = 0)
//        {
//            if (mstcode_Print == 0)
//            {
//                TempData["message"] = null;
//            }
             
//            try
//            { 
                
//                DBEntity oDB = new DBEntity();
//                comp = SessionMaster.CompCode;
//                if (comp == 0)
//                {
//                    TempData["message"] = "Please Select Company ...";
//                }

//                clsSubmitModel oSubmit = new clsSubmitModel();
//                ViewBag.MstType = Request.QueryString["MstType"];
//                Jourmaster frm = new Jourmaster();

//                //************************** Rights ****************************
//                DataTable dt;
//                dt = oSubmit.GetData("select * from usermenust where mencode =" + Request.QueryString["MenCode"] + " and menuser =" + SessionMaster.UserID, true);
//                if (dt.Rows.Count > 0)
//                {
//                    if (Convert.ToBoolean(dt.Rows[0]["menview"]) == false) Response.Redirect("../home/menuNew");
//                    frm.menaddi = Convert.ToBoolean(dt.Rows[0]["menaddi"]);
//                    frm.menedit = Convert.ToBoolean(dt.Rows[0]["menedit"]);
//                    frm.menview = Convert.ToBoolean(dt.Rows[0]["menview"]);

//                    frm.mencode = Convert.ToInt32(dt.Rows[0]["menCode"]);       // Add new variable  menu code "Rupesh"
//                    ViewBag.MenCode = frm.mencode;                       //==//===
//                }
//                //**************************************************************  
                 
//                    //ViewBag.mstsale = new SelectList(from res in db.accounts where res.acctgrou == 4 && res.compcode == comp orderby res.acctsort select new { res.acctcode, res.acctname }, "acctcode", "acctname");

//                    ViewBag.mstsale = new SelectList(from a in db.accounts
//                                                  join b in db.acgroups on new { _Comp = a.compcode, _Grou = a.acctgrou } equals new { _Comp = b.compcode, _Grou = b.groucode }
//                                                orderby a.acctname  where a.compcode == SessionMaster.CompCode && (b.groumain == 14 || b.groumain == 12 || b.groumain == 45 || b.groumain == 13 || b.groumain == 11 || b.groumain == 4 || b.groumain == 46) && (int) a.acctcform != 0
//                                                  select new { a.acctcode, a.acctname }, "acctcode", "acctname"); 

//                ViewBag.Exp1 = new SelectList(from res in db.accounts where (res.acctgrou == ConstVariable.TypeCode_IndirectExpenses || res.acctgrou == ConstVariable.TypeCode_Taxes || res.acctgrou == ConstVariable.TypeCode_DirectExpenses) && res.compcode == comp orderby res.acctname select new { res.acctcode, res.acctname }, "acctcode", "acctname");
                 
//                ViewBag.TaxHead_I = new SelectList(from res in db.accounts where (res.acctgrou == ConstVariable.TypeCode_Taxes) && res.compcode == comp select new { res.acctcode, res.acctname }, "acctcode", "acctname");

               
//                if (mstCode <= 0)
//                {
//                    TempData["FrsRec"] = "This is First record!!!";
//                }


//                if (mstCode > 0)
//                {
//                    #region  "Edit"
//                    dt = oSubmit.GetVoucher(MstType, comp, mstCode);

//                    if (dt.Rows.Count > 0)
//                    {
                         
//                        frm.msttota = Convert.ToDecimal(dt.Rows[0]["msttota"].ToString());
//                        frm.mstpofs = dt.Rows[0]["mstpofs"].ToString();
//                        if (!string.IsNullOrEmpty(dt.Rows[0]["mstbala"].ToString())) frm.mstbala = Convert.ToDecimal(dt.Rows[0]["mstbala"].ToString());
//                        if (!string.IsNullOrEmpty(dt.Rows[0]["mstpaid"].ToString())) frm.mstpaid = Convert.ToDecimal(dt.Rows[0]["mstpaid"].ToString());
//                        if (!string.IsNullOrEmpty(dt.Rows[0]["mstneta"].ToString())) frm.mstneta = Convert.ToDecimal(dt.Rows[0]["mstneta"].ToString());
//                        frm.Remark = dt.Rows[0]["Remark"].ToString();
//                        if (!string.IsNullOrEmpty(dt.Rows[0]["mstblno"].ToString())) frm.mstblno = dt.Rows[0]["mstblno"].ToString();
//                        if (!string.IsNullOrEmpty(dt.Rows[0]["mstclno"].ToString())) frm.mstclno = dt.Rows[0]["mstclno"].ToString();
//                        if (!string.IsNullOrEmpty(dt.Rows[0]["mstbldt"].ToString())) if (dt.Rows[0]["mstbldt"].ToString() != "") frm.mstbldt = Convert.ToDateTime(dt.Rows[0]["mstbldt"].ToString());
//                        if (!string.IsNullOrEmpty(dt.Rows[0]["mstcldt"].ToString())) if (dt.Rows[0]["mstcldt"].ToString() != "") frm.mstbldt = Convert.ToDateTime(dt.Rows[0]["mstcldt"].ToString());
//                        //if (!string.IsNullOrEmpty(dt.Rows[0]["CommAmt"].ToString())) frm.CommAmt = Convert.ToDecimal(dt.Rows[0]["CommAmt"].ToString());
//                        if (!string.IsNullOrEmpty(dt.Rows[0]["Comm"].ToString())) frm.Comm = Convert.ToDecimal(dt.Rows[0]["Comm"].ToString());
//                        if (!string.IsNullOrEmpty(dt.Rows[0]["mstchnV"].ToString())) frm.mstchnV = Convert.ToInt32(dt.Rows[0]["mstchnV"].ToString());
//                        if (!string.IsNullOrEmpty(dt.Rows[0]["mstchnH"].ToString())) frm.mstchnH = dt.Rows[0]["mstchnH"].ToString();
//                        if (!string.IsNullOrEmpty(dt.Rows[0]["mstchno"].ToString())) frm.mstchno = dt.Rows[0]["mstchno"].ToString();

//                        frm.acctname = dt.Rows[0]["acctname"].ToString();
//                        if (!string.IsNullOrEmpty(dt.Rows[0]["Broker"].ToString())) frm.Broker = dt.Rows[0]["Broker"].ToString();
//                        if (!string.IsNullOrEmpty(dt.Rows[0]["mstPrtc"].ToString())) frm.PartyID = Convert.ToInt32(dt.Rows[0]["mstPrtc"].ToString()); //PartyID
//                        if (!string.IsNullOrEmpty(dt.Rows[0]["mstbrok"].ToString())) frm.mstbrok = Convert.ToInt32(dt.Rows[0]["mstbrok"].ToString());

//                        if (dt.Rows[0]["msttime"].ToString().Length == 6)
//                            frm.msttimes = dt.Rows[0]["msttime"].ToString().Substring(0, 2) + ":" + dt.Rows[0]["msttime"].ToString().Substring(2, 2) + ":" + dt.Rows[0]["msttime"].ToString().Substring(4, 2);
//                        else
//                            frm.msttimes = dt.Rows[0]["msttime"].ToString();

//                        frm.mstrema = dt.Rows[0]["mstRema"].ToString();
//                        ViewBag.MstCode = frm.mstcode = Convert.ToInt32(dt.Rows[0]["mstcode"]);
//                        frm.compcode = Convert.ToInt32(SessionMaster.CompCode);
//                        frm.msttype = frm.type = MstType;
//                        frm.mstdate = Convert.ToDateTime(dt.Rows[0]["mstDate"]);
//                        frm.sMstdate = Convert.ToDateTime(dt.Rows[0]["mstdate"]).ToString("dd/MM/yyyy");
//                        if (!string.IsNullOrEmpty(dt.Rows[0]["mstbldt"].ToString())) frm.sMstBlDt = Convert.ToDateTime(dt.Rows[0]["mstbldt"]).ToString("dd/MM/yyyy");
                         
//                        if (dt.Rows[0]["mstctyp"].ToString() != "") frm.mstctyp = Convert.ToInt32(dt.Rows[0]["mstctyp"].ToString());

//                        //ViewBag.mstsale = new SelectList(from res in db.accounts where res.acctgrou == 4 && res.compcode == comp orderby res.acctsort select new { res.acctcode, res.acctname }, "acctcode", "acctname", dt.Rows[0]["mstsale"]);

//                        ViewBag.mstsale = new SelectList(from a in db.accounts
//                                                         join b in db.acgroups on new { _Comp = a.compcode, _Grou = a.acctgrou } equals new { _Comp = b.compcode, _Grou = b.groucode }
//                                                         orderby a.acctname
//                                                         where a.compcode == SessionMaster.CompCode && (b.groumain == 14 || b.groumain == 12 || b.groumain == 45 || b.groumain == 13 || b.groumain == 11 || b.groumain == 4 || b.groumain == 46) && (int)a.acctcform != 0
//                                                         select new { a.acctcode, a.acctname }, "acctcode", "acctname" , dt.Rows[0]["mstsale"]); 
                         
                         
//                        return View("ServiceInput", frm);
//                    }
//                    else
//                    {
//                        var MaxMstCode = 0;
//                        MaxMstCode = (db.jourmsts.Where(x => x.compcode == SessionMaster.CompCode).Where(x => x.msttype == MstType).Max(x => x.mstcode));
//                        if (mstCode > MaxMstCode)
//                        {
//                            TempData["lstRec"] = "This is last record!!!";
//                            return RedirectToAction("ServiceInput", "PuSL", new { comp = comp, MstType = MstType, mstcode_Print = mstcode_Print, mencode = mencode });
//                        }
//                        else
//                        {
//                            TempData["lstRec"] = "Record Not Found ...";

//                            frm.msttimes = DateTime.Now.ToString("HH:mm:ss");
//                            ViewBag.MstCode = frm.mstcode = mstCode;
//                            frm.compcode = Convert.ToInt32(SessionMaster.CompCode);
//                            frm.msttype = frm.type = MstType;
//                            frm.mstdate = DateTime.Now.Date;
                              
//                            return View("ServiceInput", frm);
                            
//                        }

//                    }
//                    #endregion
//                }
//                else
//                {
//                    #region "Create"

//                    frm.msttimes = DateTime.Now.ToString("HH:mm:ss");
//                    DataTable dt2;

//                    dt2 = oSubmit.GetData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from jourmst where compcode ='" + comp + "' and msttype ='" + MstType + "'", true);
//                    ViewBag.MstCode = frm.mstcode = Convert.ToInt32(dt2.Rows[0]["mstcode"]);
//                    frm.compcode = Convert.ToInt32(SessionMaster.CompCode);
//                    frm.msttype = frm.type = MstType;
//                    frm.mstdate = DateTime.Now.Date;
                      
//                    frm.mstcode_Print = mstcode_Print;

//                    string sMstChNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");
                     
//                    dt2 = oSubmit.GetData("select isnull(max(right(mstchno,2))+1,1) as maxchno  from jourmst where compcode = " + comp + " and msttype = " + MstType + " and left(mstchno, 6) = '" + sMstChNo + "'", true);
//                      frm.mstchno = sMstChNo + GetVoucherNo(dt2.Rows[0]["maxchno"].ToString());

//                    #endregion
//                }
//                return View("ServiceInput", frm);
//            }
//            catch
//            {
//                return View();
//            }
//        }
//        [HttpPost]
//        public ActionResult ServiceInput(Jourmaster oCls)
//        {
//            if (Convert.ToInt16(SessionMaster.CompCode) == 0) { return View("LogOff", "Home"); }

//            clsSubmitModel oSubmit = new clsSubmitModel();
//            commFunction oCom = new commFunction();
//            try
//            {
//                jourtrn oTrn = new jourtrn();
//                sapuitd oSapu = new sapuitd();
//                gathdet oGath = new gathdet();

//                string sGathCode = "";
//                string sItmTbl = "ServItd";  

//                DataTable dt2;

//                //dt2 = oSubmit.GetData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from jourmst where compcode ='" + comp + "' and msttype ='" + MstType + "'");

//                if (Request.QueryString["MstCode"] != null && Convert.ToInt32(Request.QueryString["MstCode"]) > 0) { }
//                else
//                {
//                    oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from jourmst where compcode ='" + SessionMaster.CompCode + "' and msttype ='" + oCls.msttype + "'", "0", true));
//                }

//                var json = oCls.sItemDet;
//                clsPoItem ItemDet = JsonConvert.DeserializeObject<clsPoItem>(json);

//                ViewBag.MstType = oCls.msttype;
//                int iDays = 0;
//                decimal iComm = 0, iInterest = 0, mstperd = 0;

//                oCls.mstdate = oSubmit.GetDateFormat(oCls.sMstdate);
//                oCls.mstbldt = oSubmit.GetDateFormat(oCls.sMstBlDt);

//                if (oCls.mstdued > 0) iDays = (int)oCls.mstdued;//DueDay 170831
//                if (oCls.Comm > 0) iComm = oCls.Comm;
//                if (oCls.Interest > 0) iInterest = oCls.Interest;

//                if (oCls.mstchnH == null) oCls.mstchnH = "";
//                oCls.mststat = 0;
//                oCls.msttime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
//                oCls.mstrefc = getParty(Convert.ToInt32(oCls.PartyID)) + "~" + iInterest + "~" + iDays + "~" + iComm;
               
//               // oCls.mstchno = oCls.mstchnH + oCls.mstchnV;//170830
//                oCls.mstexti = "~~#0";//170830
              
//                oCls.mstgncd = "0~0~" + oCls.acctledg + "~0";//170830
                 
//                oCls.mstAppr = 0; oCls.mstqtyd = 0; oCls.mstvat1 = 0; oCls.mstvat2 = 0; oCls.mstvat3 = 0;
//                oCls.oldwht = 0; oCls.mstsite = 0; oCls.mstbrnc = 0; oCls.mstsubt = 0; oCls.mstcust = 0;
//                oCls.mstrvsc = 0; oCls.mstintr = 0; oCls.mstcurrcd = 1; oCls.mstcurrrat = 1;
//                oCls.mstchnm = ""; oCls.msternv = "";
//                oCls.mstprtc = oCls.PartyID;
                 
//                oCls.mstDueDate = oCls.mstdate.AddDays(iDays);//170831
//                oCls.mstbuyerc = 0;
//                // oCls.mstperd = 0;
//                oCls.mstdsptoc = 0;
//                if (ItemDet.LstItem.Count > 0 && ItemDet.LstItem[0].GSTStateVal == "1")
//                    oCls.mstIorL = "I";
//                else
//                    oCls.mstIorL = "L";

//                oSubmit.BeginTran();
//                //oSubmit.InsJourmst(oCls);

//                oSubmit.insertData("delete from jourtrn  where compcode = " + oCls.compcode + " and trntype = " + oCls.msttype + " and  trncode = " + oCls.mstcode + " and  trnDate = '" + oCls.mstdate + "'");

//                oSubmit.insertData("delete b from " + sItmTbl + " a inner join GathDet b on a.itdgath = b.gathCode and a.Itdtype =b.Itdtype where a.Itdtype = " + oCls.msttype + " and Itdcode = " + oCls.mstcode);

//                oSubmit.insertData("Delete from " + sItmTbl + " where CompCode = " + oCls.compcode + " and Itdtype = " + oCls.msttype + " and Itdcode = " + oCls.mstcode);

//                int itdAcctCd = 0;
//                for (int i = 0; i < ItemDet.LstItem.Count; i++)
//                {

//                    //var q = from a in db.GetTable<charcodege>() select a; 
//                    //var w = from res in db.charcodeges select new { res.gatcode };

//                    sGathCode = oSubmit.GetUsWoCode();

//                    oSapu.compcode = Convert.ToInt16(oCls.compcode);
//                    oSapu.itdtype = Convert.ToInt32(oCls.msttype);
//                    oSapu.itdcode = Convert.ToInt32(oCls.mstcode);
//                    oSapu.itdtime = Convert.ToInt32(oCls.msttime);
//                    oSapu.itdsrno = Convert.ToInt16(i + 1);
//                    oGath.gathpuri = oGath.gathstat = Convert.ToString(oSapu.itddate = Convert.ToDateTime(oCls.mstdate));
//                    oSapu.itdtime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
//                    oSapu.itditem = Convert.ToInt32(ItemDet.LstItem[i].ItemID);
//                    oGath.gathdesc = oSapu.itdrema = "";// ItemDet.LstItem[i].Remark.ToString();// ItemRemark.ToString();
//                    //oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].Qty);
//                    if (ItemDet.LstItem[i].UnitID.ToString() != "") oSapu.itdunit = Convert.ToInt32(ItemDet.LstItem[i].UnitID); else oSapu.itdunit = 0;
//                    oSapu.itddime = ItemDet.LstItem[i].Rate.ToString();

//                    oSapu.itdvate = oSapu.itdrate = Convert.ToDecimal(ItemDet.LstItem[i].Rate);

//                    Convert.ToDecimal(oSapu.itdvate);
//                    oSapu.itdamou = Convert.ToDecimal(ItemDet.LstItem[i].Amt);
//                    //oSapu.itdtowt = 0;//Convert.ToInt32(ItemDet.LstItem[i].Qty);//170830
//                    if (oCls.msttype == 42) oSapu.itdpenq = oSapu.itdquan = -Convert.ToInt32(ItemDet.LstItem[i].Qty);
//                    else oSapu.itdpenq = oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].Qty);
//                    //oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].Qty);//170830
//                    oSapu.itdpkin = 0;
//                    oSapu.itdmill = 1;
//                    oSapu.itdgodo = 1;

//                    oSapu.itdlswt = 0;//170831


//                    oSapu.itdlabonw = 0;//170831
//                    oSapu.itdvatinc = 0;//170831
//                    // oSapu.itdcasert= 0;//170831

//                    if (ItemDet.LstItem[i].Rate.ToString() != "") oSapu.itdactprc = oGath.gathwtdi = Convert.ToDecimal(ItemDet.LstItem[i].Rate.ToString());//TRate.ToString()); 
//                    if (ItemDet.LstItem[i].DisPer.ToString() != "") oSapu.itddscp = oGath.gathqtdi = Convert.ToDecimal(ItemDet.LstItem[i].DisPer);
//                    if (ItemDet.LstItem[i].DisAmt.ToString() != "") oGath.gathlabp = Convert.ToDouble(ItemDet.LstItem[i].DisAmt);
//                    if (ItemDet.LstItem[i].DisWT.ToString() != "") oGath.gathdion = Convert.ToDecimal(ItemDet.LstItem[i].DisWT);
//                    if (ItemDet.LstItem[i].DisQtyAmt.ToString() != "") oGath.gathcdam = Convert.ToDecimal(ItemDet.LstItem[i].DisQtyAmt);
//                    if (ItemDet.LstItem[i].PerDisTota.ToString() != "") oGath.gathdscv = oSapu.itdperd = Convert.ToDecimal(ItemDet.LstItem[i].PerDisTota);
//                    if (ItemDet.LstItem[i].QtyDis.ToString() != "") oSapu.itdqtyd = Convert.ToDecimal(ItemDet.LstItem[i].QtyDis);

//                    if (ItemDet.LstItem[i].FQty.ToString() != "") oGath.gatlabg = Convert.ToDouble(ItemDet.LstItem[i].FQty);
//                    if (ItemDet.LstItem[i].BilledQty.ToString() != "") oGath.gathwtdf = Convert.ToDecimal(ItemDet.LstItem[i].BilledQty);
//                    if (ItemDet.LstItem[i].itemdisc.ToString() != "") oSapu.itdtowt = Convert.ToDecimal(ItemDet.LstItem[i].itemdisc) * oSapu.itdquan;

//                    if (ItemDet.LstItem[i].SrvAccountHead.ToString() != "") itdAcctCd = Convert.ToInt32(ItemDet.LstItem[i].SrvAccountHead); else itdAcctCd = 0;

//                    oCls.mstperd += oSapu.itdperd;

//                    if (ItemDet.LstItem[i].acctcode.ToString() != "") oSapu.itdinde = Convert.ToInt32(ItemDet.LstItem[i].acctcode);
//                    else oSapu.itdinde = 0; // Ajay on 29012018 For get proper Gst Value in Trn(Output) List in Edit Time HTML Code >> (if (itm[i].acctcode == trn[ii].ItemID))

//                    oGath.gathcode = oSapu.itdgath = sGathCode;
//                    oSapu.itdempo = SessionMaster.UserID;//0;170831 //Subodh Sir Se Puchana hai...
//                    oSapu.itdlabamt = 0;
//                    oGath.gathwast = ItemDet.LstItem[i].TaxPer.ToString();

//                    //{170830
//                    oSapu.itdgstrtv = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer);
//                    if (ItemDet.LstItem[i].GSTStateVal == "2")
//                    {
//                        if (Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal) > 0) oSapu.itdcgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal);
//                        else oSapu.itdcgstrt = 0;

//                        oSapu.itdcgstvl = Convert.ToDecimal(ItemDet.LstItem[i].Amt) * oSapu.itdcgstrt / 100;
//                        if (Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal) > 0) oSapu.itdsgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal);
//                        else oSapu.itdsgstrt = 0;

//                        oSapu.itdsgstvl = Convert.ToDecimal(ItemDet.LstItem[i].Amt) * oSapu.itdsgstrt / 100;
//                        oSapu.itdigstrt = 0;
//                        oSapu.itdigstvl = 0;
//                    }
//                    else
//                    {
//                        oSapu.itdcgstrt = 0;
//                        oSapu.itdcgstvl = 0;
//                        oSapu.itdsgstrt = 0;
//                        oSapu.itdsgstvl = 0;
//                        if (Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal) > 0) oSapu.itdigstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal); else oSapu.itdigstrt = 0;

//                        oSapu.itdigstvl = Convert.ToDecimal(ItemDet.LstItem[i].Amt) * oSapu.itdigstrt / 100;

//                    }
//                    oSapu.itdcessrt = Convert.ToDecimal(0);
//                    oSapu.itdcessvl = Convert.ToDecimal(0);
//                    oSapu.itdugstrt = Convert.ToDecimal(0);
//                    oSapu.itdugstvl = Convert.ToDecimal(0);
//                    //}170830
//                    oSubmit.insSapuItd(oSapu, sItmTbl, itdAcctCd);
//                    oSubmit.insGathDet(oGath);

//                }

//                oSubmit.InsJourmst(oCls);

//                ItemDet = JsonConvert.DeserializeObject<clsPoItem>(oCls.sTrnDet);
//                for (int i = 0; i < ItemDet.LstItem.Count; i++)
//                {
//                    if (ItemDet.LstItem[i].ItemID > 0)
//                    {
//                        oTrn.compcode = Convert.ToInt16(oCls.compcode);
//                        oTrn.trntype = oCls.msttype;
//                        oTrn.trncode = oCls.mstcode;
//                        oTrn.trntime = oCls.msttime;
//                        oTrn.trnsrno = Convert.ToInt16(i + 1);
//                        oTrn.trndate = oCls.mstdate;
//                        oTrn.trnitem = ItemDet.LstItem[i].ItemID;

//                        if (ItemDet.LstItem[i].trnledg.ToString() != "") oTrn.trnledg = ItemDet.LstItem[i].trnledg; else oTrn.trnledg = 0;

//                        if (ItemDet.LstItem[i].AdjuAmt.ToString() != "") oTrn.trnadju = Convert.ToDecimal(ItemDet.LstItem[i].AdjuAmt); else oTrn.trnadju = 0;

//                        if (oCls.msttype == 42)
//                        {
//                            oTrn.trndram = ItemDet.LstItem[i].tpDrAmt;
//                            oTrn.trncram = ItemDet.LstItem[i].tpCrAmt;
//                        }
//                        else//Purchase
//                        {
//                            oTrn.trndram = ItemDet.LstItem[i].tpCrAmt;
//                            oTrn.trncram = ItemDet.LstItem[i].tpDrAmt;
//                        }
//                        oTrn.trninde = ItemDet.LstItem[i].trninde;//170829
//                        oTrn.trnexpa = ItemDet.LstItem[i].trnexpa;//170830
//                        oTrn.trntagv = ItemDet.LstItem[i].trntagv;//170830
//                        if (ItemDet.LstItem[i].trnaddv.ToString() != "") oTrn.trnaddv = Convert.ToDecimal(ItemDet.LstItem[i].trnaddv);//170830

//                        oSubmit.InsJourTrn(oTrn);
//                    }
//                }

//                int iMode = 1;

//                if (oCls.IsEdit != true)
//                {
//                    iMode = 0;
//                    oCls.StDate = Convert.ToDateTime(oCom.getOpenDate(DateTime.Now.Date));
//                    oCls.LastDate = Convert.ToDateTime(oCom.getClosDate(DateTime.Now.Date));
//                    oSubmit.UpdCodeGen(oCls);
//                }

//                //********************************* User Work ********************************* 
//                clsUserWork oUser = new clsUserWork();
//                oUser.woruser = SessionMaster.UserID;
//                oUser.wormode = iMode;
//                oUser.worcomp = SessionMaster.CompCode;
//                oUser.wortype = oCls.msttype;
//                oUser.worcode = oCls.mstcode;
//                oUser.worsrno = oSubmit.GetUsWoCode();
//                oUser.worrema = "D-" + oCls.acctname + "#" + oCls.sMstdate + "#" + oCls.mstrema;
//                oUser.wordate = oCls.mstdate;
//                oUser.worrfsr = "";
//                oUser.worsysn = Environment.MachineName;
//                oUser.IP_Add = this.Request.UserHostAddress;
//                oUser.WorChNo = oCls.mstchno;
//                oUser.WorNarr = oCls.mstrema;
//                oSubmit.InsUserWork(oUser);
//                //********************************* User Work ********************************* 

//                oCls.mstcode_Print = oCls.mstcode;
//                oSubmit.Commit();

//                TempData["message"] = "Saved Successfully ...";
//                oCls.mstcode = 0;
//            }
//            catch (Exception ex)
//            {
//                oSubmit.RollBack();
//                TempData["message"] = ex.Message;
//                //return View();
//                return RedirectToAction("ServiceInput", new { MstType = oCls.msttype, MstCode = 0, comp = oCls.compcode, MenCode = Request.QueryString["MenCode"] });
//            }
//            return RedirectToAction("ServiceInput", new { MstType = oCls.msttype, MstCode = oCls.mstcode, comp = oCls.compcode, mstcode_Print = oCls.mstcode_Print, MenCode = Request.QueryString["MenCode"] });
//        }

//        public ActionResult GetSrvDet(int iMstcode, int iCompcode, int iType)
//        {

//            clsSubmitModel oSubmit = new clsSubmitModel();

//            DataTable dt;
//            dt = oSubmit.GetVoucher(iType, iCompcode, iMstcode);

//            List<clsPoItem> oList = new List<clsPoItem>();
//            List<clsPoItem> oExpList = new List<clsPoItem>();
//            List<clsPoItem> oItemList = new List<clsPoItem>();
//            clsPoItem oOrder1 = new clsPoItem();

//            for (int i = 0; i < dt.Rows.Count; i++)
//            {
//                oOrder1 = new clsPoItem();
//                oOrder1.ItemID = Convert.ToInt32(dt.Rows[i]["tpPartyID"]);
//                oOrder1.trnItem = dt.Rows[i]["partyname"].ToString();
//                oOrder1.AdjuAmt = Convert.ToDecimal(dt.Rows[i]["trnAdju"]);
//                oOrder1.tpDrAmt = Convert.ToDecimal(dt.Rows[i]["tpDrAmt"]);
//                oOrder1.tpCrAmt = Convert.ToDecimal(dt.Rows[i]["tpCrAmt"]);
//                oList.Add(oOrder1);

//                if (dt.Rows[i]["trntagv"].ToString() == "1")
//                {
//                    oExpList.Add(oOrder1);
//                }

//            }

//            DataTable dtExp;
//            dt.DefaultView.RowFilter = "trntagv = 1";
//            dtExp = dt.DefaultView.ToTable();

//            var ItmSapu = from a in db.servitds
//                          join b in db.srvmains on new { Sapu_Comp = a.compcode, Sapu_Item = (int)a.itditem } equals new { Sapu_Comp = b.compcode, Sapu_Item = b.itemcode }
//                          join c in db.accounts on new { Sapu_Comp = a.compcode, _Code = (int)a.itdacctcd } equals new { Sapu_Comp = c.compcode, _Code = c.acctcode } into tmpUnit
//                          from c in tmpUnit.DefaultIfEmpty()
//                          join G in db.gathdets on a.itdgath equals G.gathcode into tmpGath
//                          from G in tmpGath.DefaultIfEmpty()
//                          join d in db.accounts on new { Sapu_Comp = a.compcode, Sapu_Item = b.itemname } equals new { Sapu_Comp = d.compcode, Sapu_Item = d.acctname } into tmpAcct
//                          from d in tmpAcct.DefaultIfEmpty()
//                          where a.itdcode == iMstcode && a.itdtype == iType && a.compcode == iCompcode
//                          select new
//                          {
//                              ItemID = a.itditem,
//                              ddlItem = b.itemname,
//                              Qty = Math.Abs((int)a.itdquan),
//                              Rate = a.itdrate,
//                              Amt = a.itdamou,
//                              Remark = a.itdrema,
//                              Cases = 0,
//                              UnitID = a.itdunit,
//                              unitname = "",// c.unitname,
//                              TaxPer = a.itdgstrtv,
//                              Tax_Amt = (a.itdamou * a.itdgstrtv) / 100,
//                              //ItemNetAmt: $('#ItemNetAmt').val(),
//                              acctcode = a.itdinde,// (int?) d.acctcode, 
//                              ItemRemark = a.itdrema,
//                              ItemNetAmt = a.itdamou + (a.itdamou * a.itdgstrtv) / 100,
//                              GSTStateVal = 2,
//                              TRate = G.gathwtdi,
//                              DisPer = G.gathqtdi,
//                              DisAmt = G.gathlabp,
//                              DisWT = G.gathdion,
//                              DisQtyAmt = G.gathcdam,
//                              PerDisTota = G.gathdscv,
//                              QtyDis = a.itdqtyd,
//                              FQty = G.gatlabg,
//                              BilledQty = G.gathwtdf ,
//                              SrvAccountHeadNM = c.acctname
//                          };


//            var sData = new { oList = oList, oItemList = ItmSapu, ExpList = oExpList };
//            return Json(Json(sData).Data, JsonRequestBehavior.AllowGet);

//        }

//        public string GetVoucherNo(string ChNo)
//        {
//            if (ChNo.Length == 1)
//                return "00" + ChNo;
//            else if (ChNo.Length == 2)
//                return "0" + ChNo;
//            else
//                return ChNo;
//        }


//#endregion 
    }
}







