using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace CompxERP.Models
{
    public class Utility
    {
        public static List<T> BindList<T>(DataTable dt)
        {
            if (dt.Rows.Count == 0) return null;
            var fields = typeof(T).GetProperties();
            List<T> lst = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                var ob = Activator.CreateInstance<T>();
                foreach (var fieldInfo in fields)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        // Matching the columns with fields
                        if (fieldInfo.Name == dc.ColumnName)
                        {
                            // Get the value from the datatable cell
                            object value = dr[dc.ColumnName];
                            if (!dr.IsNull(dc))
                            {

                                //if (dc.DataType == typeof(decimal))
                                //{
                                //    fieldInfo.SetValue(ob,Convert.ToDouble(value), null);    
                                //}
                                //else 
                                // Set the value into the object
                                fieldInfo.SetValue(ob, value, null);
                                break;
                            }
                        }
                    }
                }
                lst.Add(ob);
            }
            return lst;
        }
    }
}