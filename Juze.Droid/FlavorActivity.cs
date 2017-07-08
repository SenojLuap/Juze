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
using Android.Support.V4.App;

using Xamarin.Android;

namespace paujo.juze.android {
  [Activity(Label="FlavorActivity", ParentActivity=typeof(MainActivity))]
  public class FlavorActivity : FragmentActivity {

    /// <summary>
    /// Called on the creation of the Activity.
    /// </summary>
    /// <param name="savedInstanceState">Previous state of the activity.</param>
    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      SetContentView(Resource.Layout.FlavorActivityLayout);
      FlavorListFragment listFrag = new FlavorListFragment();
      var trans = SupportFragmentManager.BeginTransaction();
      trans.Add(Resource.Id.faFragmentView, listFrag, "listFragment");
      trans.Commit();
    }

    /// <summary>
    /// Begin editing the specified flavor.
    /// </summary>
    /// <param name="toEdit">The flavor to be edited.</param>
    public void StartEditFlavor(Flavor toEdit) {
      FlavorDetailFragment fdFrag = new FlavorDetailFragment();
      fdFrag.SetFlavor(toEdit);
      var transaction = SupportFragmentManager.BeginTransaction();
      try {
        transaction.SetCustomAnimations(Resource.Animation.fromLeftAnimation, Resource.Animation.fadeOutAnimation, Resource.Animation.fadeInAnimation, Resource.Animation.toLeftAnimation);
      } catch (Exception e) {
        Console.WriteLine(e.Message);
        throw e;
      }
      transaction.Replace(Resource.Id.faFragmentView, fdFrag, "detailFragment");
      transaction.AddToBackStack("list_to_edit");
      transaction.Commit();
    }

    public void StopEditFlavor() {
      SupportFragmentManager.PopBackStackImmediate();
      var fragment = SupportFragmentManager.FindFragmentByTag("listFragment") as FlavorListFragment;
      if (fragment != null) {
        fragment.Reset();
      }
    }

    /// <summary>
    /// Called when the user presses the back button on the device.
    /// </summary>
    public override void OnBackPressed() {
      if (FragmentManager.BackStackEntryCount > 0)
        FragmentManager.PopBackStack();
      else
        base.OnBackPressed();
    }
  }
}