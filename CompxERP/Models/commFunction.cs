using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using System.Net.Mail;
using System.Collections.Generic;

public class commFunction
{
    submitData osubmit;
    DataTable dt;
    SqlCommand objcmd;
    SqlDataAdapter da;
    public static DateTime pcurr_serial;
    public static DateTime pdate_sesion;
    //AccountInfo oAccount = new AccountInfo();


    public String sql;
    public SqlCommand command;
    //connection c = new connection();
    public commFunction()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    //public string GetDateFormat(string Date, int Type = 0)
    //{
    //    try
    //    {
    //        string dd, mm, yy;
    //        dd = Date.Split('/')[0];
    //        mm = Date.Split('/')[1];
    //        yy = Date.Split('/')[2];
    //        return (mm + "/" + dd + "/" + yy).ToString();
    //    }
    //    catch { return ""; }
    //}
    public void Send_SMS1(string sMassage, string sMobile)
    {
        string sURL;

        StreamReader objReader; sURL = "http://216.245.209.132/rest/services/sendSMS/sendGroupSms?AUTH_KEY=8d2f63772eef7559f01d321c6868e5bb&message=" + sMassage + "&senderId=NPKHGN&routeId=1&mobileNos=" + sMobile + "&smsContentType=english";
          
        WebRequest wrGETURL;
        wrGETURL = WebRequest.Create(sURL);

        Stream objStream;
        objStream = wrGETURL.GetResponse().GetResponseStream();
        objReader = new StreamReader(objStream);
        objReader.Close();
    }
    public DateTime GetDateFormat(string Date, int Type = 0)
    {
        string dd, mm, yy;
        //if (Type == 0)//sourabh 16-dec-2015
        //{
        dd = Date.Substring(0, 2);
        mm = Date.Substring(3, 2);
        yy = Date.Substring(6, 4);
        //return Convert.ToDateTime(mm + "/" + dd + "/" + yy);
        DateTime dt = DateTime.ParseExact(dd + "/" + mm + "/" + yy, "d/M/yyyy", CultureInfo.InvariantCulture);
        return Convert.ToDateTime(dt);
        //}

        //return DateTime.Now.Date;
    }
    public DateTime GetDateFormat(string Date )
    {
        string dd, mm, yy;
        //if (Type == 0)//sourabh 16-dec-2015
        //{
        dd = Date.Substring(0, 2);
        mm = Date.Substring(3, 2);
        yy = Date.Substring(6, 4);
        //return Convert.ToDateTime(mm + "/" + dd + "/" + yy);
        DateTime dt = DateTime.ParseExact(dd + "/" + mm + "/" + yy, "d/M/yyyy", CultureInfo.InvariantCulture);
        return Convert.ToDateTime(dt);
        //}

        //return DateTime.Now.Date;
    }

    public void Send_Mail1(string sMailID, string sSubject, string sBody)
    {
        const string SERVER2 = "relay-hosting.secureserver.net";
        System.Web.Mail.MailMessage oMail2 = new System.Web.Mail.MailMessage();
        oMail2.From = "support@digiclayinfotech.co.in";
        oMail2.To = sMailID;

        oMail2.Subject = sSubject;
        oMail2.BodyFormat = System.Web.Mail.MailFormat.Html;
        oMail2.Priority = System.Web.Mail.MailPriority.High;
        oMail2.Body = sBody;
        SmtpMail.SmtpServer = SERVER2;
        SmtpMail.Send(oMail2);
        oMail2 = null;
    }


    public void Send_Mail(string sMailID, string sSubject, string sBody)
    {
        NetworkCredential mailAuthentication = new NetworkCredential("ajay@digiclayinfotech.com", "ajay123");
        //NetworkCredential("er.soni.sourabh@gmail.com", "4nksmganesh136");

        System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
        mailClient.Host ="smtpout.asia.secureserver.net"; //"smtp.mail.yahoo.com";//
        mailClient.Port = 587;//587,465,80,3535,25,995---not support secure connection at 80,3535
        mailClient.EnableSsl = true;
        mailClient.UseDefaultCredentials = false;
        mailClient.Credentials = mailAuthentication;

        System.Net.Mail.MailMessage MyMailMessage = new System.Net.Mail.MailMessage("subodhsharda@yahoo.in", sMailID, sSubject, sBody);
        mailClient.Send(MyMailMessage);

        const string SERVER2 = "relay-hosting.secureserver.net";

        System.Web.Mail.MailMessage oMail2 = new System.Web.Mail.MailMessage();
        oMail2.From = "support@digiclayinfotech.co.in";
        oMail2.To = sMailID;
        oMail2.Subject = sSubject;// "Acknowledgement";
        oMail2.BodyFormat = System.Web.Mail.MailFormat.Html; // enumeration
        oMail2.Priority = System.Web.Mail.MailPriority.High; // enumeration 
        oMail2.Body = sBody;
        System.Web.Mail.SmtpMail.SmtpServer = SERVER2;
        System.Web.Mail.SmtpMail.Send(oMail2);
        oMail2 = null;
    }

    public void Email_With_CCandBCC( string cc, string bcc, String Subj, string Message, string regards)
    {
         
String ToEmail ="ajay@digiclayinfotech.com,";
        string HostAdd = "smtpout.asia.secureserver.net";
        string FromEmailid = "ajay@digiclayinfotech.com";
        string Pass = "ajay123";
        int port = 587;
        System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
        mailMessage.From = new System.Net.Mail.MailAddress(FromEmailid);
        mailMessage.Subject = "Subject";
        Message = Message.Replace("\"", "'");
        if (regards != "") Message += "<br/><br/><br/><br/><br/><br/><h4>Regards:</h4><br/>" + regards;
        mailMessage.Body = Message;
        mailMessage.IsBodyHtml = true;
        string[] ToMuliId = ToEmail.Split(',');
        foreach (string ToEMailId in ToMuliId)
        {
            mailMessage.To.Add(new System.Net.Mail.MailAddress(ToEMailId));
        }

        if (cc.Contains(","))
        {
            string[] CCId = cc.Split(',');
            foreach (string CCEmail in CCId)
            {
                mailMessage.CC.Add(new MailAddress(CCEmail));
            }
        }
         
        string fileName = "", fileNewName = "";
        
        SmtpClient smtp = new SmtpClient();
        smtp.Host = HostAdd;
        smtp.Port = port;
        smtp.EnableSsl = true;
        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(FromEmailid, Pass);
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = credentials;
        smtp.Send(mailMessage);
    }

    public string GetFinancialYear(DateTime dDate)
    {
        int CurrentYear = dDate.Year;
        int PreviousYear = dDate.Year - 1;
        int NextYear = dDate.Year + 1;
        string PreYear = PreviousYear.ToString();
        string NexYear = NextYear.ToString();
        string CurYear = CurrentYear.ToString();
        string FinYear = null;

        if (dDate.Month > 3)
            FinYear = CurYear + "-" + NexYear;
        else
            FinYear = PreYear + "-" + CurYear;
        return FinYear.Trim();
    }

    public bool Send_SMS(string sMassage, string sMobile)//181029 %temp%
    {
        string senderid = ConfigurationManager.AppSettings["SMS_Senderid"].ToString();
        string sURL;
        if (senderid != "")
        {
            StreamReader objReader;
            sURL = "http://216.245.209.132/rest/services/sendSMS/sendGroupSms?AUTH_KEY=8d2f63772eef7559f01d321c6868e5bb&message=" + sMassage + "&senderId=" + senderid + "&routeId=1&mobileNos=" + sMobile + "&smsContentType=english";
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);
            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();
            objReader = new StreamReader(objStream);
            objReader.Close();
            return true;
        }
        return false;
    }

    //======================================================================

    //public void ClearInputs(ControlCollection ctrls)
    //{
    //    foreach (Control ctrl in ctrls)
    //    {
    //        if (ctrl is TextBox)
    //            ((TextBox)ctrl).Text = string.Empty;
    //        else if (ctrl is DropDownList)
    //            ((DropDownList)ctrl).ClearSelection();

    //        ClearInputs(ctrl.Controls);
    //    }
    //}

    //public void DisableControls(ControlCollection ctrls, string Contrll)
    //{
    //    foreach (Control ctrl in ctrls)
    //    {
    //        if (ctrl.ID != Contrll)
    //        {
    //            if (ctrl is TextBox)
    //            {
    //                ((TextBox)ctrl).Enabled = false;
    //            }
    //            else if (ctrl is DropDownList)
    //            {
    //                ((DropDownList)ctrl).Enabled = false;
    //            }
    //        }
    //        DisableControls(ctrl.Controls, Contrll);
    //    }
    //}
    public void CheckSession()
    {
        try
        {
            if (HttpContext.Current.Session["isLogin"].ToString() != "True")
            {
                HttpContext.Current.Response.Write("<script>window.open('Login.aspx','mainFrame')</script>");
            }
            else
                CheckPermission();
        }
        catch { HttpContext.Current.Response.Write("<script>window.open('Login.aspx','mainFrame')</script>"); }
    }
    public void GetStateCity(DropDownList ddlCityState, int Type, int stateCode)
    {
        osubmit = new submitData();
        osubmit.GetConection();
        if (Type == 0)
        {
            da = new SqlDataAdapter("select cityname,citycode from citydet  where cityType='67'", osubmit.con);
            dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ddlCityState.DataSource = dt;
                ddlCityState.DataTextField = "cityname";
                ddlCityState.DataValueField = "citycode";
                ddlCityState.DataBind();
            }
            ddlCityState.Items.Insert(0, new ListItem("--Select State--", "0"));
        }
        else
        {
            da = new SqlDataAdapter("select cityname,citycode from citydet  where cityType='15' and cityrute = '" + stateCode + "'", osubmit.con);
            dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ddlCityState.DataSource = dt;
                ddlCityState.DataTextField = "cityname";
                ddlCityState.DataValueField = "citycode";
                ddlCityState.DataBind();
            }
            ddlCityState.Items.Insert(0, new ListItem("--Select City--", "0"));
        }

    }

 


    public DataTable GetData(int menucode, int searchfld)
    {
        osubmit = new submitData();
        osubmit.GetConection();
        dt = new DataTable();
        objcmd = new SqlCommand("getData", osubmit.con);
        objcmd.CommandType = CommandType.StoredProcedure;
        objcmd.Parameters.Add("@MenuCode", SqlDbType.Int).Value = menucode;
        objcmd.Parameters.Add("@DdlCode", SqlDbType.Int).Value = searchfld;
        da = new SqlDataAdapter(objcmd);
        da.Fill(dt);
        return dt;
    }
    public bool CheckDateRange(string Startdate, string EndDate)
    {
        string dd1, mm1, yy1;
        string dd2, mm2, yy2;

        dd1 = Startdate.Substring(0, 2);
        mm1 = Startdate.Substring(3, 2);
        yy1 = Startdate.Substring(6, 4);

        dd2 = EndDate.Substring(0, 2);
        mm2 = EndDate.Substring(3, 2);
        yy2 = EndDate.Substring(6, 4);

        DateTime dt1 = Convert.ToDateTime(mm1 + "/" + dd1 + "/" + yy1);
        DateTime dt2 = Convert.ToDateTime(mm2 + "/" + dd2 + "/" + yy2);
        if (dt1 > dt2)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    //public DateTime GetDateFormat(string Date, int Type = 0)
    //{
    //    string dd, mm, yy;
    //    if (Type == 0)
    //    {
    //        dd = Date.Substring(0, 2);
    //        mm = Date.Substring(3, 2);
    //        yy = Date.Substring(6, 4);
    //        //return Convert.ToDateTime(mm + "/" + dd + "/" + yy);
    //        DateTime dt = DateTime.ParseExact(dd + "/" + mm + "/" + yy, "d/M/yyyy", CultureInfo.InvariantCulture);
    //        return Convert.ToDateTime(dt);
    //    }

    //    return DateTime.Now.Date;
    //}
    public DateTime GetDateFormatSplit(string Date)
    {
        string dd, mm, yy;
        dd = Date.Split('/')[0];
        mm = Date.Split('/')[1];
        yy = Date.Split('/')[2];
        return Convert.ToDateTime(mm + "/" + dd + "/" + yy);
    }

    public string GetDateFormatMMDDYY(string Date)
    {
        string dd, mm, yy;
        dd = Date.Substring(0, 2);
        mm = Date.Substring(3, 2);
        yy = Date.Substring(6, 4);
        return mm + "-" + dd + "-" + yy;
    }

    public string GetValidNo(string Numbers, int Type)
    {
        if (Numbers != "")
        {
            if (Type == 0)
            {
                if (Numbers.Contains("."))
                {
                    int i = Numbers.IndexOf(".");
                    Numbers = Numbers.Substring(0, i + 3);

                }
                else
                {
                    Numbers = Numbers.ToString();
                }
            }
            else
            {
                if (Numbers.Contains("."))
                {
                    int i = Numbers.IndexOf(".");
                    Numbers = Numbers.Substring(0, i);

                }
            }
        }

        return Numbers;
    }

    public string getMaxcitystateCol(int type)
    {
        osubmit = new submitData();
        osubmit.GetConection();
        string sql = "";
        if (type == 1)
        {
            sql = "Select Max(citycode)+1 from citydet where cityType='67'";
        }
        else if (type == 2)
        {
            sql = "Select Max(citycode)+1 from citydet where cityType='15'";
        }
        else if (type == 3)
        {
            sql = "Select Max(citycode)+1 from citydet where cityType='582'";
        }
        SqlDataAdapter da = new SqlDataAdapter(sql, osubmit.con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0][0].ToString() == "")
            {
                return "1";
            }
            else
            {
                return dt.Rows[0][0].ToString();
            }
        }
        else
        {
            return "1";
        }
    }
    public void CheckRole(string SessRole)
    {
        try
        {
            if (SessRole == "2250" || SessRole == "2252")
            {
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(pg, typeof(Page), "Script", "alert(You Dont Have Rights To open Masters);", true);
                HttpContext.Current.Response.Redirect("~/Default2.aspx");

            }
        }
        catch (Exception ex)
        {

        }
    }
    //===============================Current Date==================================
    public DateTime getCurrentDateTime()
    {
        return DateTime.Now.AddHours(12).AddMinutes(30);

    }
    public DateTime getCurrentDate()
    {
        return DateTime.Now;
    }
    public string GetFirmOwner(string FirmCode)
    {
        osubmit = new submitData();
        osubmit.GetConection();

        SqlDataAdapter daown = new SqlDataAdapter("select owner_name from ClientMaster where client_id='" + FirmCode + "'", osubmit.con);
        DataTable dtown = new DataTable();
        daown.Fill(dtown);
        return dtown.Rows[0]["owner_name"].ToString();
    }

    public DataTable GetTableData(string Tablename, string columnNms, string Condition)
    {
        if (Condition.Trim() != "")
        {
            Condition = " where " + Condition;
        }
        osubmit = new submitData();
        osubmit.GetConection();
        SqlDataAdapter da = new SqlDataAdapter("select " + columnNms + " From " + Tablename + Condition, osubmit.con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;

    }
    protected string GetBoundField(GridView gvDetails)
    {
        string boundFields = "";

        for (int i = 0; i < gvDetails.Columns.Count; i++)
        {
            DataControlField field = gvDetails.Columns[i];
            BoundField bfield = field as BoundField;

            if (bfield != null)
            {
                boundFields += bfield.DataField + " ,";
            }
        }
        boundFields = boundFields.TrimEnd(',');
        return boundFields;
    }
    public void FillGrid(GridView gvDetails, string Tablenm, string Condition)
    {

        if (Condition.Trim() != "")
        {
            Condition = " where " + Condition;
        }
        osubmit = new submitData();
        osubmit.GetConection();
        da = new SqlDataAdapter("select " + GetBoundField(gvDetails) + " From " + Tablenm + Condition, osubmit.con);
        dt = new DataTable();
        da.Fill(dt);
        gvDetails.DataSource = dt;
        gvDetails.DataBind();

    }
    public DataTable GetFirmDevice(string FirmCode, string status, string NoticeType)
    {
        osubmit = new submitData();
        osubmit.GetConection();
        string sql = "";
        if (NoticeType == "1")
        {
            if (status == "0")
            {
                sql = "select c.owner_name,s.code,s.DeviceNm,s.DeviceNo,s.SeizDate,'N/A'  Pno,'Seized' as PermStatus from SeizureEntry  s left join ClientMaster c on s.FirmCode = c.client_id  where s.FirmCode = '" + FirmCode + "' and s.status='" + status + "'";
            }
            else
            {
                sql = "select n.InstNo as DeviceNo,n.InstName as DeviceNm,s.SeizDate,'N/A' as Pno,case when n.PermStatus = 'P' then 'Pending' else n.PermStatus end as PermStatus from NoticeInfo n left join SeizureEntry s on s.DeviceNo = n.InstNo where n.FirmCode='" + FirmCode + "' and s.Status = '" + status + "' and isnull(n.SentStatus,'') != 'Sent' and NoticeType='" + NoticeType + "'";
            }
        }
        else
        {
            if (status == "0")
            {
                sql = "select n.FirmCode,c.owner_name,n.InstNo as DeviceNo,n.InstName as DeviceNm,s.SeizDate,n.NoticeNo as Pno,'N/A' as PermStatus from NoticeInfo n left join SeizureEntry s on s.DeviceNo = n.InstNo left join ClientMaster c on c.client_id = n.FirmCode where n.FirmCode='" + FirmCode + "' and s.Status = '1' and isnull(n.SentStatus,'') = 'Sent'";
            }
            else
            {
                sql = "select n.FirmCode,c.owner_name,n.InstNo as DeviceNo,n.InstName as DeviceNm,s.SeizDate,n.NoticeNo as Pno, case when n.PermStatus = 'P' then 'Pending' else n.PermStatus end as PermStatus from NoticeInfo n left join SeizureEntry s on s.DeviceNo = n.InstNo left join ClientMaster c on c.client_id = n.FirmCode where n.FirmCode='" + FirmCode + "' and isnull(n.SentStatus,'') != 'Sent' and NoticeType='" + NoticeType + "'";
            }

        }
        SqlDataAdapter daown = new SqlDataAdapter(sql, osubmit.con);
        DataTable dtown = new DataTable();
        daown.Fill(dtown);
        return dtown;
    }
    //============Check Permission===========================20 Aug Sourabh
    public void CheckPermission()
    {
        //osubmit = new submitData();
        //osubmit.GetConection();

        //string url = HttpContext.Current.Request.Url.AbsoluteUri;
        //string s = HttpContext.Current.Session["UserType"].ToString();
        //string sql = "";
        //if (s != "")
        //{
        //    sql = osubmit.GetSingleData("select menacce from usermenust u join menuoption_new m on u.mencode=m.mencode where menuser='" + s + "' and menform like '" + url.Substring(url.LastIndexOf('/'), url.Substring(url.LastIndexOf('/')).LastIndexOf('.')).Remove(0, 1) + ".aspx'").ToString();
        //    if (sql != "True")
        //    {
        //        //HttpContext.Current.Response.Redirect("~/Default2.aspx", false);
        //        HttpContext.Current.Response.Write("<script>window.open('Login.aspx','mainFrame')</script>");
        //    }
        //}
    }

    public void attach()
    {
        string cmpName = Dns.GetHostEntry(HttpContext.Current.Request.UserHostAddress).HostName + "\\SQLEXPRESS";

        SqlConnection conn = new SqlConnection();
        conn.ConnectionString = @"Server=" + cmpName + ";database=master;User ID=sa; password=microplexer";

        string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");

        string path1 = path + "\\WnmPro.mdf";
        string path2 = path + "\\WnmPro_log.ldf";
        try
        {
            conn.Open();

            SqlCommand com = new SqlCommand("CREATE DATABASE WnmPro ON ( FILENAME = '" + path1 + "' ), ( FILENAME = '" + path2 + "' ) FOR ATTACH", conn);

            com.ExecuteScalar();

        }
        catch (Exception ex)
        {

        }
        finally
        {
            conn.Close();
        }
    }
    //Enc & Dec
    static readonly string PasswordHash = "wnmP@@sWord";
    static readonly string SaltKey = "sahkjdhkash";
    static readonly string VIKey = "1so3@0sa6a@gfhjg";

    public static string Encrypt(string plainText)
    {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
        var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
        var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

        byte[] cipherTextBytes;

        using (var memoryStream = new MemoryStream())
        {
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                cipherTextBytes = memoryStream.ToArray();
                cryptoStream.Close();
            }
            memoryStream.Close();
        }
        return Convert.ToBase64String(cipherTextBytes);
    }
    public static string Decrypt(string encryptedText)
    {
        byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
        byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
        var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

        var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
        var memoryStream = new MemoryStream(cipherTextBytes);
        var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        byte[] plainTextBytes = new byte[cipherTextBytes.Length];

        int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        memoryStream.Close();
        cryptoStream.Close();
        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
    }
  
     
     
    public DateTime getOpenDate(DateTime EntryDate)
    {
        if (Convert.ToInt16(EntryDate.Month) >= 4 && Convert.ToInt16(EntryDate.Month) <= 12)
            return Convert.ToDateTime(DateTime.ParseExact("01/04/" + Convert.ToInt16(EntryDate.Year), "d/M/yyyy", CultureInfo.InvariantCulture));
        else
            return Convert.ToDateTime(DateTime.ParseExact("01/04/" + Convert.ToInt16(EntryDate.Year - 1), "d/M/yyyy", CultureInfo.InvariantCulture));
    }
     
    public DateTime getClosDate(DateTime EntryDate)
    {
        if (Convert.ToInt16(EntryDate.Month) >= 4 && Convert.ToInt16(EntryDate.Month) <= 12)
            return Convert.ToDateTime(DateTime.ParseExact("31/03/" + Convert.ToInt16(EntryDate.Year + 1), "d/M/yyyy", CultureInfo.InvariantCulture));
        else
            return Convert.ToDateTime(DateTime.ParseExact("31/03/" + Convert.ToInt16(EntryDate.Year), "d/M/yyyy", CultureInfo.InvariantCulture));
    }
     
    public void iniFinYrInfo()
    {
        int iYear = DateTime.Now.Year;
        int iMonth = DateTime.Now.Month;
        int iDay = DateTime.Now.DayOfYear;

        pcurr_serial = Convert.ToDateTime(iYear + "/" + iMonth + "/" + iDay);
        pdate_sesion = pcurr_serial;
    }

    /*
     /// <summary>
    /// For Show Message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="MessType"></param>
    /// <returns></returns>
    public static bool MessageBox(string message, eMessType MessType)
    {
        string bResult = "";
        try
        {
            switch (MessType)
            {
                case eMessType.Alert:
                    HttpContext.Current.Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert('" + message + "')</SCRIPT>");
                    break;
                case eMessType.Information:
                    HttpContext.Current.Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert('" + message + "')</SCRIPT>");
                    break;
                case eMessType.Confirmation:
                    HttpContext.Current.Response.Write("<SCRIPT LANGUAGE=\"JavaScript\"><%=bResult%> = confirm('" + message + "')</SCRIPT>");
                    break;
                case eMessType.Error:
                    HttpContext.Current.Response.Write("<SCRIPT LANGUAGE=\"JavaScript\">alert('" + message + "')</SCRIPT>");
                    break;
            }
        }
        catch (Exception ex)
        {

        }
        return true;
    }
    /// <summary>
    /// For Export Data Of GridView
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="gv"></param>
    /// <param name="eExpoType"></param>
    public static void ExportGridView(string fileName, GridView gv, eExportType eExpoType)
    {
        HttpContext.Current.Response.Clear();
        switch (eExpoType)
        {
            case eExportType.Excel:
                HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                break;
            case eExportType.CSV:
                HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
                HttpContext.Current.Response.ContentType = "application/text";
                break;
            case eExportType.Word:
                HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
                HttpContext.Current.Response.ContentType = "application/vnd.word";
                break;
            case eExportType.HTML:
                HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
                HttpContext.Current.Response.ContentType = "application/text";
                break;
            case eExportType.PDF:
                HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
                HttpContext.Current.Response.ContentType = "application/pdf";
                break;
        }
        gv.AllowPaging = false;
        //StringWriter oStringWriter = new StringWriter();
        //HtmlTextWriter oHtmlTextWriter = new HtmlTextWriter(oStringWriter);

        //gv.AllowSorting = false;
        //
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //gv.RenderControl(hw);

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                ////  Create a form to contain the grid
                //Table table = new Table();
                ////  add the header row to the table
                //if (gv.HeaderRow != null)
                //{
                //    PrepareControlForExport(gv.HeaderRow);
                //    table.Rows.Add(gv.HeaderRow);
                //}

                ////  add each of the data rows to the table
                //foreach (GridViewRow row in gv.Rows)
                //{
                //    PrepareControlForExport(row);
                //    table.Rows.Add(row);
                //}
                ////  add the footer row to the table
                //if (gv.FooterRow != null)
                //{
                //    PrepareControlForExport(gv.FooterRow);
                //    table.Rows.Add(gv.FooterRow);
                //}

                ////  render the table into the htmlwriter
                //table.RenderControl(htw);
                //gv.RenderControl(htw);
                if (eExpoType == eExportType.PDF)
                {
                    StringReader sr = new StringReader(sw.ToString());
                    Document pdfDoc = new Document(PageSize.A4_LANDSCAPE, 10f, 10f, 10f, 0f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();
                    try
                    {
                        htmlparser.Parse(sr);
                        pdfDoc.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox(ex.Message, eMessType.Error);
                    }
                    HttpContext.Current.Response.Write(pdfDoc);
                    HttpContext.Current.Response.End();
                }
                else if (eExpoType == eExportType.CSV)
                {
                    HttpContext.Current.Response.Write(GridViewToCSV(gv).ToString());
                    HttpContext.Current.Response.End();
                }
                else
                {
                    //  render the htmlwriter into the response
                    HttpContext.Current.Response.Write(sw.ToString());
                    HttpContext.Current.Response.End();
                }
            }
        }
    }
     */
    public int ConvertFinMonth(string mm, int type = 1)
    {
        int mmm;
        if (type == 1)
        {
            switch (mm)//MonthCode
            {
                case "Apr": return 1;
                case "May": return 2;
                case "Jun": return 3;
                case "Jul": return 4;
                case "Aug": return 5;
                case "Sep": return 6;
                case "Oct": return 7;
                case "Nov": return 8;
                case "Dec": return 9;
                case "Jan": return 10;
                case "Feb": return 11;
                case "Mar": return 12;

                case "04":
                case "4": return 1;
                case "05":
                case "5": return 2;
                case "06":
                case "6": return 3;
                case "07":
                case "7": return 4;
                case "08":
                case "8": return 5;
                case "09":
                case "9": return 6;
                case "10": return 7;
                case "11": return 8;
                case "12": return 9;
                case "01":
                case "1": return 10;
                case "02":
                case "2": return 11;
                case "03":
                case "3": return 12;
            }
        }
        else if (type == 2)
        {
            switch (mm)//MonthCode
            {
                case "1":
                case "01": return 4;
                case "2":
                case "02": return 5;
                case "3":
                case "03": return 6;
                case "4":
                case "04": return 7;
                case "5":
                case "05": return 8;
                case "6":
                case "06": return 9;
                case "7":
                case "07": return 10;
                case "8":
                case "08": return 11;
                case "9":
                case "09": return 12;
                case "10": return 1;
                case "11": return 2;
                case "12": return 3;
            }
        }
        else
        {
            switch (mm)//Original
            {
                case "Apr": return 4;
                case "May": return 5;
                case "Jun": return 6;
                case "Jul": return 7;
                case "Aug": return 8;
                case "Sep": return 9;
                case "Oct": return 10;
                case "Nov": return 11;
                case "Dec": return 12;
                case "Jan": return 1;
                case "Feb": return 2;
                case "Mar": return 3;

                case "04":
                case "4": return 4;
                case "05":
                case "5": return 5;
                case "06":
                case "6": return 6;
                case "07":
                case "7": return 7;
                case "08":
                case "8": return 8;
                case "09":
                case "9": return 9;
                case "10": return 10;
                case "11": return 11;
                case "12": return 12;
                case "01":
                case "1": return 1;
                case "02":
                case "2": return 2;
                case "03":
                case "3": return 3;
            }
        }
        int.TryParse(mm, out mmm);
        return mmm;
    }
    public string ConvertIntToMonth(int iMonth)
    {
        string sMonth = "";
        switch (iMonth)//MonthCode
        {
            case 1: return "Apr";
            case 2: return "May";
            case 3: return "Jun";
            case 4: return "Jul";
            case 5: return "Aug";
            case 6: return "Sep";
            case 7: return "Oct";
            case 8: return "Nov";
            case 9: return "Dec";
            case 10: return "Jan";
            case 11: return "Feb";
            case 12: return "Mar";

        }

        return sMonth;
    }

    public string finDate(int month, int year)
    {
        int y1 = 0, y2 = 0;
        if (month < 4)
        {
            y1 = year - 1;
            y2 = year;
        }
        else
        {
            y1 = year;
            y2 = year + 1;
        }
        return "" + y1 + "," + y2;
    }

    public string GetFinYear(int month, int year)
    {
        int y1 = 0, y2 = 0;
        if (month < 4)
        {
            y1 = year - 1;
            y2 = year;
        }
        else
        {
            y1 = year;
            y2 = year + 1;
        }
        return "" + y1 + "_" + y2.ToString().Substring(2);
    }

    public int getMonthLastDate(int year, int month, int type = 1)
    {
        int date = 0;
        if (type == 1)
        {
            switch (month)
            {
                case 4:
                case 6:
                case 8:
                case 10:
                case 11:
                case 1:
                case 3: date = 31; break;
                case 7:
                case 9:
                case 12:
                case 2: date = 30; break;
                case 5: if ((year % 4) == 0) date = 29; else date = 28; break;
            }
        }
        else
        {
            switch (month)//Actaul Month
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12: date = 31; break;
                case 4:
                case 6:
                case 9:
                case 11: date = 30; break;
                case 2: if ((year % 4) == 0) date = 29; else date = 28; break;
            }

        }
        return date;
    }
     
    public string NumberToWords(long number)  // convert numbers to word
    {
        if (number == 0)
        {
            return "Zero";
        }
        if (number < 0)

            return "Minus" + NumberToWords(Math.Abs(number)) + " ";
        string words = "";

        if ((number / 100000000) > 0)
        {
            words += NumberToWords(number / 10000000) + " " + "Crore" + " ";
            number %= 10000000;
        }
        if ((number / 1000000) > 0)
        {
            words += NumberToWords(number / 100000) + " " + "Lakh" + " ";
            number %= 100000;
        }
        if ((number / 1000) > 0)
        {
            words += NumberToWords(number / 1000) + " " + "Thousand" + " ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += NumberToWords(number / 100) + " " + "Hundred" + " ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")

                words += "and" + " ";
            var unitW = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Tweleve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tenW = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fivety", "Sixty", "Seventy", "Eighty", "Ninety" };


            if (number < 20)

                words += unitW[number];
            else
            {
                words += tenW[number / 10];
                if ((number % 10) > 0)
                {
                    words += " " + unitW[number % 10];
                }
            }
        }
        return words;
    }
      
    public DataTable GetDataTableExcel(string strFileName, string Table)
    {
        System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + strFileName + "; Extended Properties = \"Excel 8.0;HDR=Yes;IMEX=1\";");

        conn.Open();
        string strQuery = "SELECT * FROM [" + Table + "]";
        System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter(strQuery, conn);
        System.Data.DataSet ds = new System.Data.DataSet();
        adapter.Fill(ds);
        return ds.Tables[0];

    }
   
 
    #region " Drop Down List "
    public void FillDropDown(DataTable dt, string sName, string sValue, DropDownList ddl)
    {
       

        ddl.Items.Clear();

        ddl.Items.Add(new ListItem("--Select--", "0"));
        //ddl.SelectedValue = "0"; //12-10-2016 and change above

        ddl.DataTextField = sName;
        ddl.DataValueField = sValue;

        ListItem itm = null;


        for (int iIndex = 0; iIndex < dt.Rows.Count; iIndex++)
        {
            itm = new ListItem();
            itm.Text = dt.Rows[iIndex][sName].ToString();
            itm.Value = dt.Rows[iIndex][sValue].ToString();

            ddl.Items.Add(itm);
        }

    }

    public void FillDropDown(DataTable dt, string sName, string sValue, ListBox ddl)
    {
        

        ddl.Items.Clear();

        ddl.Items.Add(new ListItem("--Select--", "0"));
        //ddl.SelectedValue = "0"; //12-10-2016 and change above

        ddl.DataTextField = sName;
        ddl.DataValueField = sValue;

        ListItem itm = null;


        for (int iIndex = 0; iIndex < dt.Rows.Count; iIndex++)
        {
            itm = new ListItem();
            itm.Text = dt.Rows[iIndex][sName].ToString();
            itm.Value = dt.Rows[iIndex][sValue].ToString();

            ddl.Items.Add(itm);
        }

    }
    #endregion

    #region "Fill Grid"
    public void FillGrid(DataTable dt, GridView gv)
    {
        gv.DataSource = dt;
        gv.DataBind();
    }
    #endregion
 
}
