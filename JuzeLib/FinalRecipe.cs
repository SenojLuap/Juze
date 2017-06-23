using System;
using System.Collections.Generic;
using System.Text;

namespace paujo.juze {


  /// <summary>
  /// Represents an finished/finalized recipe with measured values.
  /// Values expressed as either g or mL, depending on how the result was generated.
  /// </summary>
  public class FinalRecipe {


    /// <summary>
    /// The recipe that created this result.
    /// </summary>
    public Recipe Origin {
      get; set;
    }


    /// <summary>
    /// Amount of nicotine.
    /// </summary>
    public float Nicotine {
      get; set;
    }


    /// <summary>
    /// Amount of PG.
    /// </summary>
    public float PG {
      get; set;
    }


    /// <summary>
    /// Amount of VG.
    /// </summary>
    public float VG {
      get; set;
    }


    /// <summary>
    /// Amount of flavorings. Order will pair to the flavors in the Recipe that generated this result.
    /// </summary>
    public IList<float> Flavors {
      get; set;
    }
  }
}
