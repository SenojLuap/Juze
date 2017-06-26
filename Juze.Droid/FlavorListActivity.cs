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
  [Activity(Label = "FlavorListActivity", ParentActivity = typeof(MainActivity))]
  public class FlavorListActivity : Activity {
    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);

      // Create your application here
      SetContentView(Resource.Layout.FlavorListLayout);
    }


    public override bool OnCreateOptionsMenu(IMenu menu) {
      MenuInflater.Inflate(Resource.Layout.FlavorListMenu, menu);
      return base.OnCreateOptionsMenu(menu);
    }
  }
}