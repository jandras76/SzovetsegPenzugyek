using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PenzugySzovetseg.aje {
  public static class AJEHelpers {
    public static List<string> UserFilter(string curuser, bool lelkeszuser = false) {
      IList<string> simpleUsers = null;
      if (!lelkeszuser) {
        simpleUsers = new List<string> { "baden", "basel", "bern", "luzern", "stgallen", "zurich" };
      } else {
        simpleUsers = new List<string> { "dnelli", "herika", "mkrisztina", "sztibor", "vnagnes" };
      }
      List<string> userFilter = new List<string>();
      if (simpleUsers.Contains(curuser)) {
        userFilter.Add(string.Format("[User]='{0}'", curuser));
      }
      return userFilter;
    }

    //public static List<AJEYear> Evek {
    //  get {
    //    return new List<AJEYear>() { 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024 };
    //  }
    //}

    // function that creates an object from the given data row
    public static T CreateItemFromRow<T>(this DataRow row) where T : new() {
      // create a new object
      T item = new T();

      // set the item
      SetItemFromRow(item, row);

      // return 
      return item;
    }

    public static void SetItemFromRow<T>(T item, DataRow row) where T : new() {
      // go through each column
      foreach (DataColumn c in row.Table.Columns) {
        // find the property for the column
        PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

        // if exists, set the value
        if (p != null && row[c] != DBNull.Value) {
          p.SetValue(item, row[c], null);
        }
      }
    }
  }
}