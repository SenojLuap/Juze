﻿using System;
using System.Collections.Generic;
using System.Text;

namespace paujo.juze {
  public class RecipeItem {
    
    /// <summary>
    /// The flavor being added to the recipe.
    /// </summary>
    public Flavor Flavor {
      get; set;
    }


    /// <summary>
    /// The percentage amount of flavor in the recipe.
    /// </summary>
    public float Percentage {
      get; set; 
    }
  }
}