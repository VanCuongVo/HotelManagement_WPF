using Repository.DTO;
using Service.UserService;
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

namespace PRN212_Assignment_01
{
    /// <summary>
    /// Interaction logic for CustomerHomeWindow.xaml
    /// </summary>
    public partial class CustomerHomeWindow : Window
    {
        CutomerService customerService;
        BookingReservationService bookingReservationService;

        public CustomerHomeWindow(CustomerDTO value)
        {
            InitializeComponent();
            customerService = new CutomerService();
            txtWelcome.Text = "Hi," + value.CustomerFullName;
            txtCustomerID.Text = value.CustomerId.ToString();
            txtCustomerEmail.Text = value.EmailAddress;
            txtCustomerlName.Text = value.CustomerFullName;
            txtCustomerTelephone.Text = value.Telephone;
            txtCustomerBirthday.Text = value.CustomerBirthday?.ToString("dd/MM/yyyy");
        }


        private CustomerDTO GetCustomerFromInputs()
        {
            return new CustomerDTO
            {
                CustomerId = int.Parse(txtCustomerID.Text),
                CustomerFullName = txtCustomerlName.Text,
                EmailAddress = txtCustomerEmail.Text,
                Telephone = txtCustomerTelephone.Text,
                CustomerStatus = 1,
                CustomerBirthday = DateTime.Parse(txtCustomerBirthday.Text)
            };
        }


        private void txtCustomerID_TextChanged(object sender, TextChangedEventArgs e)
        {

        }      

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            CustomerDTO value = GetCustomerFromInputs();
            var result = MessageBox.Show(
         "Bạn có chắc chắn muốn sửa không?",
         "Xác nhận sửa",
         MessageBoxButton.YesNo,
         MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                bool check = true;
                try
                {
                    customerService.UpdateCutomer(value);
                }
                catch
                {
                    MessageBox.Show("Update fail!");
                    check = false;
                }
                if (check)
                {
                    MessageBox.Show("Update success!");
                  
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Hide();

        }

        private void btnViewBookingReservation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int customerId = int.Parse(txtCustomerID.Text);
                bookingReservationService = new BookingReservationService();
                List<DataAccess.Models.BookingReservation> bookings = bookingReservationService.GetBookingReservationsByCustomerId(customerId);
                
                // Filter out bookings with BookingStatus = 0
                var filteredBookings = bookings.Where(b => b.BookingStatus != 0).ToList();
                
                dgBookingReservation.ItemsSource = filteredBookings;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading booking reservations: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgBookingReservation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
