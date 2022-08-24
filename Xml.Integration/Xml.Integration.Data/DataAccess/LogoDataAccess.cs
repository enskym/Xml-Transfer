using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xml.Integration.Data.Helper;

namespace Xml.Integration.Data.DataAccess
{
   public class LogoDataAccess
    {
        public static string GetCariKoduByTax(string nr)
        {
            try
            {
                using (var db = new SqlConnection(UtilityHelper.Connection))
                {
                    var sql = "select top 1 CODE  from LG_051_CLCARD  WITH (NOLOCK) WHERE TAXNR = @nr";
                    return db.Query<string>(sql, new { nr }).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
