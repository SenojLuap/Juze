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
  public class FlavorListActivity : ListActivity {
    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      DatabaseHelper helper = new DatabaseHelper(ApplicationContext);
      IList<Flavor> flavors = helper.GetFlavors();
      ListAdapter = new FlavorListAdapter(this);
    }


    public override bool OnCreateOptionsMenu(IMenu menu) {
      MenuInflater.Inflate(Resource.Layout.FlavorListMenu, menu);
      return base.OnCreateOptionsMenu(menu);
    }

    /// <summary>
    /// Remove the flavor from the database.
    /// </summary>
    /// <param name="toRemove"></param>
    public void RemoveFlavor(Flavor toRemove) {
      Toast.MakeText(ApplicationContext, "Removed: " + toRemove.Name + " (but not really)", ToastLength.Short).Show();
    }
  }


  public class FlavorListAdapter : BaseAdapter<Flavor> {

    /// <summary>
    /// The list of flavors the adapter exposes.
    /// </summary>
    public IList<Flavor> flavors;

    /// <summary>
    /// The activity that owns the ListView.
    /// </summary>
    public FlavorListActivity context;

    /// <summary>
    /// Retreives a flavor, by index.
    /// </summary>
    /// <param name="position">The index of the flavor to retrieve.</param>
    /// <returns>The flavor for the specified index.</returns>
    public override Flavor this[int position] {
      get {
        return flavors[position];
      }
    }

    /// <summary>
    /// The number of elements in the flavor list.
    /// </summary>
    public override int Count {
      get {
        return flavors.Count;
      }
    }

    /// <summary>
    /// cTor.
    /// </summary>
    /// <param name="context">The activity that owns the ListView.</param>
    /// <param name="flavorList">The list of flavors to be represented.</param>
    public FlavorListAdapter(FlavorListActivity context) : base() {
      this.context = context;
      DatabaseHelper helper = new DatabaseHelper(context.ApplicationContext);
      this.flavors = helper.GetFlavors();
    }

    /// <summary>
    /// Gets a special identifier for the index (don't ask me what that means).
    /// </summary>
    /// <param name="position">The index of the flavor</param>
    /// <returns>Special identifier for the flavor.</returns>
    public override long GetItemId(int position) {
      return position;
    }

    /// <summary>
    /// Get a view to represent the flavor at the specified index.
    /// </summary>
    /// <param name="position">The index of the desired flavor.</param>
    /// <param name="convertView">If non-null, a View that may be recycled.</param>
    /// <param name="parent">The ListView requesting the view.</param>
    /// <returns>The new view to use for the flavor in the ListView.</returns>
    public override View GetView(int position, View convertView, ViewGroup parent) {
      View res = convertView;
      if (res == null) {
        res = context.LayoutInflater.Inflate(Resource.Layout.FlavorListRow, null);
      }
      Flavor flavor = flavors[position];
      res.FindViewById<TextView>(Resource.Id.lfrText).Text = flavor.Name;
      res.FindViewById<ImageButton>(Resource.Id.lfrRemoveBtn).Click += delegate {
        context.RemoveFlavor(flavor);
      };

      return res;
    }

  }
}