using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace PenzugySzovetseg.aje {
  public class SzuroControl {

    private SqlLiteAccess m_SqlLiteAccess = new SqlLiteAccess();

    public void FilterDatumok(Repeater repHonapok) {
      List<string> datumok = new List<string>() { "Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec" };
      _FilterGeneral(repHonapok, datumok);
    }

    public void FilterEvek(Repeater repEvek) {
      List<int> evek = AJEHelpers.Evek;
      _FilterGeneral(repEvek, evek);
    }

    public void FilterVarosok(Repeater repVarosok) {
      List<string> userFilter = AJEHelpers.UserFilter(HttpContext.Current.User.Identity.Name);
      _FilterGeneral(repVarosok, m_SqlLiteAccess.LoadData("SELECT NEV FROM VAROSOK", userFilter));
    }

    public void FilterLelkeszek(Repeater repLelkeszek) {
      List<string> userFilter = AJEHelpers.UserFilter(HttpContext.Current.User.Identity.Name, true);
      _FilterGeneral(repLelkeszek, m_SqlLiteAccess.LoadData("SELECT Kod FROM Lelkeszek", userFilter));
    }

    public void FilterKiadasTipusa(Repeater repKiadasTipusa) {
      _FilterGeneral(repKiadasTipusa, m_SqlLiteAccess.LoadData("SELECT Tipus FROM KiadasokTipusa"));
    }

    private static void _FilterGeneral(Repeater repeater, object dataSource) {
      repeater.DataSource = dataSource;
      repeater.DataBind();
    }

    public List<string> GetEvFilter(Repeater rep) {
      return _GetFilterGeneral(rep, (i, year) => string.Format("datepart(yyyy, Datum)={0}", year));
    }

    public List<string> GetDatumFilter(Repeater rep) {
      return _GetFilterGeneral(rep, (i, s) => string.Format("datepart(mm, Datum)={0}", (i + 1)));
    }

    public List<string> GetVarosokFilter(Repeater rep) {
      return _GetFilterGeneral(rep, (i, txt) => string.Format("Varos = '{0}'", txt));

    }

    public List<string> GetLelkeszekFilter(Repeater rep) {
      return _GetFilterGeneral(rep, (i, txt) => string.Format("Lelkesz = '{0}'", txt));
    }

    public List<string> GetKiadasokTipusa(Repeater rep) {
      return _GetFilterGeneral(rep, (i, txt) => string.Format("KiadasTipus = '{0}'", txt));
    }

    private List<string> _GetFilterGeneral(Repeater rep, Func<int, string, string> unknown) {
      List<string> filterek = new List<string>();
      string filter = "";
      for (int i = 0; i < rep.Items.Count; i++) {
        CheckBox item = (CheckBox)rep.Items[i].FindControl("chb");
        if (item.Checked) {
          if (filter != "") {
            filter += " OR ";
          }
          Label lbl = (Label)rep.Items[i].FindControl("lbl");
          filter += unknown(i, lbl.Text);
        }
      }
      if (!String.IsNullOrEmpty(filter)) {
        filterek.Add("(" + filter + ")");
      }
      return filterek;
    }

  }
}