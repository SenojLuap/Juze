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
    public Recipe recipe;

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

    [InjectView(Resource.Id.rdNameField)]
    public EditText nameField;

    [InjectView(Resource.Id.rdNicotineField)]
    public Spinner nicotineSpinner;

    [InjectView(Resource.Id.rdTargetNicField)]
    public EditText targetNicField;

    [InjectView(Resource.Id.rdPGSlider)]
    public SeekBar pgSlider;

    [InjectView(Resource.Id.rdPGLabel)]
    public TextView pgLabel;

    public override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
      var res = inflater.Inflate(Resource.Layout.RecipeDetailLayout, container, false);
      Cheeseknife.Inject(this, res);

      nameField.Text = recipe.Name;
      targetNicField.Text = recipe.TargetNicotine.ToString();
      targetNicField.InputType = Android.Text.InputTypes.ClassNumber;

      ArrayAdapter<Nicotine> nicAdapter = new ArrayAdapter<Nicotine>(Activity, Android.Resource.Layout.SimpleSpinnerDropDownItem,
        Nicotines);
      nicotineSpinner.Adapter = nicAdapter;
      nicotineSpinner.ItemSelected += delegate(Object caller, ItemSelectedEventArgs args) {
        Nicotine nic = Nicotines[args.Position];
        recipe.Nicotine = nic;
      };

      int pg = recipe.PG;
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