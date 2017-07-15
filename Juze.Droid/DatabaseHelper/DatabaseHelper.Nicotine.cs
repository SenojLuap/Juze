using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Database.Sqlite;
using Android.Database;

namespace paujo.juze.android {
  public partial class DatabaseHelper {

    /// <summary>
    /// Names of elements in the nicotine SQLite table.
    /// </summary>
    public static string NIC_TABLE_NAME = "NICOTINE";
    public static string NIC_ID_COL = "ID";
    public static string NIC_NAME_COL = "NAME";
    public static string NIC_VG_COL = "VG";
    public static string NIC_CONC_COL = "CONCENTRATION";

    /// <summary>
    /// Create the nicotine table.
    /// </summary>
    /// <param name="db">The database the table should be added to.</param>
    public void CreateNicotineTable(SQLiteDatabase db) {
      string cmd = $@"CREATE TABLE {NIC_TABLE_NAME} ({NIC_ID_COL} INTEGER PRIMARY KEY AUTOINCREMENT, " +
        $"{NIC_NAME_COL} TEXT, {NIC_VG_COL} REAL, {NIC_CONC_COL} INTEGER);";
      db.ExecSQL(cmd);
    }

    /// <summary>
    /// Add a new nicotine to the nicotine table.
    /// </summary>
    /// <param name="nicotine">The nicotine to add.</param>
    public void PutNicotine(Nicotine nicotine) {
      string cmd = $"INSERT INTO {NIC_TABLE_NAME} ({NIC_NAME_COL}, {NIC_VG_COL}, {NIC_CONC_COL}) " +
        $"VALUES (\"{nicotine.Name}\", {nicotine.VG}, {nicotine.Concentration});";
      try {
        WritableDatabase.ExecSQL(cmd);
        nicotine.ID = GetLastInsertedID();
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Update a nicotine in the database.
    /// </summary>
    /// <param name="nicotine">The nicotine to update.</param>
    public void UpdateNicotine(Nicotine nicotine) {
      string cmd = $"UPDATE {NIC_TABLE_NAME} SET {NIC_NAME_COL}=\"{nicotine.Name}\", {NIC_VG_COL}={nicotine.VG}, " +
        $" {NIC_CONC_COL}={nicotine.Concentration} WHERE {NIC_ID_COL}={nicotine.ID};";
      try {
        WritableDatabase.ExecSQL(cmd);
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Remove a nicotine from the database.
    /// </summary>
    /// <param name="nicotine">The nicotine to remove.</param>
    public void RemoveNicotine(Nicotine nicotine) {
      string cmd = $"DELETE FROM {NIC_TABLE_NAME} WHERE {NIC_ID_COL}={nicotine.ID};";
      try {
        WritableDatabase.ExecSQL(cmd);
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Retrieve all nicotines from the database.
    /// </summary>
    /// <returns>The collection of all nicotines currently in the database.</returns>
    public IList<Nicotine> GetNicotines() {
      IList<Nicotine> res = new List<Nicotine>();
      string cmd  = $"SELECT * FROM {NIC_TABLE_NAME};";

      ICursor iter = ReadableDatabase.RawQuery(cmd, null);
      while (iter.MoveToNext()) {
        Nicotine newNic = ParseNicotine(iter);
        if (newNic != null)
          res.Add(newNic);
      }
      iter.Close();
      return res;
    }

    /// <summary>
    /// Parse a nicotine from the current row.
    /// </summary>
    /// <param name="iter">The database cursor pointing to a valid query result.</param>
    /// <returns>The parsed nicotine.</returns>
    private Nicotine ParseNicotine(ICursor iter) {
      Nicotine res = new Nicotine();

      res.ID = iter.GetInt(0);
      res.Name = iter.GetString(1);
      res.VG = iter.GetFloat(2);
      res.Concentration = iter.GetInt(3);

      return res;
    }

  }
}