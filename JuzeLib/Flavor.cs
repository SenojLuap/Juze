using System;
using Newtonsoft.Json;

namespace paujo.juze {
  public class Flavor {


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
  }
}
