using System;
using UIKit;

namespace ethanslist.ios
{
    public static class Constants
    {
        public static nfloat CityPickerRowHeight { get { return 40f; } }
        public static string NormalFont { get { return "SanFranciscoDisplay-Regular"; } }
        public static string BoldFont { get { return "SanFranciscoDisplay-Bold"; } }
        public static string LightFont { get { return "SanFranciscoDisplay-Thin"; } }

        public static UIStringAttributes LabelAttributes { get { return att.lblAttributes; } }
        public static UIStringAttributes ButtonAttributes { get { return att.btnAttributes; } }
        public static UIStringAttributes HeaderAttributes { get { return att.headerAttributes; } }
        public static UIStringAttributes CityPickerCellAttributes { get { return att.cityPickerCellAttributes; } }
        public static UIStringAttributes FeedDescriptionAttributes { get { return att.feedDescriptionAttributes; } }
        public static int ButtonHeight { get { return 36; } }

        static StringAttributes att = new StringAttributes();
    }

    public class StringAttributes
    {
        public UIStringAttributes lblAttributes = new UIStringAttributes()
        {
            Font = UIFont.FromName(Constants.NormalFont, 18f),
        };

        public UIStringAttributes btnAttributes = new UIStringAttributes()
        {
            Font = UIFont.FromName(Constants.LightFont, 18f),
            ForegroundColor = ColorScheme.Clouds
        };

        public UIStringAttributes headerAttributes = new UIStringAttributes()
        {
            Font = UIFont.FromName(Constants.BoldFont, 16f),
        };

        public UIStringAttributes cityPickerCellAttributes = new UIStringAttributes()
        {
            Font = UIFont.FromName(Constants.LightFont, 16f),
        };

        public UIStringAttributes feedDescriptionAttributes = new UIStringAttributes()
        {
            Font = UIFont.FromName(Constants.LightFont, 14f)
        };
    }
}

