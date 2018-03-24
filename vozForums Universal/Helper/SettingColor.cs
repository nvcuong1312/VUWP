using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vozForums_Universal.Model;

namespace vozForums_Universal.Helper
{
    public class SettingColor
    {
        //public AppSetting appSetting = new AppSetting();

        public string BackgroundHomeColor { get; set; }
        public string BackgroundListViewColor { get; set; }
        public string TextblockHomeColor { get; set; }
        public string TextblockLogoHomeColor { get; set; }
        public string IconColor { get; set; }
        public string TextblockTitleThreadColor { get; set; }
        public string TextblockExtraThreadColor { get; set; }
        public string TextblockCreateUserColor { get; set; }
        public string TextblockMessageColor { get; set; }
        public string TextblockQuoteColor { get; set; }
        public string BackgroundTimeColor { get; set; }
        public string BackgroundPanelUserColor { get; set; }
        public string TextblockTimePostColor { get; set; }

        public void SetColor(string str)
        {
            if (str == "Dark")
            {
                IconColor = "#0fed88";
                TextblockHomeColor = "White";
                TextblockLogoHomeColor = "#0a64f8";
                BackgroundHomeColor = "#262628";
                BackgroundListViewColor = "Black";
                //TextblockTitleThreadColor = "White";
                TextblockExtraThreadColor = "#a2a3aa";
                TextblockCreateUserColor = "#bcccb7";
                TextblockMessageColor = "White";
                TextblockQuoteColor = "#afdbb6";
                BackgroundTimeColor = "#54634e";
                BackgroundPanelUserColor = "#7c8479";
                TextblockTimePostColor = "White";
            }
            else
            {
                IconColor = "#1414d5";
                TextblockHomeColor = "Black";
                TextblockLogoHomeColor = "Black";
                BackgroundHomeColor = "#E1E4F2";
                BackgroundListViewColor = "#E1E4F2";
                //TextblockTitleThreadColor = "White";
                TextblockExtraThreadColor = "Black";
                TextblockCreateUserColor = "#b6bad6";
                TextblockMessageColor = "Black";
                TextblockQuoteColor = "#afdbb6";
                BackgroundTimeColor = "#5C7099";
                BackgroundPanelUserColor = "#E1E4F2";
                TextblockTimePostColor = "White";

            }

        }
    }
}
