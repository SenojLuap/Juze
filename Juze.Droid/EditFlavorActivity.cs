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
  [Activity(Label = "@+string/EditFlavor", ParentActivity = typeof(FlavorListActivity))]
  public class EditFlavorActivity : Activity {

    [InjectView(Resource.Id.efNameField)]
    public EditText nameField;

    [InjectView(Resource.Id.efPGBtn)]
    public CheckBox pgBtn;

    [InjectView(Resource.Id.efRecPercField)]
    public EditText recPercField;

    /// <summary>
    /// The flavor being edited.
    /// </summary>
    public Flavor flavor;

    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);

      SetContentView(Resource.Layout.EditFlavor);
      Cheeseknife.Inject(this);

      flavor = JsonConvert.DeserializeObject<Flavor>(Intent.GetStringExtra("flavor_data"));

      nameField.Text = flavor.Name;
      pgBtn.Checked = flavor.PG;
      recPercField.Text = (flavor.RecommendedPercentage * 100.0f).ToString();
    }

    /// <summary>
    /// Called when the 'Update' button is clicked.
    /// </summary>
    /// <param name="caller">The button that triggered the event.</param>
    /// <param name="args">Extra arguments about the event.</param>
    [InjectOnClick(Resource.Id.efUpdateBtn)]
    public void OnUpdateClick(Object caller, EventArgs args) {
      Flavor toUpdate = CollectFlavorData();
      DatabaseHelper helper = new DatabaseHelper(ApplicationContext);
      helper.UpdateFlavor(toUpdate);
      SetResult(Result.Ok, null);

      Finish();
    }

    /// <summary>
    /// Called to collect data from the fields to build a Flavor.
    /// </summary>
    /// <returns>A Flavor built from the fields.</returns>
    public Flavor CollectFlavorData() {
      flavor.Name = nameField.Text;
      flavor.PG = pgBtn.Checked;
      flavor.RecommendedPercentage = float.Parse(recPercField.Text) / 100f;

      return flavor;
    }

    /// <summary>
    /// Save state information when activity is cached to background.
    /// </summary>
    /// <param name="outState">Bundle to save state information to.</param>
    protected override void OnSaveInstanceState(Bundle outState) {
      outState.PutString("flavor_data", JsonConvert.SerializeObject(flavor));
      base.OnSaveInstanceState(outState);
    }

    /// <summary>
    /// Restore the state information when an activity is restored.
    /// </summary>
    /// <param name="savedInstanceState"></param>
    protected override void OnRestoreInstanceState(Bundle savedInstanceState) {
      base.OnRestoreInstanceState(savedInstanceState);
      flavor = JsonConvert.DeserializeObject<Flavor>(savedInstanceState.GetString("flavor_data"));
      nameField.Text = flavor.Name;
      pgBtn.Checked = flavor.PG;
      recPercField.Text = (flavor.RecommendedPercentage * 100.0f).ToString();
    }
  }
}