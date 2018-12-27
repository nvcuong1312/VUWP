
namespace vozForums_Universal.Model
{
    public class AppSettingModel
    {
        Windows.Storage.ApplicationDataContainer valueCommon;

        public AppSettingModel()
        {
            valueCommon = Windows.Storage.ApplicationData.Current.LocalSettings;

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

                LinkHref = "#4db2ff";
                ColorBody = "#E3E3E3";
                ColorBackground = "#262628";
                ColorBackgroundTD = "#171717";
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

                LinkHref = "#23497C";
                ColorBody = "#201F25";
                ColorBackground = "#F2F2F2";
                ColorBackgroundTD = "#E1E4F2";
            }
        }

        // For <a href>
        public string LinkHref { get; set; }
        public string ColorBody { get; set; }
        public string ColorBackground { get; set; }
        public string ColorBackgroundTD { get; set; }
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
        public string BackgroundThread { get; set; }

        public string DeviceName
        {
            get
            {
                try
                {
                    return valueCommon.Values[Resource.STR_DEVICE_NAME].ToString();
                }
                catch (System.Exception)
                {
                    valueCommon.Values[Resource.STR_DEVICE_NAME] = Resource.STR_DEFAULT_NAME_DEVICE;
                    return Resource.STR_DEFAULT_NAME_DEVICE;
                }
            }
            set
            {
                valueCommon.Values[Resource.STR_DEVICE_NAME] = value;
            }
        }

        public string UserName
        {
            get
            {
                try
                {
                    return valueCommon.Values[Resource.STR_USER_NAME].ToString();
                }
                catch (System.Exception)
                {
                    valueCommon.Values[Resource.STR_USER_NAME] = Resource.STR_EMPTY;
                    return Resource.STR_EMPTY;
                }
            }
            set
            {
                valueCommon.Values[Resource.STR_USER_NAME] = value;
            }
        }

        public string Password
        {
            get
            {
                try
                {
                    return valueCommon.Values[Resource.STR_PASSWORD].ToString();
                }
                catch (System.Exception)
                {
                    valueCommon.Values[Resource.STR_PASSWORD] = Resource.STR_EMPTY;
                    return Resource.STR_EMPTY;
                }
            }
            set
            {
                valueCommon.Values[Resource.STR_PASSWORD] = value;
            }
        }

        public string Token
        {
            get
            {
                try
                {
                    return valueCommon.Values[Resource.STR_TOKEN].ToString();
                }
                catch (System.Exception)
                {
                    valueCommon.Values[Resource.STR_TOKEN] = Resource.STR_EMPTY;
                    return Resource.STR_EMPTY;
                }
            }
            set
            {
                valueCommon.Values[Resource.STR_TOKEN] = value;
            }
        }

        public string UserId
        {
            get
            {
                try
                {
                    return valueCommon.Values[Resource.STR_USER_ID].ToString();
                }
                catch (System.Exception)
                {
                    valueCommon.Values[Resource.STR_USER_ID] = Resource.STR_EMPTY;
                    return Resource.STR_EMPTY;
                }
            }
            set
            {
                valueCommon.Values[Resource.STR_USER_ID] = value;
            }
        }

        public bool IsEnablePostLink
        {
            get
            {
                try
                {
                    return (valueCommon.Values[Resource.STR_IS_ENABLE_POST_LINK].ToString() == Resource.TRUE) ? true : false;
                }
                catch (System.Exception)
                {
                    valueCommon.Values[Resource.STR_IS_ENABLE_POST_LINK] = Resource.FALSE;
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    valueCommon.Values[Resource.STR_IS_ENABLE_POST_LINK] = Resource.TRUE;
                }
                else
                {
                    valueCommon.Values[Resource.STR_IS_ENABLE_POST_LINK] = Resource.FALSE;
                }
            }
        }

        public int TotalPosts
        {
            get
            {
                try
                {
                    return int.Parse(valueCommon.Values[Resource.STR_TOTAL_POSTS].ToString());
                }
                catch (System.Exception)
                {
                    valueCommon.Values[Resource.STR_TOTAL_POSTS] = 0;
                    return 0;
                }
            }
            set
            {
                valueCommon.Values[Resource.STR_TOTAL_POSTS] = value;
            }
        }

        public bool isSaveLogin
        {

            get
            {
                try
                {
                    return (valueCommon.Values[Resource.STR_SAVE_STATE].ToString() == Resource.TRUE) ? true : false;
                }
                catch (System.Exception)
                {
                    valueCommon.Values[Resource.STR_SAVE_STATE] = Resource.FALSE;
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    valueCommon.Values[Resource.STR_SAVE_STATE] = Resource.TRUE;
                }
                else
                {
                    valueCommon.Values[Resource.STR_SAVE_STATE] = Resource.FALSE;
                }
            }
        }

        public string ThemeSetting
        {
            set
            {
                valueCommon.Values[Resource.STR_THEME_SETTING] = value;
            }
            get
            {
                try
                {
                    return valueCommon.Values[Resource.STR_THEME_SETTING].ToString();
                }
                catch (System.Exception)
                {
                    valueCommon.Values[Resource.STR_THEME_SETTING] = Resource.STR_EMPTY;
                    return Resource.STR_EMPTY;
                }
            }
        }

        public string Cookies_Vfpassword
        {
            get
            {
                try
                {
                    return valueCommon.Values[Resource.COOKIES_VFPASSWORD].ToString();
                }
                catch (System.Exception)
                {
                    valueCommon.Values[Resource.COOKIES_VFPASSWORD] = Resource.STR_EMPTY;
                    return Resource.STR_EMPTY;
                }
            }
            set
            {
                valueCommon.Values[Resource.COOKIES_VFPASSWORD] = value;
            }
        }

        public string Cookies_Vfuserid
        {
            get
            {
                try
                {
                    return valueCommon.Values[Resource.COOKIES_VFUSERID].ToString();
                }
                catch (System.Exception)
                {
                    valueCommon.Values[Resource.COOKIES_VFUSERID] = Resource.STR_EMPTY;
                    return Resource.STR_EMPTY;
                }
            }
            set
            {
                valueCommon.Values[Resource.COOKIES_VFUSERID] = value;
            }
        }

        public string Cookies_Vbmultiquote
        {
            get
            {
                try
                {
                    return valueCommon.Values[Resource.COOKIES_VBULLETIN_MULTIQUOTE].ToString();
                }
                catch (System.Exception)
                {
                    valueCommon.Values[Resource.COOKIES_VBULLETIN_MULTIQUOTE] = Resource.STR_EMPTY;
                    return Resource.STR_EMPTY;
                }
            }
            set
            {
                valueCommon.Values[Resource.COOKIES_VBULLETIN_MULTIQUOTE] = value;
            }
        }

        public int BoxStart
        {
            get
            {
                try
                {
                    return int.Parse(valueCommon.Values[Resource.STR_BOX_START].ToString());
                }
                catch (System.Exception)
                {
                    valueCommon.Values[Resource.STR_BOX_START] = Resource.ID_BOX_START_DEFAULT;
                    return Resource.ID_BOX_START_DEFAULT;
                }
            }
            set
            {
                valueCommon.Values[Resource.STR_BOX_START] = value;
            }
        }
    }
}
