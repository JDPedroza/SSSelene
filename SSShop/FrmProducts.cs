using SeleneShop.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SSShop
{
    public partial class FrmProducts : Form
    {

        private int position = -1;
        private Image selectedImage;
        private int idTable = 0;
        private Dictionary<string, byte> providerDictionary = new Dictionary<string, byte>();

        public FrmProducts()
        {
            InitializeComponent();
            CustomizeStyle();
            LoadProviders();
        }

        
        #region Load
        private void LoadProviders()
        {
            string selectQuery = "SELECT provider_id, provider_name FROM TBL_providers";
            using (SqlCommand command = new SqlCommand(selectQuery, ClsConnection.StartConnection()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        byte providerId = Convert.ToByte(reader["provider_id"]);
                        string providerName = reader["provider_name"].ToString();

                        providerDictionary.Add(providerName, providerId);
                        cbProviders.Items.Add(providerName);
                    }
                }
            }
        }
        private void CustomizeStyle()
        {
            panelAdd.Visible = false;
        }
        private void FrmProducts_Load(object sender, EventArgs e)
        {
            ClsConnection.StartConnection();
            dgvProducts.DataSource = Index();
        }
        #endregion

        #region Functions
        private byte GetProviderIdByName(string providerName)
        {
            if (providerDictionary.ContainsKey(providerName))
            {
                return providerDictionary[providerName];
            }
            return 0; // Valor por defecto si no se encuentra
        }
        private byte[] ConvertImageToByteArray(Image image)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return memoryStream.ToArray();
            }
        }
        #endregion

        #region Main
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            clear();
            panelAdd.Visible = true;
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (position > -1)
            {
                panelAdd.Visible = true;
            }
            else
            {
                MessageBox.Show("Seleccione el registro a modificar");
            }
        }
        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            position = dgvProducts.CurrentRow.Index;
            idTable = Convert.ToInt32(dgvProducts[0, position].Value);
            tbx_product_name.Text = dgvProducts[1, position].Value.ToString();
            tbx_product_code.Text = dgvProducts[2, position].Value.ToString();
            tbx_value.Text = dgvProducts[3, position].Value.ToString();
            //datetbx_product_name.Text = dgvProducts[4, position].Value.ToString();
            //cbProviders.Select = dgvProducts[5, position].Value.ToString();

        }
        private void clear()
        {
            tbx_product_name.Text = string.Empty;
            tbx_product_code.Text = string.Empty;
            tbx_value.Text = string.Empty;
            //datetbx_product_name.Text = dgvProducts[4, position].Value.ToString();
            //tbx_provider_id.Text = string.Empty;
        }
        #endregion

        #region DB
        public DataTable Index()
        {
            ClsConnection.StartConnection();
            DataTable dataTable = new DataTable();
            string sql = "SELECT * FROM TBL_products";
            SqlCommand cmd = new SqlCommand(sql, ClsConnection.StartConnection());
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);

            return dataTable;
        }
        #endregion

        #region Dialog
        private void btnCancel_Click(object sender, EventArgs e)
        {
            panelAdd.Visible = false;
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (idTable == 0)
            {
                ClsConnection.StartConnection();
                string sql_insert = "INSERT INTO TBL_products (product_name, product_code, value, creation_date, provider_id, photo) VALUES (@product_name, @product_code, @value, @creation_date, @provider_id, @photo)";
                SqlCommand cmd = new SqlCommand(sql_insert, ClsConnection.StartConnection());

                #region textBox
                cmd.Parameters.AddWithValue("@product_name", tbx_product_name.Text);
                cmd.Parameters.AddWithValue("@product_code", tbx_product_code.Text);
                cmd.Parameters.AddWithValue("@value", Convert.ToDecimal(tbx_value.Text));
                #endregion
                #region pickers
                DateTime creation_date = dtp_creation_date.Value;
                cmd.Parameters.AddWithValue("@creation_date", creation_date);
                #endregion
                #region comboBox
                object provider_name = cbProviders.SelectedItem;
                if (provider_name != null)
                {
                    byte selectedProviderId = GetProviderIdByName(provider_name.ToString());
                    cmd.Parameters.AddWithValue("@provider_id", selectedProviderId);
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un proveedor");
                }
                #endregion
                #region pictureBox
                if (selectedImage != null)
                {
                    byte[] imageBytes = ConvertImageToByteArray(selectedImage);
                    cmd.Parameters.AddWithValue("@photo", imageBytes);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@photo", null);
                }
                #endregion
                cmd.ExecuteNonQuery();
                MessageBox.Show("Datos agregados correctamente");
            }
            else
            {
                string sql_update = "UPDATE TBL_products SET " +
                              "product_name = @product_name, " +
                              "product_code = @product_code, " +
                              "value = @value, " +
                              "creation_date = @creation_date, " +
                              "provider_id = @provider_id, " +
                              "photo = @photo " +
                              "WHERE product_id = @product_id";
                SqlCommand cmd = new SqlCommand(sql_update, ClsConnection.StartConnection());

                #region textBox
                cmd.Parameters.AddWithValue("@product_name", tbx_product_name.Text);
                cmd.Parameters.AddWithValue("@product_code", tbx_product_code.Text);
                cmd.Parameters.AddWithValue("@value", Convert.ToDecimal(tbx_value.Text));
                #endregion
                #region pickers
                DateTime creation_date = dtp_creation_date.Value;
                cmd.Parameters.AddWithValue("@creation_date", creation_date);
                #endregion
                #region comboBox
                object provider_name = cbProviders.SelectedItem;
                if (provider_name != null)
                {
                    byte selectedProviderId = GetProviderIdByName(provider_name.ToString());
                    cmd.Parameters.AddWithValue("@provider_id", selectedProviderId);
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un proveedor");
                }
                #endregion
                #region pictureBox
                if (selectedImage != null)
                {
                    byte[] imageBytes = ConvertImageToByteArray(selectedImage);
                    cmd.Parameters.AddWithValue("@photo", imageBytes);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@photo", null);
                }
                #endregion
                byte idBase = (byte)idTable;
                cmd.Parameters.AddWithValue("@product_id", idBase);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Datos actualizados correctamente");
            }
            panelAdd.Visible = false;
            dgvProducts.DataSource = Index();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM TBL_products WHERE product_id = @product_id";

            using (SqlCommand command = new SqlCommand(query, ClsConnection.StartConnection()))
            {
                command.Parameters.AddWithValue("@product_id", idTable);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Se elimino correctamente el producto");
                    panelAdd.Visible = false;
                    dgvProducts.DataSource = Index();
                }
            }
        }
        private void pbPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.png;*.gif;*.bmp|Todos los archivos|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string imagePath = openFileDialog.FileName;
                selectedImage = Image.FromFile(imagePath);
                pbPhoto.Image = selectedImage;
            }
        }
        #endregion
    }
}
