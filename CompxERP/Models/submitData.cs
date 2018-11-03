using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;


public class submitData
{
    public SqlConnection con = null;
    public SqlCommand cmd = null;
    public SqlDataAdapter da = null;
    public SqlDataReader dr = null;
    public DataSet ds = null;
    public DataTable dt = null;
    public string retValue;
    public DataRow drow;
    //dbInterface c = new dbInterface();
    public String sql;
    public int iCompanyCode;
    commFunction oCommon = new commFunction();

    int iout;
    public void insertData(String s)
    {
        long rowA;
        SqlCommand cmdL = null;
        if (dr != null)
            dr.Close();
        GetConection();
        cmdL = new SqlCommand(s, con);
        rowA = cmdL.ExecuteNonQuery();
        cmdL.Dispose();
    }
    public DataTable GetData(String sQuery)
    {
        dt = new DataTable();
        SqlCommand cmdL = null;
        if (dr != null) dr.Close();
        GetConection();
        cmdL = new SqlCommand(sQuery, con);
        //cmdL.CommandTimeout = 5000;
        da = new SqlDataAdapter(cmdL);
        da.Fill(dt);
        cmdL.Dispose();
        return dt;
    }

    public object GetSingleData(String sQuery)
    {
        SqlCommand cmdL = null;
        GetConection();
        cmdL = new SqlCommand(sQuery, con);
        cmdL.CommandTimeout = 3000;
        object s = cmdL.ExecuteScalar();
        if (s != null && s.ToString() != "")
            return s;
        else
            return "0";
    }
    public void Ins_UserRigths(bool menview, bool menaddi, bool menedit, bool mendele, bool menacce, int mencode, string menuser)
    {
        GetConection();
        SqlCommand objcmd = new SqlCommand("SP_InsUpDelUserRights", con);
        objcmd.CommandType = CommandType.StoredProcedure;

        objcmd.Parameters.Add("@menview", SqlDbType.Bit).Value = menview;
        objcmd.Parameters.Add("@menaddi", SqlDbType.Bit).Value = menaddi;
        objcmd.Parameters.Add("@menedit", SqlDbType.Bit).Value = menedit;
        objcmd.Parameters.Add("@mendele", SqlDbType.Bit).Value = mendele;
        //objcmd.Parameters.Add("@menimpo", SqlDbType.Bit).Value = menimpo;
        objcmd.Parameters.Add("@menacce", SqlDbType.Bit).Value = menacce;
        objcmd.Parameters.Add("@mencode", SqlDbType.Int).Value = mencode;
        objcmd.Parameters.Add("@menuser", SqlDbType.VarChar).Value = menuser;

        objcmd.ExecuteNonQuery();
    }
    public DataSet GetData(String sQuery, int i)
    {
        ds = new DataSet();
        SqlCommand cmdL = null;
        if (dr != null)
            dr.Close();
        GetConection();
        cmdL = new SqlCommand(sQuery, con);
        da = new SqlDataAdapter(cmdL);
        da.Fill(ds);
        cmdL.Dispose();
        return ds;
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
    public void RollBack(Page mPage)
    {
        try
        {
            sql = "rollback";
            InitilizeCommand(1);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        catch (Exception ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(mPage, GetType(), "Error", "alert('" + ex.ToString() + "');", true);
        }
    }



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

    public void InsUpd_Dealer(string name)
    {
        GetConection();
        SqlCommand objcmd = new SqlCommand("SP_InsUpd_Dealer", con);
        objcmd.CommandType = CommandType.StoredProcedure;
        objcmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = name;

        objcmd.ExecuteNonQuery();
    }

    //======================================Area Master=======================================12 Aug 2013 Sourabh

    public void FillAreaDropDwn(DropDownList ddlArea, int type)
    {
        ddlArea.Items.Clear();
        GetConection();
        SqlDataAdapter daFill = new SqlDataAdapter("select distinct cityCode,cityName from citydet where citytype=600 order by cityName", con);
        dt = new DataTable();
        daFill.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            ddlArea.DataSource = dt;
            if (type == 0)
            {
                ddlArea.DataTextField = "AreaCode";
                ddlArea.DataValueField = "AreaName";
            }
            else
            {
                ddlArea.DataTextField = "AreaName";
                ddlArea.DataValueField = "AreaCode";
            }
            ddlArea.DataBind();

        }
        ddlArea.Items.Insert(0, new ListItem("--Select--", "0"));
        dt.Dispose();
    }

    //================================BookMaster===========================================================

    public void FillBookDropDown(DropDownList ddlBook, string type)
    {
        ddlBook.Items.Clear();
        GetConection();
        SqlDataAdapter daFill = new SqlDataAdapter("select distinct book_id,book_name from Book_Master order by book_name", con);
        dt = new DataTable();
        daFill.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            ddlBook.DataSource = dt;

            if (type == "0")
            {
                ddlBook.DataTextField = "book_id";
                ddlBook.DataValueField = "book_name";
            }
            if (type == "1")
            {
                ddlBook.DataTextField = "book_name";
                ddlBook.DataValueField = "book_id";
            }

            ddlBook.DataBind();
        }
        ddlBook.Items.Insert(0, new ListItem("--Select--", "0"));
        dt.Dispose();


    }

    public void FillCategory(DropDownList ddl, string type)
    {
        ddl.Items.Clear();
        GetConection();
        SqlDataAdapter daFill = new SqlDataAdapter("SELECT (case when isnull(scAlia,'')='' then name else scAlia end)+'-'+hindinm as name,code FROM subcategorymaster WHERE category in (" + type + ") order by cast(isnull(sc_sqnc,'100') as int),name", con);
        dt = new DataTable();
        daFill.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            ddl.DataSource = dt;

            //if (type == "0")
            //{
            //    ddl.DataTextField = "code";
            //    ddl.DataValueField = "name";
            //}
            //if (type == "1")
            //{
            ddl.DataTextField = "name";
            ddl.DataValueField = "code";
            //}

            ddl.DataBind();
        }
        ddl.Items.Insert(0, new ListItem("--Select--", "0"));
        dt.Dispose();

    }

    public void FillDD(DropDownList ddl, string type)
    {
        ddl.Items.Clear();
        GetConection();
        SqlDataAdapter daFill = new SqlDataAdapter("SELECT (case when isnull(scAlia,'')='' then name else scAlia end)+'-'+hindinm as name,code FROM subcategorymaster WHERE category in (" + type + ") order by cast(isnull(sc_sqnc,'100') as int),name", con);
        dt = new DataTable();
        daFill.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            ddl.DataSource = dt;

            ddl.DataTextField = "name";
            ddl.DataValueField = "code";

            ddl.DataBind();
        }
        ddl.Items.Insert(0, new ListItem("--Select--", "0"));
        dt.Dispose();

    }

    public void FillDDL(DropDownList ddl, string sName, string sValue, DataTable dt, bool isSelect = true)
    {
        ddl.Items.Clear();
        if (dt.Rows.Count > 0)
        { ddl.DataSource = dt; ddl.DataTextField = sName; ddl.DataValueField = sValue; ddl.DataBind(); }

        if (isSelect == true)
            ddl.Items.Insert(0, new ListItem("--Select--", "0"));

        dt.Dispose();
    }

    //public bool Upd_ARV(PropProperty oProp)
    //{
    //    SqlParameter[] oPara = new SqlParameter[20];

    //    oPara[0] = new SqlParameter("@compcode", oProp.iCompCode);
    //    oPara[1] = new SqlParameter("@zoneno", oProp.iARV_ZoneID);
    //    oPara[2] = new SqlParameter("@ares1", oProp.iARV_RCC_Res);
    //    oPara[3] = new SqlParameter("@acom1", oProp.iARV_RCC_Com);
    //    oPara[4] = new SqlParameter("@ares2", oProp.iARV_Cement_Res);
    //    oPara[5] = new SqlParameter("@acom2", oProp.iARV_Cement_Com);
    //    oPara[6] = new SqlParameter("@ares3", oProp.iARV_Mud_Res);
    //    oPara[7] = new SqlParameter("@acom3", oProp.iARV_Mud_Com);
    //    oPara[8] = new SqlParameter("@ares4", oProp.iARV_Undev_Res);
    //    oPara[9] = new SqlParameter("@acom4", oProp.iARV_Undev_Com);
    //    oPara[10] = new SqlParameter("@arate1", oProp.iARV_RCC_Rate);
    //    oPara[11] = new SqlParameter("@arate2", oProp.iARV_Cement_Rate);
    //    oPara[12] = new SqlParameter("@arate3", oProp.iARV_Mud_Rate);
    //    oPara[13] = new SqlParameter("@arate4", oProp.iARV_Undev_Rate);
    //    oPara[14] = new SqlParameter("@florecode", oProp.iARV_Floor);
    //    oPara[15] = new SqlParameter("@annucode", oProp.iARV_AnnuCode);
    //    oPara[16] = new SqlParameter("@ares5", oProp.iARV_Dev_Res);
    //    oPara[17] = new SqlParameter("@acom5", oProp.iARV_Dev_Com);
    //    oPara[18] = new SqlParameter("@location", oProp.iARV_location);
    //    oPara[19] = new SqlParameter("@locationnm", oProp.iARV_locationName);

    //    connection oConnection = new connection();

    //    oConnection.SP_Execute("Upd_ARV", oPara);

    //    return true;
    //}

    //public bool Inst_WaterFee(propWater oProp)
    //{
    //    SqlParameter[] oPara = new SqlParameter[15];

    //    oPara[0] = new SqlParameter("@wcode", oProp.iWCode);
    //    oPara[1] = new SqlParameter("@compcode", oProp.iCompCode);
    //    oPara[2] = new SqlParameter("@fee", oProp.iFee);
    //    //oPara[3] = new SqlParameter("@srno", oProp.iSrNo);
    //    oPara[3] = new SqlParameter("@zoneno", oProp.iZoneno);
    //    oPara[4] = new SqlParameter("@used", oProp.iUsed);
    //    oPara[5] = new SqlParameter("@usecode", oProp.iUsecode);
    //    oPara[6] = new SqlParameter("@width", oProp.iWidth);
    //    oPara[7] = new SqlParameter("@amt", oProp.iAmt);
    //    oPara[8] = new SqlParameter("@wdate", oCommon.GetDateFormat(oProp.iwDate, 0));
    //    oPara[9] = new SqlParameter("@ChrgMode", oProp.iChrgmode);
    //    oPara[10] = new SqlParameter("@capno", oProp.iCapno);
    //    oPara[11] = new SqlParameter("@LateHeadID", oProp.iLatHeadID);
    //    oPara[12] = new SqlParameter("@LateAmt", oProp.dLateFee);
    //    oPara[13] = new SqlParameter("@wfChrgModeId", oProp.wfChrgModeId);
    //    oPara[14] = new SqlParameter("@wfFinYear", oProp.wfFinYear);

    //    //oPara[12] = new SqlParameter("@sqltype", oProp.iSqlType);
    //    //oPara[13] = new SqlParameter("@oldsrno", oProp.iOldSrno);
    //    //oPara[14] = new SqlParameter("@wtype", oProp.iwType);
    //    connection oConnection = new connection();

    //    //oConnection.SP_Execute("sp_InsWaterFee", oPara);
    //    oConnection.SP_Execute("sp_InsWaterFeeNew", oPara);
    //    return true;
    //}
    //public bool Inst_RebateSurcharge(propWater oProp)
    //{
    //    int iPara = 0;

    //    if (oProp.iAccountCode > 0)
    //        iPara = 15;
    //    else
    //        iPara = 14;

    //    SqlParameter[] oPara = new SqlParameter[iPara];

    //    oPara[0] = new SqlParameter("@compcode", oProp.iCompCode);
    //    oPara[1] = new SqlParameter("@mstcode", oProp.imstCode);
    //    oPara[2] = new SqlParameter("@wdate", oCommon.GetDateFormat(oProp.iwDate, 0));
    //    oPara[3] = new SqlParameter("@msttype", oProp.imsttype);
    //    oPara[4] = new SqlParameter("@wsrno", oProp.iSrNo);
    //    oPara[5] = new SqlParameter("@wfeeno", oProp.ifeeeno);
    //    oPara[6] = new SqlParameter("@wfeenm", oProp.ifeenm);
    //    oPara[7] = new SqlParameter("@wamt", oProp.iAmt);
    //    oPara[8] = new SqlParameter("@wwhat", oProp.iOnWhat);
    //    oPara[9] = new SqlParameter("@fromdate", oCommon.GetDateFormat(oProp.iOpenDate, 0));
    //    oPara[10] = new SqlParameter("@uptodate", oCommon.GetDateFormat(oProp.iEndDate, 0));
    //    oPara[11] = new SqlParameter("@sqltype", oProp.iSqlType);
    //    oPara[12] = new SqlParameter("@oldsrno", oProp.iOldSrno);
    //    oPara[13] = new SqlParameter("@code", oProp.iWCode);
    //    if (oProp.iAccountCode > 0)
    //        oPara[14] = new SqlParameter("@AccountCode", oProp.iAccountCode);

    //    connection oConnection = new connection();

    //    oConnection.SP_Execute("sp_InsRepbatSurcharge", oPara);
    //    return true;
    //}


    //public bool Del_WaterFeeS(propWater oProp)
    //{
    //    SqlParameter[] oPara = new SqlParameter[2];
    //    oPara[0] = new SqlParameter("@compcode", oProp.iCompCode);
    //    oPara[1] = new SqlParameter("@wcode", oProp.iWCode);
    //    connection oConnection = new connection();
    //    oConnection.SP_Execute("Del_WaterFeeSetup", oPara);
    //    return true;
    //}
    //public bool Del_RebateSucharge(propWater oProp)
    //{
    //    SqlParameter[] oPara = new SqlParameter[5];
    //    oPara[0] = new SqlParameter("@compcode", oProp.iCompCode);
    //    oPara[1] = new SqlParameter("@mstcode", oProp.imstCode);
    //    oPara[2] = new SqlParameter("@msttype", oProp.imsttype);
    //    oPara[3] = new SqlParameter("@wdate", oProp.iwDate);
    //    oPara[4] = new SqlParameter("@code", oProp.iWCode);
    //    connection oConnection = new connection();
    //    oConnection.SP_Execute("Del_RebateSuchSetup", oPara);
    //    return true;
    //}
    //public bool Ins_FinancialYear(PropProperty oProp)
    //{
    //    SqlParameter[] oPara = new SqlParameter[4];

    //    oPara[0] = new SqlParameter("@Start_dt", oProp.dStart_dt);
    //    oPara[1] = new SqlParameter("@End_dt", oProp.dEnd_dt);
    //    oPara[2] = new SqlParameter("@Session_dt", oProp.dSession_dt);
    //    oPara[3] = new SqlParameter("@Current_dt", oProp.dCurrent_dt);

    //    connection oConnection = new connection();

    //    oConnection.SP_Execute("InsFinYear", oPara);
    //    return true;
    //}

    //public bool DelPropertyRate(PropProperty oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("DelPropRate", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@date", SqlDbType.DateTime).Value = oProp.dDate;
    //    objcmd.Parameters.Add("@BuildType", SqlDbType.VarChar).Value = oProp.iTax_BuildingType;

    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}
    //public bool InsRentMst(PropRent oProp)
    //{

    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsRentMst", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@mstDate", SqlDbType.DateTime).Value = oProp.dDate;
    //    objcmd.Parameters.Add("@mstType", SqlDbType.Int).Value = oProp.iMstType;
    //    objcmd.Parameters.Add("@mstCode", SqlDbType.Int).Value = oProp.iMstCode;
    //    objcmd.Parameters.Add("@RentDesc", SqlDbType.VarChar).Value = oProp.sDesc;

    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}

    //public bool InsRentDet(PropRent oProp)
    //{

    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsRentDet", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@RentCode", SqlDbType.Int).Value = oProp.iMstCode;
    //    objcmd.Parameters.Add("@HeadID", SqlDbType.Int).Value = oProp.iHeadID;
    //    objcmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.iRentType;
    //    objcmd.Parameters.Add("@L_HeadID", SqlDbType.Int).Value = oProp.iL_HeadID;
    //    objcmd.Parameters.Add("@L_TypeID", SqlDbType.Int).Value = oProp.iL_TypeID;
    //    objcmd.Parameters.Add("@L_Amount", SqlDbType.Money).Value = oProp.dLateFee;
    //    objcmd.Parameters.Add("@Amount", SqlDbType.Money).Value = oProp.dRent;

    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}
    //public int GetSetCity(PropProperty oProp, int iHard = 0)
    //{
    //    dt = new DataTable();
    //    SqlCommand cmd = null;
    //    if (dr != null)
    //        dr.Close();
    //    GetConection();
    //    if (iHard == 0)
    //        cmd = new SqlCommand("InsCity", con);
    //    else
    //        cmd = new SqlCommand("InsHard", con);

    //    cmd.CommandType = CommandType.StoredProcedure;
    //    da = new SqlDataAdapter(cmd);

    //    cmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.iArv_Type;

    //    cmd.Parameters.Add("@Hname", SqlDbType.NVarChar).Value = oProp.sArv_HName;
    //    if (iHard == 0)
    //    {
    //        if (oProp.sBill_Bill_No != null && oProp.sBill_Bill_No != "0")
    //        {
    //            cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = oProp.sArv_Name + '-' + string.Format("{0:0000}", Convert.ToInt32(oProp.sBill_Bill_No));
    //            cmd.Parameters.Add("@cityexti", SqlDbType.VarChar).Value = string.Format("{0:0000}", Convert.ToInt32(oProp.sBill_Bill_No));
    //        }
    //        else
    //            cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = oProp.sArv_Name;
    //        if (oProp.iZWC_citycode != null && oProp.iZWC_citycode != 0)
    //            cmd.Parameters.Add("@Rute", SqlDbType.Int).Value = oProp.iZWC_citycode;
    //    }
    //    else
    //        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = oProp.sArv_Name;
    //    da.Fill(dt);
    //    cmd.Dispose();
    //    if (dt.Rows.Count > 0)
    //        return Convert.ToInt16(dt.Rows[0][0].ToString());
    //    else
    //        return 0;

    //}

    //public int InsStud(PropProperty oProp)
    //{
    //    dt = new DataTable();
    //    SqlCommand cmd = null;
    //    if (dr != null)
    //        dr.Close();
    //    GetConection();

    //    cmd = new SqlCommand("InsStud", con);
    //    cmd.CommandType = CommandType.StoredProcedure;
    //    da = new SqlDataAdapter(cmd);

    //    cmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.iArv_Type;
    //    cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = oProp.sArv_Name;
    //    cmd.Parameters.Add("@Hname", SqlDbType.NVarChar).Value = oProp.sArv_HName;

    //    da.Fill(dt);
    //    cmd.Dispose();
    //    if (dt.Rows.Count > 0)
    //        return Convert.ToInt16(dt.Rows[0][0].ToString());
    //    else
    //        return 0;

    //}

    //public bool InsArv(PropProperty oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsArv", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.AddWithValue("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.AddWithValue("@Date", SqlDbType.DateTime).Value = oProp.dDate;
    //    objcmd.Parameters.AddWithValue("@FinaYear", SqlDbType.VarChar).Value = oProp.sFinYear;
    //    objcmd.Parameters.AddWithValue("@Rate", SqlDbType.Money).Value = oProp.iArv_Rate;
    //    objcmd.Parameters.AddWithValue("@FloorID", SqlDbType.Int).Value = oProp.iARV_Floor;
    //    objcmd.Parameters.AddWithValue("@LocationID", SqlDbType.Int).Value = oProp.iARV_location;
    //    objcmd.Parameters.AddWithValue("@UseID", SqlDbType.Int).Value = oProp.iArv_Use;
    //    objcmd.Parameters.AddWithValue("@ConstructionTypeID", SqlDbType.Int).Value = oProp.iArv_ConstuctionType;
    //    objcmd.Parameters.AddWithValue("@ZoneID", SqlDbType.Int).Value = oProp.iARV_ZoneID;

    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}
    //public bool DelArv(int iCompId, int iArvID)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("DelARVSetup", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = iCompId;
    //    objcmd.Parameters.Add("@ArvID", SqlDbType.Int).Value = iArvID;

    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}
    //public bool DelPropertyRate(PropProperty oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("DelPropRate", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@date", SqlDbType.DateTime).Value = oProp.dDate;
    //    objcmd.Parameters.Add("@BuildType", SqlDbType.VarChar).Value = oProp.iTax_BuildingType;

    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}

    public bool delShopTrn(int iCompId, int iCode, int iType, int iRowID)
    {
        GetConection();
        SqlCommand objcmd = new SqlCommand("delShopTrn", con);
        objcmd.CommandType = CommandType.StoredProcedure;
        DataTable dt1 = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter();

        objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = iCompId;
        objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = iCode;
        objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = iType;
        objcmd.Parameters.Add("@trnrwid", SqlDbType.Int).Value = iRowID;

        objcmd.ExecuteNonQuery();
        return true;
    }
    public bool delWatTrn(int iCompId, int iCode, int iType, int iRowID)
    {
        GetConection();
        SqlCommand objcmd = new SqlCommand("delWatTrn", con);
        objcmd.CommandType = CommandType.StoredProcedure;
        DataTable dt1 = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter();

        objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = iCompId;
        objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = iCode;
        objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = iType;
        objcmd.Parameters.Add("@trnrwid", SqlDbType.Int).Value = iRowID;

        objcmd.ExecuteNonQuery();
        return true;
    }
    //public bool UpdShopAllot(PropRent oProp, int iDel)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("UpdShopAllot", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.iMstType;
    //    objcmd.Parameters.Add("@Shop", SqlDbType.Int).Value = oProp.iShopCode;
    //    objcmd.Parameters.Add("@CustID", SqlDbType.Int).Value = oProp.iMstCode;
    //    objcmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = oProp.sStatus;
    //    if (iDel == 1)
    //        objcmd.Parameters.Add("@Del", SqlDbType.VarChar).Value = 1;

    //    objcmd.ExecuteNonQuery();
    //    return true;

    //}

    //public bool InsPropertyRate(PropProperty oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsPropRate", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@date", SqlDbType.DateTime).Value = oProp.dDate;
    //    objcmd.Parameters.Add("@BuildType", SqlDbType.VarChar).Value = oProp.iTax_BuildingType;

    //    objcmd.Parameters.Add("@prtsrno", SqlDbType.Int).Value = oProp.iTax_PropSr;
    //    objcmd.Parameters.Add("@prtmini", SqlDbType.Int).Value = oProp.iTax_PropMin;
    //    objcmd.Parameters.Add("@prtmaxi", SqlDbType.Int).Value = oProp.iTax_PropMax;
    //    objcmd.Parameters.Add("@prtperc", SqlDbType.Int).Value = oProp.dTax_PropRate;

    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}

    //public bool InsPropTaxRate(PropProperty oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsPropTaxRate", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@date", SqlDbType.DateTime).Value = oProp.dDate;
    //    objcmd.Parameters.Add("@BuildType", SqlDbType.VarChar).Value = oProp.iTax_BuildingType;

    //    objcmd.Parameters.Add("@prtsrno", SqlDbType.Int).Value = oProp.iTax_ThSr;
    //    objcmd.Parameters.Add("@pattaxc", SqlDbType.Int).Value = oProp.iTax_TaxID;
    //    objcmd.Parameters.Add("@TName", SqlDbType.VarChar).Value = oProp.sTax_TaxName;
    //    objcmd.Parameters.Add("@patperc", SqlDbType.Int).Value = oProp.dTax_Amt;
    //    objcmd.Parameters.Add("@WhatID", SqlDbType.Int).Value = oProp.iTax_Mode;
    //    objcmd.Parameters.Add("@PType", SqlDbType.Int).Value = oProp.iTax_Type;

    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}

    //public bool InsPropOthRate(PropProperty oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsPropOthRate", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@date", SqlDbType.DateTime).Value = oProp.dDate;
    //    objcmd.Parameters.Add("@BuildType", SqlDbType.VarChar).Value = oProp.iTax_BuildingType;

    //    objcmd.Parameters.Add("@prtsrno", SqlDbType.Int).Value = oProp.iTax_OhSr;
    //    objcmd.Parameters.Add("@pCode", SqlDbType.Int).Value = oProp.iTax_OhOtherTax;
    //    objcmd.Parameters.Add("@TName", SqlDbType.VarChar).Value = oProp.sTax_OhOtherTaxNm;
    //    objcmd.Parameters.Add("@Use", SqlDbType.VarChar).Value = oProp.sTax_OhTypeNm;
    //    objcmd.Parameters.Add("@UseCode", SqlDbType.Int).Value = oProp.iTax_OhType;
    //    objcmd.Parameters.Add("@patperc", SqlDbType.Int).Value = oProp.iTax_OhPercent;
    //    objcmd.Parameters.Add("@For", SqlDbType.Int).Value = oProp.iTax_OhUsedID;
    //    objcmd.Parameters.Add("@Min", SqlDbType.Int).Value = oProp.iTax_OhMin;
    //    objcmd.Parameters.Add("@Max", SqlDbType.Int).Value = oProp.iTax_OhMax;
    //    objcmd.Parameters.Add("@WhatID", SqlDbType.Int).Value = oProp.iTax_OhOnWhat;

    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}
    //public bool InsUpdWMonth(PropProperty oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsUpdWMonth", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.iType_opbal;
    //    objcmd.Parameters.Add("@Code", SqlDbType.Int).Value = oProp.iCode_opbal;
    //    objcmd.Parameters.Add("@Cust", SqlDbType.Int).Value = oProp.iCust_opbal;
    //    objcmd.Parameters.Add("@Year", SqlDbType.Int).Value = oProp.iYear_opbal;
    //    objcmd.Parameters.Add("@HeadID", SqlDbType.Int).Value = oProp.iHeadID_opbal;
    //    objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.sCompcode_opbal;
    //    objcmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = oProp.iRate_opbal;
    //    objcmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = oProp.dDate_opbal;
    //    objcmd.Parameters.Add("@SrNo", SqlDbType.Int).Value = oProp.iSrNo_opbal;
    //    objcmd.Parameters.Add("@trnmn", SqlDbType.Int).Value = oProp.itrnmn_opbal;
    //    //objcmd.Parameters.AddWithValue("@trnpay", oProp.);
    //    //objcmd.Parameters.AddWithValue("@trnlatd",oProp.);
    //    objcmd.Parameters.AddWithValue("@trnamtp", 0);
    //    objcmd.Parameters.AddWithValue("@trmaina", oProp.iRate_opbal);
    //    if (oProp.trLatFee != "") objcmd.Parameters.AddWithValue("@trlatef", oProp.trLatFee); else objcmd.Parameters.AddWithValue("@trlatef", 0);
    //    objcmd.Parameters.AddWithValue("@trnTimeType", oProp.iTimeType);
    //    //objcmd.Parameters.AddWithValue("@trdater",oProp.);
    //    //objcmd.Parameters.AddWithValue("@trmnfrm",oProp.);
    //    //objcmd.Parameters.AddWithValue("@tryrfrm",oProp.);
    //    //objcmd.Parameters.AddWithValue("@trbilno",oProp.);
    //    //objcmd.Parameters.AddWithValue("@trlesam",oProp.);
    //    //objcmd.Parameters.AddWithValue("@trlessf",oProp.);
    //    objcmd.ExecuteNonQuery();

    //    return true;
    //}

    //public bool InsUpdSapu(PropProperty oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsUpdSapu", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.sCompcode_opbal;
    //    objcmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.iType_opbal;
    //    objcmd.Parameters.Add("@Code", SqlDbType.Int).Value = oProp.iCode_opbal;
    //    objcmd.Parameters.Add("@Year", SqlDbType.Int).Value = oProp.iYear_opbal;
    //    objcmd.Parameters.Add("@dram", SqlDbType.Decimal).Value = oProp.iAmount_opbal;
    //    objcmd.Parameters.Add("@itemID", SqlDbType.Int).Value = oProp.iHeadID_opbal;
    //    objcmd.Parameters.Add("@srno", SqlDbType.Int).Value = oProp.iSrNo_opbal;
    //    objcmd.Parameters.Add("@date", SqlDbType.DateTime).Value = oProp.dDate_opbal;
    //    objcmd.Parameters.Add("@cram", SqlDbType.Int).Value = oProp.icram_opbal;
    //    objcmd.Parameters.Add("@refc", SqlDbType.Int).Value = oProp.irefc_opbal;
    //    objcmd.Parameters.Add("@adju", SqlDbType.Int).Value = oProp.iadju_opbal;
    //    objcmd.Parameters.Add("@ledg", SqlDbType.Int).Value = oProp.iHeadID_opbal;
    //    objcmd.Parameters.Add("@expa", SqlDbType.Int).Value = oProp.iexpa_opbal;
    //    objcmd.Parameters.Add("@mstChno", SqlDbType.VarChar).Value = oProp.smstChno_opbal;
    //    objcmd.Parameters.Add("@income", SqlDbType.Int).Value = oProp.iincome_opbal;
    //    objcmd.Parameters.Add("@Cust", SqlDbType.Int).Value = oProp.iCustus_opbal;

    //    objcmd.Parameters.Add("@WmonthCode", SqlDbType.Int).Value = oProp.iWmonthCode_opbal;
    //    objcmd.Parameters.Add("@mststatus", SqlDbType.VarChar).Value = oProp.smststatus_opbal;
    //    objcmd.Parameters.Add("@mstneta", SqlDbType.Int).Value = oProp.imstneta_opbal;
    //    objcmd.Parameters.Add("@mstcfno", SqlDbType.Int).Value = oProp.imstcfno_opbal;
    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}
    //public bool UpdCodeGen(PropProperty oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("UpdCodeGen", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@comp", SqlDbType.Int).Value = oProp.icomp_opbal;
    //    objcmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.iTypee_opbal;
    //    objcmd.Parameters.Add("@Prev", SqlDbType.Int).Value = oProp.iPrev_opbal;
    //    objcmd.Parameters.Add("@Curr", SqlDbType.Int).Value = oProp.iCurr_opbal;
    //    objcmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = oProp.dDatee_opbal;
    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}
    //public bool InsPropMst(PropProperty oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsPropMst", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@COMPCODE", SqlDbType.Int).Value = oProp.iCOMPCODE_PTE;
    //    objcmd.Parameters.Add("@MSTSRNO", SqlDbType.Int).Value = oProp.iMSTSRNO_PTE;
    //    objcmd.Parameters.Add("@OWNNAME", SqlDbType.NVarChar).Value = oProp.sOWNNAME_PTE;
    //    objcmd.Parameters.Add("@FHNAME", SqlDbType.NVarChar).Value = oProp.sFHNAME_PTE;
    //    objcmd.Parameters.Add("@PLOTNO", SqlDbType.NVarChar).Value = oProp.sPLOTNO_PTE;
    //    objcmd.Parameters.Add("@COLONEY", SqlDbType.Int).Value = oProp.iCOLONEY_PTE;
    //    objcmd.Parameters.Add("@ACNO", SqlDbType.NVarChar).Value = oProp.sACNO_PTE;
    //    objcmd.Parameters.Add("@PHNO", SqlDbType.NVarChar).Value = oProp.sPHNO_PTE;
    //    objcmd.Parameters.Add("@TAPSIZE", SqlDbType.NVarChar).Value = oProp.sTAPSIZE_PTE;
    //    objcmd.Parameters.Add("@USEDFOR", SqlDbType.Int).Value = oProp.iUSEDFOR_PTE;
    //    objcmd.Parameters.Add("@AREA", SqlDbType.Int).Value = oProp.iAREA_PTE;
    //    objcmd.Parameters.Add("@mstDATE", SqlDbType.DateTime).Value = oProp.dmstDATE_PTE;
    //    objcmd.Parameters.Add("@msttype", SqlDbType.Int).Value = oProp.imsttype_PTE;

    //    objcmd.Parameters.Add("@mstTIME", SqlDbType.DateTime).Value = oProp.dmstTIME_PTE;
    //    objcmd.Parameters.Add("@MSTCHNO", SqlDbType.VarChar).Value = oProp.sMSTCHNO_PTE;
    //    objcmd.Parameters.Add("@zoneno", SqlDbType.Int).Value = oProp.izoneno_PTE;
    //    objcmd.Parameters.Add("@wardno", SqlDbType.Int).Value = oProp.iwardno_PTE;
    //    objcmd.Parameters.Add("@mstno", SqlDbType.Int).Value = oProp.imstno_PTE;
    //    objcmd.Parameters.Add("@mstnm", SqlDbType.Int).Value = oProp.imstnm_PTE;
    //    objcmd.Parameters.Add("@constyear", SqlDbType.Int).Value = oProp.iconstyear_PTE;
    //    objcmd.Parameters.Add("@Approvyear", SqlDbType.Int).Value = oProp.iApprovyear_PTE;
    //    objcmd.Parameters.Add("@tabconnyear", SqlDbType.Int).Value = oProp.itabconnyear_PTE;
    //    objcmd.Parameters.Add("@tabconnectno", SqlDbType.VarChar).Value = oProp.stabconnectno_PTE;
    //    objcmd.Parameters.Add("@otherproperty", SqlDbType.VarChar).Value = oProp.sotherproperty_PTE;
    //    objcmd.Parameters.Add("@constamt", SqlDbType.Decimal).Value = oProp.deconstamt_PTE;
    //    objcmd.Parameters.Add("@constamtperSqf", SqlDbType.Decimal).Value = oProp.deconstamtperSqf_PTE;

    //    objcmd.Parameters.Add("@nofloor", SqlDbType.Int).Value = oProp.inofloor_PTE;
    //    objcmd.Parameters.Add("@norooms", SqlDbType.Int).Value = oProp.inorooms_PTE;
    //    objcmd.Parameters.Add("@Pacno", SqlDbType.NVarChar).Value = oProp.sPacno_PTE;
    //    objcmd.Parameters.Add("@Noacno", SqlDbType.Int).Value = oProp.iNoacno_PTE;
    //    objcmd.Parameters.Add("@Wacno", SqlDbType.Int).Value = oProp.iWacno_PTE;
    //    objcmd.Parameters.Add("@Buildtype", SqlDbType.Int).Value = oProp.iBuildtype_PTE;
    //    objcmd.Parameters.Add("@bplnm", SqlDbType.VarChar).Value = oProp.sbplnm_PTE;
    //    objcmd.Parameters.Add("@newconstrctiondate", SqlDbType.DateTime).Value = oProp.dnewconstrctiondate_PTE;

    //    objcmd.Parameters.Add("@UniqueId", SqlDbType.NVarChar).Value = oProp.sUniqueid_PTE;
    //    objcmd.Parameters.Add("@SSSMId", SqlDbType.NVarChar).Value = oProp.ssssmid_PTE;
    //    objcmd.Parameters.Add("@APL_BPL", SqlDbType.NVarChar).Value = oProp.saplbplno_PTE;
    //    objcmd.Parameters.Add("@VoterId", SqlDbType.NVarChar).Value = oProp.sVoterid_PTE;
    //    objcmd.Parameters.Add("@Recharging", SqlDbType.NVarChar).Value = oProp.sWaterRecharging_PTE;
    //    objcmd.Parameters.Add("@ownHindi", SqlDbType.NVarChar).Value = oProp.sOwnerhindi_PTE;
    //    objcmd.Parameters.Add("@FatherHindi", SqlDbType.NVarChar).Value = oProp.sFatherhindi_PTE;
    //    objcmd.Parameters.Add("@CURADD", SqlDbType.NVarChar).Value = oProp.sCurrentAddress_PTE;

    //    objcmd.Parameters.Add("@SrvcNo1", SqlDbType.VarChar).Value = oProp.sSrvcNo1;
    //    objcmd.Parameters.Add("@SrvcNo2", SqlDbType.VarChar).Value = oProp.sSrvcNo2;
    //    objcmd.Parameters.Add("@SrvcNo3", SqlDbType.VarChar).Value = oProp.sSrvcNo3;
    //    objcmd.Parameters.Add("@SrvcNo4", SqlDbType.VarChar).Value = oProp.sSrvcNo4;
    //    objcmd.Parameters.Add("@ServiceNo", SqlDbType.VarChar).Value = oProp.sServiceNo;
    //    objcmd.Parameters.Add("@TenementNo", SqlDbType.VarChar).Value = oProp.sTenementNo;

    //    objcmd.Parameters.Add("@IsMainRoad", SqlDbType.Bit).Value = oProp.isMainRoad;

    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}

    //public bool InsPropDet(PropProperty oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsPropDet", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@COMPCODE", SqlDbType.Int).Value = oProp.iPTGE_COMPCODE;
    //    objcmd.Parameters.Add("@TRNSRNO", SqlDbType.Int).Value = oProp.iPTGE_TRNSRNO;
    //    objcmd.Parameters.Add("@SRNO", SqlDbType.Int).Value = oProp.iPTGE_SRNO;
    //    objcmd.Parameters.Add("@USECODE", SqlDbType.Int).Value = oProp.iPTGE_USECODE;
    //    objcmd.Parameters.Add("@CONSCODE", SqlDbType.Int).Value = oProp.iPTGE_CONSCODE;
    //    objcmd.Parameters.Add("@AREA", SqlDbType.Float).Value = oProp.fPTGE_AREA;
    //    objcmd.Parameters.Add("@RATE", SqlDbType.Decimal).Value = oProp.dePTGE_RATE;
    //    objcmd.Parameters.Add("@DISCOUNT", SqlDbType.NVarChar).Value = oProp.sPTGE_DISCOUNT;
    //    objcmd.Parameters.Add("@ANUMRATE", SqlDbType.Decimal).Value = oProp.dePTGE_ANUMRATE;
    //    objcmd.Parameters.Add("@trnDATE", SqlDbType.DateTime).Value = oProp.dPTGE_trnDATE;
    //    objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = oProp.iPTGE_trntype;
    //    objcmd.Parameters.Add("@TRNTIME", SqlDbType.DateTime).Value = oProp.dPTGE_TRNTIME;
    //    objcmd.Parameters.Add("@trnfloor", SqlDbType.Int).Value = oProp.iPTGE_trnfloor;

    //    objcmd.Parameters.Add("@noFrooms", SqlDbType.Int).Value = oProp.iPTGE_noFrooms;
    //    objcmd.Parameters.Add("@pused", SqlDbType.Int).Value = oProp.iPTGE_pused;
    //    objcmd.Parameters.Add("@ARVActual", SqlDbType.Decimal).Value = oProp.dePTGE_ARVActual;
    //    objcmd.Parameters.Add("@lenght", SqlDbType.Decimal).Value = oProp.dePTGE_lenght;
    //    objcmd.Parameters.Add("@weidth", SqlDbType.Decimal).Value = oProp.dePTGE_weidth;
    //    objcmd.Parameters.Add("@bhumikar", SqlDbType.Decimal).Value = oProp.dePTGE_bhumikar;
    //    objcmd.Parameters.Add("@newconstrctiondt", SqlDbType.DateTime).Value = oProp.dPTGE_newconstrctiondt;
    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}

    //public bool insRelation(PropProperty oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("insRelation", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@COMPCODE", SqlDbType.Int).Value = oProp.iPTA_COMPCODE;
    //    objcmd.Parameters.Add("@MSTTYPE", SqlDbType.Int).Value = oProp.iPTA_MSTTYPE;
    //    objcmd.Parameters.Add("@DRCODE", SqlDbType.Int).Value = oProp.iPTA_DRCODE;
    //    objcmd.Parameters.Add("@DRALAIA", SqlDbType.VarChar).Value = oProp.sPTA_DRALAIA;
    //    objcmd.Parameters.Add("@CRCODE", SqlDbType.Int).Value = oProp.iPTA_CRCODE;
    //    objcmd.Parameters.Add("@CRALAIA", SqlDbType.VarChar).Value = oProp.sPTA_CRALAIA;
    //    objcmd.Parameters.Add("@Code", SqlDbType.Int).Value = oProp.iPTA_Code;
    //    objcmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = oProp.dPTA_Date;
    //    if (oProp.renttype.ToString() != "" && oProp.renttype != 0)
    //        objcmd.Parameters.Add("@renttype", SqlDbType.Int).Value = oProp.renttype;
    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}
    //public bool DelReletion(PropProperty oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("DelReletion", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.iPTA_COMPCODE;
    //    objcmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.iPTA_MSTTYPE;
    //    objcmd.Parameters.Add("@Code", SqlDbType.Int).Value = oProp.iPTA_Code;
    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}

    //public bool InsCity(PropProperty oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsCity", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.iZWC_cityType;
    //    //  objcmd.Parameters.Add("@citycode", SqlDbType.Int).Value = oProp.iZWC_citycode;
    //    objcmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = oProp.sZWC_cityname;
    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}
    //#region " ******* Property Bill Save ******"

    //public bool delBill(PropProperty oProp, bool bol = true)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("delBill", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@Compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.iBill_type;
    //    objcmd.Parameters.Add("@Code", SqlDbType.Int).Value = oProp.iBill_Code;
    //    objcmd.Parameters.Add("@CfNo", SqlDbType.Int).Value = oProp.iBill_BillMst;
    //    objcmd.Parameters.Add("@OpDate", SqlDbType.DateTime).Value = oProp.iBill_OpDate;
    //    objcmd.Parameters.Add("@ClDate", SqlDbType.DateTime).Value = oProp.iBill_ClDate;
    //    objcmd.Parameters.Add("@cnclBit", SqlDbType.Bit).Value = bol;
    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}
    //public bool InsBillMst(PropProperty oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsBillMst", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iBill_Code;
    //    objcmd.Parameters.Add("@msttype", SqlDbType.Int).Value = oProp.iBill_type;
    //    objcmd.Parameters.Add("@bmstdate", SqlDbType.DateTime).Value = oProp.iBill_OpDate;
    //    objcmd.Parameters.Add("@bmstdued", SqlDbType.DateTime).Value = oProp.iBill_ClDate;
    //    objcmd.Parameters.Add("@bmsttype", SqlDbType.Int).Value = oProp.iBill_bmstType;
    //    objcmd.Parameters.Add("@bmstmn1", SqlDbType.Int).Value = oProp.iBill_OpMonth;
    //    objcmd.Parameters.Add("@bmsty1", SqlDbType.Int).Value = oProp.iBill_OpYear;
    //    objcmd.Parameters.Add("@bmstmn2", SqlDbType.Int).Value = oProp.iBill_ClMonth;
    //    objcmd.Parameters.Add("@bmsty2", SqlDbType.Int).Value = oProp.iBill_ClYear;
    //    objcmd.Parameters.Add("@bmstsrno", SqlDbType.Int).Value = oProp.iBill_SrNo1;
    //    objcmd.Parameters.Add("@bmstno", SqlDbType.VarChar).Value = oProp.sBill_Bill_No;
    //    objcmd.Parameters.Add("@bmstcust", SqlDbType.Int).Value = oProp.iBill_CustID;
    //    objcmd.Parameters.Add("@bmstfnm", SqlDbType.VarChar).Value = oProp.sBill_FName;
    //    objcmd.Parameters.Add("@bmstservice", SqlDbType.VarChar).Value = oProp.iBill_Service;
    //    objcmd.Parameters.Add("@bmstacno", SqlDbType.VarChar).Value = oProp.iBill_AcNo;
    //    objcmd.Parameters.Add("@bmstuse", SqlDbType.VarChar).Value = oProp.iBill_Use;
    //    objcmd.Parameters.Add("@bmstwidth", SqlDbType.Float).Value = oProp.iBill_Width;
    //    objcmd.Parameters.Add("@bmstzone", SqlDbType.Int).Value = oProp.iBill_Zone;
    //    objcmd.Parameters.Add("@bmstward", SqlDbType.Int).Value = oProp.iBill_Ward;
    //    objcmd.Parameters.Add("@bmstprev", SqlDbType.Money).Value = oProp.iBill_Prev;
    //    objcmd.Parameters.Add("@bmstmnch", SqlDbType.Money).Value = oProp.dBill_Mnch;
    //    objcmd.Parameters.Add("@bmstcurr", SqlDbType.Money).Value = oProp.iBill_Curr;
    //    objcmd.Parameters.Add("@bmstdue", SqlDbType.Money).Value = oProp.iBill_Due;
    //    objcmd.Parameters.Add("@bmstacct", SqlDbType.Int).Value = oProp.iBill_Acct;
    //    objcmd.Parameters.Add("@bmstopbl", SqlDbType.Money).Value = oProp.iBill_OpBal;
    //    objcmd.Parameters.Add("@bmstarea", SqlDbType.Money).Value = oProp.iBill_Area;
    //    objcmd.Parameters.Add("@bmstarate", SqlDbType.Money).Value = oProp.iBill_Rate;
    //    objcmd.Parameters.Add("@bmsttaxp", SqlDbType.Money).Value = oProp.iBill_Taxp;
    //    objcmd.Parameters.Add("@billmst", SqlDbType.Int).Value = oProp.iBill_BillMst;

    //    objcmd.Parameters.Add("@bmstChMd", SqlDbType.VarChar).Value = oProp.dBill_ChMd;
    //    objcmd.Parameters.Add("@bmstAfDt", SqlDbType.Money).Value = oProp.dBill_AfDt;
    //    objcmd.Parameters.Add("@bmstplot", SqlDbType.VarChar).Value = oProp.iBill_Plot;
    //    objcmd.Parameters.Add("@bmstLatFee", SqlDbType.Money).Value = oProp.dBill_LatFee;
    //    objcmd.Parameters.Add("@bmstLatHead", SqlDbType.Int).Value = oProp.iLatfeeHeadID;
    //    objcmd.Parameters.Add("@bmstinco", SqlDbType.Int).Value = oProp.iBill_BillIncome;
    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}

    //public bool InsBillMstSepu(PropProperty oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsBillMstSepu", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@msttype", SqlDbType.Int).Value = oProp.iBill_type;
    //    objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iBill_Code;
    //    objcmd.Parameters.Add("@mstdate", SqlDbType.DateTime).Value = oProp.iBill_OpDate;
    //    objcmd.Parameters.Add("@mstrefc", SqlDbType.VarChar).Value = oProp.iBill_SrNo + "~0~0~0";
    //    objcmd.Parameters.Add("@mstchno", SqlDbType.VarChar).Value = oProp.sBill_Bill_No;// iBill_BillNo;
    //    objcmd.Parameters.Add("@mstcfno", SqlDbType.VarChar).Value = oProp.iBill_BillMst;
    //    objcmd.Parameters.Add("@mstbldt", SqlDbType.DateTime).Value = oProp.iBill_OpDate;
    //    objcmd.Parameters.Add("@msttota", SqlDbType.Money).Value = oProp.dBill_Mnch;
    //    objcmd.Parameters.Add("@mstcust", SqlDbType.Int).Value = oProp.iBill_CustID;
    //    objcmd.Parameters.Add("@mstincome", SqlDbType.Int).Value = oProp.iBill_BillIncome;
    //    objcmd.Parameters.Add("@mstneta", SqlDbType.Money).Value = oProp.dBill_Mnch;
    //    objcmd.Parameters.Add("@mststatus", SqlDbType.Char).Value = "M";
    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}

    //public bool InsBillTrn(PropProperty oProp, int pMstType, int pTypeCode, int pBillType)
    //{
    //    DataTable dt = GetData("sp_GetHeadWiseAmt @pCompCodeVl=" + oProp.iCompCode + ", @pFromMonthV='" + DataConn.getOpenDate(oProp.iBill_OpDate) + "', @pUptoMonthV ='" + DataConn.getClosDate(oProp.iBill_OpDate) + "', @pTypeCodeVl=" + pTypeCode + ",@pTrnCode=" + oProp.iBill_CustID + ",@pMstType=" + pMstType + ",@pMonth=" + oCommon.ConvertFinMonth(oProp.iBill_OpMonth.ToString(), 2) + ",@pBillType=" + pBillType + ",@HeadId=" + oProp.iHeadID);
    //    if (dt != null && dt.Rows.Count > 0)
    //    {
    //        GetConection();
    //        SqlCommand objcmd;
    //        for (int i = 0, j = 1; i < dt.Rows.Count; i++)
    //        {
    //            if (dt.Rows[i]["DRCODE"].ToString() != "")
    //            {
    //                objcmd = new SqlCommand("InsBillTrn", con);
    //                objcmd.CommandType = CommandType.StoredProcedure;

    //                objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //                objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = oProp.iBill_type;
    //                objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = oProp.iBill_Code;
    //                objcmd.Parameters.Add("@trndate", SqlDbType.DateTime).Value = oProp.iBill_OpDate;
    //                objcmd.Parameters.Add("@trnrefc", SqlDbType.Int).Value = 0;
    //                objcmd.Parameters.Add("@trnexpr", SqlDbType.Int).Value = 0;
    //                objcmd.Parameters.Add("@trnrema", SqlDbType.NVarChar).Value = "";
    //                objcmd.Parameters.Add("@trnadju", SqlDbType.Money).Value = 0;
    //                objcmd.Parameters.Add("@trnexpa", SqlDbType.Money).Value = 0;
    //                objcmd.Parameters.Add("@trnaddv", SqlDbType.Money).Value = 0;
    //                objcmd.Parameters.Add("@trnchno", SqlDbType.NVarChar).Value = "";
    //                objcmd.Parameters.Add("@trnbank", SqlDbType.VarChar).Value = "";
    //                objcmd.Parameters.Add("@trntime", SqlDbType.Int).Value = 0;

    //                objcmd.Parameters.Add("@trnsrno", SqlDbType.Int).Value = j++;
    //                objcmd.Parameters.Add("@trnitem", SqlDbType.Int).Value = dt.Rows[i]["DRCODE"].ToString();
    //                objcmd.Parameters.Add("@trnledg", SqlDbType.Int).Value = dt.Rows[i]["DRCODE"].ToString();
    //                objcmd.Parameters.Add("@trndram", SqlDbType.Money).Value = dt.Rows[i]["trnrent"].ToString(); //oProp.iBill_Curr;
    //                objcmd.ExecuteNonQuery();
    //            }
    //            if (dt.Rows[i]["CRCODE"].ToString() != "")
    //            {
    //                objcmd = new SqlCommand("InsBillTrn", con);
    //                objcmd.CommandType = CommandType.StoredProcedure;

    //                objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //                objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = oProp.iBill_type;
    //                objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = oProp.iBill_Code;
    //                objcmd.Parameters.Add("@trndate", SqlDbType.DateTime).Value = oProp.iBill_OpDate;
    //                objcmd.Parameters.Add("@trnrefc", SqlDbType.Int).Value = 0;
    //                objcmd.Parameters.Add("@trnexpr", SqlDbType.Int).Value = 0;
    //                objcmd.Parameters.Add("@trnrema", SqlDbType.NVarChar).Value = "";
    //                objcmd.Parameters.Add("@trnadju", SqlDbType.Money).Value = 0;
    //                objcmd.Parameters.Add("@trnexpa", SqlDbType.Money).Value = 0;
    //                objcmd.Parameters.Add("@trnaddv", SqlDbType.Money).Value = 0;
    //                objcmd.Parameters.Add("@trnchno", SqlDbType.NVarChar).Value = "";
    //                objcmd.Parameters.Add("@trnbank", SqlDbType.VarChar).Value = "";
    //                objcmd.Parameters.Add("@trntime", SqlDbType.Int).Value = 0;

    //                objcmd.Parameters.Add("@trnsrno", SqlDbType.Int).Value = j++;
    //                objcmd.Parameters.Add("@trnitem", SqlDbType.Int).Value = dt.Rows[i]["CRCODE"].ToString();
    //                objcmd.Parameters.Add("@trnledg", SqlDbType.Int).Value = dt.Rows[i]["CRCODE"].ToString();
    //                objcmd.Parameters.Add("@trncram", SqlDbType.Money).Value = dt.Rows[i]["trnrent"].ToString(); //oProp.iBill_Curr;
    //                objcmd.ExecuteNonQuery();
    //            }
    //        }
    //    }
    //    return true;
    //}
    //public bool InsBillWtrn(PropProperty oProp, int iHeadwise, int iType, int pMstType, int pTypeCode, int pBillType)
    //{
    //    GetConection();
    //    SqlCommand objcmd;
    //    DataTable dt = GetData("sp_GetHeadWiseAmt @pCompCodeVl=" + oProp.iCompCode + ", @pFromMonthV='" + DataConn.getOpenDate(oProp.iBill_OpDate) + "', @pUptoMonthV ='" + DataConn.getClosDate(oProp.iBill_OpDate) + "', @pTypeCodeVl=" + pTypeCode + ",@pTrnCode=" + oProp.iBill_CustID + ",@pMstType=" + pMstType + ",@pMonth=" + oCommon.ConvertFinMonth(oProp.iBill_OpMonth.ToString(), 2) + ",@pBillType=" + pBillType + ",@HeadId=" + oProp.iHeadID);
    //    if (dt != null && dt.Rows.Count > 0)
    //    {
    //        for (int k = 0, j = 1; k < dt.Rows.Count; k++)
    //        {
    //            if (dt.Rows[k]["DRCODE"].ToString() != "")
    //            {
    //                objcmd = new SqlCommand("InsBillWtrn", con);
    //                objcmd.CommandType = CommandType.StoredProcedure;

    //                objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //                objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = oProp.iBill_Code;
    //                objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = oProp.iBill_type;
    //                objcmd.Parameters.Add("@trncust", SqlDbType.Int).Value = oProp.iBill_CustID;
    //                objcmd.Parameters.Add("@trnbillno", SqlDbType.Int).Value = oProp.iBill_Code;
    //                objcmd.Parameters.Add("@trndate", SqlDbType.DateTime).Value = oProp.iBill_OpDate;
    //                objcmd.Parameters.Add("@trnmn", SqlDbType.Int).Value = oProp.iBill_OpMonth;

    //                if (iHeadwise == 1)
    //                {
    //                    //objcmd.Parameters.Add("@trnmn", SqlDbType.Int).Value = oProp.iBill_;
    //                    objcmd.Parameters.Add("@trnyr", SqlDbType.Int).Value = oProp.iBill_OpYear;
    //                    objcmd.Parameters.Add("@trheadc", SqlDbType.Int).Value = dt.Rows[k]["DRCODE"].ToString();
    //                    objcmd.Parameters.Add("@trnamtd", SqlDbType.Money).Value = dt.Rows[k]["trnrent"].ToString(); // oProp.iBill_Curr;
    //                    objcmd.Parameters.Add("@trnamtp", SqlDbType.Money).Value = 0;
    //                    objcmd.Parameters.Add("@trmaina", SqlDbType.Money).Value = dt.Rows[k]["trnrent"].ToString();
    //                    objcmd.Parameters.Add("@trlatef", SqlDbType.Money).Value = 0;
    //                    objcmd.Parameters.AddWithValue("@trLatHead", oProp.iLatfeeHeadID);
    //                }
    //                else
    //                {
    //                    //objcmd.Parameters.Add("@trnmn", SqlDbType.Int).Value = oProp.iBill_;
    //                    //objcmd.Parameters.Add("@trnyr", SqlDbType.Int).Value = oProp.iBill_;
    //                    //objcmd.Parameters.Add("@trheadc", SqlDbType.Int).Value = oProp.iBill_;
    //                    //objcmd.Parameters.Add("@trnamtd", SqlDbType.Money).Value = oProp.iBill_;
    //                }

    //                if (iType == 1)
    //                {
    //                    objcmd.Parameters.Add("@trnpay", SqlDbType.Int).Value = oProp.iBill_CustID;
    //                    objcmd.Parameters.Add("@TRNSRNO", SqlDbType.Int).Value = oProp.iBill_CustID;
    //                    objcmd.Parameters.Add("@trmnfrm", SqlDbType.Int).Value = oProp.iBill_CustID;
    //                    objcmd.Parameters.Add("@tryrfrm", SqlDbType.Int).Value = oProp.iBill_CustID;
    //                    objcmd.Parameters.Add("@trbilno", SqlDbType.Int).Value = oProp.iBill_CustID;
    //                    objcmd.Parameters.Add("@trnlatd", SqlDbType.Money).Value = oProp.iBill_CustID;

    //                    objcmd.Parameters.Add("@trlesam", SqlDbType.Money).Value = oProp.iBill_CustID;
    //                    objcmd.Parameters.Add("@trlessf", SqlDbType.Money).Value = oProp.iBill_CustID;
    //                    objcmd.Parameters.Add("@trdater", SqlDbType.Date).Value = oProp.iBill_CustID;
    //                }
    //                objcmd.ExecuteNonQuery();
    //            }
    //        }
    //    }
    //    return true;
    //}

    //public bool InsWMtrn_Rent(PropProperty oProp)
    //{

    //    GetConection();
    //    SqlCommand objcmd;
    //    objcmd = new SqlCommand("InsBillWtrn", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = oProp.iPay_Code;
    //    objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = oProp.iPay_type;
    //    objcmd.Parameters.Add("@trncust", SqlDbType.Int).Value = oProp.iPay_Cust;
    //    objcmd.Parameters.Add("@trndate", SqlDbType.DateTime).Value = oProp.iPay_Date;
    //    objcmd.Parameters.Add("@trnmn", SqlDbType.Int).Value = oProp.iPay_MN;
    //    objcmd.Parameters.Add("@trnyr", SqlDbType.Int).Value = oProp.iPay_Yr;
    //    objcmd.Parameters.Add("@trheadc", SqlDbType.Int).Value = oProp.iPay_Head;
    //    objcmd.Parameters.Add("@trnamtd", SqlDbType.Money).Value = oProp.iPay_Amtd;
    //    objcmd.Parameters.Add("@trnpay", SqlDbType.Int).Value = oProp.iPay_Pay;
    //    objcmd.Parameters.Add("@TRNSRNO", SqlDbType.Int).Value = oProp.iPay_SrNo;
    //    objcmd.Parameters.Add("@trnamtp", SqlDbType.Money).Value = oProp.iPay_Amtp;
    //    objcmd.Parameters.Add("@trmaina", SqlDbType.Money).Value = oProp.iPay_Ina;
    //    objcmd.Parameters.Add("@trlatef", SqlDbType.Money).Value = oProp.iPay_Latef;
    //    objcmd.Parameters.Add("@trlessf", SqlDbType.Money).Value = oProp.iPay_Lessf;
    //    objcmd.Parameters.AddWithValue("@trLatHead", oProp.iLatfeeHeadID);
    //    objcmd.Parameters.AddWithValue("@trnTimeType", oProp.iTimeType);
    //    objcmd.ExecuteNonQuery();

    //    return true;
    //}


    //#endregion


    //#region " ******* Water ******"


    //public bool InsWmst(propWater oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsWmst", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@watcode", SqlDbType.Int).Value = oProp.iWCode;
    //    objcmd.Parameters.Add("@watcity", SqlDbType.Int).Value = oProp.iCity;
    //    objcmd.Parameters.Add("@watcolony", SqlDbType.Int).Value = oProp.iColony;
    //    objcmd.Parameters.Add("@watzone", SqlDbType.Int).Value = oProp.iZone;
    //    objcmd.Parameters.Add("@watward", SqlDbType.Int).Value = oProp.iWard;
    //    objcmd.Parameters.Add("@watcategory", SqlDbType.Int).Value = oProp.iwCategory;
    //    objcmd.Parameters.Add("@watrelation", SqlDbType.Int).Value = oProp.iwRelation;
    //    objcmd.Parameters.Add("@watidno", SqlDbType.Int).Value = oProp.iwIdNo;
    //    objcmd.Parameters.Add("@watonm", SqlDbType.VarChar).Value = oProp.sWOname;
    //    objcmd.Parameters.Add("@watfnm", SqlDbType.VarChar).Value = oProp.sWFName;
    //    objcmd.Parameters.Add("@watplotno", SqlDbType.VarChar).Value = oProp.sWPlotno;
    //    objcmd.Parameters.Add("@watservice", SqlDbType.VarChar).Value = oProp.sWService;
    //    objcmd.Parameters.Add("@watacno", SqlDbType.VarChar).Value = oProp.sWAcno;
    //    objcmd.Parameters.Add("@watstatus", SqlDbType.VarChar).Value = oProp.sWStatus;
    //    objcmd.Parameters.Add("@remark", SqlDbType.VarChar).Value = oProp.sWRemark;
    //    objcmd.Parameters.Add("@watphone", SqlDbType.VarChar).Value = oProp.sWPhone;
    //    objcmd.Parameters.Add("@watmobile", SqlDbType.VarChar).Value = oProp.sWMobile;
    //    objcmd.Parameters.Add("@watownerHind", SqlDbType.NVarChar).Value = oProp.sWOHind;
    //    objcmd.Parameters.Add("@watFHhindi", SqlDbType.NVarChar).Value = oProp.sWFhindi;

    //    objcmd.Parameters.Add("@SrvcNo1", SqlDbType.VarChar).Value = oProp.sSrvcNo1;
    //    objcmd.Parameters.Add("@SrvcNo2", SqlDbType.VarChar).Value = oProp.sSrvcNo2;
    //    objcmd.Parameters.Add("@SrvcNo3", SqlDbType.VarChar).Value = oProp.sSrvcNo3;
    //    objcmd.Parameters.Add("@SrvcNo4", SqlDbType.VarChar).Value = oProp.sSrvcNo4;

    //    objcmd.Parameters.Add("@watdate", SqlDbType.DateTime).Value = oProp.dWDate;
    //    objcmd.Parameters.Add("@watplotnohindi", SqlDbType.NVarChar).Value = oProp.sWPlotHindi;
    //    objcmd.Parameters.Add("@watmonthcharge", SqlDbType.Money).Value = oProp.dWMonthCharge;

    //    objcmd.Parameters.Add("@watConHldType", SqlDbType.VarChar).Value = oProp.watConHldType;
    //    objcmd.Parameters.Add("@watHouseType", SqlDbType.VarChar).Value = oProp.watHouseType;

    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}

    //public bool InsWtrn(propWater oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsWtrn", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = oProp.iWCode;
    //    objcmd.Parameters.Add("@srno", SqlDbType.Int).Value = oProp.iSrNo;
    //    objcmd.Parameters.Add("@stopno", SqlDbType.Int).Value = oProp.iStopNo;
    //    objcmd.Parameters.Add("@capno", SqlDbType.Int).Value = oProp.iCapno;
    //    objcmd.Parameters.Add("@usecode", SqlDbType.Int).Value = oProp.iUsecode;
    //    objcmd.Parameters.Add("@garea", SqlDbType.Int).Value = oProp.iGarden;

    //    objcmd.Parameters.Add("@stopnm", SqlDbType.VarChar).Value = oProp.sStopNm;
    //    objcmd.Parameters.Add("@indus", SqlDbType.VarChar).Value = oProp.sIndustrial;
    //    objcmd.Parameters.Add("@ChrgMode", SqlDbType.VarChar).Value = oProp.sMode;
    //    objcmd.Parameters.Add("@width", SqlDbType.Float).Value = oProp.dWidth;
    //    objcmd.Parameters.Add("@usedfor", SqlDbType.VarChar).Value = oProp.sUseFor;

    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}
    //#endregion

    //public bool InsSmst(PropRent oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsSmst", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@msttype", SqlDbType.Int).Value = oProp.iMstType;
    //    objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iMstCode;
    //    objcmd.Parameters.Add("@city", SqlDbType.Int).Value = oProp.iCity;
    //    objcmd.Parameters.Add("@colony", SqlDbType.Int).Value = oProp.iColony;
    //    objcmd.Parameters.Add("@Szone", SqlDbType.Int).Value = oProp.iZone;
    //    objcmd.Parameters.Add("@ward", SqlDbType.Int).Value = oProp.iWard;
    //    objcmd.Parameters.Add("@term", SqlDbType.Int).Value = oProp.iTerm;
    //    objcmd.Parameters.Add("@Agreeterm", SqlDbType.Int).Value = oProp.iAgreeterm;
    //    objcmd.Parameters.Add("@renttype", SqlDbType.Int).Value = oProp.iRentType;
    //    objcmd.Parameters.Add("@rent", SqlDbType.Money).Value = oProp.dRent;
    //    objcmd.Parameters.Add("@Incvalu", SqlDbType.Money).Value = oProp.dIncValu;
    //    objcmd.Parameters.Add("@Length", SqlDbType.Money).Value = oProp.dLength;
    //    objcmd.Parameters.Add("@breadth", SqlDbType.Money).Value = oProp.dBreadth;
    //    objcmd.Parameters.Add("@mstdate", SqlDbType.DateTime).Value = oProp.dDate;
    //    objcmd.Parameters.Add("@Incdate", SqlDbType.DateTime).Value = oProp.dIncDate;
    //    objcmd.Parameters.Add("@shopdes", SqlDbType.VarChar).Value = oProp.sDesc;
    //    objcmd.Parameters.Add("@shopno", SqlDbType.VarChar).Value = oProp.sShopNo;

    //    objcmd.Parameters.Add("@SrvcNo1", SqlDbType.VarChar).Value = oProp.sSrvcNo1;
    //    objcmd.Parameters.Add("@SrvcNo2", SqlDbType.VarChar).Value = oProp.sSrvcNo2;
    //    objcmd.Parameters.Add("@SrvcNo3", SqlDbType.VarChar).Value = oProp.sSrvcNo3;
    //    objcmd.Parameters.Add("@SrvcNo4", SqlDbType.VarChar).Value = oProp.sSrvcNo4;

    //    objcmd.Parameters.Add("@complex", SqlDbType.VarChar).Value = oProp.sComplex;
    //    objcmd.Parameters.Add("@area", SqlDbType.VarChar).Value = oProp.sArea;
    //    objcmd.Parameters.Add("@address", SqlDbType.VarChar).Value = oProp.sAddress;

    //    objcmd.Parameters.Add("@Location_I", SqlDbType.Int).Value = oProp.iLocation_I;
    //    objcmd.Parameters.Add("@Location_II", SqlDbType.Int).Value = oProp.iLocation_II;
    //    objcmd.Parameters.Add("@S_No", SqlDbType.Int).Value = oProp.iS_No;

    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}


    //public bool InsStrn(PropRent oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsStrn", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = oProp.iMstType;
    //    objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = oProp.iMstCode;
    //    objcmd.Parameters.Add("@trnsrno", SqlDbType.Int).Value = oProp.iSrNo;
    //    objcmd.Parameters.Add("@trndate", SqlDbType.DateTime).Value = oProp.dDate;
    //    objcmd.Parameters.Add("@trnfrom", SqlDbType.DateTime).Value = oProp.dFrom;
    //    //objcmd.Parameters.Add("@trnupto", SqlDbType.DateTime).Value = oProp.dUpto;
    //    objcmd.Parameters.Add("@trnrent", SqlDbType.Money).Value = oProp.dRent;
    //    objcmd.Parameters.Add("@trnHead", SqlDbType.Int).Value = oProp.iHeadID;
    //    objcmd.Parameters.Add("@trnLatFee", SqlDbType.Money).Value = oProp.dLateFee;
    //    objcmd.Parameters.Add("@trnLatHead", SqlDbType.Int).Value = oProp.iL_HeadID;//sourabh 22-dec-2014
    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}

    //public bool InsWatPayment(PropRent oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsWatPayment", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = oProp.iMstType;
    //    objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = oProp.iMstCode;
    //    objcmd.Parameters.Add("@trnsrno", SqlDbType.Int).Value = oProp.iSrNo;
    //    objcmd.Parameters.Add("@trndate", SqlDbType.DateTime).Value = oProp.dDate;
    //    objcmd.Parameters.Add("@trnfrom", SqlDbType.DateTime).Value = oProp.dFrom;
    //    //objcmd.Parameters.Add("@trnupto", SqlDbType.DateTime).Value = oProp.dUpto;
    //    objcmd.Parameters.Add("@trnrent", SqlDbType.Money).Value = oProp.dRent;
    //    objcmd.Parameters.Add("@trnHead", SqlDbType.Int).Value = oProp.iHeadID;
    //    objcmd.Parameters.Add("@trnLatFee", SqlDbType.Money).Value = oProp.dLateFee;
    //    objcmd.Parameters.Add("@trnLatHead", SqlDbType.Int).Value = oProp.iL_HeadID;
    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}

    //public bool insWatStatus(PropRent oProp, int SqlType = 0)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("insWatStatus", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@SqlType", SqlDbType.Int).Value = SqlType;
    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@MstCode", SqlDbType.Int).Value = oProp.iMstCode;
    //    if (SqlType != 2)
    //    {
    //        objcmd.Parameters.Add("@MstDate", SqlDbType.DateTime).Value = oProp.dDate;
    //        objcmd.Parameters.Add("@Remark", SqlDbType.VarChar).Value = oProp.sRemark;
    //    }
    //    objcmd.Parameters.Add("@Connect", SqlDbType.Bit).Value = oProp.iStatus;
    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}
    //public bool insRentHead(PropRent oProp)
    //{

    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("insRentHead", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iMstCode;
    //    objcmd.Parameters.Add("@mstDate", SqlDbType.DateTime).Value = oProp.dDate;
    //    objcmd.Parameters.Add("@HeadID", SqlDbType.Int).Value = oProp.iHeadID;
    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}


    //public DataTable GetSetStrn(PropRent oProp)
    //{
    //    dt = new DataTable();
    //    SqlCommand cmd = null;
    //    if (dr != null)
    //        dr.Close();
    //    GetConection();

    //    cmd = new SqlCommand("InsStrn", con); 

    //    cmd.CommandType = CommandType.StoredProcedure;
    //    da = new SqlDataAdapter(cmd);

    //    cmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    cmd.Parameters.Add("@trntype", SqlDbType.Int).Value = oProp.iMstType;
    //    cmd.Parameters.Add("@trncode", SqlDbType.Int).Value = oProp.iMstCode;
    //    cmd.Parameters.Add("@trnsrno", SqlDbType.Int).Value = oProp.iSrNo;
    //    cmd.Parameters.Add("@trndate", SqlDbType.DateTime).Value = oProp.dDate;
    //    cmd.Parameters.Add("@trnfrom", SqlDbType.DateTime).Value = oProp.dFrom;
    //    //objcmd.Parameters.Add("@trnupto", SqlDbType.DateTime).Value = oProp.dUpto;
    //    cmd.Parameters.Add("@trnrent", SqlDbType.Money).Value = oProp.dRent; 

    //    da.Fill(dt);
    //    cmd.Dispose();

    //    return dt;

    //}

    //    public bool InsShopallot(PropRent oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsShopallot", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@msttype", SqlDbType.Int).Value = oProp.iMstType;
    //        objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iMstCode;
    //        objcmd.Parameters.Add("@shopcd", SqlDbType.Int).Value = oProp.iShopCode;
    //        objcmd.Parameters.Add("@renttype", SqlDbType.Int).Value = oProp.iRentType;
    //        objcmd.Parameters.Add("@mstdate", SqlDbType.DateTime).Value = oProp.dDate;
    //        // objcmd.Parameters.Add("@uptodate", SqlDbType.DateTime).Value = oProp.iMstType; 
    //        objcmd.Parameters.Add("@custnm", SqlDbType.VarChar).Value = oProp.sCustName;
    //        objcmd.Parameters.Add("@shopno", SqlDbType.VarChar).Value = oProp.sShopNo;
    //        objcmd.Parameters.Add("@shopdesc", SqlDbType.VarChar).Value = oProp.sDesc;
    //        objcmd.Parameters.Add("@status", SqlDbType.VarChar).Value = oProp.sStatus;
    //        objcmd.Parameters.Add("@rent", SqlDbType.Money).Value = oProp.dRent;
    //        objcmd.Parameters.Add("@CustCd", SqlDbType.Int).Value = oProp.iCustCode;
    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }

    //    public bool InsCustmst(PropRent oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsCustmst", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@msttype", SqlDbType.Int).Value = oProp.iMstType;
    //        objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iMstCode;

    //        objcmd.Parameters.Add("@mstdate", SqlDbType.DateTime).Value = oProp.dDate;
    //        objcmd.Parameters.Add("@city", SqlDbType.Int).Value = oProp.iCity;
    //        objcmd.Parameters.Add("@ward", SqlDbType.Int).Value = oProp.iWard;
    //        objcmd.Parameters.Add("@colony", SqlDbType.Int).Value = oProp.iColony;
    //        objcmd.Parameters.Add("@Czone", SqlDbType.Int).Value = oProp.iZone;

    //        objcmd.Parameters.Add("@custname", SqlDbType.VarChar).Value = oProp.sCustName;
    //        objcmd.Parameters.Add("@father", SqlDbType.VarChar).Value = oProp.sFName;
    //        objcmd.Parameters.Add("@address", SqlDbType.VarChar).Value = oProp.sAddress;

    //        objcmd.Parameters.Add("@phone", SqlDbType.VarChar).Value = oProp.sPhone;
    //        objcmd.Parameters.Add("@mobile", SqlDbType.VarChar).Value = oProp.sMobile;
    //        objcmd.Parameters.Add("@rema", SqlDbType.VarChar).Value = oProp.sRema;
    //        objcmd.Parameters.Add("@custhindi", SqlDbType.NVarChar).Value = oProp.sCustHindi;
    //        objcmd.Parameters.Add("@fatherhindi", SqlDbType.NVarChar).Value = oProp.sFatherHindi;


    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    public bool UdpCode(int iCompCode, int iType, int iPrev, int iCurr)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("UdpCode", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = iCompCode;
    //        objcmd.Parameters.Add("@codetype", SqlDbType.Int).Value = iType;
    //        objcmd.Parameters.Add("@codeprev", SqlDbType.Int).Value = iPrev;
    //        objcmd.Parameters.Add("@codecurr", SqlDbType.Int).Value = iCurr;

    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }

    //    public bool InsShopDesc(int iLocationID, int iDivisionID, int iShopNo)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsShopDesc", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@LocationID", SqlDbType.Int).Value = iLocationID;
    //        objcmd.Parameters.Add("@DivisionID", SqlDbType.Int).Value = iDivisionID;
    //        objcmd.Parameters.Add("@ShopNo", SqlDbType.Int).Value = iShopNo;

    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }

    //    public object GetSingleData(String sQuery)
    //    {
    //        SqlCommand cmdL = null;
    //        GetConection();
    //        cmdL = new SqlCommand(sQuery, con);
    //        cmdL.CommandTimeout = 3000;
    //        object s = cmdL.ExecuteScalar();
    //        if (s != null && s.ToString() != "")
    //            return s;
    //        else
    //            return "0";
    //    }

    //    public bool InsWBillMst_Rent(PropProperty oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsBillMst", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iPay_Code;
    //        objcmd.Parameters.Add("@msttype", SqlDbType.Int).Value = oProp.iPay_type;
    //        objcmd.Parameters.Add("@bmstdate", SqlDbType.DateTime).Value = oProp.iPay_Date;
    //        objcmd.Parameters.Add("@bmsty1", SqlDbType.Int).Value = oProp.iPay_Yr;
    //        objcmd.Parameters.Add("@bmstcust", SqlDbType.Int).Value = oProp.iPay_Cust;
    //        objcmd.Parameters.Add("@bmstsrno", SqlDbType.Int).Value = oProp.iPay_SrNo;
    //        objcmd.Parameters.Add("@bmstservice", SqlDbType.VarChar).Value = oProp.sPay_Service;
    //        objcmd.Parameters.Add("@bmstacno", SqlDbType.VarChar).Value = oProp.sPay_AcNo;
    //        objcmd.Parameters.Add("@bmstplot", SqlDbType.VarChar).Value = oProp.iBill_Plot;
    //        objcmd.Parameters.Add("@bmstwidth", SqlDbType.Float).Value = oProp.iBill_Width;
    //        objcmd.Parameters.Add("@bmstuse", SqlDbType.VarChar).Value = oProp.iBill_Use;
    //        objcmd.Parameters.Add("@bmstzone", SqlDbType.Int).Value = oProp.iBill_Zone;
    //        objcmd.Parameters.Add("@bmstward", SqlDbType.Int).Value = oProp.iBill_Ward;
    //        objcmd.Parameters.Add("@bmstcurr", SqlDbType.Money).Value = oProp.iBill_Curr;
    //        objcmd.Parameters.Add("@bmstdue", SqlDbType.Money).Value = oProp.iBill_Due;
    //        objcmd.Parameters.Add("@bmstpydt", SqlDbType.DateTime).Value = oProp.dPay_Pydt;
    //        objcmd.Parameters.Add("@bmstbknm", SqlDbType.VarChar).Value = oProp.iPay_Bknm;
    //        objcmd.Parameters.Add("@BMSTSLNO", SqlDbType.VarChar).Value = oProp.iPay_SlNo;
    //        objcmd.Parameters.Add("@BMSTEXTN", SqlDbType.VarChar).Value = oProp.sPay_Extn;
    //        objcmd.Parameters.Add("@billmst", SqlDbType.VarChar).Value = oProp.iPay_BillNo;
    //        objcmd.Parameters.Add("@bmstcollector", SqlDbType.VarChar).Value = oProp.sCollector;
    //        objcmd.Parameters.Add("@bmstpay", SqlDbType.Money).Value = oProp.iPay_Pay;
    //        //objcmd.Parameters.Add("@mstmain", SqlDbType.Money).Value = 
    //        //objcmd.Parameters.Add("@mstlate", SqlDbType.Money).Value = 
    //        //objcmd.Parameters.Add("@mstextc", SqlDbType.Money).Value = 
    //        //objcmd.Parameters.Add("@bmstmain", SqlDbType.Money).Value =
    //        //objcmd.Parameters.Add("@bmstlate", SqlDbType.Money).Value =
    //        //objcmd.Parameters.Add("@bmstextc", SqlDbType.Money).Value =
    //        objcmd.Parameters.Add("@bmstinco", SqlDbType.Int).Value = oProp.iPay_InCome;
    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }

    //    public bool InsSepuMst_Rent(PropProperty oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsSepuMst", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compCode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@msttype", SqlDbType.Int).Value = oProp.iPay_type;
    //        objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iPay_Code;
    //        objcmd.Parameters.Add("@mstdate", SqlDbType.DateTime).Value = oProp.iPay_Date;
    //        objcmd.Parameters.Add("@mstrema", SqlDbType.VarChar).Value = oProp.sRemark;
    //        objcmd.Parameters.Add("@msttota", SqlDbType.Money).Value = oProp.iPay_Total;
    //        objcmd.Parameters.Add("@mstchno", SqlDbType.VarChar).Value = oProp.iPay_sChNo;
    //        objcmd.Parameters.Add("@mstbldt", SqlDbType.DateTime).Value = oProp.dPay_bldt;
    //        objcmd.Parameters.Add("@MSTREFC", SqlDbType.VarChar).Value = oProp.sPay_Refc;
    //        objcmd.Parameters.Add("@mstptcode", SqlDbType.Int).Value = oProp.iPay_PtCode;
    //        objcmd.Parameters.Add("@mstdrcode", SqlDbType.Int).Value = oProp.iPay_DrCode;
    //        objcmd.Parameters.Add("@mstbala", SqlDbType.Money).Value = oProp.iPay_Balance;
    //        objcmd.Parameters.Add("@mstneta", SqlDbType.Money).Value = oProp.iPay_Net;
    //        objcmd.Parameters.Add("@mstcfno", SqlDbType.VarChar).Value = oProp.sPay_Cfno;
    //        objcmd.Parameters.Add("@mstclno", SqlDbType.VarChar).Value = oProp.iPay_ClNo;
    //        objcmd.Parameters.Add("@mstappr", SqlDbType.Int).Value = oProp.iPay_Appr;
    //        objcmd.Parameters.Add("@mstpaid", SqlDbType.Money).Value = oProp.iPay_Paid;
    //        objcmd.Parameters.Add("@mstgncd", SqlDbType.VarChar).Value = oProp.sPay_Gncd;
    //        objcmd.Parameters.Add("@mststat", SqlDbType.Int).Value = oProp.iPay_Stat;
    //        objcmd.Parameters.Add("@mstexti", SqlDbType.VarChar).Value = oProp.sPay_Texi;
    //        //objcmd.Parameters.Add("@mstcldt", SqlDbType.DateTime ).Value = oProp.dPay_Cldt ; 
    //        objcmd.Parameters.Add("@mstsite", SqlDbType.Int).Value = oProp.iPay_Site;
    //        objcmd.Parameters.Add("@mstbrnc", SqlDbType.Int).Value = oProp.iPay_Branch;
    //        objcmd.Parameters.Add("@mstcust", SqlDbType.Int).Value = oProp.iPay_Cust;
    //        objcmd.Parameters.Add("@mstincome", SqlDbType.Int).Value = oProp.iPay_InCome;
    //        objcmd.Parameters.Add("@mstcollector", SqlDbType.Int).Value = oProp.iPay_iCollector;
    //        objcmd.Parameters.Add("@mstbookid", SqlDbType.Int).Value = oProp.iPay_Bknm;
    //        objcmd.Parameters.Add("@mstslipno", SqlDbType.Int).Value = oProp.iPay_SlNo;
    //        objcmd.Parameters.Add("@mstextn", SqlDbType.VarChar).Value = oProp.sPay_Extn;
    //        objcmd.Parameters.Add("@mststatus", SqlDbType.Char).Value = "M";

    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }

    //    public bool InsSepuTrn_Rent(PropProperty oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsSepuTrn", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compCode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = oProp.iPay_type;
    //        objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = oProp.iPay_Code;
    //        objcmd.Parameters.Add("@trndate", SqlDbType.DateTime).Value = oProp.iPay_Date;
    //        objcmd.Parameters.Add("@trnsrno", SqlDbType.Int).Value = oProp.iPay_SrNo;

    //        objcmd.Parameters.Add("@trnitem", SqlDbType.Int).Value = oProp.iPay_Item;
    //        objcmd.Parameters.Add("@trncram", SqlDbType.Money).Value = oProp.iPay_CrAm;
    //        objcmd.Parameters.Add("@trndram", SqlDbType.Money).Value = oProp.iPay_DrAm;
    //        objcmd.Parameters.Add("@trnrema", SqlDbType.VarChar).Value = oProp.sRemark;
    //        objcmd.Parameters.Add("@trnledg", SqlDbType.Int).Value = oProp.iPay_Head;
    //        objcmd.Parameters.Add("@trnadju", SqlDbType.Money).Value = oProp.iPay_Adju;
    //        objcmd.Parameters.Add("@trnexpa", SqlDbType.Money).Value = oProp.iPay_Expa;
    //        objcmd.Parameters.Add("@trnexpr", SqlDbType.Int).Value = oProp.iPay_Expr;
    //        objcmd.Parameters.Add("@trntime", SqlDbType.Int).Value = oProp.iPay_Time;
    //        objcmd.Parameters.Add("@trnrefc", SqlDbType.Int).Value = oProp.sPay_Refc;
    //        objcmd.Parameters.Add("@trnchno", SqlDbType.VarChar).Value = oProp.iPay_sChNo;
    //        // objcmd.Parameters.Add("@trnchdt", SqlDbType.DateTime).Value = oProp.dPay_Cldt;
    //        objcmd.Parameters.Add("@trnaddv", SqlDbType.Money).Value = oProp.iPay_Addv;
    //        objcmd.Parameters.Add("@trnbank", SqlDbType.VarChar).Value = oProp.sPay_Bank;

    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    public bool InsAcctRelaMst(propAcctRelaMst prop)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsSepuTrn", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();
    //        objcmd.Parameters.Add("@COMPCODE", SqlDbType.Int).Value = prop.COMPCODE;
    //        objcmd.Parameters.Add("@MSTTYPE", SqlDbType.Int).Value = prop.MSTTYPE;
    //        objcmd.Parameters.Add("@DRCODE", SqlDbType.Int).Value = prop.DRCODE;
    //        objcmd.Parameters.Add("@DRALAIA", SqlDbType.Int).Value = prop.DRALAIA;
    //        objcmd.Parameters.Add("@CRCODE", SqlDbType.Int).Value = prop.CRCODE;
    //        objcmd.Parameters.Add("@CRALAIA", SqlDbType.Int).Value = prop.CRALAIA;
    //        objcmd.Parameters.Add("@renttype", SqlDbType.Int).Value = prop.renttype;
    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }

    //    public bool InsStudLed(PropRent prop)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsStudLed", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@chacct", SqlDbType.Int).Value = prop.iChacct;
    //        objcmd.Parameters.Add("@chtype", SqlDbType.Int).Value = prop.iMstType;
    //        objcmd.Parameters.Add("@chcomp", SqlDbType.Int).Value = prop.iMstCode;
    //        objcmd.Parameters.Add("@chledg", SqlDbType.Int).Value = prop.iLedg;
    //        objcmd.Parameters.Add("@tramount", SqlDbType.Money).Value = prop.dTramount;
    //        objcmd.Parameters.Add("@dramount", SqlDbType.Money).Value = prop.dDramount;
    //        objcmd.Parameters.Add("@cramount", SqlDbType.Money).Value = prop.dCramount;
    //        objcmd.Parameters.Add("@balance", SqlDbType.Money).Value = prop.dBalance;

    //        if (prop.dChdate.ToString() != "")
    //            objcmd.Parameters.Add("@chdate", SqlDbType.DateTime).Value = prop.dChdate;

    //        if (prop.dLastdate.ToString() != "")
    //            objcmd.Parameters.Add("@lastdate", SqlDbType.DateTime).Value = prop.dLastdate;
    //        if (prop.sChcode != null && prop.sChcode != "")
    //            objcmd.Parameters.Add("@chcode", SqlDbType.VarChar).Value = prop.sChcode.ToString();
    //        objcmd.Parameters.Add("@particular", SqlDbType.VarChar).Value = prop.sParticular;
    //        objcmd.Parameters.Add("@chrpcd", SqlDbType.VarChar).Value = prop.sChrpcd;
    //        objcmd.Parameters.Add("@chsrno", SqlDbType.VarChar).Value = prop.sChsrno;
    //        objcmd.Parameters.Add("@chagen", SqlDbType.VarChar).Value = prop.sChagen;
    //        objcmd.Parameters.Add("@status", SqlDbType.Int).Value = prop.iStatus;
    //        objcmd.Parameters.Add("@description", SqlDbType.VarChar).Value = prop.sDesc;
    //        objcmd.Parameters.Add("@name", SqlDbType.VarChar).Value = prop.sCustName;
    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }

    //    public bool InsStudled_Summ(PropRent prop, int mstIncome, string SqlType)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsStudled_Summ", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@SqlType", SqlDbType.Int).Value = Convert.ToInt16(SqlType);
    //        objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = prop.iCompCode;
    //        objcmd.Parameters.Add("@MstCode", SqlDbType.Int).Value = prop.iMstCode;
    //        objcmd.Parameters.Add("@OpDate", SqlDbType.DateTime).Value = prop.dOpDate;
    //        objcmd.Parameters.Add("@ClDate", SqlDbType.DateTime).Value = prop.dClDate;
    //        objcmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = prop.sBillNo;
    //        objcmd.Parameters.Add("@mstIncome", SqlDbType.Int).Value = mstIncome;
    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }

    //    public bool UpdStudLed(PropRent prop)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("UpdStudLed", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@dramount", SqlDbType.Money).Value = prop.dDramount;
    //        objcmd.Parameters.Add("@chrpcd", SqlDbType.VarChar).Value = prop.sChrpcd;
    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    //public bool InsBillMst(PropProperty oProp)
    //    //{
    //    //    GetConection();
    //    //    SqlCommand objcmd = new SqlCommand("InsBillMst", con);
    //    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    //    DataTable dt1 = new DataTable();
    //    //    SqlDataAdapter da = new SqlDataAdapter();

    //    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    //    objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iBill_Code;
    //    //    objcmd.Parameters.Add("@msttype", SqlDbType.Int).Value = oProp.iBill_type;
    //    //    objcmd.Parameters.Add("@bmstdate", SqlDbType.DateTime).Value = oProp.iBill_OpDate;
    //    //    objcmd.Parameters.Add("@bmstdued", SqlDbType.DateTime).Value = oProp.iBill_ClDate;
    //    //    objcmd.Parameters.Add("@bmsttype", SqlDbType.Int).Value = oProp.iBill_bmstType;
    //    //    objcmd.Parameters.Add("@bmstmn1", SqlDbType.Int).Value = oProp.iBill_OpMonth;
    //    //    objcmd.Parameters.Add("@bmsty1", SqlDbType.Int).Value = oProp.iBill_OpYear;
    //    //    objcmd.Parameters.Add("@bmstmn2", SqlDbType.Int).Value = oProp.iBill_ClMonth;
    //    //    objcmd.Parameters.Add("@bmsty2", SqlDbType.Int).Value = oProp.iBill_ClYear;
    //    //    objcmd.Parameters.Add("@bmstsrno", SqlDbType.Int).Value = oProp.iBill_SrNo1;
    //    //    objcmd.Parameters.Add("@bmstno", SqlDbType.VarChar).Value = oProp.sBill_Bill_No;
    //    //    objcmd.Parameters.Add("@bmstcust", SqlDbType.Int).Value = oProp.iBill_CustID;
    //    //    objcmd.Parameters.Add("@bmstfnm", SqlDbType.VarChar).Value = oProp.sBill_FName;
    //    //    objcmd.Parameters.Add("@bmstservice", SqlDbType.VarChar).Value = oProp.iBill_Service;
    //    //    objcmd.Parameters.Add("@bmstacno", SqlDbType.VarChar).Value = oProp.iBill_AcNo;
    //    //    objcmd.Parameters.Add("@bmstuse", SqlDbType.VarChar).Value = oProp.iBill_Use;
    //    //    objcmd.Parameters.Add("@bmstwidth", SqlDbType.Float).Value = oProp.iBill_Width;
    //    //    objcmd.Parameters.Add("@bmstzone", SqlDbType.Int).Value = oProp.iBill_Zone;
    //    //    objcmd.Parameters.Add("@bmstward", SqlDbType.Int).Value = oProp.iBill_Ward;
    //    //    objcmd.Parameters.Add("@bmstprev", SqlDbType.Money).Value = oProp.iBill_Prev;
    //    //    objcmd.Parameters.Add("@bmstmnch", SqlDbType.Money).Value = oProp.dBill_Mnch;
    //    //    objcmd.Parameters.Add("@bmstcurr", SqlDbType.Money).Value = oProp.iBill_Curr;
    //    //    objcmd.Parameters.Add("@bmstdue", SqlDbType.Money).Value = oProp.iBill_Due;
    //    //    objcmd.Parameters.Add("@bmstacct", SqlDbType.Int).Value = oProp.iBill_Acct;
    //    //    objcmd.Parameters.Add("@bmstopbl", SqlDbType.Money).Value = oProp.iBill_OpBal;
    //    //    objcmd.Parameters.Add("@bmstarea", SqlDbType.Money).Value = oProp.iBill_Area;
    //    //    objcmd.Parameters.Add("@bmstarate", SqlDbType.Money).Value = oProp.iBill_Rate;
    //    //    objcmd.Parameters.Add("@bmsttaxp", SqlDbType.Money).Value = oProp.iBill_Taxp;
    //    //    objcmd.Parameters.Add("@billmst", SqlDbType.Int).Value = oProp.iBill_BillMst;

    //    //    objcmd.Parameters.Add("@bmstChMd", SqlDbType.Money).Value = oProp.dBill_ChMd;
    //    //    objcmd.Parameters.Add("@bmstAfDt", SqlDbType.Money).Value = oProp.dBill_AfDt;
    //    //    objcmd.Parameters.Add("@bmstplot", SqlDbType.VarChar).Value = oProp.iBill_Plot;

    //    //    objcmd.ExecuteNonQuery();
    //    //    return true;
    //    //}
    //    public void FillDropDownList(DropDownList ddlName, string TableName, string TextField, string ValueField, string Condition = "", string join = "", string tooltip = "", string order = "", bool addDistinct = true, bool addSelect = true)
    //    {
    //        ddlName.Items.Clear();
    //        DataTable dt;
    //        string newname = "";
    //        if (TextField == ValueField)
    //            newname = "" + TextField + "";
    //        else
    //            newname = TextField;

    //        if (Condition != "")
    //            Condition = " Where " + Condition;

    //        if (order == "")
    //            order = "text,value";

    //        string dist = "";
    //        if (addDistinct)
    //            dist = " distinct ";

    //        ddlName.DataSource = dt = GetData("select " + dist + ValueField + " as value,cast(" + newname + " as nvarchar(max)) as text from " + TableName + " " + join + " " + Condition + " order by " + order);
    //        ddlName.DataTextField = "text";
    //        ddlName.DataValueField = "value";
    //        ddlName.DataBind();
    //        if (addSelect)
    //            ddlName.Items.Insert(0, new ListItem("--Select--", "0"));
    //    }

    //    public bool InsTmpPaymt(PropRent oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsTmppaymt", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@msttype", SqlDbType.Int).Value = oProp.iMstType;
    //        objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iMstCode;
    //        objcmd.Parameters.Add("@custno", SqlDbType.Int).Value = oProp.iCustCode;
    //        objcmd.Parameters.Add("@paymode", SqlDbType.Int).Value = oProp.iPayMode;
    //        objcmd.Parameters.Add("@year1", SqlDbType.Int).Value = oProp.iYear1;
    //        objcmd.Parameters.Add("@year2", SqlDbType.Int).Value = oProp.iYear1;
    //        objcmd.Parameters.Add("@r1", SqlDbType.Int).Value = oProp.sAcctAlia;
    //        //objcmd.Parameters.Add("@r2", SqlDbType.Int).Value = oProp.;
    //        //objcmd.Parameters.Add("@r3", SqlDbType.Int).Value = oProp.;
    //        //objcmd.Parameters.Add("@r4", SqlDbType.Int).Value = oProp.;
    //        //objcmd.Parameters.Add("@r5", SqlDbType.Int).Value = oProp.;
    //        //objcmd.Parameters.Add("@r6", SqlDbType.Int).Value = oProp.;
    //        //objcmd.Parameters.Add("@r7", SqlDbType.Int).Value = oProp.;
    //        //objcmd.Parameters.Add("@r8", SqlDbType.Int).Value = oProp.;
    //        //objcmd.Parameters.Add("@r9", SqlDbType.Int).Value = oProp.; 

    //        objcmd.Parameters.Add("@name", SqlDbType.VarChar).Value = oProp.sCustName;
    //        objcmd.Parameters.Add("@address", SqlDbType.VarChar).Value = oProp.sAddress;
    //        objcmd.Parameters.Add("@ward", SqlDbType.VarChar).Value = oProp.sWard;
    //        objcmd.Parameters.Add("@pzone", SqlDbType.VarChar).Value = oProp.sZone;
    //        objcmd.Parameters.Add("@serviceno", SqlDbType.VarChar).Value = oProp.sServiceNo;
    //        objcmd.Parameters.Add("@billno", SqlDbType.VarChar).Value = oProp.sBillNo;
    //        objcmd.Parameters.Add("@bank", SqlDbType.VarChar).Value = oProp.sBank;
    //        //objcmd.Parameters.Add("@chkno", SqlDbType.VarChar).Value = oProp.;
    //        objcmd.Parameters.Add("@mn1", SqlDbType.VarChar).Value = oProp.sMn1;
    //        objcmd.Parameters.Add("@mn2", SqlDbType.VarChar).Value = oProp.sMn2;
    //        objcmd.Parameters.Add("@bookno", SqlDbType.VarChar).Value = oProp.sBookNo;
    //        objcmd.Parameters.Add("@slipono", SqlDbType.VarChar).Value = oProp.sSlipNo;
    //        objcmd.Parameters.Add("@CNamHind", SqlDbType.VarChar).Value = oProp.sCustHindi;
    //        objcmd.Parameters.Add("@remark", SqlDbType.VarChar).Value = oProp.sRemark;
    //        objcmd.Parameters.Add("@mn1Hind", SqlDbType.VarChar).Value = oProp.sMn1Hindi;
    //        objcmd.Parameters.Add("@mn2hind", SqlDbType.VarChar).Value = oProp.sMn2Hindi;
    //        objcmd.Parameters.Add("@wardHind", SqlDbType.VarChar).Value = oProp.sWardH;
    //        objcmd.Parameters.Add("@pzonehind", SqlDbType.VarChar).Value = oProp.sZoneH;
    //        if (oProp.sAmt1Hindi != null && oProp.sAmt1Hindi != "")
    //            objcmd.Parameters.Add("@amt1Hind", SqlDbType.VarChar).Value = oProp.sAmt1Hindi;

    //        objcmd.Parameters.Add("@pdate", SqlDbType.DateTime).Value = oProp.dDate;
    //        objcmd.Parameters.Add("@billdate", SqlDbType.DateTime).Value = oProp.dDate;

    //        if (oProp.dDueDt.ToString() != "")
    //            objcmd.Parameters.Add("@duedate", SqlDbType.DateTime).Value = oProp.dDueDt;

    //        objcmd.Parameters.Add("@amtpay", SqlDbType.Money).Value = oProp.dAmtPay;
    //        objcmd.Parameters.Add("@amtafter", SqlDbType.Money).Value = oProp.dAmtAfter;
    //        //objcmd.Parameters.Add("@ra1", SqlDbType.Money).Value = oProp.;
    //        //objcmd.Parameters.Add("@ra2", SqlDbType.Money).Value = oProp.;
    //        //objcmd.Parameters.Add("@ra3", SqlDbType.Money).Value = oProp.;
    //        //objcmd.Parameters.Add("@ra4", SqlDbType.Money).Value = oProp.;
    //        //objcmd.Parameters.Add("@ra5", SqlDbType.Money).Value = oProp.;
    //        //objcmd.Parameters.Add("@ra6", SqlDbType.Money).Value = oProp.;
    //        //objcmd.Parameters.Add("@ra7", SqlDbType.Money).Value = oProp.;
    //        //objcmd.Parameters.Add("@ra8", SqlDbType.Money).Value = oProp.;
    //        //objcmd.Parameters.Add("@ra9", SqlDbType.Money).Value = oProp.;
    //        objcmd.Parameters.Add("@total", SqlDbType.Money).Value = oProp.dTotal;

    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }

    //    public bool InsUserWork(PropRent oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsUserWork", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@woruser", SqlDbType.Int).Value = oProp.iUseCode;
    //        objcmd.Parameters.Add("@wormode", SqlDbType.Int).Value = oProp.iMode;
    //        objcmd.Parameters.Add("@worcomp", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@wortype", SqlDbType.Int).Value = oProp.iRentType;
    //        objcmd.Parameters.Add("@worcode", SqlDbType.Int).Value = oProp.iMstCode;
    //        objcmd.Parameters.Add("@worsrno", SqlDbType.VarChar).Value = oProp.sServiceNo;
    //        objcmd.Parameters.Add("@worrema", SqlDbType.VarChar).Value = oProp.sRemark;
    //        objcmd.Parameters.Add("@worrfsr", SqlDbType.VarChar).Value = "0";
    //        objcmd.Parameters.Add("@worsysn", SqlDbType.VarChar).Value = oProp.sSysName;
    //        objcmd.Parameters.Add("@wordate", SqlDbType.DateTime).Value = oProp.dDate;
    //        objcmd.Parameters.Add("@worcudt", SqlDbType.DateTime).Value = DateTime.Now;


    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    public bool DelTmpPaymt()
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("DelTmpPaymt", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }

    //    public bool InsCertificate(PropCertificate oProp, byte[] G_img, byte[] B_img)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsCertificate", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@REGNO", SqlDbType.VarChar).Value = oProp.sREGNO;
    //        objcmd.Parameters.Add("@CCODE", SqlDbType.Int).Value = oProp.iCCODE;
    //        objcmd.Parameters.Add("@ZONEID", SqlDbType.Int).Value = oProp.iZONEID;
    //        objcmd.Parameters.Add("@WARDID", SqlDbType.Int).Value = oProp.iWARDID;
    //        objcmd.Parameters.Add("@COLONYID", SqlDbType.Int).Value = oProp.iCOLONYID;
    //        objcmd.Parameters.Add("@CITYCD", SqlDbType.Int).Value = oProp.iCITYCD;
    //        objcmd.Parameters.Add("@DOCODE", SqlDbType.Int).Value = oProp.iDOCODE;
    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iMstCode;
    //        objcmd.Parameters.Add("@receiptcd", SqlDbType.Int).Value = oProp.ireceiptcd;
    //        objcmd.Parameters.Add("@Ccast", SqlDbType.Int).Value = oProp.iCcast;
    //        objcmd.Parameters.Add("@category", SqlDbType.Int).Value = oProp.icategory;
    //        objcmd.Parameters.Add("@distance", SqlDbType.Int).Value = oProp.idistance;
    //        objcmd.Parameters.Add("@Brideresident", SqlDbType.Int).Value = oProp.iBrideresident;
    //        objcmd.Parameters.Add("@Bridedistrict", SqlDbType.Int).Value = oProp.iBridedistrict;
    //        objcmd.Parameters.Add("@Groomresident", SqlDbType.Int).Value = oProp.iGroomresident;
    //        objcmd.Parameters.Add("@Groomdistrict", SqlDbType.Int).Value = oProp.iGroomdistrict;
    //        objcmd.Parameters.Add("@GroomAge", SqlDbType.Int).Value = oProp.iGroomAge;
    //        objcmd.Parameters.Add("@Brideage", SqlDbType.Int).Value = oProp.iBrideage;

    //        objcmd.Parameters.Add("@REGDATE", SqlDbType.DateTime).Value = oProp.dREGDATE;
    //        objcmd.Parameters.Add("@CCDATE", SqlDbType.DateTime).Value = oProp.dCCDATE;
    //        objcmd.Parameters.Add("@CCTIME", SqlDbType.DateTime).Value = oProp.dCCTIME;
    //        objcmd.Parameters.Add("@Marriagedate", SqlDbType.DateTime).Value = oProp.dMarriagedate;

    //        objcmd.Parameters.Add("@CCCNAME", SqlDbType.NChar).Value = oProp.sCCCNAME;
    //        objcmd.Parameters.Add("@CCCRELA", SqlDbType.NChar).Value = oProp.sCCCRELA;

    //        objcmd.Parameters.Add("@AAPNAME", SqlDbType.NVarChar).Value = oProp.sAAPNAME;
    //        objcmd.Parameters.Add("@ADDRE1", SqlDbType.NVarChar).Value = oProp.sADDRE1;
    //        objcmd.Parameters.Add("@ADDRE2", SqlDbType.NVarChar).Value = oProp.sADDRE2;
    //        objcmd.Parameters.Add("@FATHERNM", SqlDbType.NVarChar).Value = oProp.sFATHERNM;
    //        objcmd.Parameters.Add("@MOTHERNM", SqlDbType.NVarChar).Value = oProp.sMOTHERNM;
    //        objcmd.Parameters.Add("@Groomname", SqlDbType.NVarChar).Value = oProp.sGroomname;
    //        objcmd.Parameters.Add("@Groomfather", SqlDbType.NVarChar).Value = oProp.sGroomfather;
    //        objcmd.Parameters.Add("@Bridename", SqlDbType.NVarChar).Value = oProp.sBridename;
    //        objcmd.Parameters.Add("@Bridefather", SqlDbType.NVarChar).Value = oProp.sBridefather;
    //        objcmd.Parameters.Add("@MarriagePlace", SqlDbType.NVarChar).Value = oProp.sMarriagePlace;
    //        objcmd.Parameters.Add("@CCPLACE", SqlDbType.NVarChar).Value = oProp.sCCPLACE;
    //        objcmd.Parameters.Add("@REMARK", SqlDbType.NVarChar).Value = oProp.sREMARK;
    //        objcmd.Parameters.Add("@ISSUNAME", SqlDbType.NVarChar).Value = oProp.sISSUNAME;

    //        objcmd.Parameters.Add("@Jaavak", SqlDbType.VarChar).Value = oProp.sJaavak;

    //        objcmd.Parameters.Add("@CONTACT1", SqlDbType.VarChar).Value = oProp.sCONTACT1;
    //        objcmd.Parameters.Add("@CONTACT2", SqlDbType.VarChar).Value = oProp.sCONTACT2;
    //        objcmd.Parameters.Add("@MOBILENO", SqlDbType.VarChar).Value = oProp.sMOBILENO;
    //        objcmd.Parameters.Add("@FAXNO", SqlDbType.VarChar).Value = oProp.sFAXNO;
    //        objcmd.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = oProp.sEMAIL;
    //        objcmd.Parameters.Add("@GENDER", SqlDbType.VarChar).Value = oProp.sGENDER;
    //        objcmd.Parameters.Add("@receiptno", SqlDbType.VarChar).Value = oProp.sreceiptno;
    //        objcmd.Parameters.Add("@GroomImagepath", SqlDbType.VarChar).Value = oProp.sGroomImagepath;
    //        objcmd.Parameters.Add("@BrideImagepath", SqlDbType.VarChar).Value = oProp.sBrideImagepath;

    //        objcmd.Parameters.Add("@G_Img", SqlDbType.Image, 0).Value = G_img;
    //        objcmd.Parameters.Add("@B_Img", SqlDbType.Image, 0).Value = B_img;

    //        objcmd.Parameters.Add("@Panjiyan", SqlDbType.VarChar).Value = oProp.sPanjiyan;

    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }


    //    public bool insTmp_Certificate(PropCertificate oProp, byte[] G_img, byte[] B_img)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("insTmp_Certificate", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iMstCode;
    //        objcmd.Parameters.Add("@CompName", SqlDbType.NVarChar).Value = oProp.sCompName;
    //        objcmd.Parameters.Add("@NAME", SqlDbType.NVarChar).Value = oProp.sCCCNAME;
    //        objcmd.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = oProp.sGENDER;
    //        objcmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = oProp.sCityName;
    //        objcmd.Parameters.Add("@Place", SqlDbType.NVarChar).Value = oProp.sCCPLACE;
    //        objcmd.Parameters.Add("@FName", SqlDbType.NVarChar).Value = oProp.sFATHERNM;
    //        objcmd.Parameters.Add("@AppName", SqlDbType.NVarChar).Value = oProp.sAAPNAME;
    //        objcmd.Parameters.Add("@AppAddress", SqlDbType.NVarChar).Value = oProp.sADDRE1;
    //        objcmd.Parameters.Add("@IssueDate", SqlDbType.DateTime).Value = oProp.dIssueDate;
    //        objcmd.Parameters.Add("@B_Date", SqlDbType.DateTime).Value = oProp.dCCDATE;
    //        objcmd.Parameters.Add("@mstDate", SqlDbType.DateTime).Value = oProp.dREGDATE;
    //        objcmd.Parameters.Add("@M_Name", SqlDbType.NVarChar).Value = oProp.sMOTHERNM;

    //        objcmd.Parameters.Add("@Jaavak", SqlDbType.VarChar).Value = oProp.sJaavak;

    //        objcmd.Parameters.Add("@Groom", SqlDbType.NVarChar).Value = oProp.sGroomname;
    //        objcmd.Parameters.Add("@G_FName", SqlDbType.NVarChar).Value = oProp.sGroomfather;
    //        objcmd.Parameters.Add("@G_Resi", SqlDbType.NVarChar).Value = oProp.sGroomresident;
    //        objcmd.Parameters.Add("@G_Dist", SqlDbType.NVarChar).Value = oProp.sGroomdistrict;
    //        objcmd.Parameters.Add("@G_Path", SqlDbType.NVarChar).Value = oProp.sGroomImagepath;
    //        objcmd.Parameters.Add("@Bride", SqlDbType.NVarChar).Value = oProp.sBridename;
    //        objcmd.Parameters.Add("@B_FName", SqlDbType.NVarChar).Value = oProp.sBridefather;
    //        objcmd.Parameters.Add("@B_Resi", SqlDbType.NVarChar).Value = oProp.sBrideresident;
    //        objcmd.Parameters.Add("@B_Dist", SqlDbType.NVarChar).Value = oProp.sBridedistrict;
    //        objcmd.Parameters.Add("@B_Path", SqlDbType.NVarChar).Value = oProp.sBrideImagepath;
    //        objcmd.Parameters.Add("@G_Age", SqlDbType.Int).Value = oProp.iGroomAge;
    //        objcmd.Parameters.Add("@B_Age", SqlDbType.Int).Value = oProp.iBrideage;

    //        objcmd.Parameters.Add("@B_Img", SqlDbType.Image).Value = B_img;
    //        objcmd.Parameters.Add("@G_Img", SqlDbType.Image).Value = G_img;

    //        objcmd.Parameters.Add("@RegNo", SqlDbType.NVarChar).Value = oProp.sREGNO;
    //        objcmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = oProp.sADDRE2;

    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    public bool InsStudDet(PropRent oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsStudDet", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@studType", SqlDbType.Int).Value = oProp.iMstType;
    //        objcmd.Parameters.Add("@studCode", SqlDbType.Int).Value = oProp.iMstCode;
    //        objcmd.Parameters.Add("@studcity", SqlDbType.Int).Value = oProp.iZone;
    //        objcmd.Parameters.Add("@studstat", SqlDbType.Int).Value = oProp.iWard;
    //        //objcmd.Parameters.Add("@studacctId", SqlDbType.Int).Value = oProp.iMstCode;
    //        objcmd.Parameters.Add("@studcomcode", SqlDbType.Int).Value = oProp.iCompCode;

    //        objcmd.Parameters.Add("@studName", SqlDbType.VarChar).Value = oProp.sCustName;
    //        objcmd.Parameters.Add("@studadd1", SqlDbType.VarChar).Value = oProp.sAddress;
    //        objcmd.Parameters.Add("@studadd2", SqlDbType.VarChar).Value = oProp.sAddress;
    //        objcmd.Parameters.Add("@studphon", SqlDbType.VarChar).Value = oProp.sPhone;
    //        objcmd.Parameters.Add("@studfnm", SqlDbType.VarChar).Value = oProp.sFName;
    //        //objcmd.Parameters.Add("@studmnm", SqlDbType.VarChar).Value = oProp.sMOTHERNM;
    //        //objcmd.Parameters.Add("@studplace", SqlDbType.VarChar).Value = oProp.sMOTHERNM;
    //        objcmd.Parameters.Add("@studhind", SqlDbType.NVarChar).Value = oProp.sCustHindi;
    //        objcmd.Parameters.Add("@WebHindi", SqlDbType.NVarChar).Value = oProp.sFatherHindi;

    //        objcmd.Parameters.Add("@studdob", SqlDbType.DateTime).Value = oProp.dDate;

    //        objcmd.Parameters.Add("@PAN", SqlDbType.VarChar).Value = oProp.sPAN;
    //        objcmd.Parameters.Add("@TIN", SqlDbType.VarChar).Value = oProp.sTIN;
    //        objcmd.Parameters.Add("@CST", SqlDbType.VarChar).Value = oProp.sCST;
    //        objcmd.Parameters.Add("@TAN", SqlDbType.VarChar).Value = oProp.sTAN;
    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    public bool InsRashIssue(PropRent oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsRashIssue", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@code", SqlDbType.Int).Value = oProp.iIssue_code;
    //        objcmd.Parameters.Add("@ReceiptID", SqlDbType.Int).Value = oProp.iIssue_ReceiptID;
    //        objcmd.Parameters.Add("@IsUse", SqlDbType.Int).Value = oProp.iIssue_IsUse;
    //        objcmd.Parameters.Add("@CustID", SqlDbType.Int).Value = oProp.iIssue_CustID;
    //        objcmd.Parameters.Add("@FormType", SqlDbType.Int).Value = oProp.iIssue_FormType;

    //        objcmd.Parameters.Add("@FormNo", SqlDbType.VarChar).Value = oProp.sIssue_FormNo;
    //        objcmd.Parameters.Add("@Receipt", SqlDbType.NVarChar).Value = oProp.sIssue_Receipt;
    //        objcmd.Parameters.Add("@IsEdit", SqlDbType.Bit).Value = oProp.iIssue_IsEdit;


    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    public bool InsTmpReceipt(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsTmpReceipt", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@mstincome", SqlDbType.Int).Value = oProp.iRec_mstincome;
    //        objcmd.Parameters.Add("@custno", SqlDbType.Int).Value = oProp.iRec_custno;
    //        objcmd.Parameters.Add("@aactcode", SqlDbType.Int).Value = oProp.iRec_aactcode;
    //        objcmd.Parameters.Add("@custtype", SqlDbType.Int).Value = oProp.iMstType;

    //        objcmd.Parameters.Add("@mstchno", SqlDbType.VarChar).Value = oProp.sRec_mstchno;
    //        objcmd.Parameters.Add("@custname", SqlDbType.NVarChar).Value = oProp.sRec_custname;
    //        objcmd.Parameters.Add("@acctname", SqlDbType.VarChar).Value = oProp.sRec_acctname;
    //        objcmd.Parameters.Add("@acctalia", SqlDbType.VarChar).Value = oProp.sRec_acctalia;
    //        objcmd.Parameters.Add("@Ename", SqlDbType.VarChar).Value = oProp.sRec_Ename;
    //        objcmd.Parameters.Add("@incomenm", SqlDbType.VarChar).Value = oProp.sRec_incomenm;
    //        objcmd.Parameters.Add("@incDesc", SqlDbType.VarChar).Value = oProp.sRec_incDesc;
    //        objcmd.Parameters.Add("@remark", SqlDbType.VarChar).Value = oProp.sRec_remark;
    //        objcmd.Parameters.Add("@CompName", SqlDbType.NVarChar).Value = oProp.sCompName;

    //        objcmd.Parameters.Add("@amount", SqlDbType.Money).Value = oProp.dRec_amount;

    //        objcmd.Parameters.Add("@mstdate", SqlDbType.DateTime).Value = oProp.dRec_mstdate;

    //        objcmd.Parameters.Add("@UseCode", SqlDbType.Int).Value = oProp.i_UseCode;
    //        objcmd.Parameters.Add("@InWord", SqlDbType.VarChar).Value = oProp.s_InWord;
    //        objcmd.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = oProp.s_FinYear;
    //        objcmd.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = oProp.d_CreatedOn;
    //        objcmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = oProp.sAddress;

    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    //InsStudDet 

    //    public bool InsRashTrn(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsRashTrn", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = oProp.iRashan_trncode;
    //        objcmd.Parameters.Add("@srno", SqlDbType.Int).Value = oProp.iRashan_srno;
    //        objcmd.Parameters.Add("@age", SqlDbType.Int).Value = oProp.iRashan_age;
    //        objcmd.Parameters.Add("@gen", SqlDbType.Int).Value = oProp.iRashan_gen;

    //        objcmd.Parameters.Add("@ReName", SqlDbType.NVarChar).Value = oProp.sRashan_ReName;
    //        objcmd.Parameters.Add("@Relation", SqlDbType.NVarChar).Value = oProp.sRashan_Relation;
    //        objcmd.Parameters.Add("@rashservice", SqlDbType.NVarChar).Value = oProp.sRashan_rashservice;
    //        objcmd.Parameters.Add("@relationname", SqlDbType.NVarChar).Value = oProp.sRashan_relationname;
    //        objcmd.Parameters.Add("@genname", SqlDbType.NVarChar).Value = oProp.sRashan_genname;
    //        objcmd.Parameters.Add("@rema", SqlDbType.NVarChar).Value = oProp.sRashan_rema;

    //        objcmd.Parameters.Add("@trndate", SqlDbType.DateTime).Value = oProp.dRashan_trndate;
    //        objcmd.Parameters.Add("@birthdate", SqlDbType.DateTime).Value = oProp.dRashan_birthdate;

    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    public bool InsRashMst(PropCertificate oProp, byte[] img, bool isEdit)
    //    {
    //        GetConection();

    //        SqlCommand objcmd;

    //        if (isEdit == true)
    //            objcmd = new SqlCommand("UpdRashMst", con);
    //        else
    //            objcmd = new SqlCommand("InsRashMst", con);

    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@Rashcode", SqlDbType.Int).Value = oProp.iRashan_Rashcode;
    //        objcmd.Parameters.Add("@Rashcity", SqlDbType.Int).Value = oProp.iRashan_Rashcity;
    //        objcmd.Parameters.Add("@Rashcolony", SqlDbType.Int).Value = oProp.iRashan_Rashcolony;
    //        objcmd.Parameters.Add("@Rashzone", SqlDbType.Int).Value = oProp.iRashan_Rashzone;
    //        objcmd.Parameters.Add("@Rashward", SqlDbType.Int).Value = oProp.iRashan_Rashward;
    //        objcmd.Parameters.Add("@Rashtype", SqlDbType.Int).Value = oProp.iRashan_Rashtype;
    //        objcmd.Parameters.Add("@rashage", SqlDbType.Int).Value = oProp.iRashan_rashage;
    //        objcmd.Parameters.Add("@rashunit", SqlDbType.Int).Value = oProp.iRashan_rashunit;
    //        objcmd.Parameters.Add("@male", SqlDbType.Int).Value = oProp.iRashan_male;
    //        objcmd.Parameters.Add("@female", SqlDbType.Int).Value = oProp.iRashan_female;
    //        objcmd.Parameters.Add("@rashcast", SqlDbType.Int).Value = oProp.iRashan_rashcast;

    //        objcmd.Parameters.Add("@PrintCount", SqlDbType.Int).Value = oProp.iRashan_PrintCount;

    //        objcmd.Parameters.Add("@Rashdate", SqlDbType.DateTime).Value = oProp.dRashan_Rashdate;
    //        objcmd.Parameters.Add("@rashdob", SqlDbType.DateTime).Value = oProp.dRashan_rashdob;
    //        objcmd.Parameters.Add("@leavedate", SqlDbType.DateTime).Value = oProp.dRashan_leavedate;
    //        objcmd.Parameters.Add("@surveydate", SqlDbType.DateTime).Value = oProp.dRashan_surveydate;

    //        objcmd.Parameters.Add("@Rashonm", SqlDbType.NVarChar).Value = oProp.sRashan_Rashonm;
    //        objcmd.Parameters.Add("@Rashfnm", SqlDbType.NVarChar).Value = oProp.sRashan_Rashfnm;
    //        objcmd.Parameters.Add("@Rashplotno", SqlDbType.NVarChar).Value = oProp.sRashan_Rashplotno;
    //        objcmd.Parameters.Add("@Rashservice", SqlDbType.NVarChar).Value = oProp.sRashan_Rashservice;
    //        objcmd.Parameters.Add("@Rashacno", SqlDbType.NVarChar).Value = oProp.sRashan_Rashacno;
    //        objcmd.Parameters.Add("@Rashstatus", SqlDbType.NVarChar).Value = oProp.sRashan_Rashstatus;
    //        objcmd.Parameters.Add("@Rashremark", SqlDbType.NVarChar).Value = oProp.sRashan_Rashremark;
    //        objcmd.Parameters.Add("@Rashphone", SqlDbType.NVarChar).Value = oProp.sRashan_Rashphone;
    //        objcmd.Parameters.Add("@Rashmobile", SqlDbType.NVarChar).Value = oProp.sRashan_Rashmobile;
    //        objcmd.Parameters.Add("@appliphoto", SqlDbType.NVarChar).Value = oProp.sRashan_appliphoto;
    //        objcmd.Parameters.Add("@shopno", SqlDbType.NVarChar).Value = oProp.sRashan_shopno;
    //        objcmd.Parameters.Add("@shopdesc", SqlDbType.NVarChar).Value = oProp.sRashan_shopdesc;
    //        objcmd.Parameters.Add("@shopowner", SqlDbType.NVarChar).Value = oProp.sRashan_shopowner;
    //        objcmd.Parameters.Add("@issuedby", SqlDbType.NVarChar).Value = oProp.sRashan_issuedby;
    //        objcmd.Parameters.Add("@rashchno", SqlDbType.NVarChar).Value = oProp.sRashan_rashchno;
    //        objcmd.Parameters.Add("@wardnm", SqlDbType.NVarChar).Value = oProp.sRashan_wardnm;
    //        objcmd.Parameters.Add("@zonwnm", SqlDbType.NVarChar).Value = oProp.sRashan_zonwnm;

    //        objcmd.Parameters.Add("@colonynm", SqlDbType.NVarChar).Value = oProp.sRashan_colonynm;
    //        objcmd.Parameters.Add("@citynm", SqlDbType.NVarChar).Value = oProp.sRashan_citynm;
    //        objcmd.Parameters.Add("@rashPhone1", SqlDbType.NVarChar).Value = oProp.sRashan_rashPhone1;
    //        objcmd.Parameters.Add("@rashPadd", SqlDbType.NVarChar).Value = oProp.sRashan_rashPadd;
    //        objcmd.Parameters.Add("@rashradd", SqlDbType.NVarChar).Value = oProp.sRashan_rashradd;
    //        objcmd.Parameters.Add("@rashprofession", SqlDbType.NVarChar).Value = oProp.sRashan_rashprofession;
    //        objcmd.Parameters.Add("@rashPost", SqlDbType.NVarChar).Value = oProp.sRashan_rashPost;
    //        objcmd.Parameters.Add("@rashfax", SqlDbType.NVarChar).Value = oProp.sRashan_rashfax;
    //        objcmd.Parameters.Add("@rashemail", SqlDbType.NVarChar).Value = oProp.sRashan_rashemail;
    //        objcmd.Parameters.Add("@rashworking", SqlDbType.NVarChar).Value = oProp.sRashan_rashworking;
    //        objcmd.Parameters.Add("@rashconsumer", SqlDbType.NVarChar).Value = oProp.sRashan_rashconsumer;
    //        objcmd.Parameters.Add("@rashcompany", SqlDbType.NVarChar).Value = oProp.sRashan_rashcompany;
    //        objcmd.Parameters.Add("@rashconnetion", SqlDbType.NVarChar).Value = oProp.sRashan_rashconnetion;
    //        objcmd.Parameters.Add("@surveyno", SqlDbType.NVarChar).Value = oProp.sRashan_surveyno;
    //        objcmd.Parameters.Add("@DETAILENT", SqlDbType.NVarChar).Value = oProp.sRashan_DETAILENT;

    //        objcmd.Parameters.Add("@monincom", SqlDbType.Money).Value = oProp.dRashan_monincom;
    //        objcmd.Parameters.Add("@Img", SqlDbType.Image, 0).Value = img;

    //        objcmd.Parameters.Add("@Gas_Con_Holder", SqlDbType.NVarChar).Value = oProp.sRashan_Gas_Con_Holder;
    //        objcmd.Parameters.Add("@Gas_Company", SqlDbType.NVarChar).Value = oProp.sRashan_Gas_Company;
    //        objcmd.Parameters.Add("@Gas_Agency", SqlDbType.NVarChar).Value = oProp.sRashan_Gas_Agency;
    //        objcmd.Parameters.Add("@Gas_Con_No", SqlDbType.NVarChar).Value = oProp.sRashan_Gas_Con_No;

    //        if (isEdit == false)
    //        {
    //            objcmd.Parameters.Add("@DocsList", SqlDbType.VarChar).Value = oProp.sRashan_DocsList;
    //            objcmd.Parameters.Add("@APL_Code", SqlDbType.Char).Value = oProp.sRashan_APL_Code;
    //            objcmd.Parameters.Add("@Form_No", SqlDbType.Char).Value = oProp.sRashan_Form_No;
    //        }
    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    //public bool InsTmpRation(PropCertificate oProp, byte[] img)
    //    //{
    //    //    GetConection();
    //    //    SqlCommand objcmd = new SqlCommand("InsTmpRation", con);
    //    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    //    DataTable dt1 = new DataTable();
    //    //    SqlDataAdapter da = new SqlDataAdapter();

    //    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;

    //    //    if (oProp.iRashan_age > 0)
    //    //        objcmd.Parameters.Add("@Rage", SqlDbType.Int).Value = oProp.iRashan_age;

    //    //    //objcmd.Parameters.Add("@age", SqlDbType.Int).Value = oProp.iRashan_rashage;
    //    //    objcmd.Parameters.Add("@Rage", SqlDbType.Int).Value = oProp.iRashan_age;
    //    //    //objcmd.Parameters.Add("@Rmale", SqlDbType.Int).Value = oProp.iRashan_male; 
    //    //    //objcmd.Parameters.Add("@Rfemale", SqlDbType.Int).Value = oProp.iRashan_female; 
    //    //    objcmd.Parameters.Add("@Rgen", SqlDbType.Int).Value = oProp.iRashan_gen;
    //    //    objcmd.Parameters.Add("@male", SqlDbType.Int).Value = oProp.iRashan_male;
    //    //    objcmd.Parameters.Add("@female", SqlDbType.Int).Value = oProp.iRashan_female;
    //    //    objcmd.Parameters.Add("@srno", SqlDbType.Int).Value = oProp.iRashan_srno;
    //    //    objcmd.Parameters.Add("@num", SqlDbType.Int).Value = 1;

    //    //    objcmd.Parameters.Add("@chcode", SqlDbType.NVarChar).Value = oProp.iRashan_trncode;
    //    //    objcmd.Parameters.Add("@chno", SqlDbType.NVarChar).Value = oProp.sRashan_rashchno;
    //    //    objcmd.Parameters.Add("@ownernm", SqlDbType.NVarChar).Value = oProp.sRashan_Rashonm;
    //    //    objcmd.Parameters.Add("@fathernm", SqlDbType.NVarChar).Value = oProp.sRashan_Rashfnm;
    //    //    objcmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = oProp.sRashan_citynm;
    //    //    objcmd.Parameters.Add("@colny", SqlDbType.NVarChar).Value = oProp.sRashan_colonynm;
    //    //    objcmd.Parameters.Add("@rationcardno", SqlDbType.NVarChar).Value = oProp.sRashan_rashservice;
    //    //    objcmd.Parameters.Add("@zone", SqlDbType.NVarChar).Value = oProp.sRashan_zonwnm;
    //    //    objcmd.Parameters.Add("@ward", SqlDbType.NVarChar).Value = oProp.sRashan_wardnm;
    //    //    objcmd.Parameters.Add("@remark", SqlDbType.NVarChar).Value = oProp.sRashan_rema;
    //    //    objcmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = oProp.sRashan_Rashphone;
    //    //    objcmd.Parameters.Add("@mobile", SqlDbType.NVarChar).Value = oProp.sRashan_Rashmobile;
    //    //    //objcmd.Parameters.Add("@cardtype", SqlDbType.NVarChar).Value = oProp.sRashan_; 
    //    //    objcmd.Parameters.Add("@AddP", SqlDbType.NVarChar).Value = oProp.sRashan_rashPadd;
    //    //    objcmd.Parameters.Add("@Addr", SqlDbType.NVarChar).Value = oProp.sRashan_rashPadd;
    //    //    //objcmd.Parameters.Add("@cast", SqlDbType.NVarChar).Value = oProp.sRashan_; 
    //    //    objcmd.Parameters.Add("@profesion", SqlDbType.NVarChar).Value = oProp.sRashan_rashprofession;
    //    //    objcmd.Parameters.Add("@post", SqlDbType.NVarChar).Value = oProp.sRashan_rashPost;
    //    //    objcmd.Parameters.Add("@working", SqlDbType.NVarChar).Value = oProp.sRashan_rashworking;
    //    //    objcmd.Parameters.Add("@consumer", SqlDbType.NVarChar).Value = oProp.sRashan_rashconsumer;
    //    //    objcmd.Parameters.Add("@company", SqlDbType.NVarChar).Value = oProp.sRashan_rashcompany;
    //    //    //objcmd.Parameters.Add("@gasconntype", SqlDbType.NVarChar).Value = oProp.sRashan_rashconnectiontype; 
    //    //    objcmd.Parameters.Add("@surveyno", SqlDbType.NVarChar).Value = oProp.sRashan_surveyno;
    //    //    objcmd.Parameters.Add("@surveyyear", SqlDbType.NVarChar).Value = oProp.sRashan_surveyno;
    //    //    objcmd.Parameters.Add("@Detailenterby", SqlDbType.NVarChar).Value = oProp.sRashan_DETAILENT;
    //    //    objcmd.Parameters.Add("@connectno", SqlDbType.NVarChar).Value = oProp.sRashan_rashconnetion;
    //    //    objcmd.Parameters.Add("@relaname", SqlDbType.NVarChar).Value = oProp.sRashan_ReName;
    //    //    objcmd.Parameters.Add("@ralation", SqlDbType.NVarChar).Value = oProp.sRashan_Relation;
    //    //    objcmd.Parameters.Add("@appliphot", SqlDbType.NVarChar).Value = oProp.sRashan_appliphoto;
    //    //    objcmd.Parameters.Add("@shopno", SqlDbType.NVarChar).Value = oProp.sRashan_shopno;
    //    //    objcmd.Parameters.Add("@shopdesc", SqlDbType.NVarChar).Value = oProp.sRashan_shopdesc;

    //    //    objcmd.Parameters.Add("@income", SqlDbType.Money).Value = oProp.dRashan_monincom;

    //    //    objcmd.Parameters.Add("@rdob", SqlDbType.DateTime).Value = oProp.dRashan_birthdate;
    //    //    //objcmd.Parameters.Add("@dob", SqlDbType.DateTime).Value = oProp.dRashan_trndate; 
    //    //    objcmd.Parameters.Add("@chdate", SqlDbType.DateTime).Value = oProp.dRashan_Rashdate;

    //    //    objcmd.Parameters.Add("@Img", SqlDbType.Image, 0).Value = img;

    //    //    objcmd.ExecuteNonQuery();
    //    //    return true;
    //    //}

    //    public bool InsTmpRation(PropCertificate oProp, byte[] img)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsTmpRation", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;

    //        objcmd.Parameters.Add("@age", SqlDbType.Int).Value = oProp.iRashan_rashage;

    //        if (oProp.iRashan_age > 0)
    //            objcmd.Parameters.Add("@Rage", SqlDbType.Int).Value = oProp.iRashan_age;

    //        //objcmd.Parameters.Add("@Rmale", SqlDbType.Int).Value = oProp.iRashan_male; 
    //        //objcmd.Parameters.Add("@Rfemale", SqlDbType.Int).Value = oProp.iRashan_female; 
    //        objcmd.Parameters.Add("@Rgen", SqlDbType.Int).Value = oProp.iRashan_gen;
    //        objcmd.Parameters.Add("@male", SqlDbType.Int).Value = oProp.iRashan_male;
    //        objcmd.Parameters.Add("@female", SqlDbType.Int).Value = oProp.iRashan_female;

    //        if (oProp.iRashan_srno > 0)
    //            objcmd.Parameters.Add("@srno", SqlDbType.Int).Value = oProp.iRashan_srno;

    //        objcmd.Parameters.Add("@num", SqlDbType.Int).Value = 1;

    //        objcmd.Parameters.Add("@chcode", SqlDbType.NVarChar).Value = oProp.iRashan_trncode;
    //        objcmd.Parameters.Add("@chno", SqlDbType.NVarChar).Value = oProp.sRashan_rashchno;
    //        objcmd.Parameters.Add("@ownernm", SqlDbType.NVarChar).Value = oProp.sRashan_Rashonm;
    //        objcmd.Parameters.Add("@fathernm", SqlDbType.NVarChar).Value = oProp.sRashan_Rashfnm;
    //        objcmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = oProp.sRashan_citynm;
    //        objcmd.Parameters.Add("@colny", SqlDbType.NVarChar).Value = oProp.sRashan_colonynm;
    //        objcmd.Parameters.Add("@rationcardno", SqlDbType.NVarChar).Value = oProp.sRashan_rashservice;
    //        objcmd.Parameters.Add("@zone", SqlDbType.NVarChar).Value = oProp.sRashan_zonwnm;
    //        objcmd.Parameters.Add("@ward", SqlDbType.NVarChar).Value = oProp.sRashan_wardnm;
    //        objcmd.Parameters.Add("@remark", SqlDbType.NVarChar).Value = oProp.sRashan_rema;
    //        objcmd.Parameters.Add("@phone", SqlDbType.NVarChar).Value = oProp.sRashan_Rashphone;
    //        objcmd.Parameters.Add("@mobile", SqlDbType.NVarChar).Value = oProp.sRashan_Rashmobile;
    //        //objcmd.Parameters.Add("@cardtype", SqlDbType.NVarChar).Value = oProp.sRashan_; 
    //        objcmd.Parameters.Add("@AddP", SqlDbType.NVarChar).Value = oProp.sRashan_rashPadd;
    //        objcmd.Parameters.Add("@Addr", SqlDbType.NVarChar).Value = oProp.sRashan_rashPadd;
    //        //objcmd.Parameters.Add("@cast", SqlDbType.NVarChar).Value = oProp.sRashan_; 
    //        objcmd.Parameters.Add("@profesion", SqlDbType.NVarChar).Value = oProp.sRashan_rashprofession;
    //        objcmd.Parameters.Add("@post", SqlDbType.NVarChar).Value = oProp.sRashan_rashPost;
    //        objcmd.Parameters.Add("@working", SqlDbType.NVarChar).Value = oProp.sRashan_rashworking;
    //        objcmd.Parameters.Add("@consumer", SqlDbType.NVarChar).Value = oProp.sRashan_rashconsumer;
    //        objcmd.Parameters.Add("@company", SqlDbType.NVarChar).Value = oProp.sRashan_rashcompany;
    //        objcmd.Parameters.Add("@gasconntype", SqlDbType.NVarChar).Value = oProp.sRashan_rashconnectiontype;
    //        objcmd.Parameters.Add("@surveyno", SqlDbType.NVarChar).Value = oProp.sRashan_surveyno;
    //        objcmd.Parameters.Add("@surveyyear", SqlDbType.NVarChar).Value = oProp.sRashan_surveyno;
    //        objcmd.Parameters.Add("@Detailenterby", SqlDbType.NVarChar).Value = oProp.sRashan_DETAILENT;
    //        objcmd.Parameters.Add("@connectno", SqlDbType.NVarChar).Value = oProp.sRashan_rashconnetion;
    //        objcmd.Parameters.Add("@relaname", SqlDbType.NVarChar).Value = oProp.sRashan_ReName;
    //        objcmd.Parameters.Add("@ralation", SqlDbType.NVarChar).Value = oProp.sRashan_Relation;
    //        objcmd.Parameters.Add("@appliphot", SqlDbType.NVarChar).Value = oProp.sRashan_appliphoto;
    //        objcmd.Parameters.Add("@shopno", SqlDbType.NVarChar).Value = oProp.sRashan_shopno;
    //        objcmd.Parameters.Add("@shopdesc", SqlDbType.NVarChar).Value = oProp.sRashan_shopdesc;

    //        objcmd.Parameters.Add("@income", SqlDbType.Money).Value = oProp.dRashan_monincom;

    //        objcmd.Parameters.Add("@rdob", SqlDbType.DateTime).Value = oProp.dRashan_birthdate;
    //        //objcmd.Parameters.Add("@dob", SqlDbType.DateTime).Value = oProp.dRashan_trndate; 
    //        objcmd.Parameters.Add("@chdate", SqlDbType.DateTime).Value = oProp.dRashan_Rashdate;

    //        objcmd.Parameters.Add("@Img", SqlDbType.Image, 0).Value = img;

    //        objcmd.Parameters.Add("@Duplicate", SqlDbType.Int).Value = oProp.iRashan_Duplicate;

    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }

    //    //private byte[] ConvertImageToByteArray(System.Drawing.Image imageToConvert, System.Drawing.Imaging.ImageFormat formatOfImage)
    //    //{
    //    //    byte[] Ret;
    //    //    try
    //    //    {
    //    //        using (MemoryStream ms = new MemoryStream())
    //    //        {
    //    //            imageToConvert.Save(ms, formatOfImage);
    //    //            Ret = ms.ToArray();
    //    //        }
    //    //    }
    //    //    catch (Exception) { throw; }
    //    //    return Ret;
    //    //}


    //    public bool InsItemain(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsItemain", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@itemcode", SqlDbType.Int).Value = oProp.iStore_Itemcode;
    //        objcmd.Parameters.Add("@itemgrou", SqlDbType.Int).Value = oProp.iStore_itemgrou;
    //        objcmd.Parameters.Add("@itemnumb", SqlDbType.Int).Value = oProp.iStore_itemnumb;
    //        objcmd.Parameters.Add("@itemsrno", SqlDbType.Int).Value = oProp.iStore_itemsrno;
    //        objcmd.Parameters.Add("@itemsort", SqlDbType.Int).Value = oProp.iStore_itemsort;

    //        objcmd.Parameters.Add("@itemalia", SqlDbType.VarChar).Value = oProp.sStore_itemalia;
    //        objcmd.Parameters.Add("@itempart", SqlDbType.VarChar).Value = oProp.sStore_itempart;
    //        objcmd.Parameters.Add("@itemname", SqlDbType.VarChar).Value = oProp.sStore_itemname;
    //        objcmd.Parameters.Add("@itemtext", SqlDbType.VarChar).Value = oProp.sStore_itemtext;
    //        objcmd.Parameters.Add("@itemrefn", SqlDbType.VarChar).Value = oProp.sStore_itemrefn;

    //        objcmd.Parameters.Add("@itemopbl", SqlDbType.Money).Value = oProp.dStore_itemopbl;
    //        objcmd.Parameters.Add("@itemclbl", SqlDbType.Money).Value = oProp.dStore_itemclbl;
    //        objcmd.Parameters.Add("@itemmini", SqlDbType.Money).Value = oProp.dStore_itemmini;
    //        objcmd.Parameters.Add("@itemmaxi", SqlDbType.Money).Value = oProp.dStore_itemmaxi;
    //        objcmd.Parameters.Add("@itemrate", SqlDbType.Money).Value = oProp.dStore_itemrate;
    //        objcmd.Parameters.Add("@itlastrat", SqlDbType.Money).Value = oProp.dStore_itlastrat;
    //        objcmd.Parameters.Add("@itemdisc", SqlDbType.Money).Value = oProp.dStore_itemdisc;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }
    //    public int InsGroup_Unit(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsGroup_Unit", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        da = new SqlDataAdapter(objcmd);

    //        objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.iStore_Type; //1 Group , 0 Category ,2 Unit
    //        objcmd.Parameters.Add("@Unde", SqlDbType.Int).Value = oProp.iStore_Unde;

    //        objcmd.Parameters.Add("@itgpbasi", SqlDbType.Int).Value = oProp.iStore_itgpbasi;
    //        objcmd.Parameters.Add("@itgpbcqt", SqlDbType.Int).Value = oProp.dStore_itgpbcqt;

    //        objcmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = oProp.sStore_Name;
    //        objcmd.Parameters.Add("@Alia", SqlDbType.VarChar).Value = oProp.sStore_Alia;

    //        da.Fill(dt1);
    //        objcmd.Dispose();
    //        if (dt1.Rows.Count > 0)
    //            return Convert.ToInt16(dt1.Rows[0][0].ToString());
    //        else
    //            return 0;
    //    }


    //    public bool InsPricMst(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsPricMst", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@msttype", SqlDbType.Int).Value = oProp.iStore_Price_msttype;
    //        objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iStore_Price_mstcode;
    //        objcmd.Parameters.Add("@mstplty", SqlDbType.Int).Value = oProp.iStore_Price_mstplty;

    //        objcmd.Parameters.Add("@msttota", SqlDbType.Money).Value = oProp.dStore_Price_msttota;

    //        objcmd.Parameters.Add("@mstdate", SqlDbType.DateTime).Value = oProp.dStore_Price_mstdate;

    //        objcmd.Parameters.Add("@mstrema", SqlDbType.NVarChar).Value = oProp.sStore_Price_mstrema;
    //        objcmd.Parameters.Add("@mstchno", SqlDbType.NVarChar).Value = oProp.sStore_Price_mstchno;
    //        objcmd.Parameters.Add("@mstrefc", SqlDbType.NVarChar).Value = oProp.sStore_Price_mstrefc;
    //        objcmd.Parameters.Add("@mstdesc", SqlDbType.NVarChar).Value = oProp.sStore_Price_mstdesc;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }

    //    public bool InsPricItd(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsPricItd", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@itdtype", SqlDbType.Int).Value = oProp.iStore_Price_itdtype;
    //        objcmd.Parameters.Add("@itdcode", SqlDbType.Int).Value = oProp.iStore_Price_itdcode;
    //        objcmd.Parameters.Add("@itdsrno", SqlDbType.Int).Value = oProp.iStore_Price_itdsrno;
    //        objcmd.Parameters.Add("@itditem", SqlDbType.Int).Value = oProp.iStore_Price_itditem;
    //        objcmd.Parameters.Add("@itduni1", SqlDbType.Int).Value = oProp.iStore_Price_itduni1;
    //        objcmd.Parameters.Add("@itduni2", SqlDbType.Int).Value = oProp.iStore_Price_itduni2;
    //        objcmd.Parameters.Add("@itdmill", SqlDbType.Int).Value = oProp.iStore_Price_itdmill;
    //        objcmd.Parameters.Add("@itduni3", SqlDbType.Int).Value = oProp.iStore_Price_itduni3;

    //        objcmd.Parameters.Add("@itdquan", SqlDbType.Money).Value = oProp.dStore_Price_itdquan;
    //        objcmd.Parameters.Add("@itdrate", SqlDbType.Money).Value = oProp.dStore_Price_itdrate;
    //        objcmd.Parameters.Add("@itdpack", SqlDbType.Money).Value = oProp.dStore_Price_itdpack;

    //        objcmd.Parameters.Add("@itddate", SqlDbType.DateTime).Value = oProp.dStore_Price_itddate;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }

    //    public bool InsJourmst(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsJourmst", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@Msttype", SqlDbType.Int).Value = oProp.iStore_Opening_Msttype;
    //        objcmd.Parameters.Add("@Mastcode", SqlDbType.Int).Value = oProp.iStore_Opening_Mastcode;
    //        objcmd.Parameters.Add("@mstcust", SqlDbType.Int).Value = oProp.iStore_Opening_mstcust;
    //        objcmd.Parameters.Add("@mstdepa", SqlDbType.Int).Value = oProp.iStore_Opening_mstdepa;

    //        objcmd.Parameters.Add("@mstrefc", SqlDbType.VarChar).Value = oProp.sStore_Opening_mstrefc;
    //        objcmd.Parameters.Add("@mstchno", SqlDbType.VarChar).Value = oProp.sStore_Opening_mstchno;
    //        objcmd.Parameters.Add("@mstrema", SqlDbType.VarChar).Value = oProp.sStore_Opening_mstrema;
    //        objcmd.Parameters.Add("@mstblno", SqlDbType.VarChar).Value = oProp.sStore_Opening_mstblno;

    //        objcmd.Parameters.Add("@mstneta", SqlDbType.Money).Value = oProp.dStore_Opening_mstneta;
    //        objcmd.Parameters.Add("@msttota", SqlDbType.Money).Value = oProp.dStore_Opening_msttota;
    //        objcmd.Parameters.Add("@mstadde", SqlDbType.Money).Value = oProp.dStore_Opening_mstadde;
    //        objcmd.Parameters.Add("@mstdifq", SqlDbType.Money).Value = oProp.dStore_Opening_mstdifq;

    //        objcmd.Parameters.Add("@Mstdate", SqlDbType.DateTime).Value = oProp.dStore_Opening_Mstdate;

    //        objcmd.Parameters.Add("@msttime", SqlDbType.Int).Value = oProp.iStore_Opening_msttime;
    //        objcmd.Parameters.Add("@mstpaid", SqlDbType.Money).Value = oProp.dStore_Opening_mstpaid;
    //        objcmd.Parameters.Add("@mstbala", SqlDbType.Money).Value = oProp.dStore_Opening_mstbala;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }
    //    public bool UpdJourmst(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("UpdJourmst", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@Msttype", SqlDbType.Int).Value = oProp.iStore_Opening_Msttype;
    //        objcmd.Parameters.Add("@Mastcode", SqlDbType.Int).Value = oProp.iStore_Opening_Mastcode;
    //        objcmd.Parameters.Add("@mstcust", SqlDbType.Int).Value = oProp.iStore_Opening_mstcust;
    //        objcmd.Parameters.Add("@mstdepa", SqlDbType.Int).Value = oProp.iStore_Opening_mstdepa;

    //        objcmd.Parameters.Add("@mstrefc", SqlDbType.VarChar).Value = oProp.sStore_Opening_mstrefc;
    //        objcmd.Parameters.Add("@mstchno", SqlDbType.VarChar).Value = oProp.sStore_Opening_mstchno;
    //        objcmd.Parameters.Add("@mstrema", SqlDbType.VarChar).Value = oProp.sStore_Opening_mstrema;
    //        objcmd.Parameters.Add("@mstblno", SqlDbType.VarChar).Value = oProp.sStore_Opening_mstblno;

    //        objcmd.Parameters.Add("@mstneta", SqlDbType.Money).Value = oProp.dStore_Opening_mstneta;
    //        objcmd.Parameters.Add("@msttota", SqlDbType.Money).Value = oProp.dStore_Opening_msttota;
    //        objcmd.Parameters.Add("@mstadde", SqlDbType.Money).Value = oProp.dStore_Opening_mstadde;
    //        objcmd.Parameters.Add("@mstdifq", SqlDbType.Money).Value = oProp.dStore_Opening_mstdifq;

    //        objcmd.Parameters.Add("@Mstdate", SqlDbType.DateTime).Value = oProp.dStore_Opening_Mstdate;

    //        objcmd.Parameters.Add("@msttime", SqlDbType.Int).Value = oProp.iStore_Opening_msttime;
    //        objcmd.Parameters.Add("@mstpaid", SqlDbType.Money).Value = oProp.dStore_Opening_mstpaid;
    //        objcmd.Parameters.Add("@mstbala", SqlDbType.Money).Value = oProp.dStore_Opening_mstbala;


    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }
    //    public bool InsItPurSal(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsItPurSal", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@ITDtype", SqlDbType.Int).Value = oProp.iStore_Opening_ITDtype;
    //        objcmd.Parameters.Add("@ITDCode", SqlDbType.Int).Value = oProp.iStore_Opening_ITDCode;
    //        objcmd.Parameters.Add("@ITDUNIT", SqlDbType.Int).Value = oProp.iStore_Opening_ITDUNIT;
    //        objcmd.Parameters.Add("@itdmill", SqlDbType.Int).Value = oProp.iStore_Opening_itdmill;
    //        objcmd.Parameters.Add("@itditem", SqlDbType.Int).Value = oProp.iStore_Opening_itditem;
    //        objcmd.Parameters.Add("@itdgodo", SqlDbType.Int).Value = oProp.iStore_Opening_itdgodo;

    //        objcmd.Parameters.Add("@ITDREMA", SqlDbType.VarChar).Value = oProp.sStore_Opening_ITDREMA;

    //        objcmd.Parameters.Add("@ITDsrno", SqlDbType.Money).Value = oProp.dStore_Opening_ITDsrno;
    //        objcmd.Parameters.Add("@ITDQUAN", SqlDbType.Money).Value = oProp.dStore_Opening_ITDQUAN;
    //        objcmd.Parameters.Add("@ITDRATE", SqlDbType.Money).Value = oProp.dStore_Opening_ITDRATE;
    //        objcmd.Parameters.Add("@ITDAMOU", SqlDbType.Money).Value = oProp.dStore_Opening_ITDAMOU;

    //        objcmd.Parameters.Add("@ITDdate", SqlDbType.DateTime).Value = oProp.dStore_Opening_ITDdate;

    //        objcmd.Parameters.Add("@itdpenq", SqlDbType.Money).Value = oProp.dStore_Opening_itdpenq;
    //        objcmd.Parameters.Add("@itddesc", SqlDbType.VarChar).Value = oProp.sStore_Opening_itddesc;
    //        objcmd.Parameters.Add("@itddime", SqlDbType.VarChar).Value = oProp.sStore_Opening_itddime;

    //        objcmd.Parameters.Add("@itdPoDate", SqlDbType.DateTime).Value = oProp.dStore_ItPurSal_itdPoDate;
    //        objcmd.Parameters.Add("@itdrefs", SqlDbType.Int).Value = oProp.iStore_ItPurSal_itdrefs;
    //        objcmd.Parameters.Add("@itdMateType", SqlDbType.Int).Value = oProp.iStore_ItPurSal_itdMateType;
    //        objcmd.Parameters.Add("@itdPoCode", SqlDbType.Int).Value = oProp.iStore_ItPurSal_itdPoCode;

    //        objcmd.Parameters.Add("@itdtime", SqlDbType.Int).Value = oProp.iStore_Opening_itdtime;
    //        objcmd.Parameters.Add("@itdpkin", SqlDbType.Money).Value = oProp.dStore_Opening_itdpkin;
    //        objcmd.Parameters.Add("@itdvate", SqlDbType.Money).Value = oProp.dStore_Opening_itdvate;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }

    //    public bool InsOrdeMst(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsOrdeMst", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@msttype", SqlDbType.Int).Value = oProp.iStore_Ord_msttype;
    //        objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iStore_Ord_mstcode;
    //        objcmd.Parameters.Add("@mstcust", SqlDbType.Int).Value = oProp.iStore_Ord_mstcust;

    //        objcmd.Parameters.Add("@mstrema", SqlDbType.VarChar).Value = oProp.sStore_Ord_mstrema;
    //        objcmd.Parameters.Add("@mstrefc", SqlDbType.VarChar).Value = oProp.sStore_Ord_mstrefc;
    //        objcmd.Parameters.Add("@mstchno", SqlDbType.VarChar).Value = oProp.sStore_Ord_mstchno;

    //        objcmd.Parameters.Add("@msttota", SqlDbType.Money).Value = oProp.dStore_Ord_msttota;
    //        objcmd.Parameters.Add("@mstneta", SqlDbType.Money).Value = oProp.dStore_Ord_mstneta;
    //        objcmd.Parameters.Add("@mstdifq", SqlDbType.Money).Value = oProp.dStore_Ord_mstdifq;
    //        objcmd.Parameters.Add("@mstadde", SqlDbType.Money).Value = oProp.dStore_Ord_mstadde;

    //        objcmd.Parameters.Add("@mstdate", SqlDbType.DateTime).Value = oProp.dStore_Ord_mstdate;


    //        objcmd.Parameters.Add("@mstpaid", SqlDbType.Money).Value = oProp.dStore_Ord_mstpaid;
    //        objcmd.Parameters.Add("@mstbala", SqlDbType.Money).Value = oProp.dStore_Ord_mstbala;
    //        objcmd.Parameters.Add("@msttime", SqlDbType.Int).Value = oProp.iStore_Ord_msttime;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }
    //    public bool UpdOrdeMst(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("UpdOrdeMst", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@msttype", SqlDbType.Int).Value = oProp.iStore_Ord_msttype;
    //        objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iStore_Ord_mstcode;
    //        objcmd.Parameters.Add("@mstcust", SqlDbType.Int).Value = oProp.iStore_Ord_mstcust;

    //        objcmd.Parameters.Add("@mstrema", SqlDbType.VarChar).Value = oProp.sStore_Ord_mstrema;
    //        objcmd.Parameters.Add("@mstrefc", SqlDbType.VarChar).Value = oProp.sStore_Ord_mstrefc;
    //        objcmd.Parameters.Add("@mstchno", SqlDbType.VarChar).Value = oProp.sStore_Ord_mstchno;

    //        objcmd.Parameters.Add("@msttota", SqlDbType.Money).Value = oProp.dStore_Ord_msttota;
    //        objcmd.Parameters.Add("@mstneta", SqlDbType.Money).Value = oProp.dStore_Ord_mstneta;
    //        objcmd.Parameters.Add("@mstdifq", SqlDbType.Money).Value = oProp.dStore_Ord_mstdifq;
    //        objcmd.Parameters.Add("@mstadde", SqlDbType.Money).Value = oProp.dStore_Ord_mstadde;

    //        objcmd.Parameters.Add("@mstdate", SqlDbType.DateTime).Value = oProp.dStore_Ord_mstdate;


    //        objcmd.Parameters.Add("@mstpaid", SqlDbType.Money).Value = oProp.dStore_Ord_mstpaid;
    //        objcmd.Parameters.Add("@mstbala", SqlDbType.Money).Value = oProp.dStore_Ord_mstbala;
    //        objcmd.Parameters.Add("@msttime", SqlDbType.Int).Value = oProp.iStore_Ord_msttime;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }
    //    public bool InsOrdeItd(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsOrdeItd", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@itdtype", SqlDbType.Int).Value = oProp.iStore_Ord_itdtype;
    //        objcmd.Parameters.Add("@itdcode", SqlDbType.Int).Value = oProp.iStore_Ord_itdcode;
    //        objcmd.Parameters.Add("@itdsrno", SqlDbType.Int).Value = oProp.iStore_Ord_itdsrno;
    //        objcmd.Parameters.Add("@itditem", SqlDbType.Int).Value = oProp.iStore_Ord_itditem;
    //        objcmd.Parameters.Add("@itdunit", SqlDbType.Int).Value = oProp.iStore_Ord_itdunit;

    //        objcmd.Parameters.Add("@itdquan", SqlDbType.Money).Value = oProp.dStore_Ord_itdquan;
    //        objcmd.Parameters.Add("@itdrate", SqlDbType.Money).Value = oProp.dStore_Ord_itdrate;
    //        objcmd.Parameters.Add("@itdamou", SqlDbType.Money).Value = oProp.dStore_Ord_itdamou;

    //        objcmd.Parameters.Add("@itddate", SqlDbType.DateTime).Value = oProp.dStore_Ord_itddate;
    //        objcmd.Parameters.Add("@itdDesc", SqlDbType.VarChar).Value = oProp.sStore_Ord_itdDesc;

    //        objcmd.Parameters.Add("@itdmill", SqlDbType.Int).Value = oProp.iStore_Ord_itdmill;
    //        objcmd.Parameters.Add("@itdgodo", SqlDbType.Int).Value = oProp.iStore_Ord_itdgodo;
    //        objcmd.Parameters.Add("@itdtime", SqlDbType.Int).Value = oProp.iStore_Ord_itdtime;

    //        objcmd.Parameters.Add("@itdpkin", SqlDbType.Money).Value = oProp.dStore_Ord_itdpkin;
    //        objcmd.Parameters.Add("@itdvate", SqlDbType.Money).Value = oProp.dStore_Ord_itdvate;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }
    //    public bool UpdCodeGen(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("UpdCodeGen", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@comp", SqlDbType.Int).Value = oProp.icomp_opbal;
    //        objcmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.iTypee_opbal;
    //        objcmd.Parameters.Add("@Prev", SqlDbType.Int).Value = oProp.iPrev_opbal;
    //        objcmd.Parameters.Add("@Curr", SqlDbType.Int).Value = oProp.iCurr_opbal;
    //        objcmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = oProp.dDatee_opbal;
    //        objcmd.Parameters.Add("@StDate", SqlDbType.DateTime).Value = oProp.dStDate;
    //        objcmd.Parameters.Add("@LsDate", SqlDbType.DateTime).Value = oProp.dLastDate;
    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    public bool InsRefetab(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsRefetab", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@ms1code", SqlDbType.Int).Value = oProp.iStore_RefTab_ms1code;
    //        objcmd.Parameters.Add("@ms1type", SqlDbType.Int).Value = oProp.iStore_RefTab_ms1type;
    //        objcmd.Parameters.Add("@ms1srno", SqlDbType.Int).Value = oProp.iStore_RefTab_ms1srno;
    //        objcmd.Parameters.Add("@ms2code", SqlDbType.Int).Value = oProp.iStore_RefTab_ms2code;
    //        objcmd.Parameters.Add("@ms2srno", SqlDbType.Int).Value = oProp.iStore_RefTab_ms2srno;
    //        objcmd.Parameters.Add("@reftype", SqlDbType.Int).Value = oProp.iStore_RefTab_reftype;
    //        objcmd.Parameters.Add("@ms2type", SqlDbType.Int).Value = oProp.iStore_RefTab_ms2type;

    //        objcmd.Parameters.Add("@ms1date", SqlDbType.DateTime).Value = oProp.dStore_RefTab_ms1date;
    //        objcmd.Parameters.Add("@ms2date", SqlDbType.DateTime).Value = oProp.dStore_RefTab_ms2date;
    //        objcmd.Parameters.Add("@refamou", SqlDbType.Money).Value = oProp.dStore_RefTab_refamou;
    //        objcmd.Parameters.Add("@refrema", SqlDbType.VarChar).Value = oProp.sStore_RefTab_refrema;
    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    public bool InsSapuItd(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsSapuItd", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@itdtype", SqlDbType.Int).Value = oProp.iStore_SapuItd_itdtype;
    //        objcmd.Parameters.Add("@itdcode", SqlDbType.Int).Value = oProp.iStore_SapuItd_itdcode;
    //        objcmd.Parameters.Add("@itdsrno", SqlDbType.Int).Value = oProp.iStore_SapuItd_itdsrno;
    //        objcmd.Parameters.Add("@itditem", SqlDbType.Int).Value = oProp.iStore_SapuItd_itditem;
    //        objcmd.Parameters.Add("@itdunit", SqlDbType.Int).Value = oProp.iStore_SapuItd_itdunit;
    //        objcmd.Parameters.Add("@itdmill", SqlDbType.Int).Value = oProp.iStore_SapuItd_itdmill;
    //        objcmd.Parameters.Add("@itdgodo", SqlDbType.Int).Value = oProp.iStore_SapuItd_itdgodo;
    //        objcmd.Parameters.Add("@itdrefs", SqlDbType.Int).Value = oProp.iStore_SapuItd_itdrefs;
    //        objcmd.Parameters.Add("@itdempo", SqlDbType.Int).Value = oProp.iStore_SapuItd_itdempo;
    //        objcmd.Parameters.Add("@itdtime", SqlDbType.Int).Value = oProp.iStore_SapuItd_itdtime;

    //        objcmd.Parameters.Add("@itddime", SqlDbType.VarChar).Value = oProp.sStore_SapuItd_itddime;
    //        objcmd.Parameters.Add("@itdgath", SqlDbType.VarChar).Value = oProp.sStore_SapuItd_itdgath;
    //        objcmd.Parameters.Add("@itdrema", SqlDbType.VarChar).Value = oProp.sStore_SapuItd_itdrema;
    //        objcmd.Parameters.Add("@itdtagno", SqlDbType.VarChar).Value = oProp.sStore_SapuItd_itdtagno;
    //        objcmd.Parameters.Add("@status", SqlDbType.VarChar).Value = oProp.sStore_SapuItd_status;

    //        objcmd.Parameters.Add("@itddate", SqlDbType.DateTime).Value = oProp.dStore_SapuItd_itddate;

    //        objcmd.Parameters.Add("@itdquan", SqlDbType.Money).Value = oProp.dStore_SapuItd_itdquan;
    //        objcmd.Parameters.Add("@itdrate", SqlDbType.Money).Value = oProp.dStore_SapuItd_itdrate;
    //        objcmd.Parameters.Add("@itdamou", SqlDbType.Money).Value = oProp.dStore_SapuItd_itdamou;
    //        objcmd.Parameters.Add("@itdpkin", SqlDbType.Money).Value = oProp.dStore_SapuItd_itdpkin;
    //        objcmd.Parameters.Add("@itdrefq", SqlDbType.Money).Value = oProp.dStore_SapuItd_itdrefq;
    //        objcmd.Parameters.Add("@itdpenq", SqlDbType.Money).Value = oProp.dStore_SapuItd_itdpenq;
    //        objcmd.Parameters.Add("@itdvate", SqlDbType.Money).Value = oProp.dStore_SapuItd_itdvate;

    //        objcmd.Parameters.Add("@itdPoCode", SqlDbType.Int).Value = oProp.iStore_ItPurSal_itdPoCode;
    //        objcmd.Parameters.Add("@itdMateType", SqlDbType.Int).Value = oProp.iStore_ItPurSal_itdMateType;
    //        objcmd.Parameters.Add("@itdPoDate", SqlDbType.DateTime).Value = oProp.dStore_ItPurSal_itdPoDate;

    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }

    //    public bool InsJourTrn(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsJourTrn", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = oProp.iStore_JTrn_trntype;
    //        objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = oProp.iStore_JTrn_trncode;
    //        objcmd.Parameters.Add("@trnitem", SqlDbType.Int).Value = oProp.iStore_JTrn_trnitem;
    //        objcmd.Parameters.Add("@trnrefc", SqlDbType.Int).Value = oProp.iStore_JTrn_trnrefc;
    //        objcmd.Parameters.Add("@trnledg", SqlDbType.Int).Value = oProp.iStore_JTrn_trnledg;
    //        objcmd.Parameters.Add("@trntime", SqlDbType.Int).Value = oProp.iStore_JTrn_trntime;
    //        objcmd.Parameters.Add("@trnexpr", SqlDbType.Int).Value = oProp.iStore_JTrn_trnexpr;
    //        objcmd.Parameters.Add("@trncity", SqlDbType.Int).Value = oProp.iStore_JTrn_trncity;
    //        objcmd.Parameters.Add("@trntagv", SqlDbType.Int).Value = oProp.iStore_JTrn_trntagv;
    //        objcmd.Parameters.Add("@trnshow", SqlDbType.Int).Value = oProp.iStore_JTrn_trnshow;
    //        objcmd.Parameters.Add("@trnsrno", SqlDbType.Int).Value = oProp.iStore_JTrn_trnsrno;
    //        objcmd.Parameters.Add("@trncmdt", SqlDbType.Int).Value = oProp.iStore_JTrn_trncmdt;

    //        objcmd.Parameters.Add("@trnrema", SqlDbType.VarChar).Value = oProp.sStore_JTrn_trnrema;
    //        objcmd.Parameters.Add("@trnbank", SqlDbType.VarChar).Value = oProp.sStore_JTrn_trnbank;
    //        objcmd.Parameters.Add("@trnchno", SqlDbType.VarChar).Value = oProp.sStore_JTrn_trnchno;

    //        objcmd.Parameters.Add("@trndate", SqlDbType.DateTime).Value = oProp.dStore_JTrn_trndate;
    //        objcmd.Parameters.Add("@trnrefd", SqlDbType.DateTime).Value = oProp.dStore_JTrn_trnrefd;
    //        objcmd.Parameters.Add("@trnchdt", SqlDbType.DateTime).Value = oProp.dStore_JTrn_trnchdt;

    //        objcmd.Parameters.Add("@trndram", SqlDbType.Money).Value = oProp.dStore_JTrn_trndram;
    //        objcmd.Parameters.Add("@trncram", SqlDbType.Money).Value = oProp.dStore_JTrn_trncram;
    //        objcmd.Parameters.Add("@trnadju", SqlDbType.Money).Value = oProp.dStore_JTrn_trnadju;
    //        objcmd.Parameters.Add("@trnexpa", SqlDbType.Money).Value = oProp.dStore_JTrn_trnexpa;
    //        objcmd.Parameters.Add("@trnaddv", SqlDbType.Money).Value = oProp.dStore_JTrn_trnaddv;


    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    public DataTable GetDiSearch(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("GetDiSearch", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        da = new SqlDataAdapter(objcmd);

    //        objcmd.Parameters.Add("@Comp", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.iMstType;
    //        objcmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = oProp.sRashan_Rashonm;
    //        objcmd.Parameters.Add("@FName", SqlDbType.NVarChar).Value = oProp.sRashan_Rashfnm;
    //        objcmd.Parameters.Add("@Ward", SqlDbType.VarChar).Value = oProp.sRashan_wardnm;
    //        objcmd.Parameters.Add("@Zone", SqlDbType.VarChar).Value = oProp.sRashan_zonwnm;
    //        objcmd.Parameters.Add("@Colony", SqlDbType.VarChar).Value = oProp.sRashan_colonynm;
    //        objcmd.Parameters.Add("@RashType", SqlDbType.VarChar).Value = oProp.sRashan_Rashtype;
    //        objcmd.Parameters.Add("@Rashservice", SqlDbType.VarChar).Value = oProp.sRashan_rashservice;

    //        objcmd.Parameters.Add("@From", SqlDbType.DateTime).Value = oProp.dFromDate;
    //        objcmd.Parameters.Add("@To", SqlDbType.DateTime).Value = oProp.dToDate;

    //        da.Fill(dt1);
    //        objcmd.Dispose();
    //        return dt1;
    //    }

    //    public DataSet GetMonthlyStock(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("GetMonthlyStock", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataSet ds = new DataSet();
    //        da = new SqlDataAdapter(objcmd);

    //        objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@ItemID", SqlDbType.Int).Value = oProp.iStore_Itemcode;
    //        objcmd.Parameters.Add("@From", SqlDbType.DateTime).Value = oProp.dFromDate;
    //        objcmd.Parameters.Add("@To", SqlDbType.DateTime).Value = oProp.dToDate;
    //        objcmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.iStore_Type;

    //        da.Fill(ds);
    //        objcmd.Dispose();
    //        return ds;
    //    }
    //    public bool InsIncacrela(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsIncacrela", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        da = new SqlDataAdapter(objcmd);

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@inccode", SqlDbType.Int).Value = oProp.iIncacrela_inccode;
    //        objcmd.Parameters.Add("@acctcode", SqlDbType.Int).Value = oProp.iIncacrela_acctcode;
    //        objcmd.Parameters.Add("@serialno", SqlDbType.Int).Value = oProp.iIncacrela_serialno;

    //        objcmd.Parameters.Add("@reportname", SqlDbType.VarChar).Value = oProp.sIncacrela_reportname;
    //        objcmd.Parameters.Add("@acctamt", SqlDbType.Decimal).Value = oProp.dIncacrela_acctamt;

    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    public bool InsRelationMst(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsRelationMst", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        da = new SqlDataAdapter(objcmd);

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@RELCODE", SqlDbType.Int).Value = oProp.iRelationMst_RELCODE;
    //        objcmd.Parameters.Add("@RELNAME", SqlDbType.VarChar).Value = oProp.sRelationMst_RELNAME;
    //        objcmd.Parameters.Add("@WebHindi", SqlDbType.NVarChar).Value = oProp.sRelationMst_WebHindi;

    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    public bool InsCategory(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsCategory", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        da = new SqlDataAdapter(objcmd);

    //        objcmd.Parameters.Add("@Compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@itgptype", SqlDbType.Int).Value = oProp.iStore_Category_Type;
    //        objcmd.Parameters.Add("@itgpUnde", SqlDbType.Int).Value = oProp.iStore_Category_Unde;
    //        objcmd.Parameters.Add("@Code", SqlDbType.Int).Value = oProp.iStore_Category_Code;
    //        objcmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = oProp.sStore_Category_Name;
    //        objcmd.Parameters.Add("@itgpalia", SqlDbType.VarChar).Value = oProp.sStore_Category_Alia;

    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }

    //    public void ColumnGraph(PropCertificate prop)
    //    {
    //        string[] x = new string[prop.rowCount];
    //        //decimal[] y = new decimal[prop.rowCount];
    //        decimal[] y1 = new decimal[prop.rowCount];
    //        decimal[] y2 = new decimal[prop.rowCount];
    //        decimal[] y3 = new decimal[prop.rowCount];
    //        dt = prop.dtGraphData;
    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {
    //            x[i] = dt.Rows[i][prop.cMonth].ToString() + "-" + Convert.ToInt32(dt.Rows[i][prop.cYear]);
    //            //y[i] = Convert.ToInt32(dt.Rows[i][prop.cOpening]);
    //            if (prop.cInward != "")
    //                y1[i] = Convert.ToInt32(dt.Rows[i][prop.cInward]);

    //            if (prop.cOutward != "")
    //                y2[i] = Convert.ToInt32(dt.Rows[i][prop.cOutward]);
    //            if (prop.cBal != "")
    //                y3[i] = Convert.ToInt32(dt.Rows[i][prop.cBal]);
    //        }

    //        // prop.GraphID.Series.Add(new AjaxControlToolkit.BarChartSeries { Data = y, Name = prop.cOpening, });
    //        prop.GraphID.Series.Add(new AjaxControlToolkit.BarChartSeries { Data = y1, Name = prop.cInward, });
    //        prop.GraphID.Series.Add(new AjaxControlToolkit.BarChartSeries { Data = y2, Name = prop.cOutward, });
    //        if (prop.cBal != "")
    //            prop.GraphID.Series.Add(new AjaxControlToolkit.BarChartSeries { Data = y3, Name = prop.cBal });



    //        prop.GraphID.CategoriesAxis = string.Join(",", x);
    //        prop.GraphID.ChartTitle = string.Format(prop.title);
    //        if (x.Length > 2)
    //        {
    //            prop.GraphID.ChartWidth = (x.Length * 150).ToString();
    //        }
    //        prop.GraphID.Visible = true;
    //    }

    //    public bool InsEmployee(propPlanMst oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsEmployee", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.compcode;
    //        objcmd.Parameters.Add("@emparea", SqlDbType.Int).Value = oProp.iEmp_emparea;
    //        objcmd.Parameters.Add("@empbudget", SqlDbType.Int).Value = oProp.iEmp_empbudget;
    //        objcmd.Parameters.Add("@empcate", SqlDbType.Int).Value = oProp.iEmp_empcate;
    //        objcmd.Parameters.Add("@empcity", SqlDbType.Int).Value = oProp.iEmp_empcity;
    //        objcmd.Parameters.Add("@empcode", SqlDbType.Int).Value = oProp.iEmp_empcode;
    //        objcmd.Parameters.Add("@empdsa", SqlDbType.Int).Value = oProp.iEmp_empdsa;
    //        objcmd.Parameters.Add("@empdsap", SqlDbType.Int).Value = oProp.iEmp_empdsap;
    //        objcmd.Parameters.Add("@empgrou", SqlDbType.Int).Value = oProp.iEmp_empgrou;
    //        objcmd.Parameters.Add("@empincome", SqlDbType.Int).Value = oProp.iEmp_empincome;
    //        objcmd.Parameters.Add("@empitrd", SqlDbType.Int).Value = oProp.iEmp_empitrd;
    //        objcmd.Parameters.Add("@empledg", SqlDbType.Int).Value = oProp.iEmp_empledg;
    //        objcmd.Parameters.Add("@empnomi", SqlDbType.Int).Value = oProp.iEmp_empnomi;
    //        objcmd.Parameters.Add("@empprty", SqlDbType.Int).Value = oProp.iEmp_empprty;

    //        objcmd.Parameters.Add("@empaddr", SqlDbType.VarChar).Value = oProp.sEmp_empaddr;
    //        objcmd.Parameters.Add("@empalia", SqlDbType.VarChar).Value = oProp.sEmp_empalia;
    //        objcmd.Parameters.Add("@emphind", SqlDbType.NVarChar).Value = oProp.sEmp_emphind;
    //        objcmd.Parameters.Add("@empname", SqlDbType.VarChar).Value = oProp.sEmp_empname;
    //        objcmd.Parameters.Add("@emppath", SqlDbType.VarChar).Value = oProp.sEmp_emppath;
    //        objcmd.Parameters.Add("@empphon", SqlDbType.VarChar).Value = oProp.sEmp_empphon;
    //        objcmd.Parameters.Add("@emppinc", SqlDbType.VarChar).Value = oProp.sEmp_emppinc;
    //        objcmd.Parameters.Add("@emprefn", SqlDbType.VarChar).Value = oProp.sEmp_emprefn;
    //        objcmd.Parameters.Add("@emprema", SqlDbType.VarChar).Value = oProp.sEmp_emprema;
    //        objcmd.Parameters.Add("@empsort", SqlDbType.VarChar).Value = oProp.sEmp_empsort;
    //        objcmd.Parameters.Add("@empstat", SqlDbType.VarChar).Value = oProp.sEmp_empstat;


    //        objcmd.Parameters.Add("@acwithbl", SqlDbType.VarChar).Value = oProp.dEmp_acwithbl;
    //        objcmd.Parameters.Add("@empagen", SqlDbType.VarChar).Value = oProp.dEmp_empagen;
    //        objcmd.Parameters.Add("@empclcr", SqlDbType.VarChar).Value = oProp.dEmp_empclcr;
    //        objcmd.Parameters.Add("@empcldr", SqlDbType.VarChar).Value = oProp.dEmp_empcldr;
    //        objcmd.Parameters.Add("@empjmbl", SqlDbType.VarChar).Value = oProp.dEmp_empjmbl;
    //        objcmd.Parameters.Add("@empopcr", SqlDbType.VarChar).Value = oProp.dEmp_empopcr;
    //        objcmd.Parameters.Add("@empopdr", SqlDbType.VarChar).Value = oProp.dEmp_empopdr;
    //        objcmd.Parameters.Add("@emprate", SqlDbType.VarChar).Value = oProp.dEmp_emprate;

    //        objcmd.Parameters.Add("@PAN", SqlDbType.VarChar).Value = oProp.sEmp_PAN;
    //        objcmd.Parameters.Add("@ESI", SqlDbType.VarChar).Value = oProp.sEmp_ESI;
    //        objcmd.Parameters.Add("@IDProof", SqlDbType.VarChar).Value = oProp.sEmp_IDProof;
    //        objcmd.Parameters.Add("@ProofNo", SqlDbType.VarChar).Value = oProp.sEmp_ProofNo;
    //        objcmd.Parameters.Add("@AcNo", SqlDbType.VarChar).Value = oProp.sEmp_AcNo;
    //        objcmd.Parameters.Add("@PfAcNo", SqlDbType.VarChar).Value = oProp.sEmp_PfAcNo;
    //        objcmd.Parameters.Add("@BankID", SqlDbType.Int).Value = oProp.iEmp_BankID;
    //        objcmd.Parameters.Add("@pfBankID", SqlDbType.Int).Value = oProp.iEmp_pfBankID;

    //        objcmd.Parameters.Add("@Gender", SqlDbType.Int).Value = oProp.iEmp_Gender;
    //        objcmd.Parameters.Add("@CastID", SqlDbType.Int).Value = oProp.iEmp_CastID;
    //        objcmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = oProp.iEmp_CategoryID;

    //        objcmd.Parameters.Add("@BirthDt", SqlDbType.DateTime).Value = oProp.dEAcc_BirthDt;
    //        objcmd.Parameters.Add("@AnvDt", SqlDbType.DateTime).Value = oProp.dEAcc_AnvDt;

    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    public bool InsEmpAcc(propPlanMst oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsEmpAcc", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.compcode;
    //        objcmd.Parameters.Add("@acctcode", SqlDbType.Int).Value = oProp.iEAcc_acctcode;
    //        objcmd.Parameters.Add("@emposrno", SqlDbType.Int).Value = oProp.iEAcc_emposrno;
    //        objcmd.Parameters.Add("@empopano", SqlDbType.Int).Value = oProp.iEAcc_empopano;
    //        objcmd.Parameters.Add("@empobuhe", SqlDbType.Int).Value = oProp.iEAcc_empobuhe;
    //        objcmd.Parameters.Add("@empopost", SqlDbType.Int).Value = oProp.iEAcc_empopost;
    //        objcmd.Parameters.Add("@empodesg", SqlDbType.Int).Value = oProp.iEAcc_empodesg;
    //        objcmd.Parameters.Add("@empoccla", SqlDbType.Int).Value = oProp.iEAcc_empoccla;
    //        objcmd.Parameters.Add("@empbnknm", SqlDbType.Int).Value = oProp.iEAcc_empbnknm;
    //        objcmd.Parameters.Add("@empovarg", SqlDbType.Int).Value = oProp.iEAcc_empovarg;
    //        objcmd.Parameters.Add("@emppfbnk", SqlDbType.Int).Value = oProp.iEAcc_emppfbnk;
    //        objcmd.Parameters.Add("@empovvht", SqlDbType.Int).Value = oProp.iEAcc_empovvht;
    //        objcmd.Parameters.Add("@emposexd", SqlDbType.Int).Value = oProp.iEAcc_emposexd;
    //        objcmd.Parameters.Add("@empidpfc", SqlDbType.Int).Value = oProp.iEAcc_empidpfc;

    //        objcmd.Parameters.Add("@empoaddr", SqlDbType.VarChar).Value = oProp.sEAcc_empoaddr;
    //        objcmd.Parameters.Add("@empobano", SqlDbType.VarChar).Value = oProp.sEAcc_empobano;
    //        objcmd.Parameters.Add("@empogpno", SqlDbType.VarChar).Value = oProp.sEAcc_empogpno;
    //        objcmd.Parameters.Add("@empocpno", SqlDbType.VarChar).Value = oProp.sEAcc_empocpno;
    //        objcmd.Parameters.Add("@empopann", SqlDbType.VarChar).Value = oProp.sEAcc_empopann;
    //        objcmd.Parameters.Add("@empidprf", SqlDbType.VarChar).Value = oProp.sEAcc_empidprf;
    //        objcmd.Parameters.Add("@empesino", SqlDbType.VarChar).Value = oProp.sEAcc_empesino;
    //        objcmd.Parameters.Add("@empprfno", SqlDbType.VarChar).Value = oProp.sEAcc_empprfno;

    //        objcmd.Parameters.Add("@empojodt", SqlDbType.DateTime).Value = oProp.dEAcc_empojodt;
    //        objcmd.Parameters.Add("@empobidt", SqlDbType.DateTime).Value = oProp.dEAcc_empobidt;
    //        objcmd.Parameters.Add("@empoindt", SqlDbType.DateTime).Value = oProp.dEAcc_empoindt;
    //        objcmd.Parameters.Add("@empodoan", SqlDbType.DateTime).Value = oProp.dEAcc_empodoan;

    //        objcmd.Parameters.Add("@empolicn", SqlDbType.Money).Value = oProp.dEAcc_empolicn;
    //        objcmd.Parameters.Add("@empobasl", SqlDbType.Money).Value = oProp.dEAcc_empobasl;

    //        objcmd.ExecuteNonQuery();
    //        return true;
    //    }
    //    public bool InsSapuMst(propPlanMst oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsSapuMst", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.compcode;
    //        objcmd.Parameters.Add("@msttype", SqlDbType.Int).Value = oProp.iSapuMst_msttype;
    //        objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iSapuMst_mstcode;
    //        objcmd.Parameters.Add("@mstsite", SqlDbType.Int).Value = oProp.iSapuMst_mstsite;
    //        objcmd.Parameters.Add("@mstcust", SqlDbType.Int).Value = oProp.iSapuMst_mstcust;
    //        objcmd.Parameters.Add("@mstlate", SqlDbType.Int).Value = oProp.iSapuMst_mstlate;
    //        objcmd.Parameters.Add("@mstAppr", SqlDbType.Int).Value = oProp.iSapuMst_mstAppr;
    //        objcmd.Parameters.Add("@mstptcode", SqlDbType.Int).Value = oProp.iSapuMst_mstptcode;

    //        objcmd.Parameters.Add("@mstrema", SqlDbType.VarChar).Value = oProp.sSapuMst_mstrema;
    //        objcmd.Parameters.Add("@mstrefc", SqlDbType.VarChar).Value = oProp.sSapuMst_mstrefc;
    //        objcmd.Parameters.Add("@mstchno", SqlDbType.VarChar).Value = oProp.sSapuMst_mstchno;
    //        objcmd.Parameters.Add("@mststatus", SqlDbType.VarChar).Value = oProp.sSapuMst_mststatus;

    //        objcmd.Parameters.Add("@msttota", SqlDbType.Money).Value = oProp.dSapuMst_msttota;
    //        objcmd.Parameters.Add("@mstneta", SqlDbType.Money).Value = oProp.dSapuMst_mstneta;
    //        objcmd.Parameters.Add("@mstpdc", SqlDbType.Float).Value = oProp.dSapuMst_mstpdc;
    //        objcmd.Parameters.Add("@oldwht", SqlDbType.Float).Value = oProp.dSapuMst_oldwht;
    //        objcmd.Parameters.Add("@oldamt", SqlDbType.Float).Value = oProp.dSapuMst_oldamt;

    //        objcmd.Parameters.Add("@mstdate", SqlDbType.DateTime).Value = oProp.dSapuMst_mstdate;
    //        objcmd.Parameters.Add("@mstcfdt", SqlDbType.DateTime).Value = oProp.dSapuMst_mstcfdt;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }
    //    public bool insSapuTrn(propPlanMst oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("insSapuTrn", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.compcode;
    //        objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = oProp.iSapuTrn_trntype;
    //        objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = oProp.iSapuTrn_trncode;
    //        objcmd.Parameters.Add("@trnitem", SqlDbType.Int).Value = oProp.iSapuTrn_trnitem;
    //        objcmd.Parameters.Add("@trnrefc", SqlDbType.Int).Value = oProp.iSapuTrn_trnrefc;
    //        objcmd.Parameters.Add("@trnshow", SqlDbType.Int).Value = oProp.iSapuTrn_trnshow;
    //        objcmd.Parameters.Add("@trnsrno", SqlDbType.Int).Value = oProp.iSapuTrn_trnsrno;

    //        objcmd.Parameters.Add("@trnadju", SqlDbType.Money).Value = oProp.dSapuTrn_trnadju;
    //        objcmd.Parameters.Add("@trndram", SqlDbType.Money).Value = oProp.dSapuTrn_trndram;
    //        objcmd.Parameters.Add("@trncram", SqlDbType.Money).Value = oProp.dSapuTrn_trncram;
    //        objcmd.Parameters.Add("@trnexpa", SqlDbType.Money).Value = oProp.dSapuTrn_trnexpa;
    //        objcmd.Parameters.Add("@trnaddv", SqlDbType.Money).Value = oProp.dSapuTrn_trnaddv;
    //        objcmd.Parameters.Add("@trnactpay", SqlDbType.Money).Value = oProp.dSapuTrn_trnactpay;

    //        objcmd.Parameters.Add("@trnrema", SqlDbType.VarChar).Value = oProp.sSapuTrn_trnrema;

    //        objcmd.Parameters.Add("@trnrefd", SqlDbType.DateTime).Value = oProp.dSapuTrn_trnrefd;
    //        objcmd.Parameters.Add("@trndate", SqlDbType.DateTime).Value = oProp.dSapuTrn_trndate;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }
    //    public bool InstEmpLoan(propPlanMst oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InstEmpLoan", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.compcode;
    //        objcmd.Parameters.Add("@Code", SqlDbType.Int).Value = oProp.iEmpLoan_Code;
    //        objcmd.Parameters.Add("@CR", SqlDbType.Int).Value = oProp.iEmpLoan_CR;
    //        objcmd.Parameters.Add("@DR", SqlDbType.Int).Value = oProp.iEmpLoan_DR;
    //        objcmd.Parameters.Add("@Mode", SqlDbType.Int).Value = oProp.iEmpLoan_Mode;
    //        objcmd.Parameters.Add("@EmpCode", SqlDbType.Int).Value = oProp.iEmpLoan_EmpCode;
    //        objcmd.Parameters.Add("@Installment", SqlDbType.Int).Value = oProp.iEmpLoan_Installment;
    //        objcmd.Parameters.Add("@PaidInstall", SqlDbType.Int).Value = oProp.iEmpLoan_PaidInstall;
    //        objcmd.Parameters.Add("@BalInst", SqlDbType.Int).Value = oProp.iEmpLoan_BalInst;
    //        objcmd.Parameters.Add("@AdjuIntallment", SqlDbType.Int).Value = oProp.iEmpLoan_AdjuIntallment;


    //        objcmd.Parameters.Add("@Amount", SqlDbType.Money).Value = oProp.dEmpLoan_Amount;
    //        objcmd.Parameters.Add("@Interest", SqlDbType.Money).Value = oProp.dEmpLoan_Interest;
    //        objcmd.Parameters.Add("@interestAmt", SqlDbType.Money).Value = oProp.dEmpLoan_interestAmt;
    //        objcmd.Parameters.Add("@Total", SqlDbType.Money).Value = oProp.dEmpLoan_Total;
    //        objcmd.Parameters.Add("@PaidAmt", SqlDbType.Money).Value = oProp.dEmpLoan_PaidAmt;
    //        objcmd.Parameters.Add("@InstallmentAmt", SqlDbType.Money).Value = oProp.dEmpLoan_InstallmentAmt;
    //        objcmd.Parameters.Add("@BalAmt", SqlDbType.Money).Value = oProp.dEmpLoan_BalAmt;
    //        objcmd.Parameters.Add("@AdjuAmt", SqlDbType.Money).Value = oProp.dEmpLoan_AdjuAmt;

    //        objcmd.Parameters.Add("@Challan", SqlDbType.VarChar).Value = oProp.sEmpLoan_Challan;
    //        objcmd.Parameters.Add("@remark", SqlDbType.VarChar).Value = oProp.sEmpLoan_remark;

    //        objcmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = oProp.dEmpLoan_Date;
    //        objcmd.Parameters.Add("@FromMonth", SqlDbType.DateTime).Value = oProp.dEmpLoan_FromMonth;
    //        objcmd.Parameters.Add("@LoanMonth", SqlDbType.DateTime).Value = oProp.dEmpLoan_LoanMonth;
    //        objcmd.Parameters.Add("@CheckNo", SqlDbType.VarChar).Value = oProp.sCheckNo;//sourabh 12-jun-2015
    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }

    //    public bool InsPropHeadAmt(PropProperty oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsPropHeadAmt", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@MstSrNo", SqlDbType.Int).Value = oProp.iMSTSRNO_PTE;
    //        objcmd.Parameters.Add("@HeadID", SqlDbType.Int).Value = oProp.iHeadID;

    //        objcmd.Parameters.Add("@Amt", SqlDbType.Money).Value = oProp.dTax_Amt;
    //        objcmd.Parameters.Add("@ServiceNo", SqlDbType.VarChar).Value = oProp.sServiceNo;
    //        objcmd.Parameters.Add("@AcNo", SqlDbType.VarChar).Value = oProp.sACNO_PTE;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }

    //    public bool InsBillTrn_Prop(PropProperty oProp, bool isDR)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsBillTrn", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = oProp.iBill_type;
    //        objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = oProp.iBill_Code;
    //        objcmd.Parameters.Add("@trndate", SqlDbType.DateTime).Value = oProp.iBill_OpDate;
    //        objcmd.Parameters.Add("@trnrefc", SqlDbType.Int).Value = 0;
    //        objcmd.Parameters.Add("@trnexpr", SqlDbType.Int).Value = 0;
    //        objcmd.Parameters.Add("@trnrema", SqlDbType.NVarChar).Value = "";
    //        objcmd.Parameters.Add("@trnadju", SqlDbType.Money).Value = 0;
    //        objcmd.Parameters.Add("@trnexpa", SqlDbType.Money).Value = 0;
    //        objcmd.Parameters.Add("@trnaddv", SqlDbType.Money).Value = 0;
    //        objcmd.Parameters.Add("@trnchno", SqlDbType.NVarChar).Value = "";
    //        objcmd.Parameters.Add("@trnbank", SqlDbType.VarChar).Value = "";
    //        objcmd.Parameters.Add("@trntime", SqlDbType.Int).Value = 0;

    //        objcmd.Parameters.Add("@trnsrno", SqlDbType.Int).Value = oProp.iSrNo_opbal;
    //        objcmd.Parameters.Add("@trnitem", SqlDbType.Int).Value = oProp.iHeadID;
    //        objcmd.Parameters.Add("@trnledg", SqlDbType.Int).Value = oProp.iHeadID;

    //        if (isDR == true)
    //            objcmd.Parameters.Add("@trndram", SqlDbType.Money).Value = oProp.dTax_Amt; //oProp.iBill_Curr;
    //        else
    //            objcmd.Parameters.Add("@trncram", SqlDbType.Money).Value = oProp.dTax_Amt; //oProp.iBill_Curr;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }

    //    public bool InsBillWtrn_Prop(PropProperty oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsBillWtrn", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = oProp.iBill_Code;
    //        objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = oProp.iBill_type;
    //        objcmd.Parameters.Add("@trncust", SqlDbType.Int).Value = oProp.iBill_CustID;
    //        objcmd.Parameters.Add("@trnbillno", SqlDbType.Int).Value = oProp.iBill_Code;
    //        objcmd.Parameters.Add("@trndate", SqlDbType.DateTime).Value = oProp.iBill_OpDate;
    //        objcmd.Parameters.Add("@trnmn", SqlDbType.Int).Value = oProp.iBill_OpMonth;

    //        objcmd.Parameters.Add("@trnyr", SqlDbType.Int).Value = oProp.iBill_OpYear;
    //        objcmd.Parameters.Add("@trheadc", SqlDbType.Int).Value = oProp.iHeadID;
    //        objcmd.Parameters.Add("@trnamtd", SqlDbType.Money).Value = oProp.dTax_Amt; // oProp.iBill_Curr;
    //        objcmd.Parameters.Add("@trnamtp", SqlDbType.Money).Value = 0;
    //        objcmd.Parameters.Add("@trmaina", SqlDbType.Money).Value = oProp.dTax_Amt;
    //        objcmd.Parameters.Add("@trlatef", SqlDbType.Money).Value = 0;
    //        objcmd.Parameters.AddWithValue("@trLatHead", oProp.iLatfeeHeadID);

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }

    //    public bool InsBAK_SapuTrn(int iCompCode, int iBill_type, int iBill_Code)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsBAK_SapuTrn", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = iCompCode;
    //        objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = iBill_type;
    //        objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = iBill_Code;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }

    //    public bool InsBAK_WmonthTrn(int iCompCode, int iBill_type, int iBill_Code)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsBAK_WmonthTrn", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = iCompCode;
    //        objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = iBill_type;
    //        objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = iBill_Code;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }
    //    public bool InsBak_PROPTRN(int iCompCode, int iSRNO)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsBak_PROPTRN", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = iCompCode;
    //        objcmd.Parameters.Add("@SRNO", SqlDbType.Int).Value = iSRNO;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }

    //    public bool InsOwner(PropProperty oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsOwner", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@infcode", SqlDbType.Int).Value = oProp.iOwn_infcode;
    //        objcmd.Parameters.Add("@inftype", SqlDbType.Int).Value = oProp.iOwn_inftype;
    //        objcmd.Parameters.Add("@infsrno", SqlDbType.Int).Value = oProp.iOwn_infsrno;
    //        objcmd.Parameters.Add("@infdate", SqlDbType.DateTime).Value = oProp.dOwn_infdate;
    //        objcmd.Parameters.Add("@ownernm ", SqlDbType.VarChar).Value = oProp.sOwn_ownernm;
    //        objcmd.Parameters.Add("@ownerfnm", SqlDbType.VarChar).Value = oProp.sOwn_ownerfnm;
    //        objcmd.Parameters.Add("@ownerphno", SqlDbType.VarChar).Value = oProp.sOwn_ownerphno;
    //        objcmd.Parameters.Add("@owneradd", SqlDbType.VarChar).Value = oProp.sOwn_owneradd;
    //        objcmd.Parameters.Add("@AcNo", SqlDbType.VarChar).Value = oProp.sOwn_AcNo;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }



    //    public bool InsPanjiyan(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsPanjiyan", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@Compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@mstType", SqlDbType.Int).Value = oProp.iMstType;
    //        objcmd.Parameters.Add("@mstCode", SqlDbType.Int).Value = oProp.iMstCode;
    //        objcmd.Parameters.Add("@FAge", SqlDbType.Int).Value = oProp.iFAge;
    //        objcmd.Parameters.Add("@FReligion", SqlDbType.Int).Value = oProp.iFReligion;
    //        objcmd.Parameters.Add("@FNational", SqlDbType.Int).Value = oProp.iFNational;
    //        objcmd.Parameters.Add("@Gender", SqlDbType.Int).Value = oProp.iGender;
    //        objcmd.Parameters.Add("@MAge", SqlDbType.Int).Value = oProp.iMAge;
    //        objcmd.Parameters.Add("@MReligion", SqlDbType.Int).Value = oProp.iMReligion;
    //        objcmd.Parameters.Add("@MNational", SqlDbType.Int).Value = oProp.iMNational;
    //        objcmd.Parameters.Add("@Zone", SqlDbType.Int).Value = oProp.iZone;
    //        objcmd.Parameters.Add("@Colony", SqlDbType.Int).Value = oProp.iColony;
    //        objcmd.Parameters.Add("@Ward", SqlDbType.Int).Value = oProp.iWard;
    //        objcmd.Parameters.Add("@City", SqlDbType.Int).Value = oProp.iCity;
    //        objcmd.Parameters.Add("@Child", SqlDbType.Int).Value = oProp.iChild;
    //        objcmd.Parameters.Add("@DileveryNo", SqlDbType.Int).Value = oProp.iDileveryNo;

    //        objcmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = oProp.dMstDate;
    //        objcmd.Parameters.Add("@BirthDt", SqlDbType.DateTime).Value = oProp.dBirthDt;

    //        objcmd.Parameters.Add("@RegNo", SqlDbType.VarChar).Value = oProp.sRegNo;
    //        objcmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = oProp.sName;
    //        objcmd.Parameters.Add("@HName", SqlDbType.NVarChar).Value = oProp.sHName;
    //        objcmd.Parameters.Add("@Time", SqlDbType.VarChar).Value = oProp.sTime;
    //        objcmd.Parameters.Add("@Place", SqlDbType.NVarChar).Value = oProp.sPlace;
    //        objcmd.Parameters.Add("@Father", SqlDbType.VarChar).Value = oProp.sFather;
    //        objcmd.Parameters.Add("@HFather", SqlDbType.NVarChar).Value = oProp.sHFather;
    //        objcmd.Parameters.Add("@Mother", SqlDbType.VarChar).Value = oProp.sMother;
    //        objcmd.Parameters.Add("@HMother", SqlDbType.NVarChar).Value = oProp.sHMother;
    //        objcmd.Parameters.Add("@Informar", SqlDbType.VarChar).Value = oProp.sInformar;
    //        objcmd.Parameters.Add("@HInformar", SqlDbType.NVarChar).Value = oProp.sHInformar;
    //        objcmd.Parameters.Add("@IAddress", SqlDbType.VarChar).Value = oProp.sIAddress;
    //        objcmd.Parameters.Add("@FEducation", SqlDbType.VarChar).Value = oProp.sFEducation;
    //        objcmd.Parameters.Add("@FOccupation", SqlDbType.VarChar).Value = oProp.sFOccupation;
    //        objcmd.Parameters.Add("@MEducation", SqlDbType.VarChar).Value = oProp.sMEducation;
    //        objcmd.Parameters.Add("@MOccupation", SqlDbType.VarChar).Value = oProp.sMOccupation;
    //        objcmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = oProp.sAddress;
    //        objcmd.Parameters.Add("@DeathRegion", SqlDbType.VarChar).Value = oProp.sDeathRegion;
    //        objcmd.Parameters.Add("@Remark", SqlDbType.VarChar).Value = oProp.sRemark;
    //        objcmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = oProp.isCancel;


    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }

    //    public bool InsWatCharge(PropProperty oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("InsWatCharge", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        SqlDataAdapter da = new SqlDataAdapter();

    //        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //        objcmd.Parameters.Add("@code", SqlDbType.Int).Value = oProp.iMstCodeV;
    //        objcmd.Parameters.Add("@WatCode", SqlDbType.Int).Value = oProp.iBill_Code;
    //        objcmd.Parameters.Add("@eDate", SqlDbType.DateTime).Value = oProp.dDate;
    //        objcmd.Parameters.Add("@WatDate", SqlDbType.DateTime).Value = oProp.dPTA_Date;
    //        objcmd.Parameters.Add("@HeadID", SqlDbType.Int).Value = oProp.iHeadID;
    //        objcmd.Parameters.Add("@HeadAmt", SqlDbType.Money).Value = oProp.dTax_Amt;
    //        objcmd.Parameters.Add("@PaidAmt", SqlDbType.Money).Value = oProp.iBill_Due;

    //        objcmd.ExecuteNonQuery();
    //        return true;

    //    }

    //    public DataTable GetReceipt(PropCertificate oProp)
    //    {
    //        GetConection();
    //        SqlCommand objcmd = new SqlCommand("GetReceipt", con);
    //        objcmd.CommandType = CommandType.StoredProcedure;
    //        DataTable dt1 = new DataTable();
    //        da = new SqlDataAdapter(objcmd);

    //        objcmd.Parameters.Add("@OpenDate", SqlDbType.DateTime).Value = oProp.dFromDate;
    //        objcmd.Parameters.Add("@CloseDate", SqlDbType.DateTime).Value = oProp.dToDate;

    //        da.Fill(dt1);
    //        objcmd.Dispose();
    //        return dt1;
    //    }


    public DataTable Getworkclient()
    {
        GetConection();
        dt = new DataTable();
        cmd = new SqlCommand();
        cmd.CommandText = "getclient";// "getemployeeN";
        cmd.Connection = con; //oDbInterface.dbGetConn();
        cmd.CommandType = CommandType.Text;
        da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        con.Close();

        return dt;
    }
    public DataTable getemployeeN()
    {
        GetConection();
        dt = new DataTable();
        cmd = new SqlCommand();
        cmd.CommandText = "getemployeeN";
        cmd.Connection = con; //oDbInterface.dbGetConn();
        cmd.CommandType = CommandType.Text;
        da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        con.Close();

        return dt;
    }
    public DataTable getService(int iClientID)
    {
        GetConection();
        dt = new DataTable();
        cmd = new SqlCommand();
        cmd.CommandText = "getService";
        cmd.Connection = con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@empID", iClientID);
        da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        con.Close();

        return dt;
    }

    public DataTable getService_C(int iClientID)
    {
        GetConection();
        dt = new DataTable();
        cmd = new SqlCommand();
        cmd.CommandText = "getService";
        cmd.Connection = con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Client", iClientID);
        da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        con.Close();

        return dt;
    }
    public DataTable getCharge(int iClientID)
    {
        GetConection();
        dt = new DataTable();
        cmd = new SqlCommand();
        cmd.CommandText = "getCharge";
        cmd.Connection = con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ClientID", iClientID);
        da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        con.Close();

        return dt;
    }

    //public void InsBillMst(propSauda prob)
    //{
    //    GetConection();
    //    DateTime dt = DateTime.Now;
    //    cmd = new SqlCommand();
    //    cmd.CommandText = "InsBillMst";
    //    cmd.Connection = con;
    //    cmd.CommandType = CommandType.StoredProcedure;

    //    cmd.Parameters.AddWithValue("@Compcode", prob.Compcode);
    //    cmd.Parameters.AddWithValue("@mstCode", prob.mstCode);
    //    cmd.Parameters.AddWithValue("@mstDate", prob.mstDate);
    //    cmd.Parameters.AddWithValue("@RefNo", prob.RefNo);
    //    cmd.Parameters.AddWithValue("@SaudaID", prob.iSoudaID);
    //    cmd.Parameters.AddWithValue("@SaudaNo", prob.sSoudaNo);
    //    cmd.Parameters.AddWithValue("@LrNo", prob.sLR);
    //    cmd.Parameters.AddWithValue("@LorryNo", prob.sLorryNo);
    //    cmd.Parameters.AddWithValue("@Qty", prob.dQty);
    //    cmd.Parameters.AddWithValue("@Rate", prob.dRate);
    //    cmd.Parameters.AddWithValue("@Amount", prob.dAmount);
    //    cmd.Parameters.AddWithValue("@LotNo", prob.LotNo);
    //    cmd.Parameters.AddWithValue("@PrFrom", prob.sPrFrom);
    //    cmd.Parameters.AddWithValue("@PrTo", prob.sPrTo);
    //    cmd.Parameters.AddWithValue("@FormNo", prob.sFormNo);
    //    cmd.Parameters.AddWithValue("@Package", prob.sPackage);
    //    cmd.Parameters.AddWithValue("@ApproveDt", prob.dApprovDt);
    //    cmd.Parameters.AddWithValue("@Remark", prob.Remark);
    //    cmd.Parameters.AddWithValue("@isFinal", prob.isFinal);
    //    cmd.Parameters.AddWithValue("@PerAmt", prob.dPerAmt);
    //    cmd.Parameters.AddWithValue("@Percent", prob.dPercent);
    //    cmd.Parameters.AddWithValue("@NetAmt", prob.dNetAmt);
    //    cmd.Parameters.AddWithValue("@Expenses", prob.sExpenses);
    //    cmd.Parameters.AddWithValue("@BranchID", prob.iBranchID);
    //    cmd.Parameters.AddWithValue("@DespID", prob.iDespID);

    //    cmd.ExecuteNonQuery();
    //    con.Close();

    //}


    public DataTable getLotNo(string sLotNo)
    {
        GetConection();
        dt = new DataTable();
        cmd = new SqlCommand();
        cmd.CommandText = "getLotNo";
        cmd.Connection = con;
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@LotNo", sLotNo);

        da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        con.Close();

        return dt;
    }





    //public DataTable GetCompRela(int CompCode, int BranchID, int SaudaID)
    public DataTable GetCompRela(int SaudaID)
    {
        GetConection();
        dt = new DataTable();
        cmd = new SqlCommand();
        cmd.CommandText = "GetCompRela";
        cmd.Connection = con;
        cmd.CommandType = CommandType.StoredProcedure;

        //cmd.Parameters.AddWithValue("@CompCode", CompCode );
        //cmd.Parameters.AddWithValue("@BranchID", BranchID);
        cmd.Parameters.AddWithValue("@SaudaID", SaudaID);

        da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        con.Close();

        return dt;
    }



    // Irfan 08-04-2016->
    public DataTable paymentAdj(string strId)
    {
        dt = new DataTable();
        SqlCommand cmdL = null;
        if (dr != null) dr.Close();
        GetConection();
        cmdL = new SqlCommand("SELECT taamsttype,Convert(varchar(10),CONVERT(date,taaRcpDate,106),103)  as BDate,ROUND(taaAmtAdjust,2)  as BAmt FROM tblamtadjust a inner join tblpaymentent b on   a.taarcpcode=b.tpcode and a.taamsttype = b.tpseputype WHERE  a.taabillid='" + strId + "'  and taaAmtAdjust<>0 ", con);
        cmdL.CommandTimeout = 5000;
        da = new SqlDataAdapter(cmdL);
        da.Fill(dt);
        cmdL.Dispose();
        return dt;
    }

    public DataTable GetSaudaRec(string sComp, string sBranch, string sBuyer, string sType, string sCommodity, string sVarityid, string sLotNo)
    {
        dt = new DataTable();
        SqlCommand cmdL = null;
        if (dr != null) dr.Close();
        GetConection();
        cmdL = new SqlCommand(@"select billType,compalia as Company,SaudaNo , CONVERT(VARCHAR(10),SaudaDate,104) SaudaDate ,FinalInvNo as BillNo ,CONVERT(VARCHAR(10),mstDate,104) BillDate ,PartyName as 'Party Name'  ,Branch as Branch, Quality as 'Quality' ,LotNo as 'Lot No' ,	RefNo as 'Ref No'  ,PrFrom	as 'Pr No.1',	PrTo as 'Pr No.2', abs(Bales) Bales,CandyRate	'Candy Rate',GrossWt as 'Gross Weight',	tarewt as 'Tare',abs(NetWt) as 'Net Weight', Grossamount as 'Gross Amount',FormPerc as 'Tax/E-1:C:FORM',FormPercAmt as 'Tax Amount',netamt as 'Net Amount',Station,TransportNM,LorryNo as 'Truck No',LrNo 'Lr No',TaaBillId ,BillType,'' as BillType1,'' as 'RES.AMOUNT NO.-1','' as BillType2,'' as 'RES.AMOUNT NO.-2','' as BillType3,'' as 'RES.AMOUNT NO.-3','' as BillType4 ,'' as 'RES.AMOUNT NO.-4' ,'' as BillType5,'' as 'RES.AMOUNT NO.-5' ,'' as BillType6,'' as 'RES.AMOUNT NO.-6' ,'' as BillType7,'' as 'RES.AMOUNT NO.-7' ,'' as BillType8,'' as 'RES.AMOUNT NO.-8' ,'' as BillType9,'' as 'RES.AMOUNT NO.-9' ,'' as Bank ,'' as 'RES DATE NO.-1','' as 'RES DATE NO.-2' ,'' as 'RES DATE NO.-3' ,'' as 'RES DATE NO.-4' ,'' as 'RES DATE NO.-5' ,'' as 'RES DATE NO.-6' ,'' as 'RES DATE NO.-7' ,'' as 'RES DATE NO.-8' ,'' as 'RES DATE NO.-9','' as 'Balance',Remark from newSpView where 1=1  and partyname is not null  " + sComp + sBranch + sBuyer + sType + sCommodity + sVarityid + sLotNo, con);
        cmdL.CommandTimeout = 5000;
        da = new SqlDataAdapter(cmdL);
        da.Fill(dt);
        cmdL.Dispose();
        return dt;
    }


    //

    public DataTable GetSaudaRecM(string sComp, string sBranch, string sBuyer, string sType, string sCommodity, string sVarityid, string sLotNo)
    {
        dt = new DataTable();
        SqlCommand cmdL = null;
        if (dr != null) dr.Close();
        GetConection();
        cmdL = new SqlCommand(@"select compalia as Company,FinalInvNo as BillId ,Pcode,PartyName as 'Party Name',Bcode,Brokers as 'Broker Name'  ,Branch as Branch, Quality as 'Quality' ,LotNo as 'Lot No' ,	RefNo as 'Ref No'  ,PrFrom	as 'Pr No.1',	PrTo as 'Pr No.2', 	Bales,CandyRate	'Candy Rate',GrossWt as 'Gross Weight',	tarewt as 'Tare',NetWt as 'Net Weight', GrossWt as 'Gross Amount',FormPerc as 'Tax/E-1:C:FORM',FormPercAmt as 'Tax Amount',netamt as 'Net Amount',Station,Transport,LorryNo as 'Truck No',LrNo 'Lr No',TaaBillId ,BillType
    ,'' as BillType1,'' as 'RES.AMOUNT NO.-1','' as BillType2,'' as 'RES.AMOUNT NO.-2','' as BillType3,'' as 'RES.AMOUNT NO.-3','' as BillType4 ,'' as 'RES.AMOUNT NO.-4' ,'' as BillType5,'' as 'RES.AMOUNT NO.-5' ,'' as BillType6,'' as 'RES.AMOUNT NO.-6' ,'' as BillType7,'' as 'RES.AMOUNT NO.-7' ,'' as BillType8,'' as 'RES.AMOUNT NO.-8' ,'' as BillType9,'' as 'RES.AMOUNT NO.-9' ,'' as Bank
    ,'' as 'RES DATE NO.-1','' as 'RES DATE NO.-2' ,'' as 'RES DATE NO.-3' ,'' as 'RES DATE NO.-4' ,'' as 'RES DATE NO.-5' ,'' as 'RES DATE NO.-6' ,'' as 'RES DATE NO.-7' ,'' as 'RES DATE NO.-8' ,'' as 'RES DATE NO.-9' 
    ,'' as 'Balance',Remark from newSpView where 1=1  and partyname is not null  " + sComp + sBranch + sBuyer + sType + sCommodity + sVarityid + sLotNo + "", con);
        cmdL.CommandTimeout = 5000;
        da = new SqlDataAdapter(cmdL);
        da.Fill(dt);
        cmdL.Dispose();
        return dt;
    }
    //Irfan 27-04-2016

    //public void InstblClaim(ProbClaimDetail prop)
    //{
    //    GetConection();
    //    cmd = new SqlCommand();
    //    cmd.CommandText = "InsClaimDetail";
    //    cmd.Connection = con;
    //    cmd.CommandType = CommandType.StoredProcedure;
    //    dt = new DataTable();
    //    da = new SqlDataAdapter();
    //    cmd.Parameters.AddWithValue("@mstcode", prop.mstcode);
    //    cmd.Parameters.AddWithValue("@CompCode", prop.CompCode);
    //    cmd.Parameters.AddWithValue("@MstDate", prop.MstDate);
    //    cmd.Parameters.AddWithValue("@StationId", prop.StationId);
    //    cmd.Parameters.AddWithValue("@Pol_From", prop.Pol_From);
    //    cmd.Parameters.AddWithValue("@Pol_To", prop.Pol_To);
    //    cmd.Parameters.AddWithValue("@CoverNo", prop.CoverNo);
    //    cmd.Parameters.AddWithValue("@PolicyNo", prop.PolicyNo);
    //    cmd.Parameters.AddWithValue("@InsurrAmt", prop.InsurrAmt);
    //    cmd.Parameters.AddWithValue("@Location", prop.Location);
    //    cmd.Parameters.AddWithValue("@Premium ", prop.Premium);
    //    cmd.Parameters.AddWithValue("@InsuComp", prop.InsuComp);
    //    cmd.Parameters.AddWithValue("@Remark", prop.Remark);
    //    cmd.Parameters.AddWithValue("@insgroup", prop.insgroup);
    //    cmd.Parameters.AddWithValue("@Status", prop.Status);
    //    cmd.Parameters.AddWithValue("@ClaimCode", prop.ClaimCode);
    //    cmd.Parameters.AddWithValue("@ClaimDate", prop.ClaimDate);
    //    cmd.Parameters.AddWithValue("@LossAmt", prop.LossAmt);
    //    cmd.Parameters.AddWithValue("@ClaimType", prop.ClaimType);
    //    cmd.Parameters.AddWithValue("@ClaimNo", prop.ClaimNo);
    //    cmd.Parameters.AddWithValue("@ClaimAmt", prop.ClaimAmt);
    //    cmd.Parameters.AddWithValue("@ClaimRemark", prop.ClaimRemark);
    //    cmd.Parameters.AddWithValue("@DocSubmit", prop.DocSubmit);
    //    cmd.ExecuteNonQuery();
    //}

    //public void InsTranBillMst(propPaymentEnt prop)
    //{
    //    GetConection();
    //    cmd = new SqlCommand();
    //    cmd.CommandText = "InsTranBillMst";
    //    cmd.Connection = con;
    //    cmd.CommandType = CommandType.StoredProcedure;
    //    dt = new DataTable();
    //    da = new SqlDataAdapter();
    //    cmd.Parameters.AddWithValue("@MstCode", prop.mstCode);
    //    cmd.Parameters.AddWithValue("@MstDate", prop.mstDate);
    //    cmd.Parameters.AddWithValue("@SrNo", prop.SrNo);
    //    cmd.Parameters.AddWithValue("@TransportID", prop.iTranportID);
    //    cmd.Parameters.AddWithValue("@BillID", prop.iBillID);
    //    cmd.Parameters.AddWithValue("@BillAmt", prop.dAmount);
    //    cmd.Parameters.AddWithValue("@Remark", prop.Remark);
    //    cmd.Parameters.AddWithValue("@BillNo", prop.sBillNo);
    //    cmd.Parameters.AddWithValue("@tpCode", prop.tpCode);
    //    cmd.Parameters.AddWithValue("@LrNo", prop.sLrNo);
    //    cmd.Parameters.AddWithValue("@CompCode", prop.tpCompCode);
    //    cmd.Parameters.AddWithValue("@BranchID", prop.tpBrnchID);

    //    cmd.ExecuteNonQuery();
    //}

    //public void InsQuatMst(propSauda prop)
    //{
    //    GetConection();
    //    cmd = new SqlCommand();
    //    cmd.CommandText = "InsQuatMst";
    //    cmd.Connection = con;
    //    cmd.CommandType = CommandType.StoredProcedure;
    //    dt = new DataTable();
    //    da = new SqlDataAdapter();
    //    cmd.Parameters.AddWithValue("@MstCode", prop.mstCode);
    //    cmd.Parameters.AddWithValue("@MstDate", prop.mstDate);
    //    cmd.Parameters.AddWithValue("@FromSt", prop.iFrom);
    //    cmd.Parameters.AddWithValue("@ToSt", prop.iTo);
    //    cmd.Parameters.AddWithValue("@TransportID", prop.iTranportID);
    //    cmd.Parameters.AddWithValue("@Rate", prop.Rate);
    //    cmd.Parameters.AddWithValue("@Status", prop.isFinal);
    //    cmd.Parameters.AddWithValue("@Remark", prop.Remark);
    //    cmd.ExecuteNonQuery();
    //}

    //public bool InsUserWork(propSauda oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsUserWork", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@woruser", SqlDbType.Int).Value = oProp.iUseCode;
    //    objcmd.Parameters.Add("@wormode", SqlDbType.Int).Value = oProp.iMode;
    //    objcmd.Parameters.Add("@worcomp", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@wortype", SqlDbType.Int).Value = oProp.iRentType;
    //    objcmd.Parameters.Add("@worcode", SqlDbType.Int).Value = oProp.iMstCode;
    //    objcmd.Parameters.Add("@worsrno", SqlDbType.VarChar).Value = oProp.sServiceNo;
    //    objcmd.Parameters.Add("@worrema", SqlDbType.VarChar).Value = oProp.sRemark;
    //    objcmd.Parameters.Add("@worrfsr", SqlDbType.VarChar).Value = "0";
    //    objcmd.Parameters.Add("@worsysn", SqlDbType.VarChar).Value = oProp.sSysName;
    //    objcmd.Parameters.Add("@wordate", SqlDbType.DateTime).Value = oProp.dDate;
    //    objcmd.Parameters.Add("@worcudt", SqlDbType.DateTime).Value = DateTime.Now;
    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}
    //public int InsGodown(propSauda prop)
    //{
    //    GetConection();
    //    cmd = new SqlCommand();
    //    cmd.CommandText = "InsGodown";
    //    cmd.Connection = con;
    //    cmd.CommandType = CommandType.StoredProcedure;
    //    dt = new DataTable();
    //    da = new SqlDataAdapter();

    //    cmd.Parameters.AddWithValue("@compcode", prop.Compcode);
    //    cmd.Parameters.AddWithValue("@gododesc", prop.sName);
    //    cmd.Parameters.AddWithValue("@godoaddr", prop.sAddress);
    //    cmd.Parameters.AddWithValue("@GodoCity", prop.citycode);

    //    da.SelectCommand = cmd;
    //    da.Fill(dt);
    //    if (dt.Rows.Count > 0) return Convert.ToInt32(dt.Rows[0][0].ToString());
    //    else return 0;
    //}



    //public bool InsUserWork(propPayroll oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsUserWork", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@woruser", SqlDbType.Int).Value = oProp.iUseCode;
    //    objcmd.Parameters.Add("@wormode", SqlDbType.Int).Value = oProp.iMode;
    //    objcmd.Parameters.Add("@worcomp", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@wortype", SqlDbType.Int).Value = oProp.iRentType;
    //    objcmd.Parameters.Add("@worcode", SqlDbType.Int).Value = oProp.iMstCode;
    //    objcmd.Parameters.Add("@worsrno", SqlDbType.VarChar).Value = oProp.sServiceNo;
    //    objcmd.Parameters.Add("@worrema", SqlDbType.VarChar).Value = oProp.sRemark;
    //    objcmd.Parameters.Add("@worrfsr", SqlDbType.VarChar).Value = "0";
    //    objcmd.Parameters.Add("@worsysn", SqlDbType.VarChar).Value = oProp.sSysName;
    //    objcmd.Parameters.Add("@wordate", SqlDbType.DateTime).Value = oProp.dDate;
    //    objcmd.Parameters.Add("@worcudt", SqlDbType.DateTime).Value = DateTime.Now;


    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}
    //public bool InsEmployee(propPayroll oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsEmployee", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@emparea", SqlDbType.Int).Value = oProp.iEmp_emparea;
    //    objcmd.Parameters.Add("@empbudget", SqlDbType.Int).Value = oProp.iEmp_empbudget;
    //    objcmd.Parameters.Add("@empcate", SqlDbType.Int).Value = oProp.iEmp_empcate;
    //    objcmd.Parameters.Add("@empcity", SqlDbType.Int).Value = oProp.iEmp_empcity;
    //    objcmd.Parameters.Add("@empcode", SqlDbType.Int).Value = oProp.iEmp_empcode;
    //    objcmd.Parameters.Add("@empdsa", SqlDbType.Int).Value = oProp.iEmp_empdsa;
    //    objcmd.Parameters.Add("@empdsap", SqlDbType.Int).Value = oProp.iEmp_empdsap;
    //    objcmd.Parameters.Add("@empgrou", SqlDbType.Int).Value = oProp.iEmp_empgrou;
    //    objcmd.Parameters.Add("@empincome", SqlDbType.Int).Value = oProp.iEmp_empincome;
    //    objcmd.Parameters.Add("@empitrd", SqlDbType.Int).Value = oProp.iEmp_empitrd;
    //    objcmd.Parameters.Add("@empledg", SqlDbType.Int).Value = oProp.iEmp_empledg;
    //    objcmd.Parameters.Add("@empnomi", SqlDbType.Int).Value = oProp.iEmp_empnomi;
    //    objcmd.Parameters.Add("@empprty", SqlDbType.Int).Value = oProp.iEmp_empprty;

    //    objcmd.Parameters.Add("@empaddr", SqlDbType.VarChar).Value = oProp.sEmp_empaddr;
    //    objcmd.Parameters.Add("@empalia", SqlDbType.VarChar).Value = oProp.sEmp_empalia;
    //    objcmd.Parameters.Add("@emphind", SqlDbType.NVarChar).Value = oProp.sEmp_emphind;
    //    objcmd.Parameters.Add("@empname", SqlDbType.VarChar).Value = oProp.sEmp_empname;
    //    objcmd.Parameters.Add("@emppath", SqlDbType.VarChar).Value = oProp.sEmp_emppath;
    //    objcmd.Parameters.Add("@empphon", SqlDbType.VarChar).Value = oProp.sEmp_empphon;
    //    objcmd.Parameters.Add("@emppinc", SqlDbType.VarChar).Value = oProp.sEmp_emppinc;
    //    objcmd.Parameters.Add("@emprefn", SqlDbType.VarChar).Value = oProp.sEmp_emprefn;
    //    objcmd.Parameters.Add("@emprema", SqlDbType.VarChar).Value = oProp.sEmp_emprema;
    //    objcmd.Parameters.Add("@empsort", SqlDbType.VarChar).Value = oProp.sEmp_empsort;
    //    objcmd.Parameters.Add("@empstat", SqlDbType.VarChar).Value = oProp.sEmp_empstat;


    //    objcmd.Parameters.Add("@acwithbl", SqlDbType.VarChar).Value = oProp.dEmp_acwithbl;
    //    objcmd.Parameters.Add("@empagen", SqlDbType.VarChar).Value = oProp.dEmp_empagen;
    //    objcmd.Parameters.Add("@empclcr", SqlDbType.VarChar).Value = oProp.dEmp_empclcr;
    //    objcmd.Parameters.Add("@empcldr", SqlDbType.VarChar).Value = oProp.dEmp_empcldr;
    //    objcmd.Parameters.Add("@empjmbl", SqlDbType.VarChar).Value = oProp.dEmp_empjmbl;
    //    objcmd.Parameters.Add("@empopcr", SqlDbType.VarChar).Value = oProp.dEmp_empopcr;
    //    objcmd.Parameters.Add("@empopdr", SqlDbType.VarChar).Value = oProp.dEmp_empopdr;
    //    objcmd.Parameters.Add("@emprate", SqlDbType.VarChar).Value = oProp.dEmp_emprate;

    //    objcmd.Parameters.Add("@PAN", SqlDbType.VarChar).Value = oProp.sEmp_PAN;
    //    objcmd.Parameters.Add("@ESI", SqlDbType.VarChar).Value = oProp.sEmp_ESI;
    //    objcmd.Parameters.Add("@IDProof", SqlDbType.VarChar).Value = oProp.sEmp_IDProof;
    //    objcmd.Parameters.Add("@ProofNo", SqlDbType.VarChar).Value = oProp.sEmp_ProofNo;
    //    objcmd.Parameters.Add("@AcNo", SqlDbType.VarChar).Value = oProp.sEmp_AcNo;
    //    objcmd.Parameters.Add("@PfAcNo", SqlDbType.VarChar).Value = oProp.sEmp_PfAcNo;
    //    objcmd.Parameters.Add("@BankID", SqlDbType.Int).Value = oProp.iEmp_BankID;
    //    objcmd.Parameters.Add("@pfBankID", SqlDbType.Int).Value = oProp.iEmp_pfBankID;

    //    objcmd.Parameters.Add("@Gender", SqlDbType.Int).Value = oProp.iEmp_Gender;
    //    objcmd.Parameters.Add("@CastID", SqlDbType.Int).Value = oProp.iEmp_CastID;
    //    objcmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = oProp.iEmp_CategoryID;

    //    objcmd.Parameters.Add("@BirthDt", SqlDbType.DateTime).Value = oProp.dEAcc_BirthDt;
    //    objcmd.Parameters.Add("@AnvDt", SqlDbType.DateTime).Value = oProp.dEAcc_AnvDt;
    //    objcmd.Parameters.Add("@VetanMaan", SqlDbType.VarChar).Value = oProp.sEmp_VetanMaan;

    //    objcmd.Parameters.Add("@BranchID", SqlDbType.VarChar).Value = oProp.iEmp_Branch;

    //    objcmd.Parameters.Add("@Official_No", SqlDbType.VarChar).Value = oProp.sOfficial_No;
    //    objcmd.Parameters.Add("@Residential_No", SqlDbType.VarChar).Value = oProp.sResidential_No;
    //    objcmd.Parameters.Add("@ReferenceNm", SqlDbType.VarChar).Value = oProp.sReferenceNm;
    //    objcmd.Parameters.Add("@ReferenceNo", SqlDbType.VarChar).Value = oProp.sReferenceNo;
    //    objcmd.Parameters.Add("@FatherNo", SqlDbType.VarChar).Value = oProp.sFatherNo;
    //    objcmd.Parameters.Add("@TemporaryAdd", SqlDbType.VarChar).Value = oProp.sTemporaryAdd;
    //    objcmd.Parameters.Add("@PermanentAdd", SqlDbType.VarChar).Value = oProp.sPermanentAdd;
    //    objcmd.Parameters.Add("@Old_PF_No", SqlDbType.VarChar).Value = oProp.sOld_PF_No;
    //    objcmd.Parameters.Add("@UNA_No", SqlDbType.VarChar).Value = oProp.sUNA_No;
    //    objcmd.Parameters.Add("@EmailID", SqlDbType.VarChar).Value = oProp.sEmailID;
    //    objcmd.Parameters.Add("@BloodGroup", SqlDbType.VarChar).Value = oProp.sBloodGroup;


    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}

    //public int InsStud(propPayroll oProp)
    //{
    //    dt = new DataTable();
    //    SqlCommand cmd = null;
    //    if (dr != null)
    //        dr.Close();
    //    GetConection();

    //    cmd = new SqlCommand("InsStud", con);
    //    cmd.CommandType = CommandType.StoredProcedure;
    //    da = new SqlDataAdapter(cmd);

    //    cmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.iArv_Type;
    //    cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = oProp.sArv_Name;
    //    cmd.Parameters.Add("@Hname", SqlDbType.NVarChar).Value = oProp.sArv_HName;

    //    da.Fill(dt);
    //    cmd.Dispose();
    //    if (dt.Rows.Count > 0)
    //        return Convert.ToInt32(dt.Rows[0][0].ToString());
    //    else
    //        return 0;

    //}
    //public bool InsEmpAcc(propPayroll oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsEmpAcc", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@acctcode", SqlDbType.Int).Value = oProp.iEAcc_acctcode;
    //    objcmd.Parameters.Add("@emposrno", SqlDbType.Int).Value = oProp.iEAcc_emposrno;
    //    objcmd.Parameters.Add("@empopano", SqlDbType.Int).Value = oProp.iEAcc_empopano;
    //    objcmd.Parameters.Add("@empobuhe", SqlDbType.Int).Value = oProp.iEAcc_empobuhe;
    //    objcmd.Parameters.Add("@empopost", SqlDbType.Int).Value = oProp.iEAcc_empopost;
    //    objcmd.Parameters.Add("@empodesg", SqlDbType.Int).Value = oProp.iEAcc_empodesg;
    //    objcmd.Parameters.Add("@empoccla", SqlDbType.Int).Value = oProp.iEAcc_empoccla;
    //    objcmd.Parameters.Add("@empbnknm", SqlDbType.Int).Value = oProp.iEAcc_empbnknm;
    //    objcmd.Parameters.Add("@empovarg", SqlDbType.Int).Value = oProp.iEAcc_empovarg;
    //    objcmd.Parameters.Add("@emppfbnk", SqlDbType.Int).Value = oProp.iEAcc_emppfbnk;
    //    objcmd.Parameters.Add("@empovvht", SqlDbType.Int).Value = oProp.iEAcc_empovvht;
    //    objcmd.Parameters.Add("@emposexd", SqlDbType.Int).Value = oProp.iEAcc_emposexd;
    //    objcmd.Parameters.Add("@empidpfc", SqlDbType.Int).Value = oProp.iEAcc_empidpfc;

    //    objcmd.Parameters.Add("@empoaddr", SqlDbType.VarChar).Value = oProp.sEAcc_empoaddr;
    //    objcmd.Parameters.Add("@empobano", SqlDbType.VarChar).Value = oProp.sEAcc_empobano;
    //    objcmd.Parameters.Add("@empogpno", SqlDbType.VarChar).Value = oProp.sEAcc_empogpno;
    //    objcmd.Parameters.Add("@empocpno", SqlDbType.VarChar).Value = oProp.sEAcc_empocpno;
    //    objcmd.Parameters.Add("@empopann", SqlDbType.VarChar).Value = oProp.sEAcc_empopann;
    //    objcmd.Parameters.Add("@empidprf", SqlDbType.VarChar).Value = oProp.sEAcc_empidprf;
    //    objcmd.Parameters.Add("@empesino", SqlDbType.VarChar).Value = oProp.sEAcc_empesino;
    //    objcmd.Parameters.Add("@empprfno", SqlDbType.VarChar).Value = oProp.sEAcc_empprfno;

    //    objcmd.Parameters.Add("@empojodt", SqlDbType.DateTime).Value = oProp.dEAcc_empojodt;
    //    objcmd.Parameters.Add("@empobidt", SqlDbType.DateTime).Value = oProp.dEAcc_empobidt;
    //    objcmd.Parameters.Add("@empoindt", SqlDbType.DateTime).Value = oProp.dEAcc_empoindt;
    //    objcmd.Parameters.Add("@empodoan", SqlDbType.DateTime).Value = oProp.dEAcc_empodoan;

    //    objcmd.Parameters.Add("@empolicn", SqlDbType.Money).Value = oProp.dEAcc_empolicn;
    //    objcmd.Parameters.Add("@empobasl", SqlDbType.Money).Value = oProp.dEAcc_empobasl;

    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}

    //public bool insSapuTrn(propPayroll oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("insSapuTrn", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@trntype", SqlDbType.Int).Value = oProp.iSapuTrn_trntype;
    //    objcmd.Parameters.Add("@trncode", SqlDbType.Int).Value = oProp.iSapuTrn_trncode;
    //    objcmd.Parameters.Add("@trnitem", SqlDbType.Int).Value = oProp.iSapuTrn_trnitem;
    //    objcmd.Parameters.Add("@trnrefc", SqlDbType.Int).Value = oProp.iSapuTrn_trnrefc;
    //    objcmd.Parameters.Add("@trnshow", SqlDbType.Int).Value = oProp.iSapuTrn_trnshow;
    //    objcmd.Parameters.Add("@trnsrno", SqlDbType.Int).Value = oProp.iSapuTrn_trnsrno;

    //    objcmd.Parameters.Add("@trnadju", SqlDbType.Money).Value = oProp.dSapuTrn_trnadju;
    //    objcmd.Parameters.Add("@trndram", SqlDbType.Money).Value = oProp.dSapuTrn_trndram;
    //    objcmd.Parameters.Add("@trncram", SqlDbType.Money).Value = oProp.dSapuTrn_trncram;
    //    objcmd.Parameters.Add("@trnexpa", SqlDbType.Money).Value = oProp.dSapuTrn_trnexpa;
    //    objcmd.Parameters.Add("@trnaddv", SqlDbType.Money).Value = oProp.dSapuTrn_trnaddv;
    //    objcmd.Parameters.Add("@trnactpay", SqlDbType.Money).Value = oProp.dSapuTrn_trnactpay;

    //    objcmd.Parameters.Add("@trnrema", SqlDbType.VarChar).Value = oProp.sSapuTrn_trnrema;

    //    objcmd.Parameters.Add("@trnrefd", SqlDbType.DateTime).Value = oProp.dSapuTrn_trnrefd;
    //    objcmd.Parameters.Add("@trndate", SqlDbType.DateTime).Value = oProp.dSapuTrn_trndate;

    //    objcmd.ExecuteNonQuery();
    //    return true;

    //}
    //public bool UpdCodeGen(propPayroll oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("UpdCodeGen", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@comp", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@Type", SqlDbType.Int).Value = oProp.iTypee_opbal;
    //    objcmd.Parameters.Add("@Prev", SqlDbType.Int).Value = oProp.iPrev_opbal;
    //    objcmd.Parameters.Add("@Curr", SqlDbType.Int).Value = oProp.iCurr_opbal;
    //    objcmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = oProp.dDatee_opbal;
    //    objcmd.Parameters.Add("@StDate", SqlDbType.DateTime).Value = oProp.dStDate;
    //    objcmd.Parameters.Add("@LsDate", SqlDbType.DateTime).Value = oProp.dLastDate;
    //    objcmd.ExecuteNonQuery();
    //    return true;
    //}
    //public bool InsSapuMst(propPayroll oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InsSapuMst", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@msttype", SqlDbType.Int).Value = oProp.iSapuMst_msttype;
    //    objcmd.Parameters.Add("@mstcode", SqlDbType.Int).Value = oProp.iSapuMst_mstcode;
    //    objcmd.Parameters.Add("@mstsite", SqlDbType.Int).Value = oProp.iSapuMst_mstsite;
    //    objcmd.Parameters.Add("@mstcust", SqlDbType.Int).Value = oProp.iSapuMst_mstcust;
    //    objcmd.Parameters.Add("@mstlate", SqlDbType.Money).Value = oProp.iSapuMst_mstlate;
    //    objcmd.Parameters.Add("@mstAppr", SqlDbType.Int).Value = oProp.iSapuMst_mstAppr;
    //    objcmd.Parameters.Add("@mstptcode", SqlDbType.Int).Value = oProp.iSapuMst_mstptcode;

    //    objcmd.Parameters.Add("@mstrema", SqlDbType.VarChar).Value = oProp.sSapuMst_mstrema;
    //    objcmd.Parameters.Add("@mstrefc", SqlDbType.VarChar).Value = oProp.sSapuMst_mstrefc;
    //    objcmd.Parameters.Add("@mstchno", SqlDbType.VarChar).Value = oProp.sSapuMst_mstchno;
    //    objcmd.Parameters.Add("@mststatus", SqlDbType.VarChar).Value = oProp.sSapuMst_mststatus;

    //    objcmd.Parameters.Add("@msttota", SqlDbType.Money).Value = oProp.dSapuMst_msttota;
    //    objcmd.Parameters.Add("@mstneta", SqlDbType.Money).Value = oProp.dSapuMst_mstneta;
    //    objcmd.Parameters.Add("@mstpdc", SqlDbType.Float).Value = oProp.dSapuMst_mstpdc;
    //    objcmd.Parameters.Add("@oldwht", SqlDbType.Float).Value = oProp.dSapuMst_oldwht;
    //    objcmd.Parameters.Add("@oldamt", SqlDbType.Float).Value = oProp.dSapuMst_oldamt;

    //    objcmd.Parameters.Add("@mstdate", SqlDbType.DateTime).Value = oProp.dSapuMst_mstdate;
    //    objcmd.Parameters.Add("@mstcfdt", SqlDbType.DateTime).Value = oProp.dSapuMst_mstcfdt;

    //    objcmd.ExecuteNonQuery();
    //    return true;

    //}
    //public void FillDropDownList(DropDownList ddlName, string TableName, string TextField, string ValueField, string Condition = "", string join = "", string tooltip = "", string order = "", bool addDistinct = true, bool addSelect = true)
    //{
    //    ddlName.Items.Clear();
    //    DataTable dt;
    //    string newname = "";
    //    if (TextField == ValueField)
    //        newname = "" + TextField + "";
    //    else
    //        newname = TextField;

    //    if (Condition != "")
    //        Condition = " Where " + Condition;

    //    if (order == "")
    //        order = "text,value";

    //    string dist = "";
    //    if (addDistinct)
    //        dist = " distinct ";

    //    ddlName.DataSource = dt = GetData("select " + dist + ValueField + " as value,cast(" + newname + " as nvarchar(max)) as text from " + TableName + " " + join + " " + Condition + " order by " + order);
    //    ddlName.DataTextField = "text";
    //    ddlName.DataValueField = "value";
    //    ddlName.DataBind();
    //    if (addSelect)
    //        ddlName.Items.Insert(0, new ListItem("--Select--", "0"));
    //}

    //public void InsPlanMst(propPayroll oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd, objcmd1, objcmd2;
    //    if (oProp.dtPlanTrn != null && oProp.dtPlanTrn.Rows.Count > 0)
    //    {
    //        for (int i = 0; i < oProp.dtPlanTrn.Rows.Count; i++)
    //        {
    //            objcmd = new SqlCommand("sp_IUDPlanMst", con);
    //            objcmd.CommandType = CommandType.StoredProcedure;
    //            objcmd.Parameters.AddWithValue("@compcode", oProp.iCompCode);
    //            objcmd.Parameters.AddWithValue("@msttype", oProp.msttype);
    //            objcmd.Parameters.AddWithValue("@mstcode", oProp.mstcode);
    //            objcmd.Parameters.AddWithValue("@mstdate", oProp.mstdate);
    //            objcmd.Parameters.AddWithValue("@msttota", oProp.msttota);
    //            objcmd.Parameters.AddWithValue("@mstrema", oProp.mstrema);
    //            objcmd.Parameters.AddWithValue("@msttotb", oProp.msttotb);
    //            objcmd.Parameters.AddWithValue("@mstdesg", oProp.mstdesg);
    //            objcmd.Parameters.AddWithValue("@mstcate", oProp.mstcate);
    //            objcmd.Parameters.AddWithValue("@mstempo", oProp.mstempo);
    //            objcmd.Parameters.AddWithValue("@mstapdt", oProp.mstapdt);
    //            objcmd.Parameters.AddWithValue("@mstlvbsc", oProp.mstlvbsc);
    //            objcmd.Parameters.AddWithValue("@mstlvbase", oProp.mstlvbase);
    //            objcmd.Parameters.AddWithValue("@mstshow", oProp.mstshow);
    //            objcmd.Parameters.AddWithValue("@msthour", oProp.msthour);
    //            objcmd.Parameters.AddWithValue("@mstlvsy", oProp.mstlvsy);
    //            objcmd.Parameters.AddWithValue("@mstlvsm", oProp.mstlvsm);
    //            objcmd.Parameters.AddWithValue("@mstincd", oProp.mstincd);
    //            objcmd.Parameters.AddWithValue("@mstHindirema", oProp.mstHindirema);
    //            objcmd.Parameters.AddWithValue("@trnitem", oProp.dtPlanTrn.Rows[i]["mstHeadId"]);
    //            objcmd.Parameters.AddWithValue("@trnrema", oProp.dtPlanTrn.Rows[i]["mstRemark"]);
    //            if (oProp.dtPlanTrn.Rows[i]["mstHeadId"].ToString() != "213")
    //            {
    //                objcmd.Parameters.AddWithValue("@trnamt1", oProp.dtPlanTrn.Rows[i]["mstRate"]);
    //                objcmd.Parameters.AddWithValue("@trnamt2", oProp.dtPlanTrn.Rows[i]["mstAmount"]);
    //            }
    //            else
    //            {
    //                objcmd.Parameters.AddWithValue("@trnamt1", oProp.dtPlanTrn.Rows[i]["mstAmount"]);
    //                objcmd.Parameters.AddWithValue("@trnamt2", oProp.dtPlanTrn.Rows[i]["mstRate"]);
    //            }
    //            objcmd.Parameters.AddWithValue("@trnoppa", "");
    //            objcmd.Parameters.AddWithValue("@trnshow", "");
    //            objcmd.Parameters.AddWithValue("@trnIncrper", oProp.dtPlanTrn.Rows[i]["mstIncr"]);
    //            objcmd.Parameters.AddWithValue("@trnamtd", oProp.dtPlanTrn.Rows[i]["trnamtd"]);
    //            objcmd.ExecuteNonQuery();


    //        }
    //        if (oProp.dtPlanDet != null && oProp.dtPlanDet.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < oProp.dtPlanDet.Rows.Count; i++)
    //            {
    //                objcmd1 = new SqlCommand("sp_IUDPlanDet", con);
    //                objcmd1.CommandType = CommandType.StoredProcedure;
    //                objcmd1.Parameters.AddWithValue("@compcode", oProp.iCompCode);
    //                objcmd1.Parameters.AddWithValue("@detcode", oProp.mstcode);
    //                objcmd1.Parameters.AddWithValue("@dettype", oProp.msttype);
    //                objcmd1.Parameters.AddWithValue("@detdate", oProp.mstdate);
    //                objcmd1.Parameters.AddWithValue("@detitem", oProp.dtPlanDet.Rows[i]["detitem"]);
    //                objcmd1.Parameters.AddWithValue("@detsumo", oProp.dtPlanDet.Rows[i]["detsumo"]);
    //                //objcmd1.Parameters.AddWithValue("@detrpcd", oProp.dtPlanDet.Rows[i]["detrpcd"]);
    //                objcmd1.ExecuteNonQuery();
    //            }
    //        }
    //        //{sourabh 10-jun-2015
    //        objcmd2 = new SqlCommand("sp_updPlanTrn", con);
    //        objcmd2.CommandType = CommandType.StoredProcedure;
    //        objcmd2.Parameters.AddWithValue("@compcode", oProp.iCompCode);
    //        objcmd2.Parameters.AddWithValue("@trncode", oProp.mstcode);
    //        objcmd2.ExecuteNonQuery();
    //        //}sourabh 10-jun-2015
    //    }
    //}
    //public bool stp_updBasicPay(propPayroll oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("stp_updBasicPay", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    // @pCompCode=116, =21, @pUpdHeadC=213, @pUpdOnWht='213, 2204', @pAddOrSub='+', @pOperator='%', @pChgValue=10, @pMstCodeV=1221

    //    objcmd.Parameters.Add("@pCompCode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@pMstTypeV", SqlDbType.Int).Value = oProp.iMstType;
    //    objcmd.Parameters.Add("@pUpdHeadC", SqlDbType.Int).Value = oProp.iUpdHeadC;
    //    objcmd.Parameters.Add("@pUpdOnWht", SqlDbType.VarChar).Value = oProp.sUpdOnWht;
    //    objcmd.Parameters.Add("@pAddOrSub", SqlDbType.VarChar).Value = oProp.sAddOrSub;
    //    objcmd.Parameters.Add("@pOperator", SqlDbType.VarChar).Value = oProp.sOperator;
    //    objcmd.Parameters.Add("@pChgValue", SqlDbType.Int).Value = oProp.iChgValue;
    //    objcmd.Parameters.Add("@pMstCodeV", SqlDbType.Int).Value = oProp.iMstCodeV;

    //    objcmd.ExecuteNonQuery();
    //    return true;

    //}



    public bool DelAccountInfo(int CompCode, int AcctCode)
    {
        //commented on 12-10-2016
        //if (aOpenCloseConnection == true) c.GetConection();
        ////oDbInterface = new dbInterface();
        ////oDbInterface.dbConnOpen();
        GetConection();
        // c.con.Open();
        cmd = new SqlCommand();
        cmd.CommandText = "spDelAccountInfo";
        cmd.Connection = con; //oDbInterface.dbGetConn();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CompCode", CompCode);
        cmd.Parameters.AddWithValue("@AcctCode", AcctCode);
        cmd.ExecuteNonQuery();
        //commented con.close and written dispose on 12-10-2016
        //cmd.Dispose();
        //c.con.Close();
        //oDbInterface.dbConnClose();
        return true;
    }














    //public bool stp_salGetChildHds(propPayroll oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("stp_salGetChildHds", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    //    exec stp_salGetChildHds @pCompCode = 116, @pMstTypeV = 21, @pUpdHeadC = 213, @pMstCodeV = 1221

    //    objcmd.Parameters.Add("@pCompCode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@pMstTypeV", SqlDbType.Int).Value = oProp.iMstType;
    //    objcmd.Parameters.Add("@pUpdHeadC", SqlDbType.Int).Value = oProp.iUpdHeadC;
    //    objcmd.Parameters.Add("@pMstCodeV", SqlDbType.Int).Value = oProp.iMstCodeV;
    //    objcmd.CommandTimeout = 300;
    //    objcmd.ExecuteNonQuery();
    //    return true;

    //}
    public bool UdpCode(int iCompCode, int iType, int iPrev, int iCurr)
    {
        GetConection();
        SqlCommand objcmd = new SqlCommand("UdpCode", con);
        objcmd.CommandType = CommandType.StoredProcedure;
        DataTable dt1 = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter();

        objcmd.Parameters.Add("@compcode", SqlDbType.Int).Value = iCompCode;
        objcmd.Parameters.Add("@codetype", SqlDbType.Int).Value = iType;
        objcmd.Parameters.Add("@codeprev", SqlDbType.Int).Value = iPrev;
        objcmd.Parameters.Add("@codecurr", SqlDbType.Int).Value = iCurr;

        objcmd.ExecuteNonQuery();
        return true;
    }
    //public bool InstEmpLoan(propPayroll oProp)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("InstEmpLoan", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    DataTable dt1 = new DataTable();
    //    SqlDataAdapter da = new SqlDataAdapter();

    //    objcmd.Parameters.Add("@CompCode", SqlDbType.Int).Value = oProp.iCompCode;
    //    objcmd.Parameters.Add("@Code", SqlDbType.Int).Value = oProp.iEmpLoan_Code;
    //    objcmd.Parameters.Add("@CR", SqlDbType.Int).Value = oProp.iEmpLoan_CR;
    //    objcmd.Parameters.Add("@DR", SqlDbType.Int).Value = oProp.iEmpLoan_DR;
    //    objcmd.Parameters.Add("@Mode", SqlDbType.Int).Value = oProp.iEmpLoan_Mode;
    //    objcmd.Parameters.Add("@EmpCode", SqlDbType.Int).Value = oProp.iEmpLoan_EmpCode;
    //    objcmd.Parameters.Add("@Installment", SqlDbType.Int).Value = oProp.iEmpLoan_Installment;
    //    objcmd.Parameters.Add("@PaidInstall", SqlDbType.Int).Value = oProp.iEmpLoan_PaidInstall;
    //    objcmd.Parameters.Add("@BalInst", SqlDbType.Int).Value = oProp.iEmpLoan_BalInst;
    //    objcmd.Parameters.Add("@AdjuIntallment", SqlDbType.Int).Value = oProp.iEmpLoan_AdjuIntallment;


    //    objcmd.Parameters.Add("@Amount", SqlDbType.Money).Value = oProp.dEmpLoan_Amount;
    //    objcmd.Parameters.Add("@Interest", SqlDbType.Money).Value = oProp.dEmpLoan_Interest;
    //    objcmd.Parameters.Add("@interestAmt", SqlDbType.Money).Value = oProp.dEmpLoan_interestAmt;
    //    objcmd.Parameters.Add("@Total", SqlDbType.Money).Value = oProp.dEmpLoan_Total;
    //    objcmd.Parameters.Add("@PaidAmt", SqlDbType.Money).Value = oProp.dEmpLoan_PaidAmt;
    //    objcmd.Parameters.Add("@InstallmentAmt", SqlDbType.Money).Value = oProp.dEmpLoan_InstallmentAmt;
    //    objcmd.Parameters.Add("@BalAmt", SqlDbType.Money).Value = oProp.dEmpLoan_BalAmt;
    //    objcmd.Parameters.Add("@AdjuAmt", SqlDbType.Money).Value = oProp.dEmpLoan_AdjuAmt;

    //    objcmd.Parameters.Add("@Challan", SqlDbType.VarChar).Value = oProp.sEmpLoan_Challan;
    //    objcmd.Parameters.Add("@remark", SqlDbType.VarChar).Value = oProp.sEmpLoan_remark;

    //    objcmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = oProp.dEmpLoan_Date;
    //    objcmd.Parameters.Add("@FromMonth", SqlDbType.DateTime).Value = oProp.dEmpLoan_FromMonth;
    //    objcmd.Parameters.Add("@LoanMonth", SqlDbType.DateTime).Value = oProp.dEmpLoan_LoanMonth;
    //    objcmd.Parameters.Add("@CheckNo", SqlDbType.VarChar).Value = oProp.sCheckNo;//sourabh 12-jun-2015
    //    objcmd.ExecuteNonQuery();
    //    return true;

    //}

    //Irfan 06-07-2016
    GridView gr;
    //HttpResponse Response;

    public DataTable Grid2DT(GridView gv)
    {
        DataTable _datatable = new DataTable();
        for (int i = 0; i < gv.Columns.Count; i++)
        {
            if (gv.Columns[i].HeaderText.ToString() != "")
                _datatable.Columns.Add(gv.Columns[i].HeaderText.ToString());

        }
        DataRow dr;
        foreach (GridViewRow row in gv.Rows)
        {
            dr = _datatable.NewRow();
            for (int j = 0; j < gv.Columns.Count; j++)
            {
                if (gv.Columns[j].HeaderText.ToString() == "")
                    continue;

                if (!row.Cells[j].Text.Equals("&nbsp;"))
                    dr[gv.Columns[j].HeaderText.ToString()] = row.Cells[j].Text;


                if (!row.Cells[j].Text.Equals("&nbsp;") && row.Cells[j].Text.Length > 12 && row.Cells[j].Text.IndexOf(",") < 0 && row.Cells[j].Text.IndexOf("..") < 0)
                    dr[gv.Columns[j].HeaderText.ToString()] = "'" + row.Cells[j].Text + "'";
            }

            _datatable.Rows.Add(dr);

        }

        dr = _datatable.NewRow();

        for (int i = 0; i < gv.Columns.Count; i++)
        {
            if (gv.Columns[i].HeaderText.ToString() != "")
            {
                if (gv.FooterRow.Cells[i].Text.ToString() != "&nbsp;")
                    dr[gv.Columns[i].HeaderText.ToString()] = gv.FooterRow.Cells[i].Text;
                else
                    dr[gv.Columns[i].HeaderText.ToString()] = "";
            }
        }

        _datatable.Rows.Add(dr);

        return _datatable;
    }

    //public void InsUpd_Dealer(string name)
    //{
    //    GetConection();
    //    SqlCommand objcmd = new SqlCommand("SP_InsUpd_Dealer", con);
    //    objcmd.CommandType = CommandType.StoredProcedure;
    //    objcmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = name;

    //    objcmd.ExecuteNonQuery();
    //}
    public void ExportExcell(DataTable dt)
    {
        gr = new GridView();
        gr.DataSource = dt;

        // Session["export"] = "1";
        // = new HttpResponse();
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;

        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        gr.AllowPaging = false;
        gr.DataBind();

        //Change the Header Row back to white color
        // GridView1.HeaderRow.Style.Add("background-color", "#FFFFFF");

        //Apply style to Individual Cells
        //   GridView1.HeaderRow.Cells[0].Style.Add("background-color", "green");
        //  GridView1.HeaderRow.Cells[1].Style.Add("background-color", "green");
        // GridView1.HeaderRow.Cells[2].Style.Add("background-color", "green");
        //  GridView1.HeaderRow.Cells[3].Style.Add("background-color", "green");




        try
        {
            gr.RenderControl(hw);
        }
        catch (Exception ex) { }

        //style to format numbers to string
        //  string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        //  Response.Write(style);
        HttpContext.Current.Response.Output.Write(sw.ToString());
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();


    }






}








