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
  [Activity(Label="NicotineActivity", ParentActivity=typeof(MainActivity))]
  public class NicotineActivity : Android.Support.V4.App.FragmentActivity {

    /// <summary>
    /// Called on creation of the activity.
    /// </summary>
    /// <param name="savedInstanceState">The saved state of the activity.</param>
    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      SetContentView(Resource.Layout.NicotineActivityLayout);

      NicotineListFragment nicFrag = new NicotineListFragment();

      var trans = SupportFragmentManager.BeginTransaction();
      trans.Add(Resource.Id.naFragmentView, nicFrag, "listFragment");
      trans.Commit();

    }

    /// <summary>
    /// Called to begin editing a nicotine.
    /// </summary>
    /// <param name="toEdit">The nicotine to edit.</param>
    public void StartEditNicotine(Nicotine toEdit) {
      NicotineDetailFragment ndFrag = new NicotineDetailFragment();
      ndFrag.SetNicotine(toEdit);

      var trans = SupportFragmentManager.BeginTransaction();
      trans.SetCustomAnimations(Resource.Animation.fromLeftAnimation, Resource.Animation.fadeOutAnimation, Resource.Animation.fadeInAnimation, Resource.Animation.toLeftAnimation);
      trans.Replace(Resource.Id.naFragmentView, ndFrag, "detailFragment");
      trans.AddToBackStack("startEditNicotine");
      trans.Commit();
    }

    /// <summary>
    /// End the nicotine editing fragment and update the UI.
    /// </summary>
    public void StopEditNicotine() {
      SupportFragmentManager.PopBackStackImmediate();
      var list = SupportFragmentManager.FindFragmentByTag("listFragment") as NicotineListFragment;
      if (list != null)
        list.Reset();
    }
  }
}