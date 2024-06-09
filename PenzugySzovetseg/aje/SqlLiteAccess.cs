using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace PenzugySzovetseg {
  public class SqlLiteAccess {

    private SqlConnection sql_con;
    private SqlDataAdapter DB;
    private DataSet DS = new DataSet();
    private DataTable DT = new DataTable();

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private Container components = null;

    //public static void CreateDb()
    //{
    //    var dbPath = "szovetseg.sqlite";
    //    if (!File.Exists(dbPath))
    //    {
    //        SqlConnection.CreateFile(dbPath);
    //    }
    //}

    public DataTable LoadData(string commandText, List<string> list = null, string fieldOrderBy = null, string groupBy = null) {
      try {
        ResetSetConnection();
        SqlCommand cmd = sql_con.CreateCommand();
        cmd.CommandText = commandText;
        _SetFilters(cmd, list, fieldOrderBy, groupBy);
        Trace.WriteLine("LoadData sql: " + cmd.CommandText);
        DB = new SqlDataAdapter(cmd);
        DS.Reset();
        DB.Fill(DS);
        return DS.Tables[0];
      } finally {
        sql_con.Close();
      }

    }

    private void ResetSetConnection() {
      try {
        if (sql_con == null) {
          //string connectionString = @"Data Source=USER-PC\SQLEXPRESS;Initial Catalog=szovetseg;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
          //string connectionString = "Data Source=localhost;Initial Catalog=szovetseg;Integrated Security=True";
          //string connectionString = "Data Source = szovetsegdb.database.windows.net; Initial Catalog=szovetseg;Integrated Security = False; User ID = ajenei; Password = PLOkij+$4231; Connect Timeout = 15; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
          
          string connectionString = "Data Source=SQL5023.myWindowsHosting.com;Initial Catalog=DB_9F4990_szovdb;User Id=DB_9F4990_szovdb_admin;Password=PLOkij123;";
          sql_con = new SqlConnection(connectionString);
        } else {
          sql_con.Close();
        }
        sql_con.Open();
      } catch (Exception ex) {
        Trace.WriteLine(ex.Message);
      }

    }

    public int ExecuteQuery(string txtQuery, out string errorMsg) {
      try {
        errorMsg = string.Empty;
        ResetSetConnection();

        SqlCommand cmd = sql_con.CreateCommand();
        cmd.CommandText = txtQuery;

        return cmd.ExecuteNonQuery();
      } catch (Exception ex) {
        errorMsg = ex.Message;
        return -1;
      } finally {
        sql_con.Close();
      }
    }

    public DataTable GetDataTable(string tablename, List<string> filters = null, string fieldOrderBy = null, bool addRowCount = false) {
      try {
        DataTable DT = new DataTable();
        ResetSetConnection();
        SqlCommand cmd = sql_con.CreateCommand();

        if (addRowCount && !string.IsNullOrEmpty(fieldOrderBy)) {
          cmd.CommandText = string.Format("SELECT ROW_NUMBER() OVER(ORDER BY {0} ASC) AS Sor,* FROM {1}", fieldOrderBy, tablename);
        } else {
          cmd.CommandText = string.Format("SELECT * FROM {0}", tablename);
        }

        _SetFilters(cmd, filters, fieldOrderBy, addRowCount: addRowCount);

        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        adapter.AcceptChangesDuringFill = false;
        adapter.Fill(DT);
        DT.TableName = tablename;
        return DT;
      } catch (Exception Ex) {
        //System.Windows.MessageBox.Show(Ex.Message);
        Trace.WriteLine(Ex);
        return null;
      } finally {
        sql_con.Close();
      }
    }

    private static void _SetFilters(SqlCommand cmd, List<string> filters, string fieldOrderBy = null, string groupBy = null, bool addRowCount = false) {
      if (filters != null && filters.Any()) {
        cmd.CommandText += " WHERE ";
        for (int i = 0; i < filters.Count(); i++) {
          cmd.CommandText += filters[i];
          if (i < filters.Count() - 1) {
            cmd.CommandText += " and ";
          }
        }
      }

      if (!string.IsNullOrEmpty(groupBy)) {
        cmd.CommandText += " GROUP BY " + groupBy;
      }

      if (!addRowCount && !string.IsNullOrEmpty(fieldOrderBy)) {
        cmd.CommandText += " ORDER BY " + fieldOrderBy;
      }
    }

    public void SaveDataTable(DataTable DT) {
      try {
                string errorMsg;
                ExecuteQuery(string.Format("DELETE FROM {0}", DT.TableName), out errorMsg);
        ResetSetConnection();
        SqlCommand cmd = sql_con.CreateCommand();
        cmd.CommandText = string.Format("SELECT * FROM {0}", DT.TableName);
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
        adapter.Update(DT);
      } catch (Exception Ex) {
        //System.Windows.MessageBox.Show(Ex.Message);
      } finally {
        sql_con.Close();
      }
    }
  }
}
