using CompxERP.Models;
using System;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc; 
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using CompxERP.Filters; 
namespace CompxERP.Controllers
{
    [UserSessionfilter]
    public class MasterController : Controller
    {
        clsSubmitModel oSubmit = new clsSubmitModel();

        public ActionResult Index()
        {
            return View();
        }
       
        /// <summary>
        /// Use In : Account Setup
        /// </summary>
        /// <param name="acctname"></param>
        /// <returns></returns>
        //[AcceptVerbs(HttpVerbs.Post)]
        //public JsonResult GetAccount(string acctname)
        //{
        //    //clsSubmitModel oSubmit = new clsSubmitModel();
        //    DataTable dt;
        //    dt = oSubmit.GetAccount(acctname, Session["CompCode"].ToString());
        //    var result = new List<KeyValuePair<string, string>>();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        result.Add(new KeyValuePair<string, string>(dr["acctcode"].ToString(), dr["acctname"].ToString()));
        //    }
        //    var result3 = result.Where(s => s.Value.ToLower().Contains(acctname.ToLower())).Select(w => w).ToList();
        //    return Json(result3, JsonRequestBehavior.AllowGet);
        //}
        /// <summary>
        /// Use In : Account Setup
        /// </summary>
        /// <param name="acctname"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetAccountGroup(string acctname)
        {
            //clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;
            dt = oSubmit.GetAccountGroup(acctname, Session["CompCode"].ToString());
            var result = new List<KeyValuePair<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new KeyValuePair<string, string>(dr["_Code"].ToString(), dr["_Name"].ToString()));
            }
            var result3 = result.Where(s => s.Value.ToLower().Contains(acctname.ToLower())).Select(w => w).ToList();
            return Json(result3, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Use In : Account Setup 
        /// </summary>
        /// <param name="acctcity"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCity(string acctcity, int iState = 0)
        {
            //clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;
            dt = oSubmit.GetCity(acctcity, iState);
            var result = new List<KeyValuePair<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new KeyValuePair<string, string>(dr["citycode"].ToString(), dr["cityname"].ToString()));
            }
            var result3 = result.Where(s => s.Value.ToLower().Contains(acctcity.ToLower())).Select(w => w).ToList();
            return Json(result3, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Use In : Account Setup
        /// </summary>
        /// <param name="acctstat"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetState(string acctstat, int iCntry)
        {
            //clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;
            dt = oSubmit.GetState(acctstat, iCntry);
            var result = new List<KeyValuePair<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new KeyValuePair<string, string>(dr["citycode"].ToString(), dr["cityname"].ToString()));
            }
            var result3 = result.Where(s => s.Value.ToLower().Contains(acctstat.ToLower())).Select(w => w).ToList();
            return Json(result3, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCountry(string Country)
        {
            //clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;
            dt = oSubmit.GetCountry(Country);
            var result = new List<KeyValuePair<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new KeyValuePair<string, string>(dr["citycode"].ToString(), dr["cityname"].ToString()));
            }
            var result3 = result.Where(s => s.Value.ToLower().Contains(Country.ToLower())).Select(w => w).ToList();
            return Json(result3, JsonRequestBehavior.AllowGet);
        }

       
        /// <summary>
        /// Use In : Account Setup
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCategoryInfo(string name)//170608
        {
            //clsSubmitModel oSubmit = new clsSubmitModel();
            DataTable dt;
            dt = oSubmit.GetCategoryInfo(name, Session["CompCode"].ToString());
            var result = new List<KeyValuePair<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new KeyValuePair<string, string>(dr["Code"].ToString(), dr["Name"].ToString()));
            }
            var result3 = result.Where(s => s.Value.ToLower().Contains(name.ToLower())).Select(w => w).ToList();
            return Json(result3, JsonRequestBehavior.AllowGet);
        }
        ERPDataContext db = new ERPDataContext();//170706
        public ActionResult TermnCondition()//170706
        {
            Studdet obj = new Studdet();
            obj.studType = 906;
            var max = Convert.ToInt32(db.studdets.Select(t => (int?)t.studCode).Max()) + 1;
            obj.studCode = max;
            List<SelectListItem> acctcateData1 = new List<SelectListItem>();
            acctcateData1.Add(new SelectListItem { Text = "PAYMENT TERMS", Value = "1" });
            acctcateData1.Add(new SelectListItem { Text = "OTHER TERMS", Value = "2" });
            acctcateData1.Add(new SelectListItem { Text = "ARBITRATION", Value = "3" });
            acctcateData1.Add(new SelectListItem { Text = "FORCE MAJEURE", Value = "4" });
            acctcateData1.Add(new SelectListItem { Text = "SPECIAL CONDITIONS", Value = "5" });
            acctcateData1.Add(new SelectListItem { Text = "QUANTITY INSPECTION", Value = "6" });
            acctcateData1.Add(new SelectListItem { Text = "TERMS & DELIVERY CONDITION", Value = "7" });//170707
            acctcateData1.Add(new SelectListItem { Text = "ADVISING BANK", Value = "8" });//170707
            //var catItem1 = (from c in db.studdets
            //                where c.studType == 905
            //                select new
            //                {
            //                    Text = c.studName,
            //                    Value = c.studCode
            //                }).ToList().Select(u => new SelectListItem
            //                {
            //                    Text = u.Text,
            //                    Value = u.Value.ToString()
            //                });
            //acctcateData1 = catItem1.ToList();
            obj.studCityData = acctcateData1;
            return View("TermnCondition", obj);
        }
        [HttpPost]
        public ActionResult TermnCondition(Studdet obj)//170706
        {
            //clsSubmitModel oSubmit = new clsSubmitModel();
            oSubmit.InsStudDet(obj);
            obj.studname = "";
            obj.studType = 906;
            var max = Convert.ToInt32(db.studdets.Select(t => (int?)t.studCode).Max()) + 1;
            obj.studCode = max;
            return View("TermnCondition", obj);
        }
       
        public ActionResult CreatePort()//170801
        {
            //var DispStateN1 = from res in db.vewCountries
            //                  select new { res.aCode, res.aName };
            //ViewBag.studCity = new SelectList(DispStateN1, "aCode", "aName");
            return View();
        }
        public ActionResult CreateCountry()//170801
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SetPort(string a, string b, string c)//170802
        {
            //clsSubmitModel oSubmit = new clsSubmitModel();
            Studdet oprop = new Studdet();
            oprop.studAlia = c;
            oprop.studname = a;
            oprop.studType = 903;
            oprop.studCity = int.Parse(b);

            return Json(oSubmit.InsStudDet(oprop).ToString(), JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SetCountry(string a, string b)//170803
        {
            //clsSubmitModel oSubmit = new clsSubmitModel();
            Citydet oprop = new Citydet();
            oprop.cityname = a;
            oprop.cityType = 668;
            oprop.cityalia = b;
            //oprop.cityhindi= c;

            return Json(oSubmit.InsCity(oprop).ToString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeSetup(int empCode = 0)
        {

            //CRM.Models.clsSubmitModel oSubmit = new CRM.Models.clsSubmitModel();
            EmployeeSetup frm = new EmployeeSetup();

            var vwDept = from a in db.studdets where a.studType == 11 orderby a.studName select new { itgpName = a.studName, itgpcode = a.studCode };   
            ViewBag.mstDept = new SelectList(vwDept, "itgpcode", "itgpName");

            var vwDesi = from a in db.studdets where a.studType == 1233 orderby a.studName select new { itgpName = a.studName, itgpcode = a.studCode };  
            ViewBag.iEAcc_empodesg = new SelectList(vwDesi, "itgpcode", "itgpName");
            
            var vwState = from a in db.citydets where a.cityType == 67 && a.cityrute == 3 select new { a.citycode, a.cityname }; 
            ViewBag.vwState = new SelectList(vwState, "citycode", "cityname");

            ViewBag.vwBank = new SelectList(from a in db.studdets where a.studType == 163 select new { a.studCode, a.studName }, "studCode", "studName");
             
            if (empCode > 0)
            {
                frm = oSubmit.GetEmployeeDet("select empCode iEmp_empcode , empname sEmp_empname,  PAN sEmp_PAN,  Gender iEmp_Gender,  ProofNo sEmp_ProofNo,  AcNo sEmp_AcNo, Official_No sOfficial_No,Residential_No sResidential_No,ReferenceNm sReferenceNm , ReferenceNo sReferenceNo,FatherNo sFatherNo, TemporaryAdd sTemporaryAdd, PermanentAdd sPermanentAdd,EmailID sEmailID ,BirthDt dEAcc_BirthDt ,Father ,Aadhar ,IFSC ,DegreeNM ,DegreeID ,empcity iEmp_empcity ,empStat sEmp_empstat ,BloodGroup sBloodGroup ,PfAcNo ,BankID ,empDepa mstDept,empDesi iEAcc_empodesg , IMG_Path sPath, IMG_Path  ,AadharCard , VoterId , Licence , Passport , Statement , Degree  from employee where empcode =  " + empCode);

                if (frm.IMG_Path != null && frm.IMG_Path.ToString() != "")
                    ViewBag.vbImgPath = frm.IMG_Path;
                else
                    ViewBag.vbImgPath = "../../img/avatar.png";

                frm.dEAcc_BirthDts = Convert.ToDateTime(frm.dEAcc_BirthDt).ToString("dd/MM/yyyy");
                frm.isEdit = true;
            }
            else
            {

                DataTable dt;
                dt = oSubmit.GetData("select isnull(max(empcode),0)+1 empcode from employee", true);
                frm.iEmp_empcode = Convert.ToInt32(dt.Rows[0]["empcode"]);
                ViewBag.vbImgPath = "../../img/avatar.png";
                frm.isEdit = false;

            }

            //frm.lstEmployeeSetup = oSubmit.EmployeeList("select empCode iEmp_empcode , empname sEmp_empname,  PAN sEmp_PAN,  Gender iEmp_Gender,  ProofNo sEmp_ProofNo,  AcNo sEmp_AcNo, Official_No sOfficial_No,Residential_No sResidential_No,ReferenceNm sReferenceNm , ReferenceNo sReferenceNo,FatherNo sFatherNo, TemporaryAdd sTemporaryAdd, PermanentAdd sPermanentAdd,EmailID sEmailID from employee");

            return PartialView(frm);
        }


        //      try
        //{
        //    HttpFileCollection hfc = HttpContext.Current.Request.Files;
        //    string path = "/content/files/contact/";

        //    for (int i = 0; i < hfc.Count; i++)
        //    {
        //        HttpPostedFile hpf = hfc[i];
        //        if (hpf.ContentLength > 0)
        //        {
        //            string fileName = "";
        //            if (Request.Browser.Browser == "IE")
        //            {
        //                fileName = Path.GetFileName(hpf.FileName);
        //            }
        //            else
        //            {
        //                fileName = hpf.FileName;
        //            }
        //            string fullPathWithFileName = path + fileName;
        //            hpf.SaveAs(Server.MapPath(fullPathWithFileName));
        //        }
        //    }

        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}

        //public JsonResult InsEmployee(EmployeeSetup emp)
        //{ 

        //    var File = emp.ImageFile;
        //    if (File != null)
        //    {
        //        var sFileName = Path.GetFileName(File.FileName);
        //        var sExt = Path.GetExtension(File.FileName);
        //        var sFileNameWithoutExt = Path.GetFileNameWithoutExtension(File.FileName);

        //        File.SaveAs(Server.MapPath("~/UploadImg/" + File.FileName));

        //    }
        //    return Json(File.FileName, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult InsEmployee__(string sEmp_empname, string sFatherHindi, string sEmp_PAN, string sEmp_IDProof, string sPhone, string sEmp_empstat, string sEmp_AcNo, string sEmailID, string sOfficial_No, string sResidential_No, string sFatherNo, string sEAcc_empocpno, string sReferenceNm, string sReferenceNo, string sTemporaryAdd, string sPermanentAdd, int mstDept, int iEAcc_empodesg, int iEmp_Gender, int iEmp_empcity, string dEAcc_empojodts, string dEAcc_BirthDts, string BloodGroup, string Aadhar, int BankID, string PfAcNo, string IFSC, string DegreeNM, int DegreeID, string IMG_Path, bool AadharCard, bool VoterId, bool Licence, bool Passport, bool Statement, bool Degree, EmployeeSetup emp)
        //{

        //    //var File = emp.ImageFile;
        //    //if (File != null)
        //    //{
        //    //    var sFileName = Path.GetFileName(File.FileName);
        //    //    var sExt = Path.GetExtension(File.FileName);
        //    //    var sFileNameWithoutExt = Path.GetFileNameWithoutExtension(File.FileName);

        //    //    File.SaveAs(Server.MapPath("~/UploadImg/" + File.FileName));

        //    //}


        //    EmployeeSetup frm = new EmployeeSetup();
        //    CRM.Models.clsSubmitModel oSubmit = new CRM.Models.clsSubmitModel();
        //    DataTable dt;
        //    dt = oSubmit.GetData("select isnull(max(empcode),0)+1 empcode from employee");
        //    frm.iEmp_empcode = Convert.ToInt32(dt.Rows[0]["empcode"]);

        //    frm.Father = sFatherHindi;
        //    frm.iCompCode = 0;
        //    frm.sEmp_empname = sEmp_empname;
        //    frm.sFatherHindi = sFatherHindi;
        //    frm.sEmp_PAN = sEmp_PAN;
        //    frm.sEmp_IDProof = sEmp_IDProof;
        //    frm.sPhone = sPhone;
        //    frm.sEmp_empstat = sEmp_empstat;
        //    frm.sEmp_AcNo = sEmp_AcNo;
        //    frm.sEmailID = sEmailID;
        //    frm.sOfficial_No = sOfficial_No;
        //    frm.sResidential_No = sResidential_No;
        //    frm.sFatherNo = sFatherNo;
        //    frm.sEAcc_empocpno = sEAcc_empocpno;
        //    frm.sReferenceNm = sReferenceNm;
        //    frm.sReferenceNo = sReferenceNo;
        //    frm.sTemporaryAdd = sTemporaryAdd;
        //    frm.sPermanentAdd = sPermanentAdd;

        //    frm.mstDept = mstDept.ToString();
        //    frm.iEAcc_empodesg = iEAcc_empodesg;
        //    frm.iEmp_Gender = iEmp_Gender;
        //    frm.iEmp_empcity = iEmp_empcity;
        //    frm.Aadhar = Aadhar;
        //    frm.dEAcc_empojodts = dEAcc_empojodts;
        //    frm.BankID = BankID;
        //    if (dEAcc_BirthDts.ToString() != "") frm.dEAcc_BirthDt = oSubmit.GetDateFormat(dEAcc_BirthDts);
        //    frm.PfAcNo = PfAcNo;
        //    frm.sBloodGroup = BloodGroup;
        //    frm.IFSC = IFSC; frm.DegreeNM = DegreeNM; frm.DegreeID = DegreeID;

        //    frm.IMG_Path = IMG_Path;
        //    frm.AadharCard = AadharCard;
        //    frm.VoterId = VoterId;
        //    frm.Licence = Licence;
        //    frm.Passport = Passport;
        //    frm.Statement = Statement;
        //    frm.Degree = Degree;

        //    oSubmit.InsEmployee(frm);



        //    dt = oSubmit.GetData("select isnull(max(empcode),0)+1 empcode from employee");
        //    frm.iEmp_empcode = Convert.ToInt32(dt.Rows[0]["empcode"]);

        //    return Json(dt.Rows[0]["empcode"], JsonRequestBehavior.AllowGet);
        //    // return RedirectToAction("superDash", "Home");
        //    //return PartialView("", "Home");

        //}

        public ActionResult _EmpList()
        {
            //CRM.Models.clsSubmitModel oSubmit = new CRM.Models.clsSubmitModel();

            List<EmployeeSetup> lstFollow = oSubmit.EmployeeList("select usetype as empUserType,empCode iEmp_empcode , dbo.ProperCase(empname) sEmp_empname,  PAN sEmp_PAN,  Gender iEmp_Gender,  ProofNo sEmp_ProofNo,  AcNo sEmp_AcNo, Official_No sOfficial_No,Residential_No sResidential_No,ReferenceNm sReferenceNm , ReferenceNo sReferenceNo,FatherNo sFatherNo, TemporaryAdd sTemporaryAdd, PermanentAdd sPermanentAdd,EmailID sEmailID , Aadhar , convert(varchar(20), BirthDt ,103) dEAcc_BirthDts , Father ,IFSC ,DegreeID ,DegreeNM ,empcity iEmp_empcity,  empstat sEmp_empstat , BankID ,empDepa mstDept ,empDesi iEAcc_empodesg ,PfAcNo , IMG_Path sPath, IMG_Path ,b.UseName sEmpUser from employee a left join loginusers b on a.UseCode = b.UseCode order by sEmp_empname");

            return PartialView("_EmpList", lstFollow);
        }

        [HttpPost]
        public ActionResult InsEmployee(EmployeeSetup frm)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();

            string sPath = "";
            var File = frm.ImageFile;
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
            else if ( frm.sPath != null && frm.sPath.ToString() != "")
            {
                sPath = frm.sPath;
            }

            frm.IMG_Path = sPath;
            DataTable dt;
            if (frm.isEdit != true)
            {
                dt = oSubmit.GetData("select isnull(max(empcode),0)+1 empcode from employee", true);
                frm.iEmp_empcode = Convert.ToInt32(dt.Rows[0]["empcode"]);
            }

            if (frm.dEAcc_BirthDts != null && frm.dEAcc_BirthDts.ToString() != "") frm.dEAcc_BirthDt = oSubmit.GetDateFormat(frm.dEAcc_BirthDts);

            oSubmit.InsEmployee(frm);

            dt = oSubmit.GetData("select isnull(max(empcode),0)+1 empcode from employee", true);
            frm.iEmp_empcode = Convert.ToInt32(dt.Rows[0]["empcode"]);
            ViewBag.vbImgPath = "../../img/avatar.png";
            frm.isEdit = false;
            return Json(dt.Rows[0]["empcode"], JsonRequestBehavior.AllowGet);

            // return Json(1, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DepartmentSetup()
        {

            //CRM.Models.clsSubmitModel oSubmit = new CRM.Models.clsSubmitModel();
            studdet frm = new studdet();

            var vwDepartment = from a in db.studdets where a.studType == 11 orderby a.studName select new { itgpName = a.studName, itgpcode = a.studCode };
            ViewBag.vwDepartment = new SelectList(vwDepartment, "itgpcode", "itgpName");


            return PartialView(frm);
        }

        public ActionResult CityMaster()
        {
            var vwState = from a in db.citydets where a.cityType == 67 && a.cityrute == 3 select new { a.citycode, a.cityname };

            ViewBag.vwState = new SelectList(vwState, "citycode", "cityname");

            return View();
        }

        public ActionResult InsCity(int StateID, string CityName)
        {

          clsSubmitModel oSubmit = new clsSubmitModel();

            int MstCode = 0;
            DataTable dt;
            dt = oSubmit.GetData("select  isnull(max(citycode),0)+1 citycode from citydet", true);
            MstCode = Convert.ToInt32(dt.Rows[0]["citycode"].ToString());

            oSubmit.insertData("insert into citydet (citycode  ,  cityname ,citytype, cityrute) values ( " + MstCode + ",'" + CityName + "',15," + StateID + "  )", true);

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsTahsil(int DistrictID, string Tahsil)
        {

            clsSubmitModel oSubmit = new clsSubmitModel();

            int MstCode = 0;
            DataTable dt;
            dt = oSubmit.GetData("select  isnull(max(citycode),0)+1 citycode from citydet", true);
            MstCode = Convert.ToInt32(dt.Rows[0]["citycode"].ToString());

            oSubmit.insertData("insert into citydet (citycode  ,  cityname ,citytype, cityrute) values ( " + MstCode + ",'" + Tahsil + "',20," + DistrictID + "  )" ,true);

            return Json(1, JsonRequestBehavior.AllowGet);
        }
 public ActionResult GetCityList(int iState = 0)
        {

            var F_LeadNo = from a in db.citydets where a.cityType == 15 && a.cityrute == iState orderby  a.cityname select new { a.citycode, a.cityname }; 
            //ViewBag.F_LeadNo = new SelectList(F_LeadNo, "itgpcode", "itgpName");

            return Json(F_LeadNo, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTahsilList(int iState = 0)
        {

            var F_LeadNo = from a in db.citydets where a.cityType == 20 && a.cityrute == iState orderby a.cityname select new { a.citycode, a.cityname }; 
             
            return Json(F_LeadNo, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public ActionResult Register(HttpPostedFileBase image, EmployeeSetup account)
        {

            account.sEmp_empname = account.sEmp_empname;


            return Json("");
        }

        public ActionResult SourceMaster(int iCode = 0 ,int iType = 50)
        {

            //CRM.Models.clsSubmitModel oSubmit = new CRM.Models.clsSubmitModel();
            studdet frm = new studdet();
            frm.studType = iType;

            if (iCode > 0)
            {
                frm = oSubmit.GetSource("select * from studdet where  studCode =" + iCode + " and studType =" + iType);
            }

            return PartialView(frm);
        }

        [HttpPost]
        public ActionResult InsSource(studdet frm)
        {
            clsSubmitModel oSubmit = new clsSubmitModel();

            //int MstCode = 0;
            DataTable dt;
            if (frm.studCode > 0)
            {
                oSubmit.insertData("Update studdet Set studName ='" + frm.studName + "' , studadd1 ='" + frm.studadd1 + "' Where studCode =" + frm.studCode + " and studType =" + frm.studType, true);
            }
            else
            {
                dt = oSubmit.GetData("select  isnull(max(studCode),0)+1 studCode from studdet where studType =" + frm.studType, true);
                frm.studCode = Convert.ToInt32(dt.Rows[0]["studCode"].ToString());
                oSubmit.insertData("insert into studdet (studCode, studName , studType ,studadd1) values ( " + frm.studCode + ",'" + frm.studName + "'," + frm.studType + ",'" + frm.studadd1 + "')", true); 
            }
            

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSourceList(int iType)
        {
            //CRM.Models.clsSubmitModel oSubmit = new CRM.Models.clsSubmitModel();

            List<studdet> lstFollow = oSubmit.GetSourceList("Select studType, studCode ,dbo.ProperCase(studName)studName  ,studadd1 from studdet where studtype = " + iType);

            return Json(lstFollow, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DelSource(int iType, int iCode)
        {
            string sMsg = "";
            try
            {
                clsSubmitModel oSubmit = new clsSubmitModel();
                oSubmit.insertData("Delete from studdet where studtype = " + iType + " and studCode = " + iCode, true);
                sMsg = "Delete";
            }
            catch (Exception ex)
            { sMsg = ex.Message; }

            return Json(sMsg , JsonRequestBehavior.AllowGet);
        }

        //public ActionResult SchemeMaster(int iCode = 0)
        //{
        //    //CRM.Models.clsSubmitModel oSubmit = new CRM.Models.clsSubmitModel();
        //    clsScheme frm = new clsScheme();

        //    if (iCode > 0)
        //    {
        //        // frm = oSubmit.GetSource("select * from tblScheme where ID =" + iCode);
        //    }
        //    return PartialView(frm);
        //}
        //[HttpPost]
        //public ActionResult InsScheme(clsScheme frm)
        //{
        //    CRM.Models.clsCrmConnection oSubmit = new CRM.Models.clsCrmConnection();

        //    int MstCode = 0;
        //    DataTable dt;
        //    dt = oSubmit.GetData("select  isnull(max(ID),0)+1 studCode from tblScheme  ");
        //    MstCode = Convert.ToInt32(dt.Rows[0]["studCode"].ToString());

        //    oSubmit.insertData("exec InsScheme @ID =" + MstCode + " , @SchName ='" + frm.SchName + "',@NoL =" + frm.NoL + ", @Margin =" + frm.Margin + " ,@SchType =" + frm.SchType + ", @StartDt ='" + oSubmit.GetDateFormat(frm.StartDt.ToString()) + "' ,@EndDt ='" + oSubmit.GetDateFormat(frm.EndDt.ToString()) + "'");

        //    return Json(1, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult GetSchemeList()
        //{
        //  //  CRM.Models.clsSubmitModel oSubmit = new CRM.Models.clsSubmitModel();

        //    List<clsScheme> lstFollow = oSubmit.GetSchemeList("Select * from tblScheme ");

        //    return Json(lstFollow, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult EmpAllotMst()
        { 
            var vbDealer = from a in db.tblDistributors join b in db.citydets on new { _Type = 15, _code =(int) a.CityID_I } equals new { _Type = b.cityType, _code = b.citycode } into c from d in c.DefaultIfEmpty() where a.DistributorID > 0 where a.ApprovID > 0 orderby a.DisName select new { itgpName = a.DisName + ' '+ d.cityname ?? "" , itgpcode = a.MstCode }; 
            ViewBag.vbDealer = new SelectList(vbDealer, "itgpcode", "itgpName");

            var vbEmployee = from a in db.employees  orderby a.empname select new {itgpcode = a.empcode ,itgpName = a.empname };
            ViewBag.vbEmployee = new SelectList(vbEmployee, "itgpcode", "itgpName");

            return PartialView();
        }

        [HttpPost]
        public ActionResult InsEmpAllotMst(clsEmpAllotMst frm)
        {

            clsSubmitModel oSubmit = new clsSubmitModel();

            oSubmit.insertData("Exec InsEmpAllotMst @EmpID = " + frm.EmpID + " , @DealerID= " + frm.DealerID  ,true );

            var vwDealerList = from a in db.EmpAllotMsts
                               join b in db.tblDistributors on new { _ID = a.DealerID } equals new { _ID =(int?) b.MstCode }
                               where b.DistributorID >0 where a.EmpID == frm.EmpID
                               select new { a.DealerID ,a.EmpID ,a.MstCode , b.DisName, b.ContactPerson, b.Mob1  };
                               
             
            return Json(vwDealerList, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult delEmpAllot(int iEmp , int iDealer )
        {
            string s = "";
            try
            {    
                    var EmpAllot = (from a in db.EmpAllotMsts where a.EmpID == iEmp && a.DealerID == iDealer select a).FirstOrDefault();
                    db.EmpAllotMsts.DeleteOnSubmit(EmpAllot);
                    db.SubmitChanges();
                    s = "Delete Successfully !"; 

                return Json(s, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

[HttpPost]
        public ActionResult delEmpAllotMulti(string     sMstCode )
        {
            string s = "";
            string[] Row = sMstCode.Split('|');

            try
            {
                foreach (string itm in Row)
                {
                    if (itm !=""){
                    var EmpAllot = (from a in db.EmpAllotMsts where a.MstCode == Convert.ToInt32(itm) select a).FirstOrDefault();
                    db.EmpAllotMsts.DeleteOnSubmit(EmpAllot);
                    db.SubmitChanges();
                    }
                }

                    s = "Delete Successfully !"; 

                return Json(s, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult getDealerList(int empID)
        {

            clsSubmitModel oSubmit = new clsSubmitModel();

            List<clsEmpAllotMst> lstDealerList = oSubmit.getDealerList(empID);

            if (empID > 0)
                return Json(lstDealerList, JsonRequestBehavior.AllowGet);
            else
                return PartialView("_DealerList", lstDealerList);
        }

         
        public ActionResult SendMailQT(string To ,string Msg,string Subject)
        {
            int sMsg = 0;
            try
            {  
                commFunction oComm =new commFunction();
               // oComm.Send_Mail("support@digiclayinfotech.co.in", "sSubject", "sBody");
                               // oComm.Email_With_CCandBCC("", "", "", "", "");

                const string SERVER2 = "relay-hosting.secureserver.net";
                sMsg = 1;
                System.Web.Mail.MailMessage oMail2 = new System.Web.Mail.MailMessage();
                oMail2.From = "support@digiclayinfotech.co.in";
                oMail2.To = "ajay@digiclayinfotech.com";
                oMail2.Subject = "Acknowledgement";
                oMail2.BodyFormat = System.Web.Mail.MailFormat.Html; // enumeration
                oMail2.Priority = System.Web.Mail.MailPriority.High; // enumeration 
                sMsg = 2; 
                oMail2.Body = "sBody";
                System.Web.Mail.SmtpMail.SmtpServer = SERVER2;
                System.Web.Mail.SmtpMail.Send(oMail2);
                oMail2 = null;
                sMsg = 3;
                //string from = "ajay@digiclayinfotech.com"; //any valid GMail ID   
                //using (MailMessage mail = new MailMessage(from, To))
                //{
                //    mail.Subject =Subject  ;//
                //    mail.Body = Msg;
                //    //if (fileUploader != null)
                //    //{
                //    //    string fileName = Path.GetFileName(fileUploader.FileName);
                //    //    mail.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
                //    //}
                //    mail.IsBodyHtml = false;
                //    SmtpClient smtp = new SmtpClient();
                //    smtp.Host = "50.62.22.144";// "smtp.digiclayinfotech.com";
                //    smtp.EnableSsl = true;
                //    NetworkCredential networkCredential = new NetworkCredential(from, "ajay123");
                //    smtp.UseDefaultCredentials = true;
                //    smtp.Credentials = networkCredential;
                //    smtp.Port = 587;
                //    smtp.Host = "50.62.22.144"; //"localhost";
                //    smtp.Send(mail);
                //    ViewBag.Message = "Sent";
                //   // return View("Index", objModelMail);
                //}   
                 
                return Json(sMsg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("E_"+ sMsg +"_"+ ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult del_Tahsil(int iCityCode)
        {
            string s = "";
            try
            {
                s = "Category is in Use !";
                if (iCityCode > 0)
                {
                    var Tahsil = (from a in db.citydets where a.citycode == iCityCode && a.cityType == 20 select a).FirstOrDefault();
                    db.citydets.DeleteOnSubmit(Tahsil);
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

        public ActionResult Dist_Deal( bool isDistributor =true)
        {
        Distributor oDist = new Distributor();
        clsSubmitModel oCon = new clsSubmitModel();

            var vwCate = from a in db.tblDistributors where a.DistributorID == 0 select new { a.DisName, a.MstCode };
            ViewBag.vwDistributor = new SelectList(vwCate, "MstCode", "DisName");

            ViewBag.isDistributor = isDistributor;

            var vwState = from a in db.citydets where a.cityType == 67 && a.cityrute == 3 orderby a.cityname select new { a.citycode, a.cityname }; 
            ViewBag.vwState = new SelectList(vwState, "citycode", "cityname");
              
            if (isDistributor ==true)
                oDist.Dis_ID = (db.tblDistributors.Where(x => x.DistributorID == 0).Count()).ToString();
            else
                oDist.Dis_ID = (db.tblDistributors.Where(x => x.DistributorID != 0).Count()).ToString();
             
            oDist.IsDistributor = isDistributor;
             
            oDist.lstBrand = oCon.GetList(61);
            oDist.lstProduct = oCon.GetProduct();
            return View(oDist);
        }

        public ActionResult GUI()
        {
            return View();
        }
        public ActionResult ApprovDealerList()
        {
            List<tblDistributor> oList = (from a in db.tblDistributors where a.DistributorID > 0  where a.ApprovID == null select a).ToList(); 
            return View(oList);
        }
        [HttpPost]
public ActionResult ApprovDealerList(string DealerList )
        {
             
            string Message = "Successfully Approved ...";
            try
            {
                string[] Row = DealerList.Split('|');
                string[] Col;
                string sPass = "";
                StreamReader objReader;
                Stream objStream;
                string sSMS = "WelCome To Unitech ...";
                foreach (string itm in Row)
                {
                    Col = itm.Split(',');
                    if (Col[0].ToString() != "")
                    {
                        tblDistributor o = (from a in db.tblDistributors where a.MstCode == Convert.ToInt32(Col[0]) select a).SingleOrDefault();
                        o.ApprovID = SessionMaster.UserID;
                        db.SubmitChanges();

                        if (Col[1].ToString().Length == 10)
                        {
                            objStream = WebRequest.Create("http://216.245.209.132/rest/services/sendSMS/sendGroupSms?AUTH_KEY=8d2f63772eef7559f01d321c6868e5bb&message=" + sSMS + "&senderId=UNITEC&routeId=1&mobileNos=" + Col[1] + "&smsContentType=english").GetResponse().GetResponseStream();
                            objReader = new StreamReader(objStream);
                            objReader.Close();
                        }
 
                    }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            ///List<tblDistributor> oList = (from a in db.tblDistributors where a.DistributorID > 0 where (int?)a.ApprovID == 0 select a).ToList();
            //return Json(oList, JsonRequestBehavior.AllowGet);
            return Json(Message, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SendMail(int Port, string Host)
        {
            string Message = "Successfully Approved ...";
            try
            { 
                string sSMS = "WelCome To Unitech ...";
                 
                SmtpClient smtpClient = new SmtpClient(Host, Port);

                smtpClient.Credentials = new System.Net.NetworkCredential("ajay@digiclayinfotech.com", "ajay123");
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage mailMessage = new MailMessage("ajay@digiclayinfotech.com", "zoeb@digiclayinfotech.com");
                mailMessage.Subject = sSMS;
                mailMessage.Body = sSMS;

                smtpClient.Send(mailMessage);
                 
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
 
            return Json(Message, JsonRequestBehavior.AllowGet);

        }



        public ActionResult City()
        {
            ViewBag.MenCode = Request.QueryString["MenCode"];
          
            var vwState = from a in db.citydets where a.cityType == 67 where a.cityrute == 3 orderby a.cityname select new { a.citycode  ,  a.cityname} ;
            ViewBag.vwState = new SelectList(vwState, "citycode", "cityname");

            return View();
        }
        [HttpPost]
        public ActionResult InsCity(int StateID, string CityName, int iCityCode)
        { 
            //submitData oSubmit = new submitData();
            clsSubmitModel oSubmit = new clsSubmitModel();
            if (iCityCode > 0)
            {
                oSubmit.insertData("Update citydet set cityname ='" + CityName + "' where citycode = " + iCityCode ,true);
            }
            else
            {
                DataTable dt;
                dt = oSubmit.GetData("select  isnull(max(citycode),0)+1 citycode from citydet", true);
                iCityCode = Convert.ToInt32(dt.Rows[0]["citycode"].ToString());
                oSubmit.insertData("insert into citydet (citycode  ,  cityname ,citytype, cityrute) values ( " + iCityCode + ",'" + CityName + "',15," + StateID + "  )",true);
                commFunction oCom = new commFunction(); 
                CodeGen oCode = new CodeGen();
                oCode.MstCode = iCityCode;
                oCode.compcode = 0;// SessionMaster.CompCode;
                oCode.msttype = 15;

                oCode.mstdate = DateTime.Now.Date;
                oCode.StDate = Convert.ToDateTime(oCom.getOpenDate(DateTime.Now.Date));
                oCode.LastDate = Convert.ToDateTime(oCom.getClosDate(DateTime.Now.Date));
                oSubmit.UpdCodeGen_API(oCode);

            }


            return Json(1, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetCityList(int iState = 0)
        //{
        //    var F_LeadNo = from a in db.citydets where a.cityType == 15 where a.cityrute == iState orderby a.cityname select new { a.citycode, a.cityname };       
             
        //    return Json(F_LeadNo, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult DealerList(string aType="0")
        {  
            var vwCate = from a in db.tblDistributors where a.DistributorID == 0 select new { a.DisName, a.MstCode };
            ViewBag.vwDistributor = new SelectList(vwCate, "MstCode", "DisName");

            var vwState = from a in db.citydets where a.cityType == 67 && a.cityrute == 3 select new { a.citycode, a.cityname };
            ViewBag.vwState = new SelectList(vwState, "citycode", "cityname");
            ViewBag.aType = aType;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetDealer(string sName )
        {
            //  string comp = Convert.ToString(Session["CompCode"]);
            string comp = SessionMaster.CompCode.ToString();
             clsSubmitModel oSubmit = new  clsSubmitModel();
            DataTable dt;

            dt = oSubmit.GetData("select acctname + ' - ' + isnull(CityName ,'') acctname, acctCode from account a  inner join tblDistributor b on a.acctDeal = b.MstCode  left join citydet c on c.citytype = 15 and a.acctcity = c.citycode where  a.acctgrou = 21 and b.ApprovID >0  and acctname like '%" + sName + "%' order by acctname", true);
            var result = new List<KeyValuePair<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new KeyValuePair<string, string>(dr["acctcode"].ToString(), dr["acctname"].ToString()));
            }
            var result3 = result.Where(s => s.Value.ToLower().Contains(sName.ToLower())).Select(w => w).ToList();

            return Json(result3, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Cateogry(string cateory, int iType = 0)
        {
            //  string comp = Convert.ToString(Session["CompCode"]);
            string comp = SessionMaster.CompCode.ToString();
             clsSubmitModel oSubmit = new  clsSubmitModel();
            DataTable dt;
            string sCondition = "";
            if (iType == 108)
                sCondition = " and acctgrou in(23,24)";
            else if (iType == 34)
                sCondition = " and acctgrou = 34";
            else if (iType == 42 || iType == 43)
                sCondition = " and acctgrou in ( 21 ,22 ,23)";

            //dt = oSubmit.GetData("select acctname ,acctCode from account where acctname like '%" + cateory + "%' and compcode =" + comp +sCondition+ " order by acctname");
            //By Ajay On 21092017
            //dt = oSubmit.GetData("select acctname ,acctCode from account where acctname like '%" + cateory + "%' and compcode =" + comp +sCondition+ " order by acctname");
            dt = oSubmit.GetData("select acctname + ' - ' + isnull(CityName ,'') acctname, acctCode from account a  left join citydet c on c.citytype = 15 and a.acctcity = c.citycode where acctname like '%" + cateory + "%' and  a.compcode =" + comp + sCondition + " order by acctname", true);
            var result = new List<KeyValuePair<string, string>>();
            foreach (DataRow dr in dt.Rows)
            {
                result.Add(new KeyValuePair<string, string>(dr["acctcode"].ToString(), dr["acctname"].ToString()));
            }
            var result3 = result.Where(s => s.Value.ToLower().Contains(cateory.ToLower())).Select(w => w).ToList();

            return Json(result3, JsonRequestBehavior.AllowGet);
        }

        public ActionResult tblCallMst()
        {
            ERPDataContext DB = new ERPDataContext(); 
            ViewBag.vwState = new SelectList((from a in DB.citydets where a.cityType == 67 && a.cityrute == 3 orderby a.cityname select new { a.citycode, a.cityname }), "citycode", "cityname");
            return PartialView("tblCallMst");
        }
        [HttpPost]
        public ActionResult tblCallMst(tblCallMst  frm)
        {
            string sMsg = "";
            try
            {
                ERPDataContext DB = new ERPDataContext(); 

                if ((from a in DB.tblCallMsts where a.Mobile == frm.Mobile select a).Count() == 0)
                {
                    frm.ID = Convert.ToInt32(DB.tblCallDets.Max(a => (int?)a.ID) + 1);

                    frm.UseCode = SessionMaster.UserID;
                    frm.CreatedOn = DateTime.Now;

                    DB.tblCallMsts.InsertOnSubmit(frm);
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

        [HttpPost]
        public ActionResult MapDealer(int DealID, int AcctID)
        {
            string Message = "Saved";
            try
            {
                tblMapDealer oTbl = new tblMapDealer(); 
                oTbl.DealID = DealID;
                oTbl.AcctID = AcctID;
                db.tblMapDealers.InsertOnSubmit(oTbl);
                db.SubmitChanges(); 
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            } 

            return Json(Message, JsonRequestBehavior.AllowGet);
              
        }

        public ActionResult getMapDealer(int DealID)
        {
     var vwDealerList = from a in db.tblMapDealers
                               join b in db.accounts on new { _ID = a.AcctID } equals new { _ID = (int?)b.acctcode }
                               where b.acctgrou == 21
                        where a.DealID == DealID
                               select new { a.DealID,  a.AcctID, b.acctname };
   return Json(vwDealerList, JsonRequestBehavior.AllowGet);
              
        }
        public ActionResult MapDealer()
        {
            //var vbDealer = from a in db.tblDistributors join b in db.citydets on new { _Type = 15, _code = (int)a.CityID_I } equals new { _Type = b.cityType, _code = b.citycode } into c from d in c.DefaultIfEmpty() where a.DistributorID > 0 where a.ApprovID > 0 orderby a.DisName select new { itgpName = a.DisName + ' ' + d.cityname ?? "", itgpcode = a.MstCode };

            var vbDealer = from a in db.tblDistributors join b in db.citydets on new { _Type = 15, _code = (int)a.CityID_I } equals new { _Type = b.cityType, _code = b.citycode } into c from d in c.DefaultIfEmpty() where a.DistributorID > 0 where a.ApprovID > 0 orderby a.DisName select new { itgpName = a.DisName , itgpcode = a.MstCode };

            ViewBag.vbDealer = new SelectList(vbDealer, "itgpcode", "itgpName");

            var vbAccount = from a in db.accounts join b in db.citydets on new { _Type = 15, _code = (int)a.acctcity } equals new { _Type = b.cityType, _code = b.citycode } into c from d in c.DefaultIfEmpty()  where a.acctgrou == 21 orderby a.acctname select new { itgpcode = a.acctcode, itgpName = a.acctname + ' ' + d.cityname ?? "" };
            ViewBag.vbAccount = new SelectList(vbAccount, "itgpcode", "itgpName");

            return PartialView();
        }

        [HttpPost]
        public ActionResult delMapDealer(int AcctID, int iDealer)
        {
            string s = "";
            try
            {
                var EmpAllot = (from a in db.tblMapDealers where a.AcctID == AcctID && a.DealID == iDealer select a).FirstOrDefault();
                db.tblMapDealers.DeleteOnSubmit(EmpAllot);
                db.SubmitChanges();
                s = "Delete Successfully !";

                return Json(s, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

         [HttpGet]
        public ActionResult GatePass()
        {
            return PartialView("GatePass");
        }
         [HttpGet]
         public ActionResult Forecast()
        {
            return PartialView("Forecast");
        }
    }
}

