using System;
using System.Collections.Generic;
using System.Globalization;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Web.Script.Serialization;

namespace CompxERP.Models
{
    public class clsSubmitModel
    {
        //public static SqlConnection con = null;
        public SqlConnection con = null;
        public SqlCommand cmd = null;
        public SqlDataAdapter da = null;
        //public SqlDataReader dr = null;
        public DataSet ds = null;
        public DataTable dt = null;
        public string retValue;
        public DataRow drow;
        public String sql;
        public int iCompanyCode;

        public void GetConection()
        {
            if (con == null)
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBEntity"].ConnectionString);
                con.Open();
            }
            else
            {
                if (con.State == ConnectionState.Open)
                {
                }
                else
                {
                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBEntity"].ConnectionString);
                    con.Open();
                }
            }
        }
        public DataTable GetAccount(string sName, int iType, int iCompCode = 0)
        {

            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = "GetAccount";
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            cmd.Parameters.AddWithValue("@Name", sName);
            cmd.Parameters.AddWithValue("@Type", iType);

            if (iCompCode > 0)
                cmd.Parameters.AddWithValue("@CompCode", iCompCode);

            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            return dt;
        }
        public DataTable GetData(string sQry, bool IsClose = false)
        {

            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = sQry;
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 3000;
            dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (IsClose == true) // Add By Ajay On 25072017 Pool Size 
                con.Close();

            return dt;
        }
        public object GetSingleData(String sQuery, String sReturn = "0", bool IsClose = false)
        {
            SqlCommand cmdL = null;
            GetConection();
            cmdL = new SqlCommand(sQuery, con);
            cmdL.CommandTimeout = 3000;
            object s = cmdL.ExecuteScalar();
            //  CloseConnection(); //12-10-2016

            if (IsClose == true)
                con.Close();

            if (s != null && s.ToString() != "")
                return s;
            else
                return sReturn;
        }
        public DataTable GetItem()
        {

            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = "GetItem";
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            return dt;
        }

        public void insertData(String s, bool IsClose = false)
        {
            long rowA;
            SqlCommand cmdL = null;
            //if (dr != null)
            //    dr.Close();
            GetConection();
            cmdL = new SqlCommand(s, con);
            rowA = cmdL.ExecuteNonQuery();
            cmdL.Dispose();

            if (IsClose == true)
                con.Close();
        }
        //public void CloseDr()
        //{
        //    if (dr != null)
        //        dr.Close();

        //    if (con != null)
        //    {
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //}
        public void Commit()
        {
            sql = "commit";
            InitilizeCommand(1);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void CloseConnection()
        {
            if (con != null)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        public void BeginTran()
        {
            GetConection();
            sql = "begin transaction";
            InitilizeCommand(1);
            cmd.ExecuteNonQuery();
        }
        public void InitilizeCommand(int s)
        {
            cmd = new SqlCommand(sql, con);
            if (s == 1)
                cmd.CommandType = CommandType.Text;
            else
                cmd.CommandType = CommandType.StoredProcedure;
        }
        public void RollBack()
        {
            sql = "rollback";
            InitilizeCommand(1);
            cmd.ExecuteNonQuery();
            con.Close();

        }

        public DateTime GetDateFormat(string Date)
        {
            string dd, mm, yy;
            dd = Date.Substring(0, 2);
            mm = Date.Substring(3, 2);
            yy = Date.Substring(6, 4);
            DateTime dt = DateTime.ParseExact(dd + "/" + mm + "/" + yy, "d/M/yyyy", CultureInfo.InvariantCulture);
            return Convert.ToDateTime(dt);

        }
        public DateTime GetDate(string Date)
        {
            string[] Row = Date.Split('/');
            string dd, mm, yy;
            dd = GetDD(Row[0]);
            mm = GetDD(Row[1]);
            yy = Row[2].Substring(0, 4);
            DateTime dt = DateTime.ParseExact(dd + "/" + mm + "/" + yy, "d/M/yyyy", CultureInfo.InvariantCulture);
            return Convert.ToDateTime(dt);

        }


        public string GetDD(string ChNo)
        {
            if (ChNo.Length == 1)
                return "0" + ChNo;
            else
                return ChNo;


        }


        public DataTable GetItem(string sName, string sType = "", int Compcode = 0, int iGrouID = 0)
        {

            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = "GetItem";
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            cmd.Parameters.AddWithValue("@Name", sName);

            if (sType != "") cmd.Parameters.AddWithValue("@Type", sType);

            if (iGrouID > 0) cmd.Parameters.AddWithValue("@GrouID", iGrouID);

            cmd.Parameters.AddWithValue("@Comp", Compcode);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();
            return dt;
        }
        public DataTable getCategory(string sName, int Compcode = 0)
        {

            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = "getCategory";
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            cmd.Parameters.AddWithValue("@Name", sName);

            cmd.Parameters.AddWithValue("@Comp", Compcode);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            return dt;
        }
        public DataTable getGroup(string sName, int Compcode = 0, int CateID = 0)
        {

            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = "getGroup";
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            cmd.Parameters.AddWithValue("@Name", sName);
            cmd.Parameters.AddWithValue("@CateID", CateID);
            cmd.Parameters.AddWithValue("@Comp", Compcode);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            return dt;
        }

        public DataTable GetCity(string sName, int iType)
        {

            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = "GetCity";
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            cmd.Parameters.AddWithValue("@Name", sName);
            cmd.Parameters.AddWithValue("@Type", iType);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            return dt;
        }

        public DataTable GetCategoryInfo(string sName, string comp)//170513
        {
            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = "GetCategoryInfo";
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            cmd.Parameters.AddWithValue("@Name", sName);
            cmd.Parameters.AddWithValue("@CompCode", comp);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            return dt;
        }
        public DataTable GetSubCategoryInfo(string sName, string comp, string catcode)//170513
        {
            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = "GetSubCategoryInfo";
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            cmd.Parameters.AddWithValue("@Name", sName);
            cmd.Parameters.AddWithValue("@CompCode", comp);
            cmd.Parameters.AddWithValue("@CatCode", catcode);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            return dt;
        }

        public DataTable GetCity(string sName)
        {

            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = "GetCity ";

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            cmd.Parameters.AddWithValue("@Name", sName);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            return dt;
        }
        public DataTable GetState(string sName, int iType)
        {
            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = "GetState ";

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            cmd.Parameters.AddWithValue("@Name", sName);
            cmd.Parameters.AddWithValue("@Type", iType);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            return dt;
        }
        public DataTable GetCountry(string sName)
        {
            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = "GetCountry ";

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            cmd.Parameters.AddWithValue("@Name", sName);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            return dt;
        }

        public DataSet GetDataSet(string sQry, bool IsClose = false)//170826
        {
            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = sQry;
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            if (IsClose == true) // Add By Ajay On 25072017 Pool Size 
                con.Close();

            return ds;
        }

        public bool spInsUser(int compcode, int usecode, int oPropusetype, int useshowtr, string usename, string usepass, int useref, string UserNM = "", string Remark = "", string UsePhone = "", string UseEmail = "", int UseDesi = 0, int UseDepa = 0, int UseDealer = 0)
        {

            GetConection();
            SqlCommand objcmd;
            objcmd = new SqlCommand("spEmployeeLogin", con);
            //objcmd.Parameters.Add("@trninde", SqlDbType.Int).Value = oProp.trninde;
            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = compcode;
            objcmd.Parameters.Add("@usecode", SqlDbType.Int).Value = usecode;
            objcmd.Parameters.Add("@usetype", SqlDbType.Int).Value = oPropusetype;
            objcmd.Parameters.Add("@useshowtr", SqlDbType.Int).Value = useshowtr;
            objcmd.Parameters.Add("@usename", SqlDbType.VarChar).Value = usename;
            objcmd.Parameters.Add("@usepass", SqlDbType.VarChar).Value = usepass;
            objcmd.Parameters.Add("@useref ", SqlDbType.Int).Value = useref;

            if (UserNM != null && UserNM != "") objcmd.Parameters.Add("@UserNM", SqlDbType.VarChar).Value = UserNM;
            if (Remark != null && Remark != "") objcmd.Parameters.Add("@Remark", SqlDbType.VarChar).Value = Remark;

            if (UsePhone != null && UsePhone != "") objcmd.Parameters.Add("@UsePhone", SqlDbType.VarChar).Value = UsePhone;
            if (UseEmail != null && UseEmail != "") objcmd.Parameters.Add("@UseEmail", SqlDbType.VarChar).Value = UseEmail;
            if (UseDesi != null && UseDesi != 0) objcmd.Parameters.Add("@UseDesi", SqlDbType.VarChar).Value = UseDesi;
            if (UseDepa != null && UseDepa != 0) objcmd.Parameters.Add("@UseDepa", SqlDbType.VarChar).Value = UseDepa;
            if (UseDealer != null && UseDealer != 0) objcmd.Parameters.Add("@UseDealer", SqlDbType.VarChar).Value = UseDealer;

            objcmd.ExecuteNonQuery();
            CloseConnection();
            return true;
        }

        public List<CompxERP.Models.clsFollow> GetFollowUpList(int iStatus = 0, int iEmpCode = 0, int iDays = 0, int iType = 0, int isDue = 0)
        {
            GetConection();

            var oPara = new DynamicParameters();
            oPara.Add("@Status", iStatus);
            oPara.Add("@EmpID", iEmpCode);
            oPara.Add("@Days", iDays);
            oPara.Add("@IsLead", iType);
            oPara.Add("@isDue", isDue);

            var Plan_list = con.Query<CompxERP.Models.clsFollow>("GetFollowUp", oPara, null, true, 0, CommandType.StoredProcedure).ToList();
            CloseConnection();
            return Plan_list;
        }
        public List<CompxERP.Models.clsUser> UserList(string sQry)
        {
            GetConection();

            var Plan_list = con.Query<CompxERP.Models.clsUser>(sQry, null, null, true, 0, CommandType.Text).ToList();
            CloseConnection();
            return Plan_list;
        }
        public List<CompxERP.Models.EmployeeSetup> EmployeeList(string sQry)
        {
            GetConection();

            var Plan_list = con.Query<CompxERP.Models.EmployeeSetup>(sQry, null, null, true, 0, CommandType.Text).ToList();
            CloseConnection();
            return Plan_list;
        }
        public CompxERP.Models.EmployeeSetup GetEmployeeDet(string sQry)
        {
            GetConection();

            var Plan_list = con.Query<CompxERP.Models.EmployeeSetup>(sQry, null, null, true, 0, CommandType.Text).Single();
            CloseConnection();
            return Plan_list;
        }
        public CompxERP.Models.studdet GetSource(string sQry)
        {
            GetConection();

            var Plan_list = con.Query<CompxERP.Models.studdet>(sQry, null, null, true, 0, CommandType.Text).Single();
            CloseConnection();
            return Plan_list;
        }
        public List<CompxERP.Models.studdet> GetSourceList(string sQry)
        {
            GetConection();

            var Plan_list = con.Query<CompxERP.Models.studdet>(sQry, null, null, true, 0, CommandType.Text).ToList();
            CloseConnection();
            return Plan_list;
        }


        public bool InsEmployee(EmployeeSetup oProp)
        {
            GetConection();
            SqlCommand objcmd = new SqlCommand("InsEmployee", con);
            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
            objcmd.Parameters.Add("@emparea", SqlDbType.Int).Value = oProp.iEmp_emparea;
            objcmd.Parameters.Add("@empbudget", SqlDbType.Int).Value = oProp.iEmp_empbudget;
            objcmd.Parameters.Add("@empcate", SqlDbType.Int).Value = oProp.iEmp_empcate;
            objcmd.Parameters.Add("@empcity", SqlDbType.Int).Value = oProp.iEmp_empcity;
            objcmd.Parameters.Add("@empcode", SqlDbType.Int).Value = oProp.iEmp_empcode;
            objcmd.Parameters.Add("@empdsa", SqlDbType.Int).Value = oProp.iEmp_empdsa;
            objcmd.Parameters.Add("@empdsap", SqlDbType.Int).Value = oProp.iEmp_empdsap;
            objcmd.Parameters.Add("@empgrou", SqlDbType.Int).Value = oProp.iEmp_empgrou;
            objcmd.Parameters.Add("@empincome", SqlDbType.Int).Value = oProp.iEmp_empincome;
            objcmd.Parameters.Add("@empitrd", SqlDbType.Int).Value = oProp.iEmp_empitrd;
            objcmd.Parameters.Add("@empledg", SqlDbType.Int).Value = oProp.iEmp_empledg;
            objcmd.Parameters.Add("@empnomi", SqlDbType.Int).Value = oProp.iEmp_empnomi;
            //objcmd.Parameters.Add("@empprty", SqlDbType.Int).Value = oProp.iEmp_empprty;

            objcmd.Parameters.Add("@empaddr", SqlDbType.VarChar).Value = oProp.sEmp_empaddr;
            objcmd.Parameters.Add("@empalia", SqlDbType.VarChar).Value = oProp.sEmp_empalia;
            objcmd.Parameters.Add("@emphind", SqlDbType.NVarChar).Value = oProp.sEmp_emphind;
            objcmd.Parameters.Add("@empname", SqlDbType.VarChar).Value = oProp.sEmp_empname;
            objcmd.Parameters.Add("@emppath", SqlDbType.VarChar).Value = oProp.sEmp_emppath;
            objcmd.Parameters.Add("@empphon", SqlDbType.VarChar).Value = oProp.sEmp_empphon;
            objcmd.Parameters.Add("@emppinc", SqlDbType.VarChar).Value = oProp.sEmp_emppinc;
            objcmd.Parameters.Add("@emprefn", SqlDbType.VarChar).Value = oProp.sEmp_emprefn;
            objcmd.Parameters.Add("@emprema", SqlDbType.VarChar).Value = oProp.sEmp_emprema;
            objcmd.Parameters.Add("@empsort", SqlDbType.VarChar).Value = oProp.sEmp_empsort;
            objcmd.Parameters.Add("@empstat", SqlDbType.VarChar).Value = oProp.sEmp_empstat;


            objcmd.Parameters.Add("@acwithbl", SqlDbType.VarChar).Value = oProp.dEmp_acwithbl;
            objcmd.Parameters.Add("@empagen", SqlDbType.VarChar).Value = oProp.dEmp_empagen;
            objcmd.Parameters.Add("@empclcr", SqlDbType.VarChar).Value = oProp.dEmp_empclcr;
            objcmd.Parameters.Add("@empcldr", SqlDbType.VarChar).Value = oProp.dEmp_empcldr;
            objcmd.Parameters.Add("@empjmbl", SqlDbType.VarChar).Value = oProp.dEmp_empjmbl;
            objcmd.Parameters.Add("@empopcr", SqlDbType.VarChar).Value = oProp.dEmp_empopcr;
            objcmd.Parameters.Add("@empopdr", SqlDbType.VarChar).Value = oProp.dEmp_empopdr;
            objcmd.Parameters.Add("@emprate", SqlDbType.VarChar).Value = oProp.dEmp_emprate;

            objcmd.Parameters.Add("@PAN", SqlDbType.VarChar).Value = oProp.sEmp_PAN;
            objcmd.Parameters.Add("@ESI", SqlDbType.VarChar).Value = oProp.sEmp_ESI;
            objcmd.Parameters.Add("@IDProof", SqlDbType.VarChar).Value = oProp.sEmp_IDProof;
            objcmd.Parameters.Add("@ProofNo", SqlDbType.VarChar).Value = oProp.sEmp_ProofNo;
            objcmd.Parameters.Add("@AcNo", SqlDbType.VarChar).Value = oProp.sEmp_AcNo;
            objcmd.Parameters.Add("@PfAcNo", SqlDbType.VarChar).Value = oProp.PfAcNo;
            objcmd.Parameters.Add("@BankID", SqlDbType.Int).Value = oProp.BankID;
            objcmd.Parameters.Add("@pfBankID", SqlDbType.Int).Value = oProp.iEmp_pfBankID;

            objcmd.Parameters.Add("@Gender", SqlDbType.Int).Value = oProp.iEmp_Gender;
            objcmd.Parameters.Add("@CastID", SqlDbType.Int).Value = oProp.iEmp_CastID;
            objcmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = oProp.iEmp_CategoryID;

            objcmd.Parameters.Add("@BirthDt", SqlDbType.DateTime).Value = oProp.dEAcc_BirthDt;
            objcmd.Parameters.Add("@AnvDt", SqlDbType.DateTime).Value = oProp.dEAcc_AnvDt;
            objcmd.Parameters.Add("@VetanMaan", SqlDbType.VarChar).Value = oProp.sEmp_VetanMaan;

            if (oProp.mstDept != null && oProp.mstDept != "null") objcmd.Parameters.Add("@empDepa", SqlDbType.Int).Value = oProp.mstDept;
            objcmd.Parameters.Add("@empDesi", SqlDbType.Int).Value = oProp.iEAcc_empodesg;

            objcmd.Parameters.Add("@Official_No", SqlDbType.VarChar).Value = oProp.sOfficial_No;
            objcmd.Parameters.Add("@Residential_No", SqlDbType.VarChar).Value = oProp.sResidential_No;
            objcmd.Parameters.Add("@ReferenceNm", SqlDbType.VarChar).Value = oProp.sReferenceNm;
            objcmd.Parameters.Add("@ReferenceNo", SqlDbType.VarChar).Value = oProp.sReferenceNo;
            objcmd.Parameters.Add("@FatherNo", SqlDbType.VarChar).Value = oProp.sFatherNo;
            objcmd.Parameters.Add("@TemporaryAdd", SqlDbType.VarChar).Value = oProp.sTemporaryAdd;
            objcmd.Parameters.Add("@PermanentAdd", SqlDbType.VarChar).Value = oProp.sPermanentAdd;
            objcmd.Parameters.Add("@EmailID", SqlDbType.VarChar).Value = oProp.sEmailID;
            objcmd.Parameters.Add("@BloodGroup", SqlDbType.VarChar).Value = oProp.sBloodGroup;
            objcmd.Parameters.Add("@Aadhar", SqlDbType.VarChar).Value = oProp.Aadhar;
            objcmd.Parameters.Add("@Father", SqlDbType.VarChar).Value = oProp.Father;
            objcmd.Parameters.Add("@DegreeNM", SqlDbType.VarChar).Value = oProp.DegreeNM;
            objcmd.Parameters.Add("@DegreeID", SqlDbType.Int).Value = oProp.DegreeID;
            objcmd.Parameters.Add("@IFSC", SqlDbType.VarChar).Value = oProp.IFSC;

            objcmd.Parameters.Add("@IMG_Path", SqlDbType.VarChar).Value = oProp.IMG_Path;

            objcmd.Parameters.Add("@AadharCard", SqlDbType.Bit).Value = oProp.AadharCard;
            objcmd.Parameters.Add("@VoterId", SqlDbType.Bit).Value = oProp.VoterId;
            objcmd.Parameters.Add("@Licence", SqlDbType.Bit).Value = oProp.Licence;
            objcmd.Parameters.Add("@Passport", SqlDbType.Bit).Value = oProp.Passport;
            objcmd.Parameters.Add("@Statement", SqlDbType.Bit).Value = oProp.Statement;
            objcmd.Parameters.Add("@Degree", SqlDbType.Bit).Value = oProp.Degree;

            objcmd.Parameters.Add("@usename", SqlDbType.VarChar).Value = oProp.sEmpUser;
            objcmd.Parameters.Add("@usepass", SqlDbType.VarChar).Value = oProp.sEmpPass;
            objcmd.Parameters.Add("@usetype", SqlDbType.VarChar).Value = oProp.empUserType;
            objcmd.ExecuteNonQuery();
            CloseConnection();
            return true;
        }

        public List<SelectListItem> getList(DataTable dt, string Text, string Value)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new SelectListItem { Text = dr[Text].ToString(), Value = dr[Value].ToString() });
            }

            return lst;


        }

        public DataTable getUserWork(clsUserWork oProp)//170513
        {
            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = "getUserWork";
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            cmd.Parameters.AddWithValue("@UserID", oProp.woruser);
            cmd.Parameters.AddWithValue("@To", oProp.wordate);
            cmd.Parameters.AddWithValue("@From", oProp.worcudt);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            return dt;
        }

        public bool InsUserWork(clsUserWork oProp)
        {
            GetConection();
            SqlCommand objcmd = new SqlCommand("InsUserWork", con);
            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            objcmd.Parameters.Add("@woruser", SqlDbType.Int).Value = oProp.woruser;
            objcmd.Parameters.Add("@wormode", SqlDbType.Int).Value = oProp.wormode;
            objcmd.Parameters.Add("@worcomp", SqlDbType.Int).Value = oProp.worcomp;
            objcmd.Parameters.Add("@wortype", SqlDbType.Int).Value = oProp.wortype;
            objcmd.Parameters.Add("@worcode", SqlDbType.Int).Value = oProp.worcode;
            objcmd.Parameters.Add("@worsrno", SqlDbType.VarChar).Value = oProp.worsrno;
            objcmd.Parameters.Add("@worrema", SqlDbType.VarChar).Value = oProp.worrema;
            objcmd.Parameters.Add("@worrfsr", SqlDbType.VarChar).Value = "0";
            objcmd.Parameters.Add("@worsysn", SqlDbType.VarChar).Value = oProp.worsysn;
            objcmd.Parameters.Add("@wordate", SqlDbType.DateTime).Value = oProp.wordate;
            objcmd.Parameters.Add("@worcudt", SqlDbType.DateTime).Value = DateTime.Now;

            objcmd.ExecuteNonQuery();
            return true;
        }

        public bool InsLogg(string logcode, int loguser, DateTime logstdt, DateTime logendt, string IP_Add)
        {
            GetConection();
            SqlCommand objcmd = new SqlCommand("InsLogg", con);
            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            objcmd.Parameters.Add("@logcode", SqlDbType.VarChar).Value = logcode;
            objcmd.Parameters.Add("@loguser", SqlDbType.Int).Value = loguser;
            objcmd.Parameters.Add("@logstdt", SqlDbType.DateTime).Value = logstdt;
            objcmd.Parameters.Add("@logendt", SqlDbType.DateTime).Value = logendt;
            objcmd.Parameters.Add("@IP_Add", SqlDbType.VarChar).Value = IP_Add;

            objcmd.ExecuteNonQuery();
            return true;
        }
        public int insertandupdatecatitem(clsItemain oProp)
        {
            GetConection();
            SqlCommand objcmd = new SqlCommand("insertandupdatecatitem", con);
            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            objcmd.Parameters.Add("@itemname", SqlDbType.VarChar).Value = oProp.itemname;
            objcmd.Parameters.Add("@itemalia", SqlDbType.VarChar).Value = oProp.itemalia;
            objcmd.Parameters.Add("@itempart", SqlDbType.VarChar).Value = oProp.itempart;
            objcmd.Parameters.Add("@itemtext", SqlDbType.VarChar).Value = oProp.itemtext;
            objcmd.Parameters.Add("@itemhind", SqlDbType.VarChar).Value = oProp.itemhind;
            objcmd.Parameters.Add("@itemrefn", SqlDbType.VarChar).Value = oProp.itemrefn;
            objcmd.Parameters.Add("@itemtype", SqlDbType.VarChar).Value = oProp.itemtype;
            //objcmd.Parameters.Add("@itemimg", SqlDbType.VarChar).Value = oProp.itemimg ;
            objcmd.Parameters.Add("@itgpnamec", SqlDbType.VarChar).Value = oProp.itgpnamec;
            objcmd.Parameters.Add("@itgpnamesc", SqlDbType.VarChar).Value = oProp.itgpnamesc;
            objcmd.Parameters.Add("@itgpaliac", SqlDbType.VarChar).Value = oProp.itgpaliac;
            objcmd.Parameters.Add("@itgpaliasc", SqlDbType.VarChar).Value = oProp.itgpaliasc;
            //objcmd.Parameters.Add("@itgptext", SqlDbType.VarChar).Value = oProp.itgptext ;
            objcmd.Parameters.Add("@itgptyp", SqlDbType.VarChar).Value = oProp.itgptyp;
            objcmd.Parameters.Add("@itemhsnc", SqlDbType.VarChar).Value = oProp.itemhsnc;
            //objcmd.Parameters.Add("@itgprefn", SqlDbType.VarChar).Value = oProp.itgprefn ;
            // objcmd.Parameters.Add("@itgprefn1", SqlDbType.VarChar).Value = oProp.itgprefn1 ;

            objcmd.Parameters.Add("@itemsrno", SqlDbType.Int).Value = oProp.itemsrno;
            objcmd.Parameters.Add("@itemsort", SqlDbType.Int).Value = oProp.itemsort;
            objcmd.Parameters.Add("@itemvalu", SqlDbType.Int).Value = oProp.itemvalu;
            objcmd.Parameters.Add("@itebesic", SqlDbType.Int).Value = oProp.itebesic;
            //objcmd.Parameters.Add("@SrNo", SqlDbType.Int).Value = oProp.SrNo  ;
            //objcmd.Parameters.Add("@category", SqlDbType.Int).Value = oProp.category  ;
            //objcmd.Parameters.Add("@subcategory", SqlDbType.Int).Value = oProp.subcategory  ;
            //objcmd.Parameters.Add("@itgpsortc", SqlDbType.Int).Value = oProp.itgpsortc  ;
            // objcmd.Parameters.Add("@itgpsortsc", SqlDbType.Int).Value = oProp.itgpsortsc  ;
            objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.compcode;
            objcmd.Parameters.Add("@itemcode", SqlDbType.Int).Value = oProp.itemcode;
            objcmd.Parameters.Add("@itemgrou", SqlDbType.Int).Value = oProp.itemgrou;
            objcmd.Parameters.Add("@itemnumb", SqlDbType.Int).Value = oProp.itemnumb;
            objcmd.Parameters.Add("@itgpcode", SqlDbType.Int).Value = oProp.CatID;//CatID
            //objcmd.Parameters.Add("@itgpunde", SqlDbType.Int).Value =oProp.itgpunde   ;
            //objcmd.Parameters.Add("@itgpbasi", SqlDbType.Int).Value =oProp.itgpbasi   ;
            //objcmd.Parameters.Add("@itgpnumb", SqlDbType.Int).Value =oProp.itgpnumb   ;
            //objcmd.Parameters.Add("@itemgrou1", SqlDbType.Int).Value =oProp.itemgrou1   ;
            objcmd.Parameters.Add("@itgpcode1", SqlDbType.Int).Value = oProp.SubCatID;//Subca
            //objcmd.Parameters.Add("@StkVlAsPerPrchPrcLst", SqlDbType.Int).Value =oProp.StkVlAsPerPrchPrcLst   ;

            objcmd.Parameters.Add("@itemmini", SqlDbType.Decimal).Value = oProp.itemmini;
            objcmd.Parameters.Add("@itemmaxi", SqlDbType.Decimal).Value = oProp.itemmaxi;
            objcmd.Parameters.Add("@itemrate", SqlDbType.Decimal).Value = oProp.itemrate;
            objcmd.Parameters.Add("@itlastrat", SqlDbType.Decimal).Value = oProp.itlastrat;
            objcmd.Parameters.Add("@itemdisc", SqlDbType.Decimal).Value = oProp.itemdisc;
            objcmd.Parameters.Add("@itemvatr", SqlDbType.Decimal).Value = oProp.itemvatr;
            objcmd.Parameters.Add("@itemmrpv", SqlDbType.Decimal).Value = oProp.itemmrpv;
            objcmd.Parameters.Add("@iteper", SqlDbType.Decimal).Value = oProp.iteper;
            objcmd.Parameters.Add("@itemPrcnt", SqlDbType.Decimal).Value = oProp.itemPrcnt;
            // objcmd.Parameters.Add("@itgpbcqt", SqlDbType.Decimal).Value =oProp.itgpbcqt   ;
            objcmd.Parameters.Add("@itemgstr", SqlDbType.Decimal).Value = oProp.itemgstr;
            objcmd.Parameters.Add("@itemcess", SqlDbType.Decimal).Value = oProp.itemcess;

            return Convert.ToInt32(objcmd.ExecuteScalar());
        }
        public List<CompxERP.Models.clsItemain> GetItemList(int Compcode, int SubCatID = 0)
        {
            GetConection();

            var oPara = new DynamicParameters();
            oPara.Add("@Compcode ", Compcode);
            if (SubCatID > 0) oPara.Add("@SubCatID ", SubCatID);

            var ItemList = con.Query<CompxERP.Models.clsItemain>("GetItemList", oPara, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
            CloseConnection();
            return ItemList;

        }


        public bool InsDistributor(int MstCode, DateTime Dis_Date, string Dis_Country, string Dis_State, string Dis_City, string Dis_Add1, string Dis_Add2, string Dis_Add3, string Dis_pobox, string Dis_City_I, int DistributorID, string DisName, string ContactPerson, string Mob1, string Mob2, string LandLine, string Email, string Skype, string GSTIN, int usetype, string usename, string usepass, string DealCode, string CityNM, int AdminID = 0, string Brand = "", string Product = "", string ChequeNo = "", string CP_I = "", string CP_II = "", string CP_NO_I = "", string CP_NO_II = "", string DOB = "", string DOJ = "", string AnniDt = "")
        {
            GetConection();
            SqlCommand objcmd;
            objcmd = new SqlCommand("InsDistributor", con);
            //objcmd.Parameters.Add("@trninde", SqlDbType.Int).Value = oProp.trninde;
            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            objcmd.Parameters.Add("@MstCode", SqlDbType.Int).Value = MstCode;
            objcmd.Parameters.Add("@MstDate", SqlDbType.DateTime).Value = Dis_Date;

            if (DOB != "") objcmd.Parameters.Add("@DOB", SqlDbType.DateTime).Value = Convert.ToDateTime(GetDateFormat(DOB));
            if (DOJ != "") objcmd.Parameters.Add("@DOJ", SqlDbType.DateTime).Value = Convert.ToDateTime(GetDateFormat(DOJ));
            if (AnniDt != "") objcmd.Parameters.Add("@AnniDt", SqlDbType.DateTime).Value = Convert.ToDateTime(GetDateFormat(AnniDt));

            objcmd.Parameters.Add("@DistributorID", SqlDbType.Int).Value = DistributorID;
            objcmd.Parameters.Add("@DisName", SqlDbType.VarChar).Value = DisName;
            objcmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = Dis_Country;
            objcmd.Parameters.Add("@StateID", SqlDbType.Int).Value = Dis_State;
            objcmd.Parameters.Add("@CityID", SqlDbType.Int).Value = Dis_City;
            objcmd.Parameters.Add("@CityID_I", SqlDbType.Int).Value = Dis_City_I;
            objcmd.Parameters.Add("@Add_I", SqlDbType.VarChar).Value = Dis_Add1;
            objcmd.Parameters.Add("@Add_II", SqlDbType.VarChar).Value = Dis_Add2;
            objcmd.Parameters.Add("@Add_III", SqlDbType.VarChar).Value = Dis_Add3;
            objcmd.Parameters.Add("@Add_IV", SqlDbType.VarChar).Value = "";
            objcmd.Parameters.Add("@PO_Box", SqlDbType.VarChar).Value = Dis_pobox;

            objcmd.Parameters.Add("@ContactPerson", SqlDbType.VarChar).Value = ContactPerson;
            objcmd.Parameters.Add("@Mob1", SqlDbType.VarChar).Value = Mob1;
            objcmd.Parameters.Add("@Mob2", SqlDbType.VarChar).Value = Mob2;
            objcmd.Parameters.Add("@LandLine", SqlDbType.VarChar).Value = LandLine;
            objcmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;
            objcmd.Parameters.Add("@Skype", SqlDbType.VarChar).Value = Skype;
            objcmd.Parameters.Add("@GSTIN", SqlDbType.VarChar).Value = GSTIN;

            objcmd.Parameters.Add("@usetype", SqlDbType.Int).Value = usetype;
            objcmd.Parameters.Add("@usename", SqlDbType.VarChar).Value = usename;
            objcmd.Parameters.Add("@usepass", SqlDbType.VarChar).Value = usepass;
            objcmd.Parameters.Add("@DealCode", SqlDbType.VarChar).Value = DealCode;

            objcmd.Parameters.Add("@CityNM", SqlDbType.VarChar).Value = CityNM;
            objcmd.Parameters.Add("@AdminID", SqlDbType.VarChar).Value = AdminID;

            objcmd.Parameters.Add("@ChequeNo", SqlDbType.VarChar).Value = ChequeNo;
            objcmd.Parameters.Add("@CP_I", SqlDbType.VarChar).Value = CP_I;
            objcmd.Parameters.Add("@CP_II", SqlDbType.VarChar).Value = CP_II;
            objcmd.Parameters.Add("@CP_NO_I", SqlDbType.VarChar).Value = CP_NO_I;
            objcmd.Parameters.Add("@CP_NO_II", SqlDbType.VarChar).Value = CP_NO_II;

            objcmd.Parameters.Add("@Brand", SqlDbType.VarChar).Value = Brand;
            objcmd.Parameters.Add("@Product", SqlDbType.VarChar).Value = Product;

            objcmd.ExecuteNonQuery();
            return true;
        }
        public List<CompxERP.Models.clsDistributor> GetDistributor(string sQuery)
        {
            GetConection();
            var Plan_list = con.Query<CompxERP.Models.clsDistributor>(sQuery, null, null, true, 0, CommandType.Text).ToList();
            CloseConnection();
            return Plan_list;

        }
        public bool InsFollow(int MstCode, int F_by, int L_ID, int Status_I, int Status_II, string L_No, string Remark, string MstDate, string F_Date, string F_Next_Date, int isLead = 0, int F_Time = 0, string Feedback = "", int UseCode = 0)
        {
            GetConection();
            SqlCommand objcmd;
            objcmd = new SqlCommand("InsFollow", con);
            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            objcmd.Parameters.Add("@MstCode", SqlDbType.Int).Value = MstCode;
            objcmd.Parameters.Add("@F_by", SqlDbType.Int).Value = F_by;
            objcmd.Parameters.Add("@L_ID", SqlDbType.Int).Value = L_ID;
            objcmd.Parameters.Add("@Status_I", SqlDbType.Int).Value = Status_I;
            objcmd.Parameters.Add("@Status_II", SqlDbType.Int).Value = Status_II;

            objcmd.Parameters.Add("@L_No", SqlDbType.VarChar).Value = L_No;
            objcmd.Parameters.Add("@Remark", SqlDbType.VarChar).Value = Remark;

            if (MstDate != "") objcmd.Parameters.Add("@MstDate", SqlDbType.DateTime).Value = Convert.ToDateTime(GetDateFormat(MstDate));
            if (F_Date != "") objcmd.Parameters.Add("@F_Date", SqlDbType.DateTime).Value = Convert.ToDateTime(GetDateFormat(F_Date));
            if (F_Next_Date != "") objcmd.Parameters.Add("@F_Next_Date", SqlDbType.DateTime).Value = Convert.ToDateTime(GetDateFormat(F_Next_Date));

            objcmd.Parameters.Add("@isLead", SqlDbType.Int).Value = isLead;

            objcmd.Parameters.Add("@F_Time", SqlDbType.Int).Value = F_Time;
            objcmd.Parameters.Add("@Feedback", SqlDbType.VarChar).Value = Feedback;
            objcmd.Parameters.Add("@UseCode", SqlDbType.VarChar).Value = UseCode;



            objcmd.ExecuteNonQuery();
            CloseConnection();

            return true;
        }
        public DataTable SetSubCategoryInfo(itgroup prop)//170513
        {
            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = "sp_InsSubCategory";
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            cmd.Parameters.AddWithValue("@CompCode", prop.compcode);
            cmd.Parameters.AddWithValue("@LoginID", 0);
            cmd.Parameters.AddWithValue("@SysIP", 0);
            cmd.Parameters.AddWithValue("@SubName", prop.itgpname);
            cmd.Parameters.AddWithValue("@SubAlia", prop.itgpalia);
            cmd.Parameters.AddWithValue("@SubSortingNo", prop.itgpsort);
            cmd.Parameters.AddWithValue("@CatCode", prop.itgpunde);
            cmd.Parameters.AddWithValue("@CatType", prop.itgptype);
            cmd.Parameters.AddWithValue("@BasicUnit", prop.itgpbasi);
            cmd.Parameters.AddWithValue("@RateonPer", prop.itgpbcqt);
            cmd.Parameters.AddWithValue("@StkVlAsPerPrchPrcLst", prop.itgpnumb);
            cmd.Parameters.AddWithValue("@itgprefn", prop.itgprefn);//170822
            cmd.Parameters.AddWithValue("@itgpcart", prop.itgpcart);//170822

            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();
            return dt;
        }
        public string GetUsWoCode()
        {
            GetConection();
            SqlCommand objcmd = new SqlCommand("stp_GetUsWoCode", con);
            objcmd.Connection = con;
            objcmd.CommandType = CommandType.StoredProcedure;
            objcmd.Parameters.AddWithValue("@pFieldN", "gatcode");

            objcmd.Parameters.Add("@pResult", SqlDbType.NVarChar, 20);
            objcmd.Parameters["@pResult"].Direction = ParameterDirection.Output;
            objcmd.ExecuteNonQuery();
            return objcmd.Parameters["@pResult"].Value.ToString();
        }
        public DataTable DelItemInfo(itemain prop)//170513
        {
            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = "sp_DelIteMain";
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            cmd.Parameters.AddWithValue("@CompCode", prop.compcode);
            cmd.Parameters.AddWithValue("@LoginID", 0);
            cmd.Parameters.AddWithValue("@SysIP", 0);
            cmd.Parameters.AddWithValue("@ItemCode", prop.itemcode);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();
            return dt;
        }
        public string InsStudDet(Studdet oProp)//170801
        {
            GetConection();
            SqlCommand objcmd = new SqlCommand("InsStudDet", con);
            objcmd.CommandType = CommandType.StoredProcedure;

            objcmd.Parameters.AddWithValue("@studType", oProp.studType);
            objcmd.Parameters.AddWithValue("@studCode", oProp.studCode);
            objcmd.Parameters.AddWithValue("@studName", oProp.studname);
            objcmd.Parameters.AddWithValue("@studcity", oProp.studCity);
            objcmd.Parameters.AddWithValue("@studalia", oProp.studAlia);

            return objcmd.ExecuteScalar().ToString();
        }
        public string InsCity(Citydet oProp)//170803
        {
            GetConection();
            SqlCommand objcmd = new SqlCommand("InsCity", con);
            objcmd.CommandType = CommandType.StoredProcedure;

            objcmd.Parameters.AddWithValue("@Type", oProp.cityType);
            objcmd.Parameters.AddWithValue("@Name", oProp.cityname);
            objcmd.Parameters.AddWithValue("@Rute", oProp.cityrute);
            objcmd.Parameters.AddWithValue("@cityexti", oProp.cityexti);
            objcmd.Parameters.AddWithValue("@cityalia", oProp.cityalia);
            objcmd.Parameters.AddWithValue("@Hname", oProp.cityhindi);

            return objcmd.ExecuteScalar().ToString();
        }
        public DataTable GetAccountGroup(string sName, string sCompCode)//Not Uploaded GetGroup
        {
            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = "sp_GetAccountGroup";
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            cmd.Parameters.AddWithValue("@Name", sName);
            cmd.Parameters.AddWithValue("@CompCode", sCompCode);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();
            return dt;
        }
        public List<CompxERP.Models.clsEmpAllotMst> getDealerList(int EmpID)
        {
            GetConection();
            var paramater = new DynamicParameters();
            paramater.Add("@EmpID", EmpID);

            var Plan_list = con.Query<CompxERP.Models.clsEmpAllotMst>("getDealerList", paramater, null, true, 0, CommandType.StoredProcedure).ToList();
            CloseConnection();
            return Plan_list;

        }
        public List<clsList> GetList(int value)
        {
            List<clsList> lstList = new List<clsList>();
            try
            {
                GetConection();
                cmd = new SqlCommand();
                cmd.CommandText = "Select studCode NameID,studname NameNM from studdet where [studType] = " + value + " order by NameNM";
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    lstList.Add(new clsList(dr));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            CloseConnection();

            return lstList;
        }


        public List<clsList> GetProduct()
        {
            List<clsList> lstList = new List<clsList>();
            try
            {
                GetConection();
                cmd = new SqlCommand();
                cmd.CommandText = "select itgpcode NameID , itgpname NameNM from itgroup where itgpunde = 0  and Compcode = 66  order by NameNM";
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    lstList.Add(new clsList(dr));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            CloseConnection();

            return lstList;
        }
        public Jourmaster GetInquiry(int TypeCode = 0, int compCode = 0, int Code = 0, int UserCode = 0)
        {
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Mystring"].ToString()))
            //{
            GetConection();
            var paramater = new DynamicParameters();
            paramater.Add("@Comp", compCode);
            paramater.Add("@Type", TypeCode);
            paramater.Add("@Code", Code);
            paramater.Add("@UserCode", UserCode);

            var Plan_list = con.Query<Jourmaster>("GetInquiry", paramater, null, true, 0, CommandType.StoredProcedure).Single();
            CloseConnection();
            return Plan_list;
            //}
        }

        public bool InsOrdeMst(Jourmaster oProp)
        {
            // For InsJourmst / InsOrdemst /InsSapumst
            GetConection();
            SqlCommand objcmd;

            objcmd = new SqlCommand("InsOrdeMst", con);

            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.compcode;
            objcmd.Parameters.Add("@Msttype", SqlDbType.Int).Value = oProp.msttype;
            objcmd.Parameters.Add("@Mastcode", SqlDbType.Int).Value = oProp.mstcode;
            objcmd.Parameters.Add("@mstcust", SqlDbType.Int).Value = oProp.mstcust;
            objcmd.Parameters.Add("@mstdepa", SqlDbType.Int).Value = oProp.mstdepa;
            objcmd.Parameters.Add("@mstuser", SqlDbType.Int).Value = oProp.mstuser;
            objcmd.Parameters.Add("@mstfinc", SqlDbType.Int).Value = oProp.mstfinc;
            objcmd.Parameters.Add("@mstdued", SqlDbType.Int).Value = oProp.mstdued;

            objcmd.Parameters.Add("@mstrefno", SqlDbType.VarChar).Value = oProp.mstrefno;
            objcmd.Parameters.Add("@mstrefc", SqlDbType.VarChar).Value = oProp.mstrefc;
            objcmd.Parameters.Add("@mstchno", SqlDbType.VarChar).Value = oProp.mstchno;
            objcmd.Parameters.Add("@mstrema", SqlDbType.VarChar).Value = oProp.mstrema;
            objcmd.Parameters.Add("@mstblno", SqlDbType.VarChar).Value = oProp.mstblno;

            objcmd.Parameters.Add("@mstJobNo", SqlDbType.VarChar).Value = oProp.mstJobNo;
            objcmd.Parameters.Add("@mstDrawNo", SqlDbType.VarChar).Value = oProp.mstDrawNo;
            objcmd.Parameters.Add("@mstlotno", SqlDbType.VarChar).Value = oProp.mstlotno;
            objcmd.Parameters.Add("@mstsection", SqlDbType.VarChar).Value = oProp.mstsection;


            objcmd.Parameters.Add("@mstneta", SqlDbType.Money).Value = oProp.mstneta;
            objcmd.Parameters.Add("@msttota", SqlDbType.Money).Value = oProp.msttota;
            objcmd.Parameters.Add("@mstadde", SqlDbType.Money).Value = oProp.mstadde;
            objcmd.Parameters.Add("@mstdifq", SqlDbType.Money).Value = oProp.mstdifq;

            objcmd.Parameters.Add("@Mstdate", SqlDbType.DateTime).Value = oProp.mstdate;
            objcmd.Parameters.Add("@Mstbldt", SqlDbType.SmallDateTime).Value = oProp.mstbldt;

            objcmd.Parameters.Add("@msttime", SqlDbType.Int).Value = oProp.msttime;
            objcmd.Parameters.Add("@mstpaid", SqlDbType.Money).Value = oProp.mstpaid;
            objcmd.Parameters.Add("@mstbala", SqlDbType.Money).Value = oProp.mstbala;
            // objcmd.Parameters.Add("@mstconnm", SqlDbType.VarChar).Value = oProp.mstconnm;
            objcmd.Parameters.Add("@mstbrnc", SqlDbType.Int).Value = oProp.mstbrnc;
            objcmd.Parameters.Add("@mstexti", SqlDbType.VarChar).Value = oProp.mstexti;
            objcmd.Parameters.Add("@mstgncd", SqlDbType.VarChar).Value = oProp.mstgncd;

            objcmd.Parameters.Add("@mstpdc", SqlDbType.Int).Value = oProp.mstpdc;
            objcmd.Parameters.Add("@mstactpay", SqlDbType.Int).Value = oProp.mstactpay;
            //objcmd.Parameters.Add("@mstchqadj", SqlDbType.Int).Value = oProp.mstchqadj;
            objcmd.Parameters.Add("@mststat", SqlDbType.Int).Value = oProp.mststat;
            objcmd.Parameters.Add("@mstclno", SqlDbType.VarChar).Value = oProp.mstclno;
            objcmd.Parameters.Add("@mstchnm", SqlDbType.VarChar).Value = oProp.mstchnm;
            // objcmd.Parameters.Add("@PartyID", SqlDbType.Int).Value = oProp.PartyID;
            // objcmd.Parameters.Add("@DueDay", SqlDbType.Int).Value = oProp.DueDay;
            //  objcmd.Parameters.Add("@Interest", SqlDbType.Decimal).Value = oProp.Interest;
            // objcmd.Parameters.Add("@Comm", SqlDbType.Decimal).Value = oProp.Comm;
            objcmd.Parameters.Add("@mstcurrcd ", SqlDbType.Int).Value = oProp.mstcurrcd;
            objcmd.Parameters.Add("@mstptcode", SqlDbType.Int).Value = oProp.mstptcode;

            objcmd.Parameters.Add("@mstyldp", SqlDbType.Decimal).Value = oProp.mstyldp;
            objcmd.Parameters.Add("@mstchnV", SqlDbType.Int).Value = oProp.mstchnV;

            objcmd.Parameters.Add("@MstChnH", SqlDbType.VarChar).Value = oProp.mstchnH;
            objcmd.Parameters.AddWithValue("@oldwht", oProp.oldwht);  //170831
            objcmd.Parameters.AddWithValue("@mstAppr", oProp.mstAppr);//170831
            objcmd.Parameters.AddWithValue("@mstqtyd", oProp.mstqtyd);//170831
            objcmd.Parameters.AddWithValue("@mstvat1", oProp.mstvat1);//170831
            objcmd.Parameters.AddWithValue("@mstvat2", oProp.mstvat2);//170831
            objcmd.Parameters.AddWithValue("@mstvat3", oProp.mstvat3);//170831
            objcmd.Parameters.AddWithValue("@mstsite", oProp.mstsite);//170831
            objcmd.Parameters.AddWithValue("@mstsubt", oProp.mstsubt);//170831
            objcmd.Parameters.AddWithValue("@mstprtc", oProp.mstprtc);//170831
            objcmd.Parameters.AddWithValue("@msternv", oProp.msternv);//170831
            objcmd.Parameters.AddWithValue("@mstrvsc", oProp.mstrvsc);//170831
            objcmd.Parameters.AddWithValue("@mstintr", oProp.mstintr);//170831
            objcmd.Parameters.AddWithValue("@mstperd", oProp.mstperd);//170831
            objcmd.Parameters.AddWithValue("@mstIorL", oProp.mstIorL);//170831
            objcmd.Parameters.AddWithValue("@mstcurrrat", oProp.mstcurrrat);//170831
            objcmd.Parameters.AddWithValue("@mstDueDate", oProp.mstDueDate);//170831
            objcmd.Parameters.AddWithValue("@mstbuyerc", oProp.mstbuyerc);   //170831
            objcmd.Parameters.AddWithValue("@mstdsptoc", oProp.mstdsptoc);   //170831
            objcmd.Parameters.AddWithValue("@mstsale", oProp.mstsale);   //170831

            objcmd.Parameters.AddWithValue("mstbrok", oProp.mstbrok);   //170831
            objcmd.Parameters.AddWithValue("mstempo", oProp.mstempo);   //170831
            objcmd.Parameters.AddWithValue("mstexec", oProp.mstexec);   //170831
            objcmd.Parameters.AddWithValue("mstContactPerson", oProp.mstContactPerson);   //170831
            objcmd.Parameters.AddWithValue("mstContactType", oProp.mstContactType);   //170831
            objcmd.Parameters.AddWithValue("mstpaymode", oProp.mstpayMode);   //170831
            objcmd.Parameters.AddWithValue("mstreftag", oProp.mstreftag);   //170831

            objcmd.Parameters.AddWithValue("@ItemID", oProp.ItemID);   //170831
            objcmd.Parameters.AddWithValue("@ItemNM", oProp.ItemNm);   //170831

            objcmd.Parameters.Add("@mstrfvc", SqlDbType.Int).Value = oProp.mstrfvc;
            objcmd.Parameters.Add("@mstrfvt", SqlDbType.Int).Value = oProp.mstrfvt;

            objcmd.Parameters.AddWithValue("@mstpofs", oProp.mstpofs);   //170831



            objcmd.ExecuteNonQuery();
            return true;

        }

        public bool InsOrdeMst_API(jourmst oProp)
        {
            // For InsJourmst / InsOrdemst /InsSapumst
            GetConection();
            SqlCommand objcmd;

            objcmd = new SqlCommand("InsOrdeMst", con);

            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.compcode;
            objcmd.Parameters.Add("@Msttype", SqlDbType.Int).Value = oProp.msttype;
            objcmd.Parameters.Add("@Mastcode", SqlDbType.Int).Value = oProp.mstcode;
            objcmd.Parameters.Add("@mstcust", SqlDbType.Int).Value = oProp.mstcust;
            objcmd.Parameters.Add("@mstdepa", SqlDbType.Int).Value = oProp.mstdepa;
            objcmd.Parameters.Add("@mstuser", SqlDbType.Int).Value = oProp.mstuser;
            objcmd.Parameters.Add("@mstfinc", SqlDbType.Int).Value = oProp.mstfinc;

            objcmd.Parameters.Add("@mstrefno", SqlDbType.VarChar).Value = oProp.mstrefno;
            objcmd.Parameters.Add("@mstrefc", SqlDbType.VarChar).Value = oProp.mstrefc;
            objcmd.Parameters.Add("@mstchno", SqlDbType.VarChar).Value = oProp.mstchno;
            objcmd.Parameters.Add("@mstrema", SqlDbType.VarChar).Value = oProp.mstrema;
            objcmd.Parameters.Add("@mstblno", SqlDbType.VarChar).Value = oProp.mstblno;

            objcmd.Parameters.Add("@mstJobNo", SqlDbType.VarChar).Value = oProp.mstJobNo;
            objcmd.Parameters.Add("@mstDrawNo", SqlDbType.VarChar).Value = oProp.mstDrawNo;
            objcmd.Parameters.Add("@mstlotno", SqlDbType.VarChar).Value = oProp.mstlotno;
            objcmd.Parameters.Add("@mstsection", SqlDbType.VarChar).Value = oProp.mstsection;


            objcmd.Parameters.Add("@mstneta", SqlDbType.Money).Value = oProp.mstneta;
            objcmd.Parameters.Add("@msttota", SqlDbType.Money).Value = oProp.msttota;
            objcmd.Parameters.Add("@mstadde", SqlDbType.Money).Value = oProp.mstadde;
            objcmd.Parameters.Add("@mstdifq", SqlDbType.Money).Value = oProp.mstdifq;

            objcmd.Parameters.Add("@Mstdate", SqlDbType.DateTime).Value = oProp.mstdate;
            objcmd.Parameters.Add("@Mstbldt", SqlDbType.SmallDateTime).Value = oProp.mstbldt;

            objcmd.Parameters.Add("@msttime", SqlDbType.Int).Value = oProp.msttime;
            objcmd.Parameters.Add("@mstpaid", SqlDbType.Money).Value = oProp.mstpaid;
            objcmd.Parameters.Add("@mstbala", SqlDbType.Money).Value = oProp.mstbala;
            // objcmd.Parameters.Add("@mstconnm", SqlDbType.VarChar).Value = oProp.mstconnm;
            objcmd.Parameters.Add("@mstbrnc", SqlDbType.Int).Value = oProp.mstbrnc;
            objcmd.Parameters.Add("@mstexti", SqlDbType.VarChar).Value = oProp.MSTEXTI;
            objcmd.Parameters.Add("@mstgncd", SqlDbType.VarChar).Value = oProp.mstgncd;

            objcmd.Parameters.Add("@mstpdc", SqlDbType.Int).Value = oProp.mstpdc;
            objcmd.Parameters.Add("@mstactpay", SqlDbType.Int).Value = oProp.mstactpay;
            //objcmd.Parameters.Add("@mstchqadj", SqlDbType.Int).Value = oProp.mstchqadj;
            objcmd.Parameters.Add("@mststat", SqlDbType.Int).Value = oProp.mststat;
            objcmd.Parameters.Add("@mstclno", SqlDbType.VarChar).Value = oProp.mstclno;
            objcmd.Parameters.Add("@mstchnm", SqlDbType.VarChar).Value = oProp.mstchnm;
            // objcmd.Parameters.Add("@PartyID", SqlDbType.Int).Value = oProp.PartyID;
            // objcmd.Parameters.Add("@DueDay", SqlDbType.Int).Value = oProp.DueDay;
            //  objcmd.Parameters.Add("@Interest", SqlDbType.Decimal).Value = oProp.Interest;
            // objcmd.Parameters.Add("@Comm", SqlDbType.Decimal).Value = oProp.Comm;
            objcmd.Parameters.Add("@mstcurrcd ", SqlDbType.Int).Value = oProp.mstcurrcd;
            objcmd.Parameters.Add("@mstptcode", SqlDbType.Int).Value = oProp.mstptcode;

            objcmd.Parameters.Add("@mstyldp", SqlDbType.Decimal).Value = oProp.mstyldp;
            objcmd.Parameters.Add("@mstchnV", SqlDbType.Int).Value = oProp.mstchnV;

            objcmd.Parameters.Add("@MstChnH", SqlDbType.VarChar).Value = oProp.mstchnH;
            objcmd.Parameters.AddWithValue("@oldwht", oProp.oldwht);  //170831
            objcmd.Parameters.AddWithValue("@mstAppr", oProp.mstappr);//170831
            objcmd.Parameters.AddWithValue("@mstqtyd", oProp.mstqtyd);//170831
            objcmd.Parameters.AddWithValue("@mstvat1", oProp.mstvat1);//170831
            objcmd.Parameters.AddWithValue("@mstvat2", oProp.mstvat2);//170831
            objcmd.Parameters.AddWithValue("@mstvat3", oProp.mstvat3);//170831
            objcmd.Parameters.AddWithValue("@mstsite", oProp.mstsite);//170831
            objcmd.Parameters.AddWithValue("@mstsubt", oProp.mstsubt);//170831
            objcmd.Parameters.AddWithValue("@mstprtc", oProp.mstprtc);//170831
            objcmd.Parameters.AddWithValue("@msternv", oProp.msternv);//170831
            objcmd.Parameters.AddWithValue("@mstrvsc", oProp.mstrvsc);//170831
            objcmd.Parameters.AddWithValue("@mstintr", oProp.mstintr);//170831
            objcmd.Parameters.AddWithValue("@mstperd", oProp.mstperd);//170831
            objcmd.Parameters.AddWithValue("@mstIorL", oProp.mstIorL);//170831
            objcmd.Parameters.AddWithValue("@mstcurrrat", oProp.mstcurrrat);//170831
            objcmd.Parameters.AddWithValue("@mstDueDate", oProp.mstDueDate);//170831
            objcmd.Parameters.AddWithValue("@mstbuyerc", oProp.mstbuyerc);   //170831
            objcmd.Parameters.AddWithValue("@mstdsptoc", oProp.mstdsptoc);   //170831
            objcmd.Parameters.AddWithValue("@mstsale", oProp.mstsale);   //170831

            objcmd.Parameters.AddWithValue("mstbrok", oProp.mstbrok);   //170831
            objcmd.Parameters.AddWithValue("mstempo", oProp.mstempo);   //170831
            objcmd.Parameters.AddWithValue("mstexec", oProp.mstexec);   //170831
            objcmd.Parameters.AddWithValue("mstContactPerson", oProp.mstContactPerson);   //170831
            objcmd.Parameters.AddWithValue("mstContactType", oProp.mstContactType);   //170831
            objcmd.Parameters.AddWithValue("mstpaymode", oProp.mstpayMode);   //170831
            objcmd.Parameters.AddWithValue("mstreftag", oProp.mstreftag);   //170831

            //objcmd.Parameters.AddWithValue("@ItemID", oProp.ItemID);   //170831
            //objcmd.Parameters.AddWithValue("@ItemNM", oProp.ItemNm);   //170831

            objcmd.Parameters.Add("@mstrfvc", SqlDbType.Int).Value = oProp.mstrfvc;
            objcmd.Parameters.Add("@mstrfvt", SqlDbType.Int).Value = oProp.mstrfvt;

            objcmd.Parameters.AddWithValue("@mstpofs", oProp.mstpofs);   //170831



            objcmd.ExecuteNonQuery();
            return true;

        }
        public bool insOrdeItd(sapuitd oProp, string sTable = "")
        {
            GetConection();
            SqlCommand objcmd;
            objcmd = new SqlCommand("insOrdeItd", con);
            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            objcmd.Parameters.AddWithValue("@compcode", oProp.compcode);
            objcmd.Parameters.AddWithValue("@itdtype", oProp.itdtype);
            objcmd.Parameters.AddWithValue("@itdcode", oProp.itdcode);
            objcmd.Parameters.AddWithValue("@itdsrno", oProp.itdsrno);
            objcmd.Parameters.AddWithValue("@itditem", oProp.itditem);
            objcmd.Parameters.AddWithValue("@itdunit", oProp.itdunit);
            objcmd.Parameters.AddWithValue("@itddime", oProp.itddime);
            objcmd.Parameters.AddWithValue("@itdmill", oProp.itdmill);
            objcmd.Parameters.AddWithValue("@itdgodo", oProp.itdgodo);
            objcmd.Parameters.AddWithValue("@itdrefs", oProp.itdrefs);
            objcmd.Parameters.AddWithValue("@itdtime", oProp.itdtime);
            objcmd.Parameters.AddWithValue("@itdempo", oProp.itdempo);
            objcmd.Parameters.AddWithValue("@itdquan", oProp.itdquan);
            objcmd.Parameters.AddWithValue("@itdrate", oProp.itdrate);
            objcmd.Parameters.AddWithValue("@itdamou", oProp.itdamou);
            objcmd.Parameters.AddWithValue("@itdpkin", oProp.itdpkin);
            objcmd.Parameters.AddWithValue("@itdrefq", oProp.itdrefq);
            objcmd.Parameters.AddWithValue("@itdpenq", oProp.itdpenq);
            objcmd.Parameters.AddWithValue("@itdvate", oProp.itdvate);
            objcmd.Parameters.AddWithValue("@itdlabamt", oProp.itdlabamt);
            objcmd.Parameters.AddWithValue("@itdtowt", oProp.itdtowt);
            objcmd.Parameters.AddWithValue("@itdthickness", oProp.itdthickness);
            objcmd.Parameters.AddWithValue("@itdlength", oProp.itdlength);
            objcmd.Parameters.AddWithValue("@itdwidth", oProp.itdwidth);
            objcmd.Parameters.AddWithValue("@itdweight", oProp.itdweight);
            objcmd.Parameters.AddWithValue("@itddate", oProp.itddate);
            objcmd.Parameters.AddWithValue("@itdgath", oProp.itdgath);
            objcmd.Parameters.AddWithValue("@itdrema", oProp.itdrema);
            objcmd.Parameters.AddWithValue("@itdtagno", oProp.itdtagno);
            objcmd.Parameters.AddWithValue("@status", oProp.status);
            objcmd.Parameters.AddWithValue("@itdinde", oProp.itdinde);
            objcmd.Parameters.AddWithValue("@itdPODt", oProp.itdPODt);
            objcmd.ExecuteNonQuery();
            return true;
        }
        public bool UpdCodeGen(Jourmaster oProp)
        {
            GetConection();
            SqlCommand objcmd = new SqlCommand("UpdCodeGen", con);
            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            objcmd.Parameters.Add("@comp", SqlDbType.Int).Value = oProp.compcode;
            objcmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.msttype;
            objcmd.Parameters.Add("@Prev", SqlDbType.Int).Value = oProp.mstcode - 1;
            objcmd.Parameters.Add("@Curr", SqlDbType.Int).Value = oProp.mstcode;//+1
            objcmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = oProp.mstdate;
            objcmd.Parameters.Add("@StDate", SqlDbType.DateTime).Value = oProp.StDate;
            objcmd.Parameters.Add("@LsDate", SqlDbType.DateTime).Value = oProp.LastDate;
            objcmd.ExecuteNonQuery();
            return true;
        }
        public bool UpdCodeGen_API(CodeGen oProp)
        {
            GetConection();
            SqlCommand objcmd = new SqlCommand("UpdCodeGen", con);
            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            objcmd.Parameters.Add("@comp", SqlDbType.Int).Value = oProp.compcode;
            objcmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.msttype;
            objcmd.Parameters.Add("@Prev", SqlDbType.Int).Value = oProp.MstCode - 1;
            objcmd.Parameters.Add("@Curr", SqlDbType.Int).Value = oProp.MstCode;//+1
            objcmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = oProp.mstdate;
            objcmd.Parameters.Add("@StDate", SqlDbType.DateTime).Value = oProp.StDate;
            objcmd.Parameters.Add("@LsDate", SqlDbType.DateTime).Value = oProp.LastDate;
            objcmd.ExecuteNonQuery();
            return true;
        }


        public List<Jourmaster> GetLeadList(int ItemID = 0, int EmpID = 0, string To = "", string From = "", int iIndustries = 0, int MstType = 0)
        {
            GetConection();
            DateTime dFrom;
            DateTime dTo;

            var oPara = new DynamicParameters();
            oPara.Add("@ItemID", ItemID);
            oPara.Add("@EmpID", EmpID);
            oPara.Add("@Industries", iIndustries);
            oPara.Add("@MstType", MstType);

            if (From.ToString() != "")
            {
                dFrom = Convert.ToDateTime(GetDateFormat(From));
                oPara.Add("@From", dFrom);
            }

            if (To.ToString() != "")
            {
                dTo = Convert.ToDateTime(GetDateFormat(To));
                oPara.Add("@To", dTo);
            }

            var Plan_list = con.Query<Jourmaster>("GetLeadList", oPara, null, true, 0, CommandType.StoredProcedure).ToList();
            CloseConnection();
            return Plan_list;
        }
        public List<clsPoItem> GetInquiryItem(int TypeCode = 0, int compCode = 0, int Code = 0)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBEntity"].ToString()))
            {
                DynamicParameters oPara = new DynamicParameters();
                //var oPara = new DynamicParameters();
                oPara.Add("@Comp", compCode);
                oPara.Add("@Type", TypeCode);
                oPara.Add("@Code", Code);

                var ListofPlan = con.Query<clsPoItem>("GetInquiryDet", oPara, null, true, 0, commandType: CommandType.StoredProcedure).ToList();
                return ListofPlan;
            }
        }


        public List<clsList> GetInqMobile(string City, string Product, string Executive, int IUserID, int UserType)
        {
            List<clsList> lstList = new List<clsList>();
            try
            {
                GetConection();
                cmd = new SqlCommand();
                cmd.CommandText = "GetInqMobile";
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                dt = new DataTable();
                cmd.Parameters.Add("@CityNM", SqlDbType.VarChar).Value = City;
                cmd.Parameters.Add("@Product", SqlDbType.VarChar).Value = Product;
                cmd.Parameters.Add("@ExeName", SqlDbType.VarChar).Value = Executive;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = IUserID;
                cmd.Parameters.Add("@UserType", SqlDbType.Int).Value = UserType;

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    lstList.Add(new clsList(dr));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            CloseConnection();
            return lstList;
        }

        public List<clsList> GetDataList(string sQuery)
        {
            List<clsList> lstList = new List<clsList>();
            try
            {
                GetConection();
                cmd = new SqlCommand();
                cmd.CommandText = sQuery;
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    lstList.Add(new clsList(dr));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            CloseConnection();
            return lstList;
        }

        public bool InsDistributor(int MstCode, string Dis_Date, string Dis_Country, string Dis_State, string Dis_City, string Dis_Add1, string Dis_Add2, string Dis_Add3, string Dis_pobox, string Dis_City_I, int DistributorID, string DisName, string ContactPerson, string Mob1, string Mob2, string LandLine, string Email, string Skype, string GSTIN, int usetype, string usename, string usepass, string DealCode, string CityNM, int AdminID = 0)
        {
            GetConection();
            SqlCommand objcmd;
            objcmd = new SqlCommand("InsDistributor", con);
            //objcmd.Parameters.Add("@trninde", SqlDbType.Int).Value = oProp.trninde;
            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            objcmd.Parameters.Add("@MstCode", SqlDbType.Int).Value = MstCode;
            // objcmd.Parameters.Add("@MstDate", SqlDbType.DateTime).Value = Dis_Date;
            objcmd.Parameters.Add("@DistributorID", SqlDbType.Int).Value = DistributorID;
            objcmd.Parameters.Add("@DisName", SqlDbType.VarChar).Value = DisName;
            objcmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = Dis_Country;
            objcmd.Parameters.Add("@StateID", SqlDbType.Int).Value = Dis_State;
            objcmd.Parameters.Add("@CityID", SqlDbType.Int).Value = Dis_City;
            objcmd.Parameters.Add("@CityID_I", SqlDbType.Int).Value = Dis_City_I;
            objcmd.Parameters.Add("@Add_I", SqlDbType.VarChar).Value = Dis_Add1;
            objcmd.Parameters.Add("@Add_II", SqlDbType.VarChar).Value = Dis_Add2;
            objcmd.Parameters.Add("@Add_III", SqlDbType.VarChar).Value = Dis_Add3;
            objcmd.Parameters.Add("@Add_IV", SqlDbType.VarChar).Value = "";
            objcmd.Parameters.Add("@PO_Box", SqlDbType.VarChar).Value = Dis_pobox;

            objcmd.Parameters.Add("@ContactPerson", SqlDbType.VarChar).Value = ContactPerson;
            objcmd.Parameters.Add("@Mob1", SqlDbType.VarChar).Value = Mob1;
            objcmd.Parameters.Add("@Mob2", SqlDbType.VarChar).Value = Mob2;
            objcmd.Parameters.Add("@LandLine", SqlDbType.VarChar).Value = LandLine;
            objcmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;
            objcmd.Parameters.Add("@Skype", SqlDbType.VarChar).Value = Skype;
            objcmd.Parameters.Add("@GSTIN", SqlDbType.VarChar).Value = GSTIN;

            objcmd.Parameters.Add("@usetype", SqlDbType.Int).Value = usetype;
            objcmd.Parameters.Add("@usename", SqlDbType.VarChar).Value = usename;
            objcmd.Parameters.Add("@usepass", SqlDbType.VarChar).Value = usepass;
            objcmd.Parameters.Add("@DealCode", SqlDbType.VarChar).Value = DealCode;

            objcmd.Parameters.Add("@CityNM", SqlDbType.VarChar).Value = CityNM;
            objcmd.Parameters.Add("@AdminID", SqlDbType.VarChar).Value = AdminID;


            objcmd.ExecuteNonQuery();
            CloseConnection();
            return true;
        }

        //public List<clsList> GetDataList(string sQuery)
        //{
        //    GetConection();
        //    var Plan_list = con.Query<clsList>("select convert (varchar(200), inqName) NameNM, ID NameID from tblCallmst", null, null, true, 0, CommandType.Text).ToList();
        //    return Plan_list;

        //}
        public List<clsRights> GetRigths(string sQuery)
        {
            GetConection();
            var Plan_list = con.Query<clsRights>(sQuery, null, null, true, 0, CommandType.Text).ToList();
            CloseConnection();
            return Plan_list;
        }
        public List<clsHierarchyUser> GetChildTypeUser(string sQuery)
        {
            GetConection();
            var Plan_list = con.Query<clsHierarchyUser>(sQuery, null, null, true, 0, CommandType.Text).ToList();
            CloseConnection();
            return Plan_list;
        }

        public bool insSapuItd(sapuitd oProp, string sTable = "")
        {
            GetConection();

            SqlCommand objcmd;
            if (sTable != "")
            {
                objcmd = new SqlCommand("Ins" + sTable, con);
            }
            else
                objcmd = new SqlCommand("insSapuItd", con);

            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            objcmd.Parameters.AddWithValue("@compcode", oProp.compcode);
            objcmd.Parameters.AddWithValue("@itdtype", oProp.itdtype);
            objcmd.Parameters.AddWithValue("@itdcode", oProp.itdcode);
            objcmd.Parameters.AddWithValue("@itdsrno", oProp.itdsrno);
            objcmd.Parameters.AddWithValue("@itditem", oProp.itditem);
            objcmd.Parameters.AddWithValue("@itdunit", oProp.itdunit);
            objcmd.Parameters.AddWithValue("@itddime", oProp.itddime);
            objcmd.Parameters.AddWithValue("@itdmill", oProp.itdmill);
            objcmd.Parameters.AddWithValue("@itdgodo", oProp.itdgodo);
            objcmd.Parameters.AddWithValue("@itdrefs", oProp.itdrefs);
            objcmd.Parameters.AddWithValue("@itdtime", oProp.itdtime);
            objcmd.Parameters.AddWithValue("@itdempo", oProp.itdempo);
            objcmd.Parameters.AddWithValue("@itdquan", oProp.itdquan);
            objcmd.Parameters.AddWithValue("@itdrate", oProp.itdrate);
            objcmd.Parameters.AddWithValue("@itdamou", oProp.itdamou);
            objcmd.Parameters.AddWithValue("@itdpkin", oProp.itdpkin);
            objcmd.Parameters.AddWithValue("@itdrefq", oProp.itdrefq);
            objcmd.Parameters.AddWithValue("@itdpenq", oProp.itdpenq);
            objcmd.Parameters.AddWithValue("@itdvate", oProp.itdvate);
            objcmd.Parameters.AddWithValue("@itdlabamt", oProp.itdlabamt);
            objcmd.Parameters.AddWithValue("@itdtowt", oProp.itdtowt);
            objcmd.Parameters.AddWithValue("@itdthickness", oProp.itdthickness);
            objcmd.Parameters.AddWithValue("@itdlength", oProp.itdlength);
            objcmd.Parameters.AddWithValue("@itdwidth", oProp.itdwidth);
            objcmd.Parameters.AddWithValue("@itdweight", oProp.itdweight);
            objcmd.Parameters.AddWithValue("@itddate", oProp.itddate);
            objcmd.Parameters.AddWithValue("@itdgath", oProp.itdgath);
            objcmd.Parameters.AddWithValue("@itdrema", oProp.itdrema);
            objcmd.Parameters.AddWithValue("@itdtagno", oProp.itdtagno);
            objcmd.Parameters.AddWithValue("@status", oProp.status);
            objcmd.Parameters.AddWithValue("@itdinde", oProp.itdinde);
            if (sTable != "")//170830
            {
                objcmd.Parameters.AddWithValue("@itdgstrtv", oProp.itdgstrtv);
                objcmd.Parameters.AddWithValue("@itdcgstrt", oProp.itdcgstrt);
                objcmd.Parameters.AddWithValue("@itdcgstvl", oProp.itdcgstvl);
                objcmd.Parameters.AddWithValue("@itdsgstrt", oProp.itdsgstrt);
                objcmd.Parameters.AddWithValue("@itdsgstvl", oProp.itdsgstvl);
                objcmd.Parameters.AddWithValue("@itdigstrt", oProp.itdigstrt);
                objcmd.Parameters.AddWithValue("@itdigstvl", oProp.itdigstvl);
                objcmd.Parameters.AddWithValue("@itdactprc", oProp.itdactprc);
                objcmd.Parameters.AddWithValue("@itdcessrt", oProp.itdcessrt);
                objcmd.Parameters.AddWithValue("@itdcessvl", oProp.itdcessvl);
                objcmd.Parameters.AddWithValue("@itdugstrt", oProp.itdugstrt);
                objcmd.Parameters.AddWithValue("@itdugstvl", oProp.itdugstvl);
            }

            objcmd.Parameters.AddWithValue("@itdperd", oProp.itdperd);
            objcmd.Parameters.AddWithValue("@itdqtyd", oProp.itdqtyd);
            objcmd.Parameters.AddWithValue("@itddscp", oProp.itddscp);

            objcmd.ExecuteNonQuery();
            return true;
        }
        public bool insGathDet(gathdet oProp)
        {
            GetConection();
            SqlCommand objcmd = new SqlCommand("insGathDet", con);
            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            objcmd.Parameters.Add("@gathcode", SqlDbType.VarChar).Value = oProp.gathcode;
            objcmd.Parameters.Add("@gathdesc", SqlDbType.VarChar).Value = oProp.gathdesc;
            objcmd.Parameters.Add("@gathstat", SqlDbType.VarChar).Value = oProp.gathstat;
            objcmd.Parameters.Add("@gathwtdi", SqlDbType.Decimal).Value = oProp.gathwtdi;
            objcmd.Parameters.Add("@gathpuri", SqlDbType.VarChar).Value = oProp.gathpuri;

            objcmd.Parameters.Add("@gathwtdf", SqlDbType.Decimal).Value = oProp.gathwtdf;
            objcmd.Parameters.Add("@gathwtst", SqlDbType.Decimal).Value = oProp.gathwtst;
            objcmd.Parameters.Add("@gathwtnt", SqlDbType.Decimal).Value = oProp.gathwtnt;
            objcmd.Parameters.Add("@gathqtdi", SqlDbType.Decimal).Value = oProp.gathqtdi;
            objcmd.Parameters.Add("@gathwast", SqlDbType.VarChar).Value = oProp.gathwast;
            objcmd.Parameters.Add("@itdtype", SqlDbType.VarChar).Value = oProp.itdtype;
            objcmd.Parameters.Add("@gathlabp", SqlDbType.Decimal).Value = oProp.gathlabp;

            objcmd.Parameters.Add("@gathdscv", SqlDbType.Decimal).Value = oProp.gathdscv;

            objcmd.ExecuteNonQuery();
            return true;
        }
        public List<clsPoItem> GetUnitLst(int compCode = 0)
        {
            GetConection();
            DynamicParameters oPara = new DynamicParameters();
            oPara.Add("@CompCode", compCode);
            var LstUnit = con.Query<clsPoItem>("select  UnitName  ,a.UnitCode as UnitCode  from unitdet a  left Join uniconv b on a.UnitCode = b.UnitCode where a.compcode = " + compCode, null, null, true, 0, commandType: CommandType.Text).ToList();
            return LstUnit;

        }
        public List<Jourmaster> getProduction(bool IsProduction)
        {
            GetConection();
            DynamicParameters oPara = new DynamicParameters();
            oPara.Add("@IsProduction", IsProduction);
            var Plan_list = con.Query<Jourmaster>("getProduction", oPara, null, true, 0, CommandType.StoredProcedure).ToList();
            return Plan_list;
        }
        public bool InsJourmst(Jourmaster oProp, string sTable = "")
        {
            // For InsJourmst / InsOrdemst /InsSapumst
            GetConection();
            SqlCommand objcmd;

            if (sTable != "")
                objcmd = new SqlCommand("Ins" + sTable, con);
            else
                objcmd = new SqlCommand("InsJourmst", con);

            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.compcode;
            objcmd.Parameters.Add("@Msttype", SqlDbType.Int).Value = oProp.msttype;
            objcmd.Parameters.Add("@Mastcode", SqlDbType.Int).Value = oProp.mstcode;
            objcmd.Parameters.Add("@mstcust", SqlDbType.Int).Value = oProp.mstcust;
            objcmd.Parameters.Add("@mstdepa", SqlDbType.Int).Value = oProp.mstdepa;
            objcmd.Parameters.Add("@mstbrok", SqlDbType.Int).Value = oProp.mstbrok;

            objcmd.Parameters.Add("@mstrefc", SqlDbType.VarChar).Value = oProp.mstrefc;
            objcmd.Parameters.Add("@mstchno", SqlDbType.VarChar).Value = oProp.mstchno;
            objcmd.Parameters.Add("@mstrema", SqlDbType.VarChar).Value = oProp.mstrema;
            objcmd.Parameters.Add("@mstblno", SqlDbType.VarChar).Value = oProp.mstblno;
            objcmd.Parameters.Add("@mstpofs", SqlDbType.VarChar).Value = oProp.mstpofs;

            objcmd.Parameters.Add("@mstneta", SqlDbType.Money).Value = oProp.mstneta;
            objcmd.Parameters.Add("@msttota", SqlDbType.Money).Value = oProp.msttota;
            objcmd.Parameters.Add("@mstadde", SqlDbType.Money).Value = oProp.mstadde;
            objcmd.Parameters.Add("@mstdifq", SqlDbType.Money).Value = oProp.mstdifq;

            objcmd.Parameters.Add("@Mstdate", SqlDbType.DateTime).Value = oProp.mstdate;
            objcmd.Parameters.Add("@Mstbldt", SqlDbType.SmallDateTime).Value = oProp.mstbldt;

            objcmd.Parameters.Add("@msttime", SqlDbType.Int).Value = oProp.msttime;
            objcmd.Parameters.Add("@mstpaid", SqlDbType.Money).Value = oProp.mstpaid;
            objcmd.Parameters.Add("@mstbala", SqlDbType.Money).Value = oProp.mstbala;
            objcmd.Parameters.Add("@mstconnm", SqlDbType.VarChar).Value = oProp.mstconnm;
            objcmd.Parameters.Add("@mstbrnc", SqlDbType.Int).Value = oProp.mstbrnc;
            objcmd.Parameters.Add("@mstexti", SqlDbType.VarChar).Value = oProp.mstexti;
            objcmd.Parameters.Add("@mstgncd", SqlDbType.VarChar).Value = oProp.mstgncd;

            objcmd.Parameters.Add("@mstpdc", SqlDbType.Int).Value = oProp.mstpdc;
            objcmd.Parameters.Add("@mstactpay", SqlDbType.Int).Value = oProp.mstactpay;
            objcmd.Parameters.Add("@mstchqadj", SqlDbType.Int).Value = oProp.mstchqadj;
            objcmd.Parameters.Add("@mststat", SqlDbType.Int).Value = oProp.mststat;
            objcmd.Parameters.Add("@mstclno", SqlDbType.VarChar).Value = oProp.mstclno;
            objcmd.Parameters.Add("@mstchnm", SqlDbType.VarChar).Value = oProp.mstchnm;
            objcmd.Parameters.Add("@PartyID", SqlDbType.Int).Value = oProp.PartyID;
            objcmd.Parameters.Add("@DueDay", SqlDbType.Int).Value = oProp.DueDay;
            objcmd.Parameters.Add("@Interest", SqlDbType.Decimal).Value = oProp.Interest;
            objcmd.Parameters.Add("@Comm", SqlDbType.Decimal).Value = oProp.Comm;
            objcmd.Parameters.Add("@mstcurrcd ", SqlDbType.Int).Value = oProp.mstcurrcd;
            objcmd.Parameters.Add("@mstptcode", SqlDbType.Int).Value = oProp.mstptcode;

            objcmd.Parameters.Add("@mstyldp", SqlDbType.Decimal).Value = oProp.mstyldp;
            objcmd.Parameters.Add("@mstchnV", SqlDbType.Int).Value = oProp.mstchnV;

            objcmd.Parameters.Add("@MstChnH", SqlDbType.VarChar).Value = oProp.mstchnH;
            objcmd.Parameters.AddWithValue("@oldwht", oProp.oldwht);  //170831
            objcmd.Parameters.AddWithValue("@mstAppr", oProp.mstAppr);//170831
            objcmd.Parameters.AddWithValue("@mstqtyd", oProp.mstqtyd);//170831
            objcmd.Parameters.AddWithValue("@mstvat1", oProp.mstvat1);//170831
            objcmd.Parameters.AddWithValue("@mstvat2", oProp.mstvat2);//170831
            objcmd.Parameters.AddWithValue("@mstvat3", oProp.mstvat3);//170831
            objcmd.Parameters.AddWithValue("@mstsite", oProp.mstsite);//170831
            objcmd.Parameters.AddWithValue("@mstsubt", oProp.mstsubt);//170831
            objcmd.Parameters.AddWithValue("@mstprtc", oProp.mstprtc);//170831
            objcmd.Parameters.AddWithValue("@msternv", oProp.msternv);//170831
            objcmd.Parameters.AddWithValue("@mstrvsc", oProp.mstrvsc);//170831
            objcmd.Parameters.AddWithValue("@mstintr", oProp.mstintr);//170831
            objcmd.Parameters.AddWithValue("@mstperd", oProp.mstperd);//170831
            objcmd.Parameters.AddWithValue("@mstIorL", oProp.mstIorL);//170831
            objcmd.Parameters.AddWithValue("@mstcurrrat", oProp.mstcurrrat);//170831
            objcmd.Parameters.AddWithValue("@mstDueDate", oProp.mstDueDate);//170831
            objcmd.Parameters.AddWithValue("@mstbuyerc", oProp.mstbuyerc);   //170831
            objcmd.Parameters.AddWithValue("@mstdsptoc", oProp.mstdsptoc);   //170831
            objcmd.Parameters.AddWithValue("@mstsale", oProp.mstsale);   //170831
            //objcmd.Parameters.AddWithValue("@mstappl", oProp.mstappl);   //170831

            objcmd.ExecuteNonQuery();
            return true;

        }
        public bool InsJourTrn(jourtrn oProp, bool isOrder = false)
        {
            GetConection();
            SqlCommand objcmd;

            if (isOrder == true)
            {
                objcmd = new SqlCommand("InsOrdetrn", con);
            }
            else objcmd = new SqlCommand("InsJourTrn", con);

            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            objcmd.Parameters.Add("@trninde", SqlDbType.Int).Value = oProp.trninde;//170829 required in sale bill tax time
            objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.compcode;
            objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = oProp.trntype;
            objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = oProp.trncode;
            objcmd.Parameters.Add("@trnitem", SqlDbType.Int).Value = oProp.trnitem;
            objcmd.Parameters.Add("@trnrefc", SqlDbType.Int).Value = oProp.trnrefc;
            objcmd.Parameters.Add("@trnledg", SqlDbType.Int).Value = oProp.trnledg;
            objcmd.Parameters.Add("@trntime", SqlDbType.Int).Value = oProp.trntime;
            objcmd.Parameters.Add("@trnexpr", SqlDbType.Int).Value = oProp.trnexpr;
            objcmd.Parameters.Add("@trncity", SqlDbType.Int).Value = oProp.trncity;
            objcmd.Parameters.Add("@trntagv", SqlDbType.Int).Value = oProp.trntagv;
            objcmd.Parameters.Add("@trnshow", SqlDbType.Int).Value = oProp.trnshow;
            objcmd.Parameters.Add("@trnsrno", SqlDbType.Int).Value = oProp.trnsrno;
            objcmd.Parameters.Add("@trncmdt", SqlDbType.Int).Value = oProp.trncmdt;
            objcmd.Parameters.Add("@trnrema", SqlDbType.VarChar).Value = oProp.trnrema;
            objcmd.Parameters.Add("@trnbank", SqlDbType.VarChar).Value = oProp.trnbank;
            objcmd.Parameters.Add("@trnchno", SqlDbType.VarChar).Value = oProp.trnchno;
            objcmd.Parameters.Add("@trndate", SqlDbType.DateTime).Value = oProp.trndate;
            objcmd.Parameters.Add("@trnrefd", SqlDbType.DateTime).Value = oProp.trnrefd;
            objcmd.Parameters.Add("@trnchdt", SqlDbType.DateTime).Value = oProp.trnchdt;
            objcmd.Parameters.Add("@trndram", SqlDbType.Money).Value = oProp.trndram;
            objcmd.Parameters.Add("@trncram", SqlDbType.Money).Value = oProp.trncram;
            objcmd.Parameters.Add("@trnadju", SqlDbType.Money).Value = oProp.trnadju;
            objcmd.Parameters.Add("@trnexpa", SqlDbType.Money).Value = oProp.trnexpa;
            objcmd.Parameters.Add("@trnaddv", SqlDbType.Money).Value = oProp.trnaddv;
            objcmd.ExecuteNonQuery();
            return true;
        }
        public bool InsRefetab(refetab oProp, bool isOrder = false)
        {
            GetConection();
            SqlCommand cmd;

            cmd = new SqlCommand("InsRefetab", con);

            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            cmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.compcode;
            cmd.Parameters.Add("@ms1code ", SqlDbType.Int).Value = oProp.ms1code;
            cmd.Parameters.Add("@ms1type ", SqlDbType.Int).Value = oProp.ms1type;
            cmd.Parameters.Add("@ms1srno ", SqlDbType.Int).Value = oProp.ms1srno;
            cmd.Parameters.Add("@ms2code ", SqlDbType.Int).Value = oProp.ms2code;
            cmd.Parameters.Add("@ms2type ", SqlDbType.Int).Value = oProp.ms2type;
            cmd.Parameters.Add("@ms2srno ", SqlDbType.Int).Value = oProp.ms2srno;
            cmd.Parameters.Add("@reftype ", SqlDbType.Int).Value = oProp.reftype;
            cmd.Parameters.Add("@refitem ", SqlDbType.Int).Value = oProp.refitem;
            cmd.Parameters.Add("@refmill ", SqlDbType.Int).Value = oProp.refmill;

            cmd.Parameters.Add("@refrema", SqlDbType.VarChar).Value = oProp.refrema;
            cmd.Parameters.Add("@ms2date", SqlDbType.DateTime).Value = oProp.ms2date;
            cmd.Parameters.Add("@ms1date", SqlDbType.DateTime).Value = oProp.ms1date;
            cmd.Parameters.Add("@refamou", SqlDbType.Money).Value = oProp.refamou;
            cmd.ExecuteNonQuery();
            return true;
        }
        public List<JsonItem> GetItemRate(int SubCate, int compCode)
        {
            GetConection();
            var paramater = new DynamicParameters();
            paramater.Add("@CompCode", compCode);
            paramater.Add("@SubCategory", SubCate);

            var Plan_list = con.Query<JsonItem>("GetItemRate", paramater, null, true, 0, CommandType.StoredProcedure).ToList();
            return Plan_list;

        }

        public List<Jourmaster> GetPendingOrder(int BrandID, int DealerID, int StateID, int SubCateID, string From = "", string To = "", int Executive = 0, int CompanyID = 0)
        {
            GetConection();

            var oPara = new DynamicParameters();
            oPara.Add("@BrandID", BrandID);
            oPara.Add("@DealerID ", DealerID);
            oPara.Add("@StateID", StateID);
            oPara.Add("@SubCateID", SubCateID);
            oPara.Add("@Executive", Executive);
            oPara.Add("@CompanyID", CompanyID);
            if (From != "")
            {
                oPara.Add("@From", GetDateFormat(From));
                oPara.Add("@To", GetDateFormat(To));
            }

            var Plan_list = con.Query<Jourmaster>("GetPendingOrder", oPara, null, true, 0, CommandType.StoredProcedure).ToList();

            CloseConnection();


            return Plan_list;
        }

        public List<Jourmaster> GetApprovedOrder(int BrandID, int DealerID, int StateID, int SubCateID, string From = "", string To = "", int Executive = 0, int CompanyID = 0)
        {
            GetConection();

            var oPara = new DynamicParameters();
            oPara.Add("@BrandID", BrandID);
            oPara.Add("@DealerID ", DealerID);
            oPara.Add("@StateID", StateID);
            oPara.Add("@SubCateID", SubCateID);
            oPara.Add("@Executive", Executive);
            oPara.Add("@CompanyID", CompanyID);
            if (From != "")
            {
                oPara.Add("@From", GetDateFormat(From));
                oPara.Add("@To", GetDateFormat(To));
            }

            var Plan_list = con.Query<Jourmaster>("GetApprovedOrder", oPara, null, true, 0, CommandType.StoredProcedure).ToList();
            CloseConnection();
            return Plan_list;
        }

        public List<DueList> sp_GetDueList(int CompCode, string AcctList = "", string UptoDate = "", int State = 0, int City = 0, bool isSummary = true, int Due = 0, int Commit = 0, int Planning = 0)
        {
            GetConection();

            var oPara = new DynamicParameters();
            if (CompCode > 0)
                oPara.Add("@pCompCd ", CompCode);
            else
                oPara.Add("@pCompCd ", null);

            //oPara.Add("@pBranchWs", StateID);
            //oPara.Add("@pBrncCd", SubCateID);
            oPara.Add("@State", State);
            oPara.Add("@City", City);
            oPara.Add("@DueOnly", Due);
            oPara.Add("@CommitOnly", Commit);
            oPara.Add("@isPlanning", Planning);

            oPara.Add("@pAcctList", AcctList);
            if (UptoDate != "")
            {
                oPara.Add("@pUptoDate", GetDateFormat(UptoDate));
            }

            var Plan_list = con.QueryMultiple("sp_GetDueListFIFO_PaymentFolloup", oPara, null, 3000, CommandType.StoredProcedure);

            var customer = Plan_list.Read<DueList>().ToList();
            var orders = Plan_list.Read<DueList>().ToList();

            CloseConnection();

            if (isSummary == true)
                return customer;
            else
                return orders;
        }

        public List<DueList> GetDueListEmp(int CompCode, string AcctList = "", int State = 0, int City = 0, bool isSummary = true, int Due = 0, int Commit = 0, int Planning = 0, int empID = 0, int All = 0)
        {
            GetConection();

            var oPara = new DynamicParameters();
          
                oPara.Add("@pCompCd ", null);
             
            oPara.Add("@State", State);
            oPara.Add("@City", City);
            //oPara.Add("@DueOnly", Due);
            oPara.Add("@CommitOnly", Commit);
            oPara.Add("@isPlanning", Planning);

            oPara.Add("@pAcctList", AcctList);
            oPara.Add("@empID", empID);
            oPara.Add("@All", All);

                oPara.Add("@pUptoDate", DateTime.Now.Date);
             
            var Plan_list = con.Query<DueList>("sp_GetDueListFIFO_PaymentFolloup_Emp", oPara, null, true, 3000, CommandType.StoredProcedure).ToList();
            CloseConnection();
            return Plan_list;
             
        }

        public List<clsPayFollowUp> getPayFollowUp(int BrandID, int DealerID, int StateID, int SubCateID, string From = "", string To = "", int Executive = 0, int CompanyID = 0)
        {
            GetConection();

            var oPara = new DynamicParameters();
            //oPara.Add("@BrandID", BrandID);
            //oPara.Add("@DealerID ", DealerID);
            //oPara.Add("@StateID", StateID);
            //oPara.Add("@SubCateID", SubCateID);
            //oPara.Add("@Executive", Executive);
            //oPara.Add("@CompanyID", CompanyID);
            //if (From != "")
            //{
            //    oPara.Add("@From", GetDateFormat(From));
            //    oPara.Add("@To", GetDateFormat(To));
            //}

            var Plan_list = con.Query<clsPayFollowUp>("getPayFollowUp", null, null, true, 0, CommandType.StoredProcedure).ToList();
            CloseConnection();
            return Plan_list;
        }

        public Jourmaster GetOrderApp(string MstChno, int ItemId, int Compcode, int srno)
        {
            GetConection();

            var Plan_list = con.Query<Jourmaster>("select gath.gathdscv DisAmt,a.*,b.* from ordeMst a Inner Join ordeItd b on a.compcode = b.Compcode and  a.mstCode = b.itdCode  and a.MstType = b.itdType left join Gathdet gath on b.itdGath = gath.Gathcode  where a.msttype = 174 and a.Compcode = " + Compcode + " and a.MstChNo ='" + MstChno + "'  and b.ItdItem =  " + ItemId + " and itdsrno = " + srno, null, null, true, 0, CommandType.Text).Single();
            // CloseConnection();
            return Plan_list;
        }


        public List<SaleBook> GetSaleBook(int Compcode, DateTime From, DateTime To, int PType)
        {
            List<SaleBook> lstSaleBook = new List<SaleBook>();
            try
            {
                GetConection();
                cmd = new SqlCommand();
                cmd.CommandText = "GetSaleBook";
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                dt = new DataTable();
                cmd.Parameters.AddWithValue("@Compcode", Compcode);
                cmd.Parameters.AddWithValue("@To", To);
                cmd.Parameters.AddWithValue("@From", From);
                cmd.Parameters.AddWithValue("@TypeID", PType);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                lstSaleBook = Utility.BindList<SaleBook>(dt);
                CloseConnection();

                return lstSaleBook;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstSaleBook;
        }
        public DataTable GetVoucher(int TypeCode = 0, int compCode = 0, int Code = 0)
        {
            GetConection();
            cmd = new SqlCommand();
            cmd.CommandText = "GetVoucher";
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            cmd.Parameters.AddWithValue("@Comp", compCode);
            cmd.Parameters.AddWithValue("@Type", TypeCode);
            if (Code > 0)
                cmd.Parameters.AddWithValue("@Code", Code);

            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();// Add By Ajay On 29082017 Pool Size  
            return dt;
        }

        public List<Jourmaster> GetDispatchOrd(string BrandID, int DealerID, int StateID, int SubCateID, string From = "", string To = "", int Executive = 0, int CompanyID = 0)
        {
            GetConection();

            var oPara = new DynamicParameters();
            oPara.Add("@BrandID", BrandID);
            oPara.Add("@DealerID ", DealerID);
            oPara.Add("@StateID", StateID);
            oPara.Add("@SubCateID", SubCateID);
            oPara.Add("@Executive", Executive);
            oPara.Add("@CompanyID", CompanyID);
            if (From != "")
            {
                oPara.Add("@From", GetDateFormat(From));
                oPara.Add("@To", GetDateFormat(To));
            }

            var Plan_list = con.Query<Jourmaster>("GetDispatch", oPara, null, true, 0, CommandType.StoredProcedure).ToList();
            con.Close();
            return Plan_list;
        }

        public List<Jourmaster> GetPendingDispatch(string BrandID, int DealerID, int StateID, int SubCateID, string From = "", string To = "", int Executive = 0, int CompanyID = 0)
        {
            GetConection();

            var oPara = new DynamicParameters();
            oPara.Add("@BrandID", BrandID);
            oPara.Add("@DealerID ", DealerID);
            oPara.Add("@StateID", StateID);
            oPara.Add("@SubCateID", SubCateID);
            oPara.Add("@Executive", Executive);
            oPara.Add("@CompanyID", CompanyID);
            if (From != "")
            {
                oPara.Add("@From", GetDateFormat(From));
                oPara.Add("@To", GetDateFormat(To));
            }

            var Plan_list = con.Query<Jourmaster>("GetPendingDispatch", oPara, null, true, 0, CommandType.StoredProcedure).ToList();
            con.Close();
            return Plan_list;
        }

        public bool InsRights(clsRights oProp)
        {
            GetConection();
            SqlCommand objcmd = new SqlCommand("InsRights", con);
            objcmd.CommandType = CommandType.StoredProcedure;
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            objcmd.Parameters.Add("@mencode", SqlDbType.Int).Value = oProp.mencode;
            objcmd.Parameters.Add("@menuser", SqlDbType.Int).Value = oProp.menuser;

            objcmd.Parameters.Add("@menview", SqlDbType.Bit).Value = oProp.menview;
            objcmd.Parameters.Add("@menaddi", SqlDbType.Bit).Value = oProp.menaddi;
            objcmd.Parameters.Add("@menedit", SqlDbType.Bit).Value = oProp.menedit;
            objcmd.Parameters.Add("@mendele", SqlDbType.Bit).Value = oProp.mendele;
            objcmd.Parameters.Add("@menacce", SqlDbType.Bit).Value = oProp.menacce;

            objcmd.ExecuteNonQuery();
            CloseConnection();
            return true;
        }


        public List<clsOrderFollowUp> GetOrdFollowUp(int DealerID, int StateID, int Executive, int CityID, string From = "", string To = "")
        {
            GetConection();

            var oPara = new DynamicParameters();
            oPara.Add("@DealerID", DealerID);
            oPara.Add("@StateID", StateID);
            oPara.Add("@Executive", Executive);
            oPara.Add("@CityID", CityID);
            if (From != "")
            {
                oPara.Add("@From", GetDateFormat(From));
                oPara.Add("@To", GetDateFormat(To));
            }
            var Plan_list = con.Query<clsOrderFollowUp>("GetOrdFollowUp", oPara, null, true, 0, CommandType.StoredProcedure).ToList();
            con.Close();
            return Plan_list;
        }

        public List<clsOrderFollowUp> GetOrdFollowUpList(int DealerID, int StateID, int Executive, int CityID, string From = "", string To = "")
        {
            GetConection();

            var oPara = new DynamicParameters();
            oPara.Add("@DealerID", DealerID);
            oPara.Add("@StateID", StateID);
            oPara.Add("@Executive", Executive);
            oPara.Add("@CityID", CityID);
            if (From != "")
            {
                oPara.Add("@From", GetDateFormat(From));
                oPara.Add("@To", GetDateFormat(To));
            }
            var Plan_list = con.Query<clsOrderFollowUp>("GetOrdFollowUpList", oPara, null, true, 0, CommandType.StoredProcedure).ToList();
            con.Close();
            return Plan_list;
        }

        public List<clsFilter> GetBrandList()
        {
            GetConection();

            var Plan_list = con.Query<clsFilter>("Select Studcode ItemID,StudName ItemNm from studdet a where a.studType = 61 order by StudName", null, null, true, 0, CommandType.Text).ToList();
            con.Close();
            return Plan_list;
        }

        public List<clsUserWork> UserWork(int iUserCode = 0, string To = "", string From = "", int ModeID = 5, int Compcode = 0)
        {
            GetConection();

            DateTime dFrom;
            DateTime dTo;

            var oPara = new DynamicParameters();
            oPara.Add("@UserID", iUserCode);
            oPara.Add("@ModeID", ModeID);
            oPara.Add("@CompCode", Compcode);

            if (From.ToString() != "")
            {
                dFrom = Convert.ToDateTime(GetDateFormat(From));
                oPara.Add("@From", dFrom);
            }
            if (To.ToString() != "")
            {
                dTo = Convert.ToDateTime(GetDateFormat(To));
                oPara.Add("@To", dTo);
            }
            var Plan_list = con.Query<clsUserWork>("getUserWork", oPara, null, true, 0, CommandType.StoredProcedure).ToList();
            return Plan_list;
        }
        public List<clsDayWork> GetDayWork(int iUserCode = 0, string To = "", string From = "", int CompCode = 0)
        {
            GetConection();

            DateTime dFrom;
            DateTime dTo;

            var oPara = new DynamicParameters();
            oPara.Add("@UserID", iUserCode);
            oPara.Add("@CompCode", CompCode);

            if (From.ToString() != "")
            {
                dFrom = Convert.ToDateTime(GetDateFormat(From));
                oPara.Add("@From", dFrom);
            }
            if (To.ToString() != "")
            {
                dTo = Convert.ToDateTime(GetDateFormat(To));
                oPara.Add("@To", dTo);
            }
            var Plan_list = con.Query<clsDayWork>("GetDayWork", oPara, null, true, 0, CommandType.StoredProcedure).ToList();
            return Plan_list;
        }

        public DataSet GetGraph(int Compcode, int Itype)
        {
            List<clsPoItem> lstAccount = new List<clsPoItem>();
            try
            {
                GetConection();
                cmd = new SqlCommand();
                ds = new DataSet();
                cmd.CommandText = "GetChartData";
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                dt = new DataTable();
                cmd.Parameters.AddWithValue("@Compcode", Compcode);
                cmd.Parameters.AddWithValue("@msttype", Itype);
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        public List<Dictionary<string, object>> DataTableToJSON1(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return parentRow;
        }
        /*Start 181012 */
        public List<clsVisitSchMst> InsVisitSchMst(clsVisitSchMst oCls)
        {
            GetConection();

            var oPara = new DynamicParameters();
            oPara.Add("@vsCompCode", oCls.vsCompCode);
            if (oCls.vsDate != "") oPara.Add("@vsDate ", oCls.vsDate);
            if (oCls.vsEmpID != "") oPara.Add("@vsEmpID", oCls.vsEmpID);
            if (oCls.vsSchDate != "") oPara.Add("@vsSchDate", oCls.vsSchDate);

            var Plan_list = con.Query<clsVisitSchMst>("sp_InsVisitSchMst", oPara, null, true, 0, CommandType.StoredProcedure).ToList();
            con.Close();
            return Plan_list;
        }
        public List<clsVisitSchTrn> InsVisitSchTrn(clsVisitSchTrn oCls)
        {
            GetConection();

            var oPara = new DynamicParameters();

            if (oCls.vtCompCode != 0) oPara.Add("@vtCompCode", oCls.vtCompCode);
            if (oCls.vsID != 0) oPara.Add("@vsID", oCls.vsID);
            if (oCls.vtTypeName != "") oPara.Add("@vtTypeName", oCls.vtTypeName);
            if (oCls.vtPartyName != "") oPara.Add("@vtPartyName", oCls.vtPartyName);
            if (oCls.vtArea != "") oPara.Add("@vtArea", oCls.vtArea);
            if (oCls.vtMobile != "") oPara.Add("@vtMobile", oCls.vtMobile);
            if (oCls.vtAddress != "") oPara.Add("@vtAddress", oCls.vtAddress);
            if (oCls.vtModelNo != "") oPara.Add("@vtModelNo", oCls.vtModelNo);
            if (oCls.vtMachineNo != "") oPara.Add("@vtMachineNo", oCls.vtMachineNo);
            if (oCls.vtWeightName != "") oPara.Add("@vtWeightName", oCls.vtWeightName);
            if (oCls.vtValidFor != 0) oPara.Add("@vtValidFor", oCls.vtValidFor);
            if (oCls.vtMachineType != "") oPara.Add("@vtMachineType", oCls.vtMachineType);
            if (oCls.vtDueYear != 0) oPara.Add("@vtDueYear", oCls.vtDueYear);
            if (oCls.vtDueMonths != 0) oPara.Add("@vtDueMonths", oCls.vtDueMonths);
            //if (oCls.vtValidUpToDate != null) oPara.Add("@vtValidUpToDate", oCls.vtValidUpToDate);
            if (oCls.vtVcType != "") oPara.Add("@vtVcType", oCls.vtVcType);

            var Plan_list = con.Query<clsVisitSchTrn>("sp_InsVisitSchTrn", oPara, null, true, 0, CommandType.StoredProcedure).ToList();
            con.Close();
            return Plan_list;
        }
        public List<clsVisitSchTrn> UpdVisitSchTrn(clsVisitSchTrn oCls)
        {
            GetConection();

            var oPara = new DynamicParameters();

            if (oCls.vtCompCode != 0) oPara.Add("@vtCompCode", oCls.vtCompCode);
            if (oCls.vsID != 0) oPara.Add("@vsID", oCls.vsID);
            if (oCls.vtPartyName != "") oPara.Add("@vtPartyName", oCls.vtPartyName.Trim());
            if (oCls.vtArea != "") oPara.Add("@vtArea", oCls.vtArea.Trim());
            if (oCls.vtMobile != "") oPara.Add("@vtMobile", oCls.vtMobile.Trim());

            var Plan_list = con.Query<clsVisitSchTrn>("sp_UpdVisitSchTrn", oPara, null, true, 0, CommandType.StoredProcedure).ToList();
            con.Close();
            return Plan_list;
        }
        /*End 181012 */

        public List<clsVisitSchTrn> GetVisitPartyData(string sQuery)
        {
            GetConection();
            var Plan_list = con.Query<clsVisitSchTrn>(sQuery, null, null, true, 0, CommandType.Text).ToList();
            CloseConnection();
            return Plan_list;
        }
		/*start 181016 %temp%*/
        public List<clsDailyVisitEntry> InsDailyVisitEntry(clsDailyVisitEntry oCls)
        {
            GetConection();

            var oPara = new DynamicParameters();
            oPara.Add("@dvCompCode", oCls.dvCompCode);
            if (oCls.dvDate != null) oPara.Add("@dvDate", oCls.dvDate);
            if (oCls.dvPartyName != "") oPara.Add("@dvPartyName", oCls.dvPartyName);
            if (oCls.dvMobile != "") oPara.Add("@dvMobile", oCls.dvMobile);
            if (oCls.dvVisitDetail != "") oPara.Add("@dvVisitDetail", oCls.dvVisitDetail);
            if (oCls.dvEstCost != 0) oPara.Add("@dvEstCost", oCls.dvEstCost);
            if (oCls.dvNextFollowUp != null) oPara.Add("@dvNextFollowUp", oCls.dvNextFollowUp);
            if (oCls.dvRemark != "") oPara.Add("@dvRemark", oCls.dvRemark);

            var Plan_list = con.Query<clsDailyVisitEntry>("sp_InsDailyVisitEntry", oPara, null, true, 0, CommandType.StoredProcedure).ToList();
            con.Close();
            return Plan_list;
        }
        /*end 181016*/
        /*start 181017 %temp%*/
        public List<clsDailyVisitEntry> InsDailyVisitReport(clsDailyVisitEntry oCls)
        {
            GetConection();

            var oPara = new DynamicParameters();
            oPara.Add("@dvCompCode", oCls.dvCompCode);
            if (oCls.dvDate != null) oPara.Add("@dvDate", oCls.dvDate);
            if (oCls.dvPartyName != "") oPara.Add("@dvPartyName", oCls.dvPartyName);
            if (oCls.dvMobile != "") oPara.Add("@dvMobile", oCls.dvMobile);
            if (oCls.dvVisitDetail != "") oPara.Add("@dvVisitDetail", oCls.dvVisitDetail);
            if (oCls.dvQty != 0) oPara.Add("@dvQty", oCls.dvQty);
            if (oCls.dvEstCost != 0) oPara.Add("@dvEstCost", oCls.dvEstCost);
            if (oCls.dvNextFollowUp != null) oPara.Add("@dvNextFollowUp", oCls.dvNextFollowUp);
            if (oCls.dvRemark != "") oPara.Add("@dvRemark", oCls.dvRemark);

            var Plan_list = con.Query<clsDailyVisitEntry>("sp_InsDailyVisitReport", oPara, null, true, 0, CommandType.StoredProcedure).ToList();
            con.Close();
            return Plan_list;
        }
        /*End 181017 */
    }

}

