using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

using System.Data;
using System.Web;
using System.Web.Mvc;
using CompxERP.Models;
using System.Web.Security;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace CompxERP.Controllers
{
    public class APIController : ApiController
    {
        ERPDataContext dba = new ERPDataContext();

        [System.Web.Http.HttpPost]
        public JsonResult PostSRV_Login([FromUri] LoginModel login)
        {
            int UserID = 0;
            string UserNM = "";
            int CompCode = 0;
            int UserType = 0;
            int DesgType = 0;

            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;

            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("use_name")) login.use_name = headers.GetValues("use_name").First();
            if (headers.Contains("use_pass")) login.use_pass = headers.GetValues("use_pass").First();

            dt = oSubmit.GetData("sp_AreaSignUp @useId='" + login.use_name.Trim() + "',@usePass='" + login.use_pass + "'", true);
            if (dt != null && dt.Rows.Count > 0)
            {
                UserID = Convert.ToInt32(dt.Rows[0]["UseCode"]);
                UserNM = dt.Rows[0]["UseName"].ToString();
                CompCode = Convert.ToInt32(dt.Rows[0]["compcode"]);
                UserType = Convert.ToInt32(dt.Rows[0]["UseType"]);

            }

            //List<string> sItem = new List<string> { UserID.ToString(), CompCode.ToString(), UserNM, UserType.ToString() };

            return new JsonResult()
            {
                Data = new List<loginusers>() { new loginusers() { usecode = UserID, CompCode = CompCode, usename = UserNM, usetype = UserType, usepass = "" } },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }

        [System.Web.Http.HttpPost]
        public JsonResult SRV_InsUser([FromBody] loginusers login)
        {
            string Err = "1";
            int ErrCode = 10;
            //int itype = 1;
            try
            {
                //int oPropusetype = 0;
                int useshowtr = 0;
                int useref = 0;

                DataTable dt;
                clsSubmitModel oSubmit = new clsSubmitModel();
                ErrCode = 1;
                if (Convert.ToInt32(oSubmit.GetSingleData("Select count(*)count from LoginUsers where UseName ='" + login.usename + "'", "0", true)) > 0)
                {
                    Err += "Success";
                    Err += "1";
                    ErrCode = 1;
                }
                else
                {
                    ErrCode = 1;
                    dt = oSubmit.GetData("select isnull(Max(usecode),0) +1 usecode from [loginusers]", true);

                    oSubmit.spInsUser(login.CompCode, Convert.ToInt32(dt.Rows[0]["usecode"].ToString()), login.usetype, useshowtr, login.usename, login.usepass, useref, login.UserNM, login.Remark, login.UsePhone, login.UseEmail, login.UseDesi, login.UseDepa, login.UseDealer);
                    Err = "2";

                    tblComplaint oComp = new tblComplaint();
                    int iCustID = 0;

                    iCustID = Convert.ToInt32(oSubmit.GetSingleData("Select isnull(max(studCode)+1,1) from studdet where studType = 68", "0", true));
                    studdet Cust = new studdet();
                    Cust.studType = 68;
                    Cust.studCode = iCustID;
                    Cust.studName = login.UserNM;// login.usename;
                    Cust.studadd1 = login.Remark;
                    Cust.studadd2 = login.UseEmail;
                    Cust.studphon = login.UsePhone;
                    Cust.studshcdvl = Convert.ToInt32(dt.Rows[0]["usecode"].ToString());

                    ERPDataContext db = new ERPDataContext();
                    Err = "pp";

                    db.studdets.InsertOnSubmit(Cust);
                    db.SubmitChanges();
                    Err = "mm";

                    oComp.CustID = iCustID;

                    ErrCode = 2;
                }
            }
            catch (Exception ex)
            {
                ErrCode = 3;
                Err += ex.Message;
            }

            var ITEMS = new List<Result>() { new Result() { ID = ErrCode, Msg = Err } };
            return new JsonResult()
            {
                Data = new { ITEMS },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }

        [System.Web.Http.HttpPost]
        public JsonResult InsOrder([FromBody] Jourmaster oCls)
        {
            //OrderMaster
            string Msg = "pp";
            clsSubmitModel oSubmit = new clsSubmitModel();
            commFunction oCom = new commFunction();
            string sItmTbl = "ordeItd";
            DataTable dt2;

            try
            {
                jourtrn oTrn = new jourtrn();
                sapuitd oSapu = new sapuitd();
                gathdet oGath = new gathdet();


                Msg += oCls.compcode.ToString();
                Msg += oCls.msttype.ToString();
                Msg += oCls.mstcode.ToString();
                Msg += "Done@";

                if (oCls.mstcode != null && oCls.mstcode > 0) { }
                else
                {
                    Msg += "1111";
                    oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from Ordemst where compcode ='" + oCls.compcode + "' and msttype ='" + oCls.msttype + "'"));
                    Msg += "1111222" + oCls.mstcode.ToString();
                }
                Msg += "2";
                var json = oCls.sItemDet;
                Msg += "2w";
                clsPoItem ItemDet = JsonConvert.DeserializeObject<clsPoItem>(json);
                Msg += "3";
                int iDays = 0;
                decimal iComm = 0, iInterest = 0;

                oCls.mstdate = oSubmit.GetDateFormat(oCls.sMstdate);

                if (oCls.mstdued > 0) iDays = (int)oCls.mstdued;//DueDay 170831
                if (oCls.Comm > 0) iComm = oCls.Comm;
                if (oCls.Interest > 0) iInterest = oCls.Interest;

                if (oCls.mstchnH == null) oCls.mstchnH = "";
                oCls.mstdepa = oCls.mststat = 0;
                Msg += "4";
                if (oCls.msttype != 1163)
                {
                    oCls.mstDueDate = oSubmit.GetDateFormat(oCls.ChallanDate);  //oCls.mstdate.AddDays(iDays);//170831
                    oCls.mstrefc = getParty(Convert.ToInt32(oCls.PartyID)) + "~" + iInterest + "~" + iDays + "~" + iComm;
                }

                oCls.mstchno = oCls.mstchnH + oCls.mstchnV;//170830
                oCls.mstexti = "~~#0";//170830
                Msg += "5";
                oCls.mstreftag = oCls.mstchno + "~0~0~0~0~0~0~0";

                oCls.mstgncd = "0~0~" + oCls.acctledg + "~0";//170830

                oCls.mstAppr = 0; oCls.mstqtyd = 0; oCls.mstvat1 = 0; oCls.mstvat2 = 0;
                oCls.mstvat3 = 0; oCls.mstchnm = ""; oCls.oldwht = 0; oCls.mstsite = 0;
                oCls.mstbrnc = 0; oCls.mstsubt = 0;  //oCls.mstrvsc = 0;
                oCls.mstcust = oCls.PartyID; // =oCls.mstprtc 
                oCls.msternv = oCls.acctname;
                Msg += "6";
                oCls.mstcurrcd = 1;
                oCls.mstcurrrat = 1;
                oCls.mstintr = 0;

                oCls.mstbuyerc = 0; oCls.mstperd = 0; oCls.mstdsptoc = 0;
                //if (ItemDet.LstItem.Count > 0 && ItemDet.LstItem[0].GSTStateVal == "1")
                //    oCls.mstIorL = "I";
                //else
                //    oCls.mstIorL = "L";



                oCls.mstlotno = oCls.EMail;
                oCls.mstsection = oCls.NewMobile;
                oCls.mstuser = oCls.UserID;
                Msg += "7";
                oSubmit.BeginTran();

                oSubmit.InsOrdeMst(oCls);
                Msg += "8";
                oSubmit.insertData("Delete from " + sItmTbl + " where CompCode = " + oCls.compcode + " and Itdtype = " + oCls.msttype + " and Itdcode = " + oCls.mstcode);
                Msg += "9";
                for (int i = 0; i < ItemDet.LstItem.Count; i++)
                {

                    oSapu.compcode = Convert.ToInt16(oCls.compcode);
                    oSapu.itdtype = Convert.ToInt32(oCls.msttype);
                    oSapu.itdcode = Convert.ToInt32(oCls.mstcode);
                    oSapu.itdtime = Convert.ToInt32(oCls.msttime);
                    oSapu.itdsrno = Convert.ToInt16(i + 1);
                    oGath.gathpuri = oGath.gathstat = Convert.ToString(oSapu.itddate = Convert.ToDateTime(oCls.mstdate));

                    if (ItemDet.LstItem[i].itdmill != null && ItemDet.LstItem[i].itdmill > 0) oSapu.itdmill = Convert.ToInt32(ItemDet.LstItem[i].itdmill); oSapu.itdmill = 0;
                    if (ItemDet.LstItem[i].itdgodo != null && ItemDet.LstItem[i].itdgodo > 0) oSapu.itdgodo = Convert.ToInt32(ItemDet.LstItem[i].itdgodo); oSapu.itdgodo = 0;

                    oSapu.itditem = Convert.ToInt32(ItemDet.LstItem[i].itditem);
                    oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].itdquan);
                    oSapu.itdrate = Convert.ToInt32(ItemDet.LstItem[i].itdrate);
                    if (ItemDet.LstItem[i].Sitdexpd != null && ItemDet.LstItem[i].Sitdexpd != "") oSapu.itdPODt = oSubmit.GetDateFormat(ItemDet.LstItem[i].Sitdexpd);
                    oSapu.itdrema = ItemDet.LstItem[i].itdrema.ToString();
                    Msg = "10" + i;
                    oSubmit.insOrdeItd(oSapu, sItmTbl);
                    Msg = "11" + i;
                }
                Msg += "12";
                if (oCls.IsEdit != true)
                {
                    oCls.StDate = Convert.ToDateTime(oCom.getOpenDate(DateTime.Now.Date));
                    oCls.LastDate = Convert.ToDateTime(oCom.getClosDate(DateTime.Now.Date));
                    oSubmit.UpdCodeGen(oCls);
                }
                Msg += "13";
                oSubmit.Commit();
                Msg += "14";
                //ID = 2;
                Msg += "Saved Successfully ...";

                dt2 = oSubmit.GetData("SELECT count(*)+1 Total from OrdeMst where compcode =" + oCls.compcode + " and msttype = " + oCls.msttype + " and Mstuser = " + oCls.UserID);
                oCls.mstprtc = Convert.ToInt32(dt2.Rows[0]["Total"].ToString());
                oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from Ordemst where compcode ='" + oCls.compcode + "' and msttype ='" + oCls.msttype + "'"));
                //IsSave = 1;
            }
            catch (Exception ex)
            {
                Msg = Msg + ex.Message;
            }

            return new JsonResult()
            {
                Data = new List<string>() { Msg },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }


        //[ResponseType(typeof(loginuser))]
        //[System.Web.Http.HttpPost]
        //public IHttpActionResult InsUser([FromUri] loginusers login)
        //{
        //    string Err = "1";
        //    try
        //    {

        //        var re = Request;
        //        var headers = re.Headers;

        //        //if (headers.Contains("usename"))
        //        //{
        //        //    login.usename = headers.GetValues("usename").First();
        //        //}
        //        if (headers.Contains("usecode")) login.usecode = Convert.ToInt32(headers.GetValues("usecode").First());
        //        if (headers.Contains("usetype")) login.usetype = Convert.ToInt32(headers.GetValues("usetype").First());
        //        if (headers.Contains("CompCode")) login.CompCode = Convert.ToInt32(headers.GetValues("CompCode").First());
        //        if (headers.Contains("usename")) login.usename = headers.GetValues("usename").First();
        //        if (headers.Contains("usepass")) login.usepass = headers.GetValues("usepass").First();
        //        if (headers.Contains("UserNM")) login.UserNM = headers.GetValues("UserNM").First();
        //        if (headers.Contains("Remark")) login.Remark = headers.GetValues("Remark").First();

        //        if (headers.Contains("UsePhone")) login.UsePhone = headers.GetValues("UsePhone").First();
        //        if (headers.Contains("UseEmail")) login.UseEmail = headers.GetValues("UseEmail").First();
        //        if (headers.Contains("UseDesi")) login.UseDesi = Convert.ToInt32(headers.GetValues("UseDesi").First());
        //        if (headers.Contains("UseDepa")) login.UseDepa = Convert.ToInt32(headers.GetValues("UseDepa").First());
        //        if (headers.Contains("UseDealer")) login.UseDealer = Convert.ToInt32(headers.GetValues("UseDealer").First());

        //        DataTable dt;
        //        clsSubmitModel oSubmit = new clsSubmitModel();

        //        if (Convert.ToInt32(oSubmit.GetSingleData("Select count(*)count from LoginUsers where UseName ='" + login.usename + "'", "0", true)) > 0)
        //        { Err = "3"; }
        //        else
        //        {
        //            dt = oSubmit.GetData("select isnull(Max(usecode),0) +1 usecode from [loginusers]", true);
        //            oSubmit.spInsUser(login.CompCode, Convert.ToInt32(dt.Rows[0]["usecode"].ToString()), login.usetype, 0, login.usename, login.usepass, 0, login.UserNM, login.Remark, login.UsePhone, login.UseEmail, login.UseDesi, login.UseDepa, login.UseDealer);
        //            Err = "2";
        //        }

        //        // Err = login.usename;
        //    }
        //    catch (Exception ex)
        //    {
        //        Err = ex.Message;
        //    }
        //    return Ok(Err);
        //}


        //[ResponseType(typeof(loginuser))]
        //[System.Web.Http.HttpPost]
        //public IHttpActionResult InsUser_c([FromBody] loginusers login)
        //{
        //    string Err = "1";
        //    try
        //    {

        //        //var re = Request;
        //        //var headers = re.Headers;

        //        //if (headers.Contains("usecode")) login.usecode = Convert.ToInt32(headers.GetValues("usecode").First());
        //        //if (headers.Contains("usetype")) login.usetype = Convert.ToInt32(headers.GetValues("usetype").First());
        //        //if (headers.Contains("CompCode")) login.CompCode = Convert.ToInt32(headers.GetValues("CompCode").First());
        //        //if (headers.Contains("usename")) login.usename = headers.GetValues("usename").First();
        //        //if (headers.Contains("usepass")) login.usepass = headers.GetValues("usepass").First();
        //        //if (headers.Contains("UserNM")) login.UserNM = headers.GetValues("UserNM").First();
        //        //if (headers.Contains("Remark")) login.Remark = headers.GetValues("Remark").First();

        //        //if (headers.Contains("UsePhone")) login.UsePhone = headers.GetValues("UsePhone").First();
        //        //if (headers.Contains("UseEmail")) login.UseEmail = headers.GetValues("UseEmail").First();
        //        //if (headers.Contains("UseDesi")) login.UseDesi = Convert.ToInt32(headers.GetValues("UseDesi").First());
        //        //if (headers.Contains("UseDepa")) login.UseDepa = Convert.ToInt32(headers.GetValues("UseDepa").First());
        //        //if (headers.Contains("UseDealer")) login.UseDealer = Convert.ToInt32(headers.GetValues("UseDealer").First());

        //        DataTable dt;
        //        clsSubmitModel oSubmit = new clsSubmitModel();

        //        if (Convert.ToInt32(oSubmit.GetSingleData("Select count(*)count from LoginUsers where UseName ='" + login.usename + "'", "0", true)) > 0)
        //        { Err = "3"; }
        //        else
        //        {
        //            dt = oSubmit.GetData("select isnull(Max(usecode),0) +1 usecode from [loginusers]", true);
        //            oSubmit.spInsUser(login.CompCode, Convert.ToInt32(dt.Rows[0]["usecode"].ToString()), login.usetype, 0, login.usename, login.usepass, 0, login.UserNM, login.Remark, login.UsePhone, login.UseEmail, login.UseDesi, login.UseDepa, login.UseDealer);
        //            Err = "2";
        //        }

        //        //Err = login.usename;
        //    }
        //    catch (Exception ex)
        //    {
        //        Err = ex.Message;
        //    }
        //    return Ok(Err);
        //}


        //   [ResponseType(typeof(LoginModel))] 
        //[System.Web.Http.HttpGet]
        //public IHttpActionResult ChkLogin([FromUri] LoginModel login)
        //{
        //    int UserID = 0;
        //    string UserNM = "";
        //    int CompCode = 0;
        //    int UserType = 0;

        //    clsSubmitModel oSubmit = new clsSubmitModel();
        //    DataTable dt;
        //    dt = oSubmit.GetData("sp_AreaSignUp @useId='" + login.use_name.Trim() + "',@usePass='" + login.use_pass + "'", true);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        UserID = Convert.ToInt32(dt.Rows[0]["UseCode"]);
        //        UserNM = dt.Rows[0]["UseName"].ToString();
        //        CompCode = Convert.ToInt32(dt.Rows[0]["compcode"]);
        //        UserType = Convert.ToInt32(dt.Rows[0]["UseType"]);
        //    }

        //    var Data = new List<loginusers>() { new loginusers() { usecode = UserID, CompCode = CompCode, usename = UserNM, usetype = UserType, usepass = "" } };
        //    return this.Ok(Data);
        //}

        [System.Web.Http.HttpPost]
        public ActionResult InsDistributor([FromBody] Distributor Dis)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();

            DataTable dt;
            dt = oSubmit.GetData("select isnull(Max(MstCode),0) +1 MstCode from tblDistributor");
            int MstCode = Convert.ToInt32(dt.Rows[0]["MstCode"].ToString());

            if (Dis.hdnDealerCode != null && Dis.hdnDealerCode != "")
                MstCode = Convert.ToInt32(Dis.hdnDealerCode);

            oSubmit.InsDistributor(MstCode, Dis.Dis_Date, Dis.Dis_Country, Dis.Dis_State, Dis.Dis_City, Dis.Dis_Add1, Dis.Dis_Add2, Dis.Dis_Add3, Dis.Dis_pobox, Dis.Dis_City_I, Dis.DistributorID, Dis.Dis_Name, Dis.ContactPerson, Dis.Mob1, Dis.Mob2, Dis.LandLine, Dis.Email, Dis.Skype, Dis.GSTIN, Dis.usetype, Dis.usename, Dis.usepass, Dis.DealCode, Dis.CityNM, Dis.UserID);

            dt = oSubmit.GetData("select (select count(*)+1 from tblDistributor where DistributorID <> 0)Dealer,(select count(*)+1 from tblDistributor where DistributorID = 0)Distributor");

            var ITEMS = new { Dis_ID = dt.Rows[0]["Distributor"].ToString(), Deal_ID = dt.Rows[0]["Dealer"].ToString() };

            return new JsonResult()
            {
                Data = new { ITEMS },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [System.Web.Http.HttpGet]
        public JsonResult FillDistributor()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;
            string query = "select DisName Name,MstCode Code from tblDistributor where DistributorID = 0 ";
            dt = oSubmit.GetData(query);

            var result = new List<KeyValuePair<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new KeyValuePair<string, string>(dr["Code"].ToString(), dr["Name"].ToString()));
            }
            var result3 = result.Where(s => s.Value.ToLower().Contains(query.ToLower())).Select(w => w).ToList();

            return new JsonResult()
            {
                Data = new { result },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }
        [System.Web.Http.HttpGet]
        public ActionResult FillDealer(int DistributorID, int CityID = 0)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> rowelement;

            DataTable dt;

            string sCity = "";
            if (CityID > 0) sCity = " and CityID_I =" + CityID;

            if (DistributorID > 0)
                dt = oSubmit.GetData("select MstCode ,DealCode ,DisName ,ContactPerson ,Mob1,Mob2,Email,Add_I ,Skype ,CityNM City ,UseCode ,acctCode  from tblDistributor a inner join Account b on a.MstCode = b.AcctDeal where DistributorID =" + DistributorID + sCity);
            else
                dt = oSubmit.GetData("select MstCode ,DealCode ,DisName ,ContactPerson ,Mob1,Mob2,Email,Add_I ,Skype ,CityNM City ,UseCode,acctCode from tblDistributor a inner join Account b on a.MstCode = b.AcctDeal where DistributorID <>0 " + sCity);


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

            return new JsonResult()
            {
                Data = new { rows },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult FillDistributorDet(int MstCode)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            var model = oSubmit.GetDistributor("select * from tblDistributor where MstCode =" + MstCode);

            return new JsonResult()
            {
                Data = new { model },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }


        //[System.Web.Http.HttpPost]
        //public JsonResult InsNotification([FromUri] clsNotify oNotifi)
        //{
        //    EmpmstDataContext oDB = new EmpmstDataContext();
        //    tblNotification oNotification = new tblNotification();

        //    oNotification.BrandID = oNotifi.BrandID;
        //    oNotification.ID = oNotifi.ID;
        //    oNotification.For = oNotifi.For;
        //    oNotification.BrandID = oNotifi.BrandID;
        //    oNotification.CityID = oNotifi.CityID;
        //    oNotification.NameID = oNotifi.NameID;
        //    oNotification.HeaderID = oNotifi.HeaderID;
        //    oNotification.Msg = oNotifi.Msg;
        //    oNotification.From = oNotifi.From;
        //    oNotification.To = oNotifi.To;

        //    if (oNotifi.ID == null || oNotifi.ID == 0)
        //    {
        //        oNotification.ID = (oDB.tblNotifications).Max(a => a.ID) + 1;
        //        oDB.tblNotifications.InsertOnSubmit(oNotification);
        //    }
        //    //else
        //    //{
        //    //    tblNotification obj = (from a in oDB.tblNotifications where a.ID == 1 select a).SingleOrDefault();
        //    //    obj.Msg = oNotifi.Msg.ToString();
        //    //}

        //    oDB.SubmitChanges();

        //    var ITEMS = new { Dis_ID = 1 };
        //    //return oNotifi;
        //    return new JsonResult()
        //    {
        //        Data = new { ITEMS },
        //        ContentType = "application/json; charset=utf-8",
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        //        MaxJsonLength = Int32.MaxValue
        //    };
        //}
        [System.Web.Http.HttpGet]
        public ActionResult Chk_User(string sUser)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();

            return new JsonResult()
            {
                Data = new { Count = oSubmit.GetData("Select count(*)count from LoginUsers where UseName ='" + sUser + "'", true) },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [System.Web.Http.HttpGet]
        public JsonResult getSummary()
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;
            dt = oSubmit.GetData("select (select count(*) from tblDistributor where DistributorID <> 0)Dealer, (select count(*) from tblDistributor where DistributorID = 0)Distributor, (select count(*)  from loginusers where useType = 8)Employee, (select count(*)  from loginusers where useType = 9)Client ");

            var ITEMS = new { Dis_ID = dt.Rows[0]["Distributor"].ToString(), Deal_ID = dt.Rows[0]["Dealer"].ToString(), Employee = dt.Rows[0]["Employee"].ToString(), Client = dt.Rows[0]["Client"].ToString() };

            return new JsonResult()
            {
                Data = new { ITEMS },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [System.Web.Http.HttpGet]
        public ActionResult GetItemInfo(int comp)//170513
        {

            var Items = (from a in dba.itemains

                         join z in dba.studdets on new { f2 = (int)a.itemnumb, f1 = 61 } equals new { f2 = z.studCode, f1 = z.studType } into t1
                         from y in t1.DefaultIfEmpty()
                         where a.compcode == comp //&& a.itemgrou == subcat
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

            return new JsonResult()
            {
                Data = new { Items },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [System.Web.Http.HttpGet]
        public ActionResult GetUnit(int comp)//170513
        {
            //var res = dba.Database.SqlQuery<unitdet>("Select unitcode,unitname from dbo.unitdet where compcode='" + comp + "'");
            var res = from a in dba.unitdets where a.compcode == 62 select new { a.unitcode, a.unitname };

            return new JsonResult()
            {
                Data = new { res },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [System.Web.Http.HttpPost]
        public ActionResult ParkLead([FromBody]  jourmst oCls)
        {

            var re = Request;
            var headers = re.Headers;
            string sMstChNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");
            int IsSave = 1;
            clsSubmitModel oSubmit = new clsSubmitModel();
            commFunction oCom = new commFunction();
            int ID = 0;
            int UserID = 0;
            string sGathCode = ""; string MachineName = ""; string IPAdd = "";

            string Msg = "pp";
            try
            {
                Jourmaster oMst = new Jourmaster();
                jourtrn oTrn = new jourtrn();
                sapuitd oSapu = new sapuitd();
                gathdet oGath = new gathdet();

                //string sGathCode = "";
                string sItmTbl = "ordeItd";
                string GSTStateVal = "";
                DataTable dt2;
                int iEmpoID = Convert.ToInt32(oSubmit.GetSingleData("Select isNull(AcctCode ,0 ) ,* from Employee Where UseCode = " + oCls.mstexec, "0", true));
                DataTable dtState;
                dtState = oSubmit.GetData("select isNull(acctcform,0)acctcform from Account Where acctCode = " + oCls.PartyID);
                if (dtState.Rows.Count > 0)
                {
                    if (dtState.Rows[0]["acctcform"].ToString() != "") GSTStateVal = dtState.Rows[0]["acctcform"].ToString(); else GSTStateVal = "0";
                }

                if (GSTStateVal == "1")
                    oCls.mstIorL = "I";
                else
                    oCls.mstIorL = "L";

                Msg += "1";
                Msg += oCls.mstcode.ToString();
                if (oCls.mstcode != null && oCls.mstcode > 0) { Msg += "N"; }
                else
                {
                    Msg += "1111";
                    oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from Ordemst where compcode ='" + oCls.compcode + "' and msttype ='" + oCls.msttype + "'"));
                    Msg += "1111222" + oCls.mstcode.ToString();
                }

                Msg += "A";
                var json = "";
                if (headers.Contains("sMstdate"))
                {
                    Msg += "dt1";
                    if (headers.Contains("sMstdate"))
                    {
                        Msg += "d_1";
                        oCls.mstdate = oSubmit.GetDateFormat(headers.GetValues("sMstdate").First());
                        Msg += "d_2";
                    }
                    Msg += "dt2";
                }
                Msg += "B";

                if (headers.Contains("sItemDet")) json = headers.GetValues("sItemDet").First();
                if (headers.Contains("acctname")) oCls.msternv = headers.GetValues("acctname").First();
                if (headers.Contains("acctledg")) oCls.mstgncd = "0~0~" + headers.GetValues("acctledg").First() + "~0";
                if (headers.Contains("EMail")) oCls.mstlotno = headers.GetValues("EMail").First();
                if (headers.Contains("NewMobile")) oCls.mstsection = headers.GetValues("NewMobile").First();
                if (headers.Contains("UserID")) { if (headers.GetValues("UserID").First() != "") oCls.mstexec = Convert.ToInt32(headers.GetValues("UserID").First()); }
                if (headers.Contains("MachineName")) MachineName = headers.GetValues("MachineName").First();
                if (headers.Contains("IPAdd")) IPAdd = headers.GetValues("IPAdd").First();

                //var json = oCls.sItemDet;
                Msg += "2w";
                clsPoItem ItemDet = JsonConvert.DeserializeObject<clsPoItem>(json);
                Msg += "3";
                int iDays = 0;

                //oCls.mstdate = oSubmit.GetDateFormat(oCls.sMstdate);

                if (oCls.mstdued > 0) iDays = (int)oCls.mstdued;//DueDay 170831

                if (oCls.mstchnH == null) oCls.mstchnH = "";
                oCls.mstdepa = oCls.mststat = 0;
                Msg += "4";

                // oCls.mstchno = oCls.mstchnH + oCls.mstchnV;//170830
                dt2 = oSubmit.GetData("select isnull(max(right(mstchno,3))+1,1) as maxchno  from OrdeMst where msttype = " + oCls.msttype + " and  left(mstchno, 6) = '" + sMstChNo + "'", true);
                oCls.mstchno = sMstChNo + GetVoucherNo(dt2.Rows[0]["maxchno"].ToString());

                oCls.MSTEXTI = "~~#0";//170830
                Msg += "5";
                oCls.mstreftag = oCls.mstchno + "~0~0~0~0~0~0~0";

                //oCls.mstgncd = "0~0~" + oCls.acctledg + "~0";//170830

                oCls.mstappr = 0; oCls.mstqtyd = 0; oCls.mstvat1 = 0; oCls.mstvat2 = 0;
                oCls.mstvat3 = 0; oCls.mstchnm = ""; oCls.oldwht = 0; oCls.mstsite = 0;
                oCls.mstbrnc = 0; oCls.mstsubt = 0;  //oCls.mstrvsc = 0;
                oCls.mstcust = oCls.PartyID; // =oCls.mstprtc 
                oCls.mstprtc = oCls.mstcust;

                // oCls.msternv = oCls.acctname;
                Msg += "6";
                oCls.mstcurrcd = 1;
                oCls.mstcurrrat = 1;
                oCls.mstintr = 0;

                oCls.mstbuyerc = 0; oCls.mstperd = 0; oCls.mstdsptoc = 0;
                ////if (ItemDet.LstItem.Count > 0 && ItemDet.LstItem[0].GSTStateVal == "1")
                ////    oCls.mstIorL = "I";
                ////else
                ////    oCls.mstIorL = "L";


                //oCls.mstlotno = oCls.EMail;
                //oCls.mstsection = oCls.NewMobile;
                //oCls.mstuser = oCls.UserID;
                Msg += "7";
                oSubmit.BeginTran();

                oSubmit.InsOrdeMst_API(oCls);
                Msg += "8";
                oSubmit.insertData("Delete from " + sItmTbl + " where CompCode = " + oCls.compcode + " and Itdtype = " + oCls.msttype + " and Itdcode = " + oCls.mstcode);
                Msg += "9";
                //for (int i = 0; i < ItemDet.LstItem.Count; i++)
                //{

                //    oSapu.compcode = Convert.ToInt16(oCls.compcode);
                //    oSapu.itdtype = Convert.ToInt32(oCls.msttype);
                //    oSapu.itdcode = Convert.ToInt32(oCls.mstcode);
                //    oSapu.itdtime = Convert.ToInt32(oCls.msttime);
                //    oSapu.itdsrno = Convert.ToInt16(i + 1);
                //    oGath.gathpuri = oGath.gathstat = Convert.ToString(oSapu.itddate = Convert.ToDateTime(oCls.mstdate));

                //    //if (ItemDet.LstItem[i].itdmill.ToString() != "") oSapu.itdmill = Convert.ToInt32(ItemDet.LstItem[i].itdmill.ToString());//170830
                //    if (ItemDet.LstItem[i].itdmill != null && ItemDet.LstItem[i].itdmill > 0) oSapu.itdmill = Convert.ToInt32(ItemDet.LstItem[i].itdmill); else oSapu.itdmill = 0;
                //    if (ItemDet.LstItem[i].itdgodo != null && ItemDet.LstItem[i].itdgodo > 0) oSapu.itdgodo = Convert.ToInt32(ItemDet.LstItem[i].itdgodo); else oSapu.itdgodo = 0;

                //    oSapu.itditem = Convert.ToInt32(ItemDet.LstItem[i].itditem);
                //    oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].itdquan);
                //    oSapu.itdrate = Convert.ToInt32(ItemDet.LstItem[i].itdrate);
                //    oSapu.itdamou = Convert.ToInt32(ItemDet.LstItem[i].Amt);
                //    oSapu.itdgstrtv = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer);
                //    if (ItemDet.LstItem[i].ItemNetAmt.ToString() != "") oSapu.itdlabamt = Convert.ToDecimal(ItemDet.LstItem[i].ItemNetAmt.ToString()); else oSapu.itdlabamt = 0;


                //    if (ItemDet.LstItem[i].Sitdexpd != null && ItemDet.LstItem[i].Sitdexpd != "") oSapu.itdPODt = oSubmit.GetDateFormat(ItemDet.LstItem[i].Sitdexpd);
                //    oSapu.itdrema = ItemDet.LstItem[i].itdrema.ToString();
                //    Msg += "10" + i;
                //    oSubmit.insOrdeItd(oSapu, sItmTbl);
                //    Msg += "11" + i;
                //}

                for (int i = 0; i < ItemDet.LstItem.Count; i++)
                {

                    oSapu = new sapuitd();
                    oGath = new gathdet();
                    sGathCode = oSubmit.GetUsWoCode();

                    oSapu.compcode = Convert.ToInt16(oCls.compcode);
                    oSapu.itdtype = Convert.ToInt32(oCls.msttype);
                    oSapu.itdcode = Convert.ToInt32(oCls.mstcode);
                    oSapu.itdtime = Convert.ToInt32(oCls.msttime);
                    oSapu.itdsrno = Convert.ToInt16(i + 1);
                    oGath.gathpuri = oGath.gathstat = Convert.ToString(oSapu.itddate = Convert.ToDateTime(oCls.mstdate));

                    oSapu.itditem = Convert.ToInt32(ItemDet.LstItem[i].itditem);

                    if (ItemDet.LstItem[i].UnitID.ToString() != "") oSapu.itdunit = Convert.ToInt32(ItemDet.LstItem[i].UnitID); else oSapu.itdunit = 0;

                    if (ItemDet.LstItem[i].itdactprc.ToString() != "") oSapu.itdactprc = Convert.ToDecimal(ItemDet.LstItem[i].itdactprc.ToString());
                    else oSapu.itdactprc = 0;

                    oSapu.itdvate = Convert.ToDecimal(ItemDet.LstItem[i].itdvate);
                    oSapu.itdrate = Convert.ToDecimal(ItemDet.LstItem[i].itdrate);

                    oGath.gathwtdi = Convert.ToDecimal(oSapu.itdvate); //oSapu.itdvate itdrate

                    if (ItemDet.LstItem[i].Amt.ToString() != "") oSapu.itdlabamt = Convert.ToDecimal(ItemDet.LstItem[i].Amt.ToString()); else oSapu.itdlabamt = 0;
                    if (ItemDet.LstItem[i].ItemNetAmt.ToString() != "") oSapu.itdamou = Convert.ToDecimal(ItemDet.LstItem[i].ItemNetAmt.ToString()); else oSapu.itdamou = 0;

                    oSapu.itdtowt = 0;//Convert.ToInt32(ItemDet.LstItem[i].Qty);//170830
                    if (oCls.msttype == 42) oSapu.itdpenq = oSapu.itdquan = -Convert.ToInt32(ItemDet.LstItem[i].itdquan);
                    else oSapu.itdpenq = oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].itdquan);

                    oSapu.itdpkin = 0;
                    //oSapu.itdmill = 1;
                    oSapu.itdgodo = 1;
                    if (ItemDet.LstItem[i].itdmill.ToString() != "") oSapu.itdmill = Convert.ToInt32(ItemDet.LstItem[i].itdmill.ToString());//170830
                    if (ItemDet.LstItem[i].itdrema != null && ItemDet.LstItem[i].itdrema.ToString() != "") oSapu.itdrema = ItemDet.LstItem[i].itdrema.ToString();
                    if (ItemDet.LstItem[i].itdrema != null && ItemDet.LstItem[i].itdrema.ToString() != "") oSapu.itdtagno = ItemDet.LstItem[i].itdrema.ToString();//170830

                    oSapu.itdlswt = 0;//170831
                    oSapu.itdqtyd = 0;

                    if (ItemDet.LstItem[i].DisPer.ToString() != "") oSapu.itddscp = Convert.ToDecimal(ItemDet.LstItem[i].DisPer.ToString()); else oSapu.itddscp = 0;

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

                    if (GSTStateVal == "0")
                    {
                        //if (Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal) > 0) oSapu.itdcgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / Convert.ToInt16(ItemDet.LstItem[i].GSTStateVal);
                        oSapu.itdcgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / 2;
                        oSapu.itdcgstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt) / 2;

                        oSapu.itdsgstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer) / 2;
                        oSapu.itdsgstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt) / 2;

                        // oSapu.itdigstrt = 0; oSapu.itdigstvl = 0;
                    }
                    else if (GSTStateVal == "1")
                    {
                        oSapu.itdcgstrt = 0; oSapu.itdcgstvl = 0; oSapu.itdsgstrt = 0; oSapu.itdsgstvl = 0;
                        oSapu.itdigstrt = Convert.ToDecimal(ItemDet.LstItem[i].TaxPer);
                        oSapu.itdigstvl = Convert.ToDecimal(ItemDet.LstItem[i].Tax_Amt);
                    }
                    oSapu.itdcessrt = Convert.ToDecimal(0); oSapu.itdcessvl = Convert.ToDecimal(0); oSapu.itdugstrt = Convert.ToDecimal(0); oSapu.itdugstvl = Convert.ToDecimal(0);

                    oSubmit.insSapuItd(oSapu, sItmTbl);
                    oSubmit.insGathDet(oGath);

                }

                Msg += "12";
                CodeGen oCode = new CodeGen();
                //if (oCls.IsEdit != true)
                //{
                oCode.MstCode = oCls.mstcode;
                oCode.compcode = oCls.compcode;
                oCode.msttype = oCls.msttype;

                oCode.mstdate = oCls.mstdate;
                oCode.StDate = Convert.ToDateTime(oCom.getOpenDate(DateTime.Now.Date));
                oCode.LastDate = Convert.ToDateTime(oCom.getClosDate(DateTime.Now.Date));
                oSubmit.UpdCodeGen_API(oCode);
                // }
                Msg += "13";

                //********************************* User Work ********************************* 
                clsUserWork oUser = new clsUserWork();
                oUser.woruser = (int)oCls.mstuser;
                oUser.wormode = 0;
                oUser.worcomp = oCls.compcode;
                oUser.wortype = oCls.msttype;
                oUser.worcode = oCls.mstcode;
                oUser.worsrno = oSubmit.GetUsWoCode();
                oUser.worrema = "D-#" + oCls.mstdate + "#" + oCls.mstchno + "#App";
                oUser.wordate = oCls.mstdate;
                oUser.worrfsr = "";
                oUser.worsysn = MachineName;
                oUser.IP_Add = IPAdd;
                //oUser.WorChNo = oCls.mstchno;
                //oUser.WorNarr = oCls.mstrema;
                oSubmit.InsUserWork(oUser);
                //********************************* User Work ********************************* 

                oSubmit.Commit();
                Msg += "14";
                ID = 2;
                Msg = "Saved Successfully ...";

                dt2 = oSubmit.GetData("SELECT count(*)+1 Total from OrdeMst where compcode =" + oCls.compcode + " and msttype = " + oCls.msttype + " and Mstuser = " + oCls.mstuser);
                oCls.mstprtc = Convert.ToInt32(dt2.Rows[0]["Total"].ToString());
                oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from Ordemst where compcode ='" + oCls.compcode + "' and msttype ='" + oCls.msttype + "'"));
                IsSave = 1;
            }
            catch (Exception ex)
            {
                IsSave = 0;
                oSubmit.RollBack();
                // oSubmit.CloseConnection();
                ID = 3;
                Msg = Msg + ex.Message;
                // return RedirectToAction("ParkLead", new { MstType = oCls.msttype, MstCode = 0, comp = oCls.compcode });
            }

            // var sData = new { mstprtc = oCls.mstprtc, IsSave = IsSave, mstcode = oCls.mstcode };
            //return Json(Json(sData).Data, JsonRequestBehavior.AllowGet);

            var ITEMS = new List<Result>() { new Result() { ID = ID, Msg = Msg } };

            // var ITEMS = new Result() { ID = ID ,Msg= MSG }; 

            return new JsonResult()
            {
                Data = new { ITEMS },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

            //if (headers.Contains("acctname")) oCls.acctname = headers.GetValues("acctname").First(); 
            //if (headers.Contains("mstprtc")) { if (headers.GetValues("mstprtc").First() != "") oCls.mstprtc = Convert.ToInt32(  headers.GetValues("mstprtc").First());} 
            //if (headers.Contains("mstContactPerson")) oCls.mstContactPerson = headers.GetValues("mstContactPerson").First(); 
            //if (headers.Contains("mstdate")){ if (headers.GetValues("mstdate").First() != "") oCls.mstdate =Convert.ToDateTime( headers.GetValues("mstdate").First()); }
            //if (headers.Contains("sItemDet")) oCls.sItemDet = headers.GetValues("sItemDet").First(); 
            //if (headers.Contains("msttype")) { if (headers.GetValues("msttype").First() != "") oCls.msttype =Convert.ToInt32( headers.GetValues("msttype").First()); }
            //if (headers.Contains("sMstdate")) oCls.sMstdate = headers.GetValues("sMstdate").First();
            //if (headers.Contains("compcode")) { if (headers.GetValues("compcode").First() != "") oCls.compcode = Convert.ToInt32(headers.GetValues("compcode").First()); } 
            //if (headers.Contains("ChallanDate")) oCls.ChallanDate = headers.GetValues("ChallanDate").First(); 
            //if (headers.Contains("mstdued")){ if (headers.GetValues("mstdued").First() != "") oCls.mstdued =Convert.ToInt32( headers.GetValues("mstdued").First()); }
            //if (headers.Contains("PartyID")){ if (headers.GetValues("PartyID").First() != "") oCls.PartyID =Convert.ToInt32( headers.GetValues("PartyID").First()); }
            //if (headers.Contains("mstchnV")){ if (headers.GetValues("mstchnV").First() != "") oCls.mstchnV =Convert.ToInt32( headers.GetValues("mstchnV").First()); }
            //if (headers.Contains("acctledg")) oCls.acctledg = headers.GetValues("acctledg").First(); 
            //if (headers.Contains("EMail")) oCls.EMail = headers.GetValues("EMail").First();  
            //if (headers.Contains("NewMobile")) oCls.NewMobile = headers.GetValues("NewMobile").First(); 
            //if (headers.Contains("mstrema")) oCls.mstrema = headers.GetValues("mstrema").First(); 
            //if (headers.Contains("mstContactType")) oCls.mstContactType = headers.GetValues("mstContactType").First(); 
            //if (headers.Contains("mstrvsc")){ if (headers.GetValues("mstrvsc").First() != "") oCls.mstrvsc = Convert.ToInt32(headers.GetValues("mstrvsc").First()); }
            //if (headers.Contains("acctstat")){ if (headers.GetValues("acctstat").First() != "") oCls.acctstat = Convert.ToInt32(headers.GetValues("acctstat").First()); }
            //if (headers.Contains("mstJobNo")) oCls.mstJobNo = headers.GetValues("mstJobNo").First(); 
            //if (headers.Contains("mstDrawNo")) oCls.mstDrawNo = headers.GetValues("mstDrawNo").First(); 
            //if (headers.Contains("ItemNm")) oCls.ItemNm = headers.GetValues("ItemNm").First(); 
            //if (headers.Contains("ItemID")){ if (headers.GetValues("ItemID").First() != "") oCls.ItemID =Convert.ToInt32(headers.GetValues("ItemID").First()); }
            //if (headers.Contains("mstpdc")){ if (headers.GetValues("mstpdc").First() != "") oCls.mstpdc =Convert.ToInt32(headers.GetValues("mstpdc").First()); }
            //if (headers.Contains("mstactpay")) { if (headers.GetValues("mstactpay").First() != "") oCls.mstactpay =Convert.ToInt32( headers.GetValues("mstactpay").First()); }
            //if (headers.Contains("mstrfvc")){ if (headers.GetValues("mstrfvc").First() != "") oCls.mstrfvc =Convert.ToInt32( headers.GetValues("mstrfvc").First()); }
            //if (headers.Contains("mstrfvt")){ if (headers.GetValues("mstrfvt").First() != "") oCls.mstrfvt =Convert.ToInt32( headers.GetValues("mstrfvt").First()); }
            //if (headers.Contains("mstexec")){ if (headers.GetValues("mstexec").First() != "") oCls.mstexec =Convert.ToInt32( headers.GetValues("mstexec").First());}
            //if (headers.Contains("UserID")) { if (headers.GetValues("UserID").First() != "") oCls.UserID = Convert.ToInt32(headers.GetValues("UserID").First()); }

        }

        //[System.Web.Http.HttpPost]
        //public ActionResult ParkLead([FromBody]  Jourmaster oCls)
        //{

        //    var re = Request;
        //    var headers = re.Headers;

        //    int IsSave = 1;
        //    clsSubmitModel oSubmit = new clsSubmitModel();
        //    commFunction oCom = new commFunction();
        //    int ID = 0;
        //    string Msg = "pp";
        //    try
        //    {
        //        jourtrn oTrn = new jourtrn();
        //        sapuitd oSapu = new sapuitd();
        //        gathdet oGath = new gathdet();

        //        //string sGathCode = "";
        //        string sItmTbl = "ordeItd";

        //        DataTable dt2;

        //        Msg += "1";
        //        Msg += "1" + oCls.mstcode.ToString();
        //        if (oCls.mstcode != null && oCls.mstcode > 0) { }
        //        else
        //        {
        //            Msg += "1111";
        //            oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from Ordemst where compcode ='" + oCls.compcode + "' and msttype ='" + oCls.msttype + "'"));
        //            Msg += "1111222" + oCls.mstcode.ToString();
        //        }
        //        Msg += "2";
        //        var json = oCls.sItemDet;
        //        Msg += "2w";
        //        clsPoItem ItemDet = JsonConvert.DeserializeObject<clsPoItem>(json);
        //        Msg += "3";
        //        int iDays = 0;
        //        decimal iComm = 0, iInterest = 0;

        //        oCls.mstdate = oSubmit.GetDateFormat(oCls.sMstdate);

        //        if (oCls.mstdued > 0) iDays = (int)oCls.mstdued;//DueDay 170831
        //        if (oCls.Comm > 0) iComm = oCls.Comm;
        //        if (oCls.Interest > 0) iInterest = oCls.Interest;

        //        if (oCls.mstchnH == null) oCls.mstchnH = "";
        //        oCls.mstdepa = oCls.mststat = 0;
        //        Msg += "4";
        //        if (oCls.msttype != 1163)
        //        {
        //            oCls.mstDueDate = oSubmit.GetDateFormat(oCls.ChallanDate);  //oCls.mstdate.AddDays(iDays);//170831
        //            //oCls.mstrefc = getParty(Convert.ToInt32(oCls.PartyID)) + "~" + iInterest + "~" + iDays + "~" + iComm; Set Dealer Code For Adroid App
        //        }

        //        oCls.mstchno = oCls.mstchnH + oCls.mstchnV;//170830
        //        oCls.mstexti = "~~#0";//170830
        //        Msg = "5";
        //        oCls.mstreftag = oCls.mstchno + "~0~0~0~0~0~0~0";

        //        oCls.mstgncd = "0~0~" + oCls.acctledg + "~0";//170830

        //        oCls.mstAppr = 0; oCls.mstqtyd = 0; oCls.mstvat1 = 0; oCls.mstvat2 = 0;
        //        oCls.mstvat3 = 0; oCls.mstchnm = ""; oCls.oldwht = 0; oCls.mstsite = 0;
        //        oCls.mstbrnc = 0; oCls.mstsubt = 0;  //oCls.mstrvsc = 0;
        //        oCls.mstcust = oCls.PartyID; // =oCls.mstprtc 
        //        oCls.msternv = oCls.acctname;
        //        Msg = "6";
        //        oCls.mstcurrcd = 1;
        //        oCls.mstcurrrat = 1;
        //        oCls.mstintr = 0;

        //        oCls.mstbuyerc = 0; oCls.mstperd = 0; oCls.mstdsptoc = 0;
        //        //if (ItemDet.LstItem.Count > 0 && ItemDet.LstItem[0].GSTStateVal == "1")
        //        //    oCls.mstIorL = "I";
        //        //else
        //        //    oCls.mstIorL = "L";



        //        oCls.mstlotno = oCls.EMail;
        //        oCls.mstsection = oCls.NewMobile;
        //        oCls.mstuser = oCls.UserID;
        //        Msg = "7";
        //        oSubmit.BeginTran();

        //        oSubmit.InsOrdeMst(oCls);
        //        Msg = "8";
        //        oSubmit.insertData("Delete from " + sItmTbl + " where CompCode = " + oCls.compcode + " and Itdtype = " + oCls.msttype + " and Itdcode = " + oCls.mstcode);
        //        Msg = "9";
        //        for (int i = 0; i < ItemDet.LstItem.Count; i++)
        //        {

        //            oSapu.compcode = Convert.ToInt16(oCls.compcode);
        //            oSapu.itdtype = Convert.ToInt32(oCls.msttype);
        //            oSapu.itdcode = Convert.ToInt32(oCls.mstcode);
        //            oSapu.itdtime = Convert.ToInt32(oCls.msttime);
        //            oSapu.itdsrno = Convert.ToInt16(i + 1);
        //            oGath.gathpuri = oGath.gathstat = Convert.ToString(oSapu.itddate = Convert.ToDateTime(oCls.mstdate));

        //            if (ItemDet.LstItem[i].itdmill != null && ItemDet.LstItem[i].itdmill > 0) oSapu.itdmill = Convert.ToInt32(ItemDet.LstItem[i].itdmill); oSapu.itdmill = 0;
        //            if (ItemDet.LstItem[i].itdgodo != null && ItemDet.LstItem[i].itdgodo > 0) oSapu.itdgodo = Convert.ToInt32(ItemDet.LstItem[i].itdgodo); oSapu.itdgodo = 0;

        //            oSapu.itditem = Convert.ToInt32(ItemDet.LstItem[i].itditem);
        //            oSapu.itdquan = Convert.ToInt32(ItemDet.LstItem[i].itdquan);
        //            oSapu.itdrate = Convert.ToInt32(ItemDet.LstItem[i].itdrate);
        //            if (ItemDet.LstItem[i].Sitdexpd != null && ItemDet.LstItem[i].Sitdexpd != "") oSapu.itdPODt = oSubmit.GetDateFormat(ItemDet.LstItem[i].Sitdexpd);
        //            oSapu.itdrema = ItemDet.LstItem[i].itdrema.ToString();
        //            Msg = "10" + i;
        //            oSubmit.insOrdeItd(oSapu, sItmTbl);
        //            Msg = "11" + i;
        //        }
        //        Msg = "12";
        //        if (oCls.IsEdit != true)
        //        {
        //            oCls.StDate = Convert.ToDateTime(oCom.getOpenDate(DateTime.Now.Date));
        //            oCls.LastDate = Convert.ToDateTime(oCom.getClosDate(DateTime.Now.Date));
        //            oSubmit.UpdCodeGen(oCls);
        //        }
        //        Msg = "13";
        //        oSubmit.Commit();
        //        Msg = "14";
        //        ID = 2;
        //        Msg = "Saved Successfully ...";

        //        dt2 = oSubmit.GetData("SELECT count(*)+1 Total from OrdeMst where compcode =" + oCls.compcode + " and msttype = " + oCls.msttype + " and Mstuser = " + oCls.UserID);
        //        oCls.mstprtc = Convert.ToInt32(dt2.Rows[0]["Total"].ToString());
        //        oCls.mstcode = Convert.ToInt32(oSubmit.GetSingleData("SELECT ISNULL(MAX(mstcode)+1 ,1)mstcode from Ordemst where compcode ='" + oCls.compcode + "' and msttype ='" + oCls.msttype + "'"));
        //        IsSave = 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        IsSave = 0;
        //        //oSubmit.RollBack();
        //        oSubmit.CloseConnection();
        //        ID = 3;
        //        Msg = Msg + ex.Message;
        //        // return RedirectToAction("ParkLead", new { MstType = oCls.msttype, MstCode = 0, comp = oCls.compcode });
        //    }

        //    // var sData = new { mstprtc = oCls.mstprtc, IsSave = IsSave, mstcode = oCls.mstcode };
        //    //return Json(Json(sData).Data, JsonRequestBehavior.AllowGet);

        //    var ITEMS = new List<Result>() { new Result() { ID = ID, Msg = Msg } };

        //    // var ITEMS = new Result() { ID = ID ,Msg= MSG }; 

        //    return new JsonResult()
        //    {
        //        Data = new { ITEMS },
        //        ContentType = "application/json; charset=utf-8",
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        //        MaxJsonLength = Int32.MaxValue
        //    };

        //    //if (headers.Contains("acctname")) oCls.acctname = headers.GetValues("acctname").First(); 
        //    //if (headers.Contains("mstprtc")) { if (headers.GetValues("mstprtc").First() != "") oCls.mstprtc = Convert.ToInt32(  headers.GetValues("mstprtc").First());} 
        //    //if (headers.Contains("mstContactPerson")) oCls.mstContactPerson = headers.GetValues("mstContactPerson").First(); 
        //    //if (headers.Contains("mstdate")){ if (headers.GetValues("mstdate").First() != "") oCls.mstdate =Convert.ToDateTime( headers.GetValues("mstdate").First()); }
        //    //if (headers.Contains("sItemDet")) oCls.sItemDet = headers.GetValues("sItemDet").First(); 
        //    //if (headers.Contains("msttype")) { if (headers.GetValues("msttype").First() != "") oCls.msttype =Convert.ToInt32( headers.GetValues("msttype").First()); }
        //    //if (headers.Contains("sMstdate")) oCls.sMstdate = headers.GetValues("sMstdate").First();
        //    //if (headers.Contains("compcode")) { if (headers.GetValues("compcode").First() != "") oCls.compcode = Convert.ToInt32(headers.GetValues("compcode").First()); } 
        //    //if (headers.Contains("ChallanDate")) oCls.ChallanDate = headers.GetValues("ChallanDate").First(); 
        //    //if (headers.Contains("mstdued")){ if (headers.GetValues("mstdued").First() != "") oCls.mstdued =Convert.ToInt32( headers.GetValues("mstdued").First()); }
        //    //if (headers.Contains("PartyID")){ if (headers.GetValues("PartyID").First() != "") oCls.PartyID =Convert.ToInt32( headers.GetValues("PartyID").First()); }
        //    //if (headers.Contains("mstchnV")){ if (headers.GetValues("mstchnV").First() != "") oCls.mstchnV =Convert.ToInt32( headers.GetValues("mstchnV").First()); }
        //    //if (headers.Contains("acctledg")) oCls.acctledg = headers.GetValues("acctledg").First(); 
        //    //if (headers.Contains("EMail")) oCls.EMail = headers.GetValues("EMail").First();  
        //    //if (headers.Contains("NewMobile")) oCls.NewMobile = headers.GetValues("NewMobile").First(); 
        //    //if (headers.Contains("mstrema")) oCls.mstrema = headers.GetValues("mstrema").First(); 
        //    //if (headers.Contains("mstContactType")) oCls.mstContactType = headers.GetValues("mstContactType").First(); 
        //    //if (headers.Contains("mstrvsc")){ if (headers.GetValues("mstrvsc").First() != "") oCls.mstrvsc = Convert.ToInt32(headers.GetValues("mstrvsc").First()); }
        //    //if (headers.Contains("acctstat")){ if (headers.GetValues("acctstat").First() != "") oCls.acctstat = Convert.ToInt32(headers.GetValues("acctstat").First()); }
        //    //if (headers.Contains("mstJobNo")) oCls.mstJobNo = headers.GetValues("mstJobNo").First(); 
        //    //if (headers.Contains("mstDrawNo")) oCls.mstDrawNo = headers.GetValues("mstDrawNo").First(); 
        //    //if (headers.Contains("ItemNm")) oCls.ItemNm = headers.GetValues("ItemNm").First(); 
        //    //if (headers.Contains("ItemID")){ if (headers.GetValues("ItemID").First() != "") oCls.ItemID =Convert.ToInt32(headers.GetValues("ItemID").First()); }
        //    //if (headers.Contains("mstpdc")){ if (headers.GetValues("mstpdc").First() != "") oCls.mstpdc =Convert.ToInt32(headers.GetValues("mstpdc").First()); }
        //    //if (headers.Contains("mstactpay")) { if (headers.GetValues("mstactpay").First() != "") oCls.mstactpay =Convert.ToInt32( headers.GetValues("mstactpay").First()); }
        //    //if (headers.Contains("mstrfvc")){ if (headers.GetValues("mstrfvc").First() != "") oCls.mstrfvc =Convert.ToInt32( headers.GetValues("mstrfvc").First()); }
        //    //if (headers.Contains("mstrfvt")){ if (headers.GetValues("mstrfvt").First() != "") oCls.mstrfvt =Convert.ToInt32( headers.GetValues("mstrfvt").First()); }
        //    //if (headers.Contains("mstexec")){ if (headers.GetValues("mstexec").First() != "") oCls.mstexec =Convert.ToInt32( headers.GetValues("mstexec").First());}
        //    //if (headers.Contains("UserID")) { if (headers.GetValues("UserID").First() != "") oCls.UserID = Convert.ToInt32(headers.GetValues("UserID").First()); }

        //}


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

        public ActionResult GetLeadList(int iEmpCode, int ItemID = 0, string To = "", string From = "", int iIndustries = 0, int iMstType = 0, int UserType = 1)
        {
            string sSQL = "";
            if (iEmpCode > 0)
                sSQL = " and a.mstuser =" + iEmpCode;

            if (UserType > 1)
                sSQL = " and a.mstuser =" + UserType;

            clsSubmitModel oSubmit = new clsSubmitModel();
            List<Jourmaster> lstJourmaster = oSubmit.GetLeadList(ItemID, iEmpCode, To, From, iIndustries, iMstType);

            return new JsonResult()
            {
                Data = new { lstJourmaster },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }


        [System.Web.Http.HttpGet]
        public ActionResult GetDepartment()
        {
            var a = from k in dba.studdets where k.studType == 11 orderby k.studName select new { k.studName, k.studCode };

            return new JsonResult()
            {
                Data = new { a },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [System.Web.Http.HttpGet]
        public JsonResult getDegignation(int iDept = 0)
        {
            var Designation = from a in dba.studdets where a.studType == 12 where a.studadd1 == iDept.ToString() select new { a.studName, a.studCode };
            //var Designation = from a in dba.studdet1s where a.studType == 12 orderby a.studName select new { a.studName, a.studCode };

            return new JsonResult()
            {
                Data = new { Designation },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }


        public JsonResult GetOrderNo(int CompCode, int MstType)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            string sMstChNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");

            sMstChNo = sMstChNo + GetVoucherNo(oSubmit.GetSingleData("select isnull(max(right(mstchno,2))+1,1) as maxchno  from OrdeMst where compcode = " + CompCode + " and msttype = " + MstType + " and left(mstchno, 6) = '" + sMstChNo + "'", "0", true).ToString());

            //DateTime LastDate = Convert.ToDateTime(oSubmit.GetDateFormat(oSubmit.GetSingleData("Select Top 1 MstDate from OrdeMst where compcode = " + CompCode + " and msttype = " + MstType + " order by MstCode Desc", "", true).ToString()));

            string LastDate = oSubmit.GetSingleData("Select Top 1 MstDate from OrdeMst where compcode = " + CompCode + " and msttype = " + MstType + " order by MstCode Desc", "", true).ToString();

            var sData = new { mstprtc = sMstChNo, LastDate = LastDate };
            //return Json(Json(sData), JsonRequestBehavior.AllowGet);
            return new JsonResult()
            {
                Data = new { sData },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
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

        public JsonResult GetDealerSummary(int CompCode, int Msttype, int DealerID)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();

            DataTable dt;
            dt = oSubmit.GetData("exec GetDealerSummary @CompCode = " + CompCode + " , @Msttype = " + Msttype + ", @DealerID ='" + DealerID + "'");

            var sData = new { LastOrderDt = dt.Rows[0]["L_Date"].ToString(), LastOrderVl = dt.Rows[0]["L_Total"].ToString(), TotalOrdVl = dt.Rows[0]["Total"].ToString(), TotalOrdNo = dt.Rows[0]["TotalOrd"].ToString() };
            return new JsonResult()
            {
                Data = new { sData },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [System.Web.Http.HttpGet]
        public JsonResult GetBrand()
        {
            var Designation = from a in dba.studdets where a.studType == 61 select new { a.studName, a.studCode };

            return new JsonResult()
            {
                Data = new { Designation },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }

        [System.Web.Http.HttpGet]
        public JsonResult GetMstChNo(int CompCode, int msttype)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            string sMstChNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");

            //var OrderNo = sMstChNo + GetVoucherNo(oSubmit.GetSingleData("select isnull(max(right(mstchno,2))+1,1) as maxchno  from OrdeMst where compcode = " + CompCode + " and msttype = " + msttype + " and left(mstchno, 6) = '" + sMstChNo + "'","0", true).ToString());

            var OrderNo = sMstChNo + GetVoucherNo(oSubmit.GetSingleData("select isnull(max(right(mstchno,3))+1,1) as maxchno  from OrdeMst where msttype = " + msttype + " and  left(mstchno, 6) = '" + sMstChNo + "'", "0", true).ToString());

            return new JsonResult()
            {
                Data = new { OrderNo },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }

        [System.Web.Http.HttpPost]
        public ActionResult Visit([FromBody] tblVisit obj)
        {
            string sMsg = "Saved Successfully...";
            try
            {
                tblVisit oVisit = new tblVisit();

                oVisit.Address = obj.Address;
                oVisit.Name = obj.Name;
                oVisit.Mobile = obj.Mobile;
                oVisit.Email = obj.Email;
                oVisit.DealerID = obj.DealerID;
                oVisit.Remark = obj.Remark;
                oVisit.BrandID = obj.BrandID;
                oVisit.CreatedOn = DateTime.Now;

                dba.tblVisits.InsertOnSubmit(oVisit);
                dba.SubmitChanges();
            }
            catch (Exception ex)
            {
                sMsg = ex.Message;
            }

            return new JsonResult()
            {
                Data = new { sMsg },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [System.Web.Http.HttpGet]
        public JsonResult GetVisit()
        {
            var Vist = from a in dba.tblVisits
                       join b in dba.tblDistributors on
                           a.DealerID equals b.MstCode into c
                       from d in c.DefaultIfEmpty()
                       select new { a.Address, a.BrandID, a.DealerID, a.Email, a.Mobile, a.Name, d.DisName };
            return new JsonResult()
            {
                Data = new { Vist },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }

        [System.Web.Http.HttpGet]
        public ActionResult GetItem(int subcat, int Brand, int CompCode = 66)
        {
            var Item = from a in dba.itemains
                       join b in
                           (from P in dba.pricitds group P by new { P.compcode, P.itditem } into Q select new { compcode = Q.Key.compcode, itditem = Q.Key.itditem, itdrate = Q.Max(P => P.itdrate) }) on new { _Comp = a.compcode, _code = a.itemcode } equals new { _Comp = (int)b.compcode, _code = (int)b.itditem } into c
                       from d in c.DefaultIfEmpty()
                       where a.compcode == CompCode
                       where (subcat > 0 ? a.itemgrou == subcat : true)
                       where (Brand > 0 ? a.itemnumb == Brand : true)
                       select new { a.itemcode, itemrate = d.itdrate, a.itemgstr, a.itemname, a.itemnumb };

            return new JsonResult()
            {
                Data = new { Item },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        [System.Web.Http.HttpGet]
        public ActionResult GetSubCategory(int CompCode)
        {
            var Item = from a in dba.itgroups where a.compcode == CompCode where a.itgpunde != 0 select new { a.itgpname, a.itgpcode };
            return new JsonResult()
            {
                Data = new { Item },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        [System.Web.Http.HttpGet]
        public ActionResult GetDealerName(int UserType, int UserID)
        {
            var F_Dealer = from a in dba.tblDistributors where a.DistributorID == 0 select new { itgpName = a.DealCode, itgpcode = a.MstCode };

            if (UserType > 0)
                F_Dealer = from a in dba.tblDistributors join acc in dba.accounts on a.MstCode equals acc.AcctDeal join b in dba.EmpAllotMsts on a.MstCode equals b.DealerID join c in dba.employees on b.EmpID equals c.empcode where a.DistributorID != 0 where c.UseCode == UserID where acc.acctgrou == 21 orderby a.DisName select new { itgpName = a.DisName + " - " + a.DealCode, itgpcode = acc.acctcode };
            else
                F_Dealer = from a in dba.tblDistributors join acc in dba.accounts on a.MstCode equals acc.AcctDeal where a.DistributorID != 0 where acc.acctgrou == 21 orderby a.DisName select new { itgpName = a.DisName + " - " + a.DealCode, itgpcode = acc.acctcode };


            return new JsonResult()
            {
                Data = new { F_Dealer },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        public ActionResult GetOpening(int DealerID)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            var ITEMS = new { Opening = "0.00" };

            DataTable dt;
            dt = oSubmit.GetData("select top 1 Opening   from tblOpening where DealerID = " + DealerID + " order by  CreatedOn desc");
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToDecimal(dt.Rows[0]["Opening"].ToString()) > 0)
                    ITEMS = new { Opening = dt.Rows[0]["Opening"].ToString() };
            }
            return new JsonResult()
            {
                Data = new { ITEMS },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }

        public ActionResult getItemRate(int Compcode, int ItemCode, int AcctCode)
        {

            var dTax = (from a in dba.itemains where a.compcode == Compcode where a.itemcode == ItemCode select new { Tax = a.itemgstr ?? 0 }).FirstOrDefault();
            var dBrand = (from a in dba.itemains where a.compcode == Compcode where a.itemcode == ItemCode select new { Brand = a.itemnumb ?? 0 }).FirstOrDefault();
            var dRate = (from a in dba.pricitds where a.compcode == Compcode where a.itditem == ItemCode orderby a.itddate descending select new { Rate = a.itdrate ?? 0 }).FirstOrDefault();

            //Data = new { Tax = dTax, Rate = dRate ,AcctRt = 0 };

            // if (AcctCode > 0)
            // {
            //   var  Rt = (from a in dba.acctexis join b in dba.pricmsts on new { _Comp = a.compcode, _Code = a.acctpeli } equals new { _Comp = (int)b.compcode, _Code = b.mstplty } join c in dba.pricitds on new { _Comp = (int)b.compcode, _Type = b.msttype, _Code = b.mstcode } equals new { _Comp = (int)c.compcode, _Type = c.itdtype, _Code = c.itdcode } where a.acctcode == AcctCode where c.itditem == ItemCode orderby c.itddate descending select new { c.itdrate }).FirstOrDefault();
            //    // Data = new { Tax = dTax, Rate = dRate ,AcctRt = Rt };
            // }
            var ItemData = new { Tax = dTax, Rate = dRate, Brand = dBrand };
            return new JsonResult()
            {
                Data = new { ItemData },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        public ActionResult getCompany()
        {
            var Comp = from a in dba.companies orderby a.compname select new { a.compname, a.compcode };

            //var Data = new { Tax = dTax, Rate = dRate };
            return new JsonResult()
            {
                Data = new { Comp },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [System.Web.Http.HttpGet]
        public ActionResult getComplent()
        {
            var dTax = (from a in dba.tblComplaints select a);

            //var Data = new { Tax = dTax, Rate = dRate, Brand = dBrand };
            return new JsonResult()
            {
                Data = new { dTax },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [System.Web.Http.HttpGet]
        public ActionResult getEmoplyee()
        {
            DataTable dt;
            clsSubmitModel oCon = new clsSubmitModel();

            var Emp = oCon.DataTableToJSON1(oCon.GetData("Select Empname + ' ( '+ cast((Select count(*) from tblComplaint b where b.statusID= 1 and b.EmpID = a.UseCode ) as varchar(10)) + ' ) ' ,UseCode  from Employee a order by Empname ", true));

            //var Data = new { Tax = dTax, Rate = dRate, Brand = dBrand };
            return new JsonResult()
            {
                Data = new { Emp },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [System.Web.Http.HttpGet]
        public ActionResult getCustDet(string Mobile = "", string Model = "", string InvNo = "", string ItemID = "", string PartyID = "")
        {
            var sMsg = new { Message = "Successfully  ...", MsgID = 1 };
            DataTable dt;
            clsSubmitModel oCon = new clsSubmitModel();

            var CustDet = oCon.DataTableToJSON1(oCon.GetData("getCustDet @Mobile ='" + Mobile + "' , @Model ='" + Model + "', @InvNo ='" + InvNo + "', @ItemID ='" + ItemID + "', @PartyID='" + PartyID + "'", true));
            //var CustDet = oCon.DataTableToJSON1(oCon.GetData("getCustDet @Mobile =" + Mobile+ " , @Model =" + Model  , true));

            return new JsonResult()
            {
                Data = new { CustDet },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
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



        [System.Web.Http.HttpPost]
        public ActionResult Complaint([FromBody] tblComplaint oTbl)
        {
            string Trace = "";
            var sMsg = new { Message = "Successfully Saved ...", MsgID = 1 };
            try
            {
                var re = Request;
                var headers = re.Headers;

                string CompNo = DateTime.Now.Date.Date.ToString("yy") + DateTime.Now.Date.Date.ToString("MM") + DateTime.Now.Date.Date.ToString("dd");
                clsSubmitModel oSubmit = new clsSubmitModel();
                tblComplaint oComp = new tblComplaint();
                Trace += "1";
                oTbl.CompNo = CompNo + GetNo(oSubmit.GetSingleData("Select isnull(max(CompID)+1,1) from tblComplaint", "0", true).ToString());
                Trace += "2";
                Trace += oTbl.CompNo;

                if (headers.Contains("sMstdate"))
                {
                    Trace += "dt1";
                    if (headers.Contains("sMstdate"))
                    {
                        Trace += "d_1";
                        oTbl.CompDt = oSubmit.GetDate(headers.GetValues("sMstdate").First());
                        Trace += "d_2";
                    }
                    Trace += "dt2";
                }

                //if (headers.Contains("sTentetiveTm"))
                //{
                //    Trace += "TN1";
                //    if (headers.Contains("sTentetiveTm"))
                //    {
                //        Trace += "TN_1";
                //        oTbl.CompDt = oSubmit.GetDate(headers.GetValues("sTentetiveTm").First());
                //        if (headers.GetValues("sTentetiveTm").First() != null && headers.GetValues("sTentetiveTm").First() != "") oTbl.TentetiveTm = oSubmit.GetDate(headers.GetValues("sTentetiveTm").First());
                //        Trace += "TN_2";
                //    }
                //    Trace += "TN2";
                //}

                //Trace += "="+ oTbl.CompDt.ToString();
                //oTbl.CompDt = oSubmit.GetDate(oTbl.CompDt.ToString());
                Trace += "1";
                //if (oTbl.TentetiveTm != null && oTbl.TentetiveTm.ToString() != "") oTbl.TentetiveTm = oSubmit.GetDateFormat(oTbl.TentetiveTm.ToString());
                oTbl.CreatedOn = DateTime.Now;
                Trace += "1";
                oTbl.StatusID = 1;

                dba.tblComplaints.InsertOnSubmit(oTbl);
                dba.SubmitChanges();
                Trace += "1";

                sMsg = new { Message = "Complaint Successfully Register . Your Complaint No Is " + oTbl.CompNo + " .", MsgID = 1 };

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
        [System.Web.Http.HttpPost]
        public ActionResult ComplaintAllot([FromBody] tblComplaint oTbl)
        {
            string Trace = "";
            var sMsg = new { Message = "Successfully Update ...", MsgID = 1 };
            try
            {

                var Comp = (from a in dba.tblComplaints where a.CompID == oTbl.CompID select a).SingleOrDefault();
                Comp.EmpID = oTbl.EmpID;
                Comp.Remark_Mgr = oTbl.Remark_Mgr;

                dba.SubmitChanges();

                sMsg = new { Message = "Complaint Alloted ...", MsgID = 1 };

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

        [System.Web.Http.HttpPost]
        public ActionResult ComplaintStatusUpd([FromBody] tblComplaint oTbl)
        {
            string Trace = "";
            var sMsg = new { Message = "Successfully Update ...", MsgID = 1 };
            try
            {

                var Comp = (from a in dba.tblComplaints where a.CompID == oTbl.CompID select a).SingleOrDefault();
                Comp.StatusID = oTbl.StatusID;
                Comp.StatusRemark = oTbl.StatusRemark;
                Comp.cmStatusDt = DateTime.Now;
                dba.SubmitChanges();

                sMsg = new { Message = "Complaint Updated ...", MsgID = 1 };

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

        [System.Web.Http.HttpGet]
        public ActionResult GetEmpCompNo(int EmpID = 0)
        {

            var CompNo = from a in dba.tblComplaints where a.EmpID == EmpID where a.StatusID == 1 orderby a.CompNo select new { a.CompNo, a.CompDt, a.CompID };

            return new JsonResult()
            {
                Data = new { CompNo },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [System.Web.Http.HttpGet]
        public ActionResult getItemRema(int CompID)
        {
            clsSubmitModel oCon = new clsSubmitModel();
            var CustDet = oCon.DataTableToJSON1(oCon.GetData("getItemRema @CompID =" + CompID, true));

            return new JsonResult()
            {
                Data = new { CustDet },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [System.Web.Http.HttpPost]
        public ActionResult SetEngRema([FromBody] tblComplaint oTbl)
        {
            string Trace = "";
            var sMsg = new { Message = "Successfully Saved ...", MsgID = 1 };
            try
            {

                var Comp = (from a in dba.tblComplaints where a.CompID == oTbl.CompID select a).SingleOrDefault();

                Trace += "a";
                Comp.Remark_Eng = oTbl.Remark_Eng;
                Trace += "a";
                Comp.StatusID = oTbl.StatusID;
                Trace += "a";
                Comp.Charge = oTbl.Charge;
                Trace += "a";
                dba.SubmitChanges();
                Trace += "a";
                sMsg = new { Message = Trace + "Successfully Saved . ", MsgID = 1 };

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
        [System.Web.Http.HttpGet]
        public ActionResult GetVisitScheduleList(string CompCode = "", string EmpID = "")
        {
            var sMsg = new { Message = "Successfully  ...", MsgID = 1 };
            clsSubmitModel oCon = new clsSubmitModel();
            var CustDet = oCon.DataTableToJSON1(oCon.GetData("sp_GetVisitScheduleList @CompCode ='" + CompCode + "' , @EmpID ='" + EmpID + "'", true));
            return new JsonResult()
            {
                Data = new { CustDet },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        [System.Web.Http.HttpGet]
        public ActionResult GetVisitScheduleDetList(string aCompCode = "", string aPartyName = "", string aMobile = "")
        {
            var sMsg = new { Message = "Successfully  ...", MsgID = 1 };
            clsSubmitModel oCon = new clsSubmitModel();
            var CustDet = oCon.DataTableToJSON1(oCon.GetData("sp_GetVisitPartyMobData @vtCompCode =" + aCompCode + ",@vtPartyName='" + aPartyName + "',@vtMobile='" + aMobile + "'", true));
            return new JsonResult()
            {
                Data = new { CustDet },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        /*Start 181016 */
        [System.Web.Http.HttpGet]
        public ActionResult GetDailyVisitPartyList(string aCompCode = "", string aEmpID = "", string aUserType = "")
        {
            var sMsg = new { Message = "Successfully  ...", MsgID = 1 };
            clsSubmitModel oCon = new clsSubmitModel();
            var CustDet = oCon.DataTableToJSON1(oCon.GetData("sp_GetDailyVisitPartyList @CompCode=" + aCompCode + ",@EmpID=" + aEmpID + ",@UserType=" + aUserType, true));/*181017 %temp%*/
            return new JsonResult()
            {
                Data = new { CustDet },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        [System.Web.Http.HttpGet]
        public ActionResult GetInsDailyVisitEntry(string aCompCode, string aPartyName, string aVisitDetail, string aEstCost, string aNextFollowUp, string aRemark)
        {
            clsDailyVisitEntry oCls = new clsDailyVisitEntry();
            clsSubmitModel oSubmit = new clsSubmitModel();
            oCls.dvCompCode = int.Parse(aCompCode);
            oCls.dvPartyName = aPartyName.Substring(0, aPartyName.LastIndexOf('-'));
            oCls.dvMobile = aPartyName.Substring(aPartyName.LastIndexOf('-') + 1, aPartyName.Length - aPartyName.LastIndexOf('-') - 1);
            oCls.dvVisitDetail = aVisitDetail;
            if (aEstCost != "") oCls.dvEstCost = decimal.Parse(aEstCost);
            if (aNextFollowUp != "") oCls.dvNextFollowUp = Convert.ToDateTime(aNextFollowUp);
            oCls.dvRemark = aRemark;

            List<clsDailyVisitEntry> aData = oSubmit.InsDailyVisitEntry(oCls);

            return new JsonResult()
            {
                Data = new { aType = aData[0].aType, aMsg = aData[0].aMsg },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        [System.Web.Http.HttpGet]
        public ActionResult GetInsDailyVisitReport(string aCompCode, string aPartyName, string aVisitDetail, string aEstCost, string dvQty, string aNextFollowUp, string aRemark)/*181017 %temp%*/
        {
            clsDailyVisitEntry oCls = new clsDailyVisitEntry();
            clsSubmitModel oSubmit = new clsSubmitModel();
            oCls.dvCompCode = int.Parse(aCompCode);
            oCls.dvPartyName = aPartyName.Substring(0, aPartyName.LastIndexOf('-'));
            oCls.dvMobile = aPartyName.Substring(aPartyName.LastIndexOf('-') + 1, aPartyName.Length - aPartyName.LastIndexOf('-') - 1);
            oCls.dvVisitDetail = aVisitDetail;
            if (dvQty != "") oCls.dvQty = int.Parse(dvQty);
            if (aEstCost != "") oCls.dvEstCost = decimal.Parse(aEstCost);
            if (aNextFollowUp != "") oCls.dvNextFollowUp = Convert.ToDateTime(aNextFollowUp);
            oCls.dvRemark = aRemark;

            List<clsDailyVisitEntry> aData = oSubmit.InsDailyVisitReport(oCls);

            return new JsonResult()
            {
                Data = new { aType = aData[0].aType, aMsg = aData[0].aMsg },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        /*End 181016*/

        [System.Web.Http.HttpGet]
        public ActionResult getStatistics(int UserType, int UserID)
        {
            DateTime dtFrom = Convert.ToDateTime("2018-04-01");
            DateTime dtTo = DateTime.Now;

            var TotalSales = (from a in dba.jourmsts where a.msttype == 42 select a.msttota).Count();
            var TotalSaleValue = string.Format("{0:0.00}", (from a in dba.jourmsts where a.msttype == 42 select a.msttota).Sum() / 10000000) + " Cr";
            var TotalCollection = string.Format("{0:0.00}", (from a in dba.jourtrns where a.trntype == 3 select a.trndram).Sum() / 10000000) + " Cr";
            var ComplaintSolved = (from T_Compl in dba.tblComplaints where T_Compl.StatusID == 2 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) select T_Compl).Count();
            var PendingComplaint = (from T_Compl in dba.tblComplaints where T_Compl.StatusID == 1 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) select T_Compl).Count();
            var RevenFromComplains = (from T_Compl in dba.tblComplaints where T_Compl.StatusID == 2 && (T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo) select T_Compl.Charge).Sum() ?? 0;
            var TotalComplains = (from T_Compl in dba.tblComplaints where T_Compl.CompDt >= dtFrom && T_Compl.CompDt <= dtTo select T_Compl).Count();

            //var Distributor = (from a in dba.companies select a.compcode).Count();
            //var Dealer = (from a in dba.tblDistributors where a.DistributorID != 0 where a.ApprovID > 0 select a).Count();
            var Dispatch = (from a in dba.itpursals where a.itdtype == 42 && (a.itddate >= dtFrom && a.itddate <= dtTo) select a.itdquan).Sum();
            var OrdeQty = (from a in dba.ordemsts where a.msttype == 174 && (a.mstdate >= dtFrom && a.mstdate <= dtTo) select a.compcode).Count() + "/" + string.Format("{0:0.00}", (from a in dba.ordemsts where a.msttype == 174 && (a.mstdate >= dtFrom && a.mstdate <= dtTo) select a.msttota).Sum() / 10000000) + " Cr";

            var ITEMS = new
            {
                //Dis_ID = Distributor, Deal_ID = Dealer,
                OrdeQty = OrdeQty,
                Dispatch = Dispatch,
                TotalSales = TotalSales,
                TotalComplains = TotalComplains,
                TotalSaleValue = TotalSaleValue,
                TotalCollection = TotalCollection,
                ComplaintSolved = ComplaintSolved,
                PendingComplaint = PendingComplaint,
                RevenFromComplains = RevenFromComplains,
                // Q3Target = 0, Other = 0,  AvaiStckQty = 0,  ReqStckQty = 0, 
            };

            return new JsonResult()
            {
                Data = new { ITEMS },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }

        [System.Web.Http.HttpGet]
        public ActionResult getDealerDash(int UserType, int UserID)
        {

            var Complaint = from a in dba.tblComplaints
                            join b in dba.employees on a.EmpID equals b.UseCode into c
                            from d in c.DefaultIfEmpty()
                            where (UserType == 4 ? a.DealerID == UserID : UserType == 7 ? a.EmpID == UserID : a.CustID == UserID)
                            select new
                            {
                                a.CompNo,
                                a.CompDt,
                                a.EmpID,
                                d.empname,
                                a.StatusID,
                                a.CompID,
                                a.CustNM,
                                a.ModelNo,
                                a.SrvType,
                                a.ItemID,
                                a.InvNo,
                                a.TentetiveTm,
                                a.Charge,
                                a.Remark_Cust,
                                a.Remark_Eng,
                                a.cmCategory,
                                a.cmIsPaid,
                                a.Remark_Mgr,
                                a.cmSrvcMode
                            };

            var ITEMS = new
            {
                Complaint = Complaint,
            };

            return new JsonResult()
            {
                Data = new { ITEMS },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }

        /*181029 */
        [System.Web.Mvc.HttpGet]
        public ActionResult getPasswordOTP(string aMobile)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();
            commFunction oComm = new commFunction();
            DataTable dt = new DataTable();
            Random r = new Random();
            string sOTP = r.Next(999999).ToString();
            var model = dt;
            if (!aMobile.Contains("-") && !aMobile.Contains("'") && aMobile.Trim().Trim() != "")
            {
                if (oComm.Send_SMS("Your Verification OTP is " + sOTP + ".", aMobile))
                {
                    model = oSubmit.GetData("select 0 as aType,'Message Send Successfully!' as aMsg,'" + sOTP + "' as aOTP", true);
                }
                else
                {
                    model = oSubmit.GetData("select -1 as aType,'Message Sending Failed!' as aMsg", true);
                }
            }
            else
                model = oSubmit.GetData("select -1 as aType,'Message Sending Failed!' as aMsg", true);
            return new JsonResult()
            {
                Data = new { model },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        /*181029 */
        [System.Web.Http.HttpPost]
        public ActionResult SetReadStatus([FromBody] tblComplaint oTbl)
        {
            var sMsg = new { Message = "Successfully Saved ...", MsgID = 1 };
            try
            {
                var Comp = (from a in dba.tblComplaints where a.CompID == oTbl.CompID select a).SingleOrDefault();
                if (Comp != null)
                {
                    Comp.cmIsRead = 0;
                    dba.SubmitChanges();
                }
                sMsg = new { Message = "Successfully Saved . ", MsgID = 1 };

            }
            catch (Exception ex)
            {
                sMsg = new { Message = ex.Message, MsgID = 2 };
            }

            return new JsonResult()
            {
                Data = new { sMsg },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }
        public JsonResult GetManagerWsEmpCmplLst(int aStatusID,int aEmpID)
        {
            var sMsg = new { Message = "Successfully  ...", MsgID = 1 };
            clsSubmitModel oCon = new clsSubmitModel();
            var CustDet = oCon.DataTableToJSON1(oCon.GetData("sp_GetManagerWsEmpCmplLst @pStatus='" + aStatusID + "',@pEmpID='"+aEmpID+"'", true));
            return new JsonResult()
            {
                Data = new { CustDet },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        public JsonResult GetComplaintRegister(int aUserID=0,string aFromDt="",string aToDt="",int aEmpID=0,int aStatusID=0)
        {
            var sMsg = new { Message = "Successfully  ...", MsgID = 1 };
            clsSubmitModel oCon = new clsSubmitModel();
            var CustDet = oCon.DataTableToJSON1(oCon.GetData("sp_GetComplaintRegister @pHeadID='" + aUserID + "'", true));
            return new JsonResult()
            {
                Data = new { CustDet },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }
        public ActionResult GetUserList(int aUserType, int aUserID=0)
        {

            var Complaint = from a in dba.loginusers
                            join b in dba.employees on a.usecode equals b.UseCode
                            where a.usetype == aUserType
                            where (aUserID > 0 ? a.UseHeadID == aUserID : true)
                            orderby a.UseHeadID, b.empname
                            select new
                            {
                                aCode = a.usecode,
                                aName = b.empname,
                                aHeadID= a.UseHeadID,
                            }  ;

            var ITEMS = new
            {
                Complaint = Complaint,
            };

            return new JsonResult()
            {
                Data = new { ITEMS },
                ContentType = "application/json; charset=utf-8",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };

        }
    }
}
