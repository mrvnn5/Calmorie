using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.AppCompat.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App6.Activities
{
    [Activity(Label = "TestActivity", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class TestActivity : Activity
    {
        private RadioButton radiofirst1;
        private RadioButton radiofirst2;
        private RadioButton radiofirst3;
        private RadioButton radiofirst4;
        private RadioButton radiosec1;
        private RadioButton radiosec2;
        private RadioButton radiosec3;
        private RadioButton radiosec4;
        private RadioButton radiothird1;
        private RadioButton radiothird2;
        private RadioButton radiothird3;
        private RadioButton radiothird4;
        private RadioButton radiofour1;
        private RadioButton radiofour2;
        private RadioButton radiofour3;
        private RadioButton radiofour4;
        private RadioButton radiofive1;
        private RadioButton radiofive2;
        private RadioButton radiofive3;
        private RadioButton radiofive4;
        private Button testButton;
        private ImageButton back_button;
        private int result = 0;

        private int[] vals = { 1, 1, 1, 1, 1 };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.test_activity);
            // Create your application here

            radiofirst1 = FindViewById<RadioButton>(Resource.Id.radiofirst1);
            radiofirst2 = FindViewById<RadioButton>(Resource.Id.radiofirst2);
            radiofirst3 = FindViewById<RadioButton>(Resource.Id.radiofirst3);
            radiofirst4 = FindViewById<RadioButton>(Resource.Id.radiofirst4);
            radiosec1 = FindViewById<RadioButton>(Resource.Id.radiosec1);
            radiosec2 = FindViewById<RadioButton>(Resource.Id.radiosec2);
            radiosec3 = FindViewById<RadioButton>(Resource.Id.radiosec3);
            radiosec4 = FindViewById<RadioButton>(Resource.Id.radiosec4);
            radiothird1 = FindViewById<RadioButton>(Resource.Id.radiothird1);
            radiothird2 = FindViewById<RadioButton>(Resource.Id.radiothird2);
            radiothird3 = FindViewById<RadioButton>(Resource.Id.radiothird3);
            radiothird4 = FindViewById<RadioButton>(Resource.Id.radiothird4);
            radiofour1 = FindViewById<RadioButton>(Resource.Id.radiofour1);
            radiofour2 = FindViewById<RadioButton>(Resource.Id.radiofour2);
            radiofour3 = FindViewById<RadioButton>(Resource.Id.radiofour3);
            radiofour4 = FindViewById<RadioButton>(Resource.Id.radiofour4);
            radiofive1 = FindViewById<RadioButton>(Resource.Id.radiofive1);
            radiofive2 = FindViewById<RadioButton>(Resource.Id.radiofive2);
            radiofive3 = FindViewById<RadioButton>(Resource.Id.radiofive3);
            radiofive4 = FindViewById<RadioButton>(Resource.Id.radiofive4);
            testButton = FindViewById<Button>(Resource.Id.testButton);
            back_button = FindViewById<ImageButton>(Resource.Id.back_button);

            radiofirst1.Click += (s, e) => vals[0] = 1;
            radiofirst2.Click += (s, e) => vals[0] = 2;
            radiofirst3.Click += (s, e) => vals[0] = 3;
            radiofirst4.Click += (s, e) => vals[0] = 4;
            radiosec1.Click += (s, e) => vals[1] = 1;
            radiosec2.Click += (s, e) => vals[1] = 2;
            radiosec3.Click += (s, e) => vals[1] = 3;
            radiosec4.Click += (s, e) => vals[1] = 4;
            radiothird1.Click += (s, e) => vals[2] = 1;
            radiothird2.Click += (s, e) => vals[2] = 2;
            radiothird3.Click += (s, e) => vals[2] = 3;
            radiothird4.Click += (s, e) => vals[2] = 4;
            radiofour1.Click += (s, e) => vals[3] = 1;
            radiofour2.Click += (s, e) => vals[3] = 2;
            radiofour3.Click += (s, e) => vals[3] = 3;
            radiofour4.Click += (s, e) => vals[3] = 4;
            radiofive1.Click += (s, e) => vals[4] = 1;
            radiofive2.Click += (s, e) => vals[4] = 2;
            radiofive3.Click += (s, e) => vals[4] = 3;
            radiofive4.Click += (s, e) => vals[4] = 4;
            
            testButton.Click += (s, e) =>
            {
                for (int i = 0; i < 5; i++)
                {
                    result += vals[i];
                }
                Intent intentUser = new Intent();
                intentUser.PutExtra("activity_score", result);
                SetResult(Result.Ok, intentUser);
                Finish();
            };

            back_button.Click += (s, e) => Finish();

        }
    }
}