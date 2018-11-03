using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompxERP.Models
{
    public class clsEmpAllotMst
    {
       public int  MstCode {get; set;}
public int  EmpID {get; set;}
public int DealerID { get; set; }

public string  EmpNm { get; set; }
public string  DealerNm { get; set; }
public string  DisName { get; set; }
public string  ContactPerson { get; set; }
public string  Mob1 { get; set; }

public int? StatusID { get; set; }
public string sPlanDate { get; set; } 
    }
}
 