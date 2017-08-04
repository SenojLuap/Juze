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
  [Activity(Label="RecipeActivity", ParentActivity=typeof(MainActivity))]
  public class RecipeActivity : Android.Support.V4.App.FragmentActivity {

    /// <summary>
    /// The recipe currently being edited.
    /// </summary>
    public Recipe ActiveRecipe {
      get; set;
    }

    /// <summary>
    /// On creation of the activity.
    /// </summary>
    /// <param name="savedInstanceState">The previous state of the activity.</param>
    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      SetContentView(Resource.Layout.RecipeActivityLayout);

      RecipeDetailFragment recFrag = new RecipeDetailFragment();

      Recipe testRec = new Recipe();
      testRec.Name = "Test Recipe";
      testRec.PG = 70;
      testRec.TargetNicotine = 4;
      testRec.Flavors = new List<RecipeItem>();
      ActiveRecipe = testRec;

      RecipeItemListFragment recListFrag = new RecipeItemListFragment();

      var trans = SupportFragmentManager.BeginTransaction();
      trans.Add(Resource.Id.raFragmentView, recFrag, "detailFragment");
      trans.Add(Resource.Id.raFragmentView, recListFrag, "listFragment");
      trans.Commit();
    }

    /// <summary>
    /// Allow the user to select a new flavor for the recipe.
    /// </summary>
    public void SelectFlavor() {
      var trans = SupportFragmentManager.BeginTransaction();
      trans.Remove(SupportFragmentManager.FindFragmentByTag("detailFragment"));
      trans.Remove(SupportFragmentManager.FindFragmentByTag("listFragment"));
      SelectFlavorFragment frag = new SelectFlavorFragment();
      trans.Add(Resource.Id.raFragmentView, frag, "selectFlavor");
      trans.AddToBackStack("addNewFlavor");
      trans.Commit();
    }

    /// <summary>
    /// React to the 'select new flavor' action ending.
    /// </summary>
    /// <param name="newFlavor">The flavor that was selected. May be null.</param>
    public void FlavorSelected(Flavor newFlavor) {
      if (newFlavor != null) {
        RecipeItem newItem = new RecipeItem();
        newItem.Flavor = newFlavor;
        newItem.Percentage = newFlavor.RecommendedPercentage;
        //DatabaseHelper helper = new DatabaseHelper(ApplicationContext);
        //helper.PutRecipeItem(newItem, ActiveRecipe);
        ActiveRecipe.Flavors.Add(newItem);
      }
      SupportFragmentManager.PopBackStackImmediate();
      var list = SupportFragmentManager.FindFragmentByTag("listFragment") as RecipeItemListFragment;
      if (list != null)
        list.Reset();
    }
  }
}