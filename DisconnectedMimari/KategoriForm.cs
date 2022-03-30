using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisconnectedMimari
{
    public partial class KategoriForm : Form
    {
        public KategoriForm()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Server=.; Database=Northwind ; Integrated Security=true");
        private void KategoriForm_Load(object sender, EventArgs e)
        {
            KategoriListele();
        }

        private void KategoriListele()
        {
            SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM Kategoriler", con);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string adi = txtKategoriAdi.Text;
            string tanimi = txtKategoriTanimi.Text;

            if (adi == "" || tanimi == "")
            {
                MessageBox.Show("Lütfen boş alanları doldurunuz.");
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = $"INSERT INTO Kategoriler (KategoriAdi,Tanimi) VALUES ('{adi}','{tanimi}')";
                cmd.Connection = con;
                con.Open();
                int etkilenen = cmd.ExecuteNonQuery();
                if (etkilenen > 0)
                {
                    MessageBox.Show("Kayıt Başarıyla Eklendi.");
                    KategoriListele();
                }
                else
                {
                    MessageBox.Show("Kayıt Ekleme Sırasında Bir Hata Oluştu.");
                }
                con.Close();
            }
        }
    }
}
