using System.Collections.Generic;
using System.Web.Mvc;

namespace CompxERP.Models
{
    public class Studdet
    {
        public int studType { get; set; }
        public int studCode { get; set; }
        public string studname { get; set; }
		public int studCity{ get; set; }//170706
		public string studAlia { get; set; }//170801
        public List<SelectListItem> studCityData { get; set; }//170706
    }
}