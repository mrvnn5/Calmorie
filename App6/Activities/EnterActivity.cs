using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using App6.Adapters;
using App6.Singleton;
using Xamarin.Essentials;

namespace App6.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme.Splash", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class EnterActivity : Activity
    {
        private RequestService RequestService;

        private bool isSignIn = true;
        private EditText username;
        private EditText password;
        private EditText passwordRepeat;
        private TextView passwordRepeatText;
        private Button signIn;
        private Button signUp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestService = RequestService.GetInstance();
            if(RequestService.Status == RequestService.StatusCode.JSON_LOAD_ERROR || RequestService.Status == RequestService.StatusCode.SERVER_NOT_RESPONDING)
            {
                Toast.MakeText(BaseContext, "Сервер сейчас недоступен\nМы уже работаем над этим", ToastLength.Long).Show();
                base.OnCreate(savedInstanceState);
                return;
            }



            if (RequestService.Status == RequestService.StatusCode.SUCCESS)
            {
                StartActivity(typeof(MainActivity));
                OverridePendingTransition(0, 0);
                Finish();
            }


            base.SetTheme(Resource.Style.AppTheme); 
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.enter_activity);

            username = FindViewById<EditText>(Resource.Id.usernameEditText);
            password = FindViewById<EditText>(Resource.Id.passEditText);
            passwordRepeat = FindViewById<EditText>(Resource.Id.repeatpassEditText);
            signIn = FindViewById<Button>(Resource.Id.signInButton);
            signUp = FindViewById<Button>(Resource.Id.signUpButton);
            passwordRepeatText = FindViewById<TextView>(Resource.Id.repeatpass);

            username.SetFilters(new IInputFilter[] { new InputFilter(InputFilter.InputType.Username) });
            password.SetFilters(new IInputFilter[] { new InputFilter(InputFilter.InputType.Username) });

            signUp.Click += (sender, args) => ChangeIsSignIn();
            signIn.Click += (sender, args) => Continue();

            //Toast.MakeText(BaseContext, result, ToastLength.Long).Show();
        }

        public void Continue()
        {
            signIn.Enabled = false;
            if (isSignIn)
            {
                if (RequestService.GetUser(username.Text, password.Text))
                {
                    StartActivity(typeof(MainActivity));
                    Finish();
                    return;
                } 
                else
                {
                    Toast.MakeText(BaseContext, "Неправильный никнейм или пароль", ToastLength.Long).Show();
                    signIn.Enabled = true;
                    return;
                }
            }

            if(password.Text != passwordRepeat.Text)
            {
                Toast.MakeText(BaseContext, "Пароли не совпадают", ToastLength.Long).Show();
                signIn.Enabled = true;
                return;
            } 
            else if (!RequestService.IsUserExists(username.Text))
            {
                //RequestService.CreateUser(username.Text, password.Text)

                Intent intentUser = new Intent(BaseContext, typeof(UserActivity));
                intentUser.PutExtra("isSignIn", isSignIn);
                intentUser.PutExtra("username", username.Text);
                intentUser.PutExtra("password", password.Text);
                StartActivity(intentUser);
                OverridePendingTransition(0, 0);
                Finish();
            }
            else Toast.MakeText(BaseContext, "Пользователь с таким никнеймом уже существует", ToastLength.Long).Show();
            signIn.Enabled = true;
        }

        public void ChangeIsSignIn()
        {
            isSignIn = !isSignIn;

            if(isSignIn)
            {
                passwordRepeatText.Visibility = ViewStates.Invisible;
                passwordRepeat.Visibility = ViewStates.Invisible;
                signIn.Text = "Вход";
                signUp.Text = "Я новый пользователь";
            } 
            else
            {
                passwordRepeatText.Visibility = ViewStates.Visible;
                passwordRepeat.Visibility = ViewStates.Visible;
                signIn.Text = "Регистрация";
                signUp.Text = "У меня уже есть аккаунт";
            }
        }
    }
}