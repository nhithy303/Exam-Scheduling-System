using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        KyThiBLL kt_bll = new KyThiBLL();
        public frmSinhVien(TaiKhoan tk)
        {
            InitializeComponent();
            this.tk = tk;
            this.Loaded += frmSinhVien_Load;
            btnDangXuat.Click += btnDangXuat_Click;
            btnXemLichThi.Click += btnXemLichThi_Click;
            btnDoiMatKhau.Click += btnDoiMatKhau_Click;
            this.Closing += frmSinhVien_Closing;
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
            gbThongTinKyThi_Load();
            cbLocNgayThi.Checked += cbLocNgayThi_Checked;
            cbLocNgayThi.Unchecked += cbLocNgayThi_Unchecked;
            dpNgayThi.SelectedDateChanged += dpNgayThi_SelectedDateChanged;
            dgLichThiSV_Load("");
            if (dgLichThiSV.Items.Count > 0)
            {
                btnExportExcel.IsEnabled = true;
            }
            else
            {
                btnExportExcel.IsEnabled = false;
            }
            btnExportExcel.Click += btnExportExcel_Click;
        }

        private void gbThongTinKyThi_Load()
        {
            KyThi kt = kt_bll.GetInfo();
            if (kt != null)
            {
                gbThongTinKyThi.IsEnabled = true;
                txtNamHoc.Text = kt.NamHoc;
                txtHocKy.Text = kt.HocKy.ToString();
                dpNgayThi.DisplayDateStart = kt.NgayBatDau;
                dpNgayThi.DisplayDateEnd = kt.NgayKetThuc;
                cbLocNgayThi.IsChecked = dpNgayThi.IsEnabled = false;
            }
            else
            {
                gbThongTinKyThi.IsEnabled = false;
            }
        }
        
        private void cbLocNgayThi_Checked(object sender, EventArgs e)
        {
            dpNgayThi.IsEnabled = true;
        }

        private void cbLocNgayThi_Unchecked(object sender, EventArgs e)
        {
            dpNgayThi.IsEnabled = false;
            dgLichThiSV_Load("");
        }

        private void dpNgayThi_SelectedDateChanged(object sender, EventArgs e)
        {
            if (dpNgayThi.SelectedDate.HasValue)
            {
                dgLichThiSV_Load(dpNgayThi.SelectedDate.Value.ToString("yyyy/MM/dd"));
            }
        }

        private void dgLichThiSV_Load(string ngaythi)
        {
            dgLichThiSV.ItemsSource = null;
            LichThiSV[] ltsv = ltsv_bll.GetList(sv.MSSV, ngaythi);
            dgLichThiSV.ItemsSource = ltsv;
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            // Reload full data of exam schedule
            dgLichThiSV_Load("");

            // Convert DataGrid to DataTable
            LichThiSV[] ltsv = (LichThiSV[])dgLichThiSV.ItemsSource;
            DataTable table = new DataTable();
            table.Columns.Add("Môn thi");
            table.Columns.Add("Phòng thi");
            table.Columns.Add("Ngày thi");
            table.Columns.Add("Buổi thi");
            for (int i = 0; i < ltsv.Length; i++)
            {
                DataRow row = table.NewRow();
                row[0] = ltsv[i].MonThi;
                row[1] = ltsv[i].PhongThi;
                row[2] = ReverseDateFormat(ltsv[i].NgayThi);
                row[3] = ltsv[i].BuoiThi;
                table.Rows.Add(row);
            }
            
            // Export excel file
            ExcelFileBLL excel_bll = new ExcelFileBLL();
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Title = "Xuất File Excel";
            dialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
            Nullable<bool> result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                if (excel_bll.Export(table, dialog.FileName))
                {
                    ShowMessage("Export file excel thành công!");
                }
                else
                {
                    ShowError("Export file excel thất bại!");
                }
            }
            cbLocNgayThi.IsChecked = false;
        }

        private string ReverseDateFormat(string date)
        {
            string[] str = date.Split('/', '-');
            return str[2] + "/" + str[1] + "/" + str[0];
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
                    ShowMessage("Đổi mật khẩu thành công!");
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

        private void ShowMessage(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSinhVien_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Thoát",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            e.Cancel = (result == MessageBoxResult.No);
        }
    }
}
