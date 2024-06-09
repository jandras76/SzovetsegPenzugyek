using PenzugySzovetseg.aje;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PenzugySzovetseg.Models;

namespace PenzugySzovetseg {
  public abstract class BaseGridPage : Page {

    protected SqlLiteAccess sqlLiteAccess = new SqlLiteAccess();
    protected string _SpecialAddNewColumnName = "";
    protected void _LoadStores(string tablaNev) {
      DataTable table = sqlLiteAccess.GetDataTable(tablaNev, _GetFilters(), _GetOrderByField(), _GetAddRowCount());
      if (table != null && table.Rows.Count > 0) {
        gridView.DataSource = table;
        gridView.DataBind();
      } else {
        if (table != null) {
          table.Rows.Add(table.NewRow());
          gridView.DataSource = table;
        }
        gridView.DataBind();
        lblmsg.Text = " Nincs adat!";
      }
    }

    protected virtual void _AddButtonTemplatek() {
      // Create the TemplateField 

      TemplateField buttonok = new TemplateField();
      buttonok.ItemTemplate = new TemplateGeneratorButton(ListItemType.Item);
      buttonok.EditItemTemplate = new TemplateGeneratorButton(ListItemType.EditItem);
      buttonok.FooterTemplate = new TemplateGeneratorButton(ListItemType.Footer);
      gridView.Columns.Add(buttonok);
    }

    protected void _CreateTemplates() {

      _AddButtonTemplatek();
      foreach (ColumnProperty colProp in _GetColumnNames()) {
        if (colProp.IncludeInUpdate) {
          string field = colProp.Name;
          TemplateField kodTemplateField = new TemplateField();
          kodTemplateField.HeaderText = field;
          kodTemplateField.ItemTemplate = new TemplateGenerator(ListItemType.Item, colProp);
          kodTemplateField.EditItemTemplate = new TemplateGenerator(ListItemType.EditItem, colProp);
          kodTemplateField.FooterTemplate = new TemplateGenerator(ListItemType.Footer, colProp);
          gridView.Columns.Add(kodTemplateField);
        } else {
          if (colProp.ReadOnly) {
            string field = colProp.Name;
            TemplateField kodTemplateField = new TemplateField();
            kodTemplateField.HeaderText = field;
            kodTemplateField.ItemTemplate = new TemplateGenerator(ListItemType.Item, colProp);
            kodTemplateField.EditItemTemplate = new TemplateGenerator(ListItemType.Item, colProp);
            //kodTemplateField.FooterTemplate = new TemplateGenerator(ListItemType.Footer, colProp);
            gridView.Columns.Add(kodTemplateField);
          }
        }

      }

    }

    protected virtual void Page_Load(object sender, EventArgs e) {
      //if (!IsPostBack) {
      if (gridView != null) {
        gridView.Columns.Clear();
      }
      _CreateTemplates();
      _LoadStores(_GetTablaNev());
      //}
    }

    /// <summary>
    /// gridView control.
    /// </summary>
    /// <remarks>
    /// Auto-generated field.
    /// To modify move field declaration from designer file to code-behind file.
    /// </remarks>
    protected global::System.Web.UI.WebControls.GridView gridView;

    /// <summary>
    /// lblmsg control.
    /// </summary>
    /// <remarks>
    /// Auto-generated field.
    /// To modify move field declaration from designer file to code-behind file.
    /// </remarks>
    protected global::System.Web.UI.WebControls.Label lblmsg;


    protected abstract string _GetTablaNev();

    protected abstract string _GetDataKey();

    protected abstract List<ColumnProperty> _GetColumnNames();

    protected abstract List<string> _GetFilters();

    protected abstract string _GetOrderByField();

    protected abstract bool _GetAddRowCount();

    private DataTable m_ElszamolasiIdoszakok = null;

    protected void gridView_RowEditing(object sender, GridViewEditEventArgs e) {
      gridView.EditIndex = e.NewEditIndex;

      gridView.Columns.Clear();
      _CreateTemplates();
      _LoadStores(_GetTablaNev());
    }

    private DataTable _ElszamolasiIdoszakok() {
      if (m_ElszamolasiIdoszakok == null) {
        m_ElszamolasiIdoszakok = sqlLiteAccess.GetDataTable("ElszamolasIdoszakok");
      }
      return m_ElszamolasiIdoszakok;
    }

    private bool _IdoszakLezarva(string idStr) {
      try {
        int id = Convert.ToInt32(idStr);
        bool lezarva = false;
        var table = _ElszamolasiIdoszakok();
        foreach (DataRow item in table.Rows) {
          var el = AJEHelpers.CreateItemFromRow<ElszamolasIdoszakok>(item);
          if (el.Id == id) {
            lezarva = el.Zarolt;
          }
        }
        return lezarva;
      } catch {
        return false;
      }
    }

    protected void _CheckZaras(GridViewRowEventArgs e, string zarasId = "ElszamoltIdoszak") {
      bool bLezarva = _IdoszakLezarva(DataBinder.Eval(e.Row.DataItem, zarasId).ToString());

      if (bLezarva) {
        var lblLezarva = (Label)e.Row.FindControl("LabelLezarva");
        if (lblLezarva != null) {
          lblLezarva.Visible = true;
          var btnEdit = _GetEditButton(e);
          if (btnEdit != null) {
            btnEdit.Visible = false;
          }
          var btnDelete = _GetDeleteButton(e);
          if (btnDelete != null) {
            btnDelete.Visible = false;
          }
          var btnLezar = _GetLezarButton(e);
          if (btnLezar != null) {
            btnLezar.Visible = false;
          }
          return;
        }
      }
    }

    protected void gridView_RowUpdating(object sender, GridViewUpdateEventArgs e) {
      string id = gridView.DataKeys[e.RowIndex].Values[_GetDataKey()].ToString();

      string colNameList = "";
      for (int i = 0; i < _GetColumnNames().Count; i++) {
        if (_GetColumnNames()[i].IncludeInUpdate) {
          string colValue;
          string columnName = _GetColumnNames()[i].Name;
          if (_GetColumnNames()[i].IsDatum) {
            DropDownList ddY = (DropDownList)gridView.Rows[e.RowIndex].FindControl("ddY" + columnName);
            DropDownList ddM = (DropDownList)gridView.Rows[e.RowIndex].FindControl("ddM" + columnName);
            DropDownList ddD = (DropDownList)gridView.Rows[e.RowIndex].FindControl("ddD" + columnName);
            colValue = (new DateTime(Convert.ToInt32(ddY.Text), Convert.ToInt32(ddM.Text), Convert.ToInt32(ddD.Text))).ToString("yyyyMMdd");
          } else {
            var c = "txt";
            if (_GetColumnNames()[i].IsCombo) {
              c = "cmb";
            }
            ITextControl txt = (ITextControl)gridView.Rows[e.RowIndex].FindControl(c + columnName);
            colValue = txt.Text;
          }
          colNameList += columnName + "='" + colValue + "'";


          if (i < _GetColumnNames().Count - 1) {
            colNameList += ",";
          }
        }
      }

      //colNameList.EndsWith()

      var s = "update " + _GetTablaNev() + " set " + colNameList + " where " + _GetDataKey() + "='" + id + "'";
            string errorMsg;
            int result = sqlLiteAccess.ExecuteQuery(s, out errorMsg);

      if (result == 1) {
        lblmsg.BackColor = Color.Blue;
        lblmsg.ForeColor = Color.White;
        lblmsg.Text = id + ":  Sikeres módositás........ ";
      } else {
        lblmsg.BackColor = Color.Red;
        lblmsg.ForeColor = Color.White;
        lblmsg.Text = id + ": A módositás nem sikerült....." + s + $" Hiba: {errorMsg}";
      }
      gridView.EditIndex = -1;
      _LoadStores(_GetTablaNev());
    }

    protected void gridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) {
      gridView.EditIndex = -1;
      _LoadStores(_GetTablaNev());
    }

    protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e) {
      string id = gridView.DataKeys[e.RowIndex].Values[_GetDataKey()].ToString();
        string errorMsg;
      int result = sqlLiteAccess.ExecuteQuery("delete from " + _GetTablaNev() + " where " + _GetDataKey() + "='" + id + "'", out errorMsg);

      _LoadStores(_GetTablaNev());

      if (result == 1) {
        lblmsg.BackColor = Color.Green;
        lblmsg.ForeColor = Color.White;
        lblmsg.Text = id + "      sikeresen törölve.......    ";
      } else {
        lblmsg.BackColor = Color.Red;
        lblmsg.ForeColor = Color.White;
        lblmsg.Text = id + "      törölése sikertelen.......    " + $" Hiba: {errorMsg}";
      }

    }

    protected Button _GetDeleteButton(GridViewRowEventArgs e) {
      return (Button)e.Row.FindControl("ButtonDelete");
    }

    protected Button _GetLezarButton(GridViewRowEventArgs e) {
      return (Button)e.Row.FindControl("ButtonZaras");
    }

    protected Button _GetEditButton(GridViewRowEventArgs e) {
      return (Button)e.Row.FindControl("ButtonEdit");
    }


    protected virtual void gridView_RowDataBound(object sender, GridViewRowEventArgs e) {
      if (e.Row.RowType == DataControlRowType.DataRow) {
        string id = Convert.ToString(DataBinder.Eval(e.Row.DataItem, _GetDataKey()));

        Button btnDelete = _GetDeleteButton(e);
        if (btnDelete != null) {
          if (string.IsNullOrEmpty(id)) {
            btnDelete.Visible = false;
          } else {
            btnDelete.Attributes.Add("onclick", "javascript:return confirm('Biztos törlöd ezt a sort ahol a " + _GetDataKey() + "=" + id + " ?')");
          }
        }

        //ButtonEdit
        Button btnEdit = _GetEditButton(e);
        if (btnEdit != null) {
          if (string.IsNullOrEmpty(id)) {
            btnEdit.Visible = false;
          }
        }
      }
    }

    protected virtual string _SpecialAddNewColumn(DateTime datum) {
      //ITextControl txt = (ITextControl)gridView.FooterRow.FindControl("in" + columnName);
      return "";
    }


    protected virtual void _RowCommand(GridViewCommandEventArgs e) {
      if (e.CommandName.Equals("AddNew")) {
        string colNameList = "";
        string colValueList = "";
        for (int i = 0; i < _GetColumnNames().Count; i++) {
          if (_GetColumnNames()[i].IncludeInUpdate) {
            string columnName = _GetColumnNames()[i].Name;
            colNameList += columnName;

            if (_GetColumnNames()[i].IsDatum) {
              DropDownList ddY = (DropDownList)gridView.FooterRow.FindControl("inY" + columnName);
              DropDownList ddM = (DropDownList)gridView.FooterRow.FindControl("inM" + columnName);
              DropDownList ddD = (DropDownList)gridView.FooterRow.FindControl("inD" + columnName);
              var datum = new DateTime(Convert.ToInt32(ddY.Text), Convert.ToInt32(ddM.Text), Convert.ToInt32(ddD.Text));
              colValueList += "'" + datum.ToString("yyyyMMdd") + "'";
              if (_SpecialAddNewColumnName == "ElszamoltIdoszak") {
                var idoszakKereses = _SpecialAddNewColumn(datum);
                if (!string.IsNullOrEmpty(idoszakKereses)) {
                  lblmsg.BackColor = Color.Red;
                  lblmsg.ForeColor = Color.White;
                  lblmsg.Text = " Az új adat hozzáadása nem sikerült.....: " + idoszakKereses;
                  return;
                }
              }
            } 
            //else if (_SpecialAddNewColumnName == columnName) {
            //  string c = _SpecialAddNewColumn(columnName);
            //  colValueList += "'" + c + "'";
            //} 
            else {
              ITextControl txt = (ITextControl)gridView.FooterRow.FindControl("in" + columnName);
              var c = txt.Text;
              if (_GetColumnNames()[i].IsDecimal) {
                if (String.IsNullOrEmpty(c) || String.IsNullOrEmpty(c)) {
                  c = "0";
                }
              }

              if (_GetColumnNames()[i].DenyEmpty) {
                if (string.IsNullOrEmpty(c)) {
                  lblmsg.BackColor = Color.Red;
                  lblmsg.ForeColor = Color.White;
                  lblmsg.Text = string.Format("A {0} mező nem lehet üres! Köszönöm.", columnName);
                  return;
                }
              }

              colValueList += "'" + c + "'";
            }
            if (i < _GetColumnNames().Count - 1) {
              colNameList += ",";
              colValueList += ",";
            }
          }
        }

        var s = "insert into " + _GetTablaNev() + "(" + colNameList + ") values(" + colValueList + ")";
                string errorMsg;
                int result = sqlLiteAccess.ExecuteQuery(s, out errorMsg);
        if (result == 1) {
          _LoadStores(_GetTablaNev());
          lblmsg.BackColor = Color.Green;
          lblmsg.ForeColor = Color.White;
          lblmsg.Text = "      Sikeres hozzáadás!    ";
        } else {
          lblmsg.BackColor = Color.Red;
          lblmsg.ForeColor = Color.White;
          lblmsg.Text = " Az új adat hozzáadása nem sikerült.....: " + s + $" Hiba: {errorMsg}";
        }
      }
    }

    protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e) {
      _RowCommand(e);
    }

  }
}