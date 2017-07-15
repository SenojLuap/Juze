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
    /// Remove the flavor from the database.
    /// </summary>
    /// <param name="toRemove"></param>
    public override void RemoveElement(JuzeBaseType toRemove) {
      Flavor flavor = toRemove as Flavor;
      if (flavor == null)
        return;
      Toast.MakeText(Activity.ApplicationContext, "Removed: " + flavor.Name, ToastLength.Short).Show();
      DatabaseHelper helper = new DatabaseHelper(Activity.ApplicationContext);
      helper.RemoveFlavor(flavor);
      Reset();
    }

    /// <summary>
    /// Edit an existing flavor.
    /// </summary>
    /// <param name="toEdit">The flavor to edit.</param>
    public override void EditElement(JuzeBaseType toEdit) {
      Flavor flavor = toEdit as Flavor;
      if (flavor == null)
        return;
      FlavorActivity parent = Activity as FlavorActivity;
      if (parent != null) {
        parent.StartEditFlavor(flavor);
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