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
  public abstract class JuzeListFragment<T> : Android.Support.V4.App.ListFragment where T : JuzeNamedType {

    /// <summary>
    /// Called on the creation of the fragment.
    /// </summary>
    /// <param name="savedInstance">The saved state of the fragment.</param>
    public override void OnCreate(Bundle savedInstance) {
      HasOptionsMenu = true;
      base.OnCreate(savedInstance);
      ListAdapter = new JuzeListAdapter<T>(this);
    }

    /// <summary>
    /// Called when it is time to create the options menu.
    /// </summary>
    /// <param name="menu">The menu to append items to.</param>
    /// <param name="inflater">Inflator to expand the menu.</param>
    public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater) {
      base.OnCreateOptionsMenu(menu, inflater);
      inflater.Inflate(Resource.Menu.JuzeListMenu, menu);
      var btn = menu.FindItem(Resource.Id.jlCreateElement);
    }

    /// <summary>
    /// Called when an item in the menu is clicked.
    /// </summary>
    /// <param name="item">The item that was clicked.</param>
    /// <returns>Not really sure.</returns>
    public override bool OnOptionsItemSelected(IMenuItem item) {
      if (item.ItemId == Resource.Id.jlCreateElement) {
        CreateElement();
        return true;
      }
      return base.OnOptionsItemSelected(item);
    }

    /// <summary>
    /// Update an element in the database.
    /// </summary>
    /// <param name="element">The element to update.</param>
    public abstract void EditElement(JuzeBaseType element);

    /// <summary>
    /// Remove an element from the database.
    /// </summary>
    /// <param name="element">The element to remove.</param>
    public abstract void RemoveElement(JuzeBaseType element);

    /// <summary>
    /// Create a new element.
    /// </summary>
    public abstract void CreateElement();

    /// <summary>
    /// Reset the UI.
    /// </summary>
    public void Reset() {
      JuzeListAdapter<T> adapter = ListAdapter as JuzeListAdapter<T>;
      adapter.Reset();
    }
  }
}