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
        RoomService roomService;

        public HomeWindow(AdminDTO value)
        {
            InitializeComponent();
            cutomerService = new CutomerService();
            roomTypeService = new RoomTypeService();
            bookingDetailService = new BookingDetailService();
            roomService = new RoomService();
            LoadRoomInformation();
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
            roomService = new RoomService();
            LoadRoomInformation();
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
            string name = txtSearchCustomerName.Text.Trim();
            int? id = null;

            if (int.TryParse(txtSearchCustomerId.Text, out int parsedId))
                id = parsedId;

            try
            {
                List<CustomerDTO> result;

                if (string.IsNullOrWhiteSpace(name) && id == null)
                {
                    result = cutomerService.ViewCustomer();
                }
                else
                {
                    result = cutomerService.SearchCustomer(name, id ?? -1)
                                             ?? new List<CustomerDTO>();
                }

                if (result.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy khách hàng nào phù hợp.",
                                    "Thông báo",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                }

                dgCustomers.ItemsSource = null;
                dgCustomers.ItemsSource = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}",
                                "Lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }




        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // 1) Kiểm tra có chọn khách hàng chưa
            if (!(dgCustomers.SelectedItem is CustomerDTO customer))
            {
                MessageBox.Show(
                    "Vui lòng chọn khách hàng cần xóa.",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            // 2) Hỏi xác nhận
            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa khách hàng '{customer.CustomerFullName}'?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (confirm != MessageBoxResult.Yes)
                return;

            // 3) Thực hiện xóa và xử lý kết quả
            try
            {
                bool success = cutomerService.RemoveCutomer(customer.CustomerId);
                MessageBox.Show(
                    success ? "Xóa khách hàng thành công!" : "Xóa không thành công!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    success ? MessageBoxImage.Information : MessageBoxImage.Error
                );

                if (success)
                {
                    // 4) Reload và Clear form
                    LoadData();
                    Clear_Customer();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi xóa khách hàng: {ex.Message}",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // 1) Basic null/empty checks
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerPhone.Text) ||
                !dpDOB.SelectedDate.HasValue ||
                string.IsNullOrWhiteSpace(txtCustomerEmail.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerstatus.Text) ||
                !byte.TryParse(txtCustomerstatus.Text, out byte status))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ và hợp lệ thông tin khách hàng.",
                                "Thông báo",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }
            // 2) Build DTO
            var cus = new CustomerDTO
            {
                CustomerFullName = txtCustomerName.Text.Trim(),
                Telephone = txtCustomerPhone.Text.Trim(),
                CustomerBirthday = dpDOB.SelectedDate.Value,
                EmailAddress = txtCustomerEmail.Text.Trim(),
                CustomerStatus = status
            };

            // 3) Try to add
            try
            {
                cutomerService.AddCutomer(cus);
                MessageBox.Show("Thêm khách hàng thành công!",
                                "Thông báo",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                LoadData();        
                Clear_Customer();  
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Lỗi định dạng dữ liệu: " + ex.Message,
                                "Lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm khách hàng: " + ex.Message,
                                "Lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
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
            // 1) Kiểm tra đã chọn RoomType chưa
            if (!(dgRoomTypes.SelectedItem is RoomTypeDTO roomtype))
            {
                MessageBox.Show(
                    "Vui lòng chọn loại phòng cần xóa.",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            // 2) Hỏi xác nhận xóa
            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa loại phòng '{roomtype.RoomTypeName}'?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (confirm != MessageBoxResult.Yes)
                return;

            // 3) Thực hiện xóa trong try/catch
            try
            {
                bool success = roomTypeService.RemoveRoomType(roomtype.RoomTypeId);
                MessageBox.Show(
                    success ? "Xóa loại phòng thành công!" : "Xóa loại phòng không thành công!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    success ? MessageBoxImage.Information : MessageBoxImage.Error
                );

                if (success)
                {
                    // 4) Reload dữ liệu và clear form
                    LoadData_1();
                    ClearRoomForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi xóa loại phòng: {ex.Message}",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }


        private void Button_SearchRoom(object sender, RoutedEventArgs e)
        {
            // 1) Lấy và trim giá trị search
            string name = txtSearchRoomTypeName.Text.Trim();
            int? id = int.TryParse(txtSearchRoomTypeId.Text, out var parsedId)
                      ? parsedId
                      : (int?)null;

            try
            {
                List<RoomTypeDTO> result;

                // 2) Nếu không nhập gì cả, show toàn bộ
                if (string.IsNullOrWhiteSpace(name) && id == null)
                {
                    result = roomTypeService.ViewRoomType();
                }
                else
                {
                    // 3) Gọi Search; nếu null, khởi tạo list rỗng
                    result = roomTypeService
                                 .SearchRoomType(name, id ?? -1)
                                 ?? new List<RoomTypeDTO>();
                }

                // 4) Nếu không có kết quả, báo người dùng
                if (result.Count == 0)
                {
                    MessageBox.Show(
                        "Không tìm thấy loại phòng nào phù hợp.",
                        "Thông báo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }

                // 5) Bind lại DataGrid
                dgRoomTypes.ItemsSource = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Đã xảy ra lỗi khi tìm kiếm: {ex.Message}",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }



        private void LoadData_1()
        {
            dgRoomTypes.ItemsSource = null;
            dgRoomTypes.ItemsSource = roomTypeService.ViewRoomType();
        }
        private void Button_UpdateRoom(object sender, RoutedEventArgs e)
        {
            // 1) Kiểm tra ID hợp lệ
            if (!int.TryParse(txtRoomTypeId.Text, out int id))
            {
                MessageBox.Show(
                    "RoomType ID không hợp lệ hoặc chưa chọn dòng!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            // 2) Tạo đối tượng RoomType từ form
            var room = new RoomType
            {
                RoomTypeId = id,
                RoomTypeName = txtRoomTypeName.Text.Trim(),
                TypeDescription = txtRoomTypeDescription.Text.Trim(),
                TypeNote = txtRoomTypeNote.Text.Trim()
            };

            // 3) Cập nhật trong try/catch
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
            // 1) Validate inputs
            if (string.IsNullOrWhiteSpace(txtRoomTypeName.Text) ||
                string.IsNullOrWhiteSpace(txtRoomTypeDescription.Text) ||
                string.IsNullOrWhiteSpace(txtRoomTypeNote.Text))
            {
                MessageBox.Show(
                    "Vui lòng nhập đầy đủ thông tin loại phòng.",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            // 2) Build DTO
            var room = new RoomTypeDTO
            {
                RoomTypeName = txtRoomTypeName.Text.Trim(),
                TypeDescription = txtRoomTypeDescription.Text.Trim(),
                TypeNote = txtRoomTypeNote.Text.Trim()
            };

            // 3) Try to add
            try
            {
                roomTypeService.AddRoomType(room);
                MessageBox.Show(
                    "Thêm loại phòng thành công!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // 4) Reload and clear
                LoadData_1();
                ClearRoomForm();
            }
            catch (FormatException fx)
            {
                MessageBox.Show(
                    $"Lỗi định dạng dữ liệu: {fx.Message}",
                    "Lỗi thêm loại phòng",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi thêm loại phòng: {ex.Message}",
                    "Lỗi thêm loại phòng",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
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

        //ROOM INFORMATION ---------------------------------------------
        private void LoadRoomInformation()

        {

            dgRoom_Information.ItemsSource = null;
            dgRoom_Information.ItemsSource = roomService.ViewRoom();
        }
        private void dgRoomInformation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgRoom_Information.SelectedItem is RoomInformationDTO selected)
            {
                txtRoomId.Text = selected.RoomId.ToString();
                txtRoomNumber.Text = selected.RoomNumber;
                txtRoomMaxCapacity.Text = selected.RoomMaxCapacity?.ToString() ?? string.Empty;
                txtRoomTypeIdInfor.Text = selected.RoomTypeId.ToString();
                txtRoomDescription.Text = selected.RoomDetailDescription ?? string.Empty;
                txtRoomStatus.Text = selected.RoomStatus?.ToString() ?? string.Empty;
                txtPrice.Text = selected.RoomPricePerDay?.ToString() ?? string.Empty;
            }

        }
        private void Button_SearchRoomInfor(object sender, RoutedEventArgs e)
        {
            string roomNumber = txtSearchRoomNumber.Text;
            byte? status = null;

            if (byte.TryParse(txtSearchRoomStatus.Text, out byte parsedStatus))
            {
                status = parsedStatus;
            }

            try
            {
                List<RoomInformationDTO> result;

                if (string.IsNullOrWhiteSpace(roomNumber) && status == null)
                {
                    result = roomService.ViewRoom();
                }
                else
                {
                    result = roomService.SearchRoom(roomNumber, status) ?? new List<RoomInformationDTO>();
                }

                if (result.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy phòng nào phù hợp.");
                }

                dgRoom_Information.ItemsSource = null;
                dgRoom_Information.ItemsSource = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_DeleteRoomInfor(object sender, RoutedEventArgs e)
        {
            var selected = (RoomInformationDTO)dgRoom_Information.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn phòng cần xóa.");
                return;
            }
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa phòng này?", "Xác nhận xóa", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                bool result = roomService.RemoveRoom(selected.RoomId);
                MessageBox.Show(result ? "Xóa phòng thành công!" : "Xóa không thành công!", "Thông báo");
                LoadRoomInformation();
                ClearRoomInforForm();
            }
        }
        private void Button_AddRoomInfor(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoomNumber.Text) ||
                !int.TryParse(txtRoomMaxCapacity.Text, out int capacity) ||
                 !byte.TryParse(txtRoomStatus.Text, out byte status) ||
                 !double.TryParse(txtPrice.Text, out double price) ||
                 !int.TryParse(txtRoomTypeIdInfor.Text, out int typeid) )
            {
                MessageBox.Show("Vui lòng nhập đầy đủ và hợp lệ thông tin phòng.");
                return;
            }
            RoomInformationDTO room = new RoomInformationDTO
            {
                RoomNumber = txtRoomNumber.Text,
                RoomDetailDescription = txtRoomDescription.Text,
                RoomMaxCapacity = int.Parse(txtRoomMaxCapacity.Text),
                RoomStatus = byte.Parse(txtRoomStatus.Text),
                RoomPricePerDay = double.Parse(txtPrice.Text),
                RoomTypeId = int.Parse(this.txtRoomTypeIdInfor.Text),
            };

            try
            {
                roomService.AddRoom(room);
                MessageBox.Show("Thêm phòng thành công!");
                LoadRoomInformation();
                ClearRoomInforForm();

            }
            catch (FormatException ex)
            {
                MessageBox.Show("Lỗi định dạng dữ liệu: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm phòng: " + ex.Message);
            }
        }
        private void Button_UpdateRoomInfor(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtRoomId.Text, out int id))
            {
                MessageBox.Show("Phòng chưa được chọn hoặc ID không hợp lệ!");
                return;
            }

            RoomInformationDTO room = new RoomInformationDTO
            {
                RoomId = id,
                RoomNumber = txtRoomNumber.Text,
                RoomDetailDescription = txtRoomDescription.Text,
                RoomMaxCapacity = int.Parse(txtRoomMaxCapacity.Text),
                RoomStatus = byte.Parse(txtRoomStatus.Text),
                RoomPricePerDay = double.Parse(txtPrice.Text)
            };

            try
            {
                bool success = roomService.UpdateRoom(room);
                MessageBox.Show(success ? "Cập nhật thành công!" : "Cập nhật thất bại!");
                LoadRoomInformation();
                ClearRoomInforForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }
        private void Button_ClearRoomInfor(object sender, RoutedEventArgs e)
        {
            txtRoomId.Clear();
            txtRoomNumber.Clear();
            txtRoomDescription.Clear();
            txtRoomMaxCapacity.Clear();
            txtRoomTypeIdInfor.Clear();
            txtRoomStatus.Clear();
            txtPrice.Clear();

        }
        private void ClearRoomInforForm()
        {
            txtRoomNumber.Text = "";
            txtRoomDescription.Text = "";
            txtRoomMaxCapacity.Text = "";
            txtRoomTypeIdInfor.Text = "";
            txtRoomStatus.Text = "";
            txtPrice.Text = "";
        }
    }



}
