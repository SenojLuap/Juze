using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace paujo.juze {
  public class Recipe {
    

    /// <summary>
    /// Unique ID for the recipe.
    /// </summary>
    public int ID {
      get; set;
    }


    /// <summary>
    /// Human-readable name of the recipe.
    /// </summary>
    public string Name {
      get; set;
    }


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
    public FinalRecipe GetFinalRecipe(float batchSize) {
      return null;
    }
  }
}
