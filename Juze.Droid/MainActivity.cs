using Android.App;
using Android.Widget;
using Android.OS;

using System;

using Com.Lilarcor.Cheeseknife;
using Android.Content;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace paujo.juze.android {
  [Activity(Label = "@+string/JuzeName", MainLauncher = true, Icon = "@drawable/icon")]
  public partial class MainActivity : Activity {


    /// <summary>
    /// On creation of the activity.
    /// </summary>
    /// <param name="bundle">The state of the activity, if it is being restored.</param>
    protected override void OnCreate(Bundle bundle) {
      base.OnCreate(bundle);

      // Set our view from the "main" layout resource
      SetContentView(Resource.Layout.Main);

      Cheeseknife.Inject(this);

    }

    /// <summary>
    /// DEBUG: Dumps number of flavors in the database.
    /// </summary>
    /// <param name="caller"></param>
    /// <param name="args"></param>
    [InjectOnClick(Resource.Id.mDumpFlavorCountBtn)]
    public void DumpFlavorsClick(Object caller, EventArgs args) {
      IList<Flavor> flavors = GetAllFlavors();
      Toast.MakeText(ApplicationContext, "Count: " + flavors.Count, ToastLength.Short).Show();
    }

    /// <summary>
    /// Switches to 'Flavor List' activity.
    /// </summary>
    /// <param name="caller"></param>
    /// <param name="args"></param>
    [InjectOnClick(Resource.Id.mListFlavorsBtn)]
    public void StartFlavorListActivity(Object caller, EventArgs args) {
      Intent listIntent = new Intent(this, typeof(FlavorActivity));
      StartActivity(listIntent);
    }

    [InjectOnClick(Resource.Id.mDumpNicotineCountBtn)]
    public void DumpNicotienClick(Object caller, EventArgs args) {
      IList<Nicotine> nicotines = GetAllNicotines();
      Toast.MakeText(ApplicationContext, "Count: " + nicotines.Count, ToastLength.Short).Show();
    }

    /// <summary>
    /// Start the nicotine activity.
    /// </summary>
    /// <param name="caller">The button that triggered the event.</param>
    /// <param name="args">Information about the event.</param>
    [InjectOnClick(Resource.Id.mNicotineBtn)]
    public void StartNicotineActivity(Object caller, EventArgs args) {
      Intent nicIntent = new Intent(this, typeof(NicotineActivity));
      StartActivity(nicIntent);
    }

    [InjectOnClick(Resource.Id.mRecipeBtn)]
    public void StartRecipeActivity(Object caller, EventArgs args) {
      Intent recIntent = new Intent(this, typeof(RecipeActivity));
      StartActivity(recIntent);
    }
  }
}

