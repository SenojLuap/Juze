using System;
using System.Collections.Generic;
using System.Text;

namespace paujo.juze {
  public class RecipeItem {

    /// <summary>
    /// Names of elements in the recipe item SQLite table.
    /// </summary>
    public const string TABLE_NAME = "RECIPE_ITEM";
    public const string ID_COL = "ID";
    public const string FLAVOR_COL = "FLAVOR_ID";
    public const string OWNER_COL = "RECIPE_ID";
    public const string PERCENTAGE_COL = "PERCENTAGE";

    /// <summary>
    /// Unique ID for the recipe item.
    /// </summary>
    public int ID {
      get; set;
    }
    
    /// <summary>
    /// The flavor being added to the recipe.
    /// </summary>
    public Flavor Flavor {
      get; set;
    }

    /// <summary>
    /// The recipe that owns the item.
    /// </summary>
    public Recipe Owner {
      get; set;
    }

    /// <summary>
    /// The percentage amount of flavor in the recipe.
    /// </summary>
    public float Percentage {
      get; set; 
    }

    /// <summary>
    /// The command required to generate tables for the recipe item.
    /// </summary>
    public static string CreateTableCommand {
      get {
        return $"CREATE TABLE {TABLE_NAME} (" +
          $"{ID_COL} INTEGER PRIMARY KEY AUTOINCREMENT, " +
          $"FOREIGN KEY({FLAVOR_COL}) REFERENCES {Flavor.TABLE_NAME}({Flavor.ID_COL}), " +
          $"FOREIGN KEY({OWNER_COL}) REFERENCES {Recipe.TABLE_NAME}({Recipe.ID_COL}), " +
          $"{PERCENTAGE_COL} REAL);";
      }
    }

    /// <summary>
    /// The command required to insert the recipe item into its table.
    /// </summary>
    public string InsertCommand {
      get {
        return $"INSERT INTO {TABLE_NAME} ({FLAVOR_COL} {OWNER_COL} {PERCENTAGE_COL}) " +
          $"VALUES ({Flavor.ID}, {Owner.ID}, {Percentage});";
      }
    }

    /// <summary>
    /// The command required to update the recipe item in its table.
    /// </summary>
    public string UpdateCommand {
      get {
        return $"UPDATE {TABLE_NAME} SET {FLAVOR_COL}={Flavor.ID}, {OWNER_COL}={Owner.ID}, {PERCENTAGE_COL}={Percentage} " +
          $"WHERE {ID_COL}={ID};";
      }
    }

    /// <summary>
    /// The command required to remove the recipe item from its table.
    /// </summary>
    public string DeleteCommand {
      get {
        return $"DELETE FROM {TABLE_NAME} WHERE {ID_COL}={ID};";
      }
    }

    /// <summary>
    /// The command required to list all of the recipe items owned by the specified
    /// recipe.
    /// </summary>
    /// <param name="owner">The recipe to retrieve items for.</param>
    /// <returns>The command required to retrieve the items.</returns>
    public static string ListCommandByOwner(Recipe owner) {
      StringBuilder res = new StringBuilder();
      res.Append($"SELECT {TABLE_NAME}.{ID_COL}, {TABLE_NAME}.{PERCENTAGE_COL}, ");
      foreach (var flavField in new string[] { Flavor.ID_COL, Flavor.NAME_COL, Flavor.PG_COL, Flavor.RECPER_COL })
        res.Append($"{Flavor.TABLE_NAME}.{flavField}, ");
      res.Length = res.Length - 2;
      res.Append($" FROM {TABLE_NAME} JOIN {Flavor.TABLE_NAME} ");
      res.Append($"ON {TABLE_NAME}.{FLAVOR_COL}={Flavor.TABLE_NAME}.{Flavor.ID_COL} ");
      res.Append($"WHERE {TABLE_NAME}.{OWNER_COL}={owner.ID};");
      return res.ToString();
    }

    /// <summary>
    /// From the results of a SQLite query, parse a recipe item.
    /// </summary>
    /// <param name="columns">The columns of a row returned from the query.</param>
    /// <returns>The parsed recipe item.</returns>
    public static RecipeItem ParseFromQueryResult(string[] columns) {
      RecipeItem res = new RecipeItem();
      res.ID = int.Parse(columns[0]);
      res.Percentage = float.Parse(columns[1]);

      Flavor resFlav = new Flavor();
      resFlav.ID = int.Parse(columns[2]);
      resFlav.Name = columns[3];
      resFlav.PG = (int.Parse(columns[4]) > 0);
      resFlav.RecommendedPercentage = float.Parse(columns[5]);
      res.Flavor = resFlav;

      return res;
    }
  }
}
