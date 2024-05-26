using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using App6.Adapters;
using App6.Singleton;
using Com.Ajithvgiri.Searchdialog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App6.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        public DateTime currentDate { get; private set; } = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Unspecified);

        private RequestService RequestService;
        ProductService ProductService;

        private MealRecyclerAdapter recyclerDataAdapter;
        private RecyclerView recyclerView;
        private TextView dateTextView;
        
        private RecyclerView.LayoutManager mLayoutManager;
        private ImageButton userButton;

        private TextView proteinText;
        private TextView fatText;
        private TextView carbText;
        private TextView rciText;
        private TextView calText;
        private TextView calUsedTextView;
        private TextView calLeftTextView;

        private ImageButton waterButton;

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestService = RequestService.GetInstance();
            ProductService = ProductService.getInstance();

            if (RequestService.Status != RequestService.StatusCode.SUCCESS)
            {
                StartActivity(typeof(EnterActivity));
            }

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "";

            proteinText = FindViewById<TextView>(Resource.Id.proteinText);
            fatText = FindViewById<TextView>(Resource.Id.fatText);
            carbText = FindViewById<TextView>(Resource.Id.carbText);
            rciText = FindViewById<TextView>(Resource.Id.rciText);
            calText = FindViewById<TextView>(Resource.Id.caloriesText);
            calUsedTextView = FindViewById<TextView>(Resource.Id.calUsedTextView);
            calLeftTextView = FindViewById<TextView>(Resource.Id.calLeftTextView);

            userButton = FindViewById<ImageButton>(Resource.Id.userButton);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            dateTextView = FindViewById<TextView>(Resource.Id.dateTextView);

            waterButton = FindViewById<ImageButton>(Resource.Id.waterButton);

            mLayoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(mLayoutManager);
            recyclerDataAdapter = new MealRecyclerAdapter(this);
            recyclerView.SetAdapter(recyclerDataAdapter);
            UpdateTotal();

            waterButton.Click += (s, e) =>
            {
                StartActivity(typeof(WaterActivity));
            };

            userButton.Click += (sender, e) =>
            {
                StartActivityForResult(typeof(UserActivity), 2);
            };

            dateTextView.Click += (sender, args) =>
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(currentDate, delegate (DateTime time)
                {
                    currentDate = time.Date;
                    UpdateTotal();
                    recyclerDataAdapter.NotifyDataSetChanged();
                    if (time.Date == DateTime.Now.Date) 
                    {
                        dateTextView.Text = "Сегодня ▾";
                    } else if (time.Date == DateTime.Now.AddDays(1).Date)
                    {
                        dateTextView.Text = "Завтра ▾";
                    } else if (time.Date == DateTime.Now.AddDays(-1).Date)
                    {
                        dateTextView.Text = "Вчера ▾";
                    }
                    else
                        dateTextView.Text = time.ToLongDateString() + " ▾";
                });
                
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 2)
            {
                if (resultCode == Result.Ok)
                {
                    bool logout = data.GetBooleanExtra("logout", false);
                    if (logout)
                    {
                        RequestService.LogOut();
                        StartActivity(typeof(EnterActivity));
                        Finish();
                    }
                }
            }
        }

        public void UpdateTotal()
        {
            var items = RequestService.User.FoodItems
                .Where(f => f.Date.Date == currentDate.Date).ToList();

            proteinText.Text = Math.Round(items.Sum(i => i.Product.GetProtein() * i.Weight / 100),1).ToString();
            fatText.Text = Math.Round(items.Sum(i => i.Product.GetFat() * i.Weight / 100),1).ToString();
            carbText.Text = Math.Round(items.Sum(i => i.Product.GetCarb() * i.Weight / 100),1).ToString();
            calText.Text = calUsedTextView.Text = Math.Round(items.Sum(i => i.Product.Kcal * i.Weight / 100),1).ToString();
            rciText.Text = items.Sum(i =>
                (int)Math.Round(
                (i.Product.Kcal * i.Weight / 100)
                / RequestService.GetRCI() * 100)) + "%";
            calLeftTextView.Text = Math.Round((RequestService.GetRCI() - items.Sum(i => i.Product.Kcal * i.Weight / 100)),1).ToString();
            if (Math.Round((RequestService.GetRCI() - items.Sum(i => i.Product.Kcal * i.Weight / 100)), 1) < 5)
            {
                calUsedTextView.SetTextColor(Android.Graphics.Color.DarkRed);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    [Obsolete]
    public class DatePickerFragment : DialogFragment,
                                  DatePickerDialog.IOnDateSetListener
    {
        public static DateTime currently;
        // TAG can be any string of your choice.
        public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();

        // Initialize this value to prevent NullReferenceExceptions.
        Action<DateTime> _dateSelectedHandler = delegate { };

        public static DatePickerFragment NewInstance(DateTime current, Action<DateTime> onDateSelected)
        {
            currently = current;
            DatePickerFragment frag = new DatePickerFragment();
            frag._dateSelectedHandler = onDateSelected;
            return frag;
        }

        [Obsolete]
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            
            DatePickerDialog dialog = new DatePickerDialog(Activity,
                                                           this,
                                                           currently.Year,
                                                           currently.Month - 1,
                                                           currently.Day);
            return dialog;
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            // Note: monthOfYear is a value between 0 and 11, not 1 and 12!
            DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            Log.Debug(TAG, selectedDate.ToLongDateString());
            _dateSelectedHandler(selectedDate);
        }
    }
}