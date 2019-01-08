
namespace vozForums_Universal.Model
{
    public class AppSettingModel
    {
        Windows.Storage.ApplicationDataContainer valueCommon;

        public AppSettingModel()
        {
            valueCommon = Windows.Storage.ApplicationData.Current.LocalSettings;

            _isDark = ThemeSetting == "Dark" ? true : false;
            //if (ThemeSetting == "Dark")
            //{
            //    //IconColor = "#0fed88";
            //    //TextblockExtraThreadColor = "#a2a3aa";
            //    //TextblockCreateUserColor = "#bcccb7";
            //    //TextblockMessageColor = "White";
            //    //TextblockQuoteColor = "#afdbb6";
            //    //BackgroundTimeColor = "#9aacb8";
            //    //BackgroundPanelUserColor = "#1a1917";
            //    //TextblockTimePostColor = "White";
            //    //BackgroundThread = "#262628";
            //    _isDark = true;
            //}
            //else
            //{
            //    //IconColor = "#1414d5";
            //    //TextblockExtraThreadColor = "9aacb8";
            //    //TextblockCreateUserColor = "#9aacb8";
            //    //TextblockMessageColor = "Black";
            //    //TextblockQuoteColor = "#afdbb6";
            //    //BackgroundTimeColor = "#5C7099";
            //    //BackgroundPanelUserColor = "#E1E4F2";
            //    //TextblockTimePostColor = "White";
            //    //BackgroundThread = "White";
            //    _isDark = false;
            //}
        }

        private bool _isDark;

        // <a href> for Listthread.
        public string ListThreadATag
        {
            get
            {
                return _isDark ? "#E1E2FB" : "#23497C";
            }
        }

        public string ListThreadATagHover
        {
            get
            {
                return _isDark ? "#cfcfcf" : "#ff0808";
            }
        }

        public string ThreadRowHoverBackGroundColor
        {
            get
            {
                return _isDark ? "#3f3e3e" : "#ebebeb";
            }
        }

        public string ForumBoxRowThreadRowBorder
        {
            get
            {
                return _isDark ? "#4e4b4b" : "#d8d8d8";
            }
        }

        public string BodyListThreadBackGround
        {
            get
            {
                return _isDark ? "#262628" : "#fff";
            }
        }

        public string BodyListThreadColor
        {
            get
            {
                return _isDark ? "#afb9b0" : "#454545";
            }
        }

        // For <a href>
        public string LinkHref
        {
            get
            {
                return _isDark ? "#4db2ff" : "#23497C";
            }
        }

        public string ColorBody
        {
            get
            {
                return _isDark ? "#E3E3E3" : "#201F25";
            }
        }

        public string ColorBackground
        {
            get
            {
                return _isDark ? "#262628" : "#F2F2F2";
            }
        }
        public string ColorBackgroundTD
        {
            get
            {
                return _isDark ? "#171717" : "#E1E4F2";
            }
        }

        public string BackgroundHomeColor
        {
            get
            {
                return _isDark ? "#262628" : "#E1E4F2";
            }
        }

        public string BackgroundListViewColor
        {
            get
            {
                return _isDark ? "#000" : "#E1E4F2";
            }
        }

        public string TextblockHomeColor
        {
            get
            {
                return _isDark ? "#fff" : "#000";
            }
        }

        public string TextblockLogoHomeColor
        {
            get
            {
                return _isDark ? "#0a64f8" : "#000";
            }
        }

        public string IconColor
        {
            get
            {
                return _isDark ? "#0fed88" : "#1414d5";
            }
        }
        public string TextblockTitleThreadColor
        {
            get
            {
                return _isDark ? "#3f3e3e" : "#ebebeb";
            }
        }
        public string TextblockExtraThreadColor
        {
            get
            {
                return _isDark ? "#a2a3aa" : "#9aacb8";
            }
        }
        public string TextblockCreateUserColor
        {
            get
            {
                return _isDark ? "#bcccb7" : "#9aacb8";
            }
        }
        public string TextblockMessageColor
        {
            get
            {
                return _isDark ? "#fff" : "#000";
            }
        }
        public string TextblockQuoteColor
        {
            get
            {
                return _isDark ? "#afdbb6" : "#afdbb6";
            }
        }
        public string BackgroundTimeColor
        {
            get
            {
                return _isDark ? "#9aacb8" : "#5C7099";
            }
        }
        public string BackgroundPanelUserColor
        {
            get
            {
                return _isDark ? "#1a1917" : "#E1E4F2";
            }
        }
        public string TextblockTimePostColor
        {
            get
            {
                return _isDark ? "#fff" : "#fff";
            }
        }
        public string BackgroundThread
        {
            get
            {
                return _isDark ? "#262628" : "#fff";
            }
        }

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
