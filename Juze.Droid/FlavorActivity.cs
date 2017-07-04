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
  [Activity(Label="FlavorActivity", ParentActivity=typeof(MainActivity))]
  public class FlavorActivity : Activity {

    /// <summary>
    /// Called on the creation of the Activity.
    /// </summary>
    /// <param name="savedInstanceState">Previous state of the activity.</param>
    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      SetContentView(Resource.Layout.FlavorActivityLayout);
      FlavorListFragment listFrag = new FlavorListFragment();
      var trans = FragmentManager.BeginTransaction();
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
      var transaction = FragmentManager.BeginTransaction();
      transaction.Replace(Resource.Id.faFragmentView, fdFrag, "detailFragment");
      transaction.AddToBackStack("list_to_edit");
      transaction.Commit();
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