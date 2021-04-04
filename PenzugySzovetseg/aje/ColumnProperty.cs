using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PenzugySzovetseg.aje {
  public class ColumnProperty {

    private List<string> m_filters;

    public ColumnProperty(string name) {
      Name = name;
    }

    public List<string> Filters { get; set; }

    private bool includeInUpdate = true;

    public bool IncludeInUpdate
    {
      get { return includeInUpdate; }
      set { includeInUpdate = value; }
    }

    public bool ReadOnly { get; set; } = false;

    public bool DenyEmpty { get; set; }


    public bool IsDatum { get; set; }

    public bool IsCombo { get; set; }

    public bool IsDecimal { get; set; }

    public string TablaKulsoKey { get; set; }

    public string Name { get; set; }
    private int mWidth = 200;

    public int Width
    {
      get { return mWidth; }
      set { mWidth = value; }
    }

    private bool isVisible = true;

    public bool VIsible
    {
      get { return isVisible; }
      set { isVisible = value; }
    }

    public string UpdateOtherField { get; set; }

    public DataTable UpdateOtherFieldDT { get; set; }

    public Type ComboObjectType { get; set; } = null;

  }
  
}