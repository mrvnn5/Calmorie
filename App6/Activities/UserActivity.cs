using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using App6.Singleton;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace App6.Activities
{
    [Activity(Label = "UserActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class UserActivity : Activity
    {
        private RequestService RequestService;

        private TextView userGreeting;
        private EditText editHeight;
        private EditText editWeight;
        private TextView editAge;
        private Spinner spinnerGender;
        private Spinner spinnerActivity;
        private Spinner spinnerPlan;
        private TextView rci;
        private TextView rciValue;
        private Button changeData;
        private ImageButton quiz_button;
        private ImageButton backButton;
        private ImageButton logOutButton;
        private ProgressBar progressBar;
        public DateTime dt = DateTime.UtcNow.Date.AddYears(-12); 


        private bool isChangeMode;

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
            RequestService = RequestService.GetInstance();
            if (RequestService.Status != RequestService.StatusCode.SUCCESS)
            {
                if(!Intent.GetBooleanExtra("isSignIn", true))
                {
                    RequestService.ClearToken();
                    if (RequestService.Status == RequestService.StatusCode.SERVER_NOT_RESPONDING || RequestService.Status == RequestService.StatusCode.JSON_LOAD_ERROR)
                    {
                        StartActivity(typeof(EnterActivity));
                        Finish();
                    }
                }
                else
                {
                    StartActivity(typeof(EnterActivity));
                    Finish();
                }
            }

            DateTime currentDate = DateTime.UtcNow.Date;
            
            base.OnCreate(savedInstanceState); 
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_user);

            userGreeting = FindViewById<TextView>(Resource.Id.userGreeting);
            editHeight = FindViewById<EditText>(Resource.Id.editHeight);
            editWeight = FindViewById<EditText>(Resource.Id.editWeight);
            editAge = FindViewById<TextView>(Resource.Id.editAge);
            spinnerGender = FindViewById<Spinner>(Resource.Id.spinnerGender);
            spinnerActivity = FindViewById<Spinner>(Resource.Id.spinnerActivity);
            spinnerPlan = FindViewById<Spinner>(Resource.Id.spinnerPlan);
            rci = FindViewById<TextView>(Resource.Id.rci);
            rciValue = FindViewById<TextView>(Resource.Id.rciValue);
            changeData = FindViewById<Button>(Resource.Id.changeData);
            quiz_button = FindViewById<ImageButton>(Resource.Id.quiz_button);
            backButton = FindViewById<ImageButton>(Resource.Id.backButton);
            logOutButton = FindViewById<ImageButton>(Resource.Id.logOutButton);



            userGreeting.Text = "Привет, " + ((!Intent.GetBooleanExtra("isSignIn", true)) ? Intent.GetStringExtra("username") : RequestService.User.Username) + "!";

            if (Intent.GetBooleanExtra("isSignIn", true))
            {
                SetChageMode(false);
                backButton.Visibility = ViewStates.Visible;
                logOutButton.Visibility = ViewStates.Visible;
                editHeight.Text = RequestService.User.Height.ToString();
                editWeight.Text = RequestService.User.Weight.ToString();
                editAge.Text = RequestService.User.BirthDate.Date.ToString("dd/MM/yyyy");
                spinnerGender.SetSelection((int)RequestService.User.Sex);
                spinnerActivity.SetSelection((int)RequestService.User.Activity);
                spinnerPlan.SetSelection((int)RequestService.User.Plan);

                rciValue.Text = RequestService.GetRCI().ToString();
            }
            else
            {
                SetChageMode(true);
                rci.Visibility = ViewStates.Invisible;
                rciValue.Visibility = ViewStates.Invisible;
                backButton.Visibility = ViewStates.Invisible;
                logOutButton.Visibility = ViewStates.Invisible;
                StartActivityForResult(typeof(TestActivity), 1);
            }



       
            changeData.Click += (s, e) =>
            {
                if (editHeight.Text=="" || editWeight.Text=="" || editAge.Text=="")
                {
                    Toast.MakeText(BaseContext, "Пожалуйста, заполните все поля", ToastLength.Long).Show();
                    return;
                }

                if ((Convert.ToInt32(editHeight.Text) < 120) || (Convert.ToInt32(editHeight.Text) > 240) || (editHeight.Text == null))
                {
                    Toast.MakeText(BaseContext, "Введите рост в диапазоне от 120 до 240 сантиметров для корректной работы приложения", ToastLength.Long).Show();
                    return;
                }
                if ((Convert.ToInt32(editWeight.Text) < 35) || (Convert.ToInt32(editWeight.Text) > 250) || (editWeight.Text==null))
                {
                    Toast.MakeText(BaseContext, "Введите вес в диапазоне от 35 до 250 килограмм для корректной работы приложения", ToastLength.Long).Show();
                    return;
                }
                var AgeInput = editAge.Text;
                DateTime DateTimeAgeInput = currentDate;
                try
                {
                    DateTimeAgeInput = DateTime.ParseExact(editAge.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).Date;
                }
                catch
                {   
                    try
                    {
                        DateTimeAgeInput = DateTime.ParseExact(editAge.Text, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture).Date;
                    } catch
                    {
                        Toast.MakeText(BaseContext, "Упс! Кажется, вы некорректно ввели дату", ToastLength.Long).Show();
                        return;
                    }
                    
                }
                

                if (DateTimeAgeInput > dt)
                {
                    Toast.MakeText(BaseContext, "Упс! Кажется, вам ещё нет 12 лет. Приложение может быть опасным для вас!", ToastLength.Long).Show();
                    return;
                }
                changeData.Enabled = false;
                if (isChangeMode)
                {
                    if (!Intent.GetBooleanExtra("isSignIn", true))
                    {
                        if(!RequestService.CreateUser(Intent.GetStringExtra("username"), Intent.GetStringExtra("password"))) return;
                    }

                    RequestService.User.Height = Convert.ToInt32(editHeight.Text);
                    RequestService.User.Weight = Convert.ToInt32(editWeight.Text);
                    RequestService.User.BirthDate = DateTimeAgeInput;
                    RequestService.User.Sex = (Models.Sex)spinnerGender.SelectedItemPosition;
                    RequestService.User.Activity = (Models.Activity)spinnerActivity.SelectedItemPosition;
                    RequestService.User.Plan = (Models.Plan)spinnerPlan.SelectedItemPosition;

                    RequestService.UpdateUser();

                    rciValue.Text = RequestService.GetRCI().ToString();

                    if (Intent.GetBooleanExtra("isSignIn", true))
                    {
                        SetChageMode(false);
                    }
                    else
                    {
                        StartActivity(typeof(MainActivity));
                        Finish();
                    }
                }
                else
                {
                    SetChageMode(true);
                }
                changeData.Enabled = true;
            };

            quiz_button.Click += (sender, e) =>
            {
                StartActivityForResult(typeof(TestActivity), 1);
            };

            backButton.Click += (s, e) =>
            {
                Finish();
            };

            logOutButton.Click += (s, e) =>
            {

                Intent intentLogOut = new Intent();
                intentLogOut.PutExtra("logout", true);
                SetResult(Result.Ok, intentLogOut);
                Finish();
            };



            editAge.Click += (sender, args) =>
            {
                if (isChangeMode)
                {
                    DatePickerFragment frag = DatePickerFragment.NewInstance(currentDate, delegate (DateTime time)
                    {
                        currentDate = time.Date;
                        editAge.Text = currentDate.ToString("dd/MM/yyyy");
                    });

                    frag.Show(FragmentManager, DatePickerFragment.TAG);
                }
            };
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 1)
            {
                if (resultCode==Result.Ok)
                {
                    int activity_score = data.GetIntExtra("activity_score", 0);
                    if (activity_score != 0)
                    {
                        if (activity_score < 10) spinnerActivity.SetSelection(0);
                        else if (activity_score < 15) spinnerActivity.SetSelection(1);
                        else if (activity_score < 20) spinnerActivity.SetSelection(2);
                        else spinnerActivity.SetSelection(3);

                        if (Intent.GetBooleanExtra("isSignIn", true))
                        {
                            RequestService.User.Activity = (Models.Activity)spinnerActivity.SelectedItemPosition;
                            RequestService.UpdateUser();
                            rciValue.Text = RequestService.GetRCI().ToString();
                        }
                    }
                }
            }
        }

        public void SetChageMode(bool newChangeMode)
        {
            if(!newChangeMode)
            {
                editHeight.Clickable = false;
                editHeight.Focusable = false;
                editWeight.Clickable = false;
                editWeight.Focusable = false;
                spinnerGender.Clickable = false;
                spinnerActivity.Clickable = false;
                spinnerPlan.Clickable = false;

                editHeight.SetBackgroundResource(Resource.Drawable.RoundedBorder_EditText);
                editWeight.SetBackgroundResource(Resource.Drawable.RoundedBorder_EditText);
                editAge.SetBackgroundResource(Resource.Drawable.RoundedBorder_EditText);
                spinnerGender.SetBackgroundResource(Resource.Drawable.spinnerStyle);
                spinnerActivity.SetBackgroundResource(Resource.Drawable.spinnerStyle);
                spinnerPlan.SetBackgroundResource(Resource.Drawable.spinnerStyle);
                changeData.Text = "Изменить данные";

                isChangeMode = false;
            }
            else
            {
                editHeight.Clickable = true;
                editHeight.FocusableInTouchMode = true;
                editWeight.Clickable = true;
                editWeight.FocusableInTouchMode = true;
                spinnerGender.Clickable = true;
                spinnerActivity.Clickable = true;
                spinnerPlan.Clickable = true;

                editHeight.SetBackgroundResource(Resource.Drawable.RoundedBorder_EditTextSelected);
                editWeight.SetBackgroundResource(Resource.Drawable.RoundedBorder_EditTextSelected);
                editAge.SetBackgroundResource(Resource.Drawable.RoundedBorder_EditTextSelected);
                spinnerGender.SetBackgroundResource(Resource.Drawable.spinnerStyleSelected);
                spinnerActivity.SetBackgroundResource(Resource.Drawable.spinnerStyleSelected);
                spinnerPlan.SetBackgroundResource(Resource.Drawable.spinnerStyleSelected);
                rciValue.SetBackgroundResource(Resource.Drawable.RCI_EditText_Selected);
                changeData.Text = "Сохранить данные";

                isChangeMode = true;
            }
        }
    }
}