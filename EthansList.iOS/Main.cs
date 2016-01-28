using UIKit;

namespace ethanslist.ios
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			Xamarin.Insights.Initialize (XamarinInsights.ApiKey);
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}
