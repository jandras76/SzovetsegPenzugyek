using PenzugySzovetseg.aje;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PenzugySzovetseg {
  public partial class Lelkeszek : BaseGridPage {



    protected override string _GetTablaNev() {
      return "Lelkeszek";
    }

    protected override string _GetDataKey() {
      return "Kod";
    }

    protected override List<ColumnProperty> _GetColumnNames() {
      return new List<ColumnProperty>() { new ColumnProperty("Kod"), new ColumnProperty("Nev") };
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