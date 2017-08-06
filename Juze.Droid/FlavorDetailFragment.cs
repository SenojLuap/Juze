using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Newtonsoft.Json;

using Com.Lilarcor.Cheeseknife;

namespace paujo.juze.android {
  public class FlavorDetailFragment : Android.Support.V4.App.Fragment {

    /// <summary>
    /// The flavor begin created/edited.
    /// </summary>
    public Flavor flavor;

    [InjectView(Resource.Id.fdNameField)]
    public TextView nameField;

    [InjectView(Resource.Id.fdPGBtn)]
    public CheckBox pgBtn;

    [InjectView(Resource.Id.fdRecPercField)]
    public TextView recPercField;

    public override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);

      // Create your fragment here
      if (savedInstanceState != null) {
        string oldFlavor = savedInstanceState.GetString("flavor_info");
        if (oldFlavor.Length > 0)
          flavor = JsonConvert.DeserializeObject<Flavor>(oldFlavor);
      }
    }

    /// <summary>
    /// Set the flavor that should be edited.
    /// </summary>
    /// <param name="toEdit"></param>
    public void SetFlavor(Flavor toEdit) {
      this.flavor = toEdit;
    }

    public override void OnPause() {
      base.OnPause();
    }

    /// <summary>
    /// Called when the fragment's UI is to be generated.
    /// </summary>
    /// <param name="inflater">Helper to expand the AXML into a View</param>
    /// <param name="container">The ViewGroup to contain the fragment's UI</param>
    /// <param name="savedInstanceState">Previous state info</param>
    /// <returns>The UI for the fragment.</returns>
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
      // Use this to return your custom view for this Fragment
      var res = inflater.Inflate(Resource.Layout.FlavorDetailLayout, container, false);
      Cheeseknife.Inject(this, res);

      nameField.Text = flavor.Name;
      nameField.Hint = "< Enter Flavor Name >";
      pgBtn.Checked = flavor.PG;
      recPercField.Text = (flavor.RecommendedPercentage * 100.0f).ToString();
      return res;
    }

    /// <summary>
    /// 'Accept' button callback.
    /// </summary>
    /// <param name="caller">The button that triggered the event</param>
    /// <param name="args">Information about the click</param>
    [InjectOnClick(Resource.Id.fdAcceptBtn)]
    public void AcceptClick(object caller, EventArgs args) {

      flavor.Name = nameField.Text;
      flavor.PG = pgBtn.Checked;
      flavor.RecommendedPercentage = float.Parse(recPercField.Text) / 100.0f;

      DatabaseHelper helper = new DatabaseHelper(Activity.ApplicationContext);
      bool update = false;
      foreach (var flavor in helper.GetFlavors())
        if (flavor.ID == this.flavor.ID) {
          update = true;
          break;
        }
      if (update)
        helper.UpdateFlavor(this.flavor);
      else
        helper.PutFlavor(this.flavor);
      End();
    }


    [InjectOnClick(Resource.Id.fdCancelBtn)]
    public void CancelClick(object caller, EventArgs args) {
      End();
    }

    /// <summary>
    /// Hide the edit/create fragment.
    /// </summary>
    public void End() {
      FlavorActivity fa = Activity as FlavorActivity;
      if (fa != null)
        fa.StopEditFlavor();
    }
  }
}