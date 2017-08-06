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

namespace paujo.juze.android {
  public class NicotineListFragment : JuzeListFragment<Nicotine> {

    /// <summary>
    /// The text to display on the 'create item' row.
    /// </summary>
    public override string CreateText {
      get {
        return Resources.GetString(Resource.String.CreateNicotine);
      }
    }

    /// <summary>
    /// Create a new nicotine.
    /// </summary>
    public override void CreateElement() {
      Nicotine newNic = new Nicotine();
      newNic.ID = -1;
      //newNic.Name = GetString(Resource.String.DefaultNicotineName);
      NicotineActivity na = Activity as NicotineActivity;
      if (na != null) {
        na.StartEditNicotine(newNic);
      }
    }

    /// <summary>
    /// Edit the specified nicotine.
    /// </summary>
    /// <param name="element">The nicotine to edit.</param>
    public override void EditElement(Nicotine element) {
      NicotineActivity na = Activity as NicotineActivity;
      if (na != null)
        na.StartEditNicotine(element);
    }

    /// <summary>
    /// Remove a nicotine from the list.
    /// </summary>
    /// <param name="element">The nicotine to be removed.</param>
    public override void RemoveElement(Nicotine element) {
      DatabaseHelper helper = new DatabaseHelper(Activity.ApplicationContext);
      helper.RemoveNicotine(element);
      Toast.MakeText(Activity, "Removed: " + element.Name, ToastLength.Short).Show();
      Reset();
    }
  }
}