using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace ethanslist.UITests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
					.Android
                    .ApkFile("../../../ethanslist.android/bin/Release/com.xamarin.ethanslist-Signed.apk")
					.StartApp();
            }

            return ConfigureApp
				.iOS
                .AppBundle ("../../../EthansList.iOS/bin/iPhoneSimulator/Debug/ethanslist.ios.app")
				.StartApp();
        }
    }
}

