using System.Collections.Generic;
using System.Text;

namespace paujo.juze {
  public class Recipe : JuzeBaseType {

    public const string TABLE_NAME = "RECIPE";
    public const string ID_COL = "ID";
    public const string NAME_COL = "NAME";
    public const string VG_COL = "VG";
    public const string NICOTINE_COL = "NICOTINE";
    public const string TARGET_NIC_COL = "TARGET_NICOTINE";

    /// <summary>
    /// Percentage of VG in the recipe.
    /// </summary>
    public float VG {
      get; set;
    }

    /// <summary>
    /// Percentage of PG in the recipe.
    /// </summary>
    public float PG {
      get {
        return 1.0f - VG;
      }
      set {
        VG = 1.0f - value;
      }
    }

    /// <summary>
    /// Nicotine solution used.
    /// </summary>
    public Nicotine Nicotine {
      get; set;
    }

    /// <summary>
    /// Target nicotine content, as mg/mL
    /// </summary>
    public float TargetNicotine {
      get; set;
    }

    /// <summary>
    /// The flavors added to the recipe.
    /// </summary>
    public IList<RecipeItem> Flavors {
      get; set;
    }

    /// <summary>
    /// Get a brew recipe with concrete amounts.
    /// </summary>
    /// <param name="batchSize">The desired size of the brew, in mL</param>
    /// <returns>The finalized recipe, or null if the recipe isn't brewable.</returns>
    public FinalRecipe GetFinalRecipe(float batchSize, bool byWeight) {

      if (Nicotine.Concentration < TargetNicotine) {
        // Impossible to brew.
        return null;
      }

      FinalRecipe res = new FinalRecipe();
      res.Origin = this;

      float pgVolume = 0.0f;
      float vgVolume = 0.0f;

      res.Nicotine = TargetNicotine * batchSize / Nicotine.Concentration;
      pgVolume += res.Nicotine * Nicotine.PG;
      vgVolume += res.Nicotine * Nicotine.VG;

      if (byWeight)
        res.Nicotine *= Nicotine.Weight;

      res.Flavors = new List<float>();
      foreach (var flavor in Flavors) {
        float flavAmount = flavor.Percentage * batchSize;

        if (flavor.Flavor.PG)
          pgVolume += flavAmount;
        else
          vgVolume += flavAmount;

        if (byWeight)
          flavAmount *= flavor.Flavor.Weight;
        res.Flavors.Add(flavAmount);
      }

      res.PG = (batchSize * PG) - pgVolume;
      res.VG = (batchSize * VG) - vgVolume;

      if (byWeight) {
        res.PG *= Constants.PG_GRAVITY;
        res.VG *= Constants.VG_GRAVITY;
      }

      return res;
    }

    /// <summary>
    /// The command required to generate tables for recipes.
    /// </summary>
    public static string CreateTableCommand {
      get {
        return $"CREATE TABLE {TABLE_NAME} ({ID_COL} INTEGER PRIMARY KEY AUTOINCREMENT, " +
          $"{NAME_COL} TEXT, {VG_COL} REAL, " +
          $"FOREIGN KEY ({NICOTINE_COL}) REFERENCES {Nicotine.TABLE_NAME}({Nicotine.ID_COL}), " +
          $"{TARGET_NIC_COL} REAL);";
      }
    }

    /// <summary>
    /// The commands required to insert the recipe and its sub objects into their respective tables."
    /// </summary>
    public string[] InsertCommands {
      get {
        List<string> commands = new List<string>();
        commands.Add($"INSERT INTO {TABLE_NAME} ({NAME_COL}, {VG_COL}, {NICOTINE_COL}, {TARGET_NIC_COL}) VALUES (\"{Name}\", {VG}, {Nicotine.ID}, {TargetNicotine});");
        foreach (var recipeItem in Flavors)
          commands.Add(recipeItem.InsertCommand);
        return commands.ToArray();
      }
    }

    /// <summary>
    /// The commands required to update the recipe and its sub objects in their tables.
    /// </summary>
    public string[] UpdateCommands {
      get {
        List<string> res = new List<string>();
        res.Add($"UPDATE {TABLE_NAME} SET {NAME_COL}=\"{Name}\", {VG_COL}={VG}, {NICOTINE_COL}={Nicotine.ID}, {TARGET_NIC_COL}={TargetNicotine} WHERE {ID_COL} = {ID};");
        res.Add($"DELETE FROM {RecipeItem.TABLE_NAME} WHERE {RecipeItem.RECIPE_COL}={ID};");
        foreach (var recipeItem in Flavors)
          res.Add(recipeItem.InsertCommand);
        return res.ToArray();
      }
    }

    /// <summary>
    /// The commands required to remove the recipe and its sub objects from their tables.
    /// </summary>
    public string[] DeleteCommands {
      get {
        List<string> res = new List<string>();
        res.Add($"DELETE FROM {TABLE_NAME} WHERE {ID_COL}={ID};");
        res.Add($"DELETE FROM {RecipeItem.TABLE_NAME} WHERE {RecipeItem.RECIPE_COL}={ID};");
        return res.ToArray();
      }
    }

    /// <summary>
    /// The command to list all of the recipes in a database.
    /// </summary>
    public static string ListCommand {
      get {
        StringBuilder res = new StringBuilder();
        res.Append("SELECT ");
        string[] recipeFields = {ID_COL, NAME_COL, VG_COL, TARGET_NIC_COL};
        foreach (var recipeField in recipeFields)
          res.Append(TABLE_NAME + "." + recipeField + ", ");

        string[] nicotineFields = { Nicotine.ID_COL, Nicotine.NAME_COL, Nicotine.VG_COL, Nicotine.CONC_COL};
        foreach (var nicotineField in nicotineFields)
          res.Append(Nicotine.TABLE_NAME + "." + nicotineField + ", ");
        res.Length = res.Length - 2;
        res.Append($" FROM {TABLE_NAME} JOIN {Nicotine.TABLE_NAME} ON {TABLE_NAME}.{NICOTINE_COL} = {Nicotine.TABLE_NAME}.{Nicotine.ID_COL};");
        return res.ToString();
      }
    }

    /// <summary>
    /// The command to list all items owned by the recipe in the database.
    /// </summary>
    public string ListItemsCommand {
      get {
        StringBuilder res = new StringBuilder();
        res.Append($"SELECT {RecipeItem.TABLE_NAME}.{RecipeItem.PERCENTAGE_COL}, ");
        string[] flavorFields = { Flavor.ID_COL, Flavor.NAME_COL, Flavor.PG_COL, Flavor.RECPER_COL };
        foreach (var flavorField in flavorFields)
          res.Append(Flavor.TABLE_NAME + "." + flavorField + ", ");
        res.Length = res.Length - 2;
        res.Append($" FROM {RecipeItem.TABLE_NAME} JOIN {Flavor.TABLE_NAME} ON {RecipeItem.TABLE_NAME}.{RecipeItem.FLAVOR_COL} = {Flavor.TABLE_NAME}.{Flavor.ID_COL};");
        return res.ToString();
      }
    }

    /// <summary>
    /// From the results of a SQLite query, parse a flavor.
    /// </summary>
    /// <param name="columns">The values for a single row of the query.</param>
    /// <returns>The recipe represented on that row.</returns>
    public static Recipe ParseFromQueryResult(string[] columns) {
      Recipe res = new Recipe();
      res.ID = int.Parse(columns[0]);
      res.Name = columns[1];
      res.VG = float.Parse(columns[2]);
      res.TargetNicotine = float.Parse(columns[3]);

      Nicotine resNic = new Nicotine();
      resNic.ID = int.Parse(columns[4]);
      resNic.Name = columns[5];
      resNic.VG = float.Parse(columns[6]);
      resNic.Concentration = int.Parse(columns[7]);

      res.Nicotine = resNic;

      return res;
    }

    /// <summary>
    /// Frome the results of an SQLite query, parse a recipe item owned by this recipe.
    /// </summary>
    /// <param name="columns">The values from a single row of the query.</param>
    public void ParseItemFromQueryResult(string[] columns) {
      RecipeItem res = new RecipeItem();
      res.Owner = this;
      res.Percentage = float.Parse(columns[0]);

      Flavor resFlav = new Flavor();
      resFlav.ID = int.Parse(columns[1]);
      resFlav.Name = columns[2];
      resFlav.PG = (int.Parse(columns[3]) > 0);
      resFlav.RecommendedPercentage = float.Parse(columns[4]);
      res.Flavor = resFlav;
      Flavors.Add(res);
    }
  }
}
