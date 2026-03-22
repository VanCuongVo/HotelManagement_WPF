using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Service.UserService;

namespace PRN212_Assignment_01
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private AccountService accountService;
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginCustomer_Click(object sender, RoutedEventArgs e)
        {
            accountService = new();
            string email = txtemail.Text;
            string password = txtPassword.Password;
            var p = accountService.Login(email, password);
            if (p.Isuccess == false)
            {
                MessageBox.Show("Login in errors", "Please try login try again", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                CustomerHomeWindow cusrtomerhome = new CustomerHomeWindow(p.Value);
                this.Hide();
                cusrtomerhome.Show();
            }

        }

        private void LnkLoginregister(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow();
            register.Show();
            this.Hide();

        }


        private void LoginManager_Click(object sender, RoutedEventArgs e)
        {
            accountService = new();
            string email = txtemail.Text;
            string password = txtPassword.Password;
            var p = accountService.LoginWithAdmin(email, password);
            if (p.Isuccess == false)
            {
                MessageBox.Show("Login in errors", "Please try login try again", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                HomeWindow home = new HomeWindow(p.Value);
                this.Hide();
                home.Show();
            }

        }



    }
}
