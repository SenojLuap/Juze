﻿using SQLite;

namespace paujo.juze {
  public class Nicotine {


    /// <summary>
    /// Unique ID for the nicotine.
    /// </summary>
    [PrimaryKey, AutoIncrement]
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
    public float Concentration {
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
  }
}
