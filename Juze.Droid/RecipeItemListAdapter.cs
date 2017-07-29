using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace paujo.juze.android {
  public class RecipeItemListAdapter : JuzeListAdapter<RecipeItem> {

    public RecipeItemListAdapter(JuzeListFragment<RecipeItem> context) : base(context) {
      CreateText = context.Resources.GetString(Resource.String.AddFlavor);
    }

    /// <summary>
    /// Initialize the list of elements.
    /// </summary>
    public override void InitElements() {
      RecipeItemListFragment frag = context as RecipeItemListFragment;
      elements = frag.Recipe.Flavors;
    }
  }
}