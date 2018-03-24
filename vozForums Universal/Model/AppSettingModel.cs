
namespace vozForums_Universal.Model
{
    public class AppSettingModel
    {
        Windows.Storage.ApplicationDataContainer valueCommon = Windows.Storage.ApplicationData.Current.LocalSettings;

        public string BackgroundHomeColor = string.Empty;
        public string BackgroundListViewColor = string.Empty;
        public string TextblockHomeColor = string.Empty;
        public string TextblockLogoHomeColor = string.Empty;
        public string IconColor = string.Empty;
        public string TextblockTitleThreadColor = string.Empty;
        public string TextblockExtraThreadColor = string.Empty;
        public string TextblockCreateUserColor = string.Empty;
        public string TextblockMessageColor = string.Empty;
        public string TextblockQuoteColor = string.Empty;
        public string BackgroundTimeColor = string.Empty;
        public string BackgroundPanelUserColor = string.Empty;
        public string TextblockTimePostColor = string.Empty;
        public string BackgroundThread = string.Empty;
        public AppSettingModel()
        {
            if (ThemeSetting == "Dark")
            {
                BackgroundHomeColor = "#262628";
                BackgroundListViewColor = "Black";
                TextblockHomeColor = "White";
                TextblockLogoHomeColor = "#0a64f8";
                IconColor = "#0fed88";
                TextblockExtraThreadColor = "#a2a3aa";
                TextblockCreateUserColor = "#bcccb7";
                TextblockMessageColor = "White";
                TextblockQuoteColor = "#afdbb6";
                BackgroundTimeColor = "#9aacb8";
                BackgroundPanelUserColor = "#1a1917";
                TextblockTimePostColor = "White";
                BackgroundThread = "#262628";
            }
            else
            {
                BackgroundHomeColor = "#E1E4F2";
                BackgroundListViewColor = "#E1E4F2";
                TextblockHomeColor = "Black";
                TextblockLogoHomeColor = "Black";
                IconColor = "#1414d5";
                TextblockExtraThreadColor = "9aacb8";
                TextblockCreateUserColor = "#9aacb8";
                TextblockMessageColor = "Black";
                TextblockQuoteColor = "#afdbb6";
                BackgroundTimeColor = "#5C7099";
                BackgroundPanelUserColor = "#E1E4F2";
                TextblockTimePostColor = "White";
                BackgroundThread = "White";
            }
        }
        public string DeviceName
        {
            set
            {
                valueCommon.Values["devicename"] = value;
            }
            get
            {
                try
                {
                    return valueCommon.Values["devicename"].ToString();
                }
                catch (System.Exception)
                {
                    valueCommon.Values["devicename"] = string.Empty;
                    return string.Empty;
                }                                
            }
        }
        public string UserName
        {
            get
            {
                try
                {
                    return valueCommon.Values["UserName"].ToString();
                }
                catch (System.Exception)
                {
                    valueCommon.Values["UserName"] = string.Empty;
                    return string.Empty;
                }
            }
            set
            {
                valueCommon.Values["UserName"] = value;
            }
        }
        public string PassWord
        {
            get
            {
                try
                {
                    return valueCommon.Values["PassWord"].ToString();
                }
                catch (System.Exception)
                {
                    valueCommon.Values["PassWord"] = string.Empty;
                    return string.Empty;
                }
            }
            set
            {
                valueCommon.Values["PassWord"] = value;
            }
        }
        public string token
        {
            get
            {
                try
                {
                    return valueCommon.Values["token"].ToString();
                }
                catch (System.Exception)
                {
                    valueCommon.Values["token"] = string.Empty;
                    return string.Empty;
                }
            }
            set
            {
                valueCommon.Values["token"] = value;
            }
        }
        public string UserId
        {
            get
            {
                try
                {
                    return valueCommon.Values["userId"].ToString();
                }
                catch (System.Exception)
                {
                    valueCommon.Values["userId"] = string.Empty;
                    return string.Empty;
                }
            }
            set
            {
                valueCommon.Values["userId"] = value;
            }
        }
        public string CheckPosts
        {
            get
            {
                try
                {
                    return valueCommon.Values["checkPosts"].ToString();
                }
                catch (System.Exception)
                {
                    valueCommon.Values["checkPosts"] = string.Empty;
                    return string.Empty;
                }
            }
            set
            {
                valueCommon.Values["checkPosts"] = value;
            }
        }
        public string SaveState
        {
            set
            {
                valueCommon.Values["saveState"] = value;
            }
            get
            {
                try
                {
                    string sT = valueCommon.Values["saveState"].ToString();
                    return sT;
                }
                catch (System.Exception)
                {
                    valueCommon.Values["saveState"] = string.Empty;
                    return string.Empty;
                }
            }
        }
        public string ThemeSetting
        {
            set
            {
                valueCommon.Values["themeSetting"] = value;
            }
            get
            {
                try
                {
                    string tS = valueCommon.Values["themeSetting"].ToString();
                    return tS;
                }
                catch (System.Exception)
                {
                    valueCommon.Values["themeSetting"] = string.Empty;
                    return string.Empty;
                }
            }
        }
    }
}
