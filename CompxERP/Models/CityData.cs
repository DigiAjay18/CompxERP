using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompxERP.Models
{
    [Table("citydet")]
    public class CityData
    {

         [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int cityType { get; set; }
        public int citycode { get; set; }
        [Required(ErrorMessage = "Please enter city name")]
        [Display(Name = "Enter City Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "City Name must be between 3 and 50 characters!")]
        //public IEnumerable<selectlist>
        public string cityname { get; set; }
        public int citydist { get; set; }
        public Int16 citydays { get; set; }
        public Int16 cityrute { get; set; }
        public string citydate { get; set; }
        public string cityexti { get; set; }
        public string cityhind { get; set; }
        public string citydrvp { get; set; }
        public string cityalia { get; set; }
        public string statname { get; set; }
        public int statecode { get; set; }
        public string stathind { get; set; }
        public List<CityData> ListArea { get; set; }
       // public System.Data.DataSet StoreCitydata { get; set; }
    }
}