using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PenzugySzovetseg.aje {
  public partial class Varosok : BaseGridPage {

    protected override string _GetTablaNev() {
      return "Varosok";
    }

    protected override string _GetDataKey() {
      return "Nev";
    }

    protected override List<ColumnProperty> _GetColumnNames() {
      return new List<ColumnProperty>() { new ColumnProperty(_GetDataKey()), new ColumnProperty("TagokSzama") };
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