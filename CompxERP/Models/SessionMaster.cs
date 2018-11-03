using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompxERP.Models
{
    public class SessionMaster
    {
        public static int CompCode
        {
            get{return Convert.ToInt32(HttpContext.Current.Session["CompCode"]);}
            set{HttpContext.Current.Session["CompCode"] = value;}
        }
        public static int compunde
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["compunde"]); }
            set { HttpContext.Current.Session["compunde"] = value; }
        }
        public static int UserID 
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["UserID"]); }
            set { HttpContext.Current.Session["UserID"] = value; }
        }
        public static int UserType 
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["UserType"]); }
            set { HttpContext.Current.Session["UserType"] = value; }
        }
        public static bool isHide
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session["isHide"]); }
            set { HttpContext.Current.Session["isHide"] = value; }
        }
		public static string UserName//181030
        {
            get { return HttpContext.Current.Session["UserName"].ToString(); }
            set { HttpContext.Current.Session["UserName"] = value; }
        }
        public static string UserNM//181030
        {
            get { return HttpContext.Current.Session["UserNM"].ToString(); }
            set { HttpContext.Current.Session["UserNM"] = value; }
        }
    }
}