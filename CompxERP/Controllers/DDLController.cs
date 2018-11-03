using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompxERP.Models;

namespace CompxERP.Controllers
{
    public class DDLController : Controller
    {
        //
        // GET: /DDL/

        public List<SelectListItem> getMasterMenu()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = oSubmit.GetData("select MenCode,MenDesc from Menuoption where MenGrou = 0", true);

            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new SelectListItem { Text = dr["MenDesc"].ToString(), Value = dr["MenCode"].ToString() });
            }
            return lst;
        }
        public ActionResult Index()
        {
            return View();
        }
        public List<SelectListItem> getSourceList()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = oSubmit.GetData("select  studName,studCode from studdet where studtype  = 50 order By StudName");

            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new SelectListItem { Text = dr["studName"].ToString(), Value = dr["studCode"].ToString() });
            }
            return lst;
        }
          
        public List<SelectListItem> getStateList()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = oSubmit.GetData("SELECT cityName,cityCode from Citydet where cityType = 67 And CityRute=3 order by cityName");

            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new SelectListItem { Text = dr["cityName"].ToString(), Value = dr["cityCode"].ToString() });
            }
            return lst;
        }
        public List<SelectListItem> getUser()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = oSubmit.GetData("select  UseCode  ,Usename from loginusers Order by Usename", true);

            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new SelectListItem { Text = dr["Usename"].ToString(), Value = dr["UseCode"].ToString() });
            }
            return lst;
        }
        public List<SelectListItem> getGroupUser(string UseType = "0")
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = oSubmit.GetData("select  UseCode  ,Usename from loginusers where compcode="+SessionMaster.CompCode+" and  usetype=" + UseType + " Order by Usename", true);

            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new SelectListItem { Text = dr["Usename"].ToString(), Value = dr["UseCode"].ToString() });
            }
            return lst;
        }
        public List<SelectListItem> getBankList()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = oSubmit.GetData("SELECT AcctName , AcctCode from Account where acctgrou = 24 order By AcctName");

            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new SelectListItem { Text = dr["AcctName"].ToString(), Value = dr["AcctCode"].ToString() });
            }
            return lst;
        }

        public JsonResult GetDistrict(int StateID = 0)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = oSubmit.GetData("SELECT cityName,cityCode from Citydet where cityType = 68 And CityRute=" + StateID + " order by cityName");

            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new SelectListItem { Text = dr["cityName"].ToString(), Value = dr["cityCode"].ToString() });
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCategory()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = oSubmit.GetData("select itgpName ,itgpcode ,itgpalia  from  itGroup where itgpunde = 0 and Compcode =" + SessionMaster.CompCode + " order by itgpName", true);

            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new SelectListItem { Text = dr["itgpName"].ToString(), Value = dr["itgpcode"].ToString() });
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSubCategory(int CatID = 0)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = oSubmit.GetData("select itgpName ,itgpcode  from  itGroup where itgpunde = " + CatID + " and Compcode =" + SessionMaster.CompCode + " order by itgpName", true);

            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new SelectListItem { Text = dr["itgpName"].ToString(), Value = dr["itgpcode"].ToString() });
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStudDet(int studtype = 0)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = oSubmit.GetData("select  studName,studCode from studdet where studtype  = " + studtype + " order By StudName", true);

            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new SelectListItem { Text = dr["studName"].ToString(), Value = dr["studCode"].ToString() });
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllCategory()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = oSubmit.GetData("select itgpName ,itgpcode ,itgpalia ,itgpsort from  itGroup where itgpunde = 0 and Compcode =" + SessionMaster.CompCode + " order by itgpName", true);
            itgroup oMode = new itgroup();
            List<itgroup> lst = new List<itgroup>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                oMode = new itgroup();
                oMode.itgpalia = dt.Rows[i]["itgpalia"].ToString();
                oMode.itgpname = dt.Rows[i]["itgpName"].ToString();

                if (dt.Rows[i]["itgpsort"].ToString() != "") oMode.itgpsort = Convert.ToInt32(dt.Rows[i]["itgpsort"].ToString());
                else oMode.itgpsort = 0;

                oMode.itgpcode = Convert.ToInt32(dt.Rows[i]["itgpcode"]);
                lst.Add(oMode);
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllSubCategory(int CatID)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = oSubmit.GetData("select itgpName ,itgpcode ,itgpalia ,itgpsort ,itgpbcqt ,itgptype,itgpbasi,itgprefn,itgpcart ,itgpunde from itgroup where itgpunde = " + CatID + " and Compcode =" + SessionMaster.CompCode + " order by itgpName", true);
            itgroup oMode = new itgroup();
            List<itgroup> lst = new List<itgroup>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                oMode = new itgroup();
                oMode.itgpalia = dt.Rows[i]["itgpalia"].ToString();
                oMode.itgpname = dt.Rows[i]["itgpName"].ToString();

                if (dt.Rows[i]["itgpsort"].ToString() != "") oMode.itgpsort = Convert.ToInt32(dt.Rows[i]["itgpsort"].ToString());
                else oMode.itgpsort = 0;

                oMode.itgpcode = Convert.ToInt32(dt.Rows[i]["itgpcode"]);

             if (dt.Rows[i]["itgpbcqt"].ToString()!="")   oMode.itgpbcqt = Convert.ToInt32(dt.Rows[i]["itgpbcqt"]);
             if (dt.Rows[i]["itgptype"].ToString() != "") oMode.itgptype = Convert.ToInt16(dt.Rows[i]["itgptype"]);
             if (dt.Rows[i]["itgpbasi"].ToString() != "") oMode.itgpbasi = Convert.ToInt16(dt.Rows[i]["itgpbasi"]);
                oMode.itgprefn = dt.Rows[i]["itgprefn"].ToString();
                if (dt.Rows[i]["itgpcart"].ToString() != "") oMode.itgpcart = Convert.ToInt32(dt.Rows[i]["itgpcart"]); else oMode.itgpcart = 0;

                lst.Add(oMode);
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
		public List<SelectListItem> GetVisitPartyList()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = oSubmit.GetData("sp_GetVisitPartyList @CompCode=" + SessionMaster.CompCode);
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new SelectListItem { Text = dr["aName"].ToString(), Value = dr["aCode"].ToString() });
            }
            return lst;
        }
        public List<SelectListItem> GetVisitPartyAreaList()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = oSubmit.GetData("sp_GetVisitPartyAreaList @CompCode="+SessionMaster.CompCode);
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new SelectListItem { Text = dr["aName"].ToString(), Value = dr["aCode"].ToString() });
            }
            return lst;
        }
        public List<SelectListItem> GetVisitSchForList()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt = oSubmit.GetData("sp_GetVisitSchForList @CompCode=" + SessionMaster.CompCode);
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new SelectListItem { Text = dr["aName"].ToString(), Value = dr["aCode"].ToString() });
            }
            return lst;
        }
		 /*Start 181016 %temp%*/
        public List<SelectListItem> GetDailyVisitPartyList()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            int empid = 0;
            if(SessionMaster.UserType!=0 || SessionMaster.UserType!=1)
                empid = SessionMaster.UserID;
            DataTable dt = oSubmit.GetData("sp_GetDailyVisitPartyList @CompCode=" + SessionMaster.CompCode + ",@EmpID=" + empid + ",@UserType=" + SessionMaster.UserType);/*181017 %temp%*/
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new SelectListItem { Text = dr["aName"].ToString(), Value = dr["aCode"].ToString() });
            }
            return lst;
        }
        /*End 181016 */
    }
}
