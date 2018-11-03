using System;
using System.Collections.Generic;
using System.Data; 
using System.Web;


namespace CompxERP.Models
{
    public class clsPoItem
    {
		public clsPoItem(){}

        public string ddlItem { get; set; }
        public int ItemID { get; set; }
        public int Srno { get; set; }
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }  
        public decimal itdactprc { get; set; }
       
        public decimal Amt { get; set; }
        public string Remark { get; set; }

        public List<clsPoItem> ListArea { get; set; }
        public List<clsPoItem> LstItem { get; set; }
         
        public string ItmRemark { get; set; }

        public decimal TaxPer { get; set; }
        public decimal Tax_S { get; set; }
        public decimal Tax_C { get; set; }
        public decimal Tax_I { get; set; }
        public decimal Tax_Amt { get; set; }
        public decimal ItemNetAmt { get; set; }
        public string ItemNm { get; set; }

        public decimal itdgstrtv { get; set; }
        public decimal itddime { get; set; }
        public decimal DisAmt { get; set; }
        public decimal DisPer { get; set; }

        public decimal Cases { get; set; }
        public decimal AdjuAmt { get; set; }
        public decimal tpDrAmt { get; set; }
        public decimal tpCrAmt { get; set; }
        public int tpPartyID { get; set; }
        public string partyname { get; set; }
        public int acctgrou { get; set; }
        public string ItemRemark { get; set; }
        public int UnitID { get; set; }
        public int itdempo { get; set; }
        
        public string unitname { get; set; }
        public string unitcode { get; set; }
        public string trnItem { get; set; }

        public bool IsSelected { get; set; }
		public bool IsPrcnt { get; set; }//170706
        public int tnType{ get; set; }//170706
        public string TypeName { get; set; }//170706
		        public int? trninde { get; set; }//170829
        public decimal? trnexpa { get; set; }//170830
        public int? trntagv { get; set; }//170830
        public decimal? trnaddv { get; set; }//170830
        public int? trnledg { get; set; }//170830

        public string GSTStateVal { get; set; }//170830
		   
            public string LotNo { get; set; }
            public string PeLotNo { get; set; }
            public int Balance { get; set; }
   
        public int PrFrom { get; set; }
   public int PrTo { get; set; }

   public string sFrom { get; set; }
   public string sTo { get; set; }

public decimal itdperd { get; set; }
public decimal itdqtyd { get; set; }
        public decimal itddscp { get; set; }
         
   public int itdmill { get; set; }
   public int itdgodo { get; set; }
   public int itditem { get; set; }
   public decimal itdquan { get; set; }
   public decimal itdrate { get; set; }
   public decimal itdvate { get; set; }

   public string itdrema { get; set; } 
   public string Sitdexpd { get; set; }
   public Nullable<System.DateTime> itdexpd { get; set; }
   public HttpPostedFileWrapper ImageFile { get; set; }

   public Nullable<System.DateTime> schDate { get; set; }
         
   public string CateNm { get; set; }
   public string SubCateNm { get; set; }

        public clsPoItem(DataRow dr)
        {

   if (dr.Table.Columns.Contains("LotNo") && !Convert.IsDBNull(dr["LotNo"]))
       this.LotNo = Convert.ToString(dr["LotNo"]);
  if (dr.Table.Columns.Contains("PeLotNo") && !Convert.IsDBNull(dr["PeLotNo"]))
      this.PeLotNo = Convert.ToString(dr["PeLotNo"]);
   if (dr.Table.Columns.Contains("code") && !Convert.IsDBNull(dr["code"]))
       this.ItemID = Convert.ToInt32(dr["code"]);
   if (dr.Table.Columns.Contains("Balance") && !Convert.IsDBNull(dr["Balance"]))
       this.Balance = Convert.ToInt32(dr["Balance"]);   
                     

            if (dr.Table.Columns.Contains("AcctName") && !Convert.IsDBNull(dr["AcctName"]))
                this.partyname = Convert.ToString(dr["AcctName"]);

            if (dr.Table.Columns.Contains("AcctCode") && !Convert.IsDBNull(dr["AcctCode"]))
                this.tpPartyID = Convert.ToInt32(dr["AcctCode"]); 
           if (dr.Table.Columns.Contains("TypeCode") && !Convert.IsDBNull(dr["TypeCode"]))//170706
                tnType = Convert.ToInt32(dr["TypeCode"]);
            if (dr.Table.Columns.Contains("TypeName") && !Convert.IsDBNull(dr["TypeName"]))//170706
                TypeName = dr["TypeName"].ToString();
        }
        
    }
}