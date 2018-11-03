//using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CompxERP.Models;

namespace CompxERP.Pages
{
    public partial class Reports : System.Web.UI.Page
    {
        List<byte[]> lstByteArray = null; string mimeType = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            int dCode = Convert.ToInt32( Request.QueryString["Code"]);
            int compcode = Convert.ToInt32(Request.QueryString["compcode"]);
            
            string PrintType = Request.QueryString["PType"];

            if (!string.IsNullOrEmpty(PrintType) && PrintType == "QuotPrint")
                CreatePDFForInvoice(dCode);
            else if (!string.IsNullOrEmpty(PrintType) && PrintType == "InBoundExl")
                getInBoundExl();
            else if (!string.IsNullOrEmpty(PrintType) && PrintType == "UserWorkExl")
                getUserWork();
            else if (!string.IsNullOrEmpty(PrintType) && PrintType == "OrderPrint")
                Order_PDF(174, dCode, compcode);
            else if (!string.IsNullOrEmpty(PrintType) && (PrintType == "GST"))
                CreatePDFForGST_Book();
            else if (!string.IsNullOrEmpty(PrintType) && (PrintType == "GSTR1"))
                GSTR1();
            else if (!string.IsNullOrEmpty(PrintType) && (PrintType == "GSTR2"))
                GSTR2();
            else if (!string.IsNullOrEmpty(PrintType) && (PrintType == "GSTR3B"))
                GSTR3B();
            else if (!string.IsNullOrEmpty(PrintType) && (PrintType == "SalesBook"))
                SalesBook();
            else if (!string.IsNullOrEmpty(PrintType) && (PrintType == "PuSL" || PrintType == "42"))
                PuSl_PDF(dCode);
            else if (!string.IsNullOrEmpty(PrintType) && (PrintType == "5" || PrintType == "3"))
                Voucher_PDF(dCode, PrintType);
            else if (!string.IsNullOrEmpty(PrintType) && (PrintType == "Ledger"))
                LedgerPrint(Request.QueryString["sFrom"], Request.QueryString["sTO"], Request.QueryString["sParty"], 1, compcode);
            else if (!string.IsNullOrEmpty(PrintType) && (PrintType == "TralBal"))
                TrialBal(Request.QueryString["sFrom"], Request.QueryString["sTO"],  Convert.ToInt32(Request.QueryString["sParty"]), 1, compcode, Convert.ToInt32(Request.QueryString["StateID"]),Convert.ToInt32( Request.QueryString["Executive"]));
        }

        private void Order_PDF(int TypeId, int dCode, int compcode)
        {
            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            string fileName = "Order"; string rPath = Server.MapPath("~/Report/Order.rdlc");

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Report/Order.rdlc");
            viewer.LocalReport.ReportEmbeddedResource = rPath;

            viewer.LocalReport.DataSources.Clear();

            clsSubmitModel oSubmit = new clsSubmitModel();
            DataSet ds = oSubmit.GetDataSet("sp_GetOrderBill @mstcode=" + dCode + ",@compcode=" + compcode + ",@msttype=" + TypeId, true);

            ReportDataSource datasource = new ReportDataSource("DataSet1", SetRows(ds.Tables[0], 14));
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet2", ds.Tables[1]);
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet3", ds.Tables[2]);
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet4", ds.Tables[3]);
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.EnableExternalImages = true;

            string imagePath = "";

            imagePath = "~/img/" + SessionMaster.CompCode + ".png";

            imagePath = new Uri(Server.MapPath(imagePath)).AbsoluteUri;
            ReportParameter p32 = new ReportParameter("ImgPath", imagePath);
            viewer.LocalReport.SetParameters(new ReportParameter[] { p32 });

            viewer.LocalReport.EnableHyperlinks = true;
            viewer.LocalReport.Refresh();

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
             
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "inline; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }
        public DataTable SetRows(DataTable dt, int iRows)
        {
            iRows = iRows - dt.Rows.Count;
            dt.Columns.Add("RowNo");
            DataRow dr;
            for (int i = 0; i < iRows; i++)
            {
                dr = dt.NewRow();
                dr["RowNo"] = dt.Rows.Count + 1;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private void CreatePDFForInvoice( int dCode )
        {
            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            string fileName = "Quotation"; string rPath = Server.MapPath("~/Report/Quotation.rdlc");
            //SessionManager.CompanyId

            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Report/Quotation.rdlc");
            viewer.LocalReport.ReportEmbeddedResource = rPath;
 
           
            viewer.LocalReport.DataSources.Clear();
           
             clsSubmitModel oSubmit = new clsSubmitModel(); 
             DataSet ds = oSubmit.GetDataSet("GetQuotPrint @MstCode=" + dCode, true);
             
             ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables[0]); 
            viewer.LocalReport.DataSources.Add(datasource); 
             datasource = new ReportDataSource("DataSet2", ds.Tables[1]);
             viewer.LocalReport.DataSources.Add(datasource); 
             datasource = new ReportDataSource("DataSet3", ds.Tables[2]);
             viewer.LocalReport.DataSources.Add(datasource);

             viewer.LocalReport.EnableExternalImages = true;
  
             viewer.LocalReport.EnableHyperlinks = true;
             viewer.LocalReport.Refresh();
             
           byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "inline; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }

        public void getInBoundExl()
        {
            string To = ""; string From = ""; string Agent = ""; string Farmer = "";
            clsSubmitModel oSubmit = new clsSubmitModel();
            DateTime dFrom = Convert.ToDateTime("01/01/1900");
            DateTime dTo = Convert.ToDateTime("01/01/2100");
            DataTable dt;
            if (From.ToString() != "") dFrom = Convert.ToDateTime(oSubmit.GetDateFormat(From)); 
            if (To.ToString() != "") dTo = Convert.ToDateTime(oSubmit.GetDateFormat(To));

         dt =   oSubmit.GetData("Exec GetInBound @Agent ='" + Agent + "' ,@Farmer ='" + Farmer + "' , @From ='" + dFrom + "', @To ='" + dTo + "'");

         ExportExcel(dt, "InBound");
         //ExportPDF(dt, "InBound.pdf");

        }
        public void getUserWork()
        {
            string To = ""; string From = ""; int iUserCode  = 0; string Farmer = "";
            clsSubmitModel oSubmit = new clsSubmitModel();

            To = Request.QueryString["To"];
            From = Request.QueryString["From"];
            if (Request.QueryString["iUserCode"].ToString() != "") iUserCode =Convert.ToInt32( Request.QueryString["iUserCode"].ToString());

            DateTime dFrom = Convert.ToDateTime("01/01/1900");
            DateTime dTo = Convert.ToDateTime("01/01/2100");
            DataTable dt;

            clsUserWork oProp = new clsUserWork();
            if (From.ToString() != "") oProp.worcudt = Convert.ToDateTime(oSubmit.GetDateFormat(From));
            if (To.ToString() != "") oProp.wordate = Convert.ToDateTime(oSubmit.GetDateFormat(To));
             
            oProp.woruser = iUserCode;

            dt = oSubmit.getUserWork(oProp );

            ExportExcel(dt, "UserWork"); 

        }
        public void ExportExcel(DataTable dt ,string sFileName )
        {
            GridView gr = new GridView(); 
            gr.DataSource = dt; 
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Buffer = true; 
            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + sFileName + ".xls");
            System.Web.HttpContext.Current.Response.Charset = "";
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw); 
            gr.AllowPaging = false;
            gr.DataBind(); 
            try
            {
                gr.RenderControl(hw);
            }
            catch (Exception ex) { } 
            System.Web.HttpContext.Current.Response.Output.Write(sw.ToString());
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End(); 
        }
          
        private void PuSl_PDF(int dCode)
        {
            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            string fileName = "PUSL"; string rPath = Server.MapPath("~/Report/DigiclayBillGST.rdlc");

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Report/DigiclayBillGST.rdlc");
            viewer.LocalReport.ReportEmbeddedResource = rPath;

            viewer.LocalReport.DataSources.Clear();

            clsSubmitModel oSubmit = new clsSubmitModel();
            DataSet ds = oSubmit.GetDataSet("sp_GetSalesBill @mstcode=" + dCode + ",@compcode=" + Convert.ToInt32(Request.QueryString["comp"]) + ",@msttype=" + Convert.ToInt32(Request.QueryString["msttype"]), true);

            ReportDataSource datasource = new ReportDataSource("DataSet1", SetRows(ds.Tables[0], 14));
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet2", ds.Tables[1]);
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet3", ds.Tables[2]);
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet4", ds.Tables[3]);
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.EnableExternalImages = true;

            string imagePath = "";
            //if (Convert.ToInt32(Request.QueryString["comp"]) == 60) imagePath = "~/img/digi-logo.png"; else imagePath = "~/img/digi-logo.png";
            imagePath = "~/img/" + SessionMaster.CompCode + ".png";

            imagePath = new Uri(Server.MapPath(imagePath)).AbsoluteUri;
            ReportParameter p32 = new ReportParameter("ImgPath", imagePath);
            viewer.LocalReport.SetParameters(new ReportParameter[] { p32 });

            viewer.LocalReport.EnableHyperlinks = true;
            viewer.LocalReport.Refresh();

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "inline; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }

        private void Voucher_PDF(int dCode, string Type)
        {
            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            string fileName = "Voucher"; string rPath = Server.MapPath("~/Report/PAYMENT.rdlc");

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Report/PAYMENT.rdlc");
            viewer.LocalReport.ReportEmbeddedResource = rPath;

            viewer.LocalReport.DataSources.Clear();

            clsSubmitModel oSubmit = new clsSubmitModel();
            //DataSet ds = oSubmit.GetDataSet("sp_GetSalesBill @mstcode=" + dCode + ",@compcode=" + Convert.ToInt32(Request.QueryString["comp"]) + ",@msttype=" + Convert.ToInt32(Request.QueryString["msttype"]), true);

            DataTable dt = oSubmit.GetVoucher(Convert.ToInt16(Type), SessionMaster.CompCode, dCode);

            ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.EnableExternalImages = true;

            string imagePath = "";
            imagePath = "~/img/" + SessionMaster.CompCode + ".png";
            imagePath = new Uri(Server.MapPath(imagePath)).AbsoluteUri;
            ReportParameter p32 = new ReportParameter("ImgPath", imagePath);
            viewer.LocalReport.SetParameters(new ReportParameter[] { p32 });

            viewer.LocalReport.EnableHyperlinks = true;
            viewer.LocalReport.Refresh();

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "inline; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }
           
        //private void CreatePDFForGST_Book()
        //{ 
        //    string rPath = Server.MapPath("~/Report/gstReceiverSummary.rpt");

        //    string  fileName = "gstReceiverSummary";
        //    ReportDocument rd = new ReportDocument();
        //    rd.Load(rPath ); 

        //    clsSubmitModel oData = new clsSubmitModel();
        //    DataTable dt = oData.GetData("exec GetGstBook @From='" + oData.GetDateFormat(Request.QueryString["sFrom"]) + "' , @TO = '" + oData.GetDateFormat(Request.QueryString["sTO"]) + "'");


        //    rd.SetDataSource(dt);
        //    //rd.SetParameterValue("Type", Type);
        //    rd.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, fileName);
        //    rd.Dispose();
        //}


        private void CreatePDFForGST_Book()
        {
            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            string fileName = "gstReceiver"; string rPath = Server.MapPath("~/Report/gstReceiver.rdlc");

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Report/gstReceiver.rdlc");
            viewer.LocalReport.ReportEmbeddedResource = rPath;

            viewer.LocalReport.DataSources.Clear();

            clsSubmitModel oSubmit = new clsSubmitModel();

            DataTable dt = oSubmit.GetData("exec GetGstBook @CompCode =" + SessionMaster.CompCode + ", @From='" + oSubmit.GetDateFormat(Request.QueryString["sFrom"]) + "' , @TO = '" + oSubmit.GetDateFormat(Request.QueryString["sTO"]) + "', @TypeID=" + Request.QueryString["TypeID"]);

            ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.EnableExternalImages = true;

            ReportParameter pFrom = new ReportParameter("From", Request.QueryString["sFrom"]);
            ReportParameter pTo = new ReportParameter("To", Request.QueryString["sTO"]);
            viewer.LocalReport.SetParameters(new ReportParameter[] { pFrom, pTo });
            //viewer.LocalReport.SetParameters(new ReportParameter[] { pTo });

            viewer.LocalReport.EnableHyperlinks = true;
            viewer.LocalReport.Refresh();

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "inline; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }

        private void SalesBook()
        {
            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            string fileName = "SaleBook"; string rPath = Server.MapPath("~/Report/SaleBook.rdlc");

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Report/SaleBook.rdlc");
            viewer.LocalReport.ReportEmbeddedResource = rPath;

            viewer.LocalReport.DataSources.Clear();

            clsSubmitModel oSubmit = new clsSubmitModel();

            DataTable dt = oSubmit.GetData("exec GetSaleBook @Compcode =" + SessionMaster.CompCode + " , @From='" + oSubmit.GetDateFormat(Request.QueryString["sFrom"]) + "' , @TO = '" + oSubmit.GetDateFormat(Request.QueryString["sTO"]) + "'");

            ReportDataSource datasource = new ReportDataSource("DataSet1", dt);
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.EnableExternalImages = true;

            ReportParameter pFrom = new ReportParameter("From", Request.QueryString["sFrom"]);
            ReportParameter pTo = new ReportParameter("To", Request.QueryString["sTO"]);
            viewer.LocalReport.SetParameters(new ReportParameter[] { pFrom, pTo });
            //viewer.LocalReport.SetParameters(new ReportParameter[] { pTo });

            viewer.LocalReport.EnableHyperlinks = true;
            viewer.LocalReport.Refresh();

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "inline; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }
        private void GSTR1()
        {
            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            string fileName = "GST_R1"; string rPath = Server.MapPath("~/Report/GST_R1.rdlc");
            //SessionManager.CompanyId

            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Report/GST_R1.rdlc");
            viewer.LocalReport.ReportEmbeddedResource = rPath;

            viewer.LocalReport.DataSources.Clear();

            clsSubmitModel oSubmit = new clsSubmitModel();
            DataSet ds = oSubmit.GetDataSet("exec GetGST_R1  @CompCode =" + SessionMaster.CompCode + ", @From='" + oSubmit.GetDateFormat(Request.QueryString["sFrom"]) + "' , @TO = '" + oSubmit.GetDateFormat(Request.QueryString["sTO"]) + "'", true);


            ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables[0]);
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet2", ds.Tables[1]);
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet3", ds.Tables[2]);
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet4", ds.Tables[3]);
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet5", ds.Tables[4]);
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.EnableExternalImages = true;

            ReportParameter pFrom = new ReportParameter("From", Request.QueryString["sFrom"]);
            ReportParameter pTo = new ReportParameter("To", Request.QueryString["sTO"]);
            viewer.LocalReport.SetParameters(new ReportParameter[] { pFrom, pTo });

            viewer.LocalReport.EnableHyperlinks = true;
            viewer.LocalReport.Refresh();

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "inline; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }
        private void GSTR2()
        {
            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            string fileName = "GST_R2"; string rPath = Server.MapPath("~/Report/GST_R2.rdlc");
            //SessionManager.CompanyId

            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Report/GST_R2.rdlc");
            viewer.LocalReport.ReportEmbeddedResource = rPath;

            viewer.LocalReport.DataSources.Clear();

            clsSubmitModel oSubmit = new clsSubmitModel();
            DataSet ds = oSubmit.GetDataSet("exec GetGST_R2  @CompCode =" + SessionMaster.CompCode + ", @From='" + oSubmit.GetDateFormat(Request.QueryString["sFrom"]) + "' , @TO = '" + oSubmit.GetDateFormat(Request.QueryString["sTO"]) + "'", true);


            ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables[0]);
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet2", ds.Tables[1]);
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet3", ds.Tables[2]);
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet4", ds.Tables[3]);
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.EnableExternalImages = true;

            ReportParameter pFrom = new ReportParameter("From", Request.QueryString["sFrom"]);
            ReportParameter pTo = new ReportParameter("To", Request.QueryString["sTO"]);
            viewer.LocalReport.SetParameters(new ReportParameter[] { pFrom, pTo });

            viewer.LocalReport.EnableHyperlinks = true;
            viewer.LocalReport.Refresh();

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "inline; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }
        private void GSTR3B()
        {
            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            string fileName = "GSTR_3B"; string rPath = Server.MapPath("~/Report/GSTR_3B.rdlc");
            //SessionManager.CompanyId

            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Report/GSTR_3B.rdlc");
            viewer.LocalReport.ReportEmbeddedResource = rPath;

            viewer.LocalReport.DataSources.Clear();

            clsSubmitModel oSubmit = new clsSubmitModel();
            DataSet ds = oSubmit.GetDataSet("exec sp_fillform3b @pCompCode =" + SessionMaster.CompCode + ", @pItemComp =" + SessionMaster.CompCode + ",@pAcctComp =" + SessionMaster.CompCode + ", @pFromDate='" + oSubmit.GetDateFormat(Request.QueryString["sFrom"]) + "' , @pUptoDate = '" + oSubmit.GetDateFormat(Request.QueryString["sTO"]) + "'", true);

            ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables[0]);
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet2", ds.Tables[1]);
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet3", ds.Tables[2]);
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet4", ds.Tables[3]);
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet5", ds.Tables[4]);
            viewer.LocalReport.DataSources.Add(datasource);
            datasource = new ReportDataSource("DataSet6", ds.Tables[5]);
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.EnableExternalImages = true;

            //ReportParameter pFrom = new ReportParameter("Month", Request.QueryString["sFrom"]);
            //ReportParameter pTo = new ReportParameter("Year", Request.QueryString["sTO"]);
            //viewer.LocalReport.SetParameters(new ReportParameter[] { pFrom, pTo });

            viewer.LocalReport.EnableHyperlinks = true;
            viewer.LocalReport.Refresh();

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "inline; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }

        private void LedgerPrint(string sFrom, string sTO, string sParty, int WithItem = 1, int Compcode = 0)
        {
            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            string fileName = "PUSL"; string rPath = Server.MapPath("~/Report/Ledger.rdlc");

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Report/Ledger.rdlc");
            viewer.LocalReport.ReportEmbeddedResource = rPath;

            viewer.LocalReport.DataSources.Clear();

            clsSubmitModel oSubmit = new clsSubmitModel();

            DataTable dtComp;
            dtComp = oSubmit.GetData("select Compadd1 + ' ' + CompCity + ' ' + CompStat Address,Compmail ,Compmobi ,  * from Company where Compcode =" + Compcode);
            string Address = "", Email = "", Mobile = "";
            if (dtComp.Rows.Count > 0)
            {
                Address = dtComp.Rows[0]["Address"].ToString();
                Email = dtComp.Rows[0]["Compmail"].ToString();
                Mobile = dtComp.Rows[0]["Compmobi"].ToString();
            }


            if (sFrom == null || sFrom == "" || sFrom == "undefined") sFrom = "01/04/" + DateTime.Now.Year;
            if (sTO == null || sTO == "" || sTO == "undefined") sTO = "31/03/" + ((Int32)DateTime.Now.Year + 1);

            DataTable dt = oSubmit.GetData("Exec sp_GetLedger_PDF @CompList = '" + Compcode + "', @AcctList = '" + sParty + "', @BdDateFrom = '" + oSubmit.GetDateFormat(sFrom) + "', @BdDateTo = '" + oSubmit.GetDateFormat(sTO) + "' ,@pItemDtls= " + WithItem);

          ReportDataSource datasource = new ReportDataSource("DataSet1", SetRows(dt,0));
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.EnableExternalImages = true;

            ReportParameter pFrom = new ReportParameter("From", Request.QueryString["sFrom"]);
            ReportParameter pTo = new ReportParameter("To", Request.QueryString["sTO"]);
            viewer.LocalReport.SetParameters(new ReportParameter[] { pFrom, pTo });
            //ReportParameter pAddress = new ReportParameter("Address", Address);
            //ReportParameter pEmail = new ReportParameter("Email", Email);
            //ReportParameter pMobile = new ReportParameter("Mobile", Mobile);
            //viewer.LocalReport.SetParameters(new ReportParameter[] { pFrom, pTo, pAddress, pEmail, pMobile });


            viewer.LocalReport.EnableHyperlinks = true;
            viewer.LocalReport.Refresh();

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
   
            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "inline; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }

        private void TrialBal(string From, string TO, int sParty, int WithItem = 1, int Compcode = 0, int StateID = 0, int Executive = 0)
        {
            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty; 
            string fileName = "PUSL"; string rPath = Server.MapPath("~/Report/TrialBal.rdlc");

            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;

            viewer.LocalReport.ReportPath = Server.MapPath("~/Report/TrialBal.rdlc");
            viewer.LocalReport.ReportEmbeddedResource = rPath; 
            viewer.LocalReport.DataSources.Clear();

            clsSubmitModel oSubmit = new clsSubmitModel();
             
            DataTable dt;
            string sDealer = ""; string sState = "";
            if (sParty > 0) sDealer = "and acctcode = " + sParty;
            if (StateID > 0) sState = "and acctstat = " + StateID;

            if (Executive > 0)
            {
                dt = oSubmit.GetData("select a.* from  [udf_GetTrialBalQuick]('" + oSubmit.GetDateFormat(From) + "', '" + oSubmit.GetDateFormat(TO) + "',  " + Compcode + " , 0)  a Left join tblDistributor b on a.acctDeal = b.mstCode Left join EmpAllotMst c on b.mstCode = c.DealerID where groucode = 21 and empID = (select empcode from Employee where usecode = " + Executive + ") " + sDealer + sState + " order by grouposi , grouname ,acctname ");
            }
            else
            {
                dt = oSubmit.GetData("select * from [udf_GetTrialBalQuick]('" + oSubmit.GetDateFormat(From) + "', '" + oSubmit.GetDateFormat(TO) + "', " + Compcode + " , 0) where groucode = 21 " + sDealer + sState + " order by grouposi , grouname ,acctname ");
            }

           

          ReportDataSource datasource = new ReportDataSource("DataSet1", SetRows(dt,0));
            viewer.LocalReport.DataSources.Add(datasource);

            viewer.LocalReport.EnableExternalImages = true;

            ReportParameter pFrom = new ReportParameter("From", Request.QueryString["sFrom"]);
            ReportParameter pTo = new ReportParameter("To", Request.QueryString["sTO"]);
            viewer.LocalReport.SetParameters(new ReportParameter[] { pFrom, pTo });
          
            viewer.LocalReport.EnableHyperlinks = true;
            viewer.LocalReport.Refresh();

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
    
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "inline; filename=" + fileName + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }
    }
}