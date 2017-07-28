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
    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      SetContentView(Resource.Layout.RecipeActivityLayout);

      RecipeDetailFragment recFrag = new RecipeDetailFragment();

      Recipe testRec = new Recipe();
      testRec.Name = "Test Recipe";
      testRec.PG = 70;
      testRec.TargetNicotine = 4;

      recFrag.recipe = testRec;

      var trans = SupportFragmentManager.BeginTransaction();
      trans.Add(Resource.Id.raFragmentView, recFrag, "detailFragment");
      trans.Commit();
    }
  }
}