using System;
using System.Collections.Generic;
using System.Text;

namespace paujo.juze {
  public class Nicotine {

    /// <summary>
    /// Names of elements in the nicotine SQLite table.
    /// </summary>
    public static string TABLE_NAME = "NICOTINE";
    public static string ID_COL = "ID";
    public static string NAME_COL = "NAME";
    public static string VG_COL = "VG";
    public static string CONC_COL = "CONCENTRATION";

    /// <summary>
    /// Unique ID for the nicotine.
    /// </summary>
    public int ID {
      get; set;
    }

    /// <summary>
    /// Human-readable name for the nicotine.
    /// </summary>
    public string Name {
      get; set;
    }

    /// <summary>
    /// Percentage of VG in the nicotine solution.
    /// </summary>
    public float VG {
      get; set;
    }

    /// <summary>
    /// Percentage of PG in the nicotine solution.
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
    /// Nicotine concentration, as mg/mL.
    /// </summary>
    public int Concentration {
      get; set;
    }

    /// <summary>
    /// Weight of the nicotine, in g/mL.
    /// </summary>
    public float Weight {
      get {
        float nic = Concentration / Constants.NICOTINE_GRAVITY;
        float rest = 1.0f - nic;
        float vg = rest * VG;
        float pg = rest * PG;
        vg *= Constants.VG_GRAVITY;
        pg *= Constants.PG_GRAVITY;
        return (Concentration / 1000f) + vg + pg;
      }
    }

    /// <summary>
    /// The command required to generate tables for the Nicotines.
    /// </summary>
    public static string CreateTableCommand {
      get {
        return $@"CREATE TABLE {TABLE_NAME} ({ID_COL} INTEGER PRIMARY KEY AUTOINCREMENT, {NAME_COL} TEXT, {VG_COL} REAL, {CONC_COL} INTEGER);";
      }
    }

    /// <summary>
    /// The command required to insert the nicotine into a databse.
    /// </summary>
    public string InsertCommand {
      get {
        return $"INSERT INTO {TABLE_NAME} ({NAME_COL}, {VG_COL}, {CONC_COL}) VALUES (\"{Name}\", {VG}, {Concentration});";
      }
    }

    /// <summary>
    /// The command required to update the nicotine in a table.
    /// </summary>
    public string UpdateCommand {
      get {
        return $"UPDATE {TABLE_NAME} SET {NAME_COL}=\"{Name}\", {VG_COL}={VG}, {CONC_COL}={Concentration} WHERE {ID_COL}={ID};";
      }
    }

    /// <summary>
    /// The command required to remove the nicotine from the table.
    /// </summary>
    public string DeleteCommand {
      get {
        return $"DELETE FROM {TABLE_NAME} WHERE {ID_COL}={ID};";
      }
    }

    /// <summary>
    /// The command required to list out all of the nicotines in a database.
    /// </summary>
    public static string ListCommand {
      get {
        return $"SELECT * FROM {TABLE_NAME};";
      }
    }

    /// <summary>
    /// From the results of a SQLite query, parse  a nicotine.
    /// </summary>
    /// <param name="columns">The columns read from the database.</param>
    /// <returns>The resulting nicotine.</returns>
    public static Nicotine ParseFromQueryResult(string[] columns) {
      Nicotine nic = new Nicotine();

      nic.ID = int.Parse(columns[0]);
      nic.Name = columns[1];
      nic.VG = float.Parse(columns[2]);
      nic.Concentration = int.Parse(columns[3]);

      return nic;
    }
  }
}
