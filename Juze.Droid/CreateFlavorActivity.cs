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
  [Activity(Label = "@+string/CreateFlavor", ParentActivity = typeof(FlavorListActivity))]
  public class CreateFlavorActivity : Activity {

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

      nameField.Text = Resources.GetString(Resource.String.DefaultFlavorName);
      pgBtn.Checked = true;
      recPercField.Text = "5.0";
    }


    /// <summary>
    /// Called when the 'Create Flavor' button is clicked.
    /// </summary>
    /// <param name="caller">The button that triggered the event.</param>
    /// <param name="args">Extra arguments about the event.</param>
    [InjectOnClick(Resource.Id.cfCreateBtn)]
    public void OnCreateClick(Object caller, EventArgs args) {
      Flavor toReturn = CollectFlavorData();
      DatabaseHelper helper = new DatabaseHelper(ApplicationContext);
      helper.PutFlavor(toReturn);
      //Intent returnInfo = new Intent();
      //returnInfo.PutExtra("user_accept", true);
      SetResult(Result.Ok, null);

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