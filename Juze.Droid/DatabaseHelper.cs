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

namespace paujo.juze.android {
  public class DatabaseHelper : SQLiteOpenHelper {

    /// <summary>
    /// The current version of the database.
    /// </summary>
    public const int JUZE_DB_VERSION = 1;

    /// <summary>
    /// The name of the table that contains flavors.
    /// </summary>
    public const string FLAVOR_TABLE_NAME = "FLAVOR";

    public DatabaseHelper(Context context) : base(context, "juze.db", null, JUZE_DB_VERSION) {
    }

    /// <summary>
    /// Called on creation of the database.
    /// </summary>
    /// <param name="db">The database being created.</param>
    public override void OnCreate(SQLiteDatabase db) {
      CreateFlavorTable(db);
    }

    /// <summary>
    /// Create the flavors table.
    /// </summary>
    /// <param name="db">The database the table should be added to.</param>
    public void CreateFlavorTable(SQLiteDatabase db) {
      string cmd = $@"CREATE TABLE {FLAVOR_TABLE_NAME} (ID INTEGER PRIMARY KEY, NAME TEXT, PG INTEGER, REC_PER REAL)";
      db.ExecSQL(cmd);
    }

    /// <summary>
    /// Add a new flavor to the flavor table.
    /// </summary>
    /// <param name="flavor">The flavor being added.</param>
    public void PutFlavor(Flavor flavor) {
      Console.WriteLine("Put Flavor");
      int pg = flavor.PG ? 1 : 0;
      try {
        string cmd = $"INSERT INTO {FLAVOR_TABLE_NAME} VALUES ({flavor.ID}, \"{flavor.Name}\", {pg}, {flavor.RecommendedPercentage});";
        Console.WriteLine(cmd);
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
      Console.WriteLine("Get flavors");
      var iter = ReadableDatabase.Query(FLAVOR_TABLE_NAME, new string[] { "ID", "NAME", "PG", "REC_PER" }, null, null, null, null, null);
      List<Flavor> res = new List<Flavor>();
      while (iter.MoveToNext()) {
        Flavor newFlavor = new Flavor();
        newFlavor.ID = iter.GetInt(0);
        newFlavor.Name = iter.GetString(1);
        newFlavor.PG = (iter.GetInt(2) > 0 ? true : false);
        newFlavor.RecommendedPercentage = iter.GetFloat(3);
        res.Add(newFlavor);
      }
      iter.Close();
      return res;
    }

    /// <summary>
    /// Called when attempting to promote an older version of a database to a newer one.
    /// </summary>
    /// <param name="db">The database being upgraded.</param>
    /// <param name="oldVersion">The version of the old database.</param>
    /// <param name="newVersion">The version of the new database.</param>
    public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
      throw new NotImplementedException();
    }
  }
}