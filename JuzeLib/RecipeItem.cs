using System;
using System.Collections.Generic;
using System.Text;

namespace paujo.juze {
  public class RecipeItem {

    public const string TABLE_NAME = "RECIPE_ITEM";
    public const string RECIPE_COL = "RECIPE_ID";
    public const string FLAVOR_COL = "FLAVOR_ID";
    public const string PERCENTAGE_COL = "PERCENTAGE";

    /// <summary>
    /// The recipe this item belongs to.
    /// </summary>
    public Recipe Owner {
      get; set;
    }

    /// <summary>
    /// The flavor being added to the recipe.
    /// </summary>
    public Flavor Flavor {
      get; set;
    }

    /// <summary>
    /// The percentage amount of flavor in the recipe.
    /// </summary>
    public float Percentage {
      get; set; 
    }

    /// <summary>
    /// The command required to generate a table for recipe items.
    /// </summary>
    public static string CreateTableCommand {
      get {
        return $"CREATE TABLE {TABLE_NAME} (FOREIGN KEY({RECIPE_COL}) REFERENCES {Recipe.TABLE_NAME}({Recipe.ID_COL}), " +
        $"FOREIGN KEY({FLAVOR_COL}) REFERENCES {Flavor.TABLE_NAME}({Flavor.ID_COL}), " +
        $"{PERCENTAGE_COL} REAL);";
      }
    }

    /// <summary>
    /// The command to insert the recipe item into its table.
    /// </summary>
    public string InsertCommand {
      get {
        return $"INSERT INTO {TABLE_NAME} ({RECIPE_COL}, {FLAVOR_COL}, {PERCENTAGE_COL}) VALUES ({Owner.ID}, {Flavor.ID}, {Percentage});";
      }
    }
  }
}
