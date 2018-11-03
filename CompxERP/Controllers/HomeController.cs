using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompxERP.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace CompxERP.Controllers
{
    public class HomeController : AsyncController
    {
        private SqlConnection conn;
        private SqlDataAdapter da;
        private DataTable dt;
        ERPDataContext db = new ERPDataContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {


                CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
                DataTable dt;
                dt = oSubmit.GetData("sp_AreaSignUp @useId='" + login.use_name.Trim() + "',@usePass='" + login.use_pass + "'", true);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //if (Convert.ToInt32(dt.Rows[0]["Useref"]) > 999)
                    //{
                    // Session["isLogin"] = "True";
                    Session["UserID"] = dt.Rows[0]["UseCode"].ToString();
                    Session["UserName"] = dt.Rows[0]["UseName"].ToString();
                    Session["CompCode"] = dt.Rows[0]["compcode"].ToString();
                    SessionMaster.CompCode = Convert.ToInt16(Session["CompCode"]);
                    Session["CompName"] = dt.Rows[0]["compname"].ToString();
                    Session["UserType"] = dt.Rows[0]["UseType"].ToString();
                    Session["UserTypes"] = dt.Rows[0]["useshowtr"].ToString();
                    if (dt.Rows[0]["UserNM"].ToString() != "") Session["UserNM"] = dt.Rows[0]["UserNM"].ToString(); else Session["UserNM"] = dt.Rows[0]["UseName"].ToString();
                    //return Redirect("/home/menuNew"); 

                    //************* User Logg ********** 
                    Session["LoggID"] = Guid.NewGuid().ToString();
                    oSubmit.InsLogg(Session["LoggID"].ToString(), Convert.ToInt32(dt.Rows[0]["UseCode"]), DateTime.Now, Convert.ToDateTime("01/01/1900"), this.Request.UserHostAddress.ToString());
                    //**********************************

                    return Redirect("/home/CompList?MstType=0");
                    //}
                    //else
                    //    ModelState.AddModelError("/Index/Index", "Invalid login credentials.");
                }
                else
                {
                    ModelState.AddModelError("/Index/Index", "Invalid login credentials.");
                    // Response.Redirect("~/Index/Index");
                }
            }
            return View(login);
        }



        public ActionResult Details(int compcode, string compname)
        {
            SessionMaster.CompCode = Convert.ToInt16(compcode);
            Session["CompCode"] = Convert.ToInt16(compcode);
            Session["CompName"] = Convert.ToString(compname);
            ViewBag.Company = compname;
            //return Redirect("/home/Allmenu");
            return Redirect("/home/menuNew");
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            //SessionManager.RemoveAll();
            HttpContext.Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            //************* User Logg ********** 
            clsSubmitModel oSubmit = new clsSubmitModel();
            oSubmit.InsLogg(Guid.NewGuid().ToString(), Convert.ToInt32(dt.Rows[0]["UseCode"]), DateTime.Now, Convert.ToDateTime("01/01/1900"), this.Request.UserHostAddress.ToString());
            //**********************************
            return RedirectToAction("Index", "Index");
        }



        public ActionResult Account()
        {
            return View();
        }

        //public ActionResult menuNew()
        //{

        //    var entities = new ERPDataContext();
        //    ViewBag.rootmenu = (from a in entities.menuoptions where a.mengrou == 0 && a.menpath != null select a).AsEnumerable();
        //    //ViewBag.adSubMenu = (from a in entities.menuoptions where a.mengrou == 731 && a.menpath != null select a).AsEnumerable();
        //   // ViewBag.EnSubMenu = (from a in entities.menuoptions where a.mengrou == 732 && a.menpath != null select a).AsEnumerable();
        //    ViewBag.reSubMenu = (from a in entities.menuoptions where a.mengrou == 733 && a.menpath != null select a).AsEnumerable();
        //    ViewBag.coSubMenu = (from a in entities.menuoptions where a.mengrou == 734 && a.menpath != null select a).AsEnumerable();
        //    ViewBag.fiSubMenu = (from a in entities.menuoptions where a.mengrou == 735 && a.menpath != null select a).AsEnumerable();
        //    ViewBag.seSubMenu = (from a in entities.menuoptions where a.mengrou == 736 && a.menpath != null select a).AsEnumerable();

        //    //ViewBag.urSubMenu = (from a in entities.menuoptions where a.mengrou == 737 && a.menpath != null select a).AsEnumerable();





        //    ViewBag.gnSubMenu = (from a in entities.menuoptions where a.mengrou == 738 && a.menpath != null select a).AsEnumerable();
        //    ViewBag.SuttoSubMenu = (from a in entities.menuoptions where a.mengrou == 781 && a.menpath != null select a).AsEnumerable();

        //    ViewBag.Company = Session["CompName"].ToString();
        //    ViewBag.usetype = Convert.ToInt32(Session["UserType"]);
        //    ViewBag.UserName = Session["UserName"];

        //    return View("menuNew");


        //}


        [HttpGet]
        public ActionResult SetSession(int session)
        {
            SessionMaster.CompCode = session;
            //Session["CompCode"] = session;
            return Json(session, JsonRequestBehavior.AllowGet);
        }


        public ActionResult LogOff()
        {
            //************* User Logg ********** 
            clsSubmitModel oSubmit = new clsSubmitModel();
            oSubmit.InsLogg(Session["LoggID"].ToString(), Convert.ToInt32(SessionMaster.UserID), DateTime.Now, DateTime.Now, this.Request.UserHostAddress.ToString());
            //**********************************

            Session["UserID"] = null;
            Session["UserName"] = null; //it's my session variable
            Session["Password"] = null;
            Session["CompName"] = null;
            Session["CompCode"] = null;
            Session.Clear();
            Session.Abandon();

            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Index/Index");
            return View();
        }

        public ActionResult Chk_TIN(string sTIN)
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();

            DataTable dt;
            dt = oSubmit.GetData("Select * from Company where compitno ='" + sTIN + "'", true);

            return Json(Json(dt.Rows.Count).Data, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Chk_GSTIN(string sGSTIN, int Type)
        //{
        //    //Type ==> 1 = Comp , 2 = Account

        //    CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();

        //    DataTable dt;
        //    if (Type == 1)
        //        dt = oSubmit.GetData("Select * from Company where compgstin ='" + sGSTIN + "'");
        //    else //if (Type == 2)
        //        dt = oSubmit.GetData("Select * from Account where acctgstin ='" + sGSTIN + "'");

        //    return Json(Json(dt.Rows.Count).Data, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult Chk_GSTIN(string sGSTIN, int Type, int iCode = 0)
        {
            //Type ==> 1 = Comp , 2 = Account
            string sAcctCode = "";
            if (iCode > 0) sAcctCode = " and acctdeal <>" + iCode;
            //if (iCode > 0) sAcctCode = " and acctcode <>" + iCode;

            ERPDataContext oDB = new ERPDataContext();

            clsSubmitModel oSubmit = new clsSubmitModel();

            DataTable dt;
            if (Type == 1)
                dt = oSubmit.GetData("Select * from Company where compgstin ='" + sGSTIN + "'", true);
            else
                dt = oSubmit.GetData("Select * from Account where acctgstin ='" + sGSTIN + "'" + sAcctCode, true);

            return Json(Json(dt.Rows.Count).Data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Chk_CST(string sCSTN, int Type)
        {
            //Type ==> 1 = Comp , 2 = Account

            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();

            DataTable dt;
            if (Type == 1)
                dt = oSubmit.GetData("Select * from Company where compcstn ='" + sCSTN + "'", true);
            else //if (Type == 2)
                dt = oSubmit.GetData("Select * from Account where acctgstin ='" + sCSTN + "'", true);

            return Json(Json(dt.Rows.Count).Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Chk_CT(string sCT, int Type)
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            DataTable dt;
            dt = oSubmit.GetData("Select * from Company where compctno ='" + sCT + "'", true);

            return Json(Json(dt.Rows.Count).Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Chk_User(string sUser)
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            DataTable dt;
            dt = oSubmit.GetData("Select * from LoginUsers where UseName ='" + sUser + "'", true);

            return Json(Json(dt.Rows.Count).Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult spInsUser(string usename, string usepass, int UserType = 0, string UserNM = "", string Remark = "")
        {
            //int oPropusetype = 0;
            int useshowtr = 0;
            int useref = SessionMaster.UserID;

            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();

            DataTable dt;

            if (Convert.ToInt32(oSubmit.GetSingleData("select  usecode from [loginusers] where usename ='" + usename + "'", "0", true)) > 0)
            {
                return Json(Json(2).Data, JsonRequestBehavior.AllowGet);
            }

            dt = oSubmit.GetData("select isnull(Max(usecode),0) +1 usecode from [loginusers]", true);

            oSubmit.spInsUser(SessionMaster.CompCode, Convert.ToInt32(dt.Rows[0]["usecode"].ToString()), UserType, useshowtr, usename, usepass, useref, UserNM, Remark);

            return Json(Json(1).Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangPass(string CurrPass, string NewPass)
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            DataTable dt;
            int iResult = 0;
            dt = oSubmit.GetData("sp_AreaSignUp @useId='" + Session["UserName"] + "',@usePass='" + CurrPass + "'", true);
            if (dt != null && dt.Rows.Count > 0)
            {
                iResult = 1;
                oSubmit.insertData("update [loginusers] set usepass = dbo.encrypt('" + NewPass + "') where usecode = " + SessionMaster.UserID, true);
            }
            else
            {
                iResult = 2;
            }
            return Json(Json(iResult).Data, JsonRequestBehavior.AllowGet);
        }


        public JsonResult FillSubMenu(int iMenu)//170513
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            DataTable dt;
            string query = "select MenCode Code, MenName Name from menuoption where mengrou = " + iMenu + " And menpath is not null";
            dt = oSubmit.GetData(query, true);

            var result = new List<KeyValuePair<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new KeyValuePair<string, string>(dr["Code"].ToString(), dr["Name"].ToString()));
            }
            var result3 = result.Where(s => s.Value.ToLower().Contains(query.ToLower())).Select(w => w).ToList();
            // return Json(result3, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult partnerlogin()
        {
            return View("partnerlogin");
        }

        [HttpPost]
        public ActionResult partnerlogin(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                clsSubmitModel oSubmit = new clsSubmitModel();
                DataTable dt;
                dt = oSubmit.GetData("dbo.sp_AreaSignUp @useId='" + login.use_name.Trim() + "',@usePass='" + login.use_pass + "'", true);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Session["UserID"] = dt.Rows[0]["UseCode"].ToString();
                    Session["UserName"] = dt.Rows[0]["UseName"].ToString();
                    Session["CompCode"] = dt.Rows[0]["compcode"].ToString();
                    SessionMaster.CompCode = Convert.ToInt16(Session["CompCode"]);
                    SessionMaster.UserID = Convert.ToInt16(Session["UserID"]);
                    SessionMaster.UserType = Convert.ToInt16(dt.Rows[0]["UseType"].ToString());
                    Session["CompName"] = dt.Rows[0]["compname"].ToString();
                    Session["UserType"] = dt.Rows[0]["UseType"].ToString();
                    Session["UserTypes"] = dt.Rows[0]["useshowtr"].ToString();
                    if (dt.Rows[0]["UserNM"].ToString() != "") Session["UserNM"] = dt.Rows[0]["UserNM"].ToString(); else Session["UserNM"] = dt.Rows[0]["UseName"].ToString();
                    Session["CurrentCulture"] = "1";
                    //ViewBag.DashName = sURL;

                    if (SessionMaster.UserType == 0 || SessionMaster.UserType == 1)
                        return Redirect("/Home/superDash");
                    else if (SessionMaster.UserType == 8)
                       return Redirect("/Home/EmpDash");
                    else
                        return Redirect("/Home/superDash");

                    //return Redirect(sURL);

                    // return Redirect("/home/partnerArea?MstType=0");
                }
                else
                {
                    ModelState.AddModelError("/Index/Index", "Invalid login credentials.");
                }
            }
            return View();


        }

        public JsonResult GetData()
        {
            //EmpmstDataContext o = new EmpmstDataContext();
            //o.GetStatistic(SessionMaster.CompCode);
            using (var db = new ERPDataContext())
            {
                var result = (from tags in db.GetStatistic(SessionMaster.CompCode)
                              orderby tags.Title ascending
                              select new { tags.Title, tags.Credits }).ToList();
                return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

            }
        }

        public string getPaymentID()
        {
            string conn = ConfigurationManager.ConnectionStrings["ConCRM"].ConnectionString;
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(ServiceID)+1 ,1)ServiceID from RateEnt", con);
            con.Open();
            string values = cmd.ExecuteScalar().ToString();
            con.Close();
            return values;
        }

        public ActionResult Dash()
        {
            ViewBag.UserType = "8";
            return View("Dash");
        }
        public ActionResult EmpDash()
        {

            if (SessionMaster.CompCode == 0) return Redirect("/home/partnerlogin");

            //clsSubmitModel oSubmit = new clsSubmitModel();
            //DataTable dt;
            //dt = oSubmit.GetData("exec GetSummary " + Session["UserType"] + " ,  " + Session["UserID"], true);
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    ViewBag.Total_Ticket = dt.Rows[0]["TotTicket"].ToString();
            //    ViewBag.Resolve_Ticket = dt.Rows[0]["ResolvTicket"].ToString();
            //    ViewBag.TotalLead = dt.Rows[0]["TotalLead"].ToString();
            //    ViewBag.TotalFolloUp = dt.Rows[0]["TotalFolloUp"].ToString();
            //    ViewBag.Bal_Ticket = Convert.ToInt32(dt.Rows[0]["TotTicket"].ToString()) - Convert.ToInt32(dt.Rows[0]["ResolvTicket"].ToString());
            //}

            ViewBag.UserName = Session["UserNM"];// Session["UserName"].ToString(); 
            ViewBag.UserType = Session["UserType"];

            //ViewBag.ItemNm = new SelectList(from res in db.itemains where res.itemgrou == 11 orderby res.itemname select new { res.itemcode, res.itemname }, "itemcode", "itemname");

            //var vwDepartment = from a in db.studdets where a.studType == 11 orderby a.studName select new { itgpName = a.studName, itgpcode = a.studCode };
            //ViewBag.vwDepartment = new SelectList(vwDepartment, "itgpcode", "itgpName");

            //ViewBag.adSubMenu = (from a in db.menuoptions join b in db.usermenusts on a.mencode equals b.mencode where b.menuser == SessionMaster.UserID && a.mengrou == 731 && b.menview == true && a.meniscrm == 1 orderby a.mensort select a).AsEnumerable();

            ViewBag.vbTransactions = (from a in db.menuoptions join b in db.usermenusts on a.mencode equals b.mencode where b.menuser == SessionMaster.UserID && a.mengrou == 732 && b.menview == true && a.meniscrm == 1 orderby a.mensort select a).AsEnumerable();

            ViewBag.vbReports = (from a in db.menuoptions join b in db.usermenusts on a.mencode equals b.mencode where b.menuser == SessionMaster.UserID && a.mengrou == 733 && b.menview == true && a.meniscrm == 1 orderby a.mensort select a).AsEnumerable();

          //  ViewBag.vbService = (from a in db.menuoptions join b in db.usermenusts on a.mencode equals b.mencode where b.menuser == SessionMaster.UserID && a.mengrou == 1333 && b.menview == true && a.meniscrm == 1 orderby a.mensort select a).AsEnumerable();

            return View("EmpDash");
        }


        public ActionResult superDash()
        {

            if (SessionMaster.CompCode == 0) return Redirect("/home/partnerlogin");

            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;
            dt = oSubmit.GetData("exec GetSummary " + Session["UserType"] + " ,  " + Session["UserID"], true);
            if (dt != null && dt.Rows.Count > 0)
            {
                ViewBag.Total_Ticket = dt.Rows[0]["TotTicket"].ToString();
                ViewBag.Resolve_Ticket = dt.Rows[0]["ResolvTicket"].ToString();
                ViewBag.TotalLead = dt.Rows[0]["TotalLead"].ToString();
                ViewBag.TotalFolloUp = dt.Rows[0]["TotalFolloUp"].ToString();
                ViewBag.Bal_Ticket = Convert.ToInt32(dt.Rows[0]["TotTicket"].ToString()) - Convert.ToInt32(dt.Rows[0]["ResolvTicket"].ToString());
            }

            ViewBag.UserName = Session["UserNM"];// Session["UserName"].ToString(); 
            ViewBag.UserType = Session["UserType"];

            ViewBag.ItemNm = new SelectList(from res in db.itemains where res.itemgrou == 11 orderby res.itemname select new { res.itemcode, res.itemname }, "itemcode", "itemname");

            var vwDepartment = from a in db.studdets where a.studType == 11 orderby a.studName select new { itgpName = a.studName, itgpcode = a.studCode };
            ViewBag.vwDepartment = new SelectList(vwDepartment, "itgpcode", "itgpName");

            ViewBag.adSubMenu = (from a in db.menuoptions join b in db.usermenusts on a.mencode equals b.mencode where b.menuser == SessionMaster.UserID && a.mengrou == 731 && b.menview == true && a.meniscrm == 1 orderby a.mensort select a).AsEnumerable();

            ViewBag.vbTransactions = (from a in db.menuoptions join b in db.usermenusts on a.mencode equals b.mencode where b.menuser == SessionMaster.UserID && a.mengrou == 732 && b.menview == true && a.meniscrm == 1 orderby a.mensort select a).AsEnumerable();

            ViewBag.vbReports = (from a in db.menuoptions join b in db.usermenusts on a.mencode equals b.mencode where b.menuser == SessionMaster.UserID && a.mengrou == 733 && b.menview == true && a.meniscrm == 1 orderby a.mensort select a).AsEnumerable();

            ViewBag.vbService = (from a in db.menuoptions join b in db.usermenusts on a.mencode equals b.mencode where b.menuser == SessionMaster.UserID && a.mengrou == 1333 && b.menview == true && a.meniscrm == 1 orderby a.mensort select a).AsEnumerable();

            return View("superDash");
        }

        //public ActionResult subDash()
        //{

        //    if (SessionMaster.CompCode == 0) return Redirect("/home/partnerlogin");

        //    clsSubmitModel oSubmit = new clsSubmitModel();
        //    DataTable dt;
        //    dt = oSubmit.GetData("exec GetSummary " + Session["UserType"] + " ,  " + Session["UserID"], true);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        ViewBag.Total_Ticket = dt.Rows[0]["TotTicket"].ToString();
        //        ViewBag.Resolve_Ticket = dt.Rows[0]["ResolvTicket"].ToString();
        //        ViewBag.TotalLead = dt.Rows[0]["TotalLead"].ToString();
        //        ViewBag.TotalFolloUp = dt.Rows[0]["TotalFolloUp"].ToString();
        //        ViewBag.Bal_Ticket = Convert.ToInt32(dt.Rows[0]["TotTicket"].ToString()) - Convert.ToInt32(dt.Rows[0]["ResolvTicket"].ToString());
        //    }

        //    ViewBag.UserName = Session["UserNM"];// Session["UserName"].ToString(); 
        //    ViewBag.UserType = Session["UserType"];

        //    ViewBag.ItemNm = new SelectList(from res in db.itemains where res.itemgrou == 11 orderby res.itemname select new { res.itemcode, res.itemname }, "itemcode", "itemname");

        //    var vwDepartment = from a in db.studdets where a.studType == 11 orderby a.studName select new { itgpName = a.studName, itgpcode = a.studCode };
        //    ViewBag.vwDepartment = new SelectList(vwDepartment, "itgpcode", "itgpName");

        //    ViewBag.adSubMenu = (from a in db.menuoptions join b in db.usermenusts on a.mencode equals b.mencode where b.menuser == SessionMaster.UserID && a.mengrou == 731 && b.menview == true && a.meniscrm == 1 orderby a.mensort select a).AsEnumerable();

        //    ViewBag.vbTransactions = (from a in db.menuoptions join b in db.usermenusts on a.mencode equals b.mencode where b.menuser == SessionMaster.UserID && a.mengrou == 732 && b.menview == true && a.meniscrm == 1 orderby a.mensort select a).AsEnumerable();

        //    ViewBag.vbReports = (from a in db.menuoptions join b in db.usermenusts on a.mencode equals b.mencode where b.menuser == SessionMaster.UserID && a.mengrou == 733 && b.menview == true && a.meniscrm == 1 orderby a.mensort select a).AsEnumerable();

        //    ViewBag.vbService = (from a in db.menuoptions join b in db.usermenusts on a.mencode equals b.mencode where b.menuser == SessionMaster.UserID && a.mengrou == 1333 && b.menview == true && a.meniscrm == 1 orderby a.mensort select a).AsEnumerable();

        //    return View("subDash");
        //}
        public ActionResult InsDistributor(string Dis_ID, string Dis_Country, string Dis_State, string Dis_City, string Dis_Add1, string Dis_Add2, string Dis_Add3, string Dis_pobox, string Dis_City_I, string Dis_Name, int DistributorID, string ContactPerson, string Mob1, string Mob2, string LandLine, string Email, string Skype, string GSTIN, int usetype, string usename, string usepass, string DealCode, string CityNM, string hdnDealerCode, string Brand, string Product, string ChequeNo, string CP_I, string CP_II, string CP_NO_I, string CP_NO_II, string DOB, string DOJ, string AnniDt)
        {
            var ITEMS = new { Dis_ID = 0, Deal_ID = 0 };
            try
            {
                commFunction oCom = new commFunction();
                clsSubmitModel oSubmit = new clsSubmitModel();
                account frm_Acct = new account();
                int CompStat = Convert.ToInt32(oSubmit.GetSingleData("select b.CityCode from Company a inner join cityDet b on a.compStat = b.cityName and Citytype = 67 where Compcode = " + SessionMaster.CompCode, "1", true));
                int AcctCode = Convert.ToInt32(oSubmit.GetSingleData("Select isnull(max(AcctCode),0)+1 MstCode from Account", "1", true));
                int GstA = 0;
                DataTable dt;
                dt = oSubmit.GetData("select isnull(Max(MstCode),0) +1 MstCode from tblDistributor", true);
                int MstCode = Convert.ToInt32(dt.Rows[0]["MstCode"].ToString());
                if (Convert.ToInt32(Dis_State) == CompStat) GstA = 0; else GstA = 1;

                if (hdnDealerCode != "")
                    MstCode = Convert.ToInt32(hdnDealerCode);

                oSubmit.InsDistributor(MstCode, DateTime.Now.Date, Dis_Country, Dis_State, Dis_City, Dis_Add1, Dis_Add2, Dis_Add3, Dis_pobox, Dis_City_I, DistributorID, Dis_Name, ContactPerson, Mob1, Mob2, LandLine, Email, Skype, GSTIN, usetype, usename, usepass, DealCode, CityNM, SessionMaster.UserID, Brand, Product, ChequeNo, CP_I, CP_II, CP_NO_I, CP_NO_II, DOB, DOJ, AnniDt);

                if (hdnDealerCode == "")
                {
                    frm_Acct.compcode = SessionMaster.CompCode;
                    frm_Acct.acctgrou = 21;
                    frm_Acct.acctcode = AcctCode;
                    frm_Acct.acctname = Dis_Name;
                    frm_Acct.acctcity = Convert.ToInt16(Dis_City_I);
                    frm_Acct.acctstat = Convert.ToInt32(Dis_State);
                    frm_Acct.acctcform = GstA;
                    frm_Acct.acctphon = Mob2;
                    frm_Acct.acctaddr = Dis_Add2;
                    frm_Acct.acctalia = DealCode;
                    frm_Acct.AcctDeal = MstCode;

                    frm_Acct.acctgstin = GSTIN;
                    frm_Acct.acctpanc = Dis_pobox;
                    frm_Acct.acctrema = "," + Dis_pobox + ",,";

                    db.accounts.InsertOnSubmit(frm_Acct);
                    db.SubmitChanges();

                    CodeGen oCode = new CodeGen();
                    //if (oCls.IsEdit != true)
                    //{
                    oCode.MstCode = AcctCode;
                    oCode.compcode = SessionMaster.CompCode;
                    oCode.msttype = 0;

                    oCode.mstdate = DateTime.Now.Date;
                    oCode.StDate = Convert.ToDateTime(oCom.getOpenDate(DateTime.Now.Date));
                    oCode.LastDate = Convert.ToDateTime(oCom.getClosDate(DateTime.Now.Date));
                    oSubmit.UpdCodeGen_API(oCode);
                }
                else
                {
                    var acc = (from a in db.accounts where a.acctgrou == 21 where a.AcctDeal == MstCode select a).SingleOrDefault();
                    acc.acctname = Dis_Name;
                    acc.acctcity = Convert.ToInt16(Dis_City_I);
                    acc.acctstat = Convert.ToInt32(Dis_State);
                    acc.acctcform = GstA;
                    acc.acctphon = Mob2;
                    acc.acctalia = DealCode;
                    acc.acctaddr = Dis_Add2;
                    acc.acctgstin = GSTIN;
                    acc.acctpanc = Dis_pobox;
                    acc.acctrema = "," + Dis_pobox + ",,";
                    db.SubmitChanges();
                }

                dt = oSubmit.GetData("select (select count(*)+1 from tblDistributor where DistributorID <> 0)Dealer,(select count(*)+1 from tblDistributor where DistributorID = 0)Distributor", true);

                return Json(new { Dis_ID = dt.Rows[0]["Distributor"].ToString(), Deal_ID = dt.Rows[0]["Dealer"].ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ITEMS = new { Dis_ID = 0, Deal_ID = 0 };
            }
            return Json(ITEMS, JsonRequestBehavior.AllowGet);
        }

        public ActionResult chkDealer(string Name, string City)
        {
            var ITEMS = new { DealCount = 0 };
            try
            {
                clsSubmitModel oSubmit = new clsSubmitModel();
                dt = oSubmit.GetData("select DisName from tblDistributor where DisName = '" + Name + "'  and isnull(cityID_I , 0) = " + City, true);

                return Json(new { DealCount = dt.Rows.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ITEMS = new { DealCount = 0 };
            }
            return Json(ITEMS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FillDistributor()
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            DataTable dt;
            string sType = "";
            if (SessionMaster.UserType == 3)
                sType = " and UseCode =" + SessionMaster.UserID;
            //else
            //    sType = " and AdminID =" + SessionMaster.UserID ;

            string query = "select DisName Name,MstCode Code from tblDistributor where DistributorID = 0 " + sType;
            dt = oSubmit.GetData(query, true);

            var result = new List<KeyValuePair<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new KeyValuePair<string, string>(dr["Code"].ToString(), dr["Name"].ToString()));
            }
            var result3 = result.Where(s => s.Value.ToLower().Contains(query.ToLower())).Select(w => w).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FillMaxNoDist()
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            DateTime dtFrom = Convert.ToDateTime("2018-04-01");
            DateTime dtTo = DateTime.Now;
            var ComplaintSolved = 0;
            var PendingComplaint = 0;
            var TotalComplaint = 0;
            var RejectComplaint6 = 0;
            var HaltedComplaint6 = 0;
            var AllotComplaint6 = 0;
            var UnAllotComplaint6 = 0;
            var OldCompalint6 = 0;
            var LogComplaint6 = 0;
            var BillSummary = "0";
			var NewComplaint = 0;//181030 %temp%

            if (SessionMaster.UserType == 0 || SessionMaster.UserType == 1)
            {

                var TotalSales = (from a in db.jourmsts where a.msttype == 42 select a.msttota).Count();
                var TotalSaleValue = string.Format("{0:0.00}", (from a in db.jourmsts where a.msttype == 42 select a.msttota).Sum() / 10000000) + " Cr";
                var TotalCollection = string.Format("{0:0.00}", (from a in db.jourtrns where a.trntype == 3 select a.trndram).Sum() / 10000000) + " Cr";
                ComplaintSolved = (from T_Compl in db.tblComplaints where T_Compl.StatusID == 2 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) select T_Compl).Count();
                PendingComplaint = (from T_Compl in db.tblComplaints where T_Compl.StatusID == 1 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) select T_Compl).Count();
                var RevenFromComplains = (from T_Compl in db.tblComplaints where T_Compl.StatusID == 2 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) select T_Compl.Charge).Sum() ?? 0;
                var Distributor = (from a in db.companies select a.compcode).Count();
                var Dealer = (from a in db.tblDistributors where a.DistributorID != 0 where a.ApprovID > 0 select a).Count();
                var Dispatch = (from a in db.itpursals where a.itdtype == 42 && (a.itddate >= dtFrom && a.itddate <= dtTo) select a.itdquan).Sum();
                var OrdeQty = (from a in db.ordemsts where a.msttype == 174 && (a.mstdate >= dtFrom && a.mstdate <= dtTo) select a.compcode).Count() + "/" + string.Format("{0:0.00}", (from a in db.ordemsts where a.msttype == 174 && (a.mstdate >= dtFrom && a.mstdate <= dtTo) select a.msttota).Sum() / 10000000) + " Cr";
NewComplaint = (from T_Compl in db.tblComplaints where T_Compl.cmIsRead == 1 && T_Compl.StatusID == 1 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) select T_Compl).Count();//181030_3
               AllotComplaint6 = (from T_Compl in db.tblComplaints where (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && T_Compl.EmpID >0 select T_Compl).Count();//181030
                TotalComplaint = (from T_Compl in db.tblComplaints where (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) select T_Compl).Count();//181030
                RejectComplaint6 = (from T_Compl in db.tblComplaints where T_Compl.StatusID == 3 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) select T_Compl).Count();//181030
                HaltedComplaint6 = (from T_Compl in db.tblComplaints where T_Compl.StatusID == 4 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) select T_Compl).Count();//181030
                UnAllotComplaint6 = (from T_Compl in db.tblComplaints where (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && (T_Compl.EmpID??0) == 0 select T_Compl).Count();//181030
                var ITEMS = new
                {
                    OrdeQty = OrdeQty,//dt.Rows[0]["OrdeQty"].ToString(),
                    Dispatch = Dispatch,//dt.Rows[0]["Dispatch"].ToString(),
                    Dis_ID = Distributor,// dt.Rows[0]["Distributor"].ToString(),
                    Deal_ID = Dealer,// dt.Rows[0]["Dealer"].ToString(),
                    TotalSales = TotalSales,//dt.Rows[0]["TotalSales"].ToString(),
                    TotalSaleValue = TotalSaleValue,// dt.Rows[0]["TotalSaleValue"].ToString(),
                    TotalCollection = TotalCollection,// dt.Rows[0]["TotalCollection"].ToString(),
                    ComplaintSolved = ComplaintSolved, //dt.Rows[0]["ComplaintSolved"].ToString(),
                    PendingComplaint = PendingComplaint, //dt.Rows[0]["PendingComplaint"].ToString(),
                    RevenFromComplains = RevenFromComplains,//dt.Rows[0]["RevenFromComplains"].ToString(),
                    Q3Target = 0,// dt.Rows[0]["Q3Target"].ToString(),
                    Other = 0,// dt.Rows[0]["Other"].ToString(),
                    AvaiStckQty = 0,// dt.Rows[0]["AvaiStckQty"].ToString(),
                    ReqStckQty = 0,// dt.Rows[0]["ReqStckQty"].ToString()
                    UnAllotComplaint6 = UnAllotComplaint6,
                    HaltedComplaint6 = HaltedComplaint6,
                    RejectComplaint6 = RejectComplaint6,
                    AllotComplaint6 = AllotComplaint6,
                    OldCompalint6 = OldCompalint6,
                    LogComplaint6 = LogComplaint6,
                    TotalComplaint = TotalComplaint,
                    BillSummary = BillSummary,
					NewComplaint=NewComplaint//181030
                };
                return Json(ITEMS, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var TotalSales = (from a in db.jourmsts where a.msttype == 42 && a.mstexec == SessionMaster.UserID select a.msttota).Count();
                var TotalSaleValue = 0;// string.Format("{0:0.00}", (from a in db.jourmsts where a.msttype == 42 && a.mstexec == SessionMaster.UserID select a.msttota).Sum() / 10000000) + " Cr";
                var TotalCollection = "0 Cr";// (from a in db.jourtrns
                //where a.trntype == 3
                //select a.trndram).Sum();
                /*{181029 %temp%*/
                
                switch (SessionMaster.UserType)
                {
                    case 4://Dealer
                        ComplaintSolved = (from T_Compl in db.tblComplaints
                                           join b in db.accounts on new { _Code = (int)T_Compl.DealerID } equals new { _Code = b.acctcode }
                                               into d
                                           from e in d.DefaultIfEmpty()
                                           where T_Compl.StatusID == 2 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && e.AcctUser == SessionMaster.UserID
                                           select T_Compl).Count();
                        PendingComplaint = (from T_Compl in db.tblComplaints
                                            join b in db.accounts on new { _Code = (int)T_Compl.DealerID } equals new { _Code = b.acctcode }
                                                into d
                                            from e in d.DefaultIfEmpty()
                                            where T_Compl.StatusID == 1 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && e.AcctUser == SessionMaster.UserID
                                            select T_Compl).Count();
                        TotalComplaint = (from T_Compl in db.tblComplaints
                                          join b in db.accounts on new { _Code = (int)T_Compl.DealerID } equals new { _Code = b.acctcode }
                                              into d
                                          from e in d.DefaultIfEmpty()
                                          where (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && e.AcctUser == SessionMaster.UserID
                                          select T_Compl).Count();
                        LogComplaint6 = (from T_Compl in db.tblComplaints
                                         join b in db.accounts on new { _Code = (int)T_Compl.DealerID } equals new { _Code = b.acctcode }
                                         into d
                                         from e in d.DefaultIfEmpty()
                                         where T_Compl.StatusID == 1 && T_Compl.CompDt >= dtTo && e.AcctUser == SessionMaster.UserID
                                         select T_Compl).Count();//181030_3
                        break;
                    case 68://Customer
                        ComplaintSolved = (from T_Compl in db.tblComplaints where T_Compl.StatusID == 2 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && T_Compl.CustID == SessionMaster.UserID select T_Compl).Count();
                        PendingComplaint = (from T_Compl in db.tblComplaints where T_Compl.StatusID == 1 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && T_Compl.CustID == SessionMaster.UserID select T_Compl).Count();
                        TotalComplaint = (from T_Compl in db.tblComplaints where (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && T_Compl.CustID == SessionMaster.UserID select T_Compl).Count();
                        LogComplaint6 = (from T_Compl in db.tblComplaints where T_Compl.CompDt == dtTo && T_Compl.CustID == SessionMaster.UserID select T_Compl).Count();
                        break;
                    case 6://Service Mgr
                        AllotComplaint6 = (from T_Compl in db.tblComplaints where (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && T_Compl.EmpID >0 select T_Compl).Count();
                        ComplaintSolved = (from T_Compl in db.tblComplaints where T_Compl.StatusID == 2 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo)  select T_Compl).Count();
                        PendingComplaint = (from T_Compl in db.tblComplaints where T_Compl.StatusID == 1 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo)  select T_Compl).Count();
                        TotalComplaint = (from T_Compl in db.tblComplaints where (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) select T_Compl).Count();
                        RejectComplaint6 = (from T_Compl in db.tblComplaints where T_Compl.StatusID == 3 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo)  select T_Compl).Count();
                        HaltedComplaint6 = (from T_Compl in db.tblComplaints where T_Compl.StatusID == 4 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo)  select T_Compl).Count();
                        
                        UnAllotComplaint6 = (from T_Compl in db.tblComplaints where (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && (T_Compl.EmpID??0) == 0 select T_Compl).Count();
                        NewComplaint = (from T_Compl in db.tblComplaints where T_Compl.cmIsRead == 1  && T_Compl.StatusID == 1 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) select T_Compl).Count();//181030_3
                        break;
                    case 7://Service Eng
                         PendingComplaint = (from T_Compl in db.tblComplaints where T_Compl.StatusID == 1 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && T_Compl.EmpID == SessionMaster.UserID select T_Compl).Count();//181031
                        OldCompalint6=(from T_Compl in db.tblComplaints where T_Compl.StatusID == 1 &&  (DateTime)T_Compl.CompDt <=Convert.ToDateTime(dtTo.ToString("MM/01/yyyy")) && T_Compl.EmpID == SessionMaster.UserID select T_Compl).Count();//181031
                        AllotComplaint6 = (from T_Compl in db.tblComplaints where (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && T_Compl.EmpID == SessionMaster.UserID select T_Compl).Count();
                        TotalComplaint = (from T_Compl in db.tblComplaints where (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && T_Compl.EmpID == SessionMaster.UserID select T_Compl).Count();
                        RejectComplaint6 = (from T_Compl in db.tblComplaints where T_Compl.StatusID == 3 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && T_Compl.EmpID == SessionMaster.UserID select T_Compl).Count();
                        HaltedComplaint6 = (from T_Compl in db.tblComplaints where T_Compl.StatusID == 4 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && T_Compl.EmpID == SessionMaster.UserID select T_Compl).Count();
                        ComplaintSolved = (from T_Compl in db.tblComplaints where T_Compl.StatusID == 2 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && T_Compl.EmpID == SessionMaster.UserID select T_Compl).Count();
                        BillSummary = string.Format("{0:0.00}", (from T_Compl in db.tblComplaints where T_Compl.StatusID == 1 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && T_Compl.EmpID == SessionMaster.UserID select T_Compl.Charge).Sum());
                        break;
                
                }
                var RevenFromComplains = 0;// (from T_Compl in db.tblComplaints where T_Compl.StatusID == 2 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && T_Compl.EmpID == SessionMaster.UserID select T_Compl.Charge).Sum();
                var Distributor = 0;// (from a in db.companies select a.compcode).Count();
                var Dealer = 0;// (from a in db.tblDistributors where a.DistributorID != 0 where a.ApprovID > 0 select a).Count();
                var Dispatch = 0;//(from a in db.itpursals
                //join b in db.jourmsts on new { _Comp = a.compcode, _type = a.itdtype, _Code = a.itdcode } equals new { _Comp = b.compcode, _type = b.msttype, _Code = b.mstcode }
                //where a.itdtype == 42
                //where b.mstexec == SessionMaster.UserID && (a.itddate >= dtFrom && a.itddate <= dtTo)
                //select a.itdquan).Sum();
                var OrdeQty = 0;//(from a in db.ordemsts where a.mstexec==SessionMaster.UserID && a.msttype == 174 && (a.mstdate >= dtFrom && a.mstdate <= dtTo) select a.compcode).Count() + "/" + (from a in db.ordemsts where a.msttype == 174 && (a.mstdate >= dtFrom && a.mstdate <= dtTo) select a.msttota).Sum();
                
                /*}181029 %temp%*/

                var ITEMS = new
                {
                    OrdeQty = OrdeQty,//dt.Rows[0]["OrdeQty"].ToString(),
                    Dispatch = Dispatch,//dt.Rows[0]["Dispatch"].ToString(),
                    Dis_ID = Distributor,// dt.Rows[0]["Distributor"].ToString(),
                    Deal_ID = Dealer,// dt.Rows[0]["Dealer"].ToString(),
                    TotalSales = TotalSales,//dt.Rows[0]["TotalSales"].ToString(),
                    TotalSaleValue = TotalSaleValue,// dt.Rows[0]["TotalSaleValue"].ToString(),
                    TotalCollection = TotalCollection,// dt.Rows[0]["TotalCollection"].ToString(),
                    ComplaintSolved = ComplaintSolved, //dt.Rows[0]["ComplaintSolved"].ToString(),
                    PendingComplaint = PendingComplaint, //dt.Rows[0]["PendingComplaint"].ToString(),
                    RevenFromComplains = RevenFromComplains,//dt.Rows[0]["RevenFromComplains"].ToString(),
                    Q3Target = 0,// dt.Rows[0]["Q3Target"].ToString(),
                    Other = 0,// dt.Rows[0]["Other"].ToString(),
                    AvaiStckQty = 0,// dt.Rows[0]["AvaiStckQty"].ToString(),
                    ReqStckQty = 0,// dt.Rows[0]["ReqStckQty"].ToString()
                    UnAllotComplaint6 = UnAllotComplaint6,
                    HaltedComplaint6 = HaltedComplaint6,
                    RejectComplaint6 = RejectComplaint6,
                    AllotComplaint6 = AllotComplaint6,
                    OldCompalint6 = OldCompalint6,
                    LogComplaint6 = LogComplaint6,
                    TotalComplaint = TotalComplaint,
                    BillSummary=BillSummary,
                    NewComplaint = NewComplaint
                };
                return Json(ITEMS, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult FillMaxNoDist1()
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            DataTable dt;

            if (SessionMaster.UserType == 0 || SessionMaster.UserType == 1)
            {
                dt = oSubmit.GetData("Exec dbo.GetStatistic @pFromDate='2018-04-01'", true);
            }
            else
            {
                dt = oSubmit.GetData("Exec dbo.GetStatisticEmp @pFromDate='2018-04-01',@pEmpCode='"+SessionMaster.UserID+"'", true);
            }
            var ITEMS = new
            {
                PendingDispatch = dt.Rows[0]["PendingDispatch"].ToString(),
                ApprQty = dt.Rows[0]["ApprQty"].ToString(),
                TotalOutstanding = dt.Rows[0]["TotalOutstanding"].ToString(),
                TotalDue = dt.Rows[0]["TotalDue"].ToString(),
            };
            return Json(ITEMS, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillDealer(int DistributorID=0, int CityID = 0, int StateID = 0)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> rowelement;

            DataTable dt;

            string sCity = ""; string sState = "";string sDistributorID = "";
            if(DistributorID>0)sDistributorID = " and DistributorID =" + DistributorID;
            if (CityID > 0) sCity = " and CityID_I =" + CityID;
            if (StateID > 0) sState = " and StateID =" + StateID;

            dt = oSubmit.GetData("select MstCode ,DealCode ,DisName + ' ' +  ISNULL(b.CityName ,'') + ' ' +  ISNULL(C.CityName ,'') DisName   ,ContactPerson ,Mob1,Mob2,Email,Add_I ,Skype ,CityNM City  ,b.CityName CityNM ,c.CityName StateNM ,GSTIN  from tblDistributor a left Join cityDet b on b.cityType = 15 and a.CityID_I = b.CityCode left Join cityDet c on c.cityType = 67 and a.StateID = c.CityCode  where ApprovID >0 "+sDistributorID + sCity + sState + " Order By DisName ", true);

            //var rows = from a in db.tblDistributors join b in db.citydets on new { _Code = a.StateID ,_Type = 67  } equals new { _Code = b.citycode ,_Type = b.cityType } into c from d in c.DefaultIfEmpty()  join e in db.citydets on new { _Code = a.StateID ,_Type = 15 } equals new { _Code = a.ci ,_Type = 67  } 

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    rowelement = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        rowelement.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(rowelement);
                }
            }
            return Json(rows, JsonRequestBehavior.AllowGet);

        }

        public ActionResult FillDistributorDet(int MstCode)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            var model = oSubmit.GetDistributor("select convert(varchar(10), DOB ,103)DOB , convert(varchar(10), DOJ ,103)DOJ ,convert(varchar(10), AnniDt ,103)AnniDt ,MstCode,MstDate,DistributorID,DisName,CountryID,StateID,CityID,CityID_I,Add_I, Add_II,Add_III,Add_IV,PO_Box,ContactPerson,Mob1,Mob2,LandLine,Email,Skype, GSTIN,DealCode,CityNM,UseCode,AdminID,Brand,Product,ChequeNo,CP_I,CP_II, CP_NO_I,CP_NO_II from tblDistributor where MstCode =" + MstCode);
            return Json(model, JsonRequestBehavior.AllowGet);

        }





        public ActionResult InsDepartment(int Department_ID, string Designation)
        {

            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();

            int MstCode = 0;
            DataTable dt;
            dt = oSubmit.GetData("select isnull(Max(studCode),0)+1 studCode from studdet where studType = 12", true);
            MstCode = Convert.ToInt32(dt.Rows[0]["studCode"].ToString());

            oSubmit.insertData("insert into studdet (StudType , studCode , studname ,studadd1  ) values ( 12 ," + MstCode + ",'" + Designation + "'," + Department_ID + ")", true);

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FillLeadNo(int EmpCode)
        {

            //var F_LeadNo = from a in db.tblFollows
            //join Mst in db.ordemsts on
            //new { ID = a.L_ID.ToString(), TP = 1147 } equals
            //new { ID = Mst.mstrefc.ToString(), TP = (int)Mst.msttype }
            //where a.Status_I == 1
            //where Mst.mstrefc == null
            //orderby a.L_No
            //select new { itgpName = a.L_No, itgpcode = a.L_ID };

            var F_LeadNo = from a in db.ordemsts where a.msttype == 999999 select new { itgpName = a.mstchno, itgpcode = a.mstcode }; ;

            if (EmpCode > 0)
            {
                F_LeadNo = from a in db.ordemsts
                           join Mst in db.tblFollows on
                           new { ID = a.mstcode, TP = 1 } equals
                           new { ID = (int)Mst.L_ID, TP = (int)Mst.Status_I } into c
                           from d in c.DefaultIfEmpty()
                           where a.msttype == 1147
                           where d.L_ID == null
                           where a.mstuser == EmpCode
                           orderby a.mstchno
                           select new { itgpName = a.mstchno, itgpcode = a.mstcode };
            }
            else
            {
                F_LeadNo = from a in db.ordemsts
                           join Mst in db.tblFollows on
                           new { ID = a.mstcode, TP = 1 } equals
                           new { ID = (int)Mst.L_ID, TP = (int)Mst.Status_I } into c
                           from d in c.DefaultIfEmpty()
                           where a.msttype == 1147
                           where d.L_ID == null
                           orderby a.mstchno
                           select new { itgpName = a.mstchno, itgpcode = a.mstcode };
            }

            ViewBag.F_LeadNo = new SelectList(F_LeadNo, "itgpcode", "itgpName");

            return Json(F_LeadNo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FillDegignationList(int iDept = 0)
        {
            //string sDept = "";
            //if (iDept > 0) sDept = " and a.studadd1 = " + iDept;

            var DegignationList = from a in db.studdets
                                  join b in db.studdets on new { _Code = Convert.ToInt32(a.studadd1) } equals new { _Code = (int)b.studCode }
                                  where a.studType == 12 && b.studType == 11 && Convert.ToInt32(a.studadd1) == iDept
                                  select new { StudName = a.studName, studadd2 = b.studName, studCode = a.studCode };

            //var DegignationList = db.Database.SqlQuery<studdet1>("select a.StudName StudName,b.StudName studadd2 ,a.studCode from studdet a inner join  studdet b on a.studadd1 = b.studcode and b.studType = 11 where a.studType = 12" + sDept);


            return Json(DegignationList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserList()
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();

            List<clsUser> lstFollow = oSubmit.UserList("select usecode ,usename , usetype ,usestatus , dbo.Decrypt(a.usePass) PassWord ,  case usetype when 0 then 'Supper Admin'  when 1 then 'Admin'  when 2 then 'Partner'  when 3 then 'Distributor'  when 4 then 'Dealer'   when 5 then 'Tele Caller'  when 6 then 'Service Mgr'  when 7 then 'Service Eng'  when 8 then 'Employee - Admin'  when 9 then 'Client'   when 10 then 'Employee ,Sale'  when 11 then 'Employee ,Tech Supp'  end UserType from loginusers a");

            return PartialView("_UserList", lstFollow);
        }

        public JsonResult GetUserCount()
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();

            DataTable dt;
            dt = oSubmit.GetData("select (select Count(*) from loginusers where UseType = 3)Tot_Dist ,(select Count(*) from loginusers where UseType = 4) Tot_Deal , (select Count(*) from loginusers where UseType in (8,10,11)) Tot_Emp", true);

            var ITEMS = new { Tot_Dist = dt.Rows[0]["Tot_Dist"].ToString(), Tot_Deal = dt.Rows[0]["Tot_Deal"].ToString(), Tot_Emp = dt.Rows[0]["Tot_Emp"].ToString() };

            return Json(ITEMS, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdUserDet(int usecode, string PassWord, string Usename)
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            oSubmit.insertData("update loginusers set usepass= dbo.Encrypt('" + PassWord + "') ,usename ='" + Usename + "' where usecode = " + usecode, true);

            return Json("1", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChkUser(string sUserName)
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            DataTable dt = oSubmit.GetData("select * from loginusers where Usename = '" + sUserName + "'", true);

            return Json(dt.Rows.Count, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ImportInBound(EmployeeSetup emp)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            int ii = 0;

            string sMsg = "";
            string sPath = ""; DateTime dDate = DateTime.Now;
            try
            {

                var File = emp.ImageFile;
                if (File != null)
                {
                    //LineNo = 1;
                    var sFileName = Path.GetFileName(File.FileName);
                    var sExt = Path.GetExtension(File.FileName);
                    var sFileNameWithoutExt = Path.GetFileNameWithoutExtension(File.FileName);
                    //LineNo = 2;

                    sPath = Server.MapPath("~/UploadImg/" + Convert.ToString(DateTime.Now.Ticks) + "_" + File.FileName);
                    //sPath = "CRMcloud.in/UploadImg/" + Convert.ToString(DateTime.Now.Ticks) + "_" + File.FileName;
                    //LineNo = 3;

                    //sPath = "E:/UploadImg/" + Convert.ToString(DateTime.Now.Ticks) + "_" + File.FileName;
                    //sPath = Server.MapPath("~/UploadImg/" + File.FileName );
                    File.SaveAs(sPath);
                    // LineNo = 4;

                    //sPath = ("~/UploadImg/" + File.FileName);
                }

                DataTable dt;
                commFunction oCommFn = new commFunction();
                //dt = oCommFn.GetDataTableExcel("E:\\InBound.xls", "InBound$");
                dt = oCommFn.GetDataTableExcel(sPath, "Calling$");
                // LineNo = 5;

                ERPDataContext db = new ERPDataContext();
                tblCallMst frm = new tblCallMst();

                int EmpID = 0;
                int CityID = 0;
                int StateID = 0;
                string EmpNM = "";
                string CityNM = "";
                string StateNM = "";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Mobile"].ToString() != "" && (from a in db.tblCallMsts where a.Mobile == dt.Rows[i]["Mobile"].ToString() select a).Count() == 0)
                    {
                        if (EmpNM != dt.Rows[i]["Executive"].ToString())
                            EmpID = Convert.ToInt32((from a in db.employees where a.empname == dt.Rows[i]["Executive"].ToString() select a.UseCode).FirstOrDefault());

                        if (CityNM != dt.Rows[i]["City"].ToString())
                            CityID = Convert.ToInt32((from a in db.citydets where a.cityname == dt.Rows[i]["City"].ToString() where a.cityType == 15 select a.citycode).FirstOrDefault());
                        if (StateNM != dt.Rows[i]["State"].ToString())
                            StateID = Convert.ToInt32((from a in db.citydets where a.cityname == dt.Rows[i]["State"].ToString() where a.cityType == 67 select a.citycode).FirstOrDefault());

                        EmpNM = dt.Rows[i]["Executive"].ToString();
                        CityNM = dt.Rows[i]["City"].ToString();
                        StateNM = dt.Rows[i]["State"].ToString();

                        frm = new tblCallMst();
                        frm.Add1 = dt.Rows[i]["Add1"].ToString();
                        frm.Add2 = dt.Rows[i]["Add2"].ToString();
                        frm.InqName = dt.Rows[i]["Name"].ToString();
                        frm.Mobile = dt.Rows[i]["Mobile"].ToString();
                        frm.Email = dt.Rows[i]["Email"].ToString();
                        frm.Mobile1 = dt.Rows[i]["Mobile1"].ToString();

                        frm.Product = dt.Rows[i]["Product"].ToString();
                        frm.ExeName = dt.Rows[i]["Executive"].ToString();
                        frm.CityNM = dt.Rows[i]["City"].ToString();

                        frm.UseCode = EmpID;
                        frm.CityID = CityID;
                        frm.StateID = StateID;

                        db.tblCallMsts.InsertOnSubmit(frm);
                        db.SubmitChanges();
                    }
                }
                sMsg = "Import Successfully !";
                return Json(sMsg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DealerImport(EmployeeSetup emp)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            int ii = 0;

            string sMsg = "";
            string sPath = ""; DateTime dDate = DateTime.Now;
            try
            {
                var File = emp.ImageFile;
                if (File != null)
                {
                    //LineNo = 1;
                    var sFileName = Path.GetFileName(File.FileName);
                    var sExt = Path.GetExtension(File.FileName);
                    var sFileNameWithoutExt = Path.GetFileNameWithoutExtension(File.FileName);
                    //LineNo = 2;

                    sPath = Server.MapPath("~/UploadImg/" + Convert.ToString(DateTime.Now.Ticks) + "_" + File.FileName);

                    File.SaveAs(sPath);

                }

                DataTable dt;
                commFunction oCommFn = new commFunction();

                dt = oCommFn.GetDataTableExcel(sPath, "Dealer$");

                ERPDataContext db = new ERPDataContext();

                tblDistributor frm = new tblDistributor();
                loginuser frm_Log = new loginuser();
                EmpAllotMst frm_Allot = new EmpAllotMst();
                account frm_Acct = new account();
                string sErr = "";
                int EmpID = 0; int GstA = 0;
                int CityID = 0; int BrandID = 0;
                int StateID = 0;
                int AllotID = Convert.ToInt32(oSubmit.GetSingleData("Select isnull(max(MstCode),0) MstCode from EmpAllotMst", "1", true));
                string CompStat = oSubmit.GetSingleData("select compStat from Company where Compcode = " + SessionMaster.CompCode, "1", true).ToString();
                int MstCode = Convert.ToInt32(oSubmit.GetSingleData("Select isnull(max(MstCode),0)+1 MstCode from tblDistributor", "1", true));
                int UseCode = Convert.ToInt32(oSubmit.GetSingleData("Select isnull(max(UseCode),0)+1 MstCode from loginusers", "1", true));
                int AcctCode = Convert.ToInt32(oSubmit.GetSingleData("Select isnull(max(AcctCode),0)+1 MstCode from Account", "1", true));
                string EmpNM = ""; string BrandNM = "";
                string CityNM = "";
                string StateNM = "";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Name"].ToString() == "")
                        continue;

                    try
                    {
                        db = new ERPDataContext();
                        AllotID += 1;

                        if (EmpNM != dt.Rows[i]["Executive"].ToString())
                            EmpID = Convert.ToInt32((from a in db.employees where a.empname == dt.Rows[i]["Executive"].ToString() select a.empcode).FirstOrDefault());

                        if (CityNM != dt.Rows[i]["City"].ToString())
                            CityID = Convert.ToInt32((from a in db.citydets where a.cityname == dt.Rows[i]["City"].ToString() where a.cityType == 15 select a.citycode).FirstOrDefault());
                        if (StateNM != dt.Rows[i]["State"].ToString())
                            StateID = Convert.ToInt32((from a in db.citydets where a.cityname == dt.Rows[i]["State"].ToString() where a.cityType == 67 select a.citycode).FirstOrDefault());

                        if (BrandNM != dt.Rows[i]["Brand"].ToString())
                            BrandID = Convert.ToInt32((from a in db.studdets where a.studName == dt.Rows[i]["Brand"].ToString() where a.studType == 61 select a.studCode).FirstOrDefault());

                        EmpNM = dt.Rows[i]["Executive"].ToString();
                        CityNM = dt.Rows[i]["City"].ToString();
                        StateNM = dt.Rows[i]["State"].ToString();
                        BrandNM = dt.Rows[i]["Brand"].ToString();

                        if (StateNM == CompStat) GstA = 0; else GstA = 1;

                        frm_Log = new loginuser();
                        frm_Log.usecode = UseCode;
                        frm_Log.usename = "U" + GetDealerNo(UseCode.ToString());
                        frm_Log.usetype = 4;
                        frm_Log.usepass = "t";
                        frm_Log.Compcode = 62;
                        frm_Log.UserNM = dt.Rows[i]["Name"].ToString();
                        db.loginusers.InsertOnSubmit(frm_Log);

                        frm = new tblDistributor();
                        frm.MstCode = MstCode;
                        frm.DistributorID = 1;
                        frm.DealCode = "D" + GetDealerNo(MstCode.ToString());
                        frm.DisName = dt.Rows[i]["Name"].ToString();
                        frm.Grade = dt.Rows[i]["Grade"].ToString();
                        frm.Mob2 = dt.Rows[i]["Mobile"].ToString();
                        frm.Add_I = dt.Rows[i]["Address_1"].ToString();
                        frm.Add_II = dt.Rows[i]["Address_2"].ToString();
                        frm.ExecID = EmpID;
                        frm.Brand = BrandID.ToString();
                        frm.ApprovID = 1;
                        frm.UseCode = UseCode;
                        frm.CityID = CityID;
                        frm.CityID_I = CityID;
                        frm.StateID = StateID;
                        frm.Email = dt.Rows[i]["Email"].ToString();
                        frm.GSTIN = dt.Rows[i]["GSTIN"].ToString();
                        frm.Mob1 = dt.Rows[i]["Phone"].ToString();

                        db.tblDistributors.InsertOnSubmit(frm);

                        frm_Allot = new EmpAllotMst();
                        frm_Allot.EmpID = EmpID;
                        frm_Allot.DealerID = MstCode;
                        frm_Allot.MstCode += AllotID;
                        db.EmpAllotMsts.InsertOnSubmit(frm_Allot);

                        //frm_Acct = new account();
                        //frm_Acct.compcode = SessionMaster.CompCode;
                        //frm_Acct.acctgrou = 21;
                        //frm_Acct.acctcode = AcctCode;
                        //frm_Acct.acctname = dt.Rows[i]["Name"].ToString();
                        //frm_Acct.acctcity = (Int16)CityID;
                        //frm_Acct.acctstat = StateID;
                        //frm_Acct.acctcform = GstA;
                        //frm_Acct.acctphon = dt.Rows[i]["Mobile"].ToString();
                        //frm_Acct.acctaddr = dt.Rows[i]["Address_1"].ToString();

                        //frm_Acct.acctgstin = dt.Rows[i]["GSTIN"].ToString();
                        //frm_Acct.acctpanc = dt.Rows[i]["Pan"].ToString();

                        //frm_Acct.acctalia = "D" + GetDealerNo(MstCode.ToString());
                        //frm_Acct.AcctDeal = MstCode;
                        //frm_Acct.AcctUser = UseCode;
                        //db.accounts.InsertOnSubmit(frm_Acct);

                        MstCode = MstCode + 1;
                        UseCode = UseCode + 1;
                        AcctCode = AcctCode + 1;

                        db.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        sErr += ex.Message.ToString();
                    }
                }

                if (sErr != "") sMsg = "Import Successfully ! With Some Issue - " + sErr; else sMsg = "Import Successfully ! " + sErr;

                return Json(sMsg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public string GetDealerNo(string ChNo)
        {
            if (ChNo.Length == 1)
                return "000" + ChNo;
            else if (ChNo.Length == 2)
                return "00" + ChNo;
            else if (ChNo.Length == 3)
                return "0" + ChNo;
            else
                return ChNo;
        }

        [HttpPost]
        public ActionResult OpeningImport(EmployeeSetup emp)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();

            string sMsg = "";
            string sPath = ""; DateTime dDate = DateTime.Now;
            try
            {
                var File = emp.ImageFile;
                if (File != null)
                {
                    var sFileName = Path.GetFileName(File.FileName);
                    var sExt = Path.GetExtension(File.FileName);
                    var sFileNameWithoutExt = Path.GetFileNameWithoutExtension(File.FileName);

                    sPath = Server.MapPath("~/UploadImg/" + Convert.ToString(DateTime.Now.Ticks) + "_" + File.FileName);
                    File.SaveAs(sPath);
                }

                DataTable dt;
                commFunction oCommFn = new commFunction();

                dt = oCommFn.GetDataTableExcel(sPath, "Opening$");

                ERPDataContext db = new ERPDataContext();
                tblOpening frm = new tblOpening();

                int DealerID = 0;
                string[] sDealerNM;


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Particulars"].ToString() == "" && (dt.Rows[i]["Debit"].ToString() != "" && dt.Rows[i]["Credit"].ToString() != ""))
                        continue;
                    sDealerNM = dt.Rows[i]["Particulars"].ToString().Split(',');

                    DealerID = Convert.ToInt32((from a in db.tblDistributors where a.DistributorID != 0 where a.DisName.Replace(" ", "") == sDealerNM[0].ToString().Replace(" ", "") select a.MstCode).FirstOrDefault());

                    if (DealerID > 0)
                    {
                        frm = new tblOpening();
                        frm.UseCode = SessionMaster.UserID;
                        frm.DealerID = DealerID;
                        if (dt.Rows[i]["Debit"].ToString() != "") frm.Opening = Convert.ToDecimal(dt.Rows[i]["Debit"].ToString());
                        if (dt.Rows[i]["Credit"].ToString() != "") frm.Closing = Convert.ToDecimal(dt.Rows[i]["Credit"].ToString());
                        frm.CreatedOn = DateTime.Now;

                        db.tblOpenings.InsertOnSubmit(frm);
                        db.SubmitChanges();
                    }
                }

                sMsg = "Import Successfully !";
                return Json(sMsg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult LedgerImport(EmployeeSetup emp)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();

            string sMsg = "";
            string sPath = ""; DateTime dDate = DateTime.Now;
            try
            {
                var File = emp.ImageFile;
                if (File != null)
                {
                    var sFileName = Path.GetFileName(File.FileName);
                    var sExt = Path.GetExtension(File.FileName);
                    var sFileNameWithoutExt = Path.GetFileNameWithoutExtension(File.FileName);

                    sPath = Server.MapPath("~/UploadImg/" + Convert.ToString(DateTime.Now.Ticks) + "_" + File.FileName);
                    File.SaveAs(sPath);
                }

                DataTable dt;
                commFunction oCommFn = new commFunction();

                dt = oCommFn.GetDataTableExcel(sPath, "Ledger$");

                ERPDataContext db = new ERPDataContext();
                tblLedger frm = new tblLedger();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["VchNo"].ToString() == "")
                        continue;

                    frm = new tblLedger();
                    frm.HeadDes = dt.Rows[i]["HeadDescription"].ToString();
                    frm.VchNo = dt.Rows[i]["VchNo"].ToString();
                    frm.VchTp = dt.Rows[i]["VchType"].ToString();
                    // if (dt.Rows[i]["VchDt"].ToString() != "") frm.VchDt = Convert.ToDateTime(oSubmit.GetDateFormat(dt.Rows[i]["VchDt"].ToString()));
                    if (dt.Rows[i]["VchDt"].ToString() != "") frm.VchDt = Convert.ToDateTime(dt.Rows[i]["VchDt"].ToString());
                    frm.Particulars = dt.Rows[i]["Particulars"].ToString();
                    frm.Narration = dt.Rows[i]["Narration"].ToString();
                    frm.GSTType = dt.Rows[i]["GSTType"].ToString();
                    if (dt.Rows[i]["Cr"].ToString() != "") frm.CrAmt = Convert.ToDecimal(dt.Rows[i]["Cr"].ToString());
                    if (dt.Rows[i]["Dr"].ToString() != "") frm.DrAmt = Convert.ToDecimal(dt.Rows[i]["Dr"].ToString());
                    frm.CreatedOn = DateTime.Now;

                    db.tblLedgers.InsertOnSubmit(frm);
                    db.SubmitChanges();
                }

                sMsg = "Import Successfully !";
                return Json(sMsg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult TestMail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendMail(string EmailFrom, string EmailTo, string Pass, string User, string Msg, int Port, string Srv)
        {
            try
            {
                //SmtpClient smtpClient = new SmtpClient("mi3-wts5.a2hosting.com", Port); // 25 mi3-wtr1.supercp.com
                SmtpClient smtpClient = new SmtpClient(Srv, Port); // 25
                smtpClient.Credentials = new System.Net.NetworkCredential(User, Pass);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage mailMessage = new MailMessage(EmailFrom, EmailTo);

                mailMessage.Subject = "Ledger";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = Msg;

                smtpClient.Send(mailMessage);

                // Msg += "Send Successfully";

                return Json(new { Success = true, Message = "Send Success..." });
            }
            catch (Exception EX)
            {
                return Json(new { Success = false, Message = "Something went wrong. Please try again. " + EX.Message });
            }

        }
        public ActionResult FillReference(int RefTypeID = 0)
        {
            if (RefTypeID == 1)
            {
                var DegignationList = from a in db.tblDistributors where a.DistributorID == 0 select new { studCode = a.MstCode, studName = a.DisName };
                return Json(DegignationList, JsonRequestBehavior.AllowGet);
            }
            else if (RefTypeID == 2)
            {
                var DegignationList = from a in db.tblDistributors where a.DistributorID != 0 select new { studCode = a.MstCode, studName = a.DisName };
                //var DegignationList = db1.Database.SqlQuery<studdet1>(("select MstCode studCode,DisName StudName from tblDistributor where DistributorID <> 0"));
                return Json(DegignationList, JsonRequestBehavior.AllowGet);
            }
            else if (RefTypeID == 3)
            {
                var DegignationList = from a in db.loginusers where a.usetype == 12 select new { studCode = a.usecode, studName = a.usename };
                return Json(DegignationList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var DegignationList = from a in db.loginusers where (a.usetype == 8 || a.usetype == 10 || a.usetype == 11) select new { studCode = a.usecode, studName = a.usename };
                return Json(DegignationList, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);

        }

        public JsonResult getChartData(int iType)
        {

            ERPDataContext oDB = new ERPDataContext();
            clsSubmitModel oSubmit = new clsSubmitModel();

            DataSet ds;

            ds = oSubmit.GetGraph(SessionMaster.CompCode, iType);

            List<clsPoItem> lstGraph = new List<clsPoItem>();
            List<clsPoItem> lstStatistic = new List<clsPoItem>();

            lstGraph = Utility.BindList<clsPoItem>(ds.Tables[0]);
            lstStatistic = Utility.BindList<clsPoItem>(ds.Tables[1]);

            var oList = new { lstGraph = lstGraph, lstStatistic = lstStatistic };

            return Json(oList, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetDueListEmp()
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel(); 
            int CompCode = 0; string AcctList = ""; int State = 0; int City = 0; int Due = 0; int Commit = 0; int Planning = 0;
            List<DueList> lstOrder = oSubmit.GetDueListEmp(CompCode, AcctList, State, City, true, Due, Commit, Planning , SessionMaster.UserID ,0);
             
            return PartialView("PayFollowUpEmp", lstOrder);
        }

    }

}



