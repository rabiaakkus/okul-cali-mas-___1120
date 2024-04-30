using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace okul__calısşaması1120
{
    public partial class TümOgrenciEkleForm : Form
    {
        string baglanti = "Server=localhost;Database=hnk_okul;Uid=root;Pwd=;";
        public TümOgrenciEkleForm()
        {
            InitializeComponent();
        }

        private void TümOgrenciEkleForm_Load(object sender, EventArgs e)
        {
            DgwDoldur();
            CmbDoldur();
        }

        void DgwDoldur()
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "SELECT * FROM ogrenci;";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();


                da.Fill(dt);
                dgwOgrenciler.DataSource = dt;
            }
        }
    

        void CmbDoldur()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Cinsiyet", typeof(string));
            dt.Columns.Add("KisaAd", typeof(string));

            dt.Rows.Add("Kız", "K");
            dt.Rows.Add("Erkek", "E");

            cmbCinsiyet.DataSource = dt;

            cmbCinsiyet.DisplayMember = "Cinsiyet";   
            cmbCinsiyet.ValueMember = "tur";


        }

        private void dgwOgrenciler_SelectionChanged(object sender, EventArgs e)
        {
            if (dgwOgrenciler.SelectedRows.Count > 0)
            {
                txtAd.Text = dgwOgrenciler.SelectedRows[0].Cells["ad"].Value.ToString();
                txtSoyad.Text = dgwOgrenciler.SelectedRows[0].Cells["sanatci"].Value.ToString();
                dtTarih.Value = Convert.ToDateTime(dgwOgrenciler.SelectedRows[0].Cells["dtarih"].Value);
                cbMezun.Checked = Convert.ToBoolean(dgwOgrenciler.SelectedRows[0].Cells["mezun"].Value);
                cmbCinsiyet.SelectedValue =  dgwOgrenciler.SelectedRows[0].Cells["cinsiyet"].Value.ToString();
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgwOgrenciler.SelectedRows[0];
            int satirId = Convert.ToInt32(dr.Cells["okul_no"].Value);

            DialogResult cevap = MessageBox.Show("öğrenciyi  Silmek İstediğinizden Emin Misiniz?",
                                "öğrenciyi Sil",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Error);

            if (cevap == DialogResult.Yes)
            {

                string sorgu = "DELETE FROM ogrenci where okul_no = @satirid;";

                using (MySqlConnection baglan = new MySqlConnection(baglanti))
                {
                    baglan.Open();
                    MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                    cmd.Parameters.AddWithValue("@satirid", satirId);
                    cmd.ExecuteNonQuery();

                    DgwDoldur(); //tekrar doldurur
                }


            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            string sorgu = "UPDATE ogrenci SET ad =@ad, soyad=@soyad, dtarih=@dtarih," +
                " cinsiyet=@cinsiyet ,mezun_durum=@mezun_durum  WHERE okul_no = @satirid;";
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                cmd.Parameters.AddWithValue("@ad", txtAd.Text);
                cmd.Parameters.AddWithValue("@soyad", txtSoyad.Text);
                cmd.Parameters.AddWithValue("@dtarih", dtTarih.Value);
                cmd.Parameters.AddWithValue("@cinsiyet", cmbCinsiyet.SelectedValue);
                cmd.Parameters.AddWithValue("@mezun", cbMezun.Checked);

                int id = Convert.ToInt32(dgwOgrenciler.SelectedRows[0].Cells["okul_no"].Value);
                cmd.Parameters.AddWithValue("@satirid", id);

                cmd.ExecuteNonQuery();

                DgwDoldur();

            }
        }
    }

}
