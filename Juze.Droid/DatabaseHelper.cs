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
  public class DatabaseHelper : SQLiteOpenHelper {

    public DatabaseHelper(Context context) : base(context, "juze.db", null, paujo.juze.Constants.SCHEMA_VERSION) {
    }

    /// <summary>
    /// Called on creation of the database.
    /// </summary>
    /// <param name="db">The database being created.</param>
    public override void OnCreate(SQLiteDatabase db) {
      CreateFlavorTable(db);
      CreateNicotineTable(db);
      CreateRecipeTables(db);
    }

    /// <summary>
    /// Called when attempting to promote an older version of a database to a newer one.
    /// </summary>
    /// <param name="db">The database being upgraded.</param>
    /// <param name="oldVersion">The version of the old database.</param>
    /// <param name="newVersion">The version of the new database.</param>
    public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
    {
      throw new NotImplementedException();
    }

    #region Create Tables

    /// <summary>
    /// Create the nicotine table.
    /// </summary>
    /// <param name="db">The database the table should be added to.</param>
    public void CreateNicotineTable(SQLiteDatabase db) {
      db.ExecSQL(Nicotine.CreateTableCommand);
    }

    /// <summary>
    /// Create the flavors table.
    /// </summary>
    /// <param name="db">The database the table should be added to.</param>
    public void CreateFlavorTable(SQLiteDatabase db) {
      db.ExecSQL(Flavor.CreateTableCommand);
    }

    /// <summary>
    /// Create the recipe items table.
    /// </summary>
    /// <param name="db">The database to add the table to.</param>
    public void CreateRecipeTables(SQLiteDatabase db) {
      db.ExecSQL(Recipe.CreateTableCommand);
      db.ExecSQL(RecipeItem.CreateTableCommand);
    }

    #endregion

    #region Flavor methods

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

    #endregion

    #region Nicotine methods

    /// <summary>
    /// Add a new nicotine to the nicotine table.
    /// </summary>
    /// <param name="nicotine">The nicotine to add.</param>
    public void PutNicotine(Nicotine nicotine) {
      try {
        WritableDatabase.ExecSQL(nicotine.InsertCommand);
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Update a nicotine in the database.
    /// </summary>
    /// <param name="nicotine">The nicotine to update.</param>
    public void UpdateNicotine(Nicotine nicotine) {
      try {
        WritableDatabase.ExecSQL(nicotine.UpdateCommand);
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Remove a nicotine from the database.
    /// </summary>
    /// <param name="nicotine">The nicotine to remove.</param>
    public void RemoveNicotine(Nicotine nicotine) {
      try {
        WritableDatabase.ExecSQL(nicotine.DeleteCommand);
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

      ICursor iter = ReadableDatabase.RawQuery(Nicotine.ListCommand, null);
      int colCount = iter.ColumnCount;
      while (iter.MoveToNext()) {
        string[] columns = new string[colCount];
        for (int i = 0; i < colCount; i++)
          columns[i] = iter.GetString(i);
        Nicotine newNic = Nicotine.ParseFromQueryResult(columns);
        if (newNic != null)
          res.Add(newNic);
      }
      iter.Close();
      return res;
    }

    #endregion

    #region Recipe methods

    /// <summary>
    /// Add a new recipe to the recipe table.
    /// </summary>
    /// <param name="recipe">The recipe to be added.</param>
    public void PutRecipe(Recipe recipe) {
      var db = WritableDatabase;
      try {
        foreach (var stmt in recipe.InsertCommands)
          db.ExecSQL(stmt);
      } catch (SQLException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Update a recipe in the database.
    /// </summary>
    /// <param name="recipe">The recipe to be updated.</param>
    public void UpdateRecipe(Recipe recipe) {
      var db = WritableDatabase;
      try {
        foreach (var stmt in recipe.UpdateCommands)
          db.ExecSQL(stmt);
      } catch (SQLException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Remove a recipe from the database.
    /// </summary>
    /// <param name="recipe">The recipe to be removed.</param>
    public void RemoveRecipe(Recipe recipe) {
      var db = WritableDatabase;
      try {
        foreach (var stmt in recipe.DeleteCommands)
          db.ExecSQL(stmt);
      } catch (SQLException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Retrieve all recipes from the database.
    /// </summary>
    /// <returns>The collection of all recipes in the database.</returns>
    public IList<Recipe> GetRecipes() {
      List<Recipe> res = new List<Recipe>();

      var db = ReadableDatabase;

      try {
        ICursor iter = db.RawQuery(Recipe.ListCommand, null);
        int colCount = iter.ColumnCount;
        while (iter.MoveToNext()) {
          string[] columns = new string[colCount];
          for (int i = 0; i < colCount; i++)
            columns[i] = iter.GetString(i);
          Recipe newRecipe = Recipe.ParseFromQueryResult(columns);
          if (newRecipe != null)
            res.Add(newRecipe);
        }
      
        iter.Close();
      } catch (SQLException e) {
        Console.WriteLine("While parsing recipes: " + e.Message);
      }


      foreach (var recipe in res) {
        try {
          ICursor iter = db.RawQuery(recipe.ListItemsCommand, null);
          int colCount = iter.ColumnCount;
          while (iter.MoveToNext()) {
            string[] columns = new string[colCount];
            for (int i = 0; i < colCount; i++)
              columns[i] = iter.GetString(i);
            recipe.ParseItemFromQueryResult(columns);
          }
          iter.Close();
        } catch (SQLException e) {
          Console.WriteLine($"While parsing items for {recipe.Name}: " + e.Message);
        }
      }

      return res;
    }

    #endregion
  }
}