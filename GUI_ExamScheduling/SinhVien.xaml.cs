using BLL;
using DTO;
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

namespace GUI
{
    /// <summary>
    /// Interaction logic for SinhVien.xaml
    /// </summary>
    public partial class frmSinhVien : Window
    {
        TaiKhoan tk;
        SinhVien sv;
        SinhVienBLL sv_bll = new SinhVienBLL();
        LichThiSVBLL ltsv_bll = new LichThiSVBLL();
        TaiKhoanBLL tk_bll = new TaiKhoanBLL();
        public frmSinhVien(TaiKhoan tk)
        {
            InitializeComponent();
            this.tk = tk;
            this.Loaded += frmSinhVien_Load;
            btnThoat.Click += btnThoat_Click;
            btnXemLichThi.Click += btnXemLichThi_Click;
            btnDoiMatKhau.Click += btnDoiMatKhau_Click;
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            string condition = "MSSV = '" + tk.TenDangNhap + "'";
            sv = sv_bll.GetList(condition)[0];
            InfoLabel.Content = sv.HoSV + " " + sv.TenSV + "\nMSSV: " + sv.MSSV;
            tciXemLichThi_Load();
            tciDoiMatKhau_Load();
        }

        /// <summary>
        /// XEM LỊCH THI
        /// </summary>
        private void tciXemLichThi_Load()
        {
            dgLichThiSV_Load();
        }

        private void dgLichThiSV_Load()
        {
            dgLichThiSV.ItemsSource = null;
            LichThiSV[] ltsv = ltsv_bll.GetList(sv.MSSV);
            dgLichThiSV.ItemsSource = ltsv;
        }

        /// <summary>
        /// tciDoiMatKhau
        /// </summary>
        private void tciDoiMatKhau_Load()
        {
            btnLuuThayDoi.Click += btnLuuThayDoi_Click;
        }

        private void btnLuuThayDoi_Click(object sender, EventArgs e)
        {
            string result = tk_bll.Update(tk, txtMatKhauHienTai.Password, txtMatKhauMoi.Password, txtXacNhanMatKhauMoi.Password);
            switch (result)
            {
                case "empty":
                    ShowError("Vui lòng nhập đầy đủ thông tin!"); break;
                case "unmatched":
                    ShowError("Mật khẩu mới không khớp!"); break;
                case "wrong password":
                    ShowError("Sai mật khẩu!"); break;
                default:
                    MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtMatKhauHienTai.Clear(); txtMatKhauMoi.Clear(); txtXacNhanMatKhauMoi.Clear(); break;
            }
        }

        /// <summary>
        /// MENU BUTTON
        /// </summary>
        private void btnXemLichThi_Click(object sender, EventArgs e)
        {
            tcMenu.SelectedValue = tciXemLichThi;
            btn_changeFontWeight(ref btnXemLichThi);
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            tcMenu.SelectedValue = tciDoiMatKhau;
            btn_changeFontWeight(ref btnDoiMatKhau);
        }

        private void btn_changeFontWeight(ref Button btn)
        {
            foreach (Button item in MenuGrid.Children)
            {
                if (item.FontWeight == FontWeights.Bold)
                {
                    item.FontWeight = FontWeights.Normal;
                }
            }
            btn.FontWeight = FontWeights.Bold;
        }

        private void ShowError(string error)
        {
            MessageBox.Show(error, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
