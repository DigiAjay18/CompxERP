using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using CompxERP.Models;
namespace CompxERP.Controllers
{
    public class CRMController : Controller
    {
        //
        // GET: /CRM/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RaiseTicket()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RaiseTicket(clsTicketMst obj)
        {
            string sMsg = "";
            try
            {
                clsSubmitModel oSubmit = new clsSubmitModel(); 
                ERPDataContext DB = new ERPDataContext();
                //if (ModelState.IsValid)
                //{
                    string sPath = "";
                    var File = obj.ImageFile;
                    if (File != null)
                    {
                        var sFileName = Path.GetFileName(File.FileName);
                        var sExt = Path.GetExtension(File.FileName);
                        var sFileNameWithoutExt = Path.GetFileNameWithoutExtension(File.FileName);

                        sPath = Server.MapPath("~/UploadImg/" + File.FileName);
                        File.SaveAs(sPath);

                        //sPath = ("~/UploadImg/" + File.FileName); For Set Image From JavaScript 
                        sPath = ("../UploadImg/" + File.FileName);
                    }

                    obj.tPath = sPath;

                    TicketMst tblObj = new TicketMst();
                    tblObj.tFor = obj.tFor;
                    tblObj.tTopic = obj.tTopic;
                    tblObj.tType = obj.tType;
                    tblObj.tUserID = SessionMaster.UserID;
                    tblObj.tStatus = 0;
                    tblObj.tDesc = obj.tDesc;
                    tblObj.tPath = obj.tPath;
                    tblObj.tDate =oSubmit.GetDateFormat( obj.sDate);
                    tblObj.tCreatedOn = DateTime.Now.Date;

                    obj.tNo = GetNoFormate(Convert.ToInt32(DB.TicketMsts.Max(a => (int?)a.ID) + 1).ToString());
                    DB.TicketMsts.InsertOnSubmit(tblObj);

                    DB.SubmitChanges();
                    sMsg = "Your Ticket No is " + obj.tNo + " !";
               //  }
           }
            catch (Exception ex)
            {
                sMsg = ex.Message;
            }

            return Json(Json(sMsg).Data, JsonRequestBehavior.AllowGet);
        }
        public string GetNoFormate(string sNo)
        {
            if (sNo.Length == 1)
                sNo = "00" + sNo;
            else if (sNo.Length == 2)
                sNo = "0" + sNo;
            else
                sNo = sNo;

            return sNo + DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");
        }


    }
}
