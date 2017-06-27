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

    public const int CREATE_FLAVOR_REQUEST = 1000;

    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      DatabaseHelper helper = new DatabaseHelper(ApplicationContext);
      IList<Flavor> flavors = helper.GetFlavors();
      ListAdapter = new FlavorListAdapter(this);
    }

    /// <summary>
    /// Create the options menu.
    /// </summary>
    /// <param name="menu">The menu to append items to.</param>
    /// <returns>No idea.</returns>
    public override bool OnCreateOptionsMenu(IMenu menu) {
      MenuInflater.Inflate(Resource.Layout.FlavorListMenu, menu);
      return base.OnCreateOptionsMenu(menu);
    }

    /// <summary>
    /// Callback for user selection of menu item.
    /// </summary>
    /// <param name="item">The item that has been selected.</param>
    /// <returns>Don't know.</returns>
    public override bool OnOptionsItemSelected(IMenuItem item) {
      if (item.ItemId == Resource.Id.flCreateFlavor) {
        Intent createFlavorIntent = new Intent(this, typeof(CreateFlavorActivity));
        StartActivityForResult(createFlavorIntent, CREATE_FLAVOR_REQUEST);
      }
      return base.OnOptionsItemSelected(item);
    }


    /// <summary>
    /// Called when an intent has concluded.
    /// </summary>
    /// <param name="requestCode">The code passed when requesting info from the Intent.</param>
    /// <param name="resultCode">The result of the Activity.</param>
    /// <param name="data">The data returned from the Activity.</param>
    protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data) {
      if (requestCode == CREATE_FLAVOR_REQUEST) {
        if (data.GetBooleanExtra("user_accept", false)) {
          FlavorListAdapter ba = ListAdapter as FlavorListAdapter;
          if (ba != null) {
            ba.Reset();
          }
        }
      }
      base.OnActivityResult(requestCode, resultCode, data);
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

    /// <summary>
    /// The list of flavors has changed. Update and redraw.
    /// </summary>
    public void Reset() {
      DatabaseHelper helper = new DatabaseHelper(context.ApplicationContext);
      flavors = helper.GetFlavors();
      NotifyDataSetChanged();
    }
  }
}