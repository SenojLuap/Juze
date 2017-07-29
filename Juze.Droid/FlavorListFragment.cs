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

using Newtonsoft.Json;
using System.Diagnostics;

namespace paujo.juze.android {
  public class FlavorListFragment : JuzeListFragment<Flavor> {

    /// <summary>
    /// The text to display on the 'create item' row.
    /// </summary>
    public override string CreateText {
      get {
        return Resources.GetString(Resource.String.CreateFlavor);
      }
    }

    /// <summary>
    /// Remove the flavor from the database.
    /// </summary>
    /// <param name="toRemove"></param>
    public override void RemoveElement(Flavor toRemove) {
      Toast.MakeText(Activity.ApplicationContext, "Removed: " + toRemove.Name, ToastLength.Short).Show();
      DatabaseHelper helper = new DatabaseHelper(Activity.ApplicationContext);
      helper.RemoveFlavor(toRemove);
      Reset();
    }

    /// <summary>
    /// Edit an existing flavor.
    /// </summary>
    /// <param name="toEdit">The flavor to edit.</param>
    public override void EditElement(Flavor toEdit) {
      FlavorActivity parent = Activity as FlavorActivity;
      if (parent != null) {
        parent.StartEditFlavor(toEdit);
      }
    }

    /// <summary>
    /// Create a new flavor.
    /// </summary>
    public override void CreateElement() {
      Flavor newFlavor = new Flavor();
      newFlavor.ID = -1;
      newFlavor.Name = GetString(Resource.String.DefaultFlavorName);
      FlavorActivity parent = Activity as FlavorActivity;
      if (parent != null)
        parent.StartEditFlavor(newFlavor);
    }
  }
}