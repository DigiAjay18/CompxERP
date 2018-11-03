using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompxERP.Models
{
    public class clsItemain
    {
        public int compcode { get; set; }
        public int itemcode { get; set; }
        public int itemgrou { get; set; }
        public int itemsrno { get; set; }
        public int itemnumb { get; set; }
        public int itemsort { get; set; }
        public int itembold { get; set; }
        public int itemvalu { get; set; }
        public int UnitID { get; set; }
        public int itmSize { get; set; }
        public int itebesic { get; set; }

        public int CatID { get; set; }
        public int SubCatID { get; set; }


        public string itemdisc { get; set; }
        public string itemname { get; set; }
        public string itempart { get; set; }
        public string itemalia { get; set; }
        public string itemhind { get; set; }
        public string itemtype { get; set; }
        public string itedrwng { get; set; }
        public string itemhsnc { get; set; }
        public string itemtext { get; set; }
        public string itemrefn { get; set; }
        public string Cate { get; set; }
        public string SubCate { get; set; }
        public string UnitName { get; set; }
        public string itgpnamec { get; set; }
        public string itgpnamesc { get; set; }
        public string itgpaliac { get; set; }
        public string itgpaliasc { get; set; }
        public string itgptyp { get; set; }

        public decimal itemopbl { get; set; }
        public decimal itemclbl { get; set; }
        public decimal itemmini { get; set; }
        public decimal itemmaxi { get; set; }
        public decimal itemrate { get; set; }
        public decimal itlastrat { get; set; }
        public decimal itemvatr { get; set; }
        public decimal itemmrpv { get; set; }
        public decimal itemgstr { get; set; }
        public decimal itemcess { get; set; }
        public decimal iteper { get; set; }
        public decimal itemPrcnt { get; set; }
        public decimal itmpric { get; set; }
        public DateTime itemexp { get; set; }
  
    }
}