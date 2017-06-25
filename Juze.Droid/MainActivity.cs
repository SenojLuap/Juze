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
    /// Request code for creating a new flavor.
    /// </summary>
    public const int CREATE_FLAVOR_REQUEST = 1;


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
    /// OnClick for "Create Flavor" button.
    /// </summary>
    /// <param name="caller">The button that triggered the callback.</param>
    /// <param name="args">Information about the click event.</param>
    [InjectOnClick(Resource.Id.mCreateFlavorBtn)]
    public void CreateFlavorClick(Object caller, EventArgs args) {

      //Toast.MakeText(ApplicationContext, "Clicked: " + args.GetType().ToString(), ToastLength.Short).Show();
      Intent flavIntent = new Intent(this, typeof(CreateFlavor));

      Flavor testFlav = new Flavor();
      testFlav.Name = "CAP Sweet Cream";
      testFlav.PG = true;
      testFlav.RecommendedPercentage = .05f;
      testFlav.ID = 1;

      string payload = JsonConvert.SerializeObject(testFlav);
      flavIntent.PutExtra(Constants.FLAVOR_TYPE_KEY, payload);

      StartActivityForResult(flavIntent, CREATE_FLAVOR_REQUEST);
    }

    [InjectOnClick(Resource.Id.mDumpFlavorCountBtn)]
    public void DumpFlavorsClick(Object caller, EventArgs args) {
      IList<Flavor> flavors = GetAllFlavors();
      Toast.MakeText(ApplicationContext, "Count: " + flavors.Count, ToastLength.Short).Show();
    }

    /// <summary>
    /// Called when an activity returns with a result.
    /// </summary>
    /// <param name="requestCode">The code passed to the activity on start.</param>
    /// <param name="resultCode">The type of result returned.</param>
    /// <param name="data">The result payload.</param>
    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
      if (requestCode == CREATE_FLAVOR_REQUEST) {
        if (resultCode == Result.Ok) {
          Flavor resultFlavor = JsonConvert.DeserializeObject<Flavor>(data.GetStringExtra(Constants.FLAVOR_TYPE_KEY));
          var rand = new Random();
          resultFlavor.ID = rand.Next();
          Toast.MakeText(ApplicationContext, "Created: " + resultFlavor.Name + " (" + (resultFlavor.PG ? "PG)" : "VG)") + " %" + (resultFlavor.RecommendedPercentage * 100f), ToastLength.Short).Show();
          PutFlavor(resultFlavor);
        }
      }
    }
  }
}

