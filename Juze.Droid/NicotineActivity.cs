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
    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      SetContentView(Resource.Layout.NicotineActivityLayout);

      NicotineDetailFragment nicFrag = new NicotineDetailFragment();
      nicFrag.SetNicotine(new Nicotine());

      var trans = SupportFragmentManager.BeginTransaction();
      trans.Add(Resource.Id.naFragmentView, nicFrag, "edit_nicotine");
      trans.Commit();

    }
  }
}