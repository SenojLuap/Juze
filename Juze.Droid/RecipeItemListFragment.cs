using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace paujo.juze.android {
  public class RecipeItemListFragment : JuzeListFragment<RecipeItem> {

    /// <summary>
    /// The text to display on the 'create item' row.
    /// </summary>
    public override string CreateText {
      get {
        return Resources.GetString(Resource.String.AddFlavor);
      }
    }

    /// <summary>
    /// Should a 'create item' menu button be created.
    /// </summary>
    public override bool CreateAddMenu {
      get {
        return false;
      }
    }

    /// <summary>
    /// The recipe to list items for.
    /// </summary>
    public Recipe Recipe {
      get; set;
    }

    /// <summary>
    /// cTor.
    /// </summary>
    /// <param name="recipe">The recipe to list items for.</param>
    public RecipeItemListFragment(Recipe recipe) : base() {
      Recipe = recipe;
    }

    /// <summary>
    /// Create a new list adapter.
    /// </summary>
    /// <returns></returns>
    public override JuzeListAdapter<RecipeItem> CreateAdapter() {
      return new RecipeItemListAdapter(this);
    }

    /// <summary>
    /// Create a new recipe item.
    /// </summary>
    public override void CreateElement() {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Edit a recipe item.
    /// </summary>
    /// <param name="element"></param>
    public override void EditElement(RecipeItem element) {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Remove a recipe item from the recipe.
    /// </summary>
    /// <param name="element">The element to remove.</param>
    public override void RemoveElement(RecipeItem element) {
      Toast.MakeText(Activity.ApplicationContext, "Removed: " + element.Flavor.Name, ToastLength.Short).Show();
      Recipe.Flavors.Remove(element);
      Reset();
    }
  }
}