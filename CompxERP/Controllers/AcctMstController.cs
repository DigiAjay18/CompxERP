//Sourabh Last Update 170817
//pick 170817
//Insert View 
using System;
using System.Linq;
using System.Web.Mvc;
using CompxERP.Models; 
using System.Data;
using System.IO;
using System.Collections.Generic;

namespace CRM.Controllers
{
    public class AcctMstController : Controller
    {
           
       ERPDataContext db = new ERPDataContext();
        //ACCILEntities dba = new ACCILEntities();
        public ActionResult Insert(int id=0)
        {
            AccountMaster act = new AccountMaster();
            List<SelectListItem> Type = new List<SelectListItem>();
            Type.Add(new SelectListItem { Text = "--Select--", Value = "-1" });
            ViewBag.FormReq = Type;
            act.compcode = SessionMaster.CompCode;
            if (id == 0)//Add Data in Account
            {
                var max = db.accounts.Max(i => i.acctcode);
                act.acctcode = Convert.ToInt16(max) + 1;
            }
            else//Edit Data in Account
            {
                act.acctcode =id;
            }
              
            CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel();
            DataTable dt;
            dt = oSubmit.GetData("select * from usermenust where mencode =" + Request.QueryString["MenCode"] + " and menuser ="+ SessionMaster.UserID );
            if (dt.Rows.Count > 0)
            {
                act.menaddi = Convert.ToBoolean(dt.Rows[0]["menaddi"]);
                act.menedit = Convert.ToBoolean(dt.Rows[0]["menedit"]);
            }
            
           

            return View(act);
            
        }
        [HttpPost]
        public ActionResult Insert(account act, string command, AccountMaster oCls)
        {
            if (command == "Save")
            { 
                CompxERP.Models.clsSubmitModel oSubmit = new CompxERP.Models.clsSubmitModel(); 
                DataTable dt;
                dt = oSubmit.GetData("select isnull(max(AcctCode) ,0 )+1 AcctCode from Account");
                act.acctcode  = Convert.ToInt32( dt.Rows[0]["AcctCode"]);

                act.compcode = int.Parse(Session["CompCode"].ToString());
                act.acctOwner = oCls.acctOwner;

                act.acctcldr = 0;
                act.acctclcr = 0;
                act.acctjmbl = -1;
                act.acctagen = 0;
                act.acctrema = act.CSTNo + ',' + act.PAN + ',' + act.TinNo + ',' + act.acctrema;
                act.acwithbl = 0;
                act.acctitrd = 0;
                act.acctdsap = 0;
                act.acctprty = 0;
                act.acctprbl = 0;
                act.acFarmerDC = 0;
                 
                if (oCls.groumain == "9" || oCls.groumain == "10" || oCls.groumain == "40" || oCls.groumain == "41")
                    act.acctpinc = oCls.acctpinc_Hearder;

                db.accounts.InsertOnSubmit(act);
                db.SubmitChanges();


                //********************************* User Work ********************************* 
                clsUserWork oUser = new clsUserWork();
                oUser.woruser = SessionMaster.UserID;
                oUser.wormode = 0;
                oUser.worcomp = SessionMaster.CompCode;
                oUser.wortype = 0;
                oUser.worcode = act.acctcode;
                oUser.worsrno = oSubmit.GetUsWoCode(); ;
                oUser.worrema = "D-" + oCls.acctname + "#" + oCls.acctgroup + "#" + oCls.acctphon;
                oUser.wordate = Convert.ToDateTime("04/01/1900");
                oUser.worrfsr = "";
                oUser.worsysn = Environment.MachineName;
                oSubmit.InsUserWork(oUser);
                //***************************************************************************  

                TempData["Msg"] = "Data Save Successfully";
                return RedirectToAction("Insert");
            }
            else
            {
                clsSubmitModel oSubmit = new clsSubmitModel();
                var res = from k in db.accounts where k.acctcode == act.acctcode select k;
                account temp = new account();// db.accounts.First(m => m.acctcode == id);
                switch (act.acctgrou)
                {
                    case 34://Agent
                        break;
                    case 24://Bank
                        act.acctarea = 0;
                        act.acctcity = 0;
                        act.acctstat = 0;
                        break;
                }
                temp.acctOwner = act.acctOwner;
                temp.acctgsta = act.acctgsta;
                temp.acctgstin = act.acctgstin;
                temp.acctname = act.acctname;
                temp.acctgrou = act.acctgrou;
                temp.acctcode = act.acctcode;
                temp.acctalia = act.acctalia;
                temp.acctrema = act.CSTNo + ',' + act.PAN + ',' + act.TinNo + ',' + act.acctrema;
                temp.acctsort = act.acctsort;
                temp.acctconp = act.acctconp;
                temp.acctdsa = act.acctdsa;
                temp.TinNo = act.TinNo;
                temp.PAN = act.PAN;
                temp.CSTNo = act.CSTNo;
                temp.acctPhone = act.acctPhone;
                temp.acctMob2 = act.acctMob2;
                temp.acctphon = act.acctphon;
                temp.accthind = act.accthind;
                temp.acctaddr = act.acctaddr;
                temp.acctarea = act.acctarea;
                temp.acctcity = act.acctcity;
                temp.acctstat = act.acctstat;
                temp.acctledg = act.acctledg;
                temp.PIN = act.PIN;
                temp.acctconp = act.acctconp;
                temp.acctcurr = act.acctcurr;
                temp.acctcate = act.acctcate;
                temp.acctpinc = act.acctpinc;
                temp.acctmail = act.acctmail;
                temp.acctfax = act.acctfax;
                temp.acctjmbl = act.acctjmbl;//-1
                temp.acctagen = act.acctagen;
                temp.acctitrd = act.acctitrd;//0;
                temp.acctrate = act.acctrate;//0
                temp.acformreq = act.acformreq;
                temp.acwithbl = 0;
                temp.acctcldr = 0;
                temp.acctclcr = 0;
                temp.acctprty = 0;
                temp.acctprbl = 0;
                temp.acFarmerDC = 0;
                temp.acctdsap = 0;

                if (oCls.groumain == "9" || oCls.groumain == "10" || oCls.groumain == "40" || oCls.groumain == "41")
                    temp.acctpinc = oCls.acctpinc_Hearder;

                temp.compcode = Convert.ToInt32(Session["compcode"]);
                oSubmit.UpdAccount(temp);


                //********************************* User Work ********************************* 
                clsUserWork oUser = new clsUserWork();
                oUser.woruser = SessionMaster.UserID;
                oUser.wormode = 1;
                oUser.worcomp = SessionMaster.CompCode;
                oUser.wortype = 0;
                oUser.worcode = act.acctcode;
                oUser.worsrno = oSubmit.GetUsWoCode(); ;
                oUser.worrema = "D-" + oCls.acctname + "#" + oCls.acctgroup + "#" + oCls.acctphon;
                oUser.wordate = Convert.ToDateTime("04/01/1900");//DateTime.Now.Date;
                oUser.worrfsr = "";
                oUser.worsysn = Environment.MachineName;
                oSubmit.InsUserWork(oUser);
                //*************************************************************************** 

                TempData["Msg"] = "Updated Successfully";
                return RedirectToAction("Insert");
            }
        }
       
        public ActionResult ShowAllAcctdetails()
        {
            //var res = from k in db.accounts //By Ajay On 21092017
            //          where k.compcode == int.Parse(Session["CompCode"].ToString())
            //          select k;

            var res = from k in db.accounts
      join prod in db.compexis on k.compcode equals prod.compunde
                      where prod.compcode == int.Parse(Session["CompCode"].ToString())
      select k;
             
            return View(res.ToList());
        }
      
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Checkotp(string username)
        {
            var Items = 0;
            //string conn = ConfigurationManager.ConnectionStrings["Mystring"].ConnectionString;
            //SqlConnection con = new SqlConnection(conn);
            //SqlCommand cmd = new SqlCommand("SELECT acctcode from account where acctname ='" + username + "'", con);
            //con.Open();
            //object values = cmd.ExecuteScalar();
            //con.Close();

            CompxERP.Models.clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;
            dt = oSubmit.GetData("SELECT acctcode from account where acctname ='" + username + "'");

            //   return values

            if (dt.Rows.Count > 0)
            {
                Items = 1;
            }
            else
            {
                Items = 0;
            }
            return Json(Items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemDet(int iItemID)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            //DataTable cities1;
            //cities1 = oSubmit.GetData("select unitdesc , unitsrno  from itemain a inner join uniconv b on a.ItemNo = b.UnitCode where itemcode = 1639 ");
            //cities1.TableName = "cities";
            // iItemID = 1618;
            var Items = iItemID;
            //from a in db.accounts
            ////   && c.compcode equals  d.compcode 
            //where a.acctcode == iItemID
            //select new { a.acctcode, a.acctname };
            return Json(Items, JsonRequestBehavior.AllowGet);
        } 
        public ActionResult GetAccountDet(int iCode)//170513
        {
            var Items = from a in db.accounts
                        join ac in db.acgroups on new { f2 = a.compcode, f1 = a.acctgrou } equals new { f2 = ac.compcode, f1 = ac.groucode }
                        join bl in db.accounts on new { f2 = a.compcode, f1 = a.acctledg } equals new { f2 = bl.compcode, f1 = (int?)bl.acctcode } into t4
                        from bel in t4.DefaultIfEmpty()
                        join x in db.citydets on new { f2 = 15, f1 = a.acctcity } equals new { f2 = x.cityType, f1 = (short?)x.citycode } into t1
                        from cit in t1.DefaultIfEmpty()
                        join ar in db.citydets on new { f2 = 60, f1 = a.acctarea } equals new { f2 = ar.cityType, f1 = (int?)ar.citycode } into t2
                        from are in t2.DefaultIfEmpty()
                        join st in db.citydets on new { f2 = 67, f1 = a.acctstat } equals new { f2 = st.cityType, f1 = (int?)st.citycode } into t3
                        from sta in t3.DefaultIfEmpty()
                        where a.acctcode == iCode && a.compcode == int.Parse(Session["CompCode"].ToString())
                        select new
                        {

                            acctcate=a.acctcate,
                            acformreq = a.acformreq,
                            acctjmbl = a.acctjmbl,
                            acctrate = a.acctrate,
                            acctagen = a.acctagen,
                            acctitrd = a.acctitrd,
                            acctname = a.acctname,//
                            acctgrou = a.acctgrou,//
                            acctcode = a.acctcode,
                            acctalia = a.acctalia,//
                            acctrema = a.acctrema,
                            acctsort = a.acctsort,//
                            acctconp = a.acctconp,//
                            acctdsa = a.acctdsa,//
                            TinNo = a.TinNo,
                            PAN = a.PAN,
                            CSTNo = a.CSTNo,
                            acctPhone = a.acctPhone,//
                            acctMob2 = a.acctMob2,//
                            acctphon = a.acctphon,//
                            accthind = a.accthind,
                            acctaddr = a.acctaddr,//
                            acctarea = a.acctarea,//
                            acctcity = a.acctcity,//
                            acctstat = a.acctstat,//
                            acctledg = a.acctledg,//
                            PIN = a.PIN,
                            acctcurr = a.acctcurr,//
                            acctfax = a.acctfax,//
                            acctmail = a.acctmail,//
                            acctcform = a.acctcform,//
                            acctpinc = a.acctpinc,//
                            acctareaname = are.cityname,//
                            Cityname = cit.cityname, //Cityname = "",  Ajay On 22092017 Open Comment
                            Statename = sta.cityname,//
                            acctledgname = bel.acctname,//
                            acctgroup = ac.grouname,//
                            groucode = ac.groucode,
                            grouaddr = ac.grouaddr,
                            groumain = ac.groumain,
                            grourepo = ac.grourepo,
                            acctgstin = a.acctgstin,//
                            acctgsta = a.acctgsta,//
                            acctCntry = a.acctCntry,//
                            acctOwner = a.acctOwner//
                        };

            
 return Json(Items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAccountGroupDet(int iItemID)//170513
        {
            var Items = from a in db.acgroups
                        where a.groucode == iItemID && a.compcode == int.Parse(Session["CompCode"].ToString())
                        select new
                        {
                            groucode = a.groucode,
                            grouaddr = a.grouaddr,
                            groumain = a.groumain,
                            grourepo = a.grourepo,
                        };
            //            JObject o1 = new JObject();
            //            o1 =JObject.Parse(Json(Items, JsonRequestBehavior.AllowGet).ToString());
            //            o1.Merge(o2, new JsonMergeSettings
            //13{
            //                14    // union array values together to avoid duplicates
            //15    MergeArrayHandling = MergeArrayHandling.Union
            //16});

            return Json(Items, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetAccountDropDownDet(string grouaddr, string groumain, string grourepo)
        {
            AccountMaster am = new AccountMaster();
            List<SelectListItem> acctcateData1 = new List<SelectListItem>();
            List<SelectListItem> acformreqData = new List<SelectListItem>();
            List<SelectListItem> acctjmblData = new List<SelectListItem>();
            List<SelectListItem> acctitrdData = new List<SelectListItem>();
            List<SelectListItem> acctgstaData = new List<SelectListItem>();

            acctcateData1.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            acformreqData.Add(new SelectListItem { Text = "--Select--", Value = "-1" });
            acctjmblData.Add(new SelectListItem { Text = "--Select--", Value = "-1" });
            acctitrdData.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            acctgstaData.Add(new SelectListItem { Text = "--Select--", Value = "0" });

            if (grouaddr == "true")
            {
                var catItem = (from c in db.studdet1s
                               where c.studType == 376
                               select new
                               {
                                   Text = c.studName,
                                   Value = c.studCode
                               }).ToList().Select(u => new SelectListItem
                               {
                                   Text = u.Text,
                                   Value = u.Value.ToString()
                               });
                acctcateData1 = catItem.ToList();
            }
            if (grourepo == "-30001" || grourepo == "30001" || grourepo == "-40001")
            {
                var catItem1 = (from c in db.studdets
                                where c.studType == 379
                                select new
                                {
                                    Text = c.studName,
                                    Value = c.studCode
                                }).ToList().Select(u => new SelectListItem
                                {
                                    Text = u.Text,
                                    Value = u.Value.ToString()
                                });
                acctitrdData = catItem1.ToList();
            }
            switch (groumain)
            {
                case "9":
                    acformreqData = GetFormReqList();
                    break;
                case "10":
                    acformreqData = GetFormReqList();
                    break;
                case "11":
                    acctjmblData.Add(new SelectListItem { Text = "A", Value = "31" });
                    acctjmblData.Add(new SelectListItem { Text = "B", Value = "32" });
                    break;
                case "12":
                    acctjmblData.Add(new SelectListItem { Text = "A", Value = "31" });
                    acctjmblData.Add(new SelectListItem { Text = "B", Value = "32" });
                    break;
                case "13":
                    acctjmblData.Add(new SelectListItem { Text = "A", Value = "31" });
                    acctjmblData.Add(new SelectListItem { Text = "B", Value = "32" });
                    break;
                case "14":
                    acctjmblData.Add(new SelectListItem { Text = "A", Value = "31" });
                    acctjmblData.Add(new SelectListItem { Text = "B", Value = "32" });
                    break;
                case "21":
                    acctjmblData.Add(new SelectListItem { Text = "{None}", Value = "0" });
                    acctjmblData.Add(new SelectListItem { Text = "Loan", Value = "120" });
                    acctjmblData.Add(new SelectListItem { Text = "Deposite", Value = "121" });
                    acctjmblData.Add(new SelectListItem { Text = "Anivary Shulk", Value = "122" });
                    break;
                case "23":
                    acctjmblData.Add(new SelectListItem { Text = "No", Value = "0" });
                    acctjmblData.Add(new SelectListItem { Text = "Yes", Value = "1" });
                    break;
                case "24":
                    acctjmblData.Add(new SelectListItem { Text = "No", Value = "0" });
                    acctjmblData.Add(new SelectListItem { Text = "Yes", Value = "1" });
                    break;
                case "30":
                    acctjmblData.Add(new SelectListItem { Text = "{None}", Value = "0" });
                    acctjmblData.Add(new SelectListItem { Text = "P.F.", Value = "9" });
                    acctjmblData.Add(new SelectListItem { Text = "E.S.I.", Value = "10" });
                    break;
                case "33":
                    acctjmblData.Add(new SelectListItem { Text = "{None}", Value = "0" });
                    acctjmblData.Add(new SelectListItem { Text = "P.F.", Value = "9" });
                    acctjmblData.Add(new SelectListItem { Text = "E.S.I.", Value = "10" });
                    break;
                case "36": /*$('#dvacctitrd').show();*/ break;
                case "37"://Tax
                  acctjmblData=  GetAcctJmblList();
                     var catItem = (from c in db.tblHardCodes
                                    where c.hrcdType == "GstSubType"
                               select new
                               {
                                   Text = c.hrcdName,
                                   Value = c.hrcdNameID
                               }).ToList().Select(u => new SelectListItem
                               {
                                   Text = u.Text,
                                   Value = u.Value.ToString()
                               });
                acctgstaData= catItem.ToList();
                    //acctjmblData.Add(new SelectListItem { Text = "A", Value = "31" }); acctjmblData.Add(new SelectListItem { Text = "B", Value = "32" }); for specific conditions
                    break;
                case "40":
                    acformreqData = GetFormReqList();
                    break;
                case "41":
                    acformreqData = GetFormReqList();
                    break;
                case "43":
                    acctjmblData.Add(new SelectListItem { Text = "No", Value = "0" });
                    acctjmblData.Add(new SelectListItem { Text = "Yes", Value = "1" });
                    break;
                case "45":
                    acctjmblData.Add(new SelectListItem { Text = "{None}", Value = "0" });
                    acctjmblData.Add(new SelectListItem{Text = "Percent Discount",Value = "41"});
                    acctjmblData.Add(new SelectListItem{Text = "Qty. Discount",Value = "42"});
                    break;
            }
            am.acctcateData = acctcateData1;
            am.acformreqData = acformreqData;
            am.acctjmblData = acctjmblData;
            am.acctitrdData = acctitrdData;
            am.acctgstaData = acctgstaData;
            return Json(am, JsonRequestBehavior.AllowGet);
        }
        private List<SelectListItem> GetFormReqList()
        {
            List<SelectListItem> acData = new List<SelectListItem>();
            //public void FillDropDownByTextDoc(DropDownList ddlName, string sSetupFitxtfile, HttpServerUtility Server)
            //{
            using (StreamReader sr = new StreamReader(Server.MapPath("~") + @"\Views\Shared\SetupFi\formtype.txt"))
            {
                //int index = 0;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    //ddlName.Items.Insert(index, new ListItem(line, index.ToString()));
                    acData.Add(new SelectListItem { Text = line.Split('=')[0].Trim(), Value = line.Split('=')[1].Trim() });
                    //index++;
                }
            }
            //}}
            return acData;
        }
      
    }
} 
