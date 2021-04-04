using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PenzugySzovetseg.aje {
  public partial class Fizetesek : System.Web.UI.Page {

    private RiportLekerdezesek lekerdezesek = new RiportLekerdezesek();
    private SzuroControl m_szuroControl = new SzuroControl();
    private List<string> filterek = new List<string>();
    private Nyomtatas m_nyomtatas = new Nyomtatas();

    protected void Page_Load(object sender, EventArgs e) {
      if (!IsPostBack) {
        m_szuroControl.FilterEvek(repEvek);
        m_szuroControl.FilterDatumok(repHonapok);
        //m_szuroControl.FilterVarosok(repVarosok);
        m_szuroControl.FilterLelkeszek(repLelkeszek);
      }
      filterek.AddRange(m_szuroControl.GetEvFilter(repEvek));
      filterek.AddRange(m_szuroControl.GetDatumFilter(repHonapok));
      //filterek.AddRange(m_szuroControl.GetVarosokFilter(repVarosok));
      filterek.AddRange(m_szuroControl.GetLelkeszekFilter(repLelkeszek));
      filterek.Add(" Lelkesz<>'' ");

      if (filterek.Count == 0) {
        filterek.Add(" Varos = 'XXXYYYZZZ33' ");
      }

      LoadStores(filterek);

    }

    public DataTable LoadStores(List<string> list) {
      DataTable table = lekerdezesek.FizetesLekerdezes(list);
      if (table != null && table.Rows.Count > 0) {
        repOsszesites.DataSource = table;
        repOsszesites.DataBind();
      }
      return table;
    }

    protected void btnImgPDF_Click(object sender, ImageClickEventArgs e) {
      string path = m_nyomtatas.Init(Request);
      List<int> honapok = new List<int>();
      for (int i = 0; i < repHonapok.Items.Count; i++) {
        CheckBox item = (CheckBox)repHonapok.Items[i].FindControl("chb");
        if (item.Checked) {
          Label lbl = (Label)repHonapok.Items[i].FindControl("lbl");
          honapok.Add(i);
        }
      }

      m_nyomtatas.PrintFizetesek(LoadStores(filterek), honapok);
      Response.Redirect(path);
    }

    protected void chb_DataBinding(object sender, EventArgs e) {
      CheckBox chb = sender as CheckBox;
      if (chb != null) {
        RepeaterItem rep = chb.BindingContainer as RepeaterItem;
        if (rep != null && rep.ItemIndex + 1 == DateTime.Now.Month) {
          chb.Checked = true;
        }
      }
    }
  }
}