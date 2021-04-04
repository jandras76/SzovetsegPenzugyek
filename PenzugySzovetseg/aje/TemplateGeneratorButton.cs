using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PenzugySzovetseg
{
  public class TemplateGeneratorButton : ITemplate // Class inheriting ITemplate
    {
    private ListItemType type;
    private string columnName;


    public TemplateGeneratorButton(ListItemType t) {
      type = t;
    }

    public bool AddAtirButton { get; set; } = true;
    public bool AddDeleteButton { get; set; } = true;
    public bool AddZarasButton { get; set; } = false;

    public List<Button> AdditionalListItemType = new List<Button>();

    // Override InstantiateIn() method
    void ITemplate.InstantiateIn(Control container) {
      switch (type) {
        case ListItemType.EditItem:
          Button buttonUpdate = new Button();
          buttonUpdate.ID = "ButtonUpdate";
          buttonUpdate.Text = "Save";
          buttonUpdate.CommandName = "Update";
          container.Controls.Add(buttonUpdate);

          Button btnCancel = new Button();
          btnCancel.ID = "ButtonCancel";
          btnCancel.Text = "Mégse";
          btnCancel.CommandName = "Cancel";
          container.Controls.Add(btnCancel);
          break;
        case ListItemType.Item:
          if (AddAtirButton) {
            Button buttonEdit = new Button();
            buttonEdit.ID = "ButtonEdit";
            buttonEdit.Text = "Átír";
            buttonEdit.CommandName = "Edit";
            container.Controls.Add(buttonEdit);
          }

          if (AddDeleteButton) {
            Button btnDElete = new Button();
            btnDElete.ID = "ButtonDelete";
            btnDElete.Text = "Töröl";
            btnDElete.CommandName = "Delete";
            container.Controls.Add(btnDElete);
          }

          if (AddZarasButton) {
            Button btnZaras = new Button();
            btnZaras.ID = "ButtonZaras";
            btnZaras.Text = "Negyedévet lezár";
            btnZaras.CommandName = "Zaras";
            container.Controls.Add(btnZaras);
          }

          Label lblLezarva = new Label();
          lblLezarva.ID = "LabelLezarva";
          lblLezarva.Text = "Lezárva";
          lblLezarva.ForeColor = Color.OrangeRed;
          lblLezarva.Visible = false;
          container.Controls.Add(lblLezarva);
          break;
        case ListItemType.Footer:
          Button buttonAdd = new Button();
          buttonAdd.ID = "ButtonAdd";
          buttonAdd.Text = "Sor hozzáadása -->";
          buttonAdd.CommandName = "AddNew";
          buttonAdd.ValidationGroup = "validaiton";
          container.Controls.Add(buttonAdd);

          break;
      }
    }


  }
}