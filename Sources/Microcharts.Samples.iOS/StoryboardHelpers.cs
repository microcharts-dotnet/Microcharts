using UIKit;

namespace Microcharts.Samples.iOS
{
    public static class StoryboardHelpers
    {
        public static T CreateViewController<T>(string storyboardName, string viewControllerStoryBoardId = null) where T : UIViewController
        {
            var storyboard = UIStoryboard.FromName(storyboardName, null);
            T vc;

            if (string.IsNullOrEmpty(viewControllerStoryBoardId))
            {
                vc = (T) storyboard.InstantiateInitialViewController();

                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                {
                    vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
                }

                return vc;
            }

            vc = (T) storyboard.InstantiateViewController(viewControllerStoryBoardId);

            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            }

            return vc;
        }
    }
}
