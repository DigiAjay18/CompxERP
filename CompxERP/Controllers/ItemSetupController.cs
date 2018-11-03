using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompxERP.Models;
using System.Data.SqlClient;
using System.IO;
using Newtonsoft.Json;
using System.Data;
using CompxERP.Filters;

namespace CompxERP.Controllers
{
    [UserSessionfilter]
    public class ItemSetupController : Controller
    {
        //
        // GET: /ItemSetup/
        ERPDataContext db = new ERPDataContext();
       
        public ActionResult Index()//int compcode = 0, string username = ""
        {
            

            int compcode1 = SessionMaster.CompCode;//170817
            var res = from k in db.itemains
                      where k.compcode == compcode1
                      select new
                      {
                          compcode = k.compcode,
                          itemcode = k.itemcode,
                          itemname = k.itemname,
                          itemgrou = k.itemgrou,
                          itemnumb = k.itemnumb,
                          itemsrno = k.itemsrno,
                          itemopbl = k.itemopbl,
                          itemclbl = k.itemclbl,
                          itemmini = k.itemmini,
                          itemmaxi = k.itemmaxi,
                          itemrate = k.itemrate,
                          itlastrat = k.itlastrat,
                          itemsort = k.itemsort,
                          itemalia = k.itemalia,
                          itempart = k.itempart,
                          itemdisc = k.itemdisc,
                          itemrefn = k.itemrefn,
                          itemtext = k.itemtext,
                          itemtype = k.itemtype,
                          itemvalu = k.itemvalu,
                          itemexp = k.itemexp,
                          itemvatr = k.itemvatr,
                          itemmrpv = k.itemmrpv,
                          itemhind = k.itemhind,
                          itedrwng = k.itedrwng,
                          itmpric = k.itmpric,
                          iteper = k.iteper,
                          itebesic = k.itebesic,
                          itemPrcnt = k.itemPrcnt
                      };
            return View(res.ToList());

        }
        public ActionResult IndexItemSetupList()
        {
            //Session["compcode"] = 6;
            int compcode1 = SessionMaster.CompCode;//170817
            var res = from k in db.itemains
                      where k.compcode == compcode1
                      select new
                      {
                          compcode = k.compcode,
                          itemcode = k.itemcode,
                          itemname = k.itemname,
                          itemgrou = k.itemgrou,
                          itemnumb = k.itemnumb,
                          itemsrno = k.itemsrno,
                          itemopbl = k.itemopbl,
                          itemclbl = k.itemclbl,
                          itemmini = k.itemmini,
                          itemmaxi = k.itemmaxi,
                          itemrate = k.itemrate,
                          itlastrat = k.itlastrat,
                          itemsort = k.itemsort,
                          itemalia = k.itemalia,
                          itempart = k.itempart,
                          itemdisc = k.itemdisc,
                          itemrefn = k.itemrefn,
                          itemtext = k.itemtext,
                          itemtype = k.itemtype,
                          itemvalu = k.itemvalu,
                          itemexp = k.itemexp,
                          itemvatr = k.itemvatr,
                          itemmrpv = k.itemmrpv,
                          itemhind = k.itemhind,
                          itedrwng = k.itedrwng,
                          itmpric = k.itmpric,
                          iteper = k.iteper,
                          itebesic = k.itebesic,
                          itemPrcnt = k.itemPrcnt
                      };
            return View(res.ToList());
        }
        public ActionResult demo()
        {
            return View();
        }
     
        public ActionResult GetCategoryDet(int iCatID, int comp)//170513
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            itgroup pmst = new itgroup();
            var Items = from a in db.itgroups
                        where a.itgpcode == iCatID && a.compcode == comp
                        select new {name=a.itgpname, code = a.itgpcode, sort = a.itgpsort, alia = a.itgpalia, itgpcart = a.itgpcart, itgprefn = a.itgprefn };//170824

            return Json(Items, JsonRequestBehavior.AllowGet);

        }
       
        public ActionResult GetItemInfo(int comp, int subcat)//170513
        {
            if (comp == 0) comp = SessionMaster.CompCode;

            var Items = (from a in db.itemains
                         //join x in db.unitdets on new { f2 = a.compcode, f1 = a.itemmaxi } equals new { f2 = (int)x.compcode, f1 = (decimal?)x.unitcode } into t1 from y in t1.DefaultIfEmpty()
                         join z in db.studdets on new { f2 = (int)a.itemnumb, f1 = 61 } equals new { f2 = z.studCode, f1 = z.studType } into t1
                         from y in t1.DefaultIfEmpty()
                         where a.compcode == comp && a.itemgrou == subcat
                         select new
                         {
                             itemcode = a.itemcode,
                             Desc = a.itemname,
                             Unit = a.itemmaxi, /*UnitName = y.unitname,*/
                             Packing = a.itemsort,
                             Avg = 0,
                             Alias = a.itemalia,
                             Extra = a.itemtext,
                             Vat = a.itemvatr,
                             Hindi = a.itemhind,
                             Weight = a.itemdisc,
                             itemhsnc = a.itemhsnc,
                             itemgstr = a.itemgstr,
                             itemcess = a.itemcess,
                             itemmini = a.itemmini,
                             itemnumb = a.itemnumb,
                             itemnumbName = y.studName,
                             ItemRate = a.itemrate,
                             ItemOpening = a.itemopbl
                         });
 
            return Json(Items, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetSubCategoryInfo(string query, string catcode)//170513
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            DataTable dt;
            dt = oSubmit.GetSubCategoryInfo(query, Session["compcode"].ToString(), catcode);
            var result = new List<KeyValuePair<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new KeyValuePair<string, string>(dr["Code"].ToString(), dr["Name"].ToString()));
            }
            var result3 = result.Where(s => s.Value.ToLower().Contains(query.ToLower())).Select(w => w).ToList();
            return Json(result3, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubCategoryDet(int iCatID)//170513
        {
            var Items = from a in db.itgroups
                        where a.itgpcode == iCatID && a.compcode == int.Parse(Session["compcode"].ToString())
                        select new { name = a.itgpname, code = a.itgpcode, sort = a.itgpsort, alia = a.itgpalia, BasicUnit = a.itgpbasi, RateonPer = a.itgpbcqt, StkVlAsPerPrchPrcLst = a.itgpnumb, itgprefn1 = a.itgprefn, itgpcart1 = a.itgpcart };//170824
            return Json(Items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetSubCategoryInfo(string name, string alia, string sort, string catid, string CatType, string sBasicUnit, string RateonPer, string StkVlAsPerPrchPrcLst, string hsn, string gst)//170822
        {
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            itgroup pmst = new itgroup();

            int compcode1 = Convert.ToInt32(Session["compcode"]);
            int iSort = 0;
            if (sort != "") iSort = int.Parse(sort);
            decimal dGst = 0;//170822
            pmst.compcode = compcode1;
            pmst.itgpname = name;
            pmst.itgpalia = alia;
            pmst.itgpsort = iSort;
            pmst.itgpunde = short.Parse(catid);

            if (CatType != "") pmst.itgptype = short.Parse(CatType);
            if (sBasicUnit != "") pmst.itgpbasi = short.Parse(sBasicUnit);
            if (RateonPer != "") pmst.itgpbcqt = int.Parse(RateonPer);
            if (StkVlAsPerPrchPrcLst != "") pmst.itgpnumb = short.Parse(StkVlAsPerPrchPrcLst);
            pmst.itgprefn = hsn;//170822
            decimal.TryParse(gst, out dGst);//170822
            pmst.itgpcart = dGst;//170822
            DataTable dt = oSubmit.SetSubCategoryInfo(pmst);

            //********************************* User Work ********************************* 
            clsUserWork oUser = new clsUserWork();
            oUser.woruser = SessionMaster.UserID;
            oUser.wormode = 0;
            oUser.worcomp = SessionMaster.CompCode;
            oUser.wortype = 14;
            oUser.worcode = Convert.ToInt32(dt.Rows[0][2].ToString());
            oUser.worsrno = oSubmit.GetUsWoCode(); ;
            oUser.worrema = "D-" + catid + "#" + name + "#" + alia + "#" + hsn;
            oUser.wordate = Convert.ToDateTime("04/01/1900");
            oUser.worrfsr = "";
            oUser.worsysn = Environment.MachineName;
            oSubmit.InsUserWork(oUser);
            //********************************* User Work ********************************* 
            return Json(Json(dt.Rows[0][2].ToString()).Data, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Remotdata(String query)
        //{
        //    int compcode1 = Convert.ToInt32(Session["compcode"]);
        //    // List<String> listData = null;
        //    if (!String.IsNullOrEmpty(query))
        //    {
        //        var distinct = db.Database.SqlQuery<DistinctList>("Select itgpname,itgpcode FROM [dbo].[itgroup] where compcode=" + compcode1 + " and itgpunde=0 and itgpname like '%" + query.ToLower() + "%'").Distinct().ToList();

        //        //var distinct = db.Database.SqlQuery<DistinctList>("Select catsub,itemgrou FROM dbo.Item_SubCet where compcode=" + compcode1 + " and itgpunde!=0").ToList();

        //        //ViewBag.Categories = new SelectList(distinct, "itgpcode", "catsub", item_subcet.It);
        //        //item_subcet.It = ViewBag.Categories;

        //        //var listDat21 = db.Item_SubCet.Where(i => i.compcode == compcode1 && i.category.ToLower().StartsWith(query.ToLower())).Select(i => i.category).Distinct();

        //        //listData = db.Item_SubCet.Where(i => i.compcode == compcode1 && i.category.ToLower().StartsWith(query.ToLower())).Select(i => i.category).Distinct().ToList();
        //        return Json(new { Data = distinct.Select(i => i.itgpname) });//
        //    }
        //    else
        //    {
        //        //return Json(new { Data = listData });
        //        return Json(new { Data = "" });
        //    }
        //}
        //
        // GET: /ItemSetup/RemotdataEdit/query
        [HttpPost]
        public ActionResult RemotdataEdit(String query)
        {
            //int compcode1 = Convert.ToInt32(Session["compcode"]);
            //List<String> listData = null;
            //if (!String.IsNullOrEmpty(query))
            //{
            //    var listDat21 = db.Item_SubCet.Where(i => i.compcode == compcode1 && i.itemname.ToLower().StartsWith(query.ToLower())).Select(i => i.itemname).Distinct();
            //    listData = db.Item_SubCet.Where(i => i.compcode == compcode1 && i.itemname.ToLower().StartsWith(query.ToLower())).Select(i => i.itemname).Distinct().ToList();
            //}
            return Json(new { Data = "" });
        }
        //
        // GET: /ItemSetup/GetSubcatAjax/query
       
        


        public ActionResult Delete(int id = 0)
        {
            //Item_SubCet item_subcet = db.Item_SubCet.Find(id);
            //if (item_subcet == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }
        //
        // POST: /ItemSetup/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            //Item_SubCet item_subcet = db.Item_SubCet.Find(id);
            //db.Item_SubCet.Remove(item_subcet);
            //db.SaveChanges();
            //TempData["Msg"] = "Data has been deleted successfully";
            return RedirectToAction("Index");
        }

 public ActionResult getCategory()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
             
            DataTable dt;
     string query ="select itgpName Name,itgpcode Code from  itGroup where itgpunde = 0 and Compcode = " + SessionMaster.CompCode;
            dt = oSubmit.GetData("select itgpName Name,itgpcode Code from  itGroup where itgpunde = 0 and Compcode = " + SessionMaster.CompCode);
            var result = new List<KeyValuePair<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new KeyValuePair<string, string>(dr["Code"].ToString(), dr["Name"].ToString()));
            }
            var result3 = result.Where(s => s.Value.ToLower().Contains(query.ToLower())).Select(w => w).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult delItem(string itemcode)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            itemain pmst = new itemain();

            int compcode1 = Convert.ToInt32(Session["compcode"]);
            pmst.compcode = compcode1;
            pmst.itemcode = int.Parse(itemcode);
            DataTable dt = oSubmit.DelItemInfo(pmst);
            return Json(Json(dt.Rows[0][1].ToString()).Data, JsonRequestBehavior.AllowGet);
        }
        
        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}

        public JsonResult GetItemList(int SubCatID = 0)
        {
            clsSubmitModel obj = new clsSubmitModel();
            List<clsItemain> Item = obj.GetItemList(Convert.ToInt16(SessionMaster.CompCode), SubCatID);

            return Json(Item, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CreateType()
        {
            short compcode1 = Convert.ToInt16(Session["compcode"]);
            ViewBag.MenCode = Request.QueryString["MenCode"];
            itgroup item_subcet = new itgroup();
           
            //var postTitles = db.Database.SqlQuery<AddCategory>("Select max(itgpcode) as maxitgpcode from dbo.itgroup where compcode='" + compcode1 + "'").SingleOrDefault();

            //var postTitles1 = postTitles.maxitgpcode;
            //postTitles1 = Convert.ToInt16(postTitles1 + 1);

            clsSubmitModel o = new clsSubmitModel();

            item_subcet.itgpcode = Convert.ToInt32( o.GetSingleData("Select isnull(max(itgpcode),0)+1  as maxitgpcode from dbo.itgroup where compcode=" + compcode1 ));
            return PartialView (item_subcet);
        }

        [HttpPost]
        public ActionResult CreateType(itgroup item_subcet, FormCollection form, int isEdit = 0)
        {
            //if (Convert.ToInt16(SessionMaster.CompCode) == 0) { Response.Redirect("~/Index/Index"); }

            int compcode1 = Convert.ToInt32(Session["compcode"]);
            //var maxitgpcode = db.Item_SubCet.Where(i => i.compcode == compcode1).Max(i => i.itgpcode);
            //maxitgpcode = Convert.ToInt16(maxitgpcode + 1);
            // var postTitles=0;
            int postTitles = 0;

            //var  postTitles1 = db.Database.SqlQuery<AddCategory>("Select max(itgpcode) as maxitgpcode from dbo.itgroup where compcode='" + compcode1 + "'").SingleOrDefault());

            clsSubmitModel oSubmit = new clsSubmitModel();

            if (item_subcet.itgpcode > 0)
                postTitles = item_subcet.itgpcode;
            else
            {
                postTitles = Convert.ToInt32(oSubmit.GetSingleData("Select isnull(max(itgpcode),0)+1 as maxitgpcode from dbo.itgroup where compcode=" + compcode1, "0", true));
            }

            //var postTitles1 = postTitles.maxitgpcode;
            //postTitles1 = Convert.ToInt16(postTitles1 + 1);

            // int itgpcode = maxitgpcode ?? 0;
            // short itgptype = item_subcet.itgptype ?? 0;
            // int itgptype = item_subcet.itgptype;
            String itgpalia = item_subcet.itgpalia;
            int itgpsort = item_subcet.itgpsort ?? 0;
            String itgpname = item_subcet.itgpname;
            int itgpbasi = item_subcet.itgpbasi ?? 0;
            decimal itgpbcqt = item_subcet.itgpbcqt ?? 0;
            short itgpnumb = item_subcet.itgpnumb ?? 0;

            String sql1 = "";

            if (item_subcet.itgpcode > 0)
                sql1 = "Update itgroup set itgpalia = '" + item_subcet.itgpalia + "',itgpsort = " + itgpsort + ",itgpname= '" + item_subcet.itgpname + "' where compcode = " + compcode1 + " and itgpcode =" + item_subcet.itgpcode;
            else
                sql1 = "insert into itgroup (compcode,itgpcode,itgpalia,itgpsort,itgpname,itgpbasi,itgpbcqt,itgpnumb,itgpunde,itgptype) values('" + compcode1 + "','" + postTitles + "','" + itgpalia + "','" + itgpsort + "','" + itgpname + "',0,0,0,0,0)";

            //int result1 = db.Database.ExecuteSqlCommand(sql1);
            oSubmit.insertData(sql1);
            //********************************* User Work ********************************* 
            //clsSubmitModel oSubmit = new clsSubmitModel();
            clsUserWork oUser = new clsUserWork();
            oUser.woruser = SessionMaster.UserID;
            oUser.worcomp = SessionMaster.CompCode;
            oUser.wortype = 14;
            oUser.worcode = postTitles;
            if (item_subcet.itgpcode > 0) oUser.wormode = 1; else oUser.wormode = 0;
            oUser.worsrno = oSubmit.GetUsWoCode();
            oUser.worrema = "D-" + itgpname + "#Cat";
            oUser.wordate = Convert.ToDateTime("04/01/1900");
            oUser.worrfsr = "";
            oUser.worsysn = Environment.MachineName;
            oUser.IP_Add = this.Request.UserHostAddress;
            oSubmit.InsUserWork(oUser);
            //********************************* User Work ********************************* 

            db.SubmitChanges();

            //return RedirectToAction("Index");
            return Json(postTitles, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult CreateSubType()
        {
            int compcode1 = Convert.ToInt32(Session["compcode"]);
            int id = Convert.ToInt32(Session["GetCat"]);
            ViewBag.MenCode = Request.QueryString["MenCode"];
            //Item_SubCet item_subcet = new Item_SubCet();
            itgroup item_subcet = new itgroup();

            //var postTitles = db.Database.SqlQuery<unitdet>("Select unitcode,unitname from dbo.unitdet where compcode=" + compcode1).ToList();
            var postTitles = from a in db.unitdets where a.compcode == compcode1 select new { a.unitcode, a.unitname };

            var vwCate = from a in db.itgroups where a.itgpunde == 0 where a.compcode == compcode1 orderby a.itgpname select new { a.itgpname, a.itgpcode };
            ViewBag.vwCategory = new SelectList(vwCate, "itgpcode", "itgpName");


            var vwUnit = from a in db.unitdets where a.compcode == compcode1 orderby a.unitname select new {itgpcode = a.unitcode,itgpName =a.unitname }; 
            ViewBag.vwUnit = new SelectList(vwUnit, "itgpcode", "itgpName");

            List<SelectListItem> Type = new List<SelectListItem>(){

            new SelectListItem { Text="Final", Value="0", Selected=true},
            new SelectListItem {Text="Raw", Value="1", Selected=false},
            new SelectListItem {Text="Packing", Value="2", Selected=false},
            new SelectListItem {Text="Spare", Value="3", Selected=false}
            };

            return PartialView(item_subcet); 
        }
        [HttpPost]
        public ActionResult CreateSubType(itgroup item_subcet, FormCollection form)
        {
            clsSubmitModel oSubmit = new clsSubmitModel(); 
            //if (Convert.ToInt16(SessionMaster.CompCode) == 0) { Response.Redirect("~/Index/Index"); }
            int compcode1 = Convert.ToInt32(Session["compcode"]);
            int id = Convert.ToInt32(Session["GetCat"]);
            // var maxitgpcode = db.Item_SubCet.Where(i => i.compcode == compcode1).Max(i => i.itgpcode);

            int itgpcode = 0;

            if (item_subcet.itgpcode > 0)
                itgpcode = item_subcet.itgpcode;
            else
            {
                itgpcode = (db.itgroups.Where(x => x.compcode == compcode1).Max(x => x.itgpcode));
                //var postTitles = db.Database.SqlQuery<AddCategory>("Select max(itgpcode) as maxitgpcode from dbo.itgroup where compcode='" + compcode1 + "'").SingleOrDefault();
                itgpcode = itgpcode + 1 ;
            }

            // int itgpcode = postTitles.maxitgpcode +1;
            // int itgptype = item_subcet.itgptype;
            String itgpalia = item_subcet.itgpalia;
            int itgpsort = item_subcet.itgpsort ?? 0;
            String itgpname = item_subcet.itgpname;
            int itgpbasi =(int) item_subcet.itgpbasi;// ?? 0;
            int itgptype = item_subcet.itgptype ?? 0;
            decimal itgpbcqt = item_subcet.itgpbcqt ?? 0;
            short itgpnumb = item_subcet.itgpnumb ?? 0;
            decimal itgpcart = item_subcet.itgpcart ?? 0;

            String sql1 = "";
            if (item_subcet.itgpcode > 0)
                sql1 = "Update itgroup set itgpalia= '" + itgpalia + "',itgpsort= " + itgpsort + ",itgpname ='" + itgpname + "',itgpbasi= " + itgpbasi + ",itgpbcqt =" + itgpbcqt + ",itgpnumb= " + itgpnumb + ",itgpunde= " + item_subcet.itgpunde + " ,itgprefn ='" + item_subcet.itgprefn + "' ,itgpcart= " + itgpcart + " ,itgptype= " + itgptype + " where compcode = " + compcode1 + " and itgpcode =" + itgpcode;
            else
                sql1 = "insert into itgroup (compcode,itgpcode,itgpalia,itgpsort,itgpname,itgpbasi,itgpbcqt,itgpnumb,itgpunde ,itgprefn ,itgpcart ,itgptype) values('" + compcode1 + "','" + itgpcode + "','" + itgpalia + "','" + itgpsort + "','" + itgpname + "','" + itgpbasi + "','" + itgpbcqt + "',0,'" + item_subcet.itgpunde + "' ,'" + item_subcet.itgprefn + "'," + itgpcart + "," + itgptype + ")";
            //int result1 = db.Database.ExecuteSqlCommand(sql1);
            oSubmit.insertData(sql1);
            //********************************* User Work ********************************* 
           
            clsUserWork oUser = new clsUserWork();
            oUser.woruser = SessionMaster.UserID;
            oUser.worcomp = SessionMaster.CompCode;
            oUser.wortype = 14;
            oUser.worcode = itgpcode;
            if (item_subcet.itgpcode > 0) oUser.wormode = 1; else oUser.wormode = 0;
            oUser.worsrno = oSubmit.GetUsWoCode();
            oUser.worrema = "D-" + itgpname + "#Cat";
            oUser.wordate = Convert.ToDateTime("04/01/1900");
            oUser.worrfsr = "";
            oUser.worsysn = Environment.MachineName;
            oUser.IP_Add = this.Request.UserHostAddress;
            oSubmit.InsUserWork(oUser);
            //********************************* User Work ********************************* 

            db.SubmitChanges();

            //Session["GetCat"] = null;
            //return RedirectToAction("Index");
            return Json(itgpcode, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Itemain()
        {
            //if (Convert.ToInt16(SessionMaster.CompCode) == 0) { Response.Redirect("~/Index/Index"); }

            ViewBag.MenCode = Request.QueryString["MenCode"];
            var vwCate = from a in db.itgroups  where a.itgpunde == 0 where a.compcode == SessionMaster.CompCode  orderby a.itgpname select new { a.itgpname ,a.itgpcode} ;
            ViewBag.vwCategory = new SelectList(vwCate, "itgpcode", "itgpName");

            var vwSubCate = from a in db.itgroups  where a.itgpunde != 0 where a.compcode == SessionMaster.CompCode orderby a.itgpname select new { a.itgpname, a.itgpcode };
            ViewBag.vwSubCate = new SelectList(vwCate, "itgpcode", "itgpName");
            var res1 = from a in db.studdets where a.studType==61 orderby a.studName select new { a.studCode, a.studName };
            ViewBag.vwItemnumb = new SelectList(res1, "studcode", "studname");

            return PartialView();
        }

        [HttpPost]
        public ActionResult Itemain(clsItemain frm)
        {
            //if (Convert.ToInt16(SessionMaster.CompCode) == 0) { Response.Redirect("~/Index/Index"); }

            clsSubmitModel oSubmit = new clsSubmitModel();
            frm.compcode = SessionMaster.CompCode;
            int iCode = oSubmit.insertandupdatecatitem(frm);

            //********************************* User Work ********************************* 
            clsUserWork oUser = new clsUserWork();
            oUser.woruser = SessionMaster.UserID;
            oUser.worcomp = SessionMaster.CompCode;
            oUser.wortype = 16;
            oUser.worcode = iCode;
            if (frm.itemcode > 0) oUser.wormode = 1; else oUser.wormode = 0;
            oUser.worsrno = oSubmit.GetUsWoCode();
            oUser.worrema = "D-" + frm.itemname + "#" + frm.itemhsnc;
            oUser.wordate = Convert.ToDateTime("04/01/1900");
            oUser.worrfsr = "";
            oUser.worsysn = Environment.MachineName;
            //oUser.IP_Add = this.Request.UserHostAddress;
            oSubmit.InsUserWork(oUser);
            //********************************* User Work ********************************* 

            return Json(iCode, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult DelCate(int itgpcode)
        {
            string s = "";
            try
            {
                s = "Category is in Use !";
                if ((from a in db.itgroups where a.itgpunde == itgpcode select a).Count() == 0)
                {
                    var Cate = (from a in db.itgroups where a.compcode == SessionMaster.CompCode && a.itgpunde == 0 && a.itgpcode == itgpcode select a).FirstOrDefault();
                    db.itgroups.DeleteOnSubmit(Cate);
                    db.SubmitChanges();
                    s = "Delete Successfully !";
                }

                return Json(s, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DelSubCate(int itgpcode)
        {
            string s = "";
            try
            {
                s = "Sub Category is in Use !";
                if ((from a in db.itemains where a.itemgrou == itgpcode select a).Count() == 0)
                {
                    var Cate = (from a in db.itgroups where a.compcode == SessionMaster.CompCode && a.itgpunde != 0 && a.itgpcode == itgpcode select a).FirstOrDefault();
                    db.itgroups.DeleteOnSubmit(Cate);
                    db.SubmitChanges();
                    s = "Delete Successfully !";
                }
                return Json(s, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DelItem(int itemcode)
        {
            string s = "";
            try
            {
                s = "Item is in Use !";
                if ((from a in db.itpursals where a.itditem == itemcode select a).Count() == 0)
                {
                    var Cate = (from a in db.itemains where a.compcode == SessionMaster.CompCode && a.itemcode == itemcode select a).FirstOrDefault();
                    db.itemains.DeleteOnSubmit(Cate);
                    db.SubmitChanges();
                    s = "Delete Successfully !";
                }
                return Json(s, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SalePriceList()
        {
              
            var vwCate = from a in db.itgroups where a.itgpunde != 0 where a.compcode == SessionMaster.CompCode orderby a.itgpname select new { a.itgpname, a.itgpcode };
            ViewBag.vwCategory = new SelectList(vwCate, "itgpcode", "itgpName");

            ViewBag.vwMstCode = (db.pricmsts.Where(aa => aa.compcode == SessionMaster.CompCode).Max(aa => (int?)aa.mstcode) ?? 0) + 1;


            return View();
        }
        [HttpPost]
        public ActionResult SavePriceList(String itm)
        {
            if (Convert.ToInt16(SessionMaster.CompCode) == 0) { Response.Redirect("~/Index/Index"); }

            int compcode1 = Convert.ToInt32(Session["compcode"]);
             clsSubmitModel oSubmit = new  clsSubmitModel();

            try
            {
                var json = itm;
                JsonItemFormData res = JsonConvert.DeserializeObject<JsonItemFormData>(json);

                int itemcode = Convert.ToInt32(res.itemcode);

                var countarray = res.Data.ToList();
                var countarray1 = countarray.Count();
                DateTime Opdate = oSubmit.GetDateFormat(res.dDate.ToString()).Date;
                 
                decimal dItemRate = 0;
                decimal dItemOpening = 0;

                int iMstCode = (db.pricmsts.Where(aa => aa.compcode == SessionMaster.CompCode).Max(aa => (int?)aa.mstcode) ?? 0) + 1;
                int iItemCode = 0;

                oSubmit.BeginTran();

                oSubmit.insertData("Exec InsPricMst @compcode = " + SessionMaster.CompCode + " ,@msttype= 571	, @mstcode	=" + iMstCode + " , @mstdate ='" + Opdate + "'");

                for (var i = 0; i < res.Data.Length; i++)
                {

                    if (res.Data[i].ItemRate != "") dItemRate = Convert.ToDecimal(res.Data[i].ItemRate); else dItemRate = 0;
                    if (res.Data[i].itemcode != "") iItemCode = Convert.ToInt32(res.Data[i].itemcode); else iItemCode = 0;
                    if (dItemRate > 0)
                    {
                        oSubmit.insertData("Exec InsPricItd @compcode =" + SessionMaster.CompCode + ",@itdtype	=571  ,@itdcode	=" + iMstCode + ",@itddate	= '" + Opdate + "'  ,@itditem	=" + iItemCode + " ,@itdquan =0  ,@itduni1 = 0 ,@itduni2	=0 ,@itdrate=" + dItemRate + "  ,@itdpack =0 ,@itdmill =0 ,@itduni3=0");
                    }
                }
                oSubmit.Commit();
                TempData["Msg"] = "Data has been saved successfully";

            }
            catch
            {
                oSubmit.RollBack();
                var redirectUrl1 = new UrlHelper(Request.RequestContext).Action("Index", "ItemSetup", new { companyId = "7e96b930-a786-44dd-8576-052ce608e38f" });
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ItemRateList(int subcat)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            var model = oSubmit.GetItemRate(subcat, SessionMaster.CompCode);

            return Json(model, JsonRequestBehavior.AllowGet);

        }

    //public ActionResult getItemRt(int ItemID)
    //    {
            
    //    var model = from a in db.itemains join b in db.pricitds new { Comp = } equals new {}

    //        return Json(model, JsonRequestBehavior.AllowGet);

    //    }
    }
}