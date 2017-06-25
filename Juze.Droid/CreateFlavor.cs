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

using Java.Text;
using Java.Util;

using Com.Lilarcor.Cheeseknife;
using Newtonsoft.Json;

namespace paujo.juze.android {
  [Activity(Label = "@+string/CreateFlavor", ParentActivity = typeof(MainActivity))]
  public class CreateFlavor : Activity {

    [InjectView(Resource.Id.cfNameField)]
    public EditText nameField;

    [InjectView(Resource.Id.cfPGBtn)]
    public CheckBox pgBtn;

    [InjectView(Resource.Id.cfRecPercField)]
    public EditText recPercField;

    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);

      SetContentView(Resource.Layout.CreateFlavor);
      Cheeseknife.Inject(this);

      Flavor flav = JsonConvert.DeserializeObject<Flavor>(Intent.GetStringExtra(Constants.FLAVOR_TYPE_KEY));
      nameField.Text = flav.Name;
      pgBtn.Checked = flav.PG;
      recPercField.Text = (flav.RecommendedPercentage * 100f).ToString();
    }


    /// <summary>
    /// Called when the 'Create Flavor' button is clicked.
    /// </summary>
    /// <param name="caller">The button that triggered the event.</param>
    /// <param name="args">Extra arguments about the event.</param>
    [InjectOnClick(Resource.Id.cfCreateBtn)]
    public void OnCreateClick(Object caller, EventArgs args) {
      Intent resultIntent = new Intent();
      Flavor toReturn = CollectFlavorData();
      string payload = JsonConvert.SerializeObject(toReturn);
      resultIntent.PutExtra(Constants.FLAVOR_TYPE_KEY, payload);
      SetResult(Result.Ok, resultIntent);
      Finish();
    }


    /// <summary>
    /// Called to collect data from the fields to build a Flavor.
    /// </summary>
    /// <returns>A Flavor built from the fields.</returns>
    public Flavor CollectFlavorData() {
      Flavor res = new Flavor();
      res.Name = nameField.Text;
      res.PG = pgBtn.Checked;
      res.RecommendedPercentage = float.Parse(recPercField.Text) / 100f;

      return res;
    }
  }
}