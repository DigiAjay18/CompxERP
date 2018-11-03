using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompxERP.Models
{
    public class clsFollow
    { 
        public string  Remark {get; set ;}
        public string  L_No  {get; set ;}
        public  int  MstCode  {get; set ;}
        public  int F_by   {get; set ;}
        public  int L_ID   {get; set ;}
        public string Status { get; set; }
        public string Company { get; set; }
       
        public  int Status_II   {get; set ;}
        public  DateTime ? MstDate  {get; set ;}
        public  DateTime ? F_Date  {get; set ;}
        public int isLead { get; set; }
        public int F_Time { get; set; }
        public Decimal ? CommitPay { get; set; }
    }
    public class CodeGen
    {  
        public  int  MstCode  {get; set ;}
        public  int  compcode   {get; set ;}
        public  int  msttype   {get; set ;}
        public  DateTime  mstdate   {get; set ;}
        public  DateTime  StDate   {get; set ;}
        public  DateTime  LastDate   {get; set ;}
         
    }
}