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
using Service.UserService;

namespace PRN212_Assignment_01
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void LnkLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();     
            this.Hide();      
        }

        private void Button_Register_Click(object sender, RoutedEventArgs e)
        {
            var FullName = txtFullName.Text;
            var Telepphone = txtTelephone.Text;
            var Email = txtEmail.Text;
            var password = pbPassword.Password;
            var CPassword = pbConfirmPassword.Password;
            var selectedItem = cbStatus.SelectedItem as ComboBoxItem;
            byte status = selectedItem != null ? byte.Parse(selectedItem.Tag.ToString()) : (byte)0;
            DateTime? birthDate = dpBirthday.SelectedDate;
            if (!password.Equals(CPassword))
            {
                MessageBox.Show("Password do not match", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                AccountService accountService = new AccountService();
                var customer = new DataAccess.Models.Customer
                {
                    CustomerFullName = FullName,
                    Telephone = Telepphone,
                    EmailAddress = Email,
                    Password = password,
                    CustomerBirthday = birthDate,
                    CustomerStatus = status,
                    RoleId = 1 
                };
                bool result = accountService.AddCustomer(customer);

                if (result)
                {
                    MessageBox.Show("Register successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                    new LoginWindow().Show();

                }
                else
                {
                    MessageBox.Show("Register failed! Please Fill out Information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

       
    }
}
