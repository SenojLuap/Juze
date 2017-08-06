using System.Collections.Generic;
using System.Text;

namespace paujo.juze {
  public class Recipe : JuzeNamedType {

    /// <summary>
    /// Percentage of VG in the recipe.
    /// </summary>
    public byte VG {
      get; set;
    }

    /// <summary>
    /// Percentage of PG in the recipe.
    /// </summary>
    public byte PG {
      get {
        return (byte)(100 - VG);
      }
      set {
        VG = (byte)(100 - value);
      }
    }

    /// <summary>
    /// Batch size, in mL.
    /// </summary>
    public int BatchSize {
      get; set;
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
    public FinalRecipe GetFinalRecipe(bool byWeight) {

      if (Nicotine.Concentration < TargetNicotine) {
        // Impossible to brew.
        return null;
      }

      FinalRecipe res = new FinalRecipe();
      res.Origin = this;

      float pgVolume = 0.0f;
      float vgVolume = 0.0f;

      res.Nicotine = TargetNicotine * BatchSize / Nicotine.Concentration;
      pgVolume += res.Nicotine * Nicotine.PG;
      vgVolume += res.Nicotine * Nicotine.VG;

      if (byWeight)
        res.Nicotine *= Nicotine.Weight;

      res.Flavors = new List<float>();
      foreach (var flavor in Flavors) {
        float flavAmount = flavor.Percentage * BatchSize;

        if (flavor.Flavor.PG)
          pgVolume += flavAmount;
        else
          vgVolume += flavAmount;

        if (byWeight)
          flavAmount *= flavor.Flavor.Weight;
        res.Flavors.Add(flavAmount);
      }

      res.PG = (BatchSize * (PG / 100f)) - pgVolume;
      res.VG = (BatchSize * (VG / 100f)) - vgVolume;

      if (byWeight) {
        res.PG *= Constants.PG_GRAVITY;
        res.VG *= Constants.VG_GRAVITY;
      }

      return res;
    }
  }
}
