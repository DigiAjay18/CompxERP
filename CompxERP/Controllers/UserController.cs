using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompxERP.Models;
//using CompxERP.Filters;
namespace CompxERP.Controllers
{
    
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Rights()
        {
            submitData oCon = new submitData();
            clsRights frm = new clsRights();
            
            DDLController oDDL = new DDLController();
            frm.lstGrpMenu = oDDL.getMasterMenu();
            frm.lstUser = oDDL.getUser();
            frm.lstGrpUser = oDDL.getGroupUser();
            return View(frm);
        }
        public ActionResult GetGroupUser(string sGroupID)
        {
            clsRights frm = new clsRights();
            DDLController oDDL = new DDLController();
            frm.lstGrpUser = oDDL.getGroupUser(sGroupID);
            return Json(frm, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetChildTypeUser(int sHeadID=0,int sTypeID=0)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            var model1 = oSubmit.GetChildTypeUser("exec sp_GetChildTypeUser @CompCode =" + SessionMaster.CompCode + ",@UseCode=" + sHeadID + ",@TypeID=" + sTypeID);
            return Json(model1, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetParty(int iGroup, int iUser)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();

            var model = oSubmit.GetRigths("Exec GetUserRight @MenGrou =" + iGroup + " , @UserCode = " + iUser);
             
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsRights(int iGroup, int iUser, bool View, bool Add, bool Del, bool Acc, bool Edit)
        {

            try
            {
                clsSubmitModel oSubmit = new clsSubmitModel();

                clsRights oProp = new clsRights();
                oProp.mencode = iGroup;
                oProp.menuser = iUser;
                oProp.menview = View;
                oProp.menaddi = Add;
                oProp.menedit = Edit;
                oProp.mendele = Del;
                oProp.menacce = Acc;

                oSubmit.InsRights(oProp);

                var Result = "Saved Successfully .";
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpGet]
        //public ActionResult UserWork()
        //{
        //    submitData oCon = new submitData();
        //    clsUserWork frm = new clsUserWork();

        //    DDLController oDDL = new DDLController();
        //    frm.lstUser = oDDL.getUser();

        //    clsSubmitModel oSubmit = new clsSubmitModel();
             
        //    return PartialView("_UserWork", frm);
        //}

        // public JsonResult GetUserWork(int iUserCode =0,   string To = "", string From = "")
        //{
        //    clsSubmitModel oSubmit = new clsSubmitModel();

        //    List<clsUserWork> lstUser = oSubmit.UserWork(iUserCode ,To ,From);

        //    return Json(lstUser ,JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult GetUserWork(int iUserCode = 0, string To = "", string From = "", int ModeID = 5)
        //{
        //    clsSubmitModel oSubmit = new  clsSubmitModel();

        //    List<clsUserWork> lstUser = oSubmit.UserWork(iUserCode, To, From, ModeID , SessionMaster.CompCode);

        //    return PartialView("_UserWork", lstUser); 
        //}

        //public ActionResult GetDayWork(int iUserCode = 0, string To = "", string From = "")
        //{
        //    clsSubmitModel oSubmit = new clsSubmitModel();

        //    List<clsDayWork> lstUser = oSubmit.GetDayWork(iUserCode, To, From ,SessionMaster .CompCode);

        //    return PartialView("_DayWork", lstUser);
        //}

        [HttpGet]
        public ActionResult UserReport()
        {
            submitData oCon = new submitData();
            clsUserWork frm = new clsUserWork();

            DDLController oDDL = new DDLController();
            frm.lstUser = oDDL.getUser();

            return View(frm);
        }

        public ActionResult UserList()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();

            List<clsUser> lstFollow = oSubmit.UserList("select usecode ,usename , usetype ,usestatus , dbo.Decrypt(a.usePass) PassWord ,  case usetype when 0 then 'Admin'  when 1 then 'User'  else ''  end UserType ,usecrdt ,usemodt ,UserNM from loginusers a order by UseName");

            return PartialView("_UserList", lstFollow);
        }

        public ActionResult ChangePassword()
        {
            return View("ChangePassword");
        }


        public ActionResult GetUserWork(int iUserCode = 0, string To = "", string From = "", int ModeID = 5 ,int CompCode=66)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();

            List<clsUserWork> lstUser = oSubmit.UserWork(iUserCode, To, From, ModeID,  CompCode);

            return PartialView("_UserWork", lstUser);
        }

        public ActionResult GetDayWork(int iUserCode = 0, string To = "", string From = "")
        {
             clsSubmitModel oSubmit = new  clsSubmitModel();

            List<clsDayWork> lstUser = oSubmit.GetDayWork(iUserCode, To, From, SessionMaster.CompCode);

            return PartialView("_DayWork", lstUser);
        }

 //public ActionResult InvSett()
 //       { 
 //    EmpmstDataContext oDB = new EmpmstDataContext(); 

 //   var olist  = from a  in oDB.invesetts select a ;
   
 //           return PartialView("InvSett", olist.ToList());
 //       }
        
 //       [HttpPost]
 //       public ActionResult InvSett( int iCode , string Value ,bool YN)
 //       { 
 //           try {
 //    EmpmstDataContext oDB = new EmpmstDataContext();

 //    invesett frm = new invesett();

 //    var olist = (from a in oDB.invesetts where a.optcode == iCode select a).SingleOrDefault();
 //    olist.optvalu = Value;
 //    olist.optYN = YN;
  
 //           oDB.SubmitChanges();

 //           return Json("Saved", JsonRequestBehavior.AllowGet);
 //             }
 //           catch (Exception ex)
 //           {
 //              return Json(ex.Message , JsonRequestBehavior.AllowGet);
 //           }
 //       }
    }
}
