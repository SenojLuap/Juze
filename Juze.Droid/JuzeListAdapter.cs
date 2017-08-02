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
using System.Diagnostics;

namespace paujo.juze.android {

  public class JuzeListAdapter<T> : BaseAdapter<JuzeNamedType> where T : JuzeNamedType {

    /// <summary>
    /// The list of elements the adapter exposes.
    /// </summary>
    public IList<T> elements;

    /// <summary>
    /// The activity that owns the ListView.
    /// </summary>
    public JuzeListFragment<T> context;

    /// <summary>
    /// Debounce timer.
    /// </summary>
    public Stopwatch clickTimer;

    /// <summary>
    /// Text to display for the 'create new item' row.
    /// </summary>
    public string CreateText {
      get;
      set;
    }

    /// <summary>
    /// Retreives a flavor, by index.
    /// </summary>
    /// <param name="position">The index of the flavor to retrieve.</param>
    /// <returns>The flavor for the specified index.</returns>
    public override JuzeNamedType this[int position] {
      get {
        return elements[position];
      }
    }

    /// <summary>
    /// The number of elements in the flavor list.
    /// </summary>
    public override int Count {
      get {
        return elements.Count + 1;
      }
    }

    /// <summary>
    /// cTor.
    /// </summary>
    /// <param name="context">The activity that owns the ListView.</param>
    /// <param name="flavorList">The list of flavors to be represented.</param>
    public JuzeListAdapter(JuzeListFragment<T> context) : base() {
      this.context = context;
      InitElements();
    }

    /// <summary>
    /// Initialize the list of elements.
    /// </summary>
    public virtual void InitElements() {
      DatabaseHelper helper = new DatabaseHelper(context.Activity.ApplicationContext);
      if (typeof(T) == typeof(Flavor))
        this.elements = helper.GetFlavors().Cast<T>().ToList();
      if (typeof(T) == typeof(Nicotine))
        this.elements = helper.GetNicotines().Cast<T>().ToList();
      if (typeof(T) == typeof(Recipe))
        this.elements = helper.GetRecipes().Cast<T>().ToList();
    }

    /// <summary>
    /// Gets a special identifier for the index (don't ask me what that means).
    /// </summary>
    /// <param name="position">The index of the flavor</param>
    /// <returns>Special identifier for the flavor.</returns>
    public override long GetItemId(int position) {
      return position;
    }

    /// <summary>
    /// Get a view to represent the flavor at the specified index.
    /// </summary>
    /// <param name="position">The index of the desired flavor.</param>
    /// <param name="convertView">If non-null, a View that may be recycled.</param>
    /// <param name="parent">The ListView requesting the view.</param>
    /// <returns>The new view to use for the flavor in the ListView.</returns>
    public override View GetView(int position, View convertView, ViewGroup parent) {
      if (position == elements.Count) {
        var create = context.Activity.LayoutInflater.Inflate(Resource.Layout.CreateRow, null);
        TextView label = create.FindViewById<TextView>(Resource.Id.crText);
        label.Text = CreateText;
        label.Click += delegate {
          context.CreateElement();
        };
        return create;
      }
      View res = convertView;
      if (res == null || res.FindViewById<Button>(Resource.Id.srText) == null) {
        res = context.Activity.LayoutInflater.Inflate(Resource.Layout.SimpleRow, null);
      }

      T element = elements[position];
      var labelBtn = res.FindViewById<Button>(Resource.Id.srText);
      labelBtn.Text = element.Name;
      labelBtn.Click += delegate {
        if (Debounce())
          context.EditElement(element);
        else
          Console.WriteLine("Bounce!");
      };
      res.FindViewById<ImageButton>(Resource.Id.srRemoveBtn).Click += delegate {
        if (Debounce())
          context.RemoveElement(element);
        else
          Console.WriteLine("Bounce!");
      };

      return res;
    }

    /// <summary>
    /// The list of flavors has changed. Update and redraw.
    /// </summary>
    public void Reset() {
      InitElements();
      NotifyDataSetChanged();
    }

    /// <summary>
    /// Prevent buttons from being triggered in rapid succession.
    /// </summary>
    /// <returns>'true' if the operation should be allowed, 'false' if called too quickly.</returns>
    public bool Debounce() {
      if (clickTimer == null || clickTimer.ElapsedMilliseconds > 1000) {
        clickTimer = Stopwatch.StartNew();
        return true;
      }
      return false;
    }
  }

}