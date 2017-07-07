﻿using System;
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
  public class NicotineDetailFragment : Android.Support.V4.App.Fragment {

    /// <summary>
    /// The nicotine name field.
    /// </summary>
    [InjectView(Resource.Id.ndNameField)]
    public EditText nameField;

    /// <summary>
    /// The nicotine PG/VG ratio slider.
    /// </summary>
    [InjectView(Resource.Id.ndPGSlider)]
    public SeekBar pgSlider;

    /// <summary>
    /// The label displaying the value of the slider.
    /// </summary>
    [InjectView(Resource.Id.ndPGValueLabel)]
    public TextView pgValueLabel;

    /// <summary>
    /// The nicotine concentration field.
    /// </summary>
    [InjectView(Resource.Id.ndConcField)]
    public EditText concField;

    /// <summary>
    /// The nicotine being edited.
    /// </summary>
    public Nicotine nicotine;

    /// <summary>
    /// Called on the creation of the fragment.
    /// </summary>
    /// <param name="savedInstanceState">The previous state of the fragment</param>
    public override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);

    }

    /// <summary>
    /// Called to create the UI for the fragment.
    /// </summary>
    /// <param name="inflater">Inflater to expand the xml to UI elements.</param>
    /// <param name="container">The ViewGroup that will contain the UI.</param>
    /// <param name="savedInstanceState">No clue.</param>
    /// <returns>The created View to be used as the UI.</returns>
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
      var res = inflater.Inflate(Resource.Layout.NicotineDetailLayout, container, false);
      Cheeseknife.Inject(this, res);

      nameField.Text = nicotine.Name;
      int pg = (int)(nicotine.PG * 100);
      int vg = 100 - pg;
      pgSlider.Progress = pg;
      pgSlider.ProgressChanged += delegate {
        pgValueLabel.Text = "" + pgSlider.Progress + "% / " + (100 - pgSlider.Progress) + "%";
      };
      pgValueLabel.Text = "" + pg + "% / " + vg + "%";
      concField.Text = nicotine.Concentration.ToString();

      return res;
    }

    /// <summary>
    /// Set the nicotine to edited/created.
    /// </summary>
    /// <param name="newNicotine"></param>
    public void SetNicotine(Nicotine newNicotine) {
      this.nicotine = newNicotine;
    }


    [InjectOnClick(Resource.Id.ndAcceptBtn)]
    public void OnAccept(object caller, EventArgs args) {
      nicotine.Name = nameField.Text;
      nicotine.PG = (pgSlider.Progress / 100);
      nicotine.Concentration = float.Parse(concField.Text);

      // TODO: Put the nicotine in the database.

      End();
    }

    public void End() {
      // TODO: End the fragment
    }
  }
}