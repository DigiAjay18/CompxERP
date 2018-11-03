using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using CompxERP.Models;

using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace CompxERP.Controllers
{
    public class ServiceController : Controller
    {
        //
        // GET: /Service/

        ERPDataContext db = new ERPDataContext();
        clsSubmitModel oSubmit = new clsSubmitModel();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SrvCycle()
        {
            ViewBag.vbService = new SelectList(from a in db.studdets where a.studType == 873 select new { a.studCode, a.studName }, "studCode", "studName");

            return PartialView();
        }

        [HttpPost]
        public ActionResult SaveSrvCycle(int SrvTypeID, int SrvNo, string SrvCycleDt)
        {
            ERPDataContext oTran = new ERPDataContext();
            oTran.Connection.Open();
            System.Data.Common.DbTransaction transaction = oTran.Connection.BeginTransaction();
            string Message = "Successfully Saved ...";
            try
            {
                string[] Row = SrvCycleDt.Split('^');

                tblSrvCycleMst oSrvM;
                tblSrvCycleItd oSrvI;
                commFunction oCom = new commFunction();

                oSrvM = new tblSrvCycleMst();
                oSrvM.SrvNo = SrvNo;
                oSrvM.SrvTypeID = SrvTypeID;
                db.tblSrvCycleMsts.InsertOnSubmit(oSrvM);
                db.SubmitChanges();

                foreach (string itm in Row)
                {
                    if (itm.ToString() != "")
                    {
                        oSrvI = new tblSrvCycleItd();
                        oSrvI.SrvNo = SrvNo;
                        oSrvI.ItdCode = oSrvM.MstCode;
                        oSrvI.SrvDate = Convert.ToDateTime(itm);
                        db.tblSrvCycleItds.InsertOnSubmit(oSrvI);
                        db.SubmitChanges();
                    }
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                //   transaction.Connection.Close();
                Message = ex.Message;
            }
            //finally
            //{
            //  //  transaction.Connection.Close();
            //} 

            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSrvCycle(int TypeID)
        {
            var Data = from a in db.tblSrvCycleMsts join b in db.tblSrvCycleItds on a.MstCode equals b.ItdCode where a.SrvTypeID == TypeID where a.MstCode == db.tblSrvCycleMsts.Where(aa => aa.SrvTypeID == TypeID).Max(aa => (int?)aa.MstCode) select new { a.MstCode, a.SrvNo, sSrvDate = String.Format("{0:dd/MM/yyyy}", b.SrvDate.ToString()), srNo = b.SrvNo };

            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Complaint()
        {

            string CompNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");
            ViewBag.CompNo = CompNo + GetNo(oSubmit.GetSingleData("Select isnull(max(CompID)+1,1) from tblComplaint", "0", true).ToString());

            ViewBag.Executive = new SelectList(from res in db.employees orderby res.empname select new { res.UseCode, res.empname }, "UseCode", "empname");
            //ViewBag.SubCategory = new SelectList(from a in db.itgroups where a.itgpunde != 0 where a.compcode == SessionMaster.CompCode orderby a.itgpname select new { a.itgpname, a.itgpcode }, "itgpcode", "itgpname");
            ViewBag.SubCategory = new SelectList(from a in db.itgroups where a.itgpunde == 0 where a.compcode == SessionMaster.CompCode orderby a.itgpname select new { a.itgpname, a.itgpcode }, "itgpcode", "itgpname");

            ViewBag.vwState = new SelectList(from a in db.citydets where a.cityType == 67 && a.cityrute == 3 orderby a.cityname select new { a.citycode, a.cityname }, "citycode", "cityname");
            ViewBag.vbService = new SelectList(from a in db.studdets where a.studType == 873 select new { a.studCode, a.studName }, "studCode", "studName");

            return PartialView("Complaint");
        }
        [HttpGet]
        public ActionResult Complaint1()
        {

            string CompNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");
            ViewBag.CompNo = CompNo + GetNo(oSubmit.GetSingleData("Select isnull(max(CompID)+1,1) from tblComplaint", "0", true).ToString());

            ViewBag.Executive = new SelectList(from res in db.employees orderby res.empname select new { res.UseCode, res.empname }, "UseCode", "empname");
            //ViewBag.SubCategory = new SelectList(from a in db.itgroups where a.itgpunde != 0 where a.compcode == SessionMaster.CompCode orderby a.itgpname select new { a.itgpname, a.itgpcode }, "itgpcode", "itgpname");
            ViewBag.SubCategory = new SelectList(from a in db.itgroups where a.itgpunde == 0 where a.compcode == SessionMaster.CompCode orderby a.itgpname select new { a.itgpname, a.itgpcode }, "itgpcode", "itgpname");

            ViewBag.vwState = new SelectList(from a in db.citydets where a.cityType == 67 && a.cityrute == 3 orderby a.cityname select new { a.citycode, a.cityname }, "citycode", "cityname");
            ViewBag.vbService = new SelectList(from a in db.studdets where a.studType == 873 select new { a.studCode, a.studName }, "studCode", "studName");

            return PartialView("Complaint1");
        }
        [HttpGet]
        public ActionResult getComplaintNo()
        {
            string CompNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");

            var ComplaintNo = new { CompNo = CompNo + GetNo(oSubmit.GetSingleData("Select isnull(max(CompNo)+1,1) from tblComplaint", "0", true).ToString()) };

            return Json(Json(ComplaintNo).Data, JsonRequestBehavior.AllowGet);
        }

        public string GetNo(string ChNo)
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


        //public ActionResult Index(User ObjUserData)
        //{
        //    using (var context = new MultiFileDbContext())
        //    {
        //        using (DbContextTransaction dbTran = context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                User pd = new User()
        //                {
        //                    EmplCode = ObjUserData.EmplCode,
        //                    EmplName = ObjUserData.EmplName
        //                };
        //                context.ObjUserData.Add(pd);
        //                context.SaveChanges();

        //                var id = pd.RefUserID;

        //                var usrFile = ObjUserData.objUsrEducation;
        //                if (usrFile != null)
        //                {
        //                    foreach (var item in usrFile)
        //                    {
        //                        item.RefUserID = id;
        //                        context.objUserEducation.Add(item);
        //                    }
        //                }
        //                context.SaveChanges();
        //                dbTran.Commit();
        //            }
        //            catch (DbEntityValidationException ex)
        //            {
        //                dbTran.Rollback();
        //                throw;
        //            }
        //        }
        //    }
        //    return View();
        //}  

        [HttpPost]
        public ActionResult Complaint(clsComplaint oTbl)
        {
            string sLine = "";
            var sMsg = new { Message = "Successfully Saved ...", MsgID = 1, ComplaintNo = "0" };
            try
            {
                string CompNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");
                int iCustID = 0;
                tblComplaint oComp = new tblComplaint();

                oComp.CustID = oTbl.CustID;
                if (oTbl.IsNewCust == 1)
                {
                    iCustID = Convert.ToInt32(oSubmit.GetSingleData("Select isnull(max(studCode)+1,1) from studdet where studType = 68", "0", true));
                    studdet Cust = new studdet();
                    Cust.studType = 68;
                    Cust.studCode = iCustID;
                    Cust.studName = oTbl.studName;
                    Cust.studadd1 = oTbl.studadd1;
                    Cust.studadd2 = oTbl.studadd2;
                    Cust.studcity = oTbl.studcity;
                    Cust.studstat = oTbl.studstat;
                    Cust.studphon = oTbl.studphon;

                    db.studdets.InsertOnSubmit(Cust);
                    db.SubmitChanges();

                    oComp.CustID = iCustID;
                }

                sLine = "1";

                oComp.CompNo = CompNo + GetNo(oSubmit.GetSingleData("Select isnull(max(CompID)+1,1) from tblComplaint", "0", true).ToString());
                //oComp.CompDt = oSubmit.GetDateFormat(oTbl.CompDt.ToString());
                sLine = "2";
                oComp.CreatedOn = DateTime.Now;
                oComp.StatusID = 1;
                oComp.UserID = SessionMaster.UserID;
                sLine = "3";
                if (oTbl.sTentetiveTm != null && oTbl.sTentetiveTm.ToString() != "") oComp.TentetiveTm = oSubmit.GetDate(oTbl.sTentetiveTm.ToString());

                sLine = "4";
                sLine += oTbl.CompDt.ToString();

                oComp.CompDt = oSubmit.GetDate(oTbl.sCompDt.ToString());
                sLine = "5";
                oComp.DealerID = oTbl.DealerID;

                oComp.CustNM = oTbl.CustNM;
                oComp.ModelNo = oTbl.ModelNo;
                oComp.SrvType = oTbl.SrvType;
                oComp.EmpID = oTbl.EmpID;
                oComp.ItemID = oTbl.ItemID;
                oComp.InvNo = oTbl.InvNo;
                oComp.Charge = oTbl.Charge;
                oComp.Remark_Cust = oTbl.Remark_Cust;
                oComp.Remark_Mgr = oTbl.Remark_Eng;//Remark_Eng to Remark_Mgr 181101

                oComp.cmIsRead = 1;//181027
                oComp.cmIsPaid = oTbl.IsPaid;//181027
                oComp.cmSrvcMode = oTbl.IsSrvcMode;//181027
                oComp.cmCategory = oTbl.Category;//181027

                db.tblComplaints.InsertOnSubmit(oComp);
                db.SubmitChanges();

                CompNo = CompNo + GetNo(oSubmit.GetSingleData("Select isnull(max(CompID)+1,1) from tblComplaint", "0", true).ToString());

                sMsg = new { Message = "Complaint Successfully Registered . Your Complaint No Is " + CompNo + " .", MsgID = 1, ComplaintNo = CompNo };

            }
            catch (Exception ex)
            {
                sMsg = new { Message = sLine + "#" + ex.Message, MsgID = 2, ComplaintNo = "0" };
            }

            return Json(sMsg, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpGet]
        public ActionResult getComplent(int CustID = 0, int DealID = 0, string From_Date = "", string To_Date = "", int StatusID = 0, string aRpType = "", int IsRead = 2)//181031
        {
            ViewBag.RpType = aRpType;
            int EmpID = 0;
            if (SessionMaster.UserType == 68) CustID = SessionMaster.UserID;
            else if (SessionMaster.UserType == 7) EmpID = SessionMaster.UserID;
            else if (SessionMaster.UserType == 4)
            {
                DealID = (from a in db.accounts where a.AcctUser == SessionMaster.UserID select a.acctcode).SingleOrDefault();
            }

            if (From_Date == "") From_Date = "01/01/1900";
            if (To_Date == "") To_Date = "01/01/2070";
            var dTax = (from a in db.tblComplaints
                        //join b in db.employees on a.EmpID equals b.UseCode into c
                        //from d in c.DefaultIfEmpty()
                        where (CustID > 0 ? a.CustID == CustID : true)
                        where (DealID > 0 ? a.DealerID == DealID : true)
                        where (StatusID > 0 ? a.StatusID == StatusID : true)
                        //where (StatusID == 1 ? a.CustNM == "Done" : StatusID == 2 ? a.CustNM == "Pen" : StatusID == 3 ? a.CustNM == "Done" : true)
                        where (aRpType == "AC" ? a.EmpID > 0 : aRpType == "UC" ? (a.EmpID ?? 0) == 0 : EmpID > 0 ? a.EmpID == EmpID : true)//where (EmpID > 0 ? a.EmpID == EmpID : true)          
                        where (IsRead == 0 || IsRead == 1 ? a.cmIsRead == IsRead : true)
                        where a.CompDt >= oSubmit.GetDateFormat(From_Date)
                        where (aRpType == "OC" ? (DateTime)a.CompDt < Convert.ToDateTime(oSubmit.GetDateFormat(To_Date).ToString("MM/01/yyyy")) : a.CompDt <= oSubmit.GetDateFormat(To_Date))
                        select a);
            //select new {   a.CompNo,   a.CompDt,   a.CustNM,   a.Remark_Cust,   a.Remark_Eng,  a.Charge,   a.StatusID });

            //switch (aRpType), ItemID = d.empname
            //{
            //    case "PC":
            //        case "CR":
            //        case "RC":
            //        case "HC":
            //        case "CL":
            //        dTax = (from a in db.tblComplaints
            //                join b in db.employees on a.EmpID equals b.UseCode into c
            //                from d in c.DefaultIfEmpty()
            //                where (CustID > 0 ? a.CustID == CustID : true)
            //                where (DealID > 0 ? a.DealerID == DealID : true)
            //                where (StatusID > 0 ? a.StatusID == StatusID : true)
            //                where (EmpID > 0 ? a.EmpID == EmpID : true)
            //                where a.CompDt >= oSubmit.GetDateFormat(From_Date)
            //                where a.CompDt <= oSubmit.GetDateFormat(To_Date)
            //                select a);
            //        break;
            //    case "NC":
            //        dTax = (from a in db.tblComplaints
            //                join b in db.employees on a.EmpID equals b.UseCode into c
            //                from d in c.DefaultIfEmpty()
            //                where (CustID > 0 ? a.CustID == CustID : true)
            //                where (DealID > 0 ? a.DealerID == DealID : true)
            //                where (StatusID > 0 ? a.StatusID == StatusID : true)
            //                where (EmpID > 0 ? a.EmpID == EmpID : true)
            //                where (a.cmIsRead == 1)
            //                where a.CompDt >= oSubmit.GetDateFormat(From_Date)
            //                where a.CompDt <= oSubmit.GetDateFormat(To_Date)
            //                select a);
            //        break;
            //    case "AC":
            //        dTax = (from a in db.tblComplaints
            //                join b in db.employees on a.EmpID equals b.UseCode into c
            //                from d in c.DefaultIfEmpty()
            //                where (CustID > 0 ? a.CustID == CustID : true)
            //                where (DealID > 0 ? a.DealerID == DealID : true)
            //                where (StatusID > 0 ? a.StatusID == StatusID : true)
            //                where (a.EmpID != null)
            //                where a.CompDt >= oSubmit.GetDateFormat(From_Date)
            //                where a.CompDt <= oSubmit.GetDateFormat(To_Date)
            //                select a);
            //        break;
            //    case "UC":
            //        dTax = (from a in db.tblComplaints
            //                join b in db.employees on a.EmpID equals b.UseCode into c
            //                from d in c.DefaultIfEmpty()
            //                where (CustID > 0 ? a.CustID == CustID : true)
            //                where (DealID > 0 ? a.DealerID == DealID : true)
            //                where (StatusID > 0 ? a.StatusID == StatusID : true)
            //                where (a.EmpID == null)
            //                where a.CompDt >= oSubmit.GetDateFormat(From_Date)
            //                where a.CompDt <= oSubmit.GetDateFormat(To_Date)
            //                select a);
            //        break;
            //    case "OC":
            //        dTax = (from a in db.tblComplaints
            //                join b in db.employees on a.EmpID equals b.UseCode into c
            //                from d in c.DefaultIfEmpty()
            //                where (CustID > 0 ? a.CustID == CustID : true)
            //                where (DealID > 0 ? a.DealerID == DealID : true)
            //                where (StatusID > 0 ? a.StatusID == StatusID : true)
            //                where (EmpID > 0 ? a.EmpID == EmpID : true)
            //                where (DateTime)a.CompDt <=Convert.ToDateTime(oSubmit.GetDateFormat(To_Date).ToString("MM/01/yyyy"))
            //                select a);
            //        break;
            //    case "TC":
            //        dTax = (from a in db.tblComplaints
            //                join b in db.employees on a.EmpID equals b.UseCode into c
            //                from d in c.DefaultIfEmpty()
            //                where (CustID > 0 ? a.CustID == CustID : true)
            //                where (DealID > 0 ? a.DealerID == DealID : true)
            //                where (StatusID > 0 ? a.StatusID == StatusID : true)
            //                where (EmpID > 0 ? a.EmpID == EmpID : true)
            //                where a.CompDt >= oSubmit.GetDateFormat(From_Date)
            //                where a.CompDt <= oSubmit.GetDateFormat(To_Date)
            //                select a);
            //        break;
            //}


            //var dTax = (from a in db.tblComplaints
            //            join b in db.accounts on a.DealerID equals b.acctcode into c
            //            from d in c.DefaultIfEmpty()
            //            where (CustID > 0 ? a.CustID == CustID : true)
            //            where (DealID > 0 ? a.DealerID == DealID : true)
            //            where a.StatusID == 1
            //            where a.CompDt >= oSubmit.GetDateFormat(From_Date)
            //            where a.CompDt <= oSubmit.GetDateFormat(To_Date)
            //            select new clsComplaint() { CustNM = d.acctname, CompNo = a.CompNo, Remark_Cust =a.Remark_Cust , Remark_Eng = a.Remark_Eng });

            return PartialView("ComplaintList", dTax);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetDealer(string sName, bool IsDealer)
        {

            var Dealer = from a in db.accounts where a.acctcode == 100 select new { acctname = a.acctname, acctCode = a.acctcode };

            if (IsDealer == true)
                Dealer = from a in db.accounts join b in db.citydets on new { _Type = 15, _Code = (int)a.acctcity } equals new { _Type = b.cityType, _Code = b.citycode } into d from e in d.DefaultIfEmpty() where a.acctgrou == 21 where a.AcctDeal > 0 where a.acctname.Contains(sName) select new { acctname = a.acctname + " - ", acctCode = a.acctcode };
            else
                Dealer = from a in db.studdets join b in db.citydets on new { _Type = 15, _Code = (int)a.studcity } equals new { _Type = b.cityType, _Code = b.citycode } into d from e in d.DefaultIfEmpty() where a.studType == 68 where a.studName.Contains(sName) select new { acctname = a.studName + " - ", acctCode = a.studCode };

            return Json(Dealer, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SchemeMaster()
        {
            ViewBag.ItemNm = new SelectList(from res in db.itemains orderby res.itemname select new { res.itemcode, res.itemname }, "itemcode", "itemname");

            ViewBag.Brand = new SelectList(from a in db.studdets where a.studType == 61 select new { a.studName, a.studCode }, "studCode", "studName");

            ViewBag.SubCategory = new SelectList(from a in db.itgroups where a.itgpunde != 0 where a.compcode == SessionMaster.CompCode orderby a.itgpname select new { a.itgpname, a.itgpcode }, "itgpcode", "itgpname");

            string sMstChNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");

            ViewBag.MaxSchm = sMstChNo + GetVoucherNo(oSubmit.GetData("select isnull(max(MstCode)+1,1) as maxno from tblSchemeMst", true).ToString());

            return PartialView("SchemeMaster");
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
        public ActionResult SchemeMaster(clsSchemeMst oSch)
        {

            string sLine = "";
            var sMsg = new { Message = "Successfully Saved ...", MsgID = 1, ComplaintNo = "0" };
            try
            {
                string CompNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");
                tblSchemeMst oMst = new tblSchemeMst();

                oMst.MstCode = Convert.ToInt32(oSubmit.GetSingleData("Select isnull(max(MstCode)+1,1) from tblSchemeMst", "0", true));

                oMst.MstDate = oSubmit.GetDate(oSch.sMstDate.ToString());
                oMst.SchNo = CompNo + GetNo(oMst.MstCode.ToString());
                oMst.SchName = oSch.SchName;
                oMst.ClaimFrom = oSubmit.GetDate(oSch.sMstDate.ToString());
                oMst.ClaimTo = oSubmit.GetDate(oSch.sMstDate.ToString());

                if (oSch.SchFrom.ToString() != "") oMst.SchFrom = oSubmit.GetDate(oSch.SchFrom.ToString());
                if (oSch.SchTo.ToString() != "") oMst.SchTO = oSubmit.GetDate(oSch.SchTo.ToString());

                oMst.CreatedOn = DateTime.Now;

                db.tblSchemeMsts.InsertOnSubmit(oMst);
                db.SubmitChanges();

                tblSchemeItd oItd;
                var json = oSch.SchemeItm;
                clsPoItem ItemDet = JsonConvert.DeserializeObject<clsPoItem>(json);

                for (int i = 0; i < ItemDet.LstItem.Count; i++)
                {
                    oItd = new tblSchemeItd();

                    oItd.ItdCode = oMst.MstCode;
                    oItd.ItemID = ItemDet.LstItem[i].ItemID;
                    oItd.BrandID = ItemDet.LstItem[i].itdmill;
                    oItd.Qty = Convert.ToInt32(ItemDet.LstItem[i].Qty);
                    oItd.itdFrom = oSubmit.GetDate(ItemDet.LstItem[i].sFrom.ToString());
                    oItd.itdTo = oSubmit.GetDate(ItemDet.LstItem[i].sTo.ToString());
                    oItd.Offer = ItemDet.LstItem[i].Remark;

                    db.tblSchemeItds.InsertOnSubmit(oItd);
                    db.SubmitChanges();

                }


                CompNo = CompNo + GetNo(oSubmit.GetSingleData("Select isnull(max(CompID)+1,1) from tblComplaint", "0", true).ToString());

                sMsg = new { Message = "Successfully Saved .", MsgID = 1, ComplaintNo = CompNo };

            }
            catch (Exception ex)
            {
                sMsg = new { Message = sLine + "#" + ex.Message, MsgID = 2, ComplaintNo = "0" };
            }

            return Json(sMsg, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FloatScheme()
        {


            ViewBag.SchemeNm = new SelectList(from a in db.tblSchemeMsts orderby a.MstDate select new { a.MstCode, a.SchName }, "MstCode", "SchName");
            //ViewBag.Employee = new SelectList(from a in db.employees orderby a.empname select new { a.empcode, a.empname }, "empcode", "empname");

            if (SessionMaster.UserType > 0)
                ViewBag.Employee = new SelectList(from res in db.employees orderby res.empname select new { res.UseCode, res.empname }, "UseCode", "empname", SessionMaster.UserID);
            else
                ViewBag.Employee = new SelectList(from res in db.employees orderby res.empname select new { res.UseCode, res.empname }, "UseCode", "empname");

            return PartialView("FloatScheme");
        }


        [HttpPost]
        public ActionResult FloatScheme(int iScheme, int iEmployee, string sDealer)
        {

            string sLine = "";
            var sMsg = new { Message = "Successfully Saved ...", MsgID = 1 };
            try
            {
                tblFloatScheme oFloat;
                string[] Row = sDealer.Split(',');
                tblOrderApprove oOrder = new tblOrderApprove();

                foreach (string itm in Row)
                {
                    oFloat = new tblFloatScheme();
                    oFloat.EmpID = iEmployee;
                    oFloat.SchemeID = iScheme;
                    oFloat.DealID = Convert.ToInt32(itm);
                    oFloat.CreatedOn = DateTime.Now;

                    db.tblFloatSchemes.InsertOnSubmit(oFloat);
                    db.SubmitChanges();
                }

                sMsg = new { Message = "Successfully Saved", MsgID = 1 };

            }
            catch (Exception ex)
            {
                sMsg = new { Message = sLine + "#" + ex.Message, MsgID = 2 };
            }

            return Json(sMsg, JsonRequestBehavior.AllowGet);

        }

        public ActionResult OfferAccept()
        {
            ViewBag.vwDealerNm = new SelectList(from a in db.accounts orderby a.acctname where a.acctgrou == 21 select new { a.acctcode, a.acctname }, "acctcode", "acctname");

            // ViewBag.vbSchemeList = (from a in db.tblSchemeMsts orderby a.SchName select new { a.MstCode, a.SchName }).AsEnumerable();
            ViewBag.vbSchemeList = (from a in db.tblSchemeMsts orderby a.SchName select a).AsEnumerable();

            return PartialView("OfferAccept");
        }

        [HttpPost]
        public ActionResult OfferAccept(int iDealer, string sScheme)
        {

            string sLine = "";
            var sMsg = new { Message = "Successfully Saved ...", MsgID = 1 };
            try
            {
                tblOfferAccept oFloat;
                string[] Row = sScheme.Split(',');

                foreach (string itm in Row)
                {
                    oFloat = new tblOfferAccept();
                    oFloat.DealerID = iDealer;
                    oFloat.SchemeID = Convert.ToInt32(itm);
                    oFloat.CreatedOn = DateTime.Now;

                    db.tblOfferAccepts.InsertOnSubmit(oFloat);
                    db.SubmitChanges();
                }

                sMsg = new { Message = "Successfully Saved", MsgID = 1 };

            }
            catch (Exception ex)
            {
                sMsg = new { Message = sLine + "#" + ex.Message, MsgID = 2 };
            }

            return Json(sMsg, JsonRequestBehavior.AllowGet);

        }

        /*Start 181012 */
        [System.Web.Http.HttpGet]
        public ActionResult VisitScheduleMst()
        {
            //try
            //{
            //DDLController oDDL = new DDLController();
            clsVisitSchMst oProp = new clsVisitSchMst();
            //oProp.lstSchEmp = oDDL.getMasterMenu();
            //oProp.lstPartyName = oDDL.getMasterMenu();
            //oProp.lstArea = oDDL.getMasterMenu();
            //    //oProp.mencode = iGroup;
            //    //oProp.menuser = iUser;
            //    //oProp.menview = View;
            //    //oProp.menaddi = Add;
            //    //oProp.menedit = Edit;
            //    //oProp.mendele = Del;
            //    //oProp.menacce = Acc;
            //    //
            //    //oSubmit.InsRights(oProp);

            //    var sMsg = new { Message = "Successfully Saved ...", MsgID = 1, ComplaintNo = "0" };
            //    return Json(sMsg, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    return Json(ex.Message, JsonRequestBehavior.AllowGet);
            //}
            return View(oProp);
        }
        [HttpPost]
        public ActionResult VisitScheduleMst(string txtDate, string vsEmpID, string txtSchDate, string sTrnDet)//(clsVisitSchMst oCls)
        {
            clsVisitSchTrn cTrn = new clsVisitSchTrn();
            clsVisitSchMst oCls = new clsVisitSchMst();
            oCls.vsCompCode = SessionMaster.CompCode;
            oCls.vsDate = txtDate;
            oCls.vsSchDate = txtSchDate;
            oCls.vsEmpID = vsEmpID;

            List<clsVisitSchMst> aData = oSubmit.InsVisitSchMst(oCls);

            if (sTrnDet != "")
            {
                var json = sTrnDet;// oCls.sTrnDet;
                clsVisitSchTrn TrnDet = JsonConvert.DeserializeObject<clsVisitSchTrn>(json);
                for (int i = 0; i < TrnDet.LstItem.Count; i++)
                {
                    cTrn.vtCompCode = oCls.vsCompCode;
                    cTrn.vtCode = TrnDet.LstItem[i].vtCode;
                    cTrn.vsID = aData[0].aCode;
                    //cTrn.vtTypeName = TrnDet.LstItem[i].vtTypeName;
                    cTrn.vtPartyName = TrnDet.LstItem[i].vtPartyName;
                    cTrn.vtArea = TrnDet.LstItem[i].vtArea;
                    cTrn.vtMobile = TrnDet.LstItem[i].vtMobile;
                    //cTrn.vtAddress = TrnDet.LstItem[0].vtAddress;
                    //cTrn.vtModelNo = TrnDet.LstItem[0].vtModelNo;
                    //cTrn.vtMachineNo = TrnDet.LstItem[0].vtMachineNo;
                    //cTrn.vtWeightName = TrnDet.LstItem[0].vtWeightName;
                    //cTrn.vtValidFor = TrnDet.LstItem[0].vtValidFor;
                    //cTrn.vtMachineType = TrnDet.LstItem[0].vtMachineType;
                    //cTrn.vtDueYear = TrnDet.LstItem[0].vtDueYear;
                    //cTrn.vtDueMonths = TrnDet.LstItem[0].vtDueMonths;
                    //cTrn.vtValidUpToDate = TrnDet.LstItem[0].vtValidUpToDate;
                    //cTrn.vtVcType = TrnDet.LstItem[0].vtVcType;
                    oSubmit.UpdVisitSchTrn(cTrn);
                }
            }

            return RedirectToAction("VisitScheduleMst", new { aType = oCls.aType, aMsg = oCls.aMsg, MenCode = Request.QueryString["MenCode"] });
        }
        public JsonResult GetVisitPartyData(string vtPartyName)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            var model1 = oSubmit.GetVisitPartyData("exec sp_GetVisitPartyData @vtCompCode =" + SessionMaster.CompCode + ",@vtPartyName='" + vtPartyName + "'");
            return Json(model1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetVisitPartyList()
        {
            clsVisitSchMst frm = new clsVisitSchMst();
            DDLController oDDL = new DDLController();
            frm.lstPartyName = oDDL.GetVisitPartyList();
            return Json(frm, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetVisitPartyAreaList()
        {
            clsVisitSchMst frm = new clsVisitSchMst();
            DDLController oDDL = new DDLController();
            frm.lstArea = oDDL.GetVisitPartyAreaList();
            return Json(frm, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetVisitSchForList()
        {
            clsVisitSchMst frm = new clsVisitSchMst();
            DDLController oDDL = new DDLController();
            frm.lstSchEmp = oDDL.GetVisitSchForList();
            return Json(frm, JsonRequestBehavior.AllowGet);
        }
        [System.Web.Http.HttpGet]
        public ActionResult DailyVisitEntry()
        {
            clsDailyVisitEntry prop = new clsDailyVisitEntry();
            return View(prop);
        }
        [System.Web.Http.HttpGet]
        public ActionResult InwardEntry()
        {
            clsVisitSchMst oProp = new clsVisitSchMst();
            return View(oProp);
        }
        public JsonResult GetVisitPartyMobData(string vtPartyName, string vtMobile)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            var model1 = oSubmit.GetVisitPartyData("exec sp_GetVisitPartyMobData @vtCompCode =" + SessionMaster.CompCode + ",@vtPartyName='" + vtPartyName + "',@vtMobile='" + vtMobile + "'");
            return Json(model1, JsonRequestBehavior.AllowGet);
        }
        /*End 181012 */
        /*Start 181016 */
        public ActionResult GetDailyVisitPartyList()
        {
            clsDailyVisitEntry frm = new clsDailyVisitEntry();
            DDLController oDDL = new DDLController();
            frm.lstSchEmp = oDDL.GetDailyVisitPartyList();
            return Json(frm, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DailyVisitEntry(string dvPartyName, string dvVisitDetail, string dvEstCost, string dvNextFollowUp, string dvRemark)//(clsVisitSchMst oCls)
        {
            clsDailyVisitEntry oCls = new clsDailyVisitEntry();
            oCls.dvCompCode = SessionMaster.CompCode;
            oCls.dvPartyName = dvPartyName.Substring(0, dvPartyName.LastIndexOf('-'));
            oCls.dvMobile = dvPartyName.Substring(dvPartyName.LastIndexOf('-') + 1, dvPartyName.Length - dvPartyName.LastIndexOf('-') - 1);
            oCls.dvVisitDetail = dvVisitDetail;
            if (dvEstCost != "") oCls.dvEstCost = decimal.Parse(dvEstCost);
            if (dvNextFollowUp != "") oCls.dvNextFollowUp = Convert.ToDateTime(dvNextFollowUp);
            oCls.dvRemark = dvRemark;

            List<clsDailyVisitEntry> aData = oSubmit.InsDailyVisitEntry(oCls);

            return RedirectToAction("DailyVisitEntry", new { aType = aData[0].aType, aMsg = aData[0].aMsg, MenCode = Request.QueryString["MenCode"] });
        }
        /*End 181016*/
        /*Start 181017 %temp%*/
        [System.Web.Http.HttpGet]
        public ActionResult DailyVisitReport()
        {
            clsDailyVisitEntry prop = new clsDailyVisitEntry();
            return View(prop);
        }
        [HttpPost]
        public ActionResult DailyVisitReport(string dvPartyName, string dvVisitDetail, string dvEstCost, string dvQty, string dvNextFollowUp, string dvRemark)
        {
            clsDailyVisitEntry oCls = new clsDailyVisitEntry();
            oCls.dvCompCode = SessionMaster.CompCode;
            oCls.dvPartyName = dvPartyName.Substring(0, dvPartyName.LastIndexOf('-'));
            oCls.dvMobile = dvPartyName.Substring(dvPartyName.LastIndexOf('-') + 1, dvPartyName.Length - dvPartyName.LastIndexOf('-') - 1);
            oCls.dvVisitDetail = dvVisitDetail;
            if (dvQty != "") oCls.dvQty = int.Parse(dvQty);
            if (dvEstCost != "") oCls.dvEstCost = decimal.Parse(dvEstCost);
            if (dvNextFollowUp != "") oCls.dvNextFollowUp = Convert.ToDateTime(dvNextFollowUp);
            oCls.dvRemark = dvRemark;

            List<clsDailyVisitEntry> aData = oSubmit.InsDailyVisitReport(oCls);

            return RedirectToAction("DailyVisitReport", new { aType = aData[0].aType, aMsg = aData[0].aMsg, MenCode = Request.QueryString["MenCode"] });
        }
        [System.Web.Http.HttpGet]
        public ActionResult VisitScheduleList()
        {
            clsDailyVisitEntry prop = new clsDailyVisitEntry();
            return View(prop);
        }
        /*End 181017*/

        public ActionResult PayFollowUpPlaning()
        {

            //var DealNM = (from a in db.accounts where a.acctgrou == 21 where a.acctcode >1000 select new clsEmpAllotMst() { DealerNm = a.acctname, DealerID = a.acctcode }).ToList();

            var DealNM = (from a in db.accounts join b in (from c in db.tblPayFolloupPlannings where c.IsUse == 0 where c.PlanningDT <= DateTime.Now group c by c.DealerID into g select new { DealerID = g.Key ?? 0 }) on a.acctcode equals b.DealerID into d from e in d.DefaultIfEmpty() where a.acctgrou == 21 where a.acctcode > 1000 select new clsEmpAllotMst() { DealerNm = a.acctname, DealerID = a.acctcode, StatusID = (int)e.DealerID }).ToList();


            return View("PayFollowUpPlaning", DealNM);
        }
        [HttpPost]
        public ActionResult PayFollowUpPlaning(string sDealer, string PlanningDt)
        {
            string sLine = "";
            var sMsg = new { Message = "Successfully Saved ...", MsgID = 1 };
            try
            {
                string[] Row = sDealer.Split(',');
                tblPayFolloupPlanning oPlannig;
                int iDealer = 0;
                foreach (string itm in Row)
                {
                    if (itm.ToString() != "")
                    {
                        var objDealer = (from a in db.tblPayFolloupPlannings where a.IsUse == 0 where a.DealerID == Convert.ToInt32(itm) where a.PlanningDT == oSubmit.GetDateFormat(PlanningDt) select new { DealerID = a.DealerID ?? 0 }).FirstOrDefault();

                        if (objDealer == null)
                        {
                            oPlannig = new tblPayFolloupPlanning();
                            oPlannig.DealerID = Convert.ToInt32(itm);
                            oPlannig.PlanningDT = oSubmit.GetDateFormat(PlanningDt);
                            oPlannig.CreatedOn = DateTime.Now;
                            oPlannig.ExecID = SessionMaster.UserID;
                            oPlannig.IsUse = 0;

                            db.tblPayFolloupPlannings.InsertOnSubmit(oPlannig);
                            db.SubmitChanges();
                        }
                    }
                }
                sMsg = new { Message = "Successfully Saved", MsgID = 1 };
            }
            catch (Exception ex)
            {
                sMsg = new { Message = sLine + "#" + ex.Message, MsgID = 2 };
            }
            return Json(sMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PaymentPlanning(string PayPlanningDate)
        {

            var DealNM = (from a in db.tblPayFolloupPlannings join b in db.accounts on new { _Type = 21, _Code = (int)a.DealerID } equals new { _Type = (int)b.acctgrou, _Code = b.acctcode } where b.acctgrou == 21 where a.PlanningDT == oSubmit.GetDateFormat(PayPlanningDate) select new clsEmpAllotMst() { DealerNm = b.acctname, DealerID = (int)a.DealerID, sPlanDate = String.Format("{0:dd/MM/yyyy}", a.PlanningDT.ToString()), StatusID = a.IsUse }).ToList();

            return View("_PaymentPlanning", DealNM);
        }

        public ActionResult DelPlanning(int DealerID, string mstDate)
        {
            string sMsg = "Delete Successfully ...";
            try
            {
                var d = Convert.ToDateTime(mstDate);

                var Mst = (from a in db.tblPayFolloupPlannings where a.DealerID == DealerID where a.PlanningDT == Convert.ToDateTime(mstDate) select a).FirstOrDefault();
                db.tblPayFolloupPlannings.DeleteOnSubmit(Mst);

                db.SubmitChanges();
                sMsg = "Delete Successfully ...";
            }
            catch (Exception ex)
            {
                sMsg = ex.ToString();
            }

            return Json(sMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemIssue()
        {

            ViewBag.Executive = new SelectList(from res in db.employees orderby res.empname select new { res.UseCode, res.empname }, "UseCode", "empname");
            ViewBag.SubCategory = new SelectList(from a in db.itgroups where a.itgpunde != 0 where a.compcode == SessionMaster.CompCode orderby a.itgpname select new { a.itgpname, a.itgpcode }, "itgpcode", "itgpname");
            ViewBag.Brand = new SelectList(from a in db.studdets where a.studType == 61 select new { a.studName, a.studCode }, "studCode", "studName");

            return View("tblItemIssue");
        }

        public ActionResult ItemIssue(tblItemIssue oTbl)
        {
            var sMsg = new { Message = "Successfully Saved ...", MsgID = 1 };
            try
            {
                db.tblItemIssues.InsertOnSubmit(oTbl);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                sMsg = new { Message = ex.Message, MsgID = 2 };
            }
            return Json(sMsg, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ComplaintAllot(int ComplID)/*181031*/
        {
            ViewBag.ComplID = ComplID;

            var Comp = (from a in db.tblComplaints where a.CompID == ComplID select a).SingleOrDefault();
            if (Comp != null)
            {
                Comp.cmIsRead = 0;
                db.SubmitChanges();
            }

            if (SessionMaster.UserType == 0 || SessionMaster.UserType == 1)
                ViewBag.Executive = new SelectList(from res in db.employees
                                                   join x in db.loginusers on new {_code=(int)res.UseCode} equals new {_code=x.usecode} into y from z in y.DefaultIfEmpty()
                                                   where z.usetype==7
                                                   orderby res.empname select new { res.UseCode, res.empname }, "UseCode", "empname", SessionMaster.UserID);
            else
                ViewBag.Executive = new SelectList(from res in db.employees
                                                   join x in db.loginusers on new { _code = (int)res.UseCode } equals new { _code = x.usecode } into y
                                                   from z in y.DefaultIfEmpty()
                                                   where z.usetype == 7 && z.UseHeadID==SessionMaster.UserID
                                                   orderby res.empname
                                                   select new { res.UseCode, res.empname }, "UseCode", "empname");
            return PartialView("ComplaintAllot");
        }
        public ActionResult GetComplaintDetail(int ComplID)/*181031*/
        {
            var sMsg = (from a in db.tblComplaints
                        join b in db.itemains on new { _Code = Convert.ToInt32(a.ItemID) } equals new { _Code = b.itemcode } into d from e in d.DefaultIfEmpty()
                        join c in db.jourmsts on new { _Code = a.InvNo } equals new { _Code = c.mstchno } into f
                        from g in f.DefaultIfEmpty()
                        where a.CompID == ComplID
                        select new
                        {
                            InvDt =string.Format("{0:dd/MM/yyyy}", g.mstdate),
                            CompDt =string.Format("{0:dd/MM/yyyy}", a.CompDt),
                            CompID= a.CompID,
                            CompNo = a.CompNo,
                            DealerID = a.DealerID,
                            CustID = a.CustID,
                            CustNM = a.CustNM,
                            ModelNo = a.ModelNo,
                            SrvType = a.SrvType,
                            EmpID = a.EmpID,
                            ItemID = a.ItemID,
                            InvNo = a.InvNo,
                            TentetiveTm = a.TentetiveTm,
                            Charge = a.Charge,
                            Remark_Cust = a.Remark_Cust,
                            Remark_Eng = a.Remark_Eng,
                            StatusID = a.StatusID,
                            CreatedOn = a.CreatedOn,
                            UserID = a.UserID,
                            cmCategory = a.cmCategory,
                            cmIsPaid = a.cmIsPaid,
                            cmSrvcMode = a.cmSrvcMode,
                            cmIsRead = a.cmIsRead,
                            cmStatusRemark = a.cmStatusRemark,
                            cmStatusDt = a.cmStatusDt,
                            StatusRemark = a.StatusRemark,
                            Remark_Mgr = a.Remark_Mgr,
                            WarrantyLeft=string.Format("{0:0}",(g.mstdate-DateTime.Now).TotalDays),
                Mobile="",
                ItemNm=e.itemname,
                        });
            return Json(sMsg, JsonRequestBehavior.AllowGet);
        }
        [System.Web.Http.HttpPost]
        public ActionResult ComplaintAllotToEmp(tblComplaint oTbl)//181031
        {
            string Trace = "";
            var sMsg = new { Message = "Successfully Update ...", MsgID = 1 };
            try
            {
                var Comp = (from a in db.tblComplaints where a.CompID == oTbl.CompID select a).SingleOrDefault();
                if (Comp != null)
                {
                    Comp.EmpID = oTbl.EmpID;
                    Comp.cmIsPaid = oTbl.cmIsPaid;
                    Comp.cmSrvcMode = oTbl.cmSrvcMode;
                    Comp.TentetiveTm = oTbl.TentetiveTm;
                    Comp.Charge = oTbl.Charge;
                    Comp.Remark_Mgr = oTbl.Remark_Eng;

                    db.SubmitChanges();
                    sMsg = new { Message = "Complaint Alloted ...", MsgID = 1 };
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

        public ActionResult TrackComplaint()
        {
            return View();
        }
        public ActionResult CompalintRegister()
        {
            return View();
        }
        public ActionResult ManagerWsEmpComplainList()
        {
            return View();
        }

        public ActionResult Survey()
        {
            return View("Survey");
        }
        public ActionResult Survey_InwardEntry()
        {
            return View("Survey_InwardEntry");
        }


    }
}
