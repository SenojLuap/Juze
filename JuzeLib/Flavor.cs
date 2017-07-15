using System;

namespace paujo.juze {
  public class Flavor : JuzeNamedType {

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
  }
}
