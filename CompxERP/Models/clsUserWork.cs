using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompxERP.Models
{
    public class clsUserWork
    { 
        public  int woruser  {get;set ;}
        public  int wormode  {get;set ;}
        public  int worcomp  {get;set ;}
        public  int wortype  {get;set ;}
        public  int worcode  {get;set ;}

        public  string worsrno  {get;set ;}
        public  string worrema  {get;set ;}
        public  string worrfsr  {get;set ;}
        public  string worsysn  {get;set ;}
        public string IP_Add { get; set; }
        public string MstRema { get; set; }
        public string sMode { get; set; }

        public string MstChno { get; set; }

        public string  sworcudt { get; set; }
        //public DateTime worcudt { get; set; }
        //public DateTime wordate { get; set; }

        public System.Nullable<System.DateTime> worcudt  { get; set; }
        public System.Nullable<System.DateTime> wordate  { get; set; }

        public string  swordate { get; set; }
        public List<SelectListItem> lstUser { get; set; }
    }
}