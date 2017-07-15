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
    /// Names of elements in the recipe item SQLite table.
    /// </summary>
    public const string RI_TABLE_NAME = "RECIPE_ITEM";
    public const string RI_ID_COL = "ID";
    public const string RI_FLAVOR_COL = "FLAVOR_ID";
    public const string RI_PERCENTAGE_COL = "PERCENTAGE";

    /// <summary>
    /// Names of elements in the recipe/recipe item join table.
    /// </summary>
    public const string RECIPE_RI_TABLE_NAME = "RECIPE_RECIPE_ITEM_JOIN";
    public const string RECIPE_RI_RECIPE_ID = "RECIPE_ID";
    public const string RECIPE_RI_RI_ID = "RI_ID";

    /// <summary>
    /// Create the recipe item table in the database.
    /// </summary>
    /// <param name="db">The database to add the table to.</param>
    public void CreateRecipeItemTable(SQLiteDatabase db) {
      string cmd = $"CREATE TABLE {RI_TABLE_NAME} (" +
          $"{RI_ID_COL} INTEGER PRIMARY KEY AUTOINCREMENT, " +
          $"FOREIGN KEY({RI_FLAVOR_COL}) REFERENCES {FLAVOR_TABLE_NAME}({FLAVOR_ID_COL}), " +
          $"{RI_PERCENTAGE_COL} REAL);";
      db.ExecSQL(cmd);

      cmd = $"CREATE TABLE {RECIPE_RI_TABLE_NAME} (" +
        $"FOREIGN KEY({RECIPE_RI_RECIPE_ID}) REFERENCES {RECIPE_TABLE_NAME}({RECIPE_ID_COL}), " +
        $"FOREIGN KEY({RECIPE_RI_RI_ID}) REFERENCES {RI_TABLE_NAME}({RI_ID_COL}));";
      db.ExecSQL(cmd);
    }

    /// <summary>
    /// Add the recipe item to its table.
    /// </summary>
    /// <param name="recipeItem">The recipe item to add.</param>
    public void PutRecipeItem(RecipeItem recipeItem, Recipe owner) {
      string cmd = $"INSERT INTO {RI_TABLE_NAME} ({RI_FLAVOR_COL}, {RI_PERCENTAGE_COL}) " +
          $"VALUES ({recipeItem.Flavor.ID}, {recipeItem.Percentage});";
      string joinCmd = $"INSERT INTO {RECIPE_RI_TABLE_NAME} ({RECIPE_RI_RECIPE_ID}, " +
        $" {RECIPE_RI_RI_ID}) VALUES ({owner.ID}, {recipeItem.ID});";
      try {
        WritableDatabase.ExecSQL(cmd);
        recipeItem.ID = GetLastInsertedID();
        WritableDatabase.ExecSQL(cmd);
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Update a recipe item in its table.
    /// </summary>
    /// <param name="recipeItem">The recipe item to update.</param>
    public void UpdateRecipeItem(RecipeItem recipeItem) {
      string cmd = $"UPDATE {RI_TABLE_NAME} SET {RI_FLAVOR_COL}={recipeItem.Flavor.ID}, {RI_PERCENTAGE_COL}={recipeItem.Percentage} " +
          $"WHERE {RI_ID_COL}={recipeItem.ID};";
      try {
        WritableDatabase.ExecSQL(cmd);
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Remove a recipe item from the database.
    /// </summary>
    /// <param name="recipeItem">The recipe item to remove.</param>
    public void RemoveRecipeItem(RecipeItem recipeItem) {
      string cmd = $"DELETE FROM {RI_TABLE_NAME} WHERE {RI_ID_COL}={recipeItem.ID};";
      string joinCmd = $"DELETE FROM {RECIPE_RI_TABLE_NAME} WHERE {RECIPE_RI_RI_ID}={recipeItem.ID};";
      try {
        WritableDatabase.ExecSQL(cmd);
        WritableDatabase.ExecSQL(joinCmd);
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Retrieve all recipe items owned by a recipe.
    /// </summary>
    /// <param name="owner">The owner of the desired recipe items.</param>
    /// <returns>The owned recipe items.</returns>
    public IList<RecipeItem> GetRecipeItemsByOwner(Recipe owner) {
      List<RecipeItem> res = new List<RecipeItem>();

      string cmd = $"SELECT {RI_TABLE_NAME}.{RI_ID_COL}, {RI_TABLE_NAME}.{RI_PERCENTAGE_COL}, " +
        $"{FLAVOR_TABLE_NAME}.{FLAVOR_ID_COL}, {FLAVOR_TABLE_NAME}.{FLAVOR_NAME_COL}, " +
        $"{FLAVOR_TABLE_NAME}.{FLAVOR_PG_COL}, {FLAVOR_TABLE_NAME}.{FLAVOR_RECPER_COL} " +
        $"FROM {RI_TABLE_NAME} JOIN {FLAVOR_TABLE_NAME} " +
        $"ON {RI_TABLE_NAME}.{RI_FLAVOR_COL}={FLAVOR_TABLE_NAME}.{FLAVOR_ID_COL} " +
        $"JOIN {RECIPE_RI_TABLE_NAME} " +
        $"ON {RI_TABLE_NAME}.{RI_ID_COL}={RECIPE_RI_TABLE_NAME}.{RECIPE_RI_RI_ID} " +
        $"WHERE {RECIPE_RI_RECIPE_ID}={owner.ID};";
      try {
        ICursor iter = ReadableDatabase.RawQuery(cmd, null);
        while (iter.MoveToNext()) {
          RecipeItem ri = ParseRecipeItem(iter);
          if (ri != null)
            res.Add(ri);
        }
        iter.Close();
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }

      return res;
    }

    /// <summary>
    /// Parse a recipe item from the current row.
    /// </summary>
    /// <param name="cursor">The database cursor, pointing to a valid query result row.</param>
    /// <returns>The parsed recipe item.</returns>
    private RecipeItem ParseRecipeItem(ICursor iter) {
      RecipeItem res = new RecipeItem();
      Flavor resFlav = new Flavor();
      res.Flavor = resFlav;
      res.ID = iter.GetInt(0);
      res.Percentage = iter.GetFloat(1);
      resFlav.ID = iter.GetInt(2);
      resFlav.Name = iter.GetString(3);
      resFlav.PG = (iter.GetInt(4) > 0);
      resFlav.RecommendedPercentage = iter.GetFloat(5);

      return res;
    }
  }
}