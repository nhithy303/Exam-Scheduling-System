using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmLogin : Window
    {
        public frmLogin()
        {
            InitializeComponent();
            this.Loaded += frmLogin_Load;
            btnDangNhap.Click += btnDangNhap_Click;
            btnThoat.Click += btnThoat_Click;
            this.Closing += frmLogin_Closing;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtTenDangNhap.Focus();
            rdoAdmin.IsChecked = true;
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            TaiKhoan tk = new TaiKhoan();
            tk.TenDangNhap = txtTenDangNhap.Text.Trim();
            tk.MatKhau = pwbMatKhau.Password;
            tk.QuanTriVien = (rdoAdmin.IsChecked == true) ? 1 : 0;
            try
            {
                TaiKhoanBLL tk_bll = new TaiKhoanBLL();
                switch (tk_bll.CheckLoginBLL(tk))
                {
                    case "empty tendangnhap":
                        ShowError("Tên đăng nhập còn trống!");
                        txtTenDangNhap.Focus(); return;
                    case "empty matkhau":
                        ShowError("Mật khẩu còn trống!");
                        pwbMatKhau.Focus(); return;
                    case "failed":
                        ShowError("Tên đăng nhập hoặc mật khẩu không chính xác!"); return;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                ShowError("Đăng nhập thất bại!\nNguyên nhân: " + ex.Message);
            }
            this.Hide();
            if (rdoAdmin.IsChecked == true)
            {
                new frmAdmin(tk).ShowDialog();
            }
            else if (rdoSinhVien.IsChecked == true)
            {
                new frmSinhVien(tk).ShowDialog();
            }
            txtTenDangNhap.Clear(); pwbMatKhau.Clear();
            this.Show();
            txtTenDangNhap.Focus();
        }

        private void ShowError(string error)
        {
            MessageBox.Show(error, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogin_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thoát",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            e.Cancel = (result == MessageBoxResult.No);
        }
    }
}
