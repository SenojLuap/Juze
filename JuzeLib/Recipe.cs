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
  }
}
