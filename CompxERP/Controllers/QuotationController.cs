using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompxERP.Models;
using CompxERP.Filters;

using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace CompxERP.Controllers
{
    [UserSessionfilter]
    public class QuotationController : Controller
    {
        //
        // GET: /Quotation/

        ERPDataContext DB = new ERPDataContext();
        clsSubmitModel oSubmit = new clsSubmitModel();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ParkLead(int comp = 0, int id = 0, int mstCode = 0, int MstType = 0, int mstcode_Print = 0)
        {

            Jourmaster frm = new Jourmaster();
            try
            {
                string sURL = "";
                if (Session["UserType"].ToString() == "0")
                    sURL = "/Home/superDash?MstType=0";
                else if (Session["UserType"].ToString() == "2")
                    sURL = "/Home/partnerArea?MstType=0";
                else if (Session["UserType"].ToString() == "3")
                    sURL = "/home/DistributorDash?MstType=0";
                else if (Session["UserType"].ToString() == "4")
                    sURL = "/home/DealerDash?MstType=0";
                else if (Session["UserType"].ToString() == "10")
                    sURL = "/home/EmployeeDashboard?MstType=0";
                else if (Session["UserType"].ToString() == "11")
                    sURL = "/home/TaxDash";

                ViewBag.DashName = sURL;

                clsSubmitModel oSubmit = new clsSubmitModel();
                //ERPDataContext oDB = new ERPDataContext();
                comp = SessionMaster.CompCode;
                if (comp == 0)
                {
                    TempData["message"] = "Please Select Company ...";
                }

                MstType = 1147;
                DataTable dt2;
                int iMaxCode = 0;
                dt2 = oSubmit.GetData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from OrdeMst where compcode ='" + SessionMaster.CompCode + "' and msttype = " + MstType, true);
                iMaxCode = Convert.ToInt32(dt2.Rows[0]["mstcode"]);


                ViewBag.MstType = MstType;

                ViewBag.ItemNm = new SelectList(from res in DB.itemains orderby res.itemname select new { res.itemcode, res.itemname }, "itemcode", "itemname");

                //var CityNm = db1.Database.SqlQuery<citydet>("select  citycode  ,  cityname from citydet where citytype =15 and cityrute = 1257 order by cityname");
                var CityNm = from a in DB.citydets where a.cityType == 15 where a.cityrute == 1257 orderby a.cityname select new { a.citycode, a.cityname };
                ViewBag.vwCityNm = new SelectList(CityNm, "citycode", "cityname");

                var vwCate = from a in DB.itgroups where a.itgpunde == 0 where a.compcode == SessionMaster.CompCode select new { a.itgpname, a.itgpcode };
                ViewBag.vwCategory = new SelectList(vwCate, "itgpcode", "itgpName");

                var vwState = from a in DB.citydets where a.cityType == 67 && a.cityrute == 3 orderby a.cityname select new { a.citycode, a.cityname };
                ViewBag.vwState = new SelectList(vwState, "citycode", "cityname");

                if (mstCode > 0 && mstCode < iMaxCode)
                {
                    #region  "Edit"
                    var model = oSubmit.GetInquiry(MstType, comp, mstCode, Convert.ToInt32(Session["UserID"]));

                    model.PartyID = model.mstcust;
                    model.ChallanDate = Convert.ToDateTime(model.mstDueDate).ToString("dd/MM/yyyy");
                    model.sMstdate = Convert.ToDateTime(model.mstdate).ToString("dd/MM/yyyy");
                    ViewBag.MstCode = model.mstcode;
                    model.acctname = model.msternv;

                    model.EMail = model.mstlotno;
                    model.NewMobile = model.mstsection;

                    ViewBag.mstsale = new SelectList(from res in DB.studdets where res.studType == 860 orderby res.studName select new { res.studCode, res.studName }, "studCode", "studName", model.mstsale);
                    ViewBag.SubCate = new SelectList(from res in DB.itgroups where res.compcode == SessionMaster.CompCode orderby res.itgpname select new { res.itgpcode, res.itgpname }, "itgpcode", "itgpname");
                    ViewBag.itemnumb = new SelectList(from res in DB.studdets where res.studType == 61 select new { res.studCode, res.studName }, "studcode", "studname");

                    //return View("ParkLead", model);
                    model.IsEdit = true;

                    return PartialView(model);
                    #endregion
                }
                else
                {
                    #region "Create"

                    frm.msttimes = DateTime.Now.ToString("HH:mm:ss");

                    ViewBag.MstCode = frm.mstcode = iMaxCode;
                    //frm.mstcode = Convert.ToInt32(dt2.Rows[0]["mstcode"]);
                    frm.compcode = Convert.ToInt32(SessionMaster.CompCode);
                    frm.msttype = frm.type = MstType;
                    frm.mstdate = DateTime.Now.Date;
                    frm.mstcode_Print = mstcode_Print;
                    ViewBag.mstsale = new SelectList(from res in DB.studdets where res.studType == 860 select new { res.studCode, res.studName }, "studCode", "studName");
                    ViewBag.SubCate = new SelectList(from res in DB.itgroups where res.compcode == SessionMaster.CompCode orderby res.itgpname select new { res.itgpcode, res.itgpname }, "itgpcode", "itgpname");
                    ViewBag.itemnumb = new SelectList(from res in DB.studdets where res.studType == 61 select new { res.studCode, res.studName }, "studcode", "studname");

                    dt2 = oSubmit.GetData("SELECT count(*)+1 Total from OrdeMst where compcode =" + SessionMaster.CompCode + " and msttype = 1147 and Mstuser = " + Session["UserID"], true);
                    frm.mstprtc = Convert.ToInt32(dt2.Rows[0]["Total"].ToString());

                    string sMstChNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");

                    dt2 = oSubmit.GetData("select isnull(max(right(mstchno,2))+1,1) as maxchno  from OrdeMst where compcode = " + comp + " and msttype = " + MstType + " and left(mstchno, 6) = '" + sMstChNo + "'", true);
                    ViewBag.mstchno = frm.mstchno = sMstChNo + GetVoucherNo(dt2.Rows[0]["maxchno"].ToString());

                    frm.IsEdit = false;

                    #endregion
                }
                // return View("ParkLead", frm);
                return PartialView("ParkLead", frm);
            }
            catch
            {
                return View("ParkLead", frm);
                // return View();
            }
        }

        public string GetVoucherNo(string ChNo)
        {
            if (ChNo.Length == 1)
                return "00" + ChNo;
            else if (ChNo.Length == 2)
                return "0" + ChNo;
            else
                return ChNo;
        }

        [HttpPost]
        public ActionResult ParkLead(Jourmaster oCls)
        {
            DataTable dt2;
            clsSubmitModel oSubmit = new clsSubmitModel();
            commFunction oCom = new commFunction();
            int IsSave = 0;
            string sMstChNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");

            try
            {
                jourtrn oTrn = new jourtrn();
                sapuitd oSapu = new sapuitd();
                gathdet oGath = new gathdet();

                string sGathCode = "";
                string sItmTbl = "ordeItd";



                //dt2 = oSubmit.GetData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from jourmst where compcode ='" + comp + "' and msttype ='" + MstType + "'");

                if (Request.QueryString["MstCode"] != null && Convert.ToInt32(Request.QueryString["MstCode"]) > 0) { }
                else
                {
                    oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from Ordemst where compcode ='" + SessionMaster.CompCode + "' and msttype ='" + oCls.msttype + "'"));
                }

                var json = oCls.sItemDet;
                clsPoItem ItemDet = JsonConvert.DeserializeObject<clsPoItem>(json);

                ViewBag.MstType = oCls.msttype;
                int iDays = 0;
                decimal iComm = 0, iInterest = 0;

                oCls.mstdate = oSubmit.GetDateFormat(oCls.sMstdate);



                if (oCls.mstdued > 0) iDays = (int)oCls.mstdued;//DueDay 170831
                if (oCls.Comm > 0) iComm = oCls.Comm;
                if (oCls.Interest > 0) iInterest = oCls.Interest;

                if (oCls.mstchnH == null) oCls.mstchnH = "";
                oCls.mstdepa = oCls.mststat = 0;
                //oCls.msttime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));

                if (oCls.msttype != 1163)
                {
                    oCls.mstDueDate = oSubmit.GetDateFormat(oCls.ChallanDate);  //oCls.mstdate.AddDays(iDays);//170831
                    oCls.mstrefc = getParty(Convert.ToInt32(oCls.PartyID)) + "~" + iInterest + "~" + iDays + "~" + iComm;
                }

                oCls.mstchno = oCls.mstchnH + oCls.mstchnV;//170830
                oCls.mstexti = "~~#0";//170830

                oCls.mstreftag = oCls.mstchno + "~0~0~0~0~0~0~0";

                oCls.mstgncd = "0~0~" + oCls.acctledg + "~0";//170830

                oCls.mstAppr = 0; oCls.mstqtyd = 0; oCls.mstvat1 = 0; oCls.mstvat2 = 0;
                oCls.mstvat3 = 0; oCls.mstchnm = ""; oCls.oldwht = 0; oCls.mstsite = 0;
                oCls.mstbrnc = 0; oCls.mstsubt = 0;  //oCls.mstrvsc = 0;
                oCls.mstcust = oCls.PartyID; // =oCls.mstprtc 
                oCls.msternv = oCls.acctname;

                oCls.mstcurrcd = 1;
                oCls.mstcurrrat = 1;
                oCls.mstintr = 0;

                oCls.mstbuyerc = 0; oCls.mstperd = 0; oCls.mstdsptoc = 0;
                //if (ItemDet.LstItem.Count > 0 && ItemDet.LstItem[0].GSTStateVal == "1")
                //    oCls.mstIorL = "I";
                //else
                //    oCls.mstIorL = "L";

                oCls.mstJobNo = oCls.mstJobNo;

                oCls.mstlotno = oCls.EMail;
                oCls.mstsection = oCls.NewMobile;
                oCls.mstuser = Convert.ToInt32(Session["UserID"]);

                dt2 = oSubmit.GetData("select isnull(max(right(mstchno,2))+1,1) as maxchno  from OrdeMst where compcode = " + SessionMaster.CompCode + " and msttype = " + oCls.msttype + " and left(mstchno, 6) = '" + sMstChNo + "'", true);
                oCls.mstchno = sMstChNo + GetVoucherNo(dt2.Rows[0]["maxchno"].ToString());

                oCls.mstuser = SessionMaster.UserID;
                oCls.compcode = SessionMaster.CompCode;

                oSubmit.BeginTran();
                //oSubmit.InsJourmst(oCls ,"OrdeMst");
                oSubmit.InsOrdeMst(oCls);
                //oSubmit.insertData("delete from Ordetrn  where compcode = " + oCls.compcode + " and trntype = " + oCls.msttype + " and  trncode = " + oCls.mstcode + " and  trnDate = '" + oCls.mstdate + "'");

                // oSubmit.insertData("delete b from " + sItmTbl + " a inner join GathDet b on a.itdgath = b.gathCode and a.Itdtype =b.Itdtype where a.Itdtype = " + oCls.msttype + " and Itdcode = " + oCls.mstcode);

                oSubmit.insertData("Delete from " + sItmTbl + " where CompCode = " + oCls.compcode + " and Itdtype = " + oCls.msttype + " and Itdcode = " + oCls.mstcode);

                for (int i = 0; i < ItemDet.LstItem.Count; i++)
                {

                    oSapu.compcode = Convert.ToInt16(oCls.compcode);
                    oSapu.itdtype = Convert.ToInt32(oCls.msttype);
                    oSapu.itdcode = Convert.ToInt32(oCls.mstcode);
                    oSapu.itdtime = Convert.ToInt32(oCls.msttime);
                    oSapu.itdsrno = Convert.ToInt16(i + 1);
                    oGath.gathpuri = oGath.gathstat = Convert.ToString(oSapu.itddate = Convert.ToDateTime(oCls.mstdate));

                    oSapu.itdmill = Convert.ToInt32(ItemDet.LstItem[i].itdmill);
                    oSapu.itdgodo = Convert.ToInt32(ItemDet.LstItem[i].itdgodo);
                    oSapu.itditem = Convert.ToInt32(ItemDet.LstItem[i].itditem);
                    oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].itdquan);
                    oSapu.itdrate = Convert.ToInt32(ItemDet.LstItem[i].itdrate);
                    oSapu.itdamou = Convert.ToInt32(ItemDet.LstItem[i].Amt);

                    if (ItemDet.LstItem[i].Sitdexpd != null && ItemDet.LstItem[i].Sitdexpd != "") oSapu.itdPODt = oSubmit.GetDateFormat(ItemDet.LstItem[i].Sitdexpd);
                    oSapu.itdrema = ItemDet.LstItem[i].itdrema.ToString();

                    oSubmit.insOrdeItd(oSapu, sItmTbl);

                }

                if (oCls.IsEdit != true)
                {
                    oCls.StDate = Convert.ToDateTime(oCom.getOpenDate(DateTime.Now.Date));
                    oCls.LastDate = Convert.ToDateTime(oCom.getClosDate(DateTime.Now.Date));
                    oSubmit.UpdCodeGen(oCls);
                }
                //oCls.mstcode_Print = oCls.mstcode;3005170990969
                oSubmit.Commit();

                TempData["message"] = "Saved Successfully ...";
                // oCls.mstcode = 0;
                dt2 = oSubmit.GetData("SELECT count(*)+1 Total from OrdeMst where compcode =" + SessionMaster.CompCode + " and msttype = " + oCls.msttype + " and Mstuser = " + Session["UserID"], true);
                oCls.mstprtc = Convert.ToInt32(dt2.Rows[0]["Total"].ToString());
                oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from Ordemst where compcode ='" + SessionMaster.CompCode + "' and msttype ='" + oCls.msttype + "'"));
                IsSave = 1;
            }
            catch (Exception ex)
            {
                IsSave = 0;
                oSubmit.RollBack();
                TempData["message"] = ex.Message;
                return RedirectToAction("ParkLead", new { MstType = oCls.msttype, MstCode = 0, comp = oCls.compcode });
            }
            //  return RedirectToAction("ParkLead", new { MstType = oCls.msttype, MstCode = oCls.mstcode, comp = oCls.compcode });

            dt2 = oSubmit.GetData("select isnull(max(right(mstchno,2))+1,1) as maxchno  from OrdeMst where compcode = " + SessionMaster.CompCode + " and msttype = " + oCls.msttype + " and left(mstchno, 6) = '" + sMstChNo + "'", true);
            oCls.mstchno = sMstChNo + GetVoucherNo(dt2.Rows[0]["maxchno"].ToString());

            var sData = new { mstprtc = oCls.mstprtc, IsSave = IsSave, mstcode = oCls.mstcode, mstchno = oCls.mstchno };
            return Json(Json(sData).Data, JsonRequestBehavior.AllowGet);

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


        public ActionResult GetParkLeadDet(int iMstcode, int iCompcode, int iType)
        {
            iCompcode = SessionMaster.CompCode;
            iType = 1147;
            clsSubmitModel oSubmit = new clsSubmitModel();

            List<clsPoItem> oList = new List<clsPoItem>();

            oList = oSubmit.GetInquiryItem(iType, iCompcode, iMstcode);

            //var sData = new { oList = oList };
            return Json(Json(oList).Data, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LeadList()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;
            int comp = Convert.ToInt32(SessionMaster.CompCode);
            if (comp == 0)
            {
                TempData["message"] = "Please Select Company ...";
            }
            dt = oSubmit.GetData("select a.MstCode , mstContactPerson ,mstDueDate ChallanDate ,mstsection NewMobile ,mstchnV ,msternv acctname ,ItemNm  from OrdeMst a  Where msttype = 1147 and a.CompCode = " + SessionMaster.CompCode + " and a.mstuser =" + Convert.ToInt32(Session["UserID"]), true);
            ViewBag.MstType = Request.QueryString["MstType"];

            Jourmaster oModel = new Jourmaster();
            List<Jourmaster> oList = new List<Jourmaster>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                oModel = new Models.Jourmaster();
                oModel.mstcode = Convert.ToInt32(dt.Rows[i]["MstCode"]);
                oModel.mstDueDate = Convert.ToDateTime(dt.Rows[i]["ChallanDate"]);
                oModel.mstsection = dt.Rows[i]["NewMobile"].ToString();
                oModel.mstchnV = Convert.ToInt32(dt.Rows[i]["mstchnV"]);
                oModel.msternv = dt.Rows[i]["acctname"].ToString();
                //if (dt.Rows[i]["itdQuan"].ToString() != "") oModel.Qty_Fini = Convert.ToInt32(dt.Rows[i]["itdQuan"]);
                //else oModel.Qty_Fini = 0;

                oModel.ItemNm_Fini = dt.Rows[i]["ItemNm"].ToString();

                oList.Add(oModel);
            }
            oModel.ListArea = oList;
            return View(oModel);
        }

        public ActionResult FillReference(int RefTypeID = 0)
        {
            //ERPDataContext db = new ERPDataContext();

            if (RefTypeID == 1)
            {
                var DegignationList = from a in DB.tblDistributors where a.DistributorID == 0 select new { studCode = a.MstCode, studName = a.DisName };
                return Json(DegignationList, JsonRequestBehavior.AllowGet);
            }
            else if (RefTypeID == 2)
            {
                var DegignationList = from a in DB.tblDistributors where a.DistributorID != 0 where a.ApprovID > 0 select new { studCode = a.MstCode, StudName = a.DisName };
                return Json(DegignationList, JsonRequestBehavior.AllowGet);
            }
            else if (RefTypeID == 3)
            {
                var DegignationList = from a in DB.loginusers where a.usetype == 12 select new { studCode = a.usecode, StudName = a.usename };
                return Json(DegignationList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var DegignationList = from a in DB.loginusers where a.usetype == 8 where a.usetype == 10 where a.usetype == 11 select new { studCode = a.usecode, StudName = a.usename };
                return Json(DegignationList, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);

        }
        public ActionResult ParkLead_Del(int MstCode)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;
            int comp = Convert.ToInt32(SessionMaster.CompCode);
            if (comp == 0)
            {
                TempData["message"] = "Please Select Company ...";
            }

            oSubmit.insertData("Delete from OrdeMst Where msttype = 1147 and CompCode = " + SessionMaster.CompCode + " and Mstcode = " + MstCode);
            oSubmit.insertData("delete from Ordeitd Where itdtype = 1147 and CompCode = " + SessionMaster.CompCode + " and itdcode = " + MstCode);

            dt = oSubmit.GetData("select a.MstCode , mstContactPerson ,mstDueDate ChallanDate ,mstsection NewMobile ,mstchnV ,msternv acctname ,itdQuan ,ItemName  from OrdeMst a Left join Ordeitd b on a.MstType =b.ItdType and a.compcode = b.Compcode and a.Mstcode = b.itdCode  Left join Itemain c on b.ItdItem = c.ItemCode and b.compcode = c.Compcode   Where msttype = 1147 and a.CompCode = " + SessionMaster.CompCode + " and a.mstuser =" + Convert.ToInt32(Session["UserID"]), true);
            ViewBag.MstType = Request.QueryString["MstType"];

            Jourmaster oModel = new Jourmaster();
            List<Jourmaster> oList = new List<Jourmaster>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                oModel = new Models.Jourmaster();
                oModel.mstcode = Convert.ToInt32(dt.Rows[i]["MstCode"]);
                oModel.mstDueDate = Convert.ToDateTime(dt.Rows[i]["ChallanDate"]);
                oModel.mstsection = dt.Rows[i]["NewMobile"].ToString();
                oModel.mstchnV = Convert.ToInt32(dt.Rows[i]["mstchnV"]);
                oModel.msternv = dt.Rows[i]["acctname"].ToString();
                if (dt.Rows[i]["itdQuan"].ToString() != "") oModel.Qty_Fini = Convert.ToInt32(dt.Rows[i]["itdQuan"]);
                else oModel.Qty_Fini = 0;

                oModel.ItemNm_Fini = dt.Rows[i]["ItemName"].ToString();

                oList.Add(oModel);
            }
            oModel.ListArea = oList;
            return View("LeadList", oModel);
        }

        public ActionResult GetLeadList(int iEmpCode, int ItemID = 0, string To = "", string From = "", int iIndustries = 0, int iMstType = 0)
        {
            //string sSQL = "";
            //if (iEmpCode > 0)
            //    sSQL = " and a.mstuser =" + iEmpCode;

            if (SessionMaster.UserType > 1)
                iEmpCode = SessionMaster.UserID;

            clsSubmitModel oSubmit = new clsSubmitModel();
            List<Jourmaster> lstJourmaster = oSubmit.GetLeadList(ItemID, iEmpCode, To, From, iIndustries, iMstType);

            return PartialView("_LeadList", lstJourmaster);
        }


        public ActionResult tblCallDet()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            //ERPDataContext db = new ERPDataContext();
            tblCallDet frm = new Models.tblCallDet();
            frm.ID = Convert.ToInt32(DB.tblCallDets.Max(a => (int?)a.ID) + 1);

            ViewBag.vwCity = new SelectList(oSubmit.GetDataList("select distinct CityNM NameNM, CityNM NameDesc ,0 NameID  from tblCallmst order by NameNM"), "NameDesc", "NameNM");

            ViewBag.vwProduct = new SelectList(oSubmit.GetProduct(), "NameID", "NameNM");
            //ViewBag.vwProduct = new SelectList(oSubmit.GetDataList("select distinct Product NameNM, Product NameDesc,0 NameID from tblCallmst order by NameNM"), "NameDesc", "NameNM");
            ViewBag.vwExeNM = new SelectList(oSubmit.GetDataList("select distinct ExeName NameNM, ExeName NameDesc,0 NameID from tblCallmst order by NameNM"), "NameDesc", "NameNM");

            ViewBag.vwMobile = new SelectList(oSubmit.GetInqMobile("", "", "", SessionMaster.UserID, SessionMaster.UserType), "NameID", "NameNM");
            ViewBag.vwStatus = new SelectList(from a in DB.studdets where a.studType == 871 orderby a.studName select new { a.studCode, a.studName }, "studCode", "studName");

            return PartialView(frm);
        }

        public ActionResult GetInqMobile(string City, string Product, string Executive)
        {
            try
            {
                clsSubmitModel oSubmit = new clsSubmitModel();
                return Json(Json(oSubmit.GetInqMobile(City, Product, Executive, SessionMaster.UserID, SessionMaster.UserType)).Data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(Json(ex.Message).Data, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult tblCallDet(tblCallDet frm )
        {
            string sMsg = "";
            try
            {
                //ERPDataContext db = new ERPDataContext();
                clsSubmitModel oSubmit = new clsSubmitModel();

                if ((from a in DB.tblCallDets where a.Mobile == frm.Mobile select a).Count() == 0)
                {
                    frm.ID = Convert.ToInt32(DB.tblCallDets.Max(a => (int?)a.ID) + 1);

                    frm.UserID = SessionMaster.UserID;
                    frm.CreatedOn = DateTime.Now;
 
                    DB.tblCallDets.InsertOnSubmit(frm);
                    DB.SubmitChanges();
                    sMsg = "Saved Successfully !";
                }
                else
                { sMsg = "Already Added !"; }
            }
            catch (Exception ex)
            {
                sMsg = ex.Message;
            }
            return Json(Json(sMsg).Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCallingData(string From_Date, string To_Date)
        {
            //ERPDataContext db = new ERPDataContext();
            var Data = from a in DB.tblCallDets where a.UserID == 99999999 select a;
            if (From_Date != "" && To_Date != "")
            {
                clsSubmitModel oSubmit = new clsSubmitModel();
                if (SessionMaster.UserType == 0)
                    Data = from a in DB.tblCallDets join b in DB.studdets on a.Status equals b.studCode where b.studType == 871 where a.CreatedOn.Value.Date >= oSubmit.GetDateFormat(From_Date) where a.CreatedOn.Value.Date <= oSubmit.GetDateFormat(To_Date) orderby a.ID descending select a;
                else
                    Data = from a in DB.tblCallDets where a.UserID == SessionMaster.UserID where a.CreatedOn.Value.Date >= oSubmit.GetDateFormat(From_Date) where a.CreatedOn.Value.Date <= oSubmit.GetDateFormat(To_Date) orderby a.ID descending select a;

                //Data = from a in DB.tblCallDets join b in DB.studdets on a.Status equals b.studCode where b.studType == 871 where a.UserID == SessionMaster.UserID where a.CreatedOn.Value.Date >= oSubmit.GetDateFormat(From_Date) where a.CreatedOn.Value.Date <= oSubmit.GetDateFormat(To_Date) orderby a.ID descending select a;

                //Data = from a in db.tblCallDets where a.UserID == SessionMaster.UserID orderby a.ID descending select a;
            }
            else
            {
                if (SessionMaster.UserType == 0)
                    Data = from a in DB.tblCallDets join b in DB.studdets on a.Status equals b.studCode where b.studType == 871 orderby a.ID descending select a;
                else
                    Data = from a in DB.tblCallDets where a.UserID == SessionMaster.UserID orderby a.ID descending select a;
            }
            return PartialView("GetCallingData", Data);
            //return Json(Json(Data).Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCalling()
        {
            ///ERPDataContext db = new ERPDataContext();

            if (SessionMaster.UserType == 0)
            {
                //var Data = from a in db.tblCallDets orderby a.ID descending select a;
                //return Json(Json(Data).Data, JsonRequestBehavior.AllowGet);
                var Data = from a in DB.tblCallDets
                           join Mst in DB.tblCallMsts on a.Mobile equals Mst.Mobile
                           join b in DB.ordemsts on a.Mobile equals b.mstJobNo
                               into c
                           from d in c.DefaultIfEmpty()
                           where a.isLead == true
                           where d.mstJobNo == null
                           orderby a.ID
                           select new { Mobile = a.Mobile, sName = Mst.InqName };
                return Json(Json(Data).Data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Data = from a in DB.tblCallDets
                           join Mst in DB.tblCallMsts on a.Mobile equals Mst.Mobile
                           join b in DB.ordemsts on a.Mobile equals b.mstJobNo
                               into c
                           from d in c.DefaultIfEmpty()
                           where a.isLead == true
                           where Mst.UseCode == SessionMaster.UserID
                           where d.mstJobNo == null
                           orderby a.ID
                           select new { Mobile = a.Mobile, sName = Mst.InqName };
                return Json(Json(Data).Data, JsonRequestBehavior.AllowGet);
            }

            return Json(Json("").Data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCallingDet(string sMobile)
        {
            //ERPDataContext db = new ERPDataContext();
            var Data = from a in DB.tblCallMsts
                       join b in DB.tblCallDets on a.Mobile equals b.Mobile
                       where a.Mobile == sMobile
                       select new { Mob = a.Mobile, Name = a.InqName, Remark = b.Remark, Per = b.Ratio, StateID = a.StateID, CityID = a.CityID, CityNM = a.CityNM, ItemID = b.ItemID };


            return Json(Json(Data).Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FollowUp()
        {
            //ERPDataContext db = new ERPDataContext();
            var FollowBy_S = from a in DB.loginusers where a.usetype == 8 || a.usetype == 10 || a.usetype == 11 orderby a.usename select new { a.usename, a.usecode };
            ViewBag.FollowBy_S = new SelectList(FollowBy_S, "usecode", "usename");

            //var F_LeadNo = from a in db.ordemsts where a.compcode == SessionMaster.CompCode && a.msttype == 1147 && a.mstuser == SessionMaster.UserID select new { itgpName = a.mstchno, itgpcode = a.mstcode }; //msternv 

            //            SELECT a.MstCode ,a.Mstchno ,[mstrefc] ,L_ID from [ordemst] a
            //left join tblFollow b on a.mstCode = L_ID and b.[Status_I] = 1
            //WHERE a.MstUser = 1 and a.msttype = 1147 AND b.L_ID IS  NULL 

            var F_LeadNo = from a in DB.ordemsts
                           join Mst in DB.tblFollows on
                           new { ID = a.mstcode, TP = 1 } equals
                           new { ID = (int)Mst.L_ID, TP = (int)Mst.Status_I } into c
                           from d in c.DefaultIfEmpty()
                           where a.msttype == 1147
                           where d.L_ID == null
                           where a.mstuser == SessionMaster.UserID
                           orderby a.mstchno
                           select new { itgpName = a.mstchno, itgpcode = a.mstcode };

            ViewBag.F_LeadNo = new SelectList(F_LeadNo, "itgpcode", "itgpName");

            ViewBag.UserType = SessionMaster.UserType;
            return View();
        }

        public ActionResult _FollowUp(string sType, string sCode = "0",string sRpType="")
        {
            ViewBag.Company = new SelectList(from res in DB.companies orderby res.compname select new { res.compcode, res.compname }, "compcode", "compname");
            if (sType == "Test" || sType == "PaymentPlanning")
            {
                //clsSubmitModel o = new clsSubmitModel();
                //ViewBag.vwDealer = new SelectList(o.GetDataList("Select HeadDes NameNM, max(id) NameID from tblLedger group by HeadDes order by HeadDes"), "NameID", "NameNM");
                ViewBag.UserCode = SessionMaster.UserID;
            }
            else if (sType == "OrdApprove" || sType == "OrdApprove_I" || sType == "PayFollowUp" || sType == "PayFollowUpList" || sType == "DispatchOrd" || sType == "Order" || sType == "TrialBal" || sType == "Ledger" || sType == "OrdFollowUp" || sType == "OrdFollowUpList" || sType == "BankReceipt" || sType == "UserWork" || sType == "ComplaintList")
            {
                if (SessionMaster.UserType == 4)
                {
                    //ViewBag.vwDealer = new SelectList(from a in DB.accounts join b in DB.citydets on new { _Type = 15, _Code = (int)a.acctcity } equals new { _Type = b.cityType, _Code = b.citycode } join dlr in DB.tblMapDealers on a.acctcode equals dlr.AcctID   where a.acctgrou == 21 where dlr.DealID == SessionMaster.UserID orderby a.acctname select new { NameID = a.acctcode, NameNM = a.acctname + " - " + b.cityname }, "NameID", "NameNM");

                    ViewBag.vwDealer = new SelectList(from dl in DB.tblDistributors join dlr in DB.tblMapDealers on dl.MstCode equals dlr.DealID join a in DB.accounts on dlr.AcctID equals a.acctcode where a.acctgrou == 21 where dl.UseCode == SessionMaster.UserID orderby a.acctname select new { NameID = a.acctcode, NameNM = a.acctname }, "NameID", "NameNM");

                }
else if (SessionMaster.UserType == 8)
                {
                   // var vwDealer = new SelectList(from a in DB.tblDistributors join acc in DB.accounts on a.MstCode equals acc.AcctDeal join b in DB.EmpAllotMsts on a.MstCode equals b.DealerID join c in DB.employees on b.EmpID equals c.empcode join d in DB.citydets on new { _Type = 15, _Code = (int)acc.acctcity } equals new { _Type = d.cityType, _Code = d.citycode } where a.DistributorID != 0 where c.UseCode == SessionMaster.UserID where acc.acctgrou == 21 orderby a.DisName select new { NameNM = a.DisName + " - " + d.cityname, NameID = acc.acctcode }, "NameID", "NameNM");

                    ViewBag.vwDealer = new SelectList(from a in DB.tblDistributors join acc in DB.accounts on a.MstCode equals acc.AcctDeal join b in DB.EmpAllotMsts on a.MstCode equals b.DealerID join c in DB.employees on b.EmpID equals c.empcode join d in DB.citydets on new { _Type = 15, _Code = (int)acc.acctcity } equals new { _Type = d.cityType, _Code = d.citycode } where a.DistributorID != 0 where c.UseCode == SessionMaster.UserID where acc.acctgrou == 21 orderby a.DisName select new { NameNM = a.DisName, NameID = acc.acctcode }, "NameID", "NameNM");
                }
                else
                {  
                    ViewBag.vwDealer = new SelectList(from a in DB.accounts join b in DB.citydets on new { _Type = 15, _Code = (int)a.acctcity } equals new { _Type = b.cityType, _Code = b.citycode } where a.acctgrou == 21 orderby a.acctname select new { NameID = a.acctcode, NameNM = a.acctname + " - " + b.cityname }, "NameID", "NameNM");
                }



                ViewBag.Brand = new SelectList(from a in DB.studdets where a.studType == 61 orderby a.studName select new { a.studName, a.studCode }, "studCode", "studName");
                ViewBag.SubCategory = new SelectList(from a in DB.itgroups where a.compcode == SessionMaster.CompCode where a.itgpunde != 0 orderby a.itgpname select new { a.itgpname, a.itgpcode }, "itgpcode", "itgpname");
                ViewBag.vwState = new SelectList((from a in DB.citydets where a.cityType == 67 && a.cityrute == 3 orderby a.cityname select new { a.citycode, a.cityname }), "citycode", "cityname");

                if (SessionMaster.UserType > 0)
                    ViewBag.Executive = new SelectList(from res in DB.employees orderby res.empname select new { res.UseCode, res.empname }, "UseCode", "empname", SessionMaster.UserID);
                else
                    ViewBag.Executive = new SelectList(from res in DB.employees orderby res.empname select new { res.UseCode, res.empname }, "UseCode", "empname");

                //var F_Dealer = from a in DB.tblDistributors where a.DistributorID == 0 orderby a.DealCode  select new { itgpName = a.DealCode, itgpcode = a.MstCode };
                //if (SessionMaster.UserType > 0)
                //    F_Dealer = from a in DB.tblDistributors join acc in DB.accounts on a.MstCode equals acc.AcctDeal join b in DB.EmpAllotMsts on a.MstCode equals b.DealerID join c in DB.employees on b.EmpID equals c.empcode join d in DB.citydets on new { _Type = 15, _Code = (int)acc.acctcity } equals new { _Type = d.cityType, _Code = d.citycode } where a.DistributorID != 0 where c.UseCode == SessionMaster.UserID where acc.acctgrou == 21 orderby a.DisName select new { itgpName = a.DisName + " - " + d.cityname, itgpcode = acc.acctcode };
                //else
                //    F_Dealer = from a in DB.tblDistributors join acc in DB.accounts on a.MstCode equals acc.AcctDeal join d in DB.citydets on new { _Type = 15, _Code = (int)acc.acctcity } equals new { _Type = d.cityType, _Code = d.citycode } where a.DistributorID != 0 where acc.acctgrou == 21 orderby a.DisName select new { itgpName = a.DisName + " - " + d.cityname, itgpcode = acc.acctcode };

                // ViewBag.F_Dealer = new SelectList(F_Dealer, "itgpcode", "itgpName");


            }
            ViewBag.Type = sType;
            ViewBag.Code = sCode;
            ViewBag.RpType = sRpType;//181030
            ViewBag.IsRead = 2;
            switch (sRpType)//181030
            {
                case "PC": ViewBag.aTitle = "Pending Complaint"; break;
                case "CR": ViewBag.aTitle = "Complaint Resolved"; break;
                case "RC": ViewBag.aTitle = "Reject Complaint"; break;
                case "HC": ViewBag.aTitle = "Halted Complaint"; break;
                case "CL": ViewBag.aTitle = "Complaint List"; break;
                case "NC": ViewBag.aTitle = "New Complains List"; ViewBag.IsRead = 1; break;//181030_2
                case "AC": ViewBag.aTitle = "Alloted Complains List"; break;//181030_3
                case "UC": ViewBag.aTitle = "UnAlloted Complains List"; break;//181030_3
                case "OC": ViewBag.aTitle = "Old Complains List"; break;//181030_3
                case "TC": ViewBag.aTitle = "Track Complains List"; break;//181030_3
            }

            //***************************************************
            //DataTable dtParty; 
            //List<clsFilter> lstFilter = new List<clsFilter>();
            //dtParty = oSubmit.GetData("Select  AcctName , acctCode from Account where Compcode =" + SessionMaster.CompCode + " and acctgrou in (21 ,22) order by AcctName ");

            clsFilter sm1 = new clsFilter();
            //clsFilter sm = new clsFilter(); 

            //for (int i = 0; i < dtParty.Rows.Count; i++)
            //{
            //    sm = new clsFilter();
            //    sm.PartyName = dtParty.Rows[i]["AcctName"].ToString();
            //    sm.PartyID = Convert.ToInt32(dtParty.Rows[i]["acctCode"].ToString());
            //    lstFilter.Add(sm);
            //}

            if (SessionMaster.UserType > 1)
            {
                var Dealer = (from a in DB.tblDistributors join acc in DB.accounts on a.MstCode equals acc.AcctDeal join b in DB.EmpAllotMsts on a.MstCode equals b.DealerID join c in DB.employees on b.EmpID equals c.empcode join d in DB.citydets on new { _Type = 15, _Code = (int)acc.acctcity } equals new { _Type = d.cityType, _Code = d.citycode } where a.DistributorID != 0 where (SessionMaster.UserType > 1 ? c.UseCode == SessionMaster.UserID : true) where acc.acctgrou == 21 orderby a.DisName select new clsFilter() { PartyName = a.DisName + " - " + d.cityname, PartyID = acc.acctcode }).Distinct().ToList();
                sm1.lstFilter = Dealer;// lstFilter;
            }
            else
            {
                var Dealer = (from acc in DB.accounts join d in DB.citydets on new { _Type = 15, _Code = (int)acc.acctcity } equals new { _Type = d.cityType, _Code = d.citycode } where acc.acctgrou == 21 orderby acc.acctname select new clsFilter() { PartyName = acc.acctname + " - " + d.cityname, PartyID = acc.acctcode }).Distinct().ToList();
                sm1.lstFilter = Dealer;// lstFilter;
            }
            //*************************************************** 
            sm1.lstBrand = oSubmit.GetBrandList();

            return View(sm1);

        }
        public ActionResult OutstandnDue(string sType, string sCode = "0")
        {
            ViewBag.Company = new SelectList(from res in DB.companies orderby res.compname select new { res.compcode, res.compname }, "compcode", "compname");
            if (sType == "Test" || sType == "PaymentPlanning")
            {
                ViewBag.UserCode = SessionMaster.UserID;
            }
            else if (sType == "OrdApprove" || sType == "OrdApprove_I" || sType == "PayFollowUp" || sType == "PayFollowUpList" || sType == "DispatchOrd" || sType == "Order" || sType == "TrialBal" || sType == "Ledger" || sType == "OrdFollowUp" || sType == "OrdFollowUpList" || sType == "BankReceipt" || sType == "UserWork" || sType == "ComplaintList")
            {
                if (SessionMaster.UserType == 4)
                {
                    ViewBag.vwDealer = new SelectList(from dl in DB.tblDistributors join dlr in DB.tblMapDealers on dl.MstCode equals dlr.DealID join a in DB.accounts on dlr.AcctID equals a.acctcode where a.acctgrou == 21 where dl.UseCode == SessionMaster.UserID orderby a.acctname select new { NameID = a.acctcode, NameNM = a.acctname }, "NameID", "NameNM");
                }
                else
                {
                    ViewBag.vwDealer = new SelectList(from a in DB.accounts join b in DB.citydets on new { _Type = 15, _Code = (int)a.acctcity } equals new { _Type = b.cityType, _Code = b.citycode } where a.acctgrou == 21 orderby a.acctname select new { NameID = a.acctcode, NameNM = a.acctname + " - " + b.cityname }, "NameID", "NameNM");
                }
                ViewBag.Brand = new SelectList(from a in DB.studdets where a.studType == 61 orderby a.studName select new { a.studName, a.studCode }, "studCode", "studName");
                ViewBag.SubCategory = new SelectList(from a in DB.itgroups where a.compcode == SessionMaster.CompCode where a.itgpunde != 0 orderby a.itgpname select new { a.itgpname, a.itgpcode }, "itgpcode", "itgpname");
                ViewBag.vwState = new SelectList((from a in DB.citydets where a.cityType == 67 && a.cityrute == 3 orderby a.cityname select new { a.citycode, a.cityname }), "citycode", "cityname");

                if (SessionMaster.UserType > 0)
                    ViewBag.Executive = new SelectList(from res in DB.employees orderby res.empname select new { res.UseCode, res.empname }, "UseCode", "empname", SessionMaster.UserID);
                else
                    ViewBag.Executive = new SelectList(from res in DB.employees orderby res.empname select new { res.UseCode, res.empname }, "UseCode", "empname");
            }
            ViewBag.Type = sType;
            ViewBag.Code = sCode;
            clsFilter sm1 = new clsFilter();
            if (SessionMaster.UserType > 1)
            {
                var Dealer = (from a in DB.tblDistributors join acc in DB.accounts on a.MstCode equals acc.AcctDeal join b in DB.EmpAllotMsts on a.MstCode equals b.DealerID join c in DB.employees on b.EmpID equals c.empcode join d in DB.citydets on new { _Type = 15, _Code = (int)acc.acctcity } equals new { _Type = d.cityType, _Code = d.citycode } where a.DistributorID != 0 where (SessionMaster.UserType > 1 ? c.UseCode == SessionMaster.UserID : true) where acc.acctgrou == 21 orderby a.DisName select new clsFilter() { PartyName = a.DisName + " - " + d.cityname, PartyID = acc.acctcode }).Distinct().ToList();
                sm1.lstFilter = Dealer;// lstFilter;
            }
            else
            {
                var Dealer = (from acc in DB.accounts join d in DB.citydets on new { _Type = 15, _Code = (int)acc.acctcity } equals new { _Type = d.cityType, _Code = d.citycode } where acc.acctgrou == 21 orderby acc.acctname select new clsFilter() { PartyName = acc.acctname + " - " + d.cityname, PartyID = acc.acctcode }).Distinct().ToList();
                sm1.lstFilter = Dealer;// lstFilter;
            }
            sm1.lstBrand = oSubmit.GetBrandList();
            return View(sm1);
        }

        public ActionResult FollowUpList(int iStatus = 0, int iEmpCode = 0, int iDays = 0, int iType = 0, int isDue = 0)
        {
            string sSQL = "";
            //if (iEmpCode > 0)
            //    sSQL = " and a.mstuser =" + iEmpCode;

            if (Convert.ToInt16(Session["UserType"]) > 1)
                sSQL = " and a.mstuser =" + Session["UserID"];

            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();

            // List<clsFollow> lstFollow = oSubmit.GetFollowUpList("select UseName ,L_No ,F_Date,case when Status_I = 1 then 'Closed' when Status_I = 2 then 'Fake' when Status_I = 3 then 'Under Process' when Status_I = 4 then 'Converted' end  Status,Remark from tblFollow a  inner join loginUsers b on a.F_by = b.UseCode");

            List<clsFollow> lstFollow = oSubmit.GetFollowUpList(iStatus, iEmpCode, iDays, iType, isDue);


            return PartialView("_FollowUpList", lstFollow);
        }


        public ActionResult InsFollow(int F_by, int L_ID, int Status_I, int Status_II, string L_No, string Remark, string MstDate, string F_Date, string hdnMstCode = "", int isLead = 0, int F_Time = 0, string F_Next_Date = "", string Feedback = "")
        {

            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();

            int MstCode = 0;
            DataTable dt;
            dt = oSubmit.GetData("select isnull(Max(MstCode),0) +1 MstCode from tblFollow", true);
            MstCode = Convert.ToInt32(dt.Rows[0]["MstCode"].ToString());

            if (hdnMstCode != "0")
                MstCode = Convert.ToInt32(hdnMstCode);

            if (Convert.ToInt16(Session["UserType"]) > 1) F_by = Convert.ToInt16(Session["UserID"]);


            //if (MstDate.ToString()!="")
            if (F_Next_Date.ToString() != "") Convert.ToDateTime(oSubmit.GetDateFormat(F_Next_Date));

            oSubmit.InsFollow(MstCode, F_by, L_ID, Status_I, Status_II, L_No, Remark, MstDate, F_Date, F_Next_Date, isLead, F_Time, Feedback, SessionMaster.UserID);

            dt = oSubmit.GetData("select isnull(Max(MstCode),0) +1 MstCode from tblFollow", true);
            MstCode = Convert.ToInt32(dt.Rows[0]["MstCode"].ToString());

            var ITEMS = new { MstCode = MstCode };

            return Json(ITEMS, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetLeadNoDet(string sLeadNO, int msttype = 1147)
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            var ITEMS = new { Company = "", Person = "", F_Count = "", LDate = "", LeadNo = "", InqName = "", Mobile = "", mstprtc = "", ItemName = "", IorL = "" };

            DataTable dt;
            dt = oSubmit.GetData("SELECT mstContactPerson ,msternv ,F_Tot , F_Date ,a.MstchNo ,a.mstJobno Mobile,a.msternv InqName ,ITM.ItemName ,a.mstprtc ,a.mstIorL from OrdeMst a  Inner Join OrdeItd OI on a.MstType = OI.ItdType and a.MstCode = OI.itdCode Inner Join Itemain ITM on OI.itdItem = ITM.ItemCode left join ( select count(*) F_Tot ,max(F_Next_Date)F_Date ,L_ID  from tblFollow group by L_ID )b on a.MstCode = b.L_ID where a.compcode =" + SessionMaster.CompCode + " and a.msttype = " + msttype + " and a.MstCode = '" + sLeadNO + "'", true);

            string dDate = "";
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["F_Date"].ToString() != "")
                    dDate = Convert.ToDateTime(dt.Rows[0]["F_Date"]).ToString("dd/MM/yyyy");

                ITEMS = new { Company = dt.Rows[0]["msternv"].ToString(), Person = dt.Rows[0]["mstContactPerson"].ToString(), F_Count = dt.Rows[0]["F_Tot"].ToString(), LDate = dDate, LeadNo = dt.Rows[0]["MstchNo"].ToString(), InqName = dt.Rows[0]["InqName"].ToString(), Mobile = dt.Rows[0]["Mobile"].ToString(), mstprtc = dt.Rows[0]["mstprtc"].ToString(), ItemName = dt.Rows[0]["ItemName"].ToString(), IorL = dt.Rows[0]["mstIorL"].ToString() };
            }

            return Json(ITEMS, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Quotation(int comp = 0, int id = 0, int mstCode = 0, int MstType = 0, int mstcode_Print = 0)
        {
            try
            {
                //ERPDataContext oDB = new ERPDataContext();
                comp = SessionMaster.CompCode;
                if (comp == 0)
                {
                    TempData["message"] = "Please Select Company ...";
                }
                Jourmaster frm = new Jourmaster();
                clsSubmitModel oSubmit = new clsSubmitModel();
                ViewBag.MstType = Request.QueryString["MstType"];
                ViewBag.UnitID = new SelectList(oSubmit.GetUnitLst(SessionMaster.CompCode), "unitcode", "unitname");

                string sSql = "";
                if (Convert.ToInt16(Session["UserType"]) > 1)
                    sSql = " and a.MstUser =" + Session["UserID"];

                //var F_LeadNo = oDB.Database.SqlQuery<itgroup>("SELECT a.msternv itgpName,a.MstCode itgpcode ,b.mstrefc from OrdeMst a Left Join Ordemst b on  b.mstType = 1163 and cast(a.MstCode as varchar(10)) =  b.mstrefc where a.compcode =62 and a.msttype = 1147 and b.mstrefc is null " + sSql + " order by a.msternv ");

                var LeadNo = from a in DB.ordemsts join b in DB.ordemsts on a.mstcode.ToString() equals b.mstrefc where a.compcode == SessionMaster.CompCode where a.msttype == 1147 where b.mstrefc == null select new { a.msternv, a.mstcode };

                //ViewBag.F_LeadNo = new SelectList(F_LeadNo, "itgpcode", "itgpName");
                ViewBag.F_LeadNo = new SelectList(LeadNo, "mstcode", "msternv");

                //var F_Dealer = oDB.Database.SqlQuery<itgroup>("select dealCode +' - '+ DisName  itgpName,MstCode itgpcode  from tblDistributor where DistributorID <> 0 ");
                var F_Dealer = from a in DB.tblDistributors where a.DistributorID != 0 select new { itgpName = a.DealCode + " - " + a.DisName, itgpcode = a.MstCode };
                ViewBag.F_Dealer = new SelectList(F_Dealer, "itgpcode", "itgpName");

                DataTable dt;
                dt = oSubmit.GetData("select studName ,studCode from studdet where studtype  = 55 order By StudName", true);
                frm.lstFTerm = oSubmit.getList(dt, "studName", "studCode");
                dt = oSubmit.GetData("select studName ,studCode from studdet where studtype  = 44 order By StudName", true);
                frm.lstOTerm = oSubmit.getList(dt, "studName", "studCode");

                //var vwCate = oDB.Database.SqlQuery<itgroup>("select itgpName ,itgpcode  from  itGroup where itgpunde = 0 and Compcode = " + SessionMaster.CompCode);
                var vwCate = from a in DB.itgroups where a.itgpunde == 0 where a.compcode == SessionMaster.CompCode select new { a.itgpname, a.itgpcode };
                ViewBag.vwCategory = new SelectList(vwCate, "itgpcode", "itgpName");

                //ViewBag.ItemNm = new SelectList(from res in oDB.itemains where res.itemgrou == 11 orderby res.itemname select new { res.itemcode, res.itemname }, "itemcode", "itemname");
                ViewBag.ItemNm = new SelectList(from res in DB.itemains orderby res.itemname select new { res.itemcode, res.itemname }, "itemcode", "itemname");

                if (mstCode > 0)
                {
                    #region  "Edit"
                    var model = oSubmit.GetInquiry(MstType, comp, mstCode);

                    model.PartyID = model.mstcust;
                    model.ChallanDate = Convert.ToDateTime(model.mstDueDate).ToString("dd/MM/yyyy");
                    model.sMstdate = Convert.ToDateTime(model.mstdate).ToString("dd/MM/yyyy");

                    ViewBag.mstsale = new SelectList(from res in DB.studdets where res.studType == 860 orderby res.studName select new { res.studCode, res.studName }, "studCode", "studName", model.mstsale);
                    ViewBag.SubCate = new SelectList(from res in DB.itgroups where res.compcode == SessionMaster.CompCode orderby res.itgpname select new { res.itgpcode, res.itgpname }, "itgpcode", "itgpname");
                    ViewBag.itemnumb = new SelectList(from res in DB.studdets where res.studType == 61 select new { res.studCode, res.studName }, "studcode", "studname");

                    dt = oSubmit.GetData("select studName ,studCode from studdet where studtype  = 55 order By StudName", true);
                    model.lstFTerm = oSubmit.getList(dt, "studName", "studCode");
                    dt = oSubmit.GetData("select studName ,studCode from studdet where studtype  = 44 order By StudName", true);
                    model.lstOTerm = oSubmit.getList(dt, "studName", "studCode");

                    return View("Quotation", model);
                    #endregion
                }
                else
                {
                    #region "Create"

                    frm.msttimes = DateTime.Now.ToString("HH:mm:ss");
                    DataTable dt2;

                    dt2 = oSubmit.GetData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from OrdeMst where compcode ='" + comp + "' and msttype ='" + MstType + "'", true);
                    ViewBag.MstCode = frm.mstcode = Convert.ToInt32(dt2.Rows[0]["mstcode"]);
                    frm.compcode = Convert.ToInt32(SessionMaster.CompCode);
                    frm.msttype = frm.type = MstType;
                    frm.mstdate = DateTime.Now.Date;
                    frm.mstcode_Print = mstcode_Print;
                    ViewBag.mstsale = new SelectList(from res in DB.studdets where res.studType == 860 select new { res.studCode, res.studName }, "studCode", "studName");
                    ViewBag.SubCate = new SelectList(from res in DB.itgroups where res.compcode == SessionMaster.CompCode orderby res.itgpname select new { res.itgpcode, res.itgpname }, "itgpcode", "itgpname");
                    ViewBag.itemnumb = new SelectList(from res in DB.studdets where res.studType == 61 select new { res.studCode, res.studName }, "studcode", "studname");

                    ViewBag.MstBlno = new SelectList(from res in DB.GetInqNo(5176) select new { res.mstCode, res.MstChNo }, "mstCode", "MstChNo");

                    var BlNo = new SelectList(from res in DB.GetInqNo(5176) select new { res.mstCode, res.MstChNo }, "mstCode", "MstChNo");

                    dt2 = oSubmit.GetData("SELECT count(*)+1 Total from OrdeMst where compcode =" + SessionMaster.CompCode + " and msttype = 1163 and Mstuser = " + Session["UserID"], true);
                    frm.mstprtc = Convert.ToInt32(dt2.Rows[0]["Total"].ToString());

                    string sMstChNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");

                    dt2 = oSubmit.GetData("select isnull(max(right(mstchno,2))+1,1) as maxchno  from OrdeMst where compcode = " + comp + " and msttype = " + MstType + " and left(mstchno, 6) = '" + sMstChNo + "'", true);
                    ViewBag.mstchno = frm.mstchno = sMstChNo + GetVoucherNo(dt2.Rows[0]["maxchno"].ToString());

                    frm.IsEdit = false;

                    #endregion
                }
                return View("Quotation", frm);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Quotation(Jourmaster oCls)
        {
            DataTable dt2;
            clsSubmitModel oSubmit = new clsSubmitModel();
            commFunction oCom = new commFunction();
            int IsSave = 0;
            string sMstChNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");

            try
            {
                jourtrn oTrn = new jourtrn();
                sapuitd oSapu = new sapuitd();
                gathdet oGath = new gathdet();

                string sGathCode = "";
                string sItmTbl = "ordeItd";

                //dt2 = oSubmit.GetData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from jourmst where compcode ='" + comp + "' and msttype ='" + MstType + "'");

                if (Request.QueryString["MstCode"] != null && Convert.ToInt32(Request.QueryString["MstCode"]) > 0) { }
                else
                {
                    oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from Ordemst where compcode ='" + SessionMaster.CompCode + "' and msttype ='" + oCls.msttype + "'"));
                }

                var json = oCls.sItemDet;
                clsPoItem ItemDet = JsonConvert.DeserializeObject<clsPoItem>(json);

                ViewBag.MstType = oCls.msttype;
                int iDays = 0;
                decimal iComm = 0, iInterest = 0;

                oCls.mstdate = oSubmit.GetDateFormat(oCls.sMstdate);

                if (oCls.mstdued > 0) iDays = (int)oCls.mstdued;//DueDay 170831
                if (oCls.Comm > 0) iComm = oCls.Comm;
                if (oCls.Interest > 0) iInterest = oCls.Interest;

                if (oCls.mstchnH == null) oCls.mstchnH = "";
                oCls.mststat = 0;
                //oCls.msttime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
                //oCls.mstrefc = getParty(Convert.ToInt32(oCls.PartyID)) + "~" + iInterest + "~" + iDays + "~" + iComm;
                if (oCls.msttype == 42)
                {
                    oCls.mstchno = oCls.mstchnH + oCls.mstchnV;//170830
                    oCls.mstexti = "~~#0";//170830
                }
                else
                {
                    oCls.mstexti = "~";//170830
                    oCls.mstchnV = 0;
                }
                oCls.mstgncd = "0~0~" + oCls.acctledg + "~0";//170830



                oCls.mstAppr = 0; oCls.mstqtyd = 0; oCls.mstvat1 = 0; oCls.mstvat2 = 0;
                oCls.mstvat3 = 0; oCls.mstchnm = ""; oCls.oldwht = 0; oCls.mstsite = 0;
                oCls.mstbrnc = 0; oCls.mstsubt = 0; oCls.mstcust = 0; oCls.mstrvsc = 0;
                oCls.mstcust = oCls.PartyID; //mstprtc
                //oCls.msternv = "";

                oCls.mstcurrcd = 1;
                oCls.mstcurrrat = 1;
                oCls.mstintr = 0;

                //oCls.mstDueDate = oSubmit.GetDateFormat(oCls.ChallanDate);  //oCls.mstdate.AddDays(iDays);//170831
                oCls.mstdate = oSubmit.GetDateFormat(oCls.sMstdate);

                oCls.mstbuyerc = 0; oCls.mstperd = 0; oCls.mstdsptoc = 0;
                if (ItemDet.LstItem.Count > 0 && ItemDet.LstItem[0].GSTStateVal == "1")
                    oCls.mstIorL = "I";
                else
                    oCls.mstIorL = "L";

                dt2 = oSubmit.GetData("select isnull(max(right(mstchno,2))+1,1) as maxchno  from OrdeMst where compcode = " + SessionMaster.CompCode + " and msttype = " + oCls.msttype + " and left(mstchno, 6) = '" + sMstChNo + "'", true);
                oCls.mstchno = sMstChNo + GetVoucherNo(dt2.Rows[0]["maxchno"].ToString());

                oCls.mstuser = SessionMaster.UserID;

                oSubmit.BeginTran();
                //oSubmit.InsJourmst(oCls ,"OrdeMst");
                oSubmit.InsOrdeMst(oCls);
                oSubmit.insertData("delete from Ordetrn  where compcode = " + oCls.compcode + " and trntype = " + oCls.msttype + " and  trncode = " + oCls.mstcode + " and  trnDate = '" + oCls.mstdate + "'");

                oSubmit.insertData("delete b from " + sItmTbl + " a inner join GathDet b on a.itdgath = b.gathCode and a.Itdtype =b.Itdtype where a.Itdtype = " + oCls.msttype + " and Itdcode = " + oCls.mstcode);

                oSubmit.insertData("Delete from " + sItmTbl + " where CompCode = " + oCls.compcode + " and Itdtype = " + oCls.msttype + " and Itdcode = " + oCls.mstcode);

                for (int i = 0; i < ItemDet.LstItem.Count; i++)
                {

                    //var q = from a in db.GetTable<charcodege>() select a; 
                    //var w = from res in db.charcodeges select new { res.gatcode };

                    sGathCode = oSubmit.GetUsWoCode();

                    oSapu.compcode = Convert.ToInt16(oCls.compcode);
                    oSapu.itdtype = Convert.ToInt32(oCls.msttype);
                    oSapu.itdcode = Convert.ToInt32(oCls.mstcode);
                    oSapu.itdtime = Convert.ToInt32(oCls.msttime);
                    oSapu.itdsrno = Convert.ToInt16(i + 1);
                    oGath.gathpuri = oGath.gathstat = Convert.ToString(oSapu.itddate = Convert.ToDateTime(oCls.mstdate));
                    //oSapu.itdtime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
                    oSapu.itditem = Convert.ToInt32(ItemDet.LstItem[i].itditem);
                    //oGath.gathdesc = oSapu.itdrema = ItemDet.LstItem[i].Remark.ToString();// ItemRemark.ToString();
                    //oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].Qty);
                    if (ItemDet.LstItem[i].UnitID.ToString() != "") oSapu.itdunit = Convert.ToInt32(ItemDet.LstItem[i].UnitID); else oSapu.itdunit = 0;
                    oSapu.itddime = ItemDet.LstItem[i].Rate.ToString();
                    if (ItemDet.LstItem[i].Rate.ToString() != "") oSapu.itdactprc = Convert.ToDecimal(ItemDet.LstItem[i].itdrate.ToString());//170830
                    oSapu.itdvate = oSapu.itdrate = Convert.ToDecimal(ItemDet.LstItem[i].itdrate);
                    oGath.gathwtdi = Convert.ToDecimal(oSapu.itdvate);
                    oSapu.itdamou = Convert.ToDecimal(ItemDet.LstItem[i].Amt);
                    oSapu.itdtowt = 0;//Convert.ToInt32(ItemDet.LstItem[i].Qty);//170830
                    if (oCls.msttype == 42) oSapu.itdpenq = oSapu.itdquan = -Convert.ToInt32(ItemDet.LstItem[i].itdquan);
                    else oSapu.itdpenq = oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].itdquan);
                    //oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].Qty);//170830
                    oSapu.itdpkin = 0;
                    oSapu.itdmill = 1;
                    oSapu.itdgodo = 1;

                    oSapu.itdlswt = 0;//170831

                    if (ItemDet.LstItem[i].DisPer.ToString() != "") oSapu.itdperd = Convert.ToDecimal(ItemDet.LstItem[i].DisPer.ToString()); else oSapu.itdperd = 0;
                    if (ItemDet.LstItem[i].DisAmt.ToString() != "") oSapu.itdqtyd = Convert.ToDecimal(ItemDet.LstItem[i].DisAmt.ToString()); else oSapu.itdqtyd = 0;


                    oSapu.itdlabonw = 0;//170831
                    oSapu.itdvatinc = 0;//170831
                    oSapu.itdcasert = 0;//170831
                    oSapu.itddscp = 0;//170831

                    oGath.gathcode = oSapu.itdgath = sGathCode;
                    oSapu.itdempo = SessionMaster.UserID;//0;170831
                    oSapu.itdlabamt = 0;
                    oGath.gathwast = ItemDet.LstItem[i].TaxPer.ToString();

                    //{170830
                    oSapu.itdigstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer);
                    oSapu.itdigstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt);
                    //if (ItemDet.LstItem[i].GSTStateVal == "2")
                    //{
                    //    if (Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal) > 0) oSapu.itdcgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal);
                    //    else oSapu.itdcgstrt = 0;

                    //    oSapu.itdcgstvl = Convert.ToDecimal(ItemDet.LstItem[i].Amt) * oSapu.itdcgstrt / 100;
                    //    if (Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal) > 0) oSapu.itdsgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal);
                    //    else oSapu.itdsgstrt = 0;

                    //    oSapu.itdsgstvl = Convert.ToDecimal(ItemDet.LstItem[i].Amt) * oSapu.itdsgstrt / 100;
                    //    oSapu.itdigstrt = 0;
                    //    oSapu.itdigstvl = 0;
                    //}
                    //else
                    //{
                    //    oSapu.itdcgstrt = 0;
                    //    oSapu.itdcgstvl = 0;
                    //    oSapu.itdsgstrt = 0;
                    //    oSapu.itdsgstvl = 0;
                    //    if (Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal) > 0) oSapu.itdigstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal); else oSapu.itdigstrt = 0;

                    //    oSapu.itdigstvl = Convert.ToDecimal(ItemDet.LstItem[i].Amt) * oSapu.itdigstrt / 100;

                    //}
                    oSapu.itdcessrt = Convert.ToDecimal(0);
                    oSapu.itdcessvl = Convert.ToDecimal(0);
                    oSapu.itdugstrt = Convert.ToDecimal(0);
                    oSapu.itdugstvl = Convert.ToDecimal(0);
                    //}170830
                    oSubmit.insSapuItd(oSapu, sItmTbl);
                    oSubmit.insGathDet(oGath);

                }

                if (oCls.IsEdit != true)
                {
                    oCls.StDate = Convert.ToDateTime(oCom.getOpenDate(DateTime.Now.Date));
                    oCls.LastDate = Convert.ToDateTime(oCom.getClosDate(DateTime.Now.Date));
                    oSubmit.UpdCodeGen(oCls);
                }
                oCls.mstcode_Print = oCls.mstcode;
                oSubmit.Commit();

                TempData["message"] = "Saved Successfully ...";
                dt2 = oSubmit.GetData("SELECT count(*)+1 Total from OrdeMst where compcode =" + SessionMaster.CompCode + " and msttype = " + oCls.msttype + " and Mstuser = " + Session["UserID"], true);
                oCls.mstprtc = Convert.ToInt32(dt2.Rows[0]["Total"].ToString());
                oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from Ordemst where compcode ='" + SessionMaster.CompCode + "' and msttype ='" + oCls.msttype + "'"));
                IsSave = 1;

            }
            catch (Exception ex)
            {
                IsSave = 0;
                oSubmit.RollBack();
                TempData["message"] = ex.Message;

                return RedirectToAction("Quotation", new { MstType = oCls.msttype, MstCode = 0, comp = oCls.compcode });
                var Data = new { mstprtc = oCls.mstprtc, IsSave = IsSave, mstcode = oCls.mstcode, mstchno = oCls.mstchno };
                return Json(Json(Data).Data, JsonRequestBehavior.AllowGet);

            }
            //return RedirectToAction("Quotation", new { MstType = oCls.msttype, MstCode = oCls.mstcode, comp = oCls.compcode, mstcode_Print = oCls.mstcode_Print });

            dt2 = oSubmit.GetData("select isnull(max(right(mstchno,2))+1,1) as maxchno  from OrdeMst where compcode = " + SessionMaster.CompCode + " and msttype = " + oCls.msttype + " and left(mstchno, 6) = '" + sMstChNo + "'", true);
            oCls.mstchno = sMstChNo + GetVoucherNo(dt2.Rows[0]["maxchno"].ToString());

            var sData = new { mstprtc = oCls.mstprtc, IsSave = IsSave, mstcode = oCls.mstcode, mstchno = oCls.mstchno };
            return Json(Json(sData).Data, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetQuotList(int iEmpCode, int ItemID = 0, string To = "", string From = "", int iIndustries = 0, int iMstType = 0)
        {
            string sSQL = "";
            if (iEmpCode > 0)
                sSQL = " and a.mstuser =" + iEmpCode;

            if (Convert.ToInt16(Session["UserType"]) > 1)
                sSQL = " and a.mstuser =" + Session["UserID"];

            clsSubmitModel oSubmit = new clsSubmitModel();
            List<Jourmaster> lstJourmaster = oSubmit.GetLeadList(ItemID, iEmpCode, To, From, iIndustries, iMstType);

            return PartialView("_QuotList", lstJourmaster);
        }

        [HttpGet]
        public ActionResult Order(int comp = 0, int id = 0, int mstCode = 0, int MstType = 0, int mstcode_Print = 0)
        {
            try
            {
                //comp = SessionMaster.CompCode;

                Jourmaster frm = new Jourmaster();

                string sSql = "";
                if (Convert.ToInt16(Session["UserType"]) > 1)
                    sSql = " and a.MstUser =" + Session["UserID"];

                var LeadNo = from a in DB.ordemsts join b in DB.ordemsts on a.mstcode.ToString() equals b.mstrefc where a.compcode == SessionMaster.CompCode where a.msttype == 1147 where b.mstrefc == null select new { a.msternv, a.mstcode };

                ViewBag.F_LeadNo = new SelectList(LeadNo, "mstcode", "msternv");

                var F_Dealer = from a in DB.tblDistributors where a.DistributorID == 0 select new { itgpName = a.DealCode, itgpcode = a.MstCode };

                if (SessionMaster.UserType > 0)
                    F_Dealer = from a in DB.tblDistributors join acc in DB.accounts on a.MstCode equals acc.AcctDeal join b in DB.EmpAllotMsts on a.MstCode equals b.DealerID join c in DB.employees on b.EmpID equals c.empcode join d in DB.citydets on new { _Type = 15, _Code = (int)acc.acctcity } equals new { _Type = d.cityType, _Code = d.citycode } where a.DistributorID != 0 where c.UseCode == SessionMaster.UserID where acc.acctgrou == 21 orderby a.DisName select new { itgpName = a.DisName + " - " + d.cityname, itgpcode = acc.acctcode };
                else
                    F_Dealer = from a in DB.tblDistributors join acc in DB.accounts on a.MstCode equals acc.AcctDeal join c in DB.citydets on new { _Type = 15, _Code = (int)acc.acctcity } equals new { _Type = c.cityType, _Code = c.citycode } where a.DistributorID != 0 where acc.acctgrou == 21 orderby a.DisName select new { itgpName = a.DisName + " - " + c.cityname, itgpcode = acc.acctcode };

                ViewBag.F_Dealer = new SelectList(F_Dealer, "itgpcode", "itgpName");

                DataTable dtFterm; DataTable dtOterm;
                dtFterm = oSubmit.GetData("select studName ,studCode from studdet where studtype  = 55 order By StudName", true);
                frm.lstFTerm = oSubmit.getList(dtFterm, "studName", "studCode");
                dtOterm = oSubmit.GetData("select studName ,studCode from studdet where studtype  = 44 order By StudName", true);
                frm.lstOTerm = oSubmit.getList(dtOterm, "studName", "studCode");

                ViewBag.Executive = new SelectList(from res in DB.employees orderby res.empname select new { res.UseCode, res.empname }, "UseCode", "empname");

                ViewBag.ItemNm = new SelectList(from res in DB.itemains orderby res.itemname select new { res.itemcode, res.itemname }, "itemcode", "itemname");

                ViewBag.Company = new SelectList(from res in DB.companies orderby res.compname select new { res.compcode, res.compname }, "compcode", "compname");

                ViewBag.Brand = new SelectList(from a in DB.studdets where a.studType == 61 select new { a.studName, a.studCode }, "studCode", "studName");
                if (SessionMaster.UserType > 0)
                    ViewBag.SubCategory = new SelectList(from a in DB.itgroups where a.itgpunde != 0 where a.compcode == SessionMaster.CompCode orderby a.itgpname select new { a.itgpname, a.itgpcode }, "itgpcode", "itgpname"); //where a.itgpunde == 61
                else
                    ViewBag.SubCategory = new SelectList(from a in DB.itgroups where a.itgpunde != 0 where a.compcode == SessionMaster.CompCode orderby a.itgpname select new { a.itgpname, a.itgpcode }, "itgpcode", "itgpname");

                ViewBag.Exp1 = new SelectList(from res in DB.accounts where (res.acctgrou == ConstVariable.TypeCode_IndirectExpenses || res.acctgrou == ConstVariable.TypeCode_Taxes || res.acctgrou == ConstVariable.TypeCode_DirectExpenses) && (res.acctgsta == null || res.acctgsta == 0) && res.compcode == comp orderby res.acctname select new { res.acctcode, res.acctname }, "acctcode", "acctname");

                if (mstCode > 0)
                {
                    #region  "Edit"
                    var model = oSubmit.GetInquiry(MstType, comp, mstCode);

                    model.PartyID = model.mstcust;
                    model.compcode = comp;
                    model.ChallanDate = Convert.ToDateTime(model.mstDueDate).ToString("dd/MM/yyyy");
                    model.sMstdate = Convert.ToDateTime(model.mstdate).ToString("dd/MM/yyyy");

                    model.lstFTerm = oSubmit.getList(dtFterm, "studName", "studCode");
                    model.lstOTerm = oSubmit.getList(dtOterm, "studName", "studCode");

                    ViewBag.MstCode = mstCode;
                    // model.mstexec =  
                    model.IsEdit = true;
                    return View("Order", model);
                    #endregion
                }
                else
                {
                    #region "Create"

                    DataTable dt2;

                    dt2 = oSubmit.GetData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from OrdeMst where compcode ='" + comp + "' and msttype =174", true);
                    ViewBag.MstCode = frm.mstcode = Convert.ToInt32(dt2.Rows[0]["mstcode"]);
                    frm.compcode = Convert.ToInt32(SessionMaster.CompCode);
                    frm.msttype = frm.type = MstType;
                    frm.mstdate = DateTime.Now.Date;
                    frm.mstcode_Print = mstcode_Print;

                    dt2 = oSubmit.GetData("SELECT count(*)+1 Total from OrdeMst where compcode =" + SessionMaster.CompCode + " and msttype = 174 and Mstuser = " + Session["UserID"], true);
                    frm.mstprtc = Convert.ToInt32(dt2.Rows[0]["Total"].ToString());

                    string sMstChNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");

                    dt2 = oSubmit.GetData("select isnull(max(right(mstchno,3))+1,1) as maxchno  from OrdeMst where msttype = 174 and left(mstchno, 6) = '" + sMstChNo + "'", true);
                    ViewBag.mstchno = frm.mstchno = sMstChNo + GetVoucherNo(dt2.Rows[0]["maxchno"].ToString());

                    frm.mstexec = SessionMaster.UserID;
                    frm.IsEdit = false;

                    #endregion
                }
                return View("Order", frm);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Order(Jourmaster oCls)
        {
            DataTable dt2;
            clsSubmitModel oSubmit = new clsSubmitModel();
            commFunction oCom = new commFunction();
            int IsSave = 0;
            string sMstChNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");

            try
            {
                jourtrn oTrn = new jourtrn();
                sapuitd oSapu = new sapuitd();
                gathdet oGath = new gathdet();

                string sGathCode = "";
                string sItmTbl = "ordeItd";
                int RefCode = Convert.ToInt32(oCls.mstrefc);
                int iEmpoID = Convert.ToInt32(oSubmit.GetSingleData("Select isNull(AcctCode ,0 ) ,* from Employee Where UseCode = " + oCls.mstexec, "0", true));
                //dt2 = oSubmit.GetData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from jourmst where compcode ='" + comp + "' and msttype ='" + MstType + "'");

                if (oCls.mstprtc > 0) { }
                else
                {
                    var DataD = new { mstprtc = oCls.mstprtc, IsSave = 0, mstcode = oCls.mstcode, mstchno = oCls.mstchno, msg = "Please Select Dealer" };
                    return Json(Json(DataD).Data, JsonRequestBehavior.AllowGet);
                }

                if (oCls.IsEdit != true)
                {
                    oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from Ordemst where compcode ='" + oCls.compcode + "' and msttype ='" + oCls.msttype + "'"));

                    //dt2 = oSubmit.GetData("select isnull(max(right(mstchno,2))+1,1) as maxchno  from OrdeMst where compcode = " + oCls.compcode + " and msttype = " + oCls.msttype + " and left(mstchno, 6) = '" + sMstChNo + "'", true);
                    dt2 = oSubmit.GetData("select isnull(max(right(mstchno,3))+1,1) as maxchno  from OrdeMst where msttype = " + oCls.msttype + " and  left(mstchno, 6) = '" + sMstChNo + "'", true);
                    oCls.mstchno = sMstChNo + GetVoucherNo(dt2.Rows[0]["maxchno"].ToString());
                }

                DataTable dtState;
                dtState = oSubmit.GetData("select isNull(acctcform,0)acctcform from Account Where acctCode = " + oCls.PartyID);
                if (dtState.Rows.Count > 0)
                {
                    if (dtState.Rows[0]["acctcform"].ToString() != "") oCls.GSTStateVal = dtState.Rows[0]["acctcform"].ToString(); else oCls.GSTStateVal = "0";
                }

                var json = oCls.sItemDet;
                clsPoItem ItemDet = JsonConvert.DeserializeObject<clsPoItem>(json);

                ViewBag.MstType = oCls.msttype;
                int iDays = 0;
                decimal iComm = 0, iInterest = 0;

                oCls.mstdate = oSubmit.GetDateFormat(oCls.sMstdate);

                if (oCls.mstdued > 0) iDays = (int)oCls.mstdued;//DueDay 170831
                if (oCls.Comm > 0) iComm = oCls.Comm;
                if (oCls.Interest > 0) iInterest = oCls.Interest;

                if (oCls.mstchnH == null) oCls.mstchnH = "";
                oCls.mststat = 0;
                //oCls.msttime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
                oCls.mstrefc = getParty(Convert.ToInt32(oCls.PartyID)) + "~" + iInterest + "~" + iDays + "~" + iComm;
                if (oCls.msttype == 42)
                {
                    oCls.mstchno = oCls.mstchnH + oCls.mstchnV;//170830
                    oCls.mstexti = "~~#0";//170830
                }
                else
                {
                    oCls.mstexti = "~";//170830
                    oCls.mstchnV = 0;
                }
                oCls.mstgncd = "0~0~" + oCls.acctledg + "~0";//170830



                oCls.mstAppr = 0; oCls.mstqtyd = 0; oCls.mstvat1 = 0; oCls.mstvat2 = 0;
                oCls.mstvat3 = 0; oCls.mstchnm = ""; oCls.oldwht = 0; oCls.mstsite = 0;
                oCls.mstbrnc = 0; oCls.mstsubt = 0; oCls.mstcust = 0; oCls.mstrvsc = 0;
                oCls.mstcust = oCls.PartyID; //mstprtc
                //oCls.msternv = "";

                oCls.mstcurrcd = 1;
                oCls.mstcurrrat = 1;
                oCls.mstintr = 0;

                //oCls.mstDueDate = oSubmit.GetDateFormat(oCls.ChallanDate);  //oCls.mstdate.AddDays(iDays);//170831
                oCls.mstdate = oSubmit.GetDateFormat(oCls.sMstdate);

                oCls.mstbuyerc = 0; oCls.mstperd = 0; oCls.mstdsptoc = 0;
                if (oCls.GSTStateVal == "1")
                    oCls.mstIorL = "I";
                else
                    oCls.mstIorL = "L";


                // oCls.mstsale = 115;
                oCls.mstuser = SessionMaster.UserID;
                //oCls.mstexec = SessionMaster.UserID;

                oSubmit.BeginTran();
                //oSubmit.InsJourmst(oCls ,"OrdeMst");
                oSubmit.InsOrdeMst(oCls);
                oSubmit.insertData("delete from Ordetrn  where compcode = " + oCls.compcode + " and trntype = " + oCls.msttype + " and  trncode = " + oCls.mstcode + " and  trnDate = '" + oCls.mstdate + "'");

                oSubmit.insertData("insert into bak_ordeitd select *, getdate()," + SessionMaster.UserID + ",'" + this.Request.UserHostAddress + "','" + Environment.MachineName + "' from ordeitd where compcode =" + oCls.compcode + " and itdtype = " + oCls.msttype + " and itdcode = " + oCls.mstcode);

                oSubmit.insertData("delete b from " + sItmTbl + " a inner join GathDet b on a.itdgath = b.gathCode and a.Itdtype =b.Itdtype where a.Itdtype = " + oCls.msttype + " and Itdcode = " + oCls.mstcode);

                oSubmit.insertData("Delete from " + sItmTbl + " where CompCode = " + oCls.compcode + " and Itdtype = " + oCls.msttype + " and Itdcode = " + oCls.mstcode);

                for (int i = 0; i < ItemDet.LstItem.Count; i++)
                {

                    //var q = from a in db.GetTable<charcodege>() select a; 
                    //var w = from res in db.charcodeges select new { res.gatcode };
                    oSapu = new sapuitd();
                    oGath = new gathdet();
                    sGathCode = oSubmit.GetUsWoCode();

                    oSapu.compcode = Convert.ToInt16(oCls.compcode);
                    oSapu.itdtype = Convert.ToInt32(oCls.msttype);
                    oSapu.itdcode = Convert.ToInt32(oCls.mstcode);
                    oSapu.itdtime = Convert.ToInt32(oCls.msttime);
                    oSapu.itdsrno = Convert.ToInt16(i + 1);
                    oGath.gathpuri = oGath.gathstat = Convert.ToString(oSapu.itddate = Convert.ToDateTime(oCls.mstdate));
                    //oSapu.itdtime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
                    oSapu.itditem = Convert.ToInt32(ItemDet.LstItem[i].itditem);
                    //oGath.gathdesc = oSapu.itdrema = ItemDet.LstItem[i].Remark.ToString();// ItemRemark.ToString();
                    //oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].Qty);
                    if (ItemDet.LstItem[i].UnitID.ToString() != "") oSapu.itdunit = Convert.ToInt32(ItemDet.LstItem[i].UnitID); else oSapu.itdunit = 0;
                    //oSapu.itddime = ItemDet.LstItem[i].Rate.ToString();
                    if (ItemDet.LstItem[i].itdactprc.ToString() != "") oSapu.itdactprc = Convert.ToDecimal(ItemDet.LstItem[i].itdactprc.ToString());
                    else oSapu.itdactprc = 0;

                    oSapu.itdvate = Convert.ToDecimal(ItemDet.LstItem[i].itdvate);
                    oSapu.itdrate = Convert.ToDecimal(ItemDet.LstItem[i].itdrate);

                    //                    select GATHLABP ,GATHDSCV ,* from gathdet where gathcode = 'AMRIC'

                    //WTDI = ACTUAL RATE - TAX

                    oGath.gathwtdi = Convert.ToDecimal(oSapu.itdvate); //oSapu.itdvate itdrate

                    //oSapu.itdamou = Convert.ToDecimal(ItemDet.LstItem[i].Amt);
                    //if (ItemDet.LstItem[i].ItemNetAmt.ToString() != "") oSapu.itdlabamt = Convert.ToDecimal(ItemDet.LstItem[i].ItemNetAmt.ToString()); else oSapu.itdlabamt = 0;

                    if (ItemDet.LstItem[i].Amt.ToString() != "") oSapu.itdlabamt = Convert.ToDecimal(ItemDet.LstItem[i].Amt.ToString()); else oSapu.itdlabamt = 0;
                    if (ItemDet.LstItem[i].ItemNetAmt.ToString() != "") oSapu.itdamou = Convert.ToDecimal(ItemDet.LstItem[i].ItemNetAmt.ToString()); else oSapu.itdamou = 0;


                    oSapu.itdtowt = 0;//Convert.ToInt32(ItemDet.LstItem[i].Qty);//170830
                    if (oCls.msttype == 42) oSapu.itdpenq = oSapu.itdquan = -Convert.ToInt32(ItemDet.LstItem[i].itdquan);
                    else oSapu.itdpenq = oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].itdquan);
                    //oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].Qty);//170830
                    oSapu.itdpkin = 0;
                    //oSapu.itdmill = 1;
                    oSapu.itdgodo = 1;
                    if (ItemDet.LstItem[i].itdmill.ToString() != "") oSapu.itdmill = Convert.ToInt32(ItemDet.LstItem[i].itdmill.ToString());//170830
                    if (ItemDet.LstItem[i].itdrema != null && ItemDet.LstItem[i].itdrema.ToString() != "") oSapu.itdrema = ItemDet.LstItem[i].itdrema.ToString();
                    if (ItemDet.LstItem[i].itdrema != null && ItemDet.LstItem[i].itdrema.ToString() != "") oSapu.itdtagno = ItemDet.LstItem[i].itdrema.ToString();//170830

                    oSapu.itdlswt = 0;//170831
                    oSapu.itdqtyd = 0;
                    //if (ItemDet.LstItem[i].DisPer.ToString() != "") oSapu.itdperd = Convert.ToDecimal(ItemDet.LstItem[i].DisPer.ToString()); else oSapu.itdperd = 0;
                    //if (ItemDet.LstItem[i].DisAmt.ToString() != "") oSapu.itdqtyd = Convert.ToDecimal(ItemDet.LstItem[i].DisAmt.ToString()); else oSapu.itdqtyd = 0;

                    if (ItemDet.LstItem[i].DisPer.ToString() != "") oSapu.itddscp = Convert.ToDecimal(ItemDet.LstItem[i].DisPer.ToString()); else oSapu.itddscp = 0;
                    // if (ItemDet.LstItem[i].DisAmt.ToString() != "") oSapu.itdperd = Convert.ToDecimal(ItemDet.LstItem[i].DisAmt.ToString()); else oSapu.itdperd = 0; 300818
                    //if (ItemDet.LstItem[i].DisAmt.ToString() != "") oGath.gathdscv = oSapu.itdperd = Convert.ToDecimal(ItemDet.LstItem[i].DisAmt.ToString()) * oSapu.itdquan; else oSapu.itdperd = 0;  
                    if (ItemDet.LstItem[i].DisAmt.ToString() != "") oSapu.itdperd = Convert.ToDecimal(ItemDet.LstItem[i].DisAmt.ToString()); else oSapu.itdperd = 0;
                    if (ItemDet.LstItem[i].DisAmt.ToString() != "") oGath.gathdscv = Convert.ToDecimal(ItemDet.LstItem[i].DisAmt.ToString()) * oSapu.itdquan; else oGath.gathdscv = 0;

                    if (ItemDet.LstItem[i].DisAmt.ToString() != "") oGath.gathlabp = Convert.ToDouble(ItemDet.LstItem[i].DisAmt);

                    if (ItemDet.LstItem[i].itddime.ToString() != "") oSapu.itddime = ItemDet.LstItem[i].itddime.ToString(); else oSapu.itddime = "0";//itddscp

                    oGath.gathqtdi = (int)oSapu.itddscp;
                    oSapu.itdlabonw = 0;//170831
                    oSapu.itdvatinc = 0;//170831
                    oSapu.itdcasert = 0;//170831
                    //oSapu.itddscp = 0; 

                    oGath.gathcode = oSapu.itdgath = sGathCode;

                    oSapu.itdempo = iEmpoID;// SessionMaster.UserID;//0;170831
                    //oSapu.itdlabamt = 0;

                    oGath.gathwast = ItemDet.LstItem[i].TaxPer.ToString();

                    //{170830
                    oSapu.itdgstrtv = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer);
                    oSapu.itdigstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt);

                    if (oCls.GSTStateVal == "0")
                    {
                        //if (Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal) > 0) oSapu.itdcgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal);
                        oSapu.itdcgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / 2;
                        oSapu.itdcgstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt) / 2;

                        oSapu.itdsgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / 2;
                        oSapu.itdsgstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt) / 2;

                        // oSapu.itdigstrt = 0; oSapu.itdigstvl = 0;
                    }
                    else if (oCls.GSTStateVal == "1")
                    {
                        oSapu.itdcgstrt = 0; oSapu.itdcgstvl = 0; oSapu.itdsgstrt = 0; oSapu.itdsgstvl = 0;
                        oSapu.itdigstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer);
                        oSapu.itdigstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt);
                    }
                    oSapu.itdcessrt = Convert.ToDecimal(0); oSapu.itdcessvl = Convert.ToDecimal(0); oSapu.itdugstrt = Convert.ToDecimal(0); oSapu.itdugstvl = Convert.ToDecimal(0);

                    oSubmit.insSapuItd(oSapu, sItmTbl);
                    oSubmit.insGathDet(oGath);

                }

                //******************************************* RefTab ********************************
                if (oCls.mstfinc == 1)
                {
                    refetab Ref = new refetab();
                    Ref.compcode = oCls.compcode; Ref.ms1code = oCls.mstcode; Ref.ms1type = oCls.msttype;
                    Ref.ms1srno = 1; Ref.ms2code = RefCode; Ref.ms2type = 1174;
                    Ref.ms2srno = (int)oCls.mstprtc;
                    Ref.reftype = 18; Ref.refitem = 0; Ref.refmill = 1;
                    Ref.refrema = oCls.mstchno; Ref.ms2date = oCls.mstdate; Ref.ms1date = oCls.mstdate;
                    Ref.refamou = oCls.msttota;
                    oSubmit.InsRefetab(Ref);
                }

                //********************************* User Work ********************************* 
                clsUserWork oUser = new clsUserWork();
                oUser.woruser = SessionMaster.UserID;
                if (oCls.IsEdit != true) oUser.wormode = 0; else oUser.wormode = 1;
                oUser.worcomp = oCls.compcode;
                oUser.wortype = oCls.msttype;
                oUser.worcode = oCls.mstcode;
                oUser.worsrno = oSubmit.GetUsWoCode();
                oUser.worrema = "D-" + oCls.acctname + "#" + oCls.mstdate + "#" + oCls.mstchno;
                oUser.wordate = oCls.mstdate;
                oUser.worrfsr = "";
                oUser.worsysn = Environment.MachineName;
                oUser.IP_Add = this.Request.UserHostAddress;
                //oUser.WorChNo = oCls.mstchno;
                //oUser.WorNarr = oCls.mstrema;
                oSubmit.InsUserWork(oUser);
                //********************************* User Work ********************************* 

                if (oCls.IsEdit != true)
                {
                    oCls.StDate = Convert.ToDateTime(oCom.getOpenDate(DateTime.Now.Date));
                    oCls.LastDate = Convert.ToDateTime(oCom.getClosDate(DateTime.Now.Date));
                    oSubmit.UpdCodeGen(oCls);
                }
                oCls.mstcode_Print = oCls.mstcode;
                oSubmit.Commit();

                TempData["message"] = "Saved Successfully ...";
                dt2 = oSubmit.GetData("SELECT count(*)+1 Total from OrdeMst where compcode =" + oCls.compcode + " and msttype = " + oCls.msttype + " and Mstuser = " + Session["UserID"], true);
                oCls.mstprtc = Convert.ToInt32(dt2.Rows[0]["Total"].ToString());
                oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from Ordemst where compcode ='" + oCls.compcode + "' and msttype ='" + oCls.msttype + "'"));
                IsSave = 1;

            }
            catch (Exception ex)
            {
                IsSave = 0;
                oSubmit.RollBack();
                TempData["message"] = ex.Message;

                //return RedirectToAction("Order", new { MstType = oCls.msttype, MstCode = 0, comp = oCls.compcode });
                var Data = new { mstprtc = oCls.mstprtc, IsSave = 2, mstcode = oCls.mstcode, mstchno = oCls.mstchno, msg = ex.Message };
                return Json(Json(Data).Data, JsonRequestBehavior.AllowGet);

            }
            finally
            {
                oSubmit.CloseConnection();
            }
            //return RedirectToAction("Quotation", new { MstType = oCls.msttype, MstCode = oCls.mstcode, comp = oCls.compcode, mstcode_Print = oCls.mstcode_Print });

            dt2 = oSubmit.GetData("select isnull(max(right(mstchno,2))+1,1) as maxchno  from OrdeMst where compcode = " + oCls.compcode + " and msttype = " + oCls.msttype + " and left(mstchno, 6) = '" + sMstChNo + "'", true);
            oCls.mstchno = sMstChNo + GetVoucherNo(dt2.Rows[0]["maxchno"].ToString());

            var sData = new { mstprtc = oCls.mstprtc, IsSave = IsSave, mstcode = oCls.mstcode, mstchno = oCls.mstchno };
            return Json(Json(sData).Data, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetLeadNo()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;

            string sSql = "";
            if (Convert.ToInt16(Session["UserType"]) > 1)
                sSql = " and a.UseCode =" + Session["UserID"];

            //dt = oSubmit.GetData("SELECT a.msternv itgpName,a.MstCode itgpcode ,b.mstrefc from OrdeMst a Left Join Ordemst b on  b.mstType = 1163 and cast(a.MstCode as varchar(10)) =  b.mstrefc where a.compcode =62 and a.msttype = 1147 and b.mstrefc is null " + sSql + " order by a.msternv ");

            dt = oSubmit.GetData("SELECT a.L_No itgpName,a.L_ID itgpcode ,b.mstrefc from tblFollow a  Left Join Ordemst b on  b.mstType = 1163 and cast(a.L_ID as varchar(10)) =  b.mstrefc where a.Status_I = 1 and b.mstrefc is null  order by a.L_No ", true);

            var result = oSubmit.getList(dt, "itgpName", "itgpcode");

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetOrderNo()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;

            string sSql = "";
            if (Convert.ToInt16(Session["UserType"]) > 1)
                sSql = " and a.UseCode =" + Session["UserID"];

            //dt = oSubmit.GetData("SELECT a.msternv itgpName,a.MstCode itgpcode ,b.mstrefc from OrdeMst a Left Join Ordemst b on  b.mstType = 1163 and cast(a.MstCode as varchar(10)) =  b.mstrefc where a.compcode =62 and a.msttype = 1147 and b.mstrefc is null " + sSql + " order by a.msternv ");

            dt = oSubmit.GetData("SELECT a.MstChNo itgpName,a.MstCode itgpcode ,b.refrema mstrefc from Ordemst a Left Join refetab b on a.Compcode = b.Compcode and b.ms2Type = a.mstType and a.MstCode = b.ms2Code where a.compcode = " + SessionMaster.CompCode + " and a.mstType = 1163 and b.ms2Code is null  " + sSql + " order by a.Mstcode ", true);

            //SELECT a.MstChNo itgpName,a.MstCode itgpcode ,b.mstrefc from Ordemst a  Left Join Ordemst b on  b.mstType = 174 and a.MstCode = b.mstrefc where a.compcode = " + SessionMaster.CompCode + " and a.mstType = 1163 and b.mstrefc is null  order by a.Mstcode ");

            var result = oSubmit.getList(dt, "itgpName", "itgpcode");

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInquiryDet(int iMstcode, int iCompcode, int iType)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();

            List<clsPoItem> oList = new List<clsPoItem>();

            oList = oSubmit.GetInquiryItem(iType, iCompcode, iMstcode);

            //var sData = new { oList = oList };
            return Json(Json(oList).Data, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetOrderData(string From_Date, string To_Date, int CompCode, int iState, int iCity, int DealerID, int ExecutiveID)
        {

            //  var Data = from a in DB.ordemsts where a.msttype == 99999999 select a;
            if (From_Date == "") From_Date = "01/01/1900";
            if (To_Date == "") To_Date = "01/01/2070";

            //if (From_Date != "" && To_Date != "")
            //{

            clsSubmitModel oSubmit = new clsSubmitModel();
            //if (SessionMaster.UserType == 0)
            List<clsOrderData> Data = (from a in DB.ordemsts
                                       join comp in DB.companies on a.compcode equals comp.compcode
                                       join b in DB.accounts on new { _grou = 21, _Code = (int)a.mstprtc } equals new { _grou = (int)b.acctgrou, _Code = b.acctcode }
                                       join c in DB.citydets on new { _Type = 15, _Code = (int)b.acctcity } equals new { _Type = c.cityType, _Code = c.citycode } into d
                                       from e in d.DefaultIfEmpty()
                                       join f in DB.citydets on new { _Type = 67, _Code = (int)b.acctstat } equals new { _Type = f.cityType, _Code = f.citycode } into g
                                       from h in g.DefaultIfEmpty()
                                       where (CompCode > 0 ? a.compcode == CompCode : true)
                                       where (iCity > 0 ? b.acctcity == iCity : true)
                                       where (iState > 0 ? b.acctstat == iState : true)
                                       where (DealerID > 0 ? a.mstprtc == DealerID : true)
                                       where (ExecutiveID > 0 ? a.mstexec == ExecutiveID : true)
                                       where a.msttype == 174
                                       where a.mstdate.Date >= oSubmit.GetDateFormat(From_Date)
                                       where a.mstdate.Date <= oSubmit.GetDateFormat(To_Date)
                                       orderby a.mstdate descending
                                       select new clsOrderData { mstchno = a.mstchno, mstDrawNo = a.mstDrawNo, mstContactPerson = a.mstContactPerson, mstrema = a.mstrema, mstcode = a.mstcode, compcode = a.compcode, msttota = a.msttota, mstdate = a.mstdate, City = e.cityname, Dealer = b.acctname, Company = comp.compname }).ToList();


            //  else
            //     Data = from a in DB.ordemsts where (CompCode > 0 ? a.compcode == CompCode : true) where a.msttype == 174 where a.mstuser == SessionMaster.UserID where a.mstdate.Date >= oSubmit.GetDateFormat(From_Date) where a.mstdate.Date <= oSubmit.GetDateFormat(To_Date) orderby a.mstdate descending select a;
            // }
            //else
            //{
            //    if (SessionMaster.UserType == 0)
            //        Data = from a in DB.ordemsts where (CompCode > 0 ? a.compcode == CompCode : true) where a.msttype == 174 orderby a.mstdate descending select a;
            //    else
            //        Data = from a in DB.ordemsts where (CompCode > 0 ? a.compcode == CompCode : true) where a.msttype == 174 where a.mstuser == SessionMaster.UserID orderby a.mstdate descending select a;
            //}

            return PartialView("_OrderList", Data);
        }


        public ActionResult GetOpening(int DealerID, int iMstcode, string sDate)
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            //var ITEMS = new { Opening = 0.0 , GSTStateVal = 0 };
            DateTime trndate = oSubmit.GetDateFormat(sDate);

            decimal dOpening = 0; int dGSTStateVal = 0;
            DataTable dt;
            //dt = oSubmit.GetData("select top 1 Opening   from tblOpening where DealerID = " + DealerID + " order by  CreatedOn desc");
            dt = oSubmit.GetData("select isnull(sum(trndram-trncram),0) Opening from jourtrn a inner join jourmst b on b.compcode = a.compcode and  b.mstcode = a.trncode  and b.mstdate = a.trndate  and b.msttype = a.trntype  where a.compcode = '" + SessionMaster.CompCode + "' and trnitem = '" + DealerID + "'  and( (datediff(d, trndate, '" + trndate + "') = 0 and trncode <" + iMstcode + "  ) or (datediff(d, trndate, '" + trndate + "') = 0 ) or ((datediff(d, '" + trndate + "', trndate) <  0)))", true);

            if (dt.Rows.Count > 0)
            {
                //if (Convert.ToDecimal(dt.Rows[0]["Opening"].ToString()) > 0) dOpening = Convert.ToDecimal(dt.Rows[0]["Opening"].ToString());
                if (Convert.ToDecimal(dt.Rows[0]["Opening"].ToString()) > 0) dOpening = Convert.ToDecimal(dt.Rows[0]["Opening"].ToString());
            }

            dt = oSubmit.GetData("select isNull(acctcform,0)acctcform from Account Where acctCode = " + DealerID, true);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["acctcform"].ToString() != "") dGSTStateVal = Convert.ToInt32(dt.Rows[0]["acctcform"].ToString());
            }


            //ITEMS = new { Opening = dOpening ,GSTStateVal = dGSTStateVal  };

            return Json(new { Opening = dOpening, GSTStateVal = dGSTStateVal }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult getProduction(bool IsApprove)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            List<Jourmaster> lstJourmaster = oSubmit.getProduction(IsApprove);

            if (IsApprove == true)
                return PartialView("ApproveOrder", lstJourmaster);
            else
                return PartialView("_ProductionList", lstJourmaster);
        }

        [HttpPost]
        public ActionResult OrderApprove(string OrderList)
        {

            string Message = "Successfully Approved ...";
            try
            {
                string[] Row = OrderList.Split('|');
                string[] Col;

                //ERPDataContext DB = new ERPDataContext();
                tblOrderApprove oOrder = new tblOrderApprove();

                foreach (string itm in Row)
                {
                    Col = itm.Split(',');
                    if (Col[0].ToString() != "")
                    {
                        oOrder = new tblOrderApprove();
                        oOrder.CompCode = SessionMaster.CompCode;
                        oOrder.Msttype = Convert.ToInt32(Col[0].ToString());
                        oOrder.OrderID = Convert.ToInt32(Col[1].ToString());
                        oOrder.OrderNO = Col[2].ToString();
                        oOrder.CreatedBy = SessionMaster.UserID;
                        oOrder.CreatedOn = DateTime.Now;

                        DB.tblOrderApproves.InsertOnSubmit(oOrder);
                        DB.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return Json(Message, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetItem(int subcat, int Brand = 0)
        {
            //ERPDataContext db = new ERPDataContext();

            var Item = from a in DB.itemains where a.compcode == SessionMaster.CompCode where (subcat > 0 ? a.itemgrou == subcat : true) where (Brand > 0 ? a.itemnumb == Brand : true) orderby a.itemname select new { a.itemcode, a.itemrate, a.itemgstr, a.itemname };

            return Json(Item, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PlanDispatch(int comp = 0, int id = 0, int mstCode = 0, int MstType = 0, int mstcode_Print = 0)
        {
            try
            {

                Jourmaster frm = new Jourmaster();
                clsSubmitModel oSubmit = new clsSubmitModel();

                string sSql = "";
                if (Convert.ToInt16(Session["UserType"]) > 1)
                    sSql = " and a.MstUser =" + Session["UserID"];

                var OrderNo = from a in DB.tblOrderApproves
                              join b in DB.refetabs on
                                  new { Comp = (int)a.CompCode, TP = (int)a.Msttype, Code = (int)a.OrderID } equals new { Comp = b.compcode, TP = b.ms2type, Code = b.ms2code } into c
                              from d in c.DefaultIfEmpty()
                              where d.ms2code == null
                              orderby a.CreatedOn descending
                              select new { a.OrderID, a.OrderNO };

                ViewBag.OrderNo = new SelectList(OrderNo, "OrderID", "OrderNO");

                DataTable dtFterm; DataTable dtOterm;
                dtFterm = oSubmit.GetData("select studName ,studCode from studdet where studtype  = 55 order By StudName", true);
                frm.lstFTerm = oSubmit.getList(dtFterm, "studName", "studCode");
                dtOterm = oSubmit.GetData("select studName ,studCode from studdet where studtype  = 44 order By StudName", true);
                frm.lstOTerm = oSubmit.getList(dtOterm, "studName", "studCode");

                //var vwCate = oDB.Database.SqlQuery<itgroup>("select itgpName ,itgpcode  from  itGroup where itgpunde = 0 and Compcode = " + SessionMaster.CompCode);
                //var vwCate = from a in DB.itgroups where a.itgpunde == 0 where a.compcode == SessionMaster.CompCode select new { a.itgpname, a.itgpcode };
                //ViewBag.vwCategory = new SelectList(vwCate, "itgpcode", "itgpName");

                //ViewBag.ItemNm = new SelectList(from res in oDB.itemains where res.itemgrou == 11 orderby res.itemname select new { res.itemcode, res.itemname }, "itemcode", "itemname");
                ViewBag.ItemNm = new SelectList(from res in DB.itemains orderby res.itemname select new { res.itemcode, res.itemname }, "itemcode", "itemname");

                //select * from studdet where studType =61 

                ViewBag.Brand = new SelectList(from a in DB.studdets where a.studType == 61 select new { a.studName, a.studCode }, "studCode", "studName");
                if (SessionMaster.UserType > 0)
                    ViewBag.SubCategory = new SelectList(from a in DB.itgroups where a.compcode == SessionMaster.CompCode where a.itgpunde != 0 select new { a.itgpname, a.itgpcode }, "itgpcode", "itgpname"); //where a.itgpunde == 61
                else
                    ViewBag.SubCategory = new SelectList(from a in DB.itgroups where a.compcode == SessionMaster.CompCode where a.itgpunde != 0 select new { a.itgpname, a.itgpcode }, "itgpcode", "itgpname");

                if (mstCode > 0)
                {
                    #region  "Edit"
                    var model = oSubmit.GetInquiry(MstType, comp, mstCode);

                    model.PartyID = model.mstcust;
                    model.ChallanDate = Convert.ToDateTime(model.mstDueDate).ToString("dd/MM/yyyy");
                    model.sMstdate = Convert.ToDateTime(model.mstdate).ToString("dd/MM/yyyy");

                    //ViewBag.mstsale = new SelectList(from res in DB.studdets where res.studType == 860 orderby res.studName select new { res.studCode, res.studName }, "studCode", "studName", model.mstsale);
                    //ViewBag.SubCate = new SelectList(from res in DB.itgroups where res.compcode == SessionMaster.CompCode orderby res.itgpname select new { res.itgpcode, res.itgpname }, "itgpcode", "itgpname");
                    //ViewBag.itemnumb = new SelectList(from res in DB.studdets where res.studType == 61 select new { res.studCode, res.studName }, "studcode", "studname");

                    model.lstFTerm = oSubmit.getList(dtFterm, "studName", "studCode");
                    model.lstOTerm = oSubmit.getList(dtOterm, "studName", "studCode");

                    return View("Order", model);
                    #endregion
                }
                else
                {
                    #region "Create"

                    // frm.msttimes = DateTime.Now.ToString("HH:mm:ss");
                    DataTable dt2;

                    dt2 = oSubmit.GetData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from OrdeMst where compcode ='" + comp + "' and msttype ='" + MstType + "'", true);
                    ViewBag.MstCode = frm.mstcode = Convert.ToInt32(dt2.Rows[0]["mstcode"]);
                    frm.compcode = Convert.ToInt32(SessionMaster.CompCode);
                    frm.msttype = frm.type = MstType;
                    frm.mstdate = DateTime.Now.Date;
                    frm.mstcode_Print = mstcode_Print;
                    //ViewBag.mstsale = new SelectList(from res in DB.studdets where res.studType == 860 select new { res.studCode, res.studName }, "studCode", "studName");
                    //ViewBag.SubCate = new SelectList(from res in DB.itgroups where res.compcode == SessionMaster.CompCode orderby res.itgpname select new { res.itgpcode, res.itgpname }, "itgpcode", "itgpname");
                    //ViewBag.itemnumb = new SelectList(from res in DB.studdets where res.studType == 61 select new { res.studCode, res.studName }, "studcode", "studname");

                    //ViewBag.MstBlno = new SelectList(from res in DB.GetInqNo(5176) select new { res.mstCode, res.MstChNo }, "mstCode", "MstChNo");

                    //var BlNo = new SelectList(from res in DB.GetInqNo(5176) select new { res.mstCode, res.MstChNo }, "mstCode", "MstChNo");

                    dt2 = oSubmit.GetData("SELECT count(*)+1 Total from OrdeMst where compcode =" + comp + " and msttype = " + MstType + " and Mstuser = " + Session["UserID"], true);
                    //frm.mstprtc = Convert.ToInt32(dt2.Rows[0]["Total"].ToString());

                    string sMstChNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");

                    dt2 = oSubmit.GetData("select isnull(max(right(mstchno,2))+1,1) as maxchno  from OrdeMst where compcode = " + comp + " and msttype = " + MstType + " and left(mstchno, 6) = '" + sMstChNo + "'", true);
                    ViewBag.mstchno = frm.mstchno = sMstChNo + GetVoucherNo(dt2.Rows[0]["maxchno"].ToString());

                    frm.IsEdit = false;

                    #endregion
                }
                return View("PlanDispatch", frm);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult PlanDispatch(Jourmaster oCls)
        {
            DataTable dt2;
            clsSubmitModel oSubmit = new clsSubmitModel();
            commFunction oCom = new commFunction();
            int IsSave = 0;
            string sMstChNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");

            try
            {
                jourtrn oTrn = new jourtrn();
                sapuitd oSapu = new sapuitd();
                gathdet oGath = new gathdet();
                int RefCode = Convert.ToInt32(oCls.mstrefc);

                string sGathCode = "";
                string sItmTbl = "ordeitd";

                //dt2 = oSubmit.GetData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from jourmst where compcode ='" + comp + "' and msttype ='" + MstType + "'");

                if (Request.QueryString["MstCode"] != null && Convert.ToInt32(Request.QueryString["MstCode"]) > 0) { }
                else
                {
                    oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from Ordemst where compcode ='" + SessionMaster.CompCode + "' and msttype ='" + oCls.msttype + "'"));
                    //oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from jourmst where compcode ='" + SessionMaster.CompCode + "' and msttype ='" + oCls.msttype + "'"));
                }

                var json = oCls.sItemDet;
                clsPoItem ItemDet = JsonConvert.DeserializeObject<clsPoItem>(json);

                ViewBag.MstType = oCls.msttype;
                int iDays = 0;
                decimal iComm = 0, iInterest = 0;

                oCls.mstdate = oSubmit.GetDateFormat(oCls.sMstdate);

                if (oCls.mstdued > 0) iDays = (int)oCls.mstdued;//DueDay 170831
                if (oCls.Comm > 0) iComm = oCls.Comm;
                if (oCls.Interest > 0) iInterest = oCls.Interest;

                if (oCls.mstchnH == null) oCls.mstchnH = "";
                oCls.mststat = 0;
                //oCls.msttime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
                //oCls.mstrefc = getParty(Convert.ToInt32(oCls.PartyID)) + "~" + iInterest + "~" + iDays + "~" + iComm;
                oCls.mstrefc = getParty(Convert.ToInt32(oCls.mstprtc)) + "~" + iInterest + "~" + iDays + "~" + iComm;
                if (oCls.msttype == 42)
                {
                    oCls.mstchno = oCls.mstchnH + oCls.mstchnV;//170830
                    oCls.mstexti = "~~#0";//170830
                }
                else
                {
                    oCls.mstexti = "~";//170830
                    oCls.mstchnV = 0;
                }
                oCls.mstgncd = "0~0~" + oCls.acctledg + "~0";//170830



                oCls.mstAppr = 0; oCls.mstqtyd = 0; oCls.mstvat1 = 0; oCls.mstvat2 = 0;
                oCls.mstvat3 = 0; oCls.mstchnm = ""; oCls.oldwht = 0; oCls.mstsite = 0;
                oCls.mstbrnc = 0; oCls.mstsubt = 0; oCls.mstcust = 0; oCls.mstrvsc = 0;
                oCls.mstcust = oCls.PartyID; //mstprtc
                //oCls.msternv = "";

                oCls.mstcurrcd = 1;
                oCls.mstcurrrat = 1;
                oCls.mstintr = 0;

                //oCls.mstDueDate = oSubmit.GetDateFormat(oCls.ChallanDate);  //oCls.mstdate.AddDays(iDays);//170831
                oCls.mstdate = oSubmit.GetDateFormat(oCls.sMstdate);

                oCls.mstbuyerc = 0; oCls.mstperd = 0; oCls.mstdsptoc = 0;
                if (oCls.GSTStateVal == "1")
                    oCls.mstIorL = "I";
                else
                    oCls.mstIorL = "L";

                dt2 = oSubmit.GetData("select isnull(max(right(mstchno,2))+1,1) as maxchno  from ordemst where compcode = " + SessionMaster.CompCode + " and msttype = " + oCls.msttype + " and left(mstchno, 6) = '" + sMstChNo + "'", true);
                oCls.mstchno = sMstChNo + GetVoucherNo(dt2.Rows[0]["maxchno"].ToString());

                oCls.mstuser = SessionMaster.UserID;
                oCls.mstsale = 115;
                oSubmit.BeginTran();
                //oSubmit.InsJourmst(oCls, "");
                oSubmit.InsOrdeMst(oCls);
                oSubmit.insertData("delete from ordetrn  where compcode = " + oCls.compcode + " and trntype = " + oCls.msttype + " and  trncode = " + oCls.mstcode + " and  trnDate = '" + oCls.mstdate + "'");

                oSubmit.insertData("delete b from " + sItmTbl + " a inner join GathDet b on a.itdgath = b.gathCode and a.Itdtype =b.Itdtype where a.Itdtype = " + oCls.msttype + " and Itdcode = " + oCls.mstcode);

                oSubmit.insertData("Delete from " + sItmTbl + " where CompCode = " + oCls.compcode + " and Itdtype = " + oCls.msttype + " and Itdcode = " + oCls.mstcode);

                for (int i = 0; i < ItemDet.LstItem.Count; i++)
                {

                    //var q = from a in db.GetTable<charcodege>() select a; 
                    //var w = from res in db.charcodeges select new { res.gatcode };

                    sGathCode = oSubmit.GetUsWoCode();

                    oSapu.compcode = Convert.ToInt16(oCls.compcode);
                    oSapu.itdtype = Convert.ToInt32(oCls.msttype);
                    oSapu.itdcode = Convert.ToInt32(oCls.mstcode);
                    oSapu.itdtime = Convert.ToInt32(oCls.msttime);
                    oSapu.itdsrno = Convert.ToInt16(i + 1);
                    oGath.gathpuri = oGath.gathstat = Convert.ToString(oSapu.itddate = Convert.ToDateTime(oCls.mstdate));
                    //oSapu.itdtime = Convert.ToInt32(oCls.msttimes.ToString().Replace(":", ""));
                    oSapu.itditem = Convert.ToInt32(ItemDet.LstItem[i].itditem);
                    //oGath.gathdesc = oSapu.itdrema = ItemDet.LstItem[i].Remark.ToString();// ItemRemark.ToString();
                    //oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].Qty);
                    if (ItemDet.LstItem[i].UnitID.ToString() != "") oSapu.itdunit = Convert.ToInt32(ItemDet.LstItem[i].UnitID); else oSapu.itdunit = 0;
                    oSapu.itddime = ItemDet.LstItem[i].Rate.ToString();
                    if (ItemDet.LstItem[i].Rate.ToString() != "") oSapu.itdactprc = Convert.ToDecimal(ItemDet.LstItem[i].itdrate.ToString());//170830
                    oSapu.itdvate = oSapu.itdrate = Convert.ToDecimal(ItemDet.LstItem[i].itdrate);
                    oGath.gathwtdi = Convert.ToDecimal(oSapu.itdvate);
                    oSapu.itdamou = Convert.ToDecimal(ItemDet.LstItem[i].Amt);
                    oSapu.itdtowt = 0;//Convert.ToInt32(ItemDet.LstItem[i].Qty);//170830
                    if (oCls.msttype == 42) oSapu.itdpenq = oSapu.itdquan = -Convert.ToInt32(ItemDet.LstItem[i].itdquan);
                    else oSapu.itdpenq = oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].itdquan);
                    //oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].Qty);//170830
                    oSapu.itdpkin = 0;
                    //oSapu.itdmill = 1;
                    oSapu.itdgodo = 1;

                    if (ItemDet.LstItem[i].itdmill != null && ItemDet.LstItem[i].itdmill.ToString() != "") oSapu.itdmill = Convert.ToInt32(ItemDet.LstItem[i].itdmill.ToString());//170830
                    //if (ItemDet.LstItem[i].itdrema.ToString() != "") oSapu.itdrema = ItemDet.LstItem[i].itdrema.ToString();//170830
                    if (ItemDet.LstItem[i].itdrema != null && ItemDet.LstItem[i].itdrema.ToString() != "") oSapu.itdtagno = ItemDet.LstItem[i].itdrema.ToString();//170830

                    oSapu.itdlswt = 0;//170831

                    if (ItemDet.LstItem[i].DisPer.ToString() != "") oSapu.itdperd = Convert.ToDecimal(ItemDet.LstItem[i].DisPer.ToString()); else oSapu.itdperd = 0;
                    if (ItemDet.LstItem[i].DisAmt.ToString() != "") oSapu.itdqtyd = Convert.ToDecimal(ItemDet.LstItem[i].DisAmt.ToString()); else oSapu.itdqtyd = 0;
                    if (ItemDet.LstItem[i].itddime.ToString() != "") oSapu.itddime = ItemDet.LstItem[i].itddime.ToString(); else oSapu.itddime = "0";//itddscp

                    oSapu.itdlabonw = 0;//170831
                    oSapu.itdvatinc = 0;//170831
                    oSapu.itdcasert = 0;//170831
                    oSapu.itddscp = 0;//170831

                    oGath.gathcode = oSapu.itdgath = sGathCode;
                    oSapu.itdempo = SessionMaster.UserID;//0;170831
                    //oSapu.itdlabamt = 0;
                    if (ItemDet.LstItem[i].ItemNetAmt.ToString() != "") oSapu.itdlabamt = Convert.ToDecimal(ItemDet.LstItem[i].ItemNetAmt.ToString()); else oSapu.itdlabamt = 0;
                    oGath.gathwast = ItemDet.LstItem[i].TaxPer.ToString();

                    //{170830
                    oSapu.itdigstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer);
                    oSapu.itdigstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt);
                    if (oCls.GSTStateVal == "0")
                    {
                        oSapu.itdcgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / 2;
                        oSapu.itdcgstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt) / 2;

                        oSapu.itdsgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / 2;
                        oSapu.itdsgstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt) / 2;

                        oSapu.itdigstrt = 0; oSapu.itdigstvl = 0;
                    }
                    else if (oCls.GSTStateVal == "1")
                    {
                        oSapu.itdcgstrt = 0; oSapu.itdcgstvl = 0; oSapu.itdsgstrt = 0; oSapu.itdsgstvl = 0;
                        oSapu.itdigstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer);
                        oSapu.itdigstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt);
                    }

                    oSapu.itdcessrt = Convert.ToDecimal(0);
                    oSapu.itdcessvl = Convert.ToDecimal(0);
                    oSapu.itdugstrt = Convert.ToDecimal(0);
                    oSapu.itdugstvl = Convert.ToDecimal(0);
                    //}170830
                    oSubmit.insSapuItd(oSapu, sItmTbl);
                    oSubmit.insGathDet(oGath);

                }

                oTrn = new jourtrn();
                oTrn.compcode = Convert.ToInt16(oCls.compcode);
                oTrn.trntype = oCls.msttype;
                oTrn.trncode = oCls.mstcode;
                oTrn.trntime = oCls.msttime;
                oTrn.trnsrno = 1;
                oTrn.trndate = oCls.mstdate;
                oTrn.trnledg = oTrn.trnitem = (int)oCls.mstprtc;
                oTrn.trndram = oCls.msttota;
                oTrn.trncram = 0;
                oSubmit.InsJourTrn(oTrn, true);

                oTrn = new jourtrn();
                oTrn.compcode = Convert.ToInt16(oCls.compcode);
                oTrn.trntype = oCls.msttype;
                oTrn.trncode = oCls.mstcode;
                oTrn.trntime = oCls.msttime;
                oTrn.trnsrno = 2;
                oTrn.trndate = oCls.mstdate;
                oTrn.trnledg = oTrn.trnitem = 115;
                oTrn.trncram = oCls.msttota;
                oTrn.trndram = 0;
                oSubmit.InsJourTrn(oTrn, true);

                //******************************************* RefTab ********************************
                refetab Ref = new refetab();
                Ref.compcode = oCls.compcode; Ref.ms1code = oCls.mstcode; Ref.ms1type = oCls.msttype;
                Ref.ms1srno = 1; Ref.ms2code = RefCode; Ref.ms2type = 174;
                Ref.ms2srno = (int)oCls.mstprtc;
                Ref.reftype = 17; Ref.refitem = 0; Ref.refmill = 1;
                Ref.refrema = oCls.mstchno; Ref.ms2date = oCls.mstdate; Ref.ms1date = oCls.mstdate;
                Ref.refamou = oCls.msttota;
                oSubmit.InsRefetab(Ref);
                //******************************************* *****************************************
                if (oCls.IsEdit != true)
                {
                    oCls.StDate = Convert.ToDateTime(oCom.getOpenDate(DateTime.Now.Date));
                    oCls.LastDate = Convert.ToDateTime(oCom.getClosDate(DateTime.Now.Date));
                    oSubmit.UpdCodeGen(oCls);
                }
                oCls.mstcode_Print = oCls.mstcode;
                oSubmit.Commit();

                TempData["message"] = "Saved Successfully ...";
                dt2 = oSubmit.GetData("SELECT count(*)+1 Total from OrdeMst where compcode =" + SessionMaster.CompCode + " and msttype = " + oCls.msttype + " and Mstuser = " + Session["UserID"], true);
                //oCls.mstprtc = Convert.ToInt32(dt2.Rows[0]["Total"].ToString());
                oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from OrdeMst where compcode ='" + SessionMaster.CompCode + "' and msttype ='" + oCls.msttype + "'"));
                IsSave = 1;

            }
            catch (Exception ex)
            {
                IsSave = 0;
                oSubmit.RollBack();
                TempData["message"] = ex.Message;

                return RedirectToAction("Order", new { MstType = oCls.msttype, MstCode = 0, comp = oCls.compcode });
                var Data = new { mstprtc = oCls.mstprtc, IsSave = IsSave, mstcode = oCls.mstcode, mstchno = oCls.mstchno };
                return Json(Json(Data).Data, JsonRequestBehavior.AllowGet);

            }
            //return RedirectToAction("Quotation", new { MstType = oCls.msttype, MstCode = oCls.mstcode, comp = oCls.compcode, mstcode_Print = oCls.mstcode_Print });

            dt2 = oSubmit.GetData("select isnull(max(right(mstchno,2))+1,1) as maxchno  from OrdeMst where compcode = " + SessionMaster.CompCode + " and msttype = " + oCls.msttype + " and left(mstchno, 6) = '" + sMstChNo + "'", true);
            oCls.mstchno = sMstChNo + GetVoucherNo(dt2.Rows[0]["maxchno"].ToString());

            var sData = new { mstprtc = oCls.mstprtc, IsSave = IsSave, mstcode = oCls.mstcode, mstchno = oCls.mstchno };
            return Json(Json(sData).Data, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetOrderApproves()
        {
            //var OrderNo = from a in DB.tblOrderApproves
            //             join b in DB.ordemsts on
            //                new { TP = 80, Code = (int)a.OrderID } equals new { TP = (int)b.msttype, Code = b.mstcode } into c
            //             from d in c.DefaultIfEmpty()
            //             where d.mstcode == null
            //             orderby a.CreatedOn descending
            //             select new { a.OrderID, a.OrderNO };
            var OrderNo = from a in DB.tblOrderApproves
                          join b in DB.refetabs on
                              new { Comp = (int)a.CompCode, TP = (int)a.Msttype, Code = (int)a.OrderID } equals new { Comp = b.compcode, TP = b.ms2type, Code = b.ms2code } into c
                          from d in c.DefaultIfEmpty()
                          where d.ms2code == null
                          orderby a.CreatedOn descending
                          select new { a.OrderID, a.OrderNO };

            return Json(Json(OrderNo).Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PendingOrder()
        {
            return PartialView("_PendingOrderList");
        }

        public ActionResult PendingOrderList()
        {

            //Select a.MstDate OrdDt ,a.mstchno OrdNo , e.itemname, c.itdquan Ord_Qty, d.itdquan DP_Qty, c.itdquan - isnull(d.itdquan ,0) Pending  from ordeMst a 
            //left join ordeMst b on b.Msttype = 1165 and a.MstCode = b.mstrefc
            //inner join ordeItd c on c.ItdCode = a.MstCode and c.itdType = a.Msttype and c.CompCode = a.CompCode
            //left join ordeItd d on c.ItdCode = b.MstCode and d.itdType = b.Msttype and d.CompCode = b.CompCode and d.ItdItem = c.ItdItem 
            //inner join itemain e on c.compcode = e.compcode and c.itditem  = e.itemcode
            //where a.Msttype = 1164

            var Pending = from a in DB.ordemsts
                          join b in DB.ordeitds on new { _Comp = a.compcode, _Type = (int)a.msttype, _Code = (int)a.mstcode } equals new { _Comp = b.compcode, _Type = b.itdtype, _Code = b.itdcode }
                          join c in DB.itemains on new { _Comp = (int)b.compcode, _Item = (int)b.itditem } equals new { _Comp = c.compcode, _Item = c.itemcode }
                          join d in DB.ordemsts on new { _Comp = a.compcode, _Type = 80, _Code = a.mstcode.ToString() } equals new { _Comp = d.compcode, _Type = (int)d.msttype, _Code = d.mstrefc } into e
                          from f in e.DefaultIfEmpty()
                          join g in DB.ordeitds on new { _Comp = f.compcode, _Type = (int)f.msttype, _Code = (int)f.mstcode, _Item = b.itditem } equals new { _Comp = g.compcode, _Type = g.itdtype, _Code = g.itdcode, _Item = g.itditem } into h
                          from i in h.DefaultIfEmpty()
                          where a.msttype == 174
                          orderby a.mstchno
                          select new { PartyName = a.mstDrawNo, mstdate = String.Format("{0:dd/MM/yyyy}", a.mstdate), mstchno = a.mstchno, c.itemname, itdquan = b.itdquan, msttobil = i.itdquan, mstneta = (b.itdquan - (i.itdquan ?? 0)) };

            return Json(Pending, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult ApproveOrderList()
        //{

        //    var Pending = from a in DB.ordemsts
        //                  join acc in DB.accounts on new { _Comp = a.compcode, _Code = (int)a.mstprtc } equals new { _Comp = acc.compcode, _Code = acc.acctcode }
        //                  join accCT in DB.citydets on new { _CType = 67, _Code = (int)acc.acctstat } equals new { _CType = accCT.cityType, _Code = accCT.citycode }
        //                  into accCT_L
        //                  from State in accCT_L.DefaultIfEmpty()
        //                  join b in DB.ordeitds on new { _Comp = a.compcode, _Type = (int)a.msttype, _Code = (int)a.mstcode } equals new { _Comp = b.compcode, _Type = b.itdtype, _Code = b.itdcode }
        //                  join Brnd in DB.studdets on new { _BType = 61, _BCode = (int)b.itdmill } equals new { _BType = Brnd.studType, _BCode = Brnd.studCode }

        //                  join c in DB.itemains on new { _Comp = (int)b.compcode, _Item = (int)b.itditem } equals new { _Comp = c.compcode, _Item = c.itemcode }
        //                  join d in DB.ordemsts on new { _Comp = a.compcode, _Type = 80, _Chno = a.mstchno.ToString() } equals new { _Comp = d.compcode, _Type = (int)d.msttype, _Chno = d.mstchno } into e
        //                  from f in e.DefaultIfEmpty()
        //                  join g in DB.ordeitds on new { _Comp = f.compcode, _Type = (int)f.msttype, _Code = (int)f.mstcode, _Item = b.itditem } equals new { _Comp = g.compcode, _Type = g.itdtype, _Code = g.itdcode, _Item = g.itditem } into h
        //                  from i in h.DefaultIfEmpty()
        //                  where a.msttype == 174
        //                  where (b.itdquan - (i.itdquan ?? 0)) > 0
        //                  orderby a.mstchno
        //                  select new { PartyName = acc.acctname, mstdate = String.Format("{0:dd/MM/yyyy}", a.mstdate), mstchno = a.mstchno, c.itemname, itdquan = b.itdquan, msttobil = i.itdquan, mstneta = (b.itdquan - (i.itdquan ?? 0)), Brand = Brnd.studName, State = State.cityname };

        //    return Json(Pending, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult DelOrder(int Compcode, int MstType, int Mstcode, string mstchno = "")
        {
            string sMsg = "Order Appored , You Can't Delete It !!!";
            try
            {
                if ((from a in DB.ordemsts where a.compcode == Compcode where a.msttype == 80 where a.mstchno == mstchno select a).Count() > 0)
                { }
                else
                {
                    var Mst = (from a in DB.ordemsts where a.compcode == Compcode where a.msttype == MstType where a.mstcode == Mstcode select a).FirstOrDefault();
                    var Itd = (from a in DB.ordeitds where a.compcode == Compcode where a.itdtype == MstType where a.itdcode == Mstcode select a).FirstOrDefault();

                    DB.ordemsts.DeleteOnSubmit(Mst);
                    DB.ordeitds.DeleteOnSubmit(Itd);

                    DB.SubmitChanges();
                    sMsg = "Delete Successfully ...";
                }
            }
            catch (Exception ex)
            {
                sMsg = ex.ToString();
            }

            return Json(sMsg, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PendingOrdList(int BrandID, int DealerID, int StateID, int SubCateID, string From_Date = "", string To_Date = "", int Executive = 0, int CompanyID = 0, bool isHide = false)
        {
            List<Jourmaster> lstOrder = oSubmit.GetPendingOrder(BrandID, DealerID, StateID, SubCateID, From_Date, To_Date, Executive, CompanyID);
            SessionMaster.isHide = isHide;
            return PartialView("_PendingOrdList", lstOrder);
        }



        public ActionResult ApprovedOrdList(int BrandID, int DealerID, int StateID, int SubCateID, string From_Date = "", string To_Date = "", int Executive = 0, int CompanyID = 0)
        {
            List<Jourmaster> lstOrder = oSubmit.GetApprovedOrder(BrandID, DealerID, StateID, SubCateID, From_Date, To_Date, Executive, CompanyID);

            return PartialView("_ApprovedOrdList", lstOrder);
        }

        [HttpPost]
        public ActionResult PayFollowUp(int CompCode, string AcctList = "", string UptoDate = "", int State = 0, int City = 0, int Due = 0, int Commit = 0, int Planning = 0)
        {
            //List<Jourmaster> lstOrder = oSubmit.GetApprovedOrder(BrandID, DealerID, StateID, SubCateID, From_Date, To_Date, Executive, CompanyID);
            List<DueList> lstOrder = oSubmit.sp_GetDueList(CompCode, AcctList, UptoDate, State, City, true, Due, Commit, Planning);

           // var Dealer = (from a in DB.tblDistributors join acc in DB.accounts on a.MstCode equals acc.AcctDeal join b in DB.EmpAllotMsts on a.MstCode equals b.DealerID join c in DB.employees on b.EmpID equals c.empcode join d in DB.citydets on new { _Type = 15, _Code = (int)acc.acctcity } equals new { _Type = d.cityType, _Code = d.citycode } where a.DistributorID != 0 where (ExecID > 0 ? c.UseCode == ExecID : true) where acc.acctgrou == 21 orderby a.DisName select new { Name = a.DisName + " - " + d.cityname, Code = acc.acctcode }).Distinct().ToList();



            ViewBag.vwBank = new SelectList(from a in DB.accounts where a.acctgrou == 24 select new { a.acctcode, a.acctname }, "acctcode", "acctname");
            return PartialView("PayFollowUp", lstOrder);
        }

     

        public ActionResult PayFollowUpList(int BrandID, int DealerID, int StateID, int SubCateID, string From_Date = "", string To_Date = "", int Executive = 0, int CompanyID = 0, int CityID = 0)
        {
            if (From_Date == "") From_Date = "01/01/1900";
            if (To_Date == "") To_Date = "01/01/2070";

            List<clsPayFollowUp> Data = (from a in DB.tblPayFollowUPs
                                         join b in DB.accounts on new { _Type = 21, _Code = (int)a.PartyID } equals new { _Type = (int)b.acctgrou, _Code = b.acctcode }
                                         where (DealerID > 0 ? b.acctcode == DealerID : true)
                                         where (StateID > 0 ? b.acctstat == StateID : true)
                                         where (CityID > 0 ? b.acctcity == CityID : true)
                                         where (Executive > 0 ? a.UseCode == Executive : true)
                                         where a.NextDt.Value.Date >= oSubmit.GetDateFormat(From_Date)
                                         where a.NextDt.Value.Date <= oSubmit.GetDateFormat(To_Date)
                                         select new clsPayFollowUp { FID = a.FID, BillID = (int)a.BillID, PartyID = (int)a.PartyID, BillNo = a.BillNo, FDate = (DateTime)a.FDate, NextDt = (DateTime)a.NextDt, CommitPay = (decimal)a.CommitPay, Acctname = b.acctname, Remark = a.Remark }).ToList();

            return PartialView("PayFollowUpList", Data);
        }

        //  [HttpPost] 
        //public ActionResult SetOrderApprove(string OrderList)
        //{

        //    string Message = "Successfully Approved ...";
        //    try
        //    {
        //        string[] Row = OrderList.Split('|');
        //        string[] Col;
        //        int ItemID = 0; decimal Qty = 0;
        //        sapuitd oSapu;
        //        gathdet oGath;
        //        DataTable dtOrd;
        //        foreach (string itm in Row)
        //        {
        //            Col = itm.Split(',');
        //            if (Col[0].ToString() != "")
        //            {
        //                ItemID = Convert.ToInt32(Col[1]);
        //                if (Col[2].ToString() != "") Qty = Convert.ToDecimal(Col[2]); else Qty = 0;
        //                //Jourmaster Ord = oSubmit.GetOrderApp(Col[0].ToString(), ItemID, Convert.ToInt32(Col[4]));

        //                dtOrd = oSubmit.GetData("select * from ordeMst a Inner Join ordeItd b on a.compcode = b.Compcode and  a.mstCode = b.itdCode  and a.MstType = b.itdType  where a.msttype = 174 and a.Compcode = " + Convert.ToInt32(Col[4]) + " and a.MstChNo ='" + Col[0].ToString() + "'  and b.ItdItem =  " + ItemID);
        //                int iMstCode = 0;
        //                Jourmaster Ord = new Jourmaster();
        //                oSubmit.BeginTran();

        //iMstCode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from Ordemst where compcode =" + dtOrd.Rows[0]["compcode"] + " and msttype =80" ));

        //                for (int i = 0; i < dtOrd.Rows.Count; i++)
        //                {
        //                    Ord = new Jourmaster();

        //                    Ord.msttype = 80;
        //                    Ord.mstcode = iMstCode;// Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from Ordemst where compcode =" + dtOrd.Rows[i]["compcode"] + " and msttype =80"));
        //                    Ord.mstdate = DateTime.Now.Date;
        //                    oSubmit.InsOrdeMst(Ord);

        //                    oSapu = new sapuitd();
        //                    oGath = new gathdet();

        //                    oSapu.compcode = Convert.ToInt16(dtOrd.Rows[i]["compcode"]);
        //                    oSapu.itdtype = Convert.ToInt32(Ord.msttype);
        //                    oSapu.itdcode = Convert.ToInt32(Ord.mstcode);
        //                 if (dtOrd.Rows[i]["msttime"].ToString()!="") oSapu.itdtime = Convert.ToInt32(dtOrd.Rows[i]["msttime"]);
        //                    oSapu.itdsrno = 1;
        //                    oGath.gathpuri = "";
        //                    oGath.gathstat = Convert.ToString(oSapu.itddate = Convert.ToDateTime(dtOrd.Rows[i]["mstdate"]));

        //                    oSapu.itditem = ItemID;
        //                    if (dtOrd.Rows[i]["itdactprc"].ToString() != "") oSapu.itdactprc = (decimal)dtOrd.Rows[i]["itdactprc"];  else oSapu.itdactprc  = 0;

        //                        if (dtOrd.Rows[i]["itdvate"].ToString() != "") oSapu.itdvate = (decimal)dtOrd.Rows[i]["itdvate"]; else oSapu.itdvate = 0;
        //                    if (dtOrd.Rows[i]["itdrate"].ToString() != "") oSapu.itdrate = (decimal)dtOrd.Rows[i]["itdrate"];  else oSapu.itdrate  = 0;

        //                  if (dtOrd.Rows[i]["itdvate"].ToString() != "")     oGath.gathwtdi = Convert.ToDecimal(dtOrd.Rows[i]["itdvate"]); else oGath.gathwtdi  = 0;


        //                    oSapu.itdamou = Convert.ToDecimal(Ord.itdrate * Qty);
        //                    oSapu.itdtowt = 0; oSapu.itdpkin = 0; oSapu.itdgodo = 1; oSapu.itdlswt = 0; oSapu.itdlabonw = 0; oSapu.itdvatinc = 0; oSapu.itdcasert = 0;

        //                    oSapu.itdpenq = oSapu.itdquan = Qty;

        //                    if (dtOrd.Rows[i]["itdmill"].ToString() != "") oSapu.itdmill = (int)dtOrd.Rows[i]["itdmill"];  else oSapu.itdmill  = 0;
        //                    oSapu.itdrema = Col[3];
        //                    //oSapu.itdtagno = Ord.itdtagno;

        //                    //if (ItemDet.LstItem[i].DisPer.ToString() != "") oSapu.itdperd = Convert.ToDecimal(ItemDet.LstItem[i].DisPer.ToString()); else oSapu.itdperd = 0;
        //                    //if (ItemDet.LstItem[i].DisAmt.ToString() != "") oSapu.itdqtyd = Convert.ToDecimal(ItemDet.LstItem[i].DisAmt.ToString()); else oSapu.itdqtyd = 0;

        //                    if (dtOrd.Rows[i]["itdperd"].ToString() != "") oSapu.itdperd = (decimal)dtOrd.Rows[i]["itdperd"];  else oSapu.itdperd  = 0;
        //                    if (dtOrd.Rows[i]["itdqtyd"].ToString() != "") oSapu.itdqtyd = (decimal)dtOrd.Rows[i]["itdqtyd"];  else oSapu.itdqtyd  = 0;
        //                    if (dtOrd.Rows[i]["itddscp"].ToString() != "") oSapu.itddscp = (decimal)dtOrd.Rows[i]["itddscp"];  else oSapu.itddscp  = 0;
        //                    if (dtOrd.Rows[i]["itddscp"].ToString() != "") oGath.gathqtdi = (decimal)dtOrd.Rows[i]["itddscp"]; else oGath.gathqtdi = 0;

        //                    if (dtOrd.Rows[i]["itdperd"].ToString() != "" && (decimal)dtOrd.Rows[i]["itdperd"] > 0) oGath.gathlabp = (double)dtOrd.Rows[i]["itdperd"]; else oGath.gathlabp = 0;


        //                    oSapu.itddime = dtOrd.Rows[i]["itddime"].ToString();

        //                    oGath.gathcode = oSapu.itdgath = oSubmit.GetUsWoCode();
        //                    oSapu.itdempo = SessionMaster.UserID;

        //                    //if (ItemDet.LstItem[i].ItemNetAmt.ToString() != "") oSapu.itdlabamt = Convert.ToDecimal(ItemDet.LstItem[i].ItemNetAmt.ToString()); else oSapu.itdlabamt = 0;
        //                    oGath.gathwast = dtOrd.Rows[i]["itdgstrtv"].ToString();

        //                    //oSapu.itdgstrtv = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer);
        //                    //oSapu.itdigstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt);

        //                    //if (oCls.GSTStateVal == "0")
        //                    //{

        //                    //    oSapu.itdcgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / 2;
        //                    //    oSapu.itdcgstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt) / 2;

        //                    //    oSapu.itdsgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / 2;
        //                    //    oSapu.itdsgstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt) / 2;

        //                    //    oSapu.itdigstrt = 0; oSapu.itdigstvl = 0;
        //                    //}
        //                    //else if (oCls.GSTStateVal == "1")
        //                    //{
        //                    //    oSapu.itdcgstrt = 0; oSapu.itdcgstvl = 0; oSapu.itdsgstrt = 0; oSapu.itdsgstvl = 0;
        //                    //    oSapu.itdigstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer);
        //                    //    oSapu.itdigstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt);
        //                    //}
        //                    oSapu.itdcessrt = Convert.ToDecimal(0); oSapu.itdcessvl = Convert.ToDecimal(0);
        //                    oSapu.itdugstrt = Convert.ToDecimal(0); oSapu.itdugstvl = Convert.ToDecimal(0);

        //                    oSubmit.insSapuItd(oSapu, "OrdeItd");
        //                    oSubmit.insGathDet(oGath);
        //                    iMstCode = iMstCode + 1;
        //                }
        //oSubmit.Commit(); 
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message = ex.Message;
        //        oSubmit.RollBack();
        //    }
        //    return Json(Message, JsonRequestBehavior.AllowGet);

        //}

        [HttpPost]
        public ActionResult SetOrderApprove(string OrderList)
        {

            string Message = "Successfully Approved ...";
            try
            {
                string[] Row = OrderList.Split('|');
                string[] Col;
                int ItemID = 0; decimal Qty = 0;
                sapuitd oSapu;
                gathdet oGath;
                DataTable dtOrd;
                commFunction oCom = new commFunction();
                foreach (string itm in Row)
                {
                    Col = itm.Split('^');
                    if (Col[0].ToString() != "")
                    {
                        ItemID = Convert.ToInt32(Col[1]);
                        if (Col[2].ToString() != "") Qty = Convert.ToDecimal(Col[2]); else Qty = 0;

                        dtOrd = oSubmit.GetData("select gath.gathlabp DisPer, b.* from ordeMst a Inner Join ordeItd b on a.compcode = b.Compcode and  a.mstCode = b.itdCode  and a.MstType = b.itdType left join Gathdet gath on b.itdGath = gath.Gathcode where a.msttype = 174 and a.Compcode = " + Convert.ToInt32(Col[4]) + " and a.MstChNo ='" + Col[0].ToString() + "'  and b.ItdItem =  " + ItemID, true);

                        for (int i = 0; i < dtOrd.Rows.Count; i++)
                        {
                            Jourmaster Ord = oSubmit.GetOrderApp(Col[0].ToString(), ItemID, Convert.ToInt32(Col[4]), Convert.ToInt32(dtOrd.Rows[i]["itdsrno"]));

                            oSubmit.BeginTran();
                            Ord.msttype = 80;
                            Ord.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from Ordemst where compcode =" + Ord.compcode + " and msttype =" + Ord.msttype));
                            Ord.mstdate = DateTime.Now.Date;
                            oSubmit.InsOrdeMst(Ord);

                            oSapu = new sapuitd();
                            oGath = new gathdet();

                            oSapu.compcode = Convert.ToInt16(Ord.compcode);
                            oSapu.itdtype = Convert.ToInt32(Ord.msttype);
                            oSapu.itdcode = Convert.ToInt32(Ord.mstcode);
                            oSapu.itdtime = Convert.ToInt32(Ord.msttime);
                            oSapu.itdsrno = i + 1;

                            oGath.gathstat = Convert.ToString(oSapu.itddate = Convert.ToDateTime(Ord.mstdate));

                            oSapu.itditem = ItemID;
                            oSapu.itdactprc = Ord.itdactprc;
                            oSapu.itdvate = Ord.itdvate;
                            oSapu.itdrate = Ord.itdrate;

                            oGath.gathwtdi = Convert.ToDecimal(oSapu.itdvate);

                            oSapu.itdamou = Convert.ToDecimal(Ord.itdrate * Qty);
                            oSapu.itdtowt = 0; oSapu.itdpkin = 0; oSapu.itdgodo = 1; oSapu.itdlswt = 0; oSapu.itdlabonw = 0; oSapu.itdvatinc = 0; oSapu.itdcasert = 0;

                            oSapu.itdpenq = oSapu.itdquan = Qty;

                            oSapu.itdmill = Ord.itdmill;
                            oSapu.itdrema = ""; //oSapu.itdrema = Col[3];
                            oGath.gathpuri = Col[3];

                            //oSapu.itdtagno = Ord.itdtagno;

                            //if (ItemDet.LstItem[i].DisPer.ToString() != "") oSapu.itdperd = Convert.ToDecimal(ItemDet.LstItem[i].DisPer.ToString()); else oSapu.itdperd = 0;
                            //if (ItemDet.LstItem[i].DisAmt.ToString() != "") oSapu.itdqtyd = Convert.ToDecimal(ItemDet.LstItem[i].DisAmt.ToString()); else oSapu.itdqtyd = 0;

                            oSapu.itdperd = Ord.DisAmt; //Ord.itdperd;
                            oSapu.itdqtyd = Ord.itdqtyd;
                            oSapu.itddscp = Ord.itddscp;
                            oGath.gathqtdi = (decimal)oSapu.itddscp;

                            oGath.gathlabp = (double)Ord.itdperd;


                            oSapu.itddime = Ord.itddime.ToString();

                            oGath.gathcode = oSapu.itdgath = oSubmit.GetUsWoCode();
                            oSapu.itdempo = Ord.itdempo;// SessionMaster.UserID;

                            //if (ItemDet.LstItem[i].ItemNetAmt.ToString() != "") oSapu.itdlabamt = Convert.ToDecimal(ItemDet.LstItem[i].ItemNetAmt.ToString()); else oSapu.itdlabamt = 0;
                            oGath.gathwast = Ord.itdgstrtv.ToString();

                            //oSapu.itdgstrtv = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer);
                            //oSapu.itdigstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt);

                            //if (oCls.GSTStateVal == "0")
                            //{

                            //    oSapu.itdcgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / 2;
                            //    oSapu.itdcgstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt) / 2;

                            //    oSapu.itdsgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / 2;
                            //    oSapu.itdsgstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt) / 2;

                            //    oSapu.itdigstrt = 0; oSapu.itdigstvl = 0;
                            //}
                            //else if (oCls.GSTStateVal == "1")
                            //{
                            //    oSapu.itdcgstrt = 0; oSapu.itdcgstvl = 0; oSapu.itdsgstrt = 0; oSapu.itdsgstvl = 0;
                            //    oSapu.itdigstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer);
                            //    oSapu.itdigstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt);
                            //}
                            oSapu.itdcessrt = Convert.ToDecimal(0); oSapu.itdcessvl = Convert.ToDecimal(0);
                            oSapu.itdugstrt = Convert.ToDecimal(0); oSapu.itdugstvl = Convert.ToDecimal(0);

                            oSubmit.insSapuItd(oSapu, "OrdeItd");
                            oSubmit.insGathDet(oGath);


                            Ord.StDate = Convert.ToDateTime(oCom.getOpenDate(DateTime.Now.Date));
                            Ord.LastDate = Convert.ToDateTime(oCom.getClosDate(DateTime.Now.Date));
                            oSubmit.UpdCodeGen(Ord);



                            oSubmit.Commit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                oSubmit.RollBack();
            }
            finally
            {
                oSubmit.CloseConnection();
            }


            return Json(Message, JsonRequestBehavior.AllowGet);

        }

        //[HttpPost]
        //public ActionResult SetPayFollowUp(string sPayFollowUp)
        //{
        //    string Message = "Successfully Approved ...";
        //    try
        //    {
        //        string[] Row = sPayFollowUp.Split('|');
        //        string[] Col;
        //        tblPayFollowUP oPay = new tblPayFollowUP();
        //        foreach (string itm in Row)
        //        {
        //            Col = itm.Split(',');
        //            if (Col[0].ToString() != "")
        //            {
        //                oPay = new tblPayFollowUP();

        //                oPay.BillID = 0;
        //                oPay.BillNo = "s";
        //                oPay.FDate = DateTime.Now.Date;
        //                if (Col[0].ToString() != "" && Convert.ToDecimal(Col[0]) > 0) oPay.CommitPay = Convert.ToDecimal(Col[0]);
        //                if (Col[1].ToString() != "") oPay.NextDt = Convert.ToDateTime(oSubmit.GetDateFormat(Col[1]));
        //                oPay.StatusID = Convert.ToInt32(Col[2]);
        //                oPay.Remark = Col[3];
        //                oPay.PartyID = Convert.ToInt32(Col[4]);
        //                oPay.UseCode = SessionMaster.UserID;

        //                DB.tblPayFollowUPs.InsertOnSubmit(oPay);
        //                DB.SubmitChanges();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message = ex.Message;
        //    }
        //    return Json(Message, JsonRequestBehavior.AllowGet);

        //}

        [HttpPost]
        public ActionResult SetPayFollowUp(int vouctype, int vouccode, string voucrefn, decimal CommitedPay, string PF_From, string PF_Rema, int CompCode, int acctcode)
        {
            string Message = "Successfully Approved ...";
            try
            {
                tblPayFollowUP oPay = new tblPayFollowUP();

                oPay.BillID = 0;
                oPay.BillNo = voucrefn;
                oPay.FDate = DateTime.Now.Date;
                if (Convert.ToDecimal(CommitedPay) > 0) oPay.CommitPay = Convert.ToDecimal(CommitedPay);
                if (PF_From.ToString() != "") oPay.NextDt = Convert.ToDateTime(oSubmit.GetDateFormat(PF_From.ToString()));
                oPay.StatusID = 0;
                oPay.Remark = PF_Rema;
                oPay.PartyID = acctcode;
                oPay.UseCode = SessionMaster.UserID;
                oPay.VoucType = vouctype;
                oPay.VoucCode = vouccode;
                oPay.CompCode = CompCode;

                DB.tblPayFollowUPs.InsertOnSubmit(oPay);
                DB.SubmitChanges();

                var objDealer = (from a in DB.tblPayFolloupPlannings where a.DealerID == Convert.ToInt32(acctcode) where a.PlanningDT <= DateTime.Now.Date select new { DealerID = a.DealerID ?? 0 }).FirstOrDefault();

                if (objDealer != null && objDealer.DealerID > 0)
                {
                    //tblPayFolloupPlanning oPlan = (from a in DB.tblPayFolloupPlannings where a.IsUse == 0 where a.DealerID == acctcode where a.PlanningDT <= DateTime.Now.Date select a).FirstOrDefault();
                    var oPlan = (from a in DB.tblPayFolloupPlannings where a.IsUse == 0 where a.DealerID == acctcode where a.PlanningDT <= DateTime.Now.Date select a).ToList();

                    oPlan.ForEach(a => { a.IsUse = 1; });
                    //oPlan.IsUse = 1; 
                    DB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return Json(Message, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetPartyBal(int iItemID, int iType, int iMstcode, string iDate)
        {
            DateTime trndate = new DateTime();
            DataTable dt = new DataTable();
            int comp = Convert.ToInt32(SessionMaster.CompCode);
            clsSubmitModel oSubmit = new clsSubmitModel();
            account pmst = new account();
            if (iType == 59)
            {
                trndate = Convert.ToDateTime("1900-04-01");
            }
            else
            {
                trndate = Convert.ToDateTime(oSubmit.GetDateFormat(iDate));
            }
            dt = oSubmit.GetData("select isnull(sum(trndram-trncram),0) from jourtrn a inner join jourmst b on b.compcode = a.compcode and  b.mstcode = a.trncode  and b.mstdate = a.trndate  and b.msttype = a.trntype  where a.compcode = '" + comp + "' and trnitem = '" + iItemID + "'  and( (datediff(d, trndate, '" + trndate + "') = 0 and trncode <" + iMstcode + "            and trntype ='" + iType + "' ) or (datediff(d, trndate, '" + trndate + "') = 0 and trntype <>'" + iType + "') or ((datediff(d, '" + trndate + "', trndate) <  0)))", true);
            var Bal = Convert.ToDecimal(dt.Rows[0][0]);

            return Json(Bal, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DispatchOrd(string BrandID, int DealerID, int StateID, int SubCateID, string From_Date = "", string To_Date = "", int Executive = 0, int CompanyID = 0, int DispatchStatus = 1)
        {
            List<Jourmaster> lstOrder = new List<Jourmaster>();

            if (DispatchStatus == 1)
            {
                lstOrder = oSubmit.GetDispatchOrd(BrandID, DealerID, StateID, SubCateID, From_Date, To_Date, Executive, CompanyID);
                ViewBag.DispatchStatus = 1;
            }
            else
            {
                lstOrder = oSubmit.GetPendingDispatch(BrandID, DealerID, StateID, SubCateID, From_Date, To_Date, Executive, CompanyID);
                ViewBag.DispatchStatus = 2;
            }

            return PartialView("_DispatchOrdList", lstOrder);
        }


        [HttpPost]
        public ActionResult UndoOrder(int Compcode, int mstcode, int ItemCode)
        {
            string sMsg = "Invoice Generated , You Can't Delete It !!!";
            try
            {
                if ((from a in DB.refetabs where a.compcode == Compcode where a.ms2type == 80 where a.ms2code == mstcode where a.refitem == ItemCode select a).Count() > 0)
                { }
                else
                {
                    if ((from a in DB.ordeitds where a.compcode == Compcode where a.itdtype == 80 where a.itdcode == mstcode select a).Count() == 1)
                    {
                        var Mst = (from a in DB.ordemsts where a.compcode == Compcode where a.msttype == 80 where a.mstcode == mstcode select a).FirstOrDefault();
                        DB.ordemsts.DeleteOnSubmit(Mst);
                    }

                    var Itd = (from a in DB.ordeitds where a.compcode == Compcode where a.itdtype == 80 where a.itdcode == mstcode where a.itditem == ItemCode select a).FirstOrDefault();
                    DB.ordeitds.DeleteOnSubmit(Itd);

                    DB.SubmitChanges();
                    sMsg = "Delete Successfully ...";
                }
            }
            catch (Exception ex)
            {
                sMsg = ex.ToString();
            }
            return Json(sMsg, JsonRequestBehavior.AllowGet);
        }


        public ActionResult chkOrdAppr(int Compcode, string Mstchno)
        {
            var ITEMS = new { OrdCount = 0 };
            try
            {
                DataTable dt;
                clsSubmitModel oSubmit = new clsSubmitModel();
                dt = oSubmit.GetData("select Mstchno from ordeMst where CompCode =" + Compcode + " and Msttype = 80 and mstchno ='" + Mstchno + "'", true);

                return Json(new { OrdCount = dt.Rows.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ITEMS = new { OrdCount = 0 };
            }
            return Json(ITEMS, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrdFollowUp(int DealerID, int StateID, int Executive, int CityID, string From = "", string To = "")
        {
            List<clsOrderFollowUp> Data = oSubmit.GetOrdFollowUp(DealerID, StateID, Executive, CityID, From, To);
            return PartialView("OrdFollowUp", Data);
        }

        [HttpPost]
        public ActionResult OrdFollowUp(int DealerID, string NextDt, string Remark, int Status)
        {
            string Message = "Saved";
            try
            {
                tblOrdFollowUp oTbl = new tblOrdFollowUp();

                oTbl.DealerID = DealerID;
                oTbl.FDate = DateTime.Now;
                if (NextDt != "") oTbl.NextDt = oSubmit.GetDateFormat(NextDt);
                oTbl.Remark = Remark;
                oTbl.UseCode = SessionMaster.UserID;
                oTbl.Status = Status;

                DB.tblOrdFollowUps.InsertOnSubmit(oTbl);
                DB.SubmitChanges();

            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return Json(Message, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetOrdFollowUpList(int DealerID, int StateID, int Executive, int CityID, string From = "", string To = "")
        {
            List<clsOrderFollowUp> Data = oSubmit.GetOrdFollowUpList(DealerID, StateID, Executive, CityID, From, To);
            return PartialView("OrdFollowUpList", Data);
        }

        public ActionResult PayFollowUpHis(int trnledg, int compcode)
        {
            //var data = from a in DB.tblPayFollowUPs where a.VoucCode == vouccode  where a.VoucType ==vouctype where a.CompCode == compcode (tblPayFollowUP) select new {  }  ;
            List<tblPayFollowUP> data = (from a in DB.tblPayFollowUPs where a.PartyID == trnledg where a.CompCode == compcode select a).ToList();

            return PartialView("_PayFollowUpHis", data);
        }

        [HttpPost]
        public ActionResult SetPFReceipt(int DealerID, decimal Amount, int ModeID, int BankID, string Remark, string ChqDate, int TypeID, string ChqNo, int CompCode, decimal AccessAmt, decimal DemandAmt)
        {
            string Message = "Saved";
            try
            {
                tblPF_Receipt oTbl = new tblPF_Receipt();

                oTbl.MstDate = (DateTime)DateTime.Now;
                oTbl.DealerID = DealerID;
                oTbl.Amount = Amount;
                oTbl.ModeID = ModeID;
                oTbl.BankID = BankID;
                oTbl.TypeID = TypeID;
                oTbl.ChqNo = ChqNo;
                if (ChqDate != "") oTbl.ChqDate = oSubmit.GetDateFormat(ChqDate);
                oTbl.Remark = Remark;
                oTbl.CompCode = CompCode;
                oTbl.UserID = SessionMaster.UserID;
                oTbl.CreatedOn = DateTime.Now;
                oTbl.AccessAmt = AccessAmt;
                oTbl.DemandAmt = DemandAmt;
                DB.tblPF_Receipts.InsertOnSubmit(oTbl);
                DB.SubmitChanges();

            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return Json(Message, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetPF_Receipt(int DealerID)
        {
            var PF_Receipt = from a in DB.tblPF_Receipts
                             join b in DB.accounts on new { _Type = 24, _Code = (int)a.BankID } equals new { _Type = (int)b.acctgrou, _Code = b.acctcode } into c
                             from d in c.DefaultIfEmpty()
                             where a.DealerID == DealerID
                             select new clsPF_Receipt
                             {
                                 Bank = d.acctname,
                                 ChqNo = a.ChqNo,
                                 Remark = a.Remark,
                                 ID = a.ID,
                                 DealerID = (int)a.DealerID,
                                 UserID = (int)a.UserID,
                                 ModeID = (int)a.ModeID,
                                 BankID = (int)a.BankID,
                                 TypeID = (int)a.TypeID,
                                 MstDate = (DateTime)a.MstDate,
                                 CreatedOn = (DateTime)a.CreatedOn,
                                 sChqDate = String.Format("{0:dd/MM/yyyy}", a.ChqDate.ToString()),
                                 sMstDate = String.Format("{0:dd/MM/yyyy}", a.MstDate.ToString()),
                                 Amount = (decimal)a.Amount,
                                 //DemandAmt = (decimal)a.DemandAmt,   
                                 AccessAmt = (decimal)a.AccessAmt,
                                 sMode = (a.ModeID.Equals("1") ? "Cash" : "Bank").ToString(),
                                 sType = (a.TypeID.Equals("1") ? "Cheque" : a.TypeID.Equals("2") ? "RTGS" : a.TypeID.Equals("3") ? "NEFT" : a.TypeID.Equals("4") ? "Self" : ""),
                                 sStatus = (a.StatusID.Equals("1") ? "Verified" : a.StatusID.Equals("2") ? "Not Verified" : "Under Process")
                             };

            //qty =  (
            //                       items.ItemPromoFlag.Equals("1") ? "100000" :
            //                       items.ItemCat1.Equals("1") ? "100000" :
            //                       items.ItemSaleStatus.Equals("O") ? "0" :
            //                       (items.ItemQtyOnHand - items.ItemQtyCommitted).ToString
            //                    )

            //      public string sChqDate { get; set; }
            //public string sMstDate { get; set; }
            //public string sMode { get; set; }
            //public string sType  { get; set; }


            return Json(PF_Receipt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PFDueList(int CompCode, string AcctList = "")
        {
            List<DueList> lstOrder = oSubmit.sp_GetDueList(CompCode, AcctList, DateTime.Now.Date.ToString("dd-MM-yyyy"), 0, 0, false);
            return PartialView("PayFollowUp_Det", lstOrder);
        }

        public ActionResult GetBank_Receipt(int DealerID, string From, string TO, int StatusID)
        {
            if (From == "") From = "01/01/1900";
            if (TO == "") TO = "01/01/2070";

            List<clsPF_Receipt> PF_Receipt = (from a in DB.tblPF_Receipts
                                              join acc in DB.accounts on new { _Type = 21, _Code = (int)a.DealerID } equals new { _Type = (int)acc.acctgrou, _Code = acc.acctcode }
                                              join b in DB.accounts on new { _Type = 24, _Code = (int)a.BankID } equals new { _Type = (int)b.acctgrou, _Code = b.acctcode } into c
                                              from d in c.DefaultIfEmpty()
                                              where (a.StatusID ?? 0) == StatusID
                                              where a.ChqDate.Value.Date >= oSubmit.GetDateFormat(From)
                                              where a.ChqDate.Value.Date <= oSubmit.GetDateFormat(TO)
                                              where (DealerID > 0 ? a.DealerID == DealerID : true)
                                              select new clsPF_Receipt
                                              {
                                                  acctname = acc.acctname,
                                                  Bank = d.acctname,
                                                  ChqNo = a.ChqNo,
                                                  Remark = a.Remark,
                                                  ID = a.ID,
                                                  DealerID = (int)a.DealerID,
                                                  UserID = (int)a.UserID,
                                                  ModeID = (int)a.ModeID,
                                                  BankID = (int)a.BankID,
                                                  TypeID = (int)a.TypeID,
                                                  MstDate = (DateTime)a.MstDate,
                                                  CreatedOn = (DateTime)a.CreatedOn,
                                                  sChqDate = String.Format("{0:dd/MM/yyyy}", a.ChqDate.ToString()),
                                                  sMstDate = String.Format("{0:dd/MM/yyyy}", a.MstDate.ToString()),
                                                  ChqDate = (DateTime)a.ChqDate,
                                                  AccessAmt = a.AccessAmt ?? 0,
                                                  Amount = (decimal)a.Amount,
                                                  sMode = (a.ModeID.Equals("1") ? "Cash" : "Bank").ToString(),
                                                  sType = (a.TypeID.Equals("1") ? "Cheque" : a.TypeID.Equals("2") ? "RTGS" : a.TypeID.Equals("3") ? "NEFT" : a.TypeID.Equals("4") ? "Self" : ""),
                                                  sStatus = (a.StatusID.Equals("1") ? "Verified" : a.StatusID.Equals("2") ? "Not Verified" : "Under Process")
                                              }).ToList();

            return PartialView("_BankReceipt", PF_Receipt);
        }

        public ActionResult SetPFVerified(int DealerID, int ID, int IsVerified)
        {
            string sMsg = "Update";
            try
            {
                var Data = (from a in DB.tblPF_Receipts where a.ID == ID where a.DealerID == DealerID select a).SingleOrDefault();
                Data.StatusID = IsVerified;
                DB.SubmitChanges();

            }
            catch (Exception ex)
            {
                sMsg = ex.ToString();
            }

            return Json(sMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDealerJSon(int ExecID = 0)
        {
            if (ExecID == 0)
            {
                var Dealer = (from a in DB.accounts where a.acctgrou == 21 where a.compcode == SessionMaster.CompCode orderby a.acctname select new { Name = a.acctname, Code = a.acctcode }).Distinct().ToList();
                return Json(Dealer, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Dealer = (from a in DB.tblDistributors join acc in DB.accounts on a.MstCode equals acc.AcctDeal join b in DB.EmpAllotMsts on a.MstCode equals b.DealerID join c in DB.employees on b.EmpID equals c.empcode join d in DB.citydets on new { _Type = 15, _Code = (int)acc.acctcity } equals new { _Type = d.cityType, _Code = d.citycode } where a.DistributorID != 0 where (ExecID > 0 ? c.UseCode == ExecID : true) where acc.acctgrou == 21 orderby a.DisName select new { Name = a.DisName + " - " + d.cityname, Code = acc.acctcode }).Distinct().ToList();
                return Json(Dealer, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult delDealer(int MstCode)
        {
            string s = "";
            try
            {
                s = "Dealer is in Use !";

                int iAcctCode = Convert.ToInt32(oSubmit.GetSingleData("Select AcctCode from Account where acctgrou = 21 and acctDeal = " + MstCode, "0", true));

                if (iAcctCode > 0)
                {
                    if ((from a in DB.ordemsts where a.mstcust == iAcctCode select a).Count() > 0)
                    { }
                    else
                    {
                        var Dealer = (from a in DB.tblDistributors where a.MstCode == MstCode select a).FirstOrDefault();
                        DB.tblDistributors.DeleteOnSubmit(Dealer);
                        var Acc = (from a in DB.accounts where a.AcctDeal == MstCode select a).FirstOrDefault();
                        DB.tblDistributors.DeleteOnSubmit(Dealer);

                        DB.SubmitChanges();
                        s = "Delete Successfully !";
                    }
                }

                return Json(s, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete_RC(int ID)
        {
            string s = "";
            try
            {
                s = "Error Occurred !";
                if (ID > 0)
                {
                    var Dealer = (from a in DB.tblPF_Receipts where a.ID == ID select a).FirstOrDefault();
                    DB.tblPF_Receipts.DeleteOnSubmit(Dealer);

                    DB.SubmitChanges();
                    s = "Delete Successfully !";

                    return Json(s, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getOrderValue(int CompCode, int DealerID)
        {
            var OrderValue = from a in DB.getOrderValue(CompCode) where a.compcode == CompCode where a.mstcust == DealerID orderby a.mstdate select new { mstdate = String.Format("{0:dd/MM/yyyy}", a.mstdate), OrdValue = a.OrdValue, mstchno = a.mstchno };

            return Json(OrderValue, JsonRequestBehavior.AllowGet);
        }
        /*start 181018 %temp%*/
        public ActionResult GetExeWiseOrder(string aCode="0")
        {
            if (SessionMaster.UserType > 0)
                ViewBag.Executive = new SelectList(from res in DB.employees orderby res.empname select new { res.empcode, res.empname }, "empcode", "empname", SessionMaster.UserID);
            else
                ViewBag.Executive = new SelectList(from res in DB.employees orderby res.empname select new { res.empcode, res.empname }, "empcode", "empname");
            ViewBag.aCode = aCode;
            clsFilter sm1 = new clsFilter();
            return View(sm1);
        }
        public ActionResult GetExeWiseOrderData(string aFrom_Date, string aTo_Date, string aEmpCode)/*181020*/
        {
            var sMsg = new { Message = "Successfully  ...", MsgID = 1 };
            clsSubmitModel oCon = new clsSubmitModel();
            commFunction comm = new commFunction();
            string sSearch = "";
            if (aFrom_Date != "") sSearch += "@pFromDt='" + comm.GetDateFormat(aFrom_Date) + "',";
            if (aTo_Date != "") sSearch += "@pToDt='" + comm.GetDateFormat(aTo_Date) + "',";
            if (aEmpCode != "") sSearch += "@pEmpCode='" + aEmpCode + "',";
            if (sSearch.Contains(',')) sSearch = sSearch.Substring(0, sSearch.LastIndexOf(','));

            var CustDet = oCon.DataTableToJSON1(oCon.GetData("sp_GetExeWiseOrder " + sSearch, true));
            return new JsonResult()
            {
                Data = new { CustDet },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        /*end 181018 %temp%*/
        public ActionResult GetExeWiseOrderCompany()
        {
            var sMsg = new { Message = "Successfully  ...", MsgID = 1 };
            clsSubmitModel oCon = new clsSubmitModel();
            var CustDet = oCon.DataTableToJSON1(oCon.GetData("select compname as aName,compcode as aCode from company", true));
            return new JsonResult()
            {
                Data = new { CustDet },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [HttpPost]
        public ActionResult PlanningPayFollowup( )
        { 
            int CompCode = 0; string AcctList = ""; int State = 0; int City = 0; int Due = 0; int Commit = 0; int Planning = 0;
            List<DueList> lstOrder = oSubmit.GetDueListEmp(CompCode, AcctList, State, City, true, Due, Commit, Planning, SessionMaster.UserID, 1);

            return PartialView("PlanningPayFollowup", lstOrder);
        }
        [HttpPost]
        public ActionResult OverDueList()
        {
             
            int CompCode = 0; string AcctList = ""; int State = 0; int City = 0; int Due = 0; int Commit = 0; int Planning = 0;
            List<DueList> lstOrder = oSubmit.GetDueListEmp(CompCode, AcctList, State, City, true, Due, Commit, Planning, SessionMaster.UserID, 2);

            return PartialView("_OverDueList", lstOrder);
        }
 [HttpGet]
        public ActionResult PayFollowUpHisAll()
        { 
            List<clsFollow> lstOrder = (from a in DB.tblPayFollowUPs join b in DB.accounts on a.PartyID equals b.acctcode where b.acctgrou == 21 where a.UseCode == SessionMaster.UserID where a.FDate == DateTime.Now.Date select new clsFollow { F_Date = a.FDate, MstDate = a.NextDt, Status = b.acctname, CommitPay = a.CommitPay, Remark = a.Remark }).ToList();

            //return PartialView("_PayFollowUpHisAll", lstOrder);
            return View("_PayFollowUpHisAll", lstOrder);
        }
		public ActionResult ComplainStatus()//181031
        {
            DateTime dtFrom = Convert.ToDateTime("2018-04-01");
            DateTime dtTo = DateTime.Now;
            ViewBag.Executive = new SelectList(
                from T_Compl in DB.tblComplaints
                where T_Compl.StatusID == 1 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) && T_Compl.EmpID == SessionMaster.UserID
                orderby T_Compl.CompNo
                select new { T_Compl.CompID, T_Compl.CompNo }, "CompID", "CompNo", SessionMaster.UserID
                                               );
            return View("ComplainStatus");
        }
        public ActionResult UpdComplaintStatus(tblComplaint oTbl)//181101
        {
            string Trace = "";
            var sMsg = new { Message = "Successfully Update ...", MsgID = 1 };
            try
            {
                var Comp = (from a in DB.tblComplaints where a.CompID == oTbl.CompID select a).SingleOrDefault();
                if (Comp != null)
                {
                    Comp.StatusID = oTbl.StatusID;
                    Comp.StatusRemark = oTbl.cmStatusRemark;
                    Comp.cmStatusDt= DateTime.Now;

                    DB.SubmitChanges();
                    sMsg = new { Message = "Complaint Status Updated ...", MsgID = 1 };
                }
            }
            catch (Exception ex)
            {
                sMsg = new { Message = Trace + ex.Message, MsgID = 2 };
            }

            return new JsonResult()
            {
                Data = new { sMsg },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
    }
}