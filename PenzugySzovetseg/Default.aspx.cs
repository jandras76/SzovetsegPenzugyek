using PenzugySzovetseg.aje;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace PenzugySzovetseg {
  public partial class _Default : Page {

    private RiportLekerdezesek lekerdezesek = new RiportLekerdezesek();
    private SzuroControl m_szuroControl = new SzuroControl();
    private List<string> filterek = new List<string>();
    private Nyomtatas m_nyomtatas = new Nyomtatas();

    protected void Page_Load(object sender, EventArgs e) {
      if (!IsPostBack) {
        m_szuroControl.FilterEvek(repEvek);
        m_szuroControl.FilterDatumok(repHonapok);
        m_szuroControl.FilterVarosok(repVarosok);
        //m_szuroControl.FilterLelkeszek(repLelkeszek);
      }
      filterek.AddRange(m_szuroControl.GetEvFilter(repEvek));
      filterek.AddRange(m_szuroControl.GetDatumFilter(repHonapok));
      filterek.AddRange(m_szuroControl.GetVarosokFilter(repVarosok));
      //filterek.AddRange(m_szuroControl.GetLelkeszekFilter(repLelkeszek));

      if (filterek.Count == 0) {
        filterek.Add(" Varos = 'XXXYYYZZZ33' ");
      }

      LoadStores("Alkalmak", filterek);

    }

    public void LoadStores(string tablaNev, List<string> list) {
      DataTable table = lekerdezesek.FoOldalLekerdezes(list);
      if (table != null) {
        repOsszesites.DataSource = table;
        repOsszesites.DataBind();
      }
    }

    protected void gridVarosok_SelectedIndexChanged(object sender, EventArgs e) {

    }

    protected void repOsszesites_ItemCommand(object source, RepeaterCommandEventArgs e) {

    }

    protected void btnPDF_Click(object sender, EventArgs e) {

    }

    protected void btnImgPDF_Click(object sender, ImageClickEventArgs e) {

      string path = m_nyomtatas.Init(Request);
      DataTable table = lekerdezesek.GyulekezetekLekerdezes(filterek);
      if (table != null && table.Rows.Count > 0) {
        repOsszesites.DataSource = table;
        repOsszesites.DataBind();
      }
      m_nyomtatas.PrintVarosok(table,chbIncludeTagdij.Checked);
      Response.Redirect(path);

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