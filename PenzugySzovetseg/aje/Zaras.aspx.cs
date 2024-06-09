using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PenzugySzovetseg.aje {
  public partial class Zaras : BaseGridPage {

    protected override string _GetTablaNev() {
      return "ElszamolasIdoszakok";
    }

    protected override string _GetDataKey() {
      return "Id";
    }

    protected override void _AddButtonTemplatek() {
      // Create the TemplateField 

      TemplateField buttonok = new TemplateField();
      buttonok.ItemTemplate = new TemplateGeneratorButton(ListItemType.Item);
      ((TemplateGeneratorButton)buttonok.ItemTemplate).AddAtirButton = false;
      ((TemplateGeneratorButton)buttonok.ItemTemplate).AddDeleteButton = false;
      ((TemplateGeneratorButton)buttonok.ItemTemplate).AddZarasButton = true;

      buttonok.EditItemTemplate = new TemplateGeneratorButton(ListItemType.EditItem);
      buttonok.FooterTemplate = new TemplateGeneratorButton(ListItemType.Footer);
      gridView.Columns.Add(buttonok);
    }

    protected override void _RowCommand(GridViewCommandEventArgs e) {
      if (e.CommandName.Equals("Zaras")) {
        var rowIndex = ((GridViewRow)(((Button)e.CommandSource).NamingContainer)).RowIndex;
        string id = gridView.DataKeys[rowIndex ].Values[_GetDataKey()].ToString();
        var s = "update " + _GetTablaNev() + " set Zarolt=1 where " + _GetDataKey() + "='" + id + "'";
                string errorMsg;
                int result = sqlLiteAccess.ExecuteQuery(s, out errorMsg);

        if (result == 1) {
          lblmsg.BackColor = Color.Blue;
          lblmsg.ForeColor = Color.White;
          lblmsg.Text = id + ":  Sikeres zárás!";
        } else {
          lblmsg.BackColor = Color.Red;
          lblmsg.ForeColor = Color.White;
          lblmsg.Text = id + ": A zárás sikertelen....." + s + $" Hiba: {errorMsg}";
        }
        gridView.EditIndex = -1;
        _LoadStores(_GetTablaNev());
      } else {
        base._RowCommand(e);
      }
    }

    protected override List<ColumnProperty> _GetColumnNames() {
      List<ColumnProperty> list = new List<ColumnProperty>();

      var colId = new ColumnProperty("Id");
      colId.Width = 0;
      colId.IncludeInUpdate = false;
      list.Add(colId);

      var colLezarva = new ColumnProperty("ElszamoltIdoszak");
      colLezarva.Width = 0;
      colLezarva.IncludeInUpdate = false;
      list.Add(colLezarva);

      var colEv = new ColumnProperty("Ev");
      colEv.IsDecimal = true;
      colEv.Width = 100;
      list.Add(colEv);

      var colNegyedev = new ColumnProperty("Negyedev");
      colNegyedev.IsDecimal = true;
      colNegyedev.Width = 100;
      list.Add(colNegyedev);

      return list;
    }

    protected override void gridView_RowDataBound(object sender, GridViewRowEventArgs e) {
      if (e.Row.RowType == DataControlRowType.DataRow) {
        //string lezarva = ;
        _CheckZaras(e, _GetDataKey());
        base.gridView_RowDataBound(sender, e);
      }
    }

    protected override List<string> _GetFilters() {
      return null;
    }

    protected override string _GetOrderByField() {
      return null;
    }

    protected override bool _GetAddRowCount() {
      return false;
    }
  }
}