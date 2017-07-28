using System;
using System.Collections.Generic;
using System.Text;

namespace paujo.juze {
  public class Nicotine : JuzeNamedType {

    /// <summary>
    /// Percentage of VG in the nicotine solution.
    /// </summary>
    public int VG {
      get; set;
    }

    /// <summary>
    /// Percentage of PG in the nicotine solution.
    /// </summary>
    public int PG {
      get {
        return 100 - VG;
      }
      set {
        VG = 100 - value;
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
        float vg = rest * (VG/100.0f);
        float pg = rest * (PG/100.0f);
        vg *= Constants.VG_GRAVITY;
        pg *= Constants.PG_GRAVITY;
        return (Concentration / 1000f) + vg + pg;
      }
    }

  }
}
