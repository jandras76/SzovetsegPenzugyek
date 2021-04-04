using PenzugySzovetseg.aje;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PenzugySzovetseg.Models;
using System.Data;
namespace PenzugySzovetseg.aje {
  public partial class NaptarosAlkalmak : BaseGridPage {
    private SzuroControl m_szuroControl = new SzuroControl();
    private List<string> filterek = new List<string>();
    private DataTable m_FilteredElszamolasiIdoszakok = null;

    private DataTable _FilteredElszamolasiIdoszakok() {
      if (m_FilteredElszamolasiIdoszakok == null) {
        m_FilteredElszamolasiIdoszakok = sqlLiteAccess.GetDataTable("ElszamolasIdoszakok", new List<string> { "Zarolt=0" });
      }
      return m_FilteredElszamolasiIdoszakok;
    }

    protected override void Page_Load(object sender, EventArgs e) {
      if (!IsPostBack) {
        m_szuroControl.FilterEvek(repEvek);
        m_szuroControl.FilterDatumok(repHonapok);
        m_szuroControl.FilterVarosok(repVarosok);
        m_szuroControl.FilterLelkeszek(repLelkeszek);
        m_szuroControl.FilterKiadasTipusa(repKiadasokTipusa);
      }
      filterek.AddRange(m_szuroControl.GetEvFilter(repEvek));
      filterek.AddRange(m_szuroControl.GetDatumFilter(repHonapok));
      filterek.AddRange(m_szuroControl.GetVarosokFilter(repVarosok));
      if (repLelkeszek.Items.Count == 1) {
        CheckBox chb = repLelkeszek.Items[0].FindControl("chb") as CheckBox;
        if (chb != null) {
          chb.Checked = true;
          chb.Enabled = false;
        }
      }
      filterek.AddRange(m_szuroControl.GetLelkeszekFilter(repLelkeszek));
      filterek.AddRange(m_szuroControl.GetKiadasokTipusa(repKiadasokTipusa));
      if (filterek.Count == 0) {
        filterek.Add(" Varos = 'XXXYYYZZZ33' ");
      }
      base.Page_Load(sender, e);

    }

    protected override string _GetTablaNev() {
      return "Alkalmak";
    }

    protected override string _GetDataKey() {
      return "id";
    }



    protected override void gridView_RowDataBound(object sender, GridViewRowEventArgs e) {
      if (e.Row.RowType == DataControlRowType.DataRow) {
        //string lezarva = ;
        _CheckZaras(e);
        base.gridView_RowDataBound(sender, e);
      }
    }

    protected override List<ColumnProperty> _GetColumnNames() {
      List<ColumnProperty> list = new List<ColumnProperty>();

      var colId = new ColumnProperty("id");
      colId.Width = 0;
      colId.IncludeInUpdate = false;
      list.Add(colId);

      _SpecialAddNewColumnName = "ElszamoltIdoszak";
      var colLezarva = new ColumnProperty("ElszamoltIdoszak");
      colLezarva.Width = 0;
      colLezarva.IncludeInUpdate = false;
      list.Add(colLezarva);

      var colRowNumber = new ColumnProperty("Sor");
      colRowNumber.Width = 40;
      colRowNumber.IncludeInUpdate = false;
      colRowNumber.ReadOnly = true;
      list.Add(colRowNumber);

      var lelkesz = new ColumnProperty("Lelkesz");
      lelkesz.IsCombo = true;
      lelkesz.DenyEmpty = true;
      lelkesz.Width = 70;
      lelkesz.TablaKulsoKey = "Lelkeszek:Kod";
      colId.DenyEmpty = true;
      var curuser = HttpContext.Current.User.Identity.Name;
      List<string> userFilter = AJEHelpers.UserFilter(HttpContext.Current.User.Identity.Name, true);
      lelkesz.Filters = userFilter;
      list.Add(lelkesz);

      var kiadastipus = new ColumnProperty("KiadasTipus");
      kiadastipus.IsCombo = true;
      kiadastipus.DenyEmpty = true;
      kiadastipus.Width = 150;
      kiadastipus.TablaKulsoKey = "KiadasokTipusa:Tipus";
      kiadastipus.UpdateOtherField = "Honorarium";
      kiadastipus.UpdateOtherFieldDT = sqlLiteAccess.LoadData("select Osszeg,Tipus from KiadasokTipusa ");
      list.Add(kiadastipus);

      var honorarium = new ColumnProperty("Honorarium");
      honorarium.Width = 100;
      honorarium.IsDecimal = true;
      list.Add(honorarium);

      var datum = new ColumnProperty("Datum");
      // todo megnezni az idopot
      datum.IsDatum = true;
      datum.Width = 100;
      datum.Filters = m_szuroControl.GetDatumFilter(repHonapok);
      list.Add(datum);

      var elszamolasiIdoszak = new ColumnProperty("ElszamoltIdoszak");
      elszamolasiIdoszak.IsCombo = true;
      elszamolasiIdoszak.DenyEmpty = true;
      elszamolasiIdoszak.Width = 150;
      elszamolasiIdoszak.TablaKulsoKey = "ElszamolasIdoszakok:id";
      elszamolasiIdoszak.Filters = new List<string> { "Zarolt=0" };
      elszamolasiIdoszak.ComboObjectType = typeof(ElszamolasIdoszakok);
      list.Add(elszamolasiIdoszak);

      var varos = new ColumnProperty("Varos");
      varos.IsCombo = true;
      varos.TablaKulsoKey = "Varosok:Nev";
      varos.DenyEmpty = true;
      curuser = HttpContext.Current.User.Identity.Name;
      List<string> userFilterVaros = AJEHelpers.UserFilter(HttpContext.Current.User.Identity.Name);
      varos.Filters = userFilterVaros;
      list.Add(varos);

      var utazasOsszeg = new ColumnProperty("UtazasOsszeg");
      utazasOsszeg.IsDecimal = true;
      utazasOsszeg.Width = 100;
      list.Add(utazasOsszeg);

      return list;
    }

    protected override List<string> _GetFilters() {
      return filterek;
    }

    protected override string _GetOrderByField() {
      return "Datum";
    }

    protected override bool _GetAddRowCount() {
      return true;
    }

    private int _Negyedev(int honap) {
      if (honap < 4) return 1;
      if (honap >= 4 && honap < 7) return 2;
      if (honap >= 7 && honap < 10) return 3;
      if (honap >= 10 && honap < 13) return 4;
      return -1;
    }

    protected override string _SpecialAddNewColumn(DateTime datum) {

      DropDownList txt = (DropDownList)gridView.FooterRow.FindControl("inElszamoltIdoszak");
      var c = Convert.ToInt32(txt.SelectedValue);

      if (c < 0) {
        foreach (DataRow item in _FilteredElszamolasiIdoszakok().Rows) {
          var el = AJEHelpers.CreateItemFromRow<ElszamolasIdoszakok>(item);
          if (el.Ev == datum.Year && el.Negyedev == _Negyedev(datum.Month)) {
            txt.SelectedValue = el.Id.ToString();
            return "";
          }
        }
      }

      return "A megadott elszamolasi idoszak nem letezik, vagy le van mar zarva!";
    }

    protected void chb_OnDataBinding(object sender, EventArgs e) {
      CheckBox chb = sender as CheckBox;
      if (chb != null) {
        RepeaterItem rep = chb.BindingContainer as RepeaterItem;
        if (rep != null && rep.ItemIndex + 1 == DateTime.Now.Month) {
          chb.Checked = true;
        } else if (rep != null && rep.ItemIndex + 2016 == DateTime.Now.Year) {
          chb.Checked = true;
        }
      }
    }
  }
}