using System;
using System.Collections.Generic;
using System.Text;

namespace paujo.juze {
  public class RecipeItem : JuzeNamedType {

    /// <summary>
    /// The flavor being added to the recipe.
    /// </summary>
    public Flavor Flavor {
      get; set;
    }

    /// <summary>
    /// The recipe that owns the item.
    /// </summary>
    public Recipe Owner {
      get; set;
    }

    /// <summary>
    /// The percentage amount of flavor in the recipe.
    /// </summary>
    public float Percentage {
      get; set; 
    }

    /// <summary>
    /// The name of the recipe item.
    /// </summary>
    public override string Name {
      get {
        string flavorS = "< Undecided >";
        if (Flavor != null)
          flavorS = Flavor.Name;
        return flavorS + ": " + Percentage.ToString();
      }

      set {
        base.Name = value;
      }
    }
  }
}
