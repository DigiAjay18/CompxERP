using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompxERP.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter user name")]
        //[DataType(DataType.EmailAddress)]
        [Display(Name = "User Name")]
        [StringLength(30)]
        public string use_name { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(10)]
        public string use_pass { get; set; }

    }


    [Table("loginusers")]
    public class loginusers
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int usecode { get; set; }
        public int usetype { get; set; }
        public int CompCode { get; set; }
        public string usename { get; set; }
        public string usepass { get; set; }
        public string UserNM { get; set; }  
        public string Remark { get; set; } 

        public string UsePhone { get; set; }
        public string UseEmail { get; set; }
        public int UseDesi { get; set; }
        public int UseDepa { get; set; }
        public int UseDealer { get; set; }

    }


    public class Distributor
    {

        public string hdnDealerCode { get; set; }
        public int DistributorID { get; set; }
        public int usetype { get; set; }
        public int UserID { get; set; }
        public bool IsDistributor { get; set; }

        public string Dis_ID { get; set; }
        public string Dis_Date { get; set; }
        public string Dis_Name { get; set; }
        public string Dis_Country { get; set; }
        public string Dis_State { get; set; }
        public string Dis_City { get; set; }
        public string Dis_Add1 { get; set; }
        public string LandLine { get; set; }
        public string Email { get; set; }
        public string GSTIN { get; set; }
        public string Skype { get; set; }
        public string usename { get; set; }
        public string usepass { get; set; }

        public string Dis_Add2 { get; set; }
        public string Dis_Add3 { get; set; }
        public string Dis_pobox { get; set; }
        public string Dis_City_I { get; set; }
        public string ContactPerson { get; set; }
        public string Mob2 { get; set; }
        public string Mob1 { get; set; }
        public string DealCode { get; set; }
        public string CityNM { get; set; }

        public string ChequeNo  { get; set; } 
        public string CP_I  { get; set; } 
        public string CP_II  { get; set; } 
        public string CP_NO_I  { get; set; } 
        public string  CP_NO_II { get; set; } 
        public DateTime DOB  { get; set; } 
        public DateTime DOJ  { get; set; } 
        public DateTime AnniDt  { get; set; }   
  
        public List<clsList> lstBrand { get; set; }
        public List<clsList> lstProduct { get; set; }

    }

    public class clsList
    {
        public string NameNM { get; set; }
        public int NameID { get; set; }
        public string NameDesc { get; set; }

        public clsList(DataRow dr)
        {
            if (dr.Table.Columns.Contains("NameNM") && !Convert.IsDBNull(dr["NameNM"]))
                this.NameNM = Convert.ToString(dr["NameNM"]);
            if (dr.Table.Columns.Contains("NameID") && !Convert.IsDBNull(dr["NameID"]))
                this.NameID = Convert.ToInt32(dr["NameID"]);
            if (dr.Table.Columns.Contains("NameDesc") && !Convert.IsDBNull(dr["NameDesc"]))
                this.NameDesc = Convert.ToString(dr["NameDesc"]);
        }
    }

    public class clsNotify
    {

        public int ID { get; set; }
        public int For { get; set; }
        public int BrandID { get; set; }
        public int StateID { get; set; }
        public int CityID { get; set; }
        public int NameID { get; set; }
        public int HeaderID { get; set; }
        public string Msg { get; set; }
        public string sFrom { get; set; }
        public string sTo { get; set; }

        public System.Nullable<System.DateTime> From { get; set; }
        public System.Nullable<System.DateTime> To { get; set; }

    }

    public class Result
    {

        public int ID { get; set; }
        public string Msg { get; set; }

    }




    //[Table("branch")]
    //public class branch
    //{
    //    [Key]
    //    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    //    public int godocode { get; set; }
    //    // public int    compcode { get; set; }
    //    public string gododesc { get; set; }
    //    public string godoaddr { get; set; }
    //    //public string godocity { get; set; }
    //    //public string godostat { get; set; }
    //    //public string godophon { get; set; }
    //    //public string godoalia { get; set; }
    //    //public int godoplac { get; set; }
    //    //public int godounde { get; set; }
    //    //public string godosort { get; set; }
    //    //public string godopass { get; set; }
    //    //public string godotype { get; set; }
    //    //public int godocont { get; set; }
    //    //public string godosize { get; set; }
    //}


    [Table("menutree")]
    public class menutree
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int parentid { get; set; }
        public string text { get; set; }
        public int value { get; set; }

    }
}