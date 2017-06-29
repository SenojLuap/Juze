using System;

namespace paujo.juze {
  public class Flavor {

    /// <summary>
    /// Names of elements in the flavors SQLite table.
    /// </summary>
    public const string TABLE_NAME = "FLAVOR";
    public const string NAME_COL = "NAME";
    public const string ID_COL = "ID";
    public const string PG_COL = "PG";
    public const string RECPER_COL = "REC_PER";

    /// <summary>
    /// Unique ID for the flavor.
    /// </summary>
    public int ID {
      get; set;
    }


    /// <summary>
    /// Human-readable name of the flavor.
    /// </summary>
    public string Name {
      get; set;
    }


    /// <summary>
    /// Is the flavor PG-based?
    /// </summary>
    public bool PG {
      get; set;
    }


    /// <summary>
    /// Recommended starting percentge for new recipes.
    /// </summary>
    public float RecommendedPercentage {
      get; set;
    }


    /// <summary>
    /// The weight of the flavoring, in g/mL.
    /// </summary>
    public float Weight {
      get {
        return PG ? Constants.PG_GRAVITY : Constants.VG_GRAVITY;
      }
    }


    /// <summary>
    /// The command required to generate tables for Flavors.
    /// </summary>
    public static string CreateTableCommand {
      get {
        return $@"CREATE TABLE {TABLE_NAME} ({ID_COL} INTEGER PRIMARY KEY AUTOINCREMENT, {NAME_COL} TEXT, {PG_COL} INTEGER, {RECPER_COL} REAL)";
      }
    }

    /// <summary>
    /// The command required to insert the flavor into a database.
    /// </summary>
    public string InsertCommand {
      get {
        int pg = PG ? 1 : 0;
        return $"INSERT INTO {TABLE_NAME} ({NAME_COL}, {PG_COL}, {RECPER_COL}) VALUES (\"{Name}\", {pg}, {RecommendedPercentage});";
      }
    }

    /// <summary>
    /// The command required to update the flavor in the table.
    /// </summary>
    public string UpdateCommand {
      get {
        int pg = PG ? 1 : 0;
        return $"UPDATE {TABLE_NAME} SET {NAME_COL}=\"{Name}\", {PG_COL}={pg}, {RECPER_COL}={RecommendedPercentage} WHERE {ID_COL} = {ID};";
      }
    }

    /// <summary>
    /// The command required to remove the flavor from the table.
    /// </summary>
    public string DeleteCommand {
      get {
        return $"DELETE FROM {TABLE_NAME} WHERE {ID_COL}={ID};";
      }
    }

    /// <summary>
    /// The command required to list out all of the flavors in a database.
    /// </summary>
    public static string ListCommand {
      get {
        return $"SELECT * FROM {TABLE_NAME};";
      }
    }

    /// <summary>
    /// From the results of a SQLite query, parse a flavor.
    /// </summary>
    /// <param name="columns">The values from a single row of the query.</param>
    /// <returns>The Flavor represented on that row.</returns>
    public static Flavor ParseFromQueryResult(string[] columns) {
      Flavor res = new Flavor();
      res.ID = int.Parse(columns[0]);
      res.Name = columns[1];
      res.PG = int.Parse(columns[2]) > 0;
      res.RecommendedPercentage = float.Parse(columns[3]);
      return res;
    }
  }
}
