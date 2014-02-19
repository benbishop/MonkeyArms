using MonoTouch.Foundation;
using MonoTouch.NUnit.UI;
using MonoTouch.UIKit;

namespace iOSTests
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("UnitTestAppDelegate")]
    public class UnitTestAppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        private UIWindow _window;

        private TouchRunner _runner;

        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // create a new window instance based on the screen size
            _window = new UIWindow(UIScreen.MainScreen.Bounds);
            _runner = new TouchRunner(_window);

            // register every tests included in the main application/assembly
            _runner.Add(System.Reflection.Assembly.GetExecutingAssembly());

            _window.RootViewController = new UINavigationController(_runner.GetViewController());

            // make the window visible
            _window.MakeKeyAndVisible();

            return true;
        }
    }
}