using Android.App;
using Android.Content.PM;
using Android.OS;
using FFImageLoading.Forms.Platform;
using HockeyApp.Android;
using Prism;
using Prism.Ioc;

namespace GitHubReposExplorer.Droid
{
    [Activity(Label = "GitHubReposExplorer",
              Icon = "@mipmap/ic_launcher", 
              Theme = "@style/Theme.Splash", 
              MainLauncher = true, 
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
              ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            CachedImageRenderer.Init(true);
            LoadApplication(new App(new AndroidInitializer()));
        }

        protected override void OnResume()
        {
            base.OnResume();
            CrashManager.Register(this,
                              "9eb7234e26894815aeb83b60e7bf10bf",
                              new CustomCrashListener());
        }
    }

    public class CustomCrashListener : CrashManagerListener
    {

        public override bool ShouldAutoUploadCrashes()
        {
            return true;
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            // Register any platform specific implementations
        }
    }
}

