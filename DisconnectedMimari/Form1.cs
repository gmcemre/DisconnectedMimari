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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Server=.; Database=Northwind; Integrated Security=true");
        private void Form1_Load(object sender, EventArgs e)
        {
            UrunListesi();
        }

        private void UrunListesi()
        {
            //Disconnected mimari yöntemi ile veri listeleme yöntemidir.
            SqlDataAdapter adp = new SqlDataAdapter("SELECT * from Urunler", con);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["KategoriID"].Visible = false;
            dataGridView1.Columns["TedarikciID"].Visible = false;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string adi = txtUrunAdi.Text;
            decimal fiyat = nudFiyat.Value;
            decimal stok = nudStok.Value;
            if (txtUrunAdi.Text == "" || nudFiyat.Value.ToString() == null || nudStok.Value.ToString() == null)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.");
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = $"INSERT INTO Urunler (UrunAdi,BirimFiyati,HedefStokDuzeyi) VALUES ('{adi}',{fiyat},{stok})";
                cmd.Connection = con;
                con.Open();
                int etkilenen = cmd.ExecuteNonQuery(); // 'etkilenen' row affected değerini döndürür.
                //Etkilenen 0'dan büyük gelirse sorguda bir hata yoktur ve başarılı bir şekilde kayıt eklenmiştir anlamına geliyor.
                if (etkilenen > 0)
                {
                    MessageBox.Show("Kayıt Başarılı Bir Şekilde Eklendi.");
                    UrunListesi();
                }
                else
                    MessageBox.Show("Kayıt Ekleme Sırasında Hata Meydana Geldi. ");

                con.Close();
            }
        }

        private void btnKategoriler_Click(object sender, EventArgs e)
        {
            KategoriForm kf = new KategoriForm();
            kf.ShowDialog();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //datagridview'dan seçili satırı alma işlemi

            txtUrunAdi.Text = dataGridView1.CurrentRow.Cells["UrunAdi"].Value.ToString();
            nudFiyat.Value = (decimal)dataGridView1.CurrentRow.Cells["BirimFiyati"].Value;
            nudStok.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["HedefStokDuzeyi"].Value);
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            int id = (int)dataGridView1.CurrentRow.Cells["UrunID"].Value;
            string urunAdi = txtUrunAdi.Text;
            decimal fiyat = nudFiyat.Value;
            decimal stok = nudStok.Value;
            SqlCommand cmd = new SqlCommand($"Update Urunler set UrunAdi='{urunAdi}',BirimFiyati={fiyat},HedefStokDuzeyi={stok} where UrunID={id} ", con);

            con.Open();
            try
            {
                int etkilenen = cmd.ExecuteNonQuery();
                if (etkilenen > 0)
                {
                    MessageBox.Show("Ürün başarıyla güncellendi.");
                    UrunListesi();
                }
                else
                {
                    MessageBox.Show("Güncelleme işlemi yapılırken bir hata oluştu.");
                }
                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show("Güncelleme işlemi yapılırken bir hata oluştu." + ex.Message);

            }
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["UrunID"].Value);

                SqlCommand cmd = new SqlCommand($"Delete From Urunler where UrunID={id}", con);
                con.Open();
                try
                {
                    int etk = cmd.ExecuteNonQuery();
                    if (etk > 0)
                    {
                        MessageBox.Show("Kayıt Silindi.");
                        UrunListesi();
                    }
                    else
                    {
                        MessageBox.Show("Kayıt Silinirken Bir Hata Oluştu.");
                    }
                    con.Close();
                }
                catch (Exception)
                {
                    con.Close();
                    MessageBox.Show("Kayıt Silinirken Bir Hata Oluştu.");
                }
            }
        }
    }
}
