using System;
using System.Collections.Generic;
using System.Text;

namespace paujo.juze {
  public class Nicotine {


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
    public float Concentration {
      get; set;
    }
  }
}
