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
using Android.Text;

namespace paujo.juze.android {
  public class SelectFlavorFragment : Android.Support.V4.App.Fragment {

    /// <summary>
    /// The view for the list of flavors.
    /// </summary>
    public ListView flavorList;

    /// <summary>
    /// The list of flavors (unfiltered).
    /// </summary>
    public IList<Flavor> Flavors {
      get; set;
    }

    /// <summary>
    /// The filtered set of flavors.
    /// </summary>
    public IList<Flavor> FilteredFlavors {
      get {
        if (FilterText == null ||
          FilterText.Length == 0)
          return Flavors;
        List<Flavor> res = new List<Flavor>();
        foreach (var flav in Flavors) {
          if (flav.Name.Contains(FilterText))
            res.Add(flav);
        }
        return res;
      }
    }

    /// <summary>
    /// The filter to apply to the flavor list.
    /// </summary>
    public string FilterText {
      get; set;
    }

    /// <summary>
    /// On creation of the fragment.
    /// </summary>
    /// <param name="savedInstanceState">The previous state of the fragment.</param>
    public override void OnCreate(Bundle savedInstanceState) {
      DatabaseHelper helper = new DatabaseHelper(Activity.ApplicationContext);
      Flavors = helper.GetFlavors();
      base.OnCreate(savedInstanceState);
    }

    /// <summary>
    /// On creation of the UI for the fragment.
    /// </summary>
    /// <param name="inflater">The inflator to expand a axml layout.</param>
    /// <param name="container">The View that should contain the UI.</param>
    /// <param name="savedInstanceState">The previous state of the fragment.</param>
    /// <returns>The View for the fragment.</returns>
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
      float dpiScale = Activity.Resources.DisplayMetrics.Density;

      LinearLayout mainLayout = new LinearLayout(container.Context);
      var mainParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent,
        LinearLayout.LayoutParams.MatchParent);
      mainLayout.LayoutParameters = mainParams;
      int padding = (int)(dpiScale * 10);
      mainLayout.SetPadding(padding, padding, padding, padding);
      mainLayout.Orientation = Orientation.Vertical;

      EditText searchField = new EditText(Activity);
      searchField.Hint = "Type to Refine List";
      searchField.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent,
        LinearLayout.LayoutParams.WrapContent);
      searchField.TextChanged += delegate(Object field, TextChangedEventArgs args) {
        EditText sField = field as EditText;
        if (field != null)
          FilterText = sField.Text;
        FlavorSelectAdapter adapter = flavorList.Adapter as FlavorSelectAdapter;
        adapter.NotifyDataSetChanged();
      };
      mainLayout.AddView(searchField);

      flavorList = new ListView(Activity);
      flavorList.Adapter = new FlavorSelectAdapter(this);
      flavorList.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent,
        LinearLayout.LayoutParams.WrapContent);
      flavorList.ItemClick += delegate (Object caller, AdapterView.ItemClickEventArgs args) {
        FlavorSelected(args.Position);
      };
      mainLayout.AddView(flavorList);

      return mainLayout;
    }

    /// <summary>
    /// React to a flavor being selected.
    /// </summary>
    /// <param name="index"></param>
    public void FlavorSelected(int index) {
      RecipeActivity act = Activity as RecipeActivity;
      if (act != null)
        act.FlavorSelected(FilteredFlavors[index]);
    }

    public class FlavorSelectAdapter : BaseAdapter<Flavor> {

      /// <summary>
      /// The fragment that 'owns' the adapter.
      /// </summary>
      public SelectFlavorFragment context;

      /// <summary>
      /// cTor.
      /// </summary>
      /// <param name="frag">The fragment that owns the list.</param>
      public FlavorSelectAdapter(SelectFlavorFragment frag) : base() {
        context = frag;
      }

      /// <summary>
      /// Retrieve the element at the specified index.
      /// </summary>
      /// <param name="position">The index.</param>
      /// <returns>The element.</returns>
      public override Flavor this[int position] {
        get {
          return context.FilteredFlavors[position];
        }
      }

      /// <summary>
      /// The number of elements in the list.
      /// </summary>
      public override int Count {
        get {
          return context.FilteredFlavors.Count;
        }
      }

      /// <summary>
      /// Get the id for an item.
      /// </summary>
      /// <param name="position">The position of the item to get an ID for.</param>
      /// <returns>The ID.</returns>
      public override long GetItemId(int position) {
        return position;
      }

      /// <summary>
      /// Get a view to represent a single item in the list.
      /// </summary>
      /// <param name="position">The index of the item to retrieve a view for.</param>
      /// <param name="convertView">A View that may be recycled.</param>
      /// <param name="parent">The owner of the new view.</param>
      /// <returns>The new View.</returns>
      public override View GetView(int position, View convertView, ViewGroup parent) {
        TextView res = convertView as TextView;
        if (res == null) {
          res = new TextView(context.Activity);
          res.SetTextAppearance(Android.Resource.Style.TextAppearanceMedium);
          var layoutParams = new ViewGroup.MarginLayoutParams(ViewGroup.LayoutParams.MatchParent,
            ViewGroup.LayoutParams.WrapContent);
          int margin = (int)(5f * context.Activity.Resources.DisplayMetrics.Density);
          res.LayoutParameters = layoutParams;
          res.SetPadding(0, margin, 0, margin);
        }
        res.Text = this[position].Name;
        return res;
      }
    }

  }
}