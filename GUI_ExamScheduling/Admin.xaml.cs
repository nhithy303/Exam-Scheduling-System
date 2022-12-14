using BLL;
using DTO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
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
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class frmAdmin : Window
    {
        TaiKhoan tk;
        TaiKhoanBLL tk_bll = new TaiKhoanBLL();
        KhoaBLL kh_bll = new KhoaBLL();
        SinhVienBLL sv_bll = new SinhVienBLL();
        CaThiBLL ct_bll = new CaThiBLL();
        MonThiBLL mt_bll = new MonThiBLL();
        PhongThiBLL pt_bll = new PhongThiBLL();
        ThamGiaThiBLL tgt_bll = new ThamGiaThiBLL();
        PhanBoPhongThiBLL pbpt_bll = new PhanBoPhongThiBLL();
        KyThiBLL kt_bll = new KyThiBLL();
        LichThiBLL lt_bll = new LichThiBLL();
        ExcelFileBLL excel_bll = new ExcelFileBLL();
        public frmAdmin(TaiKhoan tk)
        {
            InitializeComponent();
            this.tk = tk;
            this.Loaded += frmAdmin_Load;
            btnDsSinhVien.Click += btnDsSinhVien_Click;
            btnDsMonThi.Click += btnDsMonThi_Click;
            btnDsPhongThi.Click += btnDsPhongThi_Click;
            btnXepLichThi.Click += btnXepLichThi_Click;
            btnDoiMatKhau.Click += btnDoiMatKhau_Click;
            btnDangXuat.Click += btnDangXuat_Click;
            this.Closing += frmAdmin_Closing;
        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {
            tciDanhSachSinhVien_Load();
            tciDanhSachMonThi_Load();
            tciDanhSachPhongThi_Load();
            tciXepLichThi_Load();
            tciDoiMatKhau_Load();
        }

        /// <summary>
        /// DANH SÁCH SINH VIÊN
        /// </summary>
        private void tciDanhSachSinhVien_Load()
        {
            cboKhoa_Load();
            dgSinhVien_Load();
            cboKhoa.SelectionChanged += cboKhoa_SelectionChanged;
            btnImportExcelSinhVien.Click += btnImportExcelSinhVien_Click;
            btnExportExcelSinhVien.Click += btnExportExcelSinhVien_Click;
        }

        private void cboKhoa_Load()
        {
            Khoa[] kh = kh_bll.GetList();
            cboKhoa.DisplayMemberPath = "TenKhoa";
            cboKhoa.SelectedValuePath = "MaKhoa";
            cboKhoa.ItemsSource = kh;
            cboKhoa.SelectedIndex = 0;
        }

        private void dgSinhVien_Load()
        {
            dgSinhVien.ItemsSource = null;
            string condition = "";
            if (cboKhoa.SelectedIndex > 0)
            {
                condition = "MaKhoa = '" + cboKhoa.SelectedValue + "'";
            }
            SinhVien[] sv = sv_bll.GetList(condition);
            dgSinhVien.ItemsSource = sv;
        }

        private void cboKhoa_SelectionChanged(object sender, EventArgs e)
        {
            if (cboKhoa.SelectedItem != null)
            {
                dgSinhVien_Load();
            }
        }

        private void btnImportExcelSinhVien_Click(object sender, EventArgs e)
        {
            string filepath = ChooseFileExcel();
            if (filepath != "")
            {
                bool overwrite = false;
                MessageBoxResult result = MessageBox.Show("Bạn có muốn thay toàn bộ dữ liệu sinh viên đã có bằng dữ liệu trong file excel không?",
                    "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes) { overwrite = true; }
                if (excel_bll.Import(filepath, "SinhVien", overwrite))
                {
                    ShowMessage("Import file excel thành công!");
                    dgSinhVien_Load();
                }
                else
                {
                    ShowError("Import file excel thất bại!");
                }
            }
        }

        private void btnExportExcelSinhVien_Click(object sender, EventArgs e)
        {
            SinhVien[] sv = (SinhVien[])dgSinhVien.ItemsSource;
            DataTable table = new DataTable();
            for (int i = 0; i < dgSinhVien.Columns.Count; i++)
            {
                table.Columns.Add(dgSinhVien.Columns[i].Header.ToString());
            }
            for (int i = 0; i < sv.Length; i++)
            {
                DataRow row = table.NewRow();
                row[0] = sv[i].MSSV;
                row[1] = sv[i].HoSV;
                row[2] = sv[i].TenSV;
                row[3] = ReverseDateFormat(sv[i].NgaySinh.ToString());
                row[4] = sv[i].GioiTinh;
                row[5] = sv[i].MaKhoa;
                table.Rows.Add(row);
            }
            ExportFileExcel(table);
        }

        /// <summary>
        /// DANH SÁCH MÔN THI
        /// </summary>
        private void tciDanhSachMonThi_Load()
        {
            cboCaThi_Load();
            dgMonThi_Load();
            cboCaThi.SelectionChanged += cboCaThi_SelectionChanged;
            btnImportExcelMonThi.Click += btnImportExcelMonThi_Click;
            btnExportExcelMonThi.Click += btnExportExcelMonThi_Click;
        }

        private void cboCaThi_Load()
        {
            CaThi[] ct = ct_bll.GetList();
            if (ct != null)
            {
                CaThi[] ct_all = new CaThi[ct.Length + 1];
                ct_all[0] = new CaThi();
                ct_all[0].MaCa = "All";
                for (int i = 1; i < ct_all.Length; i++)
                {
                    ct_all[i] = ct[i - 1];
                }
                cboCaThi.DisplayMemberPath = "MaCa";
                cboCaThi.SelectedValuePath = "MaCa";
                cboCaThi.ItemsSource = ct_all;
                cboCaThi.SelectedIndex = 0;
            }
        }

        private void dgMonThi_Load()
        {
            dgMonThi.ItemsSource = null;
            string condition = "";
            if (cboCaThi.SelectedIndex > 0)
            {
                condition = "MaCa = '" + cboCaThi.SelectedValue + "'";
            }
            MonThi[] mt = mt_bll.GetList(condition);
            dgMonThi.ItemsSource = mt;
        }

        private void cboCaThi_SelectionChanged(object sender, EventArgs e)
        {
            if (cboCaThi.SelectedItem != null)
            {
                dgMonThi_Load();
            }
        }

        private void btnImportExcelMonThi_Click(object sender, EventArgs e)
        {
            string filepath = ChooseFileExcel();
            if (filepath != "")
            {
                bool overwrite = false;
                MessageBoxResult result = MessageBox.Show("Bạn có muốn thay toàn bộ dữ liệu môn thi đã có bằng dữ liệu trong file excel không?",
                    "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes) { overwrite = true; }
                if (excel_bll.Import(filepath, "MonThi", overwrite))
                {
                    ShowMessage("Import file excel thành công!");
                    dgMonThi_Load();
                }
                else
                {
                    ShowError("Import file excel thất bại!");
                }
            }
        }

        private void btnExportExcelMonThi_Click(object sender, EventArgs e)
        {
            MonThi[] mt = (MonThi[])dgMonThi.ItemsSource;
            DataTable table = new DataTable();
            for (int i = 0; i < dgMonThi.Columns.Count; i++)
            {
                table.Columns.Add(dgMonThi.Columns[i].Header.ToString());
            }
            for (int i = 0; i < mt.Length; i++)
            {
                DataRow row = table.NewRow();
                row[0] = mt[i].MaMon;
                row[1] = mt[i].TenMon;
                row[2] = mt[i].SoLuongSV;
                row[3] = mt[i].SoPhong;
                row[4] = mt[i].MaCa;
                table.Rows.Add(row);
            }
            ExportFileExcel(table);
        }

        /// <summary>
        /// DANH SÁCH PHÒNG THI
        /// </summary>
        private void tciDanhSachPhongThi_Load()
        {
            dgPhongThi_Load();
            btnChinhSua.IsEnabled = true;
            gbPhongThi.IsEnabled = false;
            btnChinhSua.Click += btnChinhSua_Click;
            btnThemPhongThi.Click += btnThemPhongThi_Click;
            btnXoaPhongThi.Click += btnXoaPhongThi_Click;
            btnCapNhatSucChua.Click += btnCapNhatSucChua_Click;
        }

        private void dgPhongThi_Load()
        {
            dgPhongThi.ItemsSource = null;
            PhongThi[] pt = pt_bll.GetList();
            dgPhongThi.ItemsSource = pt;
        }

        private void btnChinhSua_Click(object sender, EventArgs e)
        {
            if (btnChinhSua.Content.ToString() == "Chỉnh sửa")
            {
                btnChinhSua.Content = "Hoàn tất";
                gbPhongThi.IsEnabled = true;
            }
            else
            {
                btnChinhSua.Content = "Chỉnh sửa";
                gbPhongThi.IsEnabled = false;
                txtMaPhong.Clear(); txtSucChua.Clear();
            }
        }
        
        private void btnThemPhongThi_Click(object sender, EventArgs e)
        {
            PhongThi pt = new PhongThi();
            pt.MaPhong = txtMaPhong.Text.Trim();
            if (!pt_bll.Insert(pt))
            {
                ShowError("Mã phòng không hợp lệ!"); return;
            }
            txtMaPhong.Clear();
            dgPhongThi_Load();
        }

        private void btnXoaPhongThi_Click(object sender, EventArgs e)
        {
            if (dgPhongThi.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa phòng thi này không?", "Xóa phòng thi",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    PhongThi pt = (PhongThi)dgPhongThi.SelectedItem;
                    if (!pt_bll.Delete(pt))
                    {
                        ShowError("Không thể xóa phòng thi này!"); return;
                    }
                    dgPhongThi_Load();
                }
            }
        }

        private void btnCapNhatSucChua_Click(object sender, EventArgs e)
        {
            if (!pt_bll.Update(txtSucChua.Text.Trim()))
            {
                ShowError("Sức chứa không hợp lệ"); return;
            }
            txtSucChua.Clear();
            dgPhongThi_Load();
        }

        /// <summary>
        /// XẾP LỊCH THI
        /// </summary>
        private void tciXepLichThi_Load()
        {
            KyThi kt = kt_bll.GetInfo();
            // Check if there already exists KyThi in database 
            if (kt != null)
            {
                // if yes then load info of that KyThi => can't create another KyThi until deleting current KyThi
                btnTaoKyThiMoi.IsEnabled = false;
                btnXoaKyThi.IsEnabled = btnInLichThi.IsEnabled = true;
                txtNamHoc.Text = kt.NamHoc;
                cboHocKy.Items.Add("Học kỳ 1");
                cboHocKy.Items.Add("Học kỳ 2");
                cboHocKy.Items.Add("Học kỳ 3");
                cboHocKy.SelectedIndex = kt.HocKy - 1;
                dpNgayBatDau.SelectedDate = kt.NgayBatDau;
                dpNgayKetThuc.SelectedDate = kt.NgayKetThuc;
                dgLichThi_Load();
            }
            else
            {
                // if not yet then allow to create new KyThi
                btnTaoKyThiMoi.IsEnabled = true;
                btnXoaKyThi.IsEnabled = btnInLichThi.IsEnabled = false;
            }
            gbThongTinKyThi.IsEnabled = false;
            btnTaoKyThiMoi.Click += btnTaoKyThiMoi_Click;
            btnXoaKyThi.Click += btnXoaKyThi_Click;
            btnChonFileExcelThamGiaThi.Click += btnChonFileExcelThamGiaThi_Click;
            btnImportFileExcelThamGiaThi.Click += btnTaiFileExcelThamGiaThi_Click;
            btnXepLich.Click += btnXepLich_Click;
            btnInLichThi.Click += btnInLichThi_Click;
        }

        private void gbThongTinKyThi_Load()
        {
            DateTime dt = DateTime.Today;
            if (dt.Month >= 9)
            {
                txtNamHoc.Text = dt.Year + " - " + (dt.Year + 1).ToString();
            }
            else
            {
                txtNamHoc.Text = (dt.Year - 1).ToString() + " - " + dt.Year;
            }
            cboHocKy.Items.Add("Học kỳ 1");
            cboHocKy.Items.Add("Học kỳ 2");
            cboHocKy.Items.Add("Học kỳ 3");
            cboHocKy.SelectedIndex = 0;
            dpNgayBatDau.DisplayDateStart = dpNgayKetThuc.DisplayDateStart = dt;
            dpNgayBatDau.SelectedDate = dpNgayKetThuc.SelectedDate = dt;
            btnImportFileExcelThamGiaThi.IsEnabled = false;
        }

        private void gbThongTinKyThi_Clear()
        {
            txtNamHoc.Clear();
            cboHocKy.Items.Clear();
            dpNgayBatDau.SelectedDate = null;
            dpNgayKetThuc.SelectedDate = null;
            txtExcelThamGiaThi.Clear();
        }

        private void dgLichThi_Load()
        {
            dgLichThi.ItemsSource = null;
            LichThi[] lt = lt_bll.GetList("");
            dgLichThi.ItemsSource = lt;
        }

        private void btnTaoKyThiMoi_Click(object sender, EventArgs e)
        {
            if (btnTaoKyThiMoi.Content.ToString() == "Tạo kỳ thi mới")
            {
                // Check whether there already exists data of SinhVien, MonThi, PhongThi
                bool lacking = false;
                string lackinglist = "";
                if (sv_bll.GetList("") == null)
                {
                    lacking = true;
                    lackinglist += "sinh viên, ";
                }
                if (mt_bll.GetList("") == null)
                {
                    lacking = true;
                    lackinglist += "môn thi, ";
                }
                if (pt_bll.GetList() == null)
                {
                    lacking = true;
                    lackinglist += "phòng thi, ";
                }
                if (lacking)
                {
                    ShowError("Thông tin " + lackinglist.Substring(0, lackinglist.Length - 2) + " còn trống!");
                    return;
                }

                btnTaoKyThiMoi.Content = "Hủy";
                gbThongTinKyThi.IsEnabled = true;
                gbThongTinKyThi_Load();
            }
            else
            {
                btnTaoKyThiMoi.Content = "Tạo kỳ thi mới";
                gbThongTinKyThi.IsEnabled = false;
                gbThongTinKyThi_Clear();
            }
        }

        private void btnXoaKyThi_Click(object sender, EventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa kỳ thi hiện tại không?", "Xóa kỳ thi",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Delete ThamGiaThi(*), PhanBoPhongThi(*), MonThi(MaCa), CaThi(*), KyThi(*)
                tgt_bll.Delete("");
                pbpt_bll.Delete("");
                mt_bll.Update("null", "");
                ct_bll.Delete("");
                kt_bll.Delete();

                // Reload data
                dgLichThi_Load();
                tciDanhSachMonThi_Load();

                // Reset controls
                btnTaoKyThiMoi.IsEnabled = true;
                btnXoaKyThi.IsEnabled = btnInLichThi.IsEnabled = false;
                gbThongTinKyThi_Clear();
            }
        }

        private void btnChonFileExcelThamGiaThi_Click(object sender, EventArgs e)
        {
            if (btnChonFileExcelThamGiaThi.Content.ToString() == "Chọn file")
            {
                string filepath = ChooseFileExcel();
                if (filepath != "")
                {
                    txtExcelThamGiaThi.Text = filepath;
                    btnChonFileExcelThamGiaThi.Content = "Xóa file";
                    btnImportFileExcelThamGiaThi.IsEnabled = true;
                }
            }
            else
            {
                txtExcelThamGiaThi.Clear();
                btnChonFileExcelThamGiaThi.Content = "Chọn file";
                btnImportFileExcelThamGiaThi.IsEnabled = false;
            }
        }

        private void btnTaiFileExcelThamGiaThi_Click(object sender, EventArgs e)
        {
            bool overwrite = false;
            MessageBoxResult result = MessageBox.Show("Bạn có muốn thay toàn bộ dữ liệu tham gia thi đã có bằng dữ liệu trong file excel không?",
                "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) { overwrite = true; }
            if (excel_bll.Import(txtExcelThamGiaThi.Text, "ThamGiaThi", overwrite))
            {
                ShowMessage("Import file excel thành công!");
                txtExcelThamGiaThi.Clear();
                btnImportFileExcelThamGiaThi.IsEnabled = false;
            }
            else
            {
                ShowError("Import file excel thất bại!");
            }
        }

        private void btnXepLich_Click(object sender, EventArgs e)
        {
            // Check whether there is data in ThamGiaThi or not
            if (tgt_bll.GetList("") == null)
            {
                // Ask to import excel file
                ShowError("Dữ liệu tham gia thi hiện đang trống!\nVui lòng chọn file excel để nhập dữ liệu.");
                return;
            }

            // Generate list of CaThi
            bool valid = ct_bll.Generate(dpNgayBatDau.SelectedDate.Value, dpNgayKetThuc.SelectedDate.Value);
            if (!valid)
            {
                ShowError("Thời gian kỳ thi không hợp lệ!"); return;
            }

            // Insert into KyThi
            KyThi kt = new KyThi();
            kt.NamHoc = txtNamHoc.Text;
            kt.HocKy = cboHocKy.SelectedIndex + 1;
            kt.NgayBatDau = dpNgayBatDau.SelectedDate.Value;
            kt.NgayKetThuc = dpNgayKetThuc.SelectedDate.Value;
            kt_bll.Insert(kt);

            // Reset controls
            btnTaoKyThiMoi.IsEnabled = gbThongTinKyThi.IsEnabled = false;
            btnTaoKyThiMoi.Content = "Tạo kỳ thi mới";
            btnXoaKyThi.IsEnabled = btnInLichThi.IsEnabled = true;

            // Schedule exam
            if (new ExamSchedule().ScheduleExam())
            {
                ShowMessage("Xếp lịch thi thành công!");
                // Reload data
                dgLichThi_Load();
                tciDanhSachMonThi_Load();
            }
            else
            {
                ShowError("Xếp lịch thi thất bại!");
            }
        }

        private void btnInLichThi_Click(object sender, EventArgs e)
        {
            LichThi[] lt = (LichThi[])dgLichThi.ItemsSource;
            DataTable table = new DataTable();
            for (int i = 0; i < dgLichThi.Columns.Count; i++)
            {
                table.Columns.Add(dgLichThi.Columns[i].Header.ToString());
            }
            for (int i = 0; i < lt.Length; i++)
            {
                DataRow row = table.NewRow();
                row[0] = lt[i].MaCa;
                row[1] = ReverseDateFormat(lt[i].NgayThi.ToString());
                row[2] = lt[i].BuoiThi;
                row[3] = lt[i].MonThi;
                row[4] = lt[i].PhongThi;
                table.Rows.Add(row);
            }
            ExportFileExcel(table);
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

        private void btnDsSinhVien_Click(object sender, EventArgs e)
        {
            tcMenu.SelectedValue = tciDsSinhVien;
            btn_changeFontWeight(ref btnDsSinhVien);
        }

        private void btnDsMonThi_Click(object sender, EventArgs e)
        {
            tcMenu.SelectedValue = tciDsMonThi;
            btn_changeFontWeight(ref btnDsMonThi);
        }

        private void btnDsPhongThi_Click(object sender, EventArgs e)
        {
            tcMenu.SelectedValue = tciDsPhongThi;
            btn_changeFontWeight(ref btnDsPhongThi);
        }

        private void btnXepLichThi_Click(object sender, EventArgs e)
        {
            tcMenu.SelectedValue = tciXepLichThi;
            btn_changeFontWeight(ref btnXepLichThi);
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            tcMenu.SelectedValue = tciDoiMatKhau;
            txtMatKhauHienTai.Clear(); txtMatKhauMoi.Clear(); txtXacNhanMatKhauMoi.Clear();
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

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAdmin_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Thoát",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            e.Cancel = (result == MessageBoxResult.No);
        }

        /// <summary>
        /// SUPPORTING FUNCTIONS
        /// </summary>
        private void ShowError(string error)
        {
            MessageBox.Show(error, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowMessage(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private string ChooseFileExcel()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "All Excel Files|*.xls;*.xlsx;";
            Nullable<bool> result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                return dialog.FileName;
            }
            return "";
        }

        private void ExportFileExcel(DataTable table)
        {
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
        }

        private string ReverseDateFormat(string date)
        {
            string[] str = date.Split('/', '-');
            return str[2] + "/" + str[1] + "/" + str[0];
        }
    }
}