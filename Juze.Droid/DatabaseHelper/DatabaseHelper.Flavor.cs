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
    /// Names of elements in the flavors SQLite table.
    /// </summary>
    public const string FLAVOR_TABLE_NAME = "FLAVOR";
    public const string FLAVOR_ID_COL = "ID";
    public const string FLAVOR_NAME_COL = "NAME";
    public const string FLAVOR_PG_COL = "PG";
    public const string FLAVOR_RECPER_COL = "REC_PER";

    /// <summary>
    /// Create the flavors table.
    /// </summary>
    /// <param name="db">The database the table should be added to.</param>
    public void CreateFlavorTable(SQLiteDatabase db) {
      string cmd = $@"CREATE TABLE {FLAVOR_TABLE_NAME} ({FLAVOR_ID_COL} INTEGER PRIMARY KEY AUTOINCREMENT, {FLAVOR_NAME_COL} TEXT, " +
        $"{FLAVOR_PG_COL} INTEGER, {FLAVOR_RECPER_COL} REAL)";
      db.ExecSQL(cmd);
    }

    /// <summary>
    /// Remove the flavor table from the database.
    /// </summary>
    /// <param name="db">The database to remove the table from.</param>
    public void RemoveFlavorTable(SQLiteDatabase db) {
      try {
        db.ExecSQL($"DROP TABLE {FLAVOR_TABLE_NAME};");
      } catch (SQLiteAbortException ex) {
        Console.WriteLine(ex.Message);
      }
    }

    /// <summary>
    /// Add a new flavor to the flavor table.
    /// </summary>
    /// <param name="flavor">The flavor being added.</param>
    public void PutFlavor(Flavor flavor) {
      int pg = flavor.PG ? 1 : 0;
      string cmd = $"INSERT INTO {FLAVOR_TABLE_NAME} ({FLAVOR_NAME_COL}, {FLAVOR_PG_COL}, {FLAVOR_RECPER_COL}) " +
        $"VALUES (\"{flavor.Name}\", {pg}, {flavor.RecommendedPercentage});";
      try {
        WritableDatabase.ExecSQL(cmd);
        flavor.ID = GetLastInsertedID();
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Update an existing flavor in the database.
    /// </summary>
    /// <param name="flavor">The flavor to be updated.</param>
    public void UpdateFlavor(Flavor flavor) {
      int pg = flavor.PG ? 1 : 0;
      string cmd =  $"UPDATE {FLAVOR_TABLE_NAME} SET {FLAVOR_NAME_COL}=\"{flavor.Name}\", {FLAVOR_PG_COL}={pg}, " +
        $"{FLAVOR_RECPER_COL}={flavor.RecommendedPercentage} WHERE {FLAVOR_ID_COL} = {flavor.ID};";
      try {
        WritableDatabase.ExecSQL(cmd);
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Remove the flavor from the database.
    /// </summary>
    /// <param name="flavor"></param>
    public void RemoveFlavor(Flavor flavor) {
      string cmd = $"DELETE FROM {FLAVOR_TABLE_NAME} WHERE {FLAVOR_ID_COL}={flavor.ID};";
      try {
        WritableDatabase.ExecSQL(cmd);
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Retrieve all flavors from the database.
    /// </summary>
    /// <returns>The collection of all flavors currently in the database.</returns>
    public IList<Flavor> GetFlavors() {
      List<Flavor> res = new List<Flavor>();
      ICursor iter = ReadableDatabase.RawQuery($"SELECT * FROM {FLAVOR_TABLE_NAME};", null);

      while (iter.MoveToNext()) {
        Flavor newFlavor = ParseFlavor(iter);
        if (newFlavor != null)
          res.Add(newFlavor);
      }
      iter.Close();
      return res;
    }

    /// <summary>
    /// Parse a flavor from the current row.
    /// </summary>
    /// <param name="cursor">The database cursor, pointing to a valid query result row.</param>
    /// <returns>The parsed flavor.</returns>
    private Flavor ParseFlavor(ICursor cursor) {
      Flavor res = new Flavor();

      res.ID = cursor.GetInt(0);
      res.Name = cursor.GetString(1);
      res.PG = (cursor.GetInt(2) > 0);
      res.RecommendedPercentage = cursor.GetFloat(3);

      return res;
    }

  }
}