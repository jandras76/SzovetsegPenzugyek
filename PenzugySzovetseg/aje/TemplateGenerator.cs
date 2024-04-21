using PenzugySzovetseg.aje;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using PenzugySzovetseg.Models;

namespace PenzugySzovetseg {
  public class TemplateGenerator : ITemplate // Class inheriting ITemplate
  {
    private ListItemType type;
    private ColumnProperty colProp;

    private SqlLiteAccess sqlLiteAccess = new SqlLiteAccess();
    private DataTable m_ElszamolasiIdoszakok = null;

    public TemplateGenerator(ListItemType t, ColumnProperty prop) {
      type = t;
      colProp = prop;
    }

    private DataTable _ElszamolasiIdoszakok(bool useFilter = true) {
      if (m_ElszamolasiIdoszakok == null) {
        List<string> filters = null;
        if (useFilter) {
          filters = colProp.Filters;
        }
        m_ElszamolasiIdoszakok = sqlLiteAccess.GetDataTable("ElszamolasIdoszakok", filters);
      }
      return m_ElszamolasiIdoszakok;
    }

    // Override InstantiateIn() method
    void ITemplate.InstantiateIn(System.Web.UI.Control container) {
      switch (type) {
        case ListItemType.EditItem:
          if (colProp.IsDatum) {
            _AddCalendar(container);
          } else if (colProp.IsCombo) {
            _AddCombo(container, Postback: !String.IsNullOrEmpty(colProp.UpdateOtherField));
          } else {
            TextBox txtBox = new TextBox();
            txtBox.ID = "txt" + colProp.Name;
            txtBox.Width = colProp.Width;
            txtBox.DataBinding += this.BindDataText;
            container.Controls.Add(txtBox);
          }
          break;
        case ListItemType.Item:
          Label lbl = new Label();
          lbl.ID = "lbl" + colProp.Name;
          lbl.DataBinding += this.BindData;
          lbl.Width = colProp.Width;
          container.Controls.Add(lbl);
          break;
        case ListItemType.Footer:
          if (colProp.IsDatum) {
            _AddCalendar(container, "in");
          } else if (colProp.IsCombo) {
            _AddCombo(container, true, !String.IsNullOrEmpty(colProp.UpdateOtherField));
          } else {
            TextBox txtIn = new TextBox();
            txtIn.ID = "in" + colProp.Name;
            txtIn.Text = "";
            txtIn.Width = colProp.Width;
            container.Controls.Add(txtIn);
          }
          break;
      }
    }

    private void _AddCombo(Control container, bool inField = false, bool Postback = false) {
      DropDownList cmbBox = new DropDownList();
      if (!inField) {
        cmbBox.ID = "cmb" + colProp.Name;
        cmbBox.DataBinding += this.BindDataTextCombo;
      } else {
        cmbBox.ID = "in" + colProp.Name;
      }

      if (Postback) {
        if (!inField) {
          cmbBox.SelectedIndexChanged += CmbBox_SelectedIndexChanged;
        } else {
          cmbBox.SelectedIndexChanged += CmbBox_SelectedIndexChangedIn;
        }
        cmbBox.AutoPostBack = Postback;
      }
      
      var tblNev = colProp.TablaKulsoKey.Split(':')[0];
      var colNev = colProp.TablaKulsoKey.Split(':')[1];
      bool bComboObjectType = colProp.ComboObjectType != null;

      DataTable table = null;
      if (!bComboObjectType) {
        table = sqlLiteAccess.GetDataTable(tblNev, colProp.Filters);
      } else {
        table = _ElszamolasiIdoszakok();
      }

      if (table != null && table.Rows.Count > 0) {
        if (!bComboObjectType) {
          cmbBox.Items.Add("");
        } else if (colProp.ComboObjectType == typeof(ElszamolasIdoszakok)) {
          ListItem listItem = new ListItem();
          listItem.Text = "Egyezik a Dátummal";
          listItem.Value = "-1";
          cmbBox.Items.Add(listItem);
          cmbBox.DataBinding += this.BindElszamolasiIdoszak;
        }
        foreach (DataRow item in table.Rows) {
          if (!bComboObjectType) {
            cmbBox.Items.Add(item[colNev].ToString());
          } else if (colProp.ComboObjectType == typeof(ElszamolasIdoszakok)) {
            ListItem listItem = new ListItem();
            var elem = AJEHelpers.CreateItemFromRow<ElszamolasIdoszakok>(item);
            listItem.Text = elem.ToString();
            listItem.Value = elem.Id.ToString();
            cmbBox.Items.Add(listItem);
          }
        }
      }

      cmbBox.Width = colProp.Width;
      container.Controls.Add(cmbBox);

    }

    private void _AddCalendar(Control container, string prefix = "dd") {
      DropDownList ddListYear = new DropDownList();
      ddListYear.ID = prefix + "Y" + colProp.Name;
      if (prefix == "dd") {
        ddListYear.Items.Add("");
      }

      //for (int i = 0; i < AJEYear.SupportedYears.Count; i++) {
      //  ddListYear.Items.Add(AJEYear.SupportedYears[i].ToString());
      //  if (AJEHelpers.Evek[i] == DateTime.Now.Year) {
      //    ddListYear.SelectedIndex = i;
      //  }
      //}

    foreach (var item in AJEYear.Items) {
                ddListYear.Items.Add(item.ToString());
                if (item.ID == DateTime.Now.Year) {
                    ddListYear.SelectedIndex = item.ID;
                }
            }

      if (prefix == "dd") ddListYear.DataBinding += this.BindDataCalendar;
      container.Controls.Add(ddListYear);

      DropDownList ddListMonth = new DropDownList();
      ddListMonth.ID = prefix + "M" + colProp.Name;
      if (prefix == "dd") {
        ddListMonth.Items.Add("");
      }

      List<int> filteredMonthList = new List<int>();

      //"(datepart(mm, Datum)=9 OR datepart(mm, Datum)=10 OR datepart(mm, Datum)=11 OR datepart(mm, Datum)=12)"
      string monthFiltering = String.Empty;
      if (colProp.Filters != null) {
        foreach (string s in colProp.Filters) {
          if (s.Contains("datepart(mm, Datum)")) {
            string newS = s.Replace("datepart(mm, Datum)", "d").Replace("(", "").Replace(")", "");
            string[] tomb1 = null;
            if (newS.Contains(" OR ")) {
              monthFiltering = newS.Replace(" OR ", ";");
              tomb1 = monthFiltering.Split(';');
            } else {
              tomb1 = new[] { newS };
            }
            Trace.WriteLine(tomb1);
            foreach (string szam in tomb1) {
              if (szam.Contains("=")) {
                int honap = Convert.ToInt32(szam.Split('=')[1]);
                filteredMonthList.Add(honap);
              }
            }
            break;
          }
        }
      }


      for (int i = 1; i < 13; i++) {
        if (filteredMonthList.Contains(i) || filteredMonthList.Count == 0) {
          ddListMonth.Items.Add(i.ToString());
        }
      }

      if (prefix == "dd") ddListMonth.DataBinding += this.BindDataCalendar;
      container.Controls.Add(ddListMonth);

      DropDownList ddListDay = new DropDownList();
      ddListDay.ID = prefix + "D" + colProp.Name;
      if (prefix == "dd") {
        ddListDay.Items.Add("");
      }
      for (int i = 1; i <= 31; i++) {
        ddListDay.Items.Add(i.ToString());
      }
      if (prefix == "dd") ddListDay.DataBinding += this.BindDataCalendar;
      container.Controls.Add(ddListDay);
    }

    private void BindData(object sender, EventArgs e) {
      Label l = (Label)sender;
      GridViewRow container = (GridViewRow)l.NamingContainer;
      if (colProp.IsDatum) {
        if (((DataRowView)container.DataItem)[colProp.Name] != System.DBNull.Value) {
          DateTime dateTime = ((DateTime)((DataRowView)container.DataItem)[colProp.Name]);
          l.Text = dateTime.ToString("yyyy-MMM-dd");
        } else {
          l.Text = DateTime.Now.ToString("yyyy-MMM-dd");
        }
        return;
      } else if (colProp.IsCombo) {
        if (colProp.ComboObjectType == typeof(ElszamolasIdoszakok)) {
          DataTable table = _ElszamolasiIdoszakok(false);
          foreach (DataRow item in table.Rows) {
            if (((DataRowView)container.DataItem)[colProp.Name] != System.DBNull.Value && (int)((DataRowView)container.DataItem)[colProp.Name] == (int)item["id"]) {
              l.Text = AJEHelpers.CreateItemFromRow<ElszamolasIdoszakok>(item).ToString();
              return;
            }
          }
        }
      }
      l.Text = ((DataRowView)container.DataItem)[colProp.Name].ToString();
    }

    private void BindDataText(object sender, EventArgs e) {
      TextBox l = (TextBox)sender;
      GridViewRow container = (GridViewRow)l.NamingContainer;
      l.Text = ((DataRowView)container.DataItem)[colProp.Name].ToString();
    }

    private void BindDataCalendar(object sender, EventArgs e) {
      DropDownList l = (DropDownList)sender;
      GridViewRow container = (GridViewRow)l.NamingContainer;
      DateTime dateTime = ((DateTime)((DataRowView)container.DataItem)[colProp.Name]);
      if (l.ID.StartsWith("ddY")) {
        l.SelectedItem.Text = dateTime.Year.ToString();
      }
      if (l.ID.StartsWith("ddM")) {
        l.SelectedItem.Text = dateTime.Month.ToString();
      }
      if (l.ID.StartsWith("ddD")) {
        l.SelectedItem.Text = dateTime.Day.ToString();
      }
    }
    private void BindElszamolasiIdoszak(object sender, EventArgs e) {
      DropDownList l = (DropDownList)sender;
      GridViewRow container = (GridViewRow)l.NamingContainer;
      if (container.DataItem != null) {
        int id = (int)((DataRowView)container.DataItem)[colProp.Name];
        var table = _ElszamolasiIdoszakok();
        foreach (DataRow item in table.Rows) {
          var el = AJEHelpers.CreateItemFromRow<ElszamolasIdoszakok>(item);
          if (el.Id == id) {
            l.SelectedItem.Text = el.ToString();
            l.SelectedItem.Value = id.ToString();
            return;
          }
        }
        l.SelectedItem.Text = id.ToString();
      }
    }


    private void BindDataTextCombo(object sender, EventArgs e) {
      DropDownList l = (DropDownList)sender;
      GridViewRow container = (GridViewRow)l.NamingContainer;
      l.SelectedItem.Text = ((DataRowView)container.DataItem)[colProp.Name].ToString();
    }

    private void CmbBox_SelectedIndexChanged(object sender, EventArgs e) {
      try {
        DropDownList l = (DropDownList)sender;
        GridViewRow container = (GridViewRow)l.NamingContainer;
        TextBox txt = container.FindControl("txt" + colProp.UpdateOtherField) as TextBox;
        if (txt != null) {
          DataRow[] row = colProp.UpdateOtherFieldDT.Select("Tipus = '" + l.Text + "'");
          txt.Text = row[0].ItemArray[0].ToString();
        }
      }catch(Exception ex) {
        Trace.WriteLine(ex.Message);
      }
    }

    private void CmbBox_SelectedIndexChangedIn(object sender, EventArgs e) {
      try {
        DropDownList l = (DropDownList)sender;
        GridViewRow container = (GridViewRow)l.NamingContainer;
        TextBox txt = container.FindControl("in" + colProp.UpdateOtherField) as TextBox;
        if (txt != null) {
          DataRow[] row = colProp.UpdateOtherFieldDT.Select("Tipus = '" + l.Text + "'");
          txt.Text = row[0].ItemArray[0].ToString();
        }
      } catch (Exception ex) {
        Trace.WriteLine(ex.Message);
      }
    }

  }


}