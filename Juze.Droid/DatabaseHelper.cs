﻿using System;
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
  public class DatabaseHelper : SQLiteOpenHelper {

    public DatabaseHelper(Context context) : base(context, "juze.db", null, paujo.juze.Constants.SCHEMA_VERSION) {
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
      db.ExecSQL(Flavor.CreateTableCommand);
    }

    /// <summary>
    /// Add a new flavor to the flavor table.
    /// </summary>
    /// <param name="flavor">The flavor being added.</param>
    public void PutFlavor(Flavor flavor) {
      try {
        WritableDatabase.ExecSQL(flavor.InsertCommand);
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Update an existing flavor in the database.
    /// </summary>
    /// <param name="flavor">The flavor to be updated.</param>
    public void UpdateFlavor(Flavor flavor) {
      try {
        WritableDatabase.ExecSQL(flavor.UpdateCommand);
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Remove the flavor from the database.
    /// </summary>
    /// <param name="flavor"></param>
    public void RemoveFlavor(Flavor flavor) {
      try {
        WritableDatabase.ExecSQL(flavor.DeleteCommand);
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

      List<Flavor> res = new List<Flavor>();
      ICursor iter = ReadableDatabase.RawQuery(Flavor.ListCommand, null);
      int colCount = iter.ColumnCount;

      while (iter.MoveToNext()) {
        string[] columns = new string[colCount];
        for (int i = 0; i < colCount; i++)
          columns[i] = iter.GetString(i);
        Flavor newFlavor = Flavor.ParseFromQueryResult(columns);
        if (newFlavor != null)
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