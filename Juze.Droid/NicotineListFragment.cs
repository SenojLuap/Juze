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
  public class NicotineListFragment : Android.Support.V4.App.ListFragment {
    public override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);

      // Create your fragment here
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
      // Use this to return your custom view for this Fragment
      // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

      return base.OnCreateView(inflater, container, savedInstanceState);
    }
  }

  public class NicotineListAdapter : BaseAdapter<Nicotine> {

    public IList<Nicotine> nicotines;

    public NicotineListFragment context;

    /// <summary>
    /// Retrieve a specific item from the collection
    /// </summary>
    /// <param name="position">The index to retrieve from.</param>
    /// <returns>The nicotine at the specified index.</returns>
    public override Nicotine this[int position] {
      get {
        return nicotines[position];
      }
    }

    /// <summary>
    /// The number of elements in the collection.
    /// </summary>
    public override int Count {
      get {
        return nicotines.Count;
      }
    }

    /// <summary>
    /// No idea.
    /// </summary>
    /// <param name="position">Even less idea.</param>
    /// <returns>Srsly....I don't know.</returns>
    public override long GetItemId(int position) {
      return position;
    }


    public override View GetView(int position, View convertView, ViewGroup parent) {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Reset the adapter after a change to the database.
    /// </summary>
    public void Reset() {
      DatabaseHelper db = new DatabaseHelper(context.Activity.ApplicationContext);
      nicotines = db.GetNicotines();
      NotifyDataSetChanged();
    }
  }
}