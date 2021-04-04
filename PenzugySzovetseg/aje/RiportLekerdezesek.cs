using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PenzugySzovetseg.aje {
  public class RiportLekerdezesek {
    private readonly SqlLiteAccess m_sqlLiteAccess = new SqlLiteAccess();

    public DataTable FoOldalLekerdezes(List<string> list) {
      string sql = "select Varos, sum(Honorarium) as Honorarium, sum(UtazasOsszeg) as Utazas," +
          "sum(Honorarium) + sum(UtazasOsszeg) as Osszesen from Alkalmak ";
      return m_sqlLiteAccess.LoadData(sql, list, "Varos", "Varos");
    }

    public DataTable GyulekezetekLekerdezes(List<string> list) {
      string sql = "select Varos,datepart(yyyy, Datum) as Ev,datepart(mm, Datum) as Honap, sum(Honorarium) as Honorarium, sum(UtazasOsszeg) as Utazas," +
          "sum(Honorarium) + sum(UtazasOsszeg) as Osszesen from Alkalmak ";
      return m_sqlLiteAccess.LoadData(sql, list, "Varos", "Varos,datepart(yyyy, Datum), datepart(mm, Datum)");
    }

    public DataTable FizetesLekerdezes(List<string> list) {
      string sql = "select Lelkesz,Varos, sum(Honorarium) as Honorarium, sum(UtazasOsszeg) as Utazas," +
          "sum(Honorarium) + sum(UtazasOsszeg) as Osszesen from Alkalmak ";
      return m_sqlLiteAccess.LoadData(sql, list, "Lelkesz", "Lelkesz,Varos");
    }

    }
}