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
using Android.Database;
using Android.Database.Sqlite;

namespace paujo.juze.android {
  public partial class DatabaseHelper : SQLiteOpenHelper {

    public DatabaseHelper(Context context) : base(context, "juze.db", null, paujo.juze.Constants.SCHEMA_VERSION) {
    }

    /// <summary>
    /// Called on creation of the database.
    /// </summary>
    /// <param name="db">The database being created.</param>
    public override void OnCreate(SQLiteDatabase db) {
      CreateFlavorTable(db);
      CreateNicotineTable(db);
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

    /// <summary>
    /// Returns the ID of the last inserted element.
    /// </summary>
    /// <returns>On success: the last inserted ID. On failure: -1</returns>
    public int GetLastInsertedID() {
      int res = -1;
      try {
        ICursor cur = ReadableDatabase.RawQuery("SELECT last_insert_rowid();", new string[] { });
        cur.MoveToFirst();
        res = cur.GetInt(0);
        cur.Close();
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }

      return res;
    }
  }
}