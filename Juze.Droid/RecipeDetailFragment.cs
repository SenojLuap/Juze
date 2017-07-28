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

namespace paujo.juze.android {
  public class RecipeDetailFragment : Android.Support.V4.App.Fragment {

    /// <summary>
    /// The recipe being edited.
    /// </summary>
    public Recipe recipe;

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