using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using vozForums_Universal.Model;
using Windows.UI.Popups;
using vozForums_Universal.Views;
using System.Reflection;
using vozForums_Universal.CommonControl;
using Windows.UI.Xaml.Media.Animation;
using vozForums_Universal.Helper;
using MyToolkit.Paging;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Account
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class SettingView : MtPage
    {
        AccountHelper myLogin;
        AppSettingModel appSetting;

        public SettingView()
        {
            this.InitializeComponent();

            myLogin = new AccountHelper();
            appSetting = new AppSettingModel();
        }

        protected override void OnNavigatedTo(MtNavigationEventArgs e)
        {
            string theme = appSetting.ThemeSetting;
            if (theme == "Dark")
            {
                DarkTheme.IsChecked = true;
                LightTheme.IsChecked = false;
            }
            else
            {
                DarkTheme.IsChecked = false;
                LightTheme.IsChecked = true;
            }

            if (appSetting.UserName != null && appSetting.Password != null)
            {
                txtUSERNAME.Text = appSetting.UserName;
                txtPASSWORD.Password = appSetting.Password;
            }

            if (appSetting.Token == null) btn_logout.IsEnabled = false;

            if (appSetting.DeviceName != null)
            {
                tbDeviceName.Text = appSetting.DeviceName;
            }

            MainView.GetInstance().UpdatePosSelectedListView(Resource.ID_SETTING.ToString());
        }       

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            bool save;
            txtUSERNAME.IsEnabled = false;
            txtPASSWORD.IsEnabled = false;

            if (isSave.IsChecked == true)
            {
                save = true;
            }
            else save = false;

            myLogin.LoginMethod(txtUSERNAME.Text, txtPASSWORD.Password, save);
            try
            {
                if (appSetting.Token.Length > 10)
                {
                    DialogResult.DialogOnlyOk(Resource.STR_LOGIN_SUCCESS);
                    btn_logout.IsEnabled = true;
                   
                    MainView.GetInstance().UpdateNameAndStatusBtnAccount(txtUSERNAME.Text);
                }
            }
            catch (Exception ex)
            {
                DialogResult.DialogOnlyOk(Resource.STR_ERROR_LOGIN);
            }
            txtUSERNAME.IsEnabled = true;
            txtPASSWORD.IsEnabled = true;
        }

        private void SaveTheme_Click(object sender, RoutedEventArgs e)
        {
            if (DarkTheme.IsChecked == true)
            {
                DialogResult.AskToChangeThemeApp(true);
            }
            else
            {
                DialogResult.AskToChangeThemeApp(false);
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string device = tbDeviceName.Text;
            if (device.Length < 4)
            {
                DialogResult.DialogOnlyOk("Min Length = 4");
            }
            else
            {
                appSetting.DeviceName = device;
                DialogResult.DialogOnlyOk("Done");
            }
        }

        private void btn_logout_Click(object sender, RoutedEventArgs e)
        {
            appSetting.Password = Resource.STR_EMPTY;
            appSetting.UserName = Resource.STR_EMPTY;
            appSetting.Token = Resource.STR_EMPTY;
            appSetting.Cookies_Vfpassword = Resource.STR_EMPTY;
            appSetting.Cookies_Vfuserid = Resource.STR_EMPTY;
            appSetting.isSaveLogin = false;
            DialogResult.DialogOnlyOk("Đăng xuất thành công");
            txtUSERNAME.Text = Resource.STR_EMPTY;
            txtPASSWORD.Password = Resource.STR_EMPTY;
            btn_logout.IsEnabled = false;
            MainView.GetInstance().UpdateNameAndStatusBtnAccount("Khách");
            Frame.NavigateAsync(typeof(ListThreadView), 26);
            Frame.ClearBackStack();
        }

        private void txtUSERNAME_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUSERNAME.Text))
            {
                btn_login.IsEnabled = false;
            }
            else
            {
                btn_login.IsEnabled = true;
            }
        }     
    }
}
