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
 
namespace CompxERP.Controllers
{
    [UserSessionfilter]
    public class VoucherController :Controller
    {
        //
        // GET: /Voucher/

        ERPDataContext db = new ERPDataContext();
        public ActionResult Index()
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            DataTable dt;
            int comp = Convert.ToInt32(SessionMaster.CompCode);
            if (comp == 0)
            { 
                //Response.Redirect("~/Index/Index");
                //TempData["message"] = "Please Select Company ...";
            }
            dt = oSubmit.GetVoucher(Convert.ToInt32(Request.QueryString["MstType"]), Convert.ToInt32(SessionMaster.CompCode));
            ViewBag.MstType = Request.QueryString["MstType"];
            ViewBag.MenCode = Request.QueryString["mencode"];

            Jourmaster oModel = new Jourmaster();
            List<CompxERP.Models.Jourmaster> oList = new List<CompxERP.Models.Jourmaster>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                oModel = new Models.Jourmaster();
                oModel.mstcode = Convert.ToInt32(dt.Rows[i]["mstcode"]);
                oModel.mstdate = Convert.ToDateTime(dt.Rows[i]["mstdate"]);
                oModel.ChallanDate = Convert.ToDateTime(dt.Rows[i]["mstdate"]).ToString("dd/MM/yyyy");
                oModel.msttota = Convert.ToDecimal(dt.Rows[i]["msttota"]);
                oModel.mstchno = dt.Rows[i]["mstChno"].ToString();
                oModel.mstrema = dt.Rows[i]["mstrema"].ToString();
                oModel.mstclno = dt.Rows[i]["mstclno"].ToString();
                oModel.mstchnm = dt.Rows[i]["mstchnm"].ToString();
                oModel.compcode = Convert.ToInt32(dt.Rows[i]["compcode"]);
                oModel.msttype = Convert.ToInt32(dt.Rows[i]["msttype"]);

                oList.Add(oModel);
            }
            oModel.ListArea = oList;
            return View(oModel);
        }

        [HttpGet]
        public ActionResult Create(int comp = 0, int id = 0, int mstCode = 0, int MstType = 0, int mstcode_Print = 0, int MenCode = 0)
        {
            if (mstcode_Print == 0)
            {
                TempData["message"] = null;
            }

            //TempData["CurrentCulture"] = Session["CurrentCulture"];
              
            comp = Convert.ToInt32(SessionMaster.CompCode);
            if (comp == 0)
            {
                Response.Redirect("~/Index/Index");
                //TempData["message"] = "Please Select Company ...";
            }

            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            ViewBag.MstType = Request.QueryString["MstType"];
            Jourmaster frm = new Jourmaster();

            //************************** Rights ****************************
            DataTable dt;
            dt = oSubmit.GetData("select * from usermenust where mencode =" + Request.QueryString["MenCode"] + " and menuser =" + SessionMaster.UserID, true);
            //if (dt.Rows.Count > 0)
            //{
            //    if (Convert.ToBoolean(dt.Rows[0]["menview"]) == false) Response.Redirect("../home/menuNew");
            //    frm.menaddi = Convert.ToBoolean(dt.Rows[0]["menaddi"]);
            //    frm.menedit = Convert.ToBoolean(dt.Rows[0]["menedit"]);
            //    frm.menview = Convert.ToBoolean(dt.Rows[0]["menview"]);
            //    frm.mencode = Convert.ToInt32(dt.Rows[0]["menCode"]);
            //    ViewBag.MenCode = frm.mencode;
            //}
            //**************************************************************

            if (MstType == 3)
                frm.Header = "Receipt Entry";
            else if (MstType == 5) { if (Session["CurrentCulture"].ToString() == "1" || Session["CurrentCulture"].ToString() == "0") frm.Header = "Payment Entry"; //else frm.Header = "إدخال الدفع"; 
            }
            else if (MstType == 6)
                frm.Header = "Journal Voucher";
            else if (MstType == 108)
                frm.Header = "Contra Entry";
            else
                frm.Header = "";


            if (mstCode > 0)
            {
                #region  "Edit"
                dt = oSubmit.GetVoucher(MstType, comp, mstCode);

                if (dt.Rows.Count > 0)
                {
                    ViewBag.MstType = frm.msttype = Convert.ToInt32(dt.Rows[0]["MstType"]);
                    frm.compcode = Convert.ToInt32(dt.Rows[0]["Compcode"]);
                    ViewBag.MstCode = frm.mstcode = Convert.ToInt32(dt.Rows[0]["mstCode"]);

                    frm.mstdate = Convert.ToDateTime(dt.Rows[0]["mstDate"]);
                    frm.ChallanDate = Convert.ToDateTime(dt.Rows[0]["mstdate"]).ToString("dd/MM/yyyy");

                    if (dt.Rows[0]["mstpdc"].ToString() != "")
                    {
                        frm.mstpdc = Convert.ToInt32(dt.Rows[0]["mstpdc"]);
                        if (frm.mstpdc == 1) frm.IsPostDt = true; else frm.IsPostDt = false;
                    }
                    if (dt.Rows[0]["mstactpay"].ToString() != "")
                    {
                        frm.mstactpay = Convert.ToInt32(dt.Rows[0]["mstactpay"]);
                        if (frm.mstactpay == 1) frm.IsAcctPay = true; else frm.IsAcctPay = false;
                    }
                    if (dt.Rows[0]["mstchqadj"].ToString() != "") frm.mstchqadj = Convert.ToInt32(dt.Rows[0]["mstchqadj"]);
                    if (dt.Rows[0]["mststat"].ToString() != "") frm.mststat = Convert.ToInt32(dt.Rows[0]["mststat"]);
                    if (dt.Rows[0]["msttime"].ToString() != "") frm.msttime = Convert.ToInt32(dt.Rows[0]["msttime"]);
                    //frm.msttimes = frm.msttime.ToString();
                    if (dt.Rows[0]["msttota"].ToString() != "") frm.msttota = Convert.ToInt32(dt.Rows[0]["msttota"]);
                    frm.mstrema = dt.Rows[0]["mstrema"].ToString();
                    frm.mstchno = dt.Rows[0]["mstchno"].ToString();
                    if (dt.Rows[0]["mstneta"].ToString() != "") frm.mstneta = Convert.ToInt32(dt.Rows[0]["mstneta"]);
                    frm.mstclno = dt.Rows[0]["mstclno"].ToString();

                    frm.mstchnm = dt.Rows[0]["mstchnm"].ToString();

                    frm.IsEdit = true;

                    if (frm.mstchqadj == 1) frm.IsChqAdj = true; else frm.IsChqAdj = false;

                    return View("Create", frm);
                }
                #endregion
            }
            else
            {
                #region "Create"
                var res = from k in db.accounts where k.compcode == (6) select k;
                ViewBag.tpPartyID = new SelectList(res, "acctcode", "acctname");
                //var comdt = from k in db.companies where k.compcode == 6 select new { k.compopdt };
                //dt = oSubmit.GetData("select compopdt from company where compcode =6");
                //frm.msttimes = DateTime.Now.ToString("HH:mm:ss");
                DataTable dt2;

                string sMstChNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd")  ;

                dt2 = oSubmit.GetData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from jourmst where compcode ='" + comp + "' and msttype ='" + MstType + "'", true);
                ViewBag.MstCode = frm.mstcode = Convert.ToInt32(dt2.Rows[0]["mstcode"]);

                dt2 = oSubmit.GetData("select isnull(max(right(mstchno,2))+1,1) as maxchno  from jourmst where compcode = " + comp + " and msttype = " + MstType + " and left(mstchno, 6) = '" + sMstChNo + "'", true);
            ViewBag.mstchno  =  frm.mstchno =sMstChNo + GetVoucherNo(dt2.Rows[0]["maxchno"].ToString());

                frm.compcode = Convert.ToInt32(SessionMaster.CompCode);
                frm.type = MstType;
                frm.mstdate = DateTime.Now.Date;
                frm.ChallanDate = DateTime.Now.Date.ToString("dd/MM/yyyy");

                frm.mstcode_Print = mstcode_Print;
                frm.IsEdit = false;

                #endregion
            }
            return View("Create", frm);
        }

        //[HttpPost]
        //public ActionResult Create(Jourmaster oCls)
        //{

        //    clsSubmitModel oSubmit = new clsSubmitModel();
        //    commFunction oCom = new commFunction();
        //    try
        //    {
        //        string sPartyNm = "";
        //        int iMode = 1;
        //        var json = oCls.sJourTrn;
        //        clsPoItem ItemDet = JsonConvert.DeserializeObject<clsPoItem>(json);
        //        ViewBag.MstType = oCls.msttype;
        //        if (oCls.IsPostDt == true) oCls.mstpdc = 1; else oCls.mstpdc = 0;
        //        if (oCls.IsAcctPay == true) oCls.mstactpay = 1; else oCls.mstactpay = 0;
        //        if (oCls.IsChqAdj == true) oCls.mstchqadj = 1; else oCls.mstchqadj = 0;
        //        oCls.mststat = 0;
        //        //oCls.msttime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
        //        oCls.mstrefc = getParty(Convert.ToInt32(oCls.mstprtc)) + "~0~0~0";
        //        oCls.mstdate = oSubmit.GetDateFormat(oCls.ChallanDate);


        //        if (Convert.ToInt16(SessionMaster.CompCode) == 0)
        //        {
        //            Response.Redirect("~/Index/Index");
        //        }

        //        oSubmit.BeginTran();

        //        if (oCls.IsEdit != true)
        //        {
        //            if (Request.QueryString["MstCode"] != null && Convert.ToInt32(Request.QueryString["MstCode"]) > 0) { }
        //            else
        //            {
        //                oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from jourmst where compcode =" + SessionMaster.CompCode + " and msttype =" + oCls.msttype + ""));
        //            }

        //            iMode = 0;
        //            oCls.StDate = Convert.ToDateTime(oCom.getOpenDate(DateTime.Now.Date));
        //            oCls.LastDate = Convert.ToDateTime(oCom.getClosDate(DateTime.Now.Date));
        //            oSubmit.UpdCodeGen(oCls);
        //        }

        //        oSubmit.InsJourmst(oCls);

        //        jourtrn oTrn = new jourtrn();
        //        oSubmit.insertData("delete from jourtrn  where compcode = " + oCls.compcode + " and trntype = " + oCls.msttype + " and  trncode = " + oCls.mstcode);
        //        for (int i = 0; i < ItemDet.LstItem.Count; i++)
        //        {
        //            sPartyNm = ItemDet.LstItem[0].partyname;
        //            oTrn.compcode = Convert.ToInt16(oCls.compcode);
        //            oTrn.trntype = Convert.ToInt32(oCls.msttype);
        //            oTrn.trncode = Convert.ToInt32(oCls.mstcode);
        //            oTrn.trntime = Convert.ToInt32(oCls.msttime);
        //            oTrn.trnsrno = Convert.ToInt16(i + 1);
        //            oTrn.trndate = Convert.ToDateTime(oCls.mstdate);
        //            oTrn.trnledg = oTrn.trnitem = Convert.ToInt32(ItemDet.LstItem[i].tpPartyID);
        //            oTrn.trndram = Convert.ToInt32(ItemDet.LstItem[i].tpDrAmt);
        //            oTrn.trncram = Convert.ToInt32(ItemDet.LstItem[i].tpCrAmt);
        //            oTrn.trnrema = ItemDet.LstItem[i].Remark.ToString();
        //            oSubmit.InsJourTrn(oTrn);
        //        }

        //        //********************************* User Work ********************************* 
        //        clsUserWork oUser = new clsUserWork();
        //        oUser.woruser = SessionMaster.UserID;
        //        oUser.wormode = iMode;
        //        oUser.worcomp = SessionMaster.CompCode;
        //        oUser.wortype = oCls.msttype;
        //        oUser.worcode = oCls.mstcode;
        //        oUser.worsrno = oSubmit.GetUsWoCode();
        //        oUser.worrema = "D-" + sPartyNm + "#" + oCls.ChallanDate;
        //        oUser.wordate = oCls.mstdate;
        //        oUser.worrfsr = "";
        //        oUser.worsysn = Environment.MachineName;
        //        oUser.IP_Add = this.Request.UserHostAddress;
        //        //oUser.WorChNo = oCls.mstchno;
        //        //oUser.WorNarr = oCls.mstrema;
        //        oSubmit.InsUserWork(oUser);
        //        //********************************* User Work ********************************* 

        //        oSubmit.Commit();

        //        oCls.mstcode_Print = oCls.mstcode;

        //        TempData["message"] = "Saved Successfully ...";
        //        oCls.mstcode = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        oSubmit.RollBack();
        //        TempData["message"] = ex.Message;
        //        return View();
        //    }
        //    return RedirectToAction("Create", new { MstType = oCls.msttype, MstCode = oCls.mstcode, comp = oCls.compcode, mstcode_Print = oCls.mstcode_Print, MenCode = Request.QueryString["MenCode"] });
        //}
         
         public string GetVoucherNo(string ChNo) { 
             if (ChNo.Length == 1)
                 return "00" + ChNo;
             else if (ChNo.Length == 2)
                 return "0" + ChNo;
            else
                 return ChNo;
    }
        //[HttpPost]
        //public ActionResult Create(Jourmaster oCls, string Action)
        //{  
        //    if (Action.Equals("Save"))
        //    {
        //        Save(oCls);
        //    }
        //    else if (Action.Equals("Prev"))
        //    {
        //        PrevNext(oCls.compcode, oCls.mstcode - 1, oCls.mstcode - 1, oCls.msttype);
        //    }
        //    else if (Action.Equals("Next"))
        //    {
        //        PrevNext(oCls.compcode, oCls.mstcode + 1, oCls.mstcode + 1, oCls.msttype);
        //    }
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Save(Jourmaster oCls)
        //{

        //    clsSubmitModel oSubmit = new clsSubmitModel();
        //    commFunction oCom = new commFunction();
        //    try
        //    {
        //        var json = oCls.sJourTrn;

        //        clsPoItem ItemDet = JsonConvert.DeserializeObject<clsPoItem>(json);

        //        ViewBag.MstType = oCls.msttype;

        //        if (oCls.IsPostDt == true) oCls.mstpdc = 1; else oCls.mstpdc = 0;
        //        if (oCls.IsAcctPay == true) oCls.mstactpay = 1; else oCls.mstactpay = 0;
        //        if (oCls.IsChqAdj == true) oCls.mstchqadj = 1; else oCls.mstchqadj = 0;
        //        oCls.mststat = 0;
        //        //oCls.msttime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
        //        oCls.mstrefc = getParty(Convert.ToInt32(oCls.mstprtc)) + "~0~0~0";
        //        oCls.mstdate = oSubmit.GetDateFormat(oCls.ChallanDate);

        //        oSubmit.BeginTran();
        //        oSubmit.InsJourmst(oCls);

        //        if (oCls.IsEdit != true)
        //        {
        //            oCls.StDate = Convert.ToDateTime(oCom.getOpenDate(DateTime.Now.Date));
        //            oCls.LastDate = Convert.ToDateTime(oCom.getClosDate(DateTime.Now.Date));
        //            oSubmit.UpdCodeGen(oCls);
        //        }

        //        jourtrn oTrn = new jourtrn();
        //        oSubmit.insertData("delete from jourtrn  where compcode = " + oCls.compcode + " and trntype = " + oCls.msttype + " and  trncode = " + oCls.mstcode);
        //        for (int i = 0; i < ItemDet.LstItem.Count; i++)
        //        {
        //            oTrn.compcode = Convert.ToInt16(oCls.compcode);
        //            oTrn.trntype = Convert.ToInt32(oCls.msttype);
        //            oTrn.trncode = Convert.ToInt32(oCls.mstcode);
        //            oTrn.trntime = Convert.ToInt32(oCls.msttime);
        //            oTrn.trnsrno = Convert.ToInt16(i + 1);
        //            oTrn.trndate = Convert.ToDateTime(oCls.mstdate);

        //            oTrn.trnledg = oTrn.trnitem = Convert.ToInt32(ItemDet.LstItem[i].tpPartyID);
        //            oTrn.trndram = Convert.ToInt32(ItemDet.LstItem[i].tpDrAmt);
        //            oTrn.trncram = Convert.ToInt32(ItemDet.LstItem[i].tpCrAmt);
        //            oTrn.trnrema = ItemDet.LstItem[i].Remark.ToString();


        //            oSubmit.InsJourTrn(oTrn);
        //        }
        //        oSubmit.Commit();

        //        TempData["message"] = "Saved Successfully ...";
        //        oCls.mstcode = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        oSubmit.RollBack();
        //        TempData["message"] = ex.Message;
        //        return View();
        //    }
        //    return RedirectToAction("Create", new { MstType = oCls.msttype, MstCode = oCls.mstcode, comp = oCls.compcode });
        //}

        public ActionResult PrevNext(int comp = 0, int mstCode = 0, int MstType = 0, bool IsNext = true)
        {

            if (Convert.ToInt16(SessionMaster.CompCode) == 0) { Response.Redirect("~/Index/Index"); } 

            string MSG = "";
            if (IsNext == true) mstCode = mstCode + 1; else mstCode = mstCode - 1;

            //DBEntity oDB = new DBEntity();
            comp = Convert.ToInt32(SessionMaster.CompCode);
            if (comp == 0)
            {
                TempData["message"] = "Please Select Company ...";
            }

            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            Jourmaster frm = new Jourmaster();
            frm.mstcode = mstCode;
            frm.msttype = MstType;
            frm.compcode = comp;
            DataTable dt;
            dt = oSubmit.GetVoucher(MstType, comp, mstCode);
            if (mstCode > 0)
            {
                if (dt.Rows.Count > 0)
                {
                    ViewBag.MstType = frm.msttype = Convert.ToInt32(dt.Rows[0]["MstType"]);
                    frm.compcode = Convert.ToInt32(dt.Rows[0]["Compcode"]);
                    // ViewBag.MstCode = frm.mstcode = Convert.ToInt32(dt.Rows[0]["mstCode"]);
                    frm.mstdate = Convert.ToDateTime(dt.Rows[0]["mstDate"]);
                    frm.ChallanDate = Convert.ToDateTime(dt.Rows[0]["mstdate"]).ToString("dd/MM/yyyy");

                    if (dt.Rows[0]["mstpdc"].ToString() != "")
                    {
                        frm.mstpdc = Convert.ToInt32(dt.Rows[0]["mstpdc"]);
                        if (frm.mstpdc == 1) frm.IsPostDt = true; else frm.IsPostDt = false;
                    }
                    if (dt.Rows[0]["mstactpay"].ToString() != "")
                    {
                        frm.mstactpay = Convert.ToInt32(dt.Rows[0]["mstactpay"]);
                        if (frm.mstactpay == 1) frm.IsAcctPay = true; else frm.IsAcctPay = false;
                    }
                    if (dt.Rows[0]["mstchqadj"].ToString() != "") frm.mstchqadj = Convert.ToInt32(dt.Rows[0]["mstchqadj"]);
                    if (dt.Rows[0]["mststat"].ToString() != "") frm.mststat = Convert.ToInt32(dt.Rows[0]["mststat"]);
                    if (dt.Rows[0]["msttime"].ToString() != "") frm.msttime = Convert.ToInt32(dt.Rows[0]["msttime"]);
                    //frm.msttimes = frm.msttime.ToString();
                    if (dt.Rows[0]["msttota"].ToString() != "") frm.msttota = Convert.ToInt32(dt.Rows[0]["msttota"]);
                    frm.mstrema = dt.Rows[0]["mstrema"].ToString();
                    frm.mstchno = dt.Rows[0]["mstchno"].ToString();
                    if (dt.Rows[0]["mstneta"].ToString() != "") frm.mstneta = Convert.ToInt32(dt.Rows[0]["mstneta"]);
                    frm.mstclno = dt.Rows[0]["mstclno"].ToString();

                    frm.mstchnm = dt.Rows[0]["mstchnm"].ToString();

                    if (frm.mstchqadj == 1) frm.IsChqAdj = true; else frm.IsChqAdj = false;


                }
                else
                {
                    var MaxMstCode = 0;
                    MaxMstCode = (db.jourmsts.Where(x => x.compcode == SessionMaster.CompCode).Where(x => x.msttype == MstType).Max(x => x.mstcode));
                    if (mstCode > MaxMstCode)
                    {
                      frm.MSG = "This is last record ...";
                    }
                    else
                    {
                        frm.MSG = "Record Not Found ...";

                    }

                }
            }
 else
            {
                frm.MSG = "This is First record ..."; 
                    }

            return Json(Json(frm ).Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVoucherDet(int iMstcode, int iCompcode, int iType)
        {

            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();

            DataTable dt;
            dt = oSubmit.GetVoucher(iType, iCompcode, iMstcode);

            List<CompxERP.Models.clsPoItem> oList = new List<CompxERP.Models.clsPoItem>();
            CompxERP.Models.clsPoItem oOrder1 = new CompxERP.Models.clsPoItem();

            jourtrn oTrn = new jourtrn();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                oOrder1 = new Models.clsPoItem();

                oOrder1.tpPartyID = Convert.ToInt32(dt.Rows[i]["tpPartyID"]);
                oOrder1.partyname = dt.Rows[i]["partyname"].ToString();
                oOrder1.Remark = dt.Rows[i]["Remark"].ToString();
                oOrder1.tpDrAmt = Convert.ToInt32(dt.Rows[i]["tpDrAmt"]);
                oOrder1.tpCrAmt = Convert.ToInt32(dt.Rows[i]["tpCrAmt"]);
                if (dt.Rows[i]["acctgrou"].ToString() != "") oOrder1.acctgrou = Convert.ToInt32(dt.Rows[i]["acctgrou"]); else oOrder1.acctgrou = 0;
                oList.Add(oOrder1);

            }

            return Json(Json(oList).Data, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Print(int comp = 0, int mstCode = 0, int msttype = 0)//170826
        {
            clsSubmitModel oSubmit = new clsSubmitModel();

            DataTable dt = oSubmit.GetVoucher(msttype, comp, mstCode);
            Session["Voucher"] = dt;
            return View("~/Views/Shared/Report.aspx");
        }

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

        public ActionResult NewAccount()
        {
            return View();
        }

        public ActionResult AddAccount(string Name, string Address, string Mobile, string GSTIN, int AcGroup)
        {
            var max = 0;
            try
            {
                CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
                if (Convert.ToInt16(SessionMaster.CompCode) == 0) { Response.Redirect("~/Index/Index"); } 

                max = db.accounts.Max(i => i.acctcode);
                max = max + 1;

                oSubmit.insertData("Insert into account (acctcode , compcode ,Acctname ,AcctAddr ,acctgstin ,acctphon ,acctgrou )Values (" + max + "," + SessionMaster.CompCode + ",'" + Name + "','" + Address + "','" + GSTIN + "','" + Mobile + "' , " + AcGroup + " ) ");
            }
            catch (Exception ex)
            {
                return Json(Json(0).Data, JsonRequestBehavior.AllowGet);
            }

            return Json(Json(max).Data, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult CheckVoucher(string sVoucher, string MstType ,int MstCode )
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel(); 
            DataTable dt2;

            dt2 = oSubmit.GetData("select count(*) chno  from jourmst where compcode = " + SessionMaster.CompCode + " and msttype = " + MstType + " and mstchno = '" + sVoucher + "' and mstcode <> " + MstCode, true); 
            return Json(dt2.Rows[0]["chno"], JsonRequestBehavior.AllowGet);

        }
    }
}
