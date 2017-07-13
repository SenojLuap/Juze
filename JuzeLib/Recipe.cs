using System.Collections.Generic;
using System.Text;

namespace paujo.juze {
  public class Recipe : JuzeBaseType {

    /// <summary>
    /// Names of elements in the recipe SQLite table.
    /// </summary>
    public const string TABLE_NAME = "RECIPE";
    public const string ID_COL = "ID";
    public const string NAME_COL = "NAME";
    public const string VG_COL = "VG";
    public const string NICOTINE_COL = "NICOTINE_ID";
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
    public static string[] CreateTableCommands {
      get {
        string[] res = new string[2];
        res[0] = $"CREATE TABLE {TABLE_NAME} (" +
          $"{ID_COL} INTEGER PRIMARY KEY AUTOINCREMENT, " +
          $"{NAME_COL} TEXT, " +
          $"{VG_COL} REAL, " +
          $"FOREIGN KEY ({NICOTINE_COL}) REFERENCES {Nicotine.TABLE_NAME}.{Nicotine.ID_COL}, " +
          $"{TARGET_NIC_COL} REAL);";
        res[1] = RecipeItem.CreateTableCommand;
        return res;
      }
    }

    /// <summary>
    /// The commands required to insert the recipe and its items into
    /// their tables.
    /// </summary>
    public string[] InsertCommands {
      get {
        List<string> res = new List<string>();
        res.Add($"INSERT INTO {TABLE_NAME} ({NAME_COL}, {VG_COL}, {NICOTINE_COL}, {TARGET_NIC_COL}) " +
          $"VALUES (\"{Name}\", {VG}, {Nicotine.ID}, {TargetNicotine});");
        foreach (var item in Flavors)
          res.Add(item.InsertCommand);
        return res.ToArray();
      }
    }

    /// <summary>
    /// The commands required to update the recipe and its items in 
    /// their tables.
    /// </summary>
    public string[] UpdateCommands {
      get {
        List<string> res = new List<string>();

        res.Add($"UPDATE {TABLE_NAME} SET {NAME_COL}=\"{Name}\", {VG_COL}={VG}, " +
          $"{NICOTINE_COL}={Nicotine.ID}, {TARGET_NIC_COL}={TargetNicotine} " +
          $"WHERE {ID_COL}={ID};");
        foreach (var flav in Flavors)
          res.Add(flav.UpdateCommand);

        return res.ToArray();
      }
    }

    /// <summary>
    /// The commands required to remove the recipe and its items from the
    /// their tables.
    /// </summary>
    public string[] DeleteCommands {
      get {
        List<string> res = new List<string>();

        res.Add($"DELETE FROM {TABLE_NAME} WHERE {ID_COL}={ID};");
        foreach (var flav in Flavors)
          res.Add(flav.DeleteCommand);

        return res.ToArray();
      }
    }

    /// <summary>
    /// The command required to list all recipes from their table.
    /// Note: This will NOT list the recipes' items. ListItemsCommand must be run
    /// for each and parsed by each.
    /// </summary>
    public static string ListCommand {
      get {
        StringBuilder res = new StringBuilder();
        res.Append($"SELECT ");
        foreach (var field in new string[] { ID_COL, NAME_COL, VG_COL, TARGET_NIC_COL })
          res.Append($"{TABLE_NAME}.{field}, ");
        foreach (var field in new string[] { Nicotine.ID_COL, Nicotine.NAME_COL, Nicotine.VG_COL, Nicotine.CONC_COL })
          res.Append($"{Nicotine.TABLE_NAME}.{field}, ");
        res.Length = res.Length - 2;
        res.Append($" FROM {TABLE_NAME} JOIN {Nicotine.TABLE_NAME} ");
        res.Append($" ON {TABLE_NAME}.{NICOTINE_COL}={Nicotine.TABLE_NAME}.{Nicotine.ID_COL};");

        return res.ToString();
      }
    }

    /// <summary>
    /// The command required to list all items owned by this recipe.
    /// </summary>
    public string ListItemsCommand {
      get {
        return RecipeItem.ListCommandByOwner(this);
      }
    }

    /// <summary>
    /// From the results of an SQLite query, parse a recipe.
    /// </summary>
    /// <param name="columns">The columns of a row generated by ListCommand.</param>
    /// <returns>The parsed recipe.</returns>
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
    /// From the results of an SQLite query, parse an item 
    /// belonging to this recipe.
    /// </summary>
    /// <param name="columns">The columns of a row generated from ListItemsCommand.</param>
    public void ParseItemFromQueryResult(string[] columns) {
      RecipeItem item = RecipeItem.ParseFromQueryResult(columns);
      item.Owner = this;
      Flavors.Add(item);
    }
  }
}
