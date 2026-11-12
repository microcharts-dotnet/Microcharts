using Foundation;
using UIKit;

namespace Microcharts.Samples.iOS
{
    [Register("SceneDelegate")]
    public class SceneDelegate : UIResponder, IUIWindowSceneDelegate
    {
        [Export("window")]
        public UIWindow Window { get; set; }

        [Export("scene:willConnectToSession:options:")]
        public void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
        {
            if (scene is UIWindowScene windowScene)
            {
                Window = new UIWindow(windowScene)
                {
                    RootViewController = StoryboardHelpers.CreateViewController<MainViewController>("Main", "MainViewController")
                };
                Window.MakeKeyAndVisible();
            }
        }
    }
}
