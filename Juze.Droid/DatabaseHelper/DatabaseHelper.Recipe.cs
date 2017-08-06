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
    /// Names of elements in the recipe SQLite table.
    /// </summary>
    public const string RECIPE_TABLE_NAME = "RECIPE";
    public const string RECIPE_ID_COL = "ID";
    public const string RECIPE_NAME_COL = "NAME";
    public const string RECIPE_VG_COL = "VG";
    public const string RECIPE_BATCHSIZE_COL = "BATCHSIZE";
    public const string RECIPE_NICOTINE_COL = "NICOTINE_ID";
    public const string RECIPE_TARGET_NIC_COL = "TARGET_NICOTINE";

    /// <summary>
    /// Create the database table for recipes.
    /// </summary>
    /// <param name="db">The database to add the table to.</param>
    public void CreateRecipeTable(SQLiteDatabase db) {
      string cmd = $"CREATE TABLE {RECIPE_TABLE_NAME} (" +
        $"{RECIPE_ID_COL} INTEGER PRIMARY KEY AUTOINCREMENT, " +
        $"{RECIPE_NAME_COL} TEXT, " +
        $"{RECIPE_VG_COL} TINYINT UNSIGNED, " +
        $"{RECIPE_NICOTINE_COL} INTEGER, " +
        $"{RECIPE_BATCHSIZE_COL} INTEGER, " +
        $"{RECIPE_TARGET_NIC_COL} REAL, " +
        $"FOREIGN KEY ({RECIPE_NICOTINE_COL}) REFERENCES {NIC_TABLE_NAME}({NIC_ID_COL}));";
      db.ExecSQL(cmd);
    }

    /// <summary>
    /// Add a recipe to the database.
    /// </summary>
    /// <param name="recipe">The recipe to add.</param>
    public void PutRecipe(Recipe recipe) {
      string cmd = $"INSERT INTO {RECIPE_TABLE_NAME} ({RECIPE_NAME_COL}, {RECIPE_VG_COL}, " +
        $"{RECIPE_NICOTINE_COL}, {RECIPE_BATCHSIZE_COL}, {RECIPE_TARGET_NIC_COL}) " +
        $"VALUES (\"{recipe.Name}\", {recipe.VG}, {recipe.Nicotine.ID}, {recipe.BatchSize}, {recipe.TargetNicotine});";
      try {
        SQLiteDatabase db = WritableDatabase;
        db.ExecSQL(cmd);
        recipe.ID = GetLastInsertedID();
        foreach (var item in recipe.Flavors) {
          PutRecipeItem(item, recipe);
        }
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Update the recipe in its table.
    /// </summary>
    /// <param name="recipe">The recipe to update.</param>
    public void UpdateRecipe(Recipe recipe) {
      string cmd = $"UPDATE {RECIPE_TABLE_NAME} SET {RECIPE_NAME_COL}=\"{recipe.Name}\", " +
        $"{RECIPE_VG_COL}={recipe.VG}, {RECIPE_NICOTINE_COL}={recipe.Nicotine.ID}, " +
        $"{RECIPE_BATCHSIZE_COL}={recipe.BatchSize}, " +
        $"{RECIPE_TARGET_NIC_COL}={recipe.TargetNicotine} " +
        $"WHERE {RECIPE_ID_COL}={recipe.ID};";
      try {
        SQLiteDatabase db = WritableDatabase;
        db.ExecSQL(cmd);
        foreach (var item in recipe.Flavors)
          RemoveRecipeItem(item);
        foreach (var item in recipe.Flavors)
          PutRecipeItem(item, recipe);
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Remove a recipe from the database.
    /// </summary>
    /// <param name="recipe">The recipe to remove.</param>
    public void RemoveRecipe(Recipe recipe) {
      string cmd = $"DELETE FROM {RECIPE_TABLE_NAME} WHERE {RECIPE_ID_COL}={recipe.ID};";
      try {
        SQLiteDatabase db = WritableDatabase;
        db.ExecSQL(cmd);
        foreach (var item in recipe.Flavors)
          RemoveRecipeItem(item);
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Get all the recipes from the database.
    /// </summary>
    /// <returns>The collection of all of the recipes.</returns>
    public IList<Recipe> GetRecipes() {
      List<Recipe> res = new List<Recipe>();

      StringBuilder cmd = new StringBuilder();
      cmd.Append($"SELECT ");
      foreach (var field in new string[] {RECIPE_ID_COL, RECIPE_NAME_COL, RECIPE_VG_COL, RECIPE_BATCHSIZE_COL, RECIPE_TARGET_NIC_COL })
        cmd.Append(RECIPE_TABLE_NAME + '.' + field + ", ");
      foreach (var field in new string[] { NIC_ID_COL, NIC_NAME_COL, NIC_VG_COL, NIC_CONC_COL })
        cmd.Append(NIC_TABLE_NAME + '.' + field + ", ");
      cmd.Length = cmd.Length - 2;
      cmd.Append($"FROM {RECIPE_TABLE_NAME} JOIN {NIC_TABLE_NAME} ");
      cmd.Append($"ON {RECIPE_TABLE_NAME}.{RECIPE_ID_COL}={NIC_TABLE_NAME}.{NIC_ID_COL};");
      try {
        ICursor iter = ReadableDatabase.RawQuery(cmd.ToString(), null);
        while (iter.MoveToNext()) {
          Recipe recipe = ParseRecipe(iter);
          if (recipe != null)
            res.Add(recipe);
        }
        iter.Close();
        foreach (var recipe in res)
          recipe.Flavors = GetRecipeItemsByOwner(recipe);
      } catch (SQLiteAbortException e) {
        Console.WriteLine(e.Message);
      }
      return res;
    }

    /// <summary>
    /// Parse a recipe from the current row.
    /// </summary>
    /// <param name="iter">The database cursor pointing to a row of a query.</param>
    /// <returns>The parsed recipe.</returns>
    private Recipe ParseRecipe(ICursor iter) {
      Recipe res = new Recipe();
      Nicotine resNic = new Nicotine();
      res.Nicotine = resNic;
      res.ID = iter.GetInt(0);
      res.Name = iter.GetString(1);
      res.VG = (byte)iter.GetInt(2);
      res.BatchSize = iter.GetInt(3);
      res.TargetNicotine = iter.GetFloat(4);
      resNic.ID = iter.GetInt(5);
      resNic.Name = iter.GetString(6);
      resNic.VG = (byte)iter.GetInt(7);
      resNic.Concentration = iter.GetInt(8);

      return res;
    }
  }
}