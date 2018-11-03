using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;

using CompxERP.Models;
using CompxERP.Filters;


using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.Reporting.WebForms;

namespace CompxERP.Controllers
{
[UserSessionfilter]
    public class ReportController : Controller
    {
        
        //
        // GET: /Report/
        ERPDataContext DB = new ERPDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CallSummary()
        {
            ViewBag.UserType = SessionMaster.UserType;
            ViewBag.UserCode= SessionMaster.UserID;
            return View(); 
        }
        public JsonResult GetCallSummaryData(string From ,string To , int UseType ,int UseCode )
        {
            clsSubmitModel oSubmit =new clsSubmitModel();

            var CallData = from a in DB.getCallSummary(oSubmit.GetDateFormat(From), oSubmit.GetDateFormat(To), UseType, UseCode) select a ; 
            return Json(CallData,JsonRequestBehavior.AllowGet);
        }

        [HttpGet] 
        public ActionResult Ledger_(string L_From ="",string L_To="" ,string DealerNm ="")
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            if (L_From != "" && L_To != "")
            {
                ViewBag.vwLedger = (from a in DB.tblLedgers where (DealerNm !="" ? a.HeadDes == DealerNm : true) where a.VchDt.Value.Date >= oSubmit.GetDateFormat(L_From) where a.VchDt.Value.Date <= oSubmit.GetDateFormat(L_To) orderby a.HeadDes select a).ToList();
            }
            else
            {
                ViewBag.vwLedger = (from a in DB.tblLedgers where (DealerNm !="" ? a.HeadDes == DealerNm :true) select a).ToList();
            }
            return View("Ledger"); 
        }

        public ActionResult Ledger()
        {
            clsLedger oModel = new clsLedger();
            List<clsLedger> oList1 = new List<clsLedger>();

            oModel.LstItem = oList1;
            return View(oModel);
        }

        //[HttpGet]
        //public ActionResult Edit(int mstCode = 0, int MstType = 0)
        //{
        //    switch (MstType)
        //    {
        //        case 42:
        //            return RedirectToAction("Create", "PuSL", new { MstType = MstType, MstCode = mstCode, comp = SessionMaster.CompCode });
        //        case 43:
        //            return RedirectToAction("Create", "PuSL", new { MstType = MstType, MstCode = mstCode, comp = SessionMaster.CompCode });
        //        case 5:
        //            return RedirectToAction("Create", "Voucher", new { MstType = MstType, MstCode = mstCode, comp = SessionMaster.CompCode });
        //        case 3:
        //            return RedirectToAction("Create", "Voucher", new { MstType = MstType, MstCode = mstCode, comp = SessionMaster.CompCode });
        //    }

        //    return View();
        //}

        [HttpPost]
        public ActionResult Ledger(clsLedger oCls, string Submit)
        {
            clsLedger oModel = new clsLedger();
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = new DataTable();
            int comp = Convert.ToInt32(SessionMaster.CompCode);
            if (comp == 0)
            {
                TempData["message"] = "Please Select Company ...";
            }

            switch (Submit)
            {
                case "Search":

                    //string sFilter = "";
                    //string sInnerFilter = "";

                    //if (oCls.FromDt != "" && oCls.ToDt != "")
                    //    sInnerFilter = "@BdDateFrom ='" +  oSubmit.GetDateFormat(oCls.FromDt) + "', @BdDateTo ='" + oSubmit.GetDateFormat(oCls.ToDt) + "' ,";
                    //else
                    //    sInnerFilter = "'2017-04-01', '2017-09-23' ,";

                    //if (oCls.PartyID > 0)
                    //    sFilter = " @PartyID =" + oCls.PartyID;

                    //dt = oSubmit.GetData("select * from udf_GetTrialBalLedger(" + sInnerFilter + SessionMaster.CompCode + ", 60, '2017-09-23')  where 1=1 " + sFilter);

                    dt = oSubmit.GetData("Exec sp_GetLedger @CompList = '" + SessionMaster.CompCode + "', @AcctList = " + oCls.PartyID + ", @BdDateFrom = '" + oSubmit.GetDateFormat(oCls.FromDt) + "', @BdDateTo = '" + oSubmit.GetDateFormat(oCls.ToDt) + "'");

                    List<clsLedger> oList1 = new List<clsLedger>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        oModel = new Models.clsLedger();
                        //oModel.grouname = dt.Rows[i]["groupnum"].ToString();
                        oModel.acctcode = Convert.ToInt32(dt.Rows[i]["trncode"].ToString());
                        oModel.acctname = dt.Rows[i]["ledger"].ToString();
                        //oModel.cityname = dt.Rows[i]["cityname"].ToString();
                        // oModel.opbl = Convert.ToDecimal(dt.Rows[i]["opbl"].ToString());
                        if (Convert.ToDecimal(dt.Rows[i]["Dr"].ToString()) > 0) oModel.dr = Convert.ToDecimal(dt.Rows[i]["Dr"]); else oModel.dr = 0;
                        if (Convert.ToDecimal(dt.Rows[i]["Cr"].ToString()) > 0) oModel.cr = Convert.ToDecimal(dt.Rows[i]["Cr"]); else oModel.cr = 0;

                        oModel.crdrbl = Convert.ToDecimal(dt.Rows[i]["Bal"].ToString());
                        //oModel.grourepo = dt.Rows[i]["grourepo"].ToString();
                        oModel.mstdate = Convert.ToDateTime(dt.Rows[i]["VcDt"].ToString());
                        oModel.mstcode = Convert.ToInt32(dt.Rows[i]["trncode"].ToString());
                        oModel.msttype = Convert.ToInt32(dt.Rows[i]["trntype"].ToString());



                        oModel.sType = dt.Rows[i]["VcType"].ToString();
                        oModel.VcNO = dt.Rows[i]["VcNo"].ToString();
                        oModel.Particular = dt.Rows[i]["OnAccountName"].ToString();

                        oModel.trnsrno = Convert.ToInt32(dt.Rows[i]["trnitem"].ToString());
                        oModel.trnrema = dt.Rows[i]["DrCr"].ToString();
                        //oModel.mstrema = dt.Rows[i]["mstrema"].ToString();
                        oList1.Add(oModel);
                    }
                    oModel.LstItem = oList1;
                    break;
                case "Print":

                    break;
            }
            return View(oModel);
        }

        public ActionResult Print(string FromDt, string ToDt, int PartyID = 0)//170826
        {
            clsSubmitModel oSubmit = new clsSubmitModel();

            Session["Ledger"] = oSubmit.GetData("Exec sp_GetLedger @CompList = '" + SessionMaster.CompCode + "', @AcctList = " + PartyID + ", @BdDateFrom = '" + oSubmit.GetDateFormat(FromDt) + "', @BdDateTo = '" + oSubmit.GetDateFormat(ToDt) + "'");

            return View("~/Views/Shared/Report.aspx");
        }

        public ActionResult TrialBal(int CompCode, string From, string To, int DealerCode = 0, int StateID = 0, int Executive = 0)
        {
            clsLedger oModel = new clsLedger();
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = new DataTable();
            int comp = Convert.ToInt32(SessionMaster.CompCode);
            if (comp == 0)
            {
                TempData["message"] = "Please Select Company ...";
            }
            string sDealer = ""; string sState  = "";
            if (DealerCode > 0) sDealer =  "and acctcode = "+ DealerCode ;
            if (StateID > 0) sState = "and acctstat = " + StateID;

            if (Executive > 0)
            {
                dt = oSubmit.GetData("select a.* from  [udf_GetTrialBalQuick]('" + oSubmit.GetDateFormat(From) + "', '" + oSubmit.GetDateFormat(To) + "',  " + CompCode + " , 0)  a Left join tblDistributor b on a.acctDeal = b.mstCode Left join EmpAllotMst c on b.mstCode = c.DealerID where groucode = 21 and empID = (select empcode from Employee where usecode = " + Executive + ") " + sDealer + sState + " order by grouposi , grouname ,acctname "); 
            }
            else
            {
                dt = oSubmit.GetData("select * from [udf_GetTrialBalQuick]('" + oSubmit.GetDateFormat(From) + "', '" + oSubmit.GetDateFormat(To) + "', " + CompCode + " , 0) where groucode = 21 " + sDealer + sState + " order by grouposi , grouname ,acctname ");
            }

            decimal dDR = 0;
            decimal dCR = 0;


            List<clsLedger> oList1 = new List<clsLedger>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                oModel = new Models.clsLedger();
                oModel.Compcode = Convert.ToInt32(dt.Rows[i]["Compcode"].ToString());
                oModel.acctcode = Convert.ToInt32(dt.Rows[i]["acctcode"].ToString());
                oModel.grouname = dt.Rows[i]["grouname"].ToString();
                oModel.acctname = dt.Rows[i]["acctname"].ToString();
                if (Convert.ToDecimal(dt.Rows[i]["dr"].ToString()) > 0) { oModel.dr = Convert.ToDecimal(dt.Rows[i]["dr"]);
                dDR += Convert.ToDecimal(dt.Rows[i]["dr"]);
                } else oModel.dr = 0;
                if (Convert.ToDecimal(dt.Rows[i]["cr"].ToString()) > 0) {oModel.cr = Convert.ToDecimal(dt.Rows[i]["Cr"]);
                dCR += Convert.ToDecimal(dt.Rows[i]["Cr"]);
                }
                else oModel.cr = 0;

                oModel.opbl = Convert.ToDecimal(dt.Rows[i]["opbl"].ToString());
                oModel.crdrbl = Convert.ToDecimal(dt.Rows[i]["crdrbl"].ToString());

                oList1.Add(oModel);
            }

            oModel = new Models.clsLedger();
            oModel.Compcode = 0;oModel.opbl = oModel.crdrbl =oModel.acctcode = 0;
            oModel.grouname = ""; oModel.acctname = "Total"; 
            oModel.dr = dDR;
            oModel.cr = dCR; 
            oList1.Add(oModel);
             
            oModel.LstItem = oList1;

            return View(oModel);

            //clsLedger oModel = new clsLedger();
            //List<clsLedger> oList1 = new List<clsLedger>();

            //oModel.LstItem = oList1;
            //return View(oModel);
        }

        //[HttpPost]
        //public ActionResult TrialBal (clsLedger oCls )
        //{
        //    clsLedger oModel = new clsLedger();
        //    clsSubmitModel oSubmit = new clsSubmitModel();
        //    DataTable dt = new DataTable();
        //    int comp = Convert.ToInt32(SessionMaster.CompCode);
        //    if (comp == 0)
        //    {
        //        TempData["message"] = "Please Select Company ...";
        //    }

        //    dt = oSubmit.GetData("select * from [udf_GetTrialBalQuick]('04/01/2017', '04/01/2017', 60 , 0)  order by grouposi , grouname ,acctname ");

        //            List<clsLedger> oList1 = new List<clsLedger>();
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                oModel = new Models.clsLedger(); 
        //                oModel.acctcode = Convert.ToInt32(dt.Rows[i]["acctcode"].ToString());
        //                oModel.grouname = dt.Rows[i]["grouname"].ToString(); 
        //                oModel.acctname = dt.Rows[i]["acctname"].ToString(); 
        //                if (Convert.ToDecimal(dt.Rows[i]["dr"].ToString()) > 0) oModel.dr = Convert.ToDecimal(dt.Rows[i]["dr"]).ToString("0.00"); else oModel.dr = "";
        //                if (Convert.ToDecimal(dt.Rows[i]["cr"].ToString()) > 0) oModel.cr = Convert.ToDecimal(dt.Rows[i]["Cr"]).ToString("0.00"); else oModel.cr = "";

        //                oModel.opbl = Convert.ToDecimal(dt.Rows[i]["opbl"].ToString()); 
        //                oModel.crdrbl = Convert.ToDecimal(dt.Rows[i]["crdrbl"].ToString());  

        //                oList1.Add(oModel);
        //            }
        //            oModel.LstItem = oList1;

        //    return View(oModel);
        //}

        [HttpGet]
        public ActionResult Filter()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();


            DataTable dtParty;

            List<clsFilter> lstFilter = new List<clsFilter>();
            dtParty = oSubmit.GetData("Select  AcctName , acctCode from Account where Compcode =" + SessionMaster.CompCode + " and acctgrou in (21 ,22) order by AcctName ");

            clsFilter sm1 = new clsFilter();
            clsFilter sm = new clsFilter();
            clsPoItem Po1 = new clsPoItem();

            for (int i = 0; i < dtParty.Rows.Count; i++)
            {
                sm = new clsFilter();
                sm.PartyName = dtParty.Rows[i]["AcctName"].ToString();
                sm.PartyID = Convert.ToInt32(dtParty.Rows[i]["acctCode"].ToString());
                lstFilter.Add(sm);
            }

            sm1.lstFilter = lstFilter;

            lstFilter = new List<clsFilter>();
            dtParty = oSubmit.GetData("Select  AcctName , acctCode from Account where Compcode =" + SessionMaster.CompCode + " and acctgrou = 34 order by AcctName ");
            for (int i = 0; i < dtParty.Rows.Count; i++)
            {
                sm = new clsFilter();
                sm.PartyName = dtParty.Rows[i]["AcctName"].ToString();
                sm.PartyID = Convert.ToInt32(dtParty.Rows[i]["acctCode"].ToString());
                lstFilter.Add(sm);
            }
            sm1.lstAgent = lstFilter;

            lstFilter = new List<clsFilter>();
            dtParty = oSubmit.GetData("Select  AcctName , acctCode from Account where Compcode =" + SessionMaster.CompCode + " and acctgrou = 28 order by AcctName ");
            for (int i = 0; i < dtParty.Rows.Count; i++)
            {
                sm = new clsFilter();
                sm.PartyName = dtParty.Rows[i]["AcctName"].ToString();
                sm.PartyID = Convert.ToInt32(dtParty.Rows[i]["acctCode"].ToString());
                lstFilter.Add(sm);
            }
            sm1.lstEmployee = lstFilter;

            lstFilter = new List<clsFilter>();
            dtParty = oSubmit.GetData("select cityCode ,cityName from citydet where citytype = 15 order by cityName ");
            for (int i = 0; i < dtParty.Rows.Count; i++)
            {
                sm = new clsFilter();
                sm.PartyName = dtParty.Rows[i]["cityName"].ToString();
                sm.PartyID = Convert.ToInt32(dtParty.Rows[i]["cityCode"].ToString());
                lstFilter.Add(sm);
            }
            sm1.lstCity = lstFilter;

            ERPDataContext o = new ERPDataContext();

            ViewBag.PartyList = (from a in o.accounts where a.compcode == SessionMaster.CompCode && a.acctgrou == 21 && a.acctgrou == 22 select a).AsEnumerable();


            return View(sm1);
        }

        public ActionResult LedgerReport(string sFrom, string sTO, string sParty , int WithItem =1 ,int Compcode =0)
        {
            clsLedger oModel = new clsLedger();
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = new DataTable();
            int comp = Convert.ToInt32(SessionMaster.CompCode);

            if (sFrom == null || sFrom == "") sFrom = "01/04/" + DateTime.Now.Year;
            if (sTO == null || sTO == "") sTO = "31/03/" + ((Int32)DateTime.Now.Year + 1); 

            dt = oSubmit.GetData("Exec sp_GetLedger @CompList = '" + Compcode + "', @AcctList = '" + sParty + "', @BdDateFrom = '" + oSubmit.GetDateFormat(sFrom) + "', @BdDateTo = '" + oSubmit.GetDateFormat(sTO) + "' ,@pItemDtls= " + WithItem);

            List<clsLedger> oList1 = new List<clsLedger>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                oModel = new Models.clsLedger();
                oModel.acctcode = Convert.ToInt32(dt.Rows[i]["trncode"].ToString());
                oModel.acctname = dt.Rows[i]["ledger"].ToString();
                if (Convert.ToDecimal(dt.Rows[i]["Dr"].ToString()) > 0) oModel.dr = Convert.ToDecimal(dt.Rows[i]["Dr"]); else oModel.dr = 0;
                if (Convert.ToDecimal(dt.Rows[i]["Cr"].ToString()) > 0) oModel.cr = Convert.ToDecimal(dt.Rows[i]["Cr"]); else oModel.cr = 0;

                oModel.crdrbl = Convert.ToDecimal(dt.Rows[i]["Bal"].ToString());
                oModel.mstdate = Convert.ToDateTime(dt.Rows[i]["VcDt"].ToString());
                oModel.mstcode = Convert.ToInt32(dt.Rows[i]["trncode"].ToString());
                oModel.msttype = Convert.ToInt32(dt.Rows[i]["trntype"].ToString());
                oModel.sType = dt.Rows[i]["VcType"].ToString();
                oModel.VcNO = dt.Rows[i]["VcNo"].ToString();
                oModel.Particular = dt.Rows[i]["OnAccountName"].ToString() + " <br/> " + dt.Rows[i]["Remark"].ToString().Replace("&lt;br/&gt;", " <br/> ");
                oModel.mstrema = Convert.ToDateTime(dt.Rows[i]["VcDt"].ToString()).ToString("dd/MM/yyyy");

                oModel.trnsrno = Convert.ToInt32(dt.Rows[i]["trnitem"].ToString());
                oModel.trnrema = dt.Rows[i]["DrCr"].ToString();
                oModel.Compcode = Compcode;
                oList1.Add(oModel);
            }
            // oModel.LstItem = oList1;

            // return View( "Ledger" , oModel  );
            var sData = new { oList = oList1 };
            return Json(Json(sData).Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaleBook()
        {
            return View();
        }

        public ActionResult PuSLBookData(string sFrom, string sTO, string sParty, int PType)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();

            List<SaleBook> oList = new List<SaleBook>();

            oList = oSubmit.GetSaleBook(SessionMaster.CompCode, oSubmit.GetDateFormat(sFrom), oSubmit.GetDateFormat(sTO), PType);

            return PartialView("SaleBook", oList);
        }

        public JsonResult GetCityArr()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();


            DataTable dtParty;

            List<clsFilter> lstFilter = new List<clsFilter>();

            clsFilter sm1 = new clsFilter();
            clsFilter sm = new clsFilter();

            lstFilter = new List<clsFilter>();
            dtParty = oSubmit.GetData("select cityCode ,cityName from citydet where citytype = 15 order by cityName ");
            for (int i = 0; i < dtParty.Rows.Count; i++)
            {
                sm = new clsFilter();
                sm.PartyName = dtParty.Rows[i]["cityName"].ToString();
                sm.PartyID = Convert.ToInt32(dtParty.Rows[i]["cityCode"].ToString());
                lstFilter.Add(sm);
            }
            sm1.lstCity = lstFilter;

            return Json(sm1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SendMail(string PartyIDs, int CompCode, string From, string To ,int Port ,string  Srv)
        {
            try
            {
                string Msg="";
                clsSubmitModel oSubmit = new clsSubmitModel(); 
                string sMSG = "Dear Sir,</br> Please Check Attachments </br></br> Thank you </br></br> <b>Tanishk Electronics </b>";
                string Email = ""; string sPath = "";
                string[] Row = PartyIDs.Split(',');
                //string[] Col;
                 
                sPath = Server.MapPath("~/UploadImg/Ledger");
                 
                System.IO.DirectoryInfo di = new DirectoryInfo(sPath); 
                foreach (FileInfo file in di.GetFiles())
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    //System.IO.File.Delete();

                    file.Delete();
                }

                foreach (string itm in Row)
                {
                    if (itm != "")
                    {
                        try
                        {
                            LedgerPrint(From, To, itm, 1, CompCode);

                            //Email = "support@digiclayinfotech.co.in";
                            Email = oSubmit.GetSingleData("Select Email from Account a inner join tblDistributor b on a.acctdeal = b.mstCode where a.acctCode = " + itm).ToString();
                            if (Email.ToString().Length > 6)
                            {
                                //SmtpClient smtpClient = new SmtpClient("mi3-wts5.a2hosting.com", Port); // 25 mi3-wtr1.supercp.com
                                SmtpClient smtpClient = new SmtpClient(Srv, Port); // 25
                                smtpClient.Credentials = new System.Net.NetworkCredential("sales@tanishkelectronics.com", "Uvstar@11");
                                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network ;
                                MailMessage mailMessage = new MailMessage("sales@tanishkelectronics.com", Email);

                                mailMessage.Subject = "Ledger";
                                mailMessage.IsBodyHtml = true;
                                mailMessage.Body = sMSG;

                                sPath = Server.MapPath("~/UploadImg/Ledger/Ledger_" + itm + ".pdf");

                                mailMessage.Attachments.Add(new Attachment(sPath));

                                smtpClient.Send(mailMessage);

                                //System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                                //SmtpClient SmtpServer = new SmtpClient("mi3-wts5.a2hosting.com", Port);
                                //mail.From = new MailAddress("sales@tanishkelectronics.com");
                                //mail.To.Add(Email);
                                //mail.Subject = "Enquiry";
                                //mail.IsBodyHtml = true;
                                //mail.Body = "Test".ToString();
                                //SmtpServer.Credentials = new System.Net.NetworkCredential("sales@tanishkelectronics.com", "Uvstar@11");
                                //SmtpServer.EnableSsl = false;
                                //SmtpServer.Send(mail);


                                Msg += "Send Successfully";

                                //if (System.IO.File.Exists(sPath))
                                //{ System.IO.File.Delete(sPath); }
                            }
                            else
                            {
                                Msg += "Email Not Proper .";
                            }
                        }
                        catch (Exception EXP)
                        {
                            Msg += EXP.Message;
                        }
                    }
                }
                return Json(new { Success = true, Message =Msg  });
            }
            catch (Exception EX)
            {
                return Json(new { Success = false, Message = "Something went wrong. Please try again. " + EX.Message });
            }

        }

        private void LedgerPrint(string sFrom, string sTO, string sParty, int WithItem = 1, int Compcode = 0)
        {
            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            string fileName = "PUSL"; string rPath = Server.MapPath("~/Report/Ledger.rdlc");

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Report/Ledger.rdlc");
            viewer.LocalReport.ReportEmbeddedResource = rPath;

            viewer.LocalReport.DataSources.Clear();

            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dtComp;
            dtComp = oSubmit.GetData("select Compadd1 + ' ' + CompCity + ' ' + CompStat Address,Compmail ,Compmobi ,  * from Company where Compcode ="+ Compcode);
            string Address ="",  Email ="", Mobile = "";
            if (dtComp.Rows.Count > 0)
            { 
                Address = dtComp.Rows[0]["Address"].ToString(); 
                Email = dtComp.Rows[0]["Compmail"].ToString();
                Mobile =dtComp.Rows[0]["Compmobi"].ToString();
            }
             
            if (sFrom == null || sFrom == "") sFrom = "01/04/" + DateTime.Now.Year;
            if (sTO == null || sTO == "") sTO = "31/03/" + ((Int32)DateTime.Now.Year + 1);


            //DataTable dt = oSubmit.GetData("Exec sp_GetLedger @CompList = '" + Compcode + "', @AcctList = '" + sParty + "', @BdDateFrom = '" + oSubmit.GetDateFormat(sFrom) + "',           

            DataTable dt = oSubmit.GetData("Exec sp_GetLedger_PDF @CompList = '" + Compcode + "', @AcctList = '" + sParty + "', @BdDateFrom = '" + oSubmit.GetDateFormat(sFrom) + "', @BdDateTo = '" + oSubmit.GetDateFormat(sTO) + "' ,@pItemDtls= " + WithItem);
             
            ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.EnableExternalImages = true;

            ReportParameter pFrom = new ReportParameter("From", sFrom);
            ReportParameter pTo = new ReportParameter("To", sTO);
            viewer.LocalReport.SetParameters(new ReportParameter[] { pFrom, pTo });

            //ReportParameter pAddress = new ReportParameter("Address", Address);
            //ReportParameter pEmail = new ReportParameter("Email", Email);
            //ReportParameter pMobile = new ReportParameter("Mobile", Mobile);
            //viewer.LocalReport.SetParameters(new ReportParameter[] { pFrom, pTo ,pAddress,pEmail,pMobile });

            viewer.LocalReport.EnableHyperlinks = true;
            viewer.LocalReport.Refresh();

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            string sPath = Server.MapPath("~/UploadImg/Ledger/Ledger_" + sParty + ".pdf");

            FileStream fs = new FileStream(sPath, FileMode.OpenOrCreate);
            byte[] data = new byte[fs.Length];
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
             

        }

    }
}
