using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompxERP.Models
{
    public class clsDistributor
    {

public int  MstCode {get; set;}
public int DistributorID { get; set; }
public int CountryID { get; set; }
public int StateID { get; set; }
public int CityID { get; set; }
public int CityID_I { get; set; }

public string DisName  { get; set; }
public string Add_I  { get; set; }
public string Add_II  { get; set; }
public string Add_III  { get; set; }
public string Add_IV  { get; set; }
public string PO_Box  { get; set; }
public string ContactPerson  { get; set; }
public string Mob1  { get; set; }
public string Mob2  { get; set; }
public string LandLine  { get; set; }
public string Email  { get; set; }
public string Skype  { get; set; }
public string GSTIN  { get; set; }
public string DealCode  { get; set; }
public string CityNM  { get; set; }

public string ChequeNo { get; set; }
public string CP_I { get; set; }
public string CP_II { get; set; }
public string CP_NO_I { get; set; }
public string CP_NO_II { get; set; }
public string DOB { get; set; }
public string DOJ { get; set; }
public string AnniDt { get; set; }   
  

public string Brand { get; set; }
public string Product { get; set; }
    }

   public class clsComplaint
    {
       public string CompNo  { get; set; }
       public string CustNM  { get; set; }
       public string ModelNo  { get; set; }
       public string InvNo  { get; set; }
       public string Remark_Cust	  { get; set; }
       public string Remark_Eng	  { get; set; }
        
       public int  SrvType		  { get; set; }
       public int  EmpID		  { get; set; }
       public string  ItemID		  { get; set; }
       public int  DealerID	  { get; set; }
       public int  CustID		  { get; set; }
       public int  UserID		  { get; set; }
       public int  StatusID	  { get; set; }
       public int  CompID		  { get; set; }

       public DateTime  CreatedOn		  { get; set; }
       
       public System.Nullable<System.DateTime> CompDt { get; set; } 
       public System.Nullable<System.DateTime> TentetiveTm { get; set; }
       public string sCompDt { get; set; }
       public string sTentetiveTm { get; set; }

    
       public decimal  	Charge  { get; set; }
 
       public int studType { get; set; }
       public int studCode { get; set; }

       public string studName { get; set; }
       public string studadd1 { get; set; }
       public string studadd2 { get; set; }
       public int studcity { get; set; }
       public int studstat { get; set; }
       public string studphon { get; set; }

       public int   IsNewCust { get; set; }

       public int IsPaid { get; set; }
       public int IsSrvcMode { get; set; }
       public int Category { get; set; }
         
}



   public class clsSchemeMst
   { 
       public string  SchName { get; set; }
       public string SchNo { get; set; }
       public string SchemeItm { get; set; }  
       public int MstCode  { get; set; } 
  
       public string sMstDate  { get; set; }  
       public string sClaimFrom  { get; set; }  
       public string sClaimTo  { get; set; }
       public string SchFrom { get; set; }
       public string SchTo { get; set; }  
   }

}