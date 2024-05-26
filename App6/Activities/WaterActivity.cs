using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App6.Singleton;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCoYB.Server.Models;
using static Android.Icu.Text.Transliterator;

namespace App6.Activities
{
    [Activity(Label = "WaterActivity", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class WaterActivity : Activity
    {

        private TextView dateTextView_water;
        private ImageButton back_button;
        private TextView textViewwaterget;
        private TextView textViewwaternorma;
        private ImageButton wat250add;
        private ImageButton wat250rem;
        private ImageButton wat500add;
        private ImageButton wat500rem;
        private ImageButton wat750add;
        private ImageButton wat750rem;
        private TextView mlLeftTextView;
        private TextView mlUsedTextView;

        private WaterItem currentWater;

        private RequestService RequestService;
        public DateTime currentDate { get; private set; } = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Unspecified);

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestService = RequestService.GetInstance();

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_water);
            // Create your application here
            back_button = FindViewById<ImageButton>(Resource.Id.back_button);
            dateTextView_water = FindViewById<TextView>(Resource.Id.dateTextView_water);
            textViewwaternorma = FindViewById<TextView>(Resource.Id.textViewwaternorma);
            textViewwaterget = FindViewById<TextView>(Resource.Id.textViewwaterget);
            wat250add = FindViewById<ImageButton>(Resource.Id.wat250add);
            wat250rem = FindViewById<ImageButton>(Resource.Id.wat250rem);
            wat500add = FindViewById<ImageButton>(Resource.Id.wat500add);
            wat500rem = FindViewById<ImageButton>(Resource.Id.wat500rem);
            wat750add = FindViewById<ImageButton>(Resource.Id.wat750add);
            wat750rem = FindViewById<ImageButton>(Resource.Id.wat750rem);
            mlLeftTextView = FindViewById<TextView>(Resource.Id.mlLeftTextView);
            mlUsedTextView = FindViewById<TextView>(Resource.Id.mlUsedTextView);

            try
            {
                currentWater = RequestService.User.WaterItems.Where(wi => wi.Date.Date == currentDate.Date).First();
            }
            catch (Exception ex)
            {
                currentWater = new WaterItem(new Guid(), RequestService.User.Username, 0, currentDate.Date);
                RequestService.User.WaterItems.Add(currentWater);
            }

            ChangeCurrentWater(0);


            wat250add.Click += (s, e) => ChangeCurrentWater(250);
            wat250rem.Click += (s, e) => ChangeCurrentWater(-250);
            wat500add.Click += (s, e) => ChangeCurrentWater(500);
            wat500rem.Click += (s, e) => ChangeCurrentWater(-500);
            wat750add.Click += (s, e) => ChangeCurrentWater(750);
            wat750rem.Click += (s, e) => ChangeCurrentWater(-750);


            textViewwaternorma.Text = RequestService.GetWaterNorm() + " мл.";

            back_button.Click += (s, e) =>
            {
                Finish();
            };

            dateTextView_water.Click += (s, e) =>
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(currentDate, delegate (DateTime time)
                {
                    RequestService.UpdateUser();
                    currentDate = time.Date;
                    try
                    {
                        currentWater = RequestService.User.WaterItems.Where(wi => wi.Date.Date == currentDate.Date).First();
                    }
                    catch (Exception ex)
                    {
                        currentWater = new WaterItem(new Guid(), RequestService.User.Username, 0, currentDate.Date);
                        RequestService.User.WaterItems.Add(currentWater);
                    }
                    ChangeCurrentWater(0);
                    

                    if (time.Date == DateTime.Now.Date)
                    {
                        dateTextView_water.Text = "Сегодня ▾";
                    }
                    else if (time.Date == DateTime.Now.AddDays(1).Date)
                    {
                        dateTextView_water.Text = "Завтра ▾";
                    }
                    else if (time.Date == DateTime.Now.AddDays(-1).Date)
                    {
                        dateTextView_water.Text = "Вчера ▾";
                    }
                    else
                        dateTextView_water.Text = time.ToLongDateString() + " ▾";
                });

                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };
        }

        private void ChangeCurrentWater(int val)
        {
            currentWater.Volume += val;
            textViewwaterget.Text = RequestService.User.WaterItems.Where(wi => wi.Date.Date == currentDate.Date).First().Volume + " мл.";
            mlUsedTextView.Text = currentWater.Volume + " мл.";
            mlLeftTextView.Text = (RequestService.GetWaterNorm() - currentWater.Volume) + " мл.";
        }

        public override void Finish()
        {
            RequestService.UpdateUser();
            base.Finish();
        }
    }
}