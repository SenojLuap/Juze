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
using Com.Lilarcor.Cheeseknife;
using static Android.Widget.AdapterView;

namespace paujo.juze.android {
  public class RecipeDetailFragment : Android.Support.V4.App.Fragment {

    /// <summary>
    /// The recipe being edited.
    /// </summary>
    public Recipe Recipe {
      get {
        RecipeActivity act = Activity as RecipeActivity;
        if (act != null)
          return act.ActiveRecipe;
        return null;
      }
    }

    /// <summary>
    /// The list of all nicotines available.
    /// </summary>
    public IList<Nicotine> _Nicotines;
    public IList<Nicotine> Nicotines {
      get {
        if (_Nicotines == null) {
          DatabaseHelper helper = new DatabaseHelper(Activity.ApplicationContext);
          _Nicotines = helper.GetNicotines();
        }
        return _Nicotines;
      }
    }

    /// <summary>
    /// Field for name of the recipe.
    /// </summary>
    [InjectView(Resource.Id.rdNameField)]
    public EditText nameField;

    /// <summary>
    /// Drop-down for the used nicotine.
    /// </summary>
    [InjectView(Resource.Id.rdNicotineField)]
    public Spinner nicotineSpinner;

    /// <summary>
    /// Field for the target nicotine content.
    /// </summary>
    [InjectView(Resource.Id.rdTargetNicField)]
    public EditText targetNicField;

    /// <summary>
    /// Field for batch size.
    /// </summary>
    [InjectView(Resource.Id.rdBatchSizeField)]
    public EditText batchSizeField;

    /// <summary>
    /// Slider for the PG/VG ration.
    /// </summary>
    [InjectView(Resource.Id.rdPGSlider)]
    public SeekBar pgSlider;

    /// <summary>
    /// The label for the PG/VG slider.
    /// </summary>
    [InjectView(Resource.Id.rdPGLabel)]
    public TextView pgLabel;

    /// <summary>
    /// On creation of the fragment.
    /// </summary>
    /// <param name="savedInstanceState">The previous state of the fragment.</param>
    public override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
    }

    /// <summary>
    /// Create the view for the fragment.
    /// </summary>
    /// <param name="inflater">The inflater to use to inflate an XML layout.</param>
    /// <param name="container">The container of the resulting view.</param>
    /// <param name="savedInstanceState">No clue.</param>
    /// <returns>The created view.</returns>
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
      var res = inflater.Inflate(Resource.Layout.RecipeDetailLayout, container, false);
      Cheeseknife.Inject(this, res);

      nameField.Text = Recipe.Name;
      targetNicField.Text = Recipe.TargetNicotine.ToString();
      batchSizeField.Text = Recipe.BatchSize.ToString();

      ArrayAdapter<Nicotine> nicAdapter = new ArrayAdapter<Nicotine>(Activity, Android.Resource.Layout.SimpleSpinnerDropDownItem,
        Nicotines);
      nicotineSpinner.Adapter = nicAdapter;
      nicotineSpinner.ItemSelected += delegate(Object caller, ItemSelectedEventArgs args) {
        Nicotine nic = Nicotines[args.Position];
        Recipe.Nicotine = nic;
      };

      int pg = Recipe.PG;
      int vg = 100 - pg;
      pgSlider.Progress = pg;
      pgSlider.ProgressChanged += delegate {
        pgLabel.Text = "" + pgSlider.Progress + "% / " + (100 - pgSlider.Progress) + "%";
      };
      pgLabel.Text = "" + pg + "% / " + vg + "%";

      return res;
    }
  }
}