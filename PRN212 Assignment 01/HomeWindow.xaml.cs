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
using DataAccess.Models;
using Microsoft.Identity.Client.NativeInterop;
using Repository.DTO;
using Service.RoomService;
using Service.UserService;

namespace PRN212_Assignment_01
{

    public partial class HomeWindow : Window
    {
        CutomerService cutomerService;
        RoomTypeService roomTypeService;
        BookingDetailService bookingDetailService;


        public HomeWindow(AdminDTO value)
        {
            InitializeComponent();
            cutomerService = new CutomerService();
            roomTypeService = new RoomTypeService();
            bookingDetailService = new BookingDetailService();
            dgCustomers.ItemsSource = cutomerService.ViewCustomer();
            dgRoomTypes.ItemsSource = roomTypeService.ViewRoomType();


            var reportData = bookingDetailService.GetBookingDetailsReport();
            dgBookingReport.ItemsSource = reportData;
            txtWelcome.Text = "Hi," + "Admin";
        }

        public HomeWindow(CustomerDTO value)
        {
            InitializeComponent();
            cutomerService = new CutomerService();
            roomTypeService = new RoomTypeService();
            bookingDetailService = new BookingDetailService();
            dgCustomers.ItemsSource = cutomerService.ViewCustomer();
            dgRoomTypes.ItemsSource = roomTypeService.ViewRoomType();
            var reportData = bookingDetailService.GetBookingDetailsReport();
            dgBookingReport.ItemsSource = reportData;
            txtWelcome.Text = "Hi," + value.CustomerFullName;
            handerRole(value.RoleId);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Hide();

        }
        private void dgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (CustomerDTO)dgCustomers.SelectedItem;
            if (selected != null)
            {
                txtCustomerId.Text = selected.CustomerId.ToString();
                txtCustomerName.Text = selected.CustomerFullName ?? string.Empty;
                txtCustomerEmail.Text = selected.EmailAddress.ToString();
                txtCustomerPhone.Text = selected.Telephone ?? string.Empty;
                dpDOB.SelectedDate = selected.CustomerBirthday;
                txtCustomerstatus.Text = selected.CustomerStatus.ToString();
            }

        }

        private void handerRole(int? roleID)
        {
            if (roleID == 1)
            {
                btnAdd.IsEnabled = false;
                btnUpdate.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
        }

        private void Clear_Customer()
        {

            txtCustomerId.Text = "";
            txtCustomerName.Text = "";
            txtCustomerEmail.Text = "";
            txtCustomerPhone.Text = "";
            dpDOB.SelectedDate = null;
            txtCustomerstatus.Text = "";
        }



        private void LoadData()
        {
            dgCustomers.ItemsSource = null;
            dgCustomers.ItemsSource = cutomerService.ViewCustomer();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var name = txtSearchCustomerName.Text;
            var id = txtSearchCustomerId.Text != null ? txtSearchCustomerId.Text : "";
            cutomerService = new();
            try
            {
                dgCustomers.ItemsSource = null;
                if (id != null)
                {
                    dgCustomers.ItemsSource = cutomerService.SearchCustomer(name, int.Parse(id));

                }
                else
                {
                    dgCustomers.ItemsSource = cutomerService.SearchCustomer(name, -1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var customer = (CustomerDTO)dgCustomers.SelectedItem;
            bool check = cutomerService.RemoveCutomer(customer.CustomerId);
            if (check == true)
            {
                LoadData();
                Clear_Customer();
                MessageBox.Show("Xóa thành công!");
            }
            else
            {
                MessageBox.Show("Error to remve", "Error ", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CustomerDTO cus = new();
            cus.CustomerFullName = txtCustomerName.Text;
            cus.Telephone = txtCustomerPhone.Text;
            cus.CustomerBirthday = dpDOB.SelectedDate.Value;
            cus.EmailAddress = txtCustomerEmail.Text;
            cus.CustomerStatus = byte.Parse(txtCustomerstatus.Text);
            try
            {
                cutomerService.AddCutomer(cus);
                MessageBox.Show("Thêm thành công!");
                LoadData();
                Clear_Customer();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }




        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtCustomerId.Text, out int id))
            {
                MessageBox.Show("Customer ID không hợp lệ hoặc chưa chọn dòng!");
                return;
            }
            CustomerDTO cus = new();
            cus.CustomerId = id;
            cus.CustomerFullName = txtCustomerName.Text;
            cus.Telephone = txtCustomerPhone.Text;
            cus.CustomerBirthday = dpDOB.SelectedDate.Value;
            cus.EmailAddress = txtCustomerEmail.Text;
            cus.CustomerStatus = byte.Parse(txtCustomerstatus.Text);
            try
            {
                cutomerService.UpdateCutomer(cus);
                MessageBox.Show("Cập nhật thành công!");
                LoadData();
                Clear_Customer();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);

            }

        }


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Room_Type

        private void dgRoomTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //dgRoomTypes.ItemsSource = roomTypeService.ViewRoomType();
            var selected = (RoomTypeDTO)dgRoomTypes.SelectedItem;
            if (selected != null)
            {
                txtRoomTypeId.Text = selected.RoomTypeId.ToString();
                txtRoomTypeName.Text = selected.RoomTypeName.ToString();
                txtRoomTypeNote.Text = selected.TypeNote.ToString();
                txtRoomTypeDescription.Text = selected.TypeDescription.ToString();
            }

        }

        private void Button_DeleteRoom(object sender, RoutedEventArgs e)
        {
            var roomtype = (RoomTypeDTO)dgRoomTypes.SelectedItem;
            bool check = roomTypeService.RemoveRoomType(roomtype.RoomTypeId);
            if (check == true)
            {
                LoadData_1();
                ClearRoomForm();
                MessageBox.Show("Xóa thành công!");
            }
            else
            {
                MessageBox.Show("Error to remve", "Error ", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private void Button_SearchRoom(object sender, RoutedEventArgs e)
        {

            var name = txtSearchRoomTypeName.Text;
            var id = txtSearchRoomTypeId.Text != null ? txtSearchRoomTypeId.Text : "";
            roomTypeService = new();
            try
            {
                dgRoomTypes.ItemsSource = null;
                if (id != null)
                {
                    dgRoomTypes.ItemsSource = roomTypeService.SearchRoomType(name, int.Parse(id));

                }
                else
                {
                    dgCustomers.ItemsSource = roomTypeService.SearchRoomType(name, -1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void LoadData_1()
        {
            dgRoomTypes.ItemsSource = null;
            dgRoomTypes.ItemsSource = roomTypeService.ViewRoomType();
        }
        private void Button_UpdateRoom(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtRoomTypeId.Text, out int id))
            {
                MessageBox.Show("RoomType ID không hợp lệ hoặc chưa chọn dòng!");
                return;
            }

            RoomType room = new RoomType
            {
                RoomTypeId = id,
                RoomTypeName = txtRoomTypeName.Text,
                TypeDescription = txtRoomTypeDescription.Text,
                TypeNote = txtRoomTypeNote.Text
            };

            try
            {
                roomTypeService.UpdateRoomType(room);
                MessageBox.Show("Cập nhật thành công!");
                LoadData_1(); // Load lại danh sách
                ClearRoomForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }


        private void Button_AddRoom(object sender, RoutedEventArgs e)
        {
            RoomTypeDTO room = new();
            room.RoomTypeName = txtRoomTypeName.Text;
            room.TypeDescription = txtRoomTypeDescription.Text;
            room.TypeNote = txtRoomTypeNote.Text;
            roomTypeService.AddRoomType(room);
            MessageBox.Show("Thêm thành công!");
            LoadData_1();
            ClearRoomForm();

        }

        private void ClearRoomForm()
        {
            txtRoomTypeId.Text = "";
            txtRoomTypeName.Text = "";
            txtRoomTypeDescription.Text = "";
            txtRoomTypeNote.Text = "";
        }

        private void Button_ClearRoom(object sender, RoutedEventArgs e)
        {
           
            this.Close();
        }
    }



}

