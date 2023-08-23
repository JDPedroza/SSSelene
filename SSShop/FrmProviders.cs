using SeleneShop.Config;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SSShop
{
    public partial class FrmProviders : Form
    {
        public FrmProviders()
        {
            InitializeComponent();
            CustomizeDesing();
        }

        #region Constructor
        private void CustomizeDesing()
        {
            panelAdd.Visible = false;
        }
        private void FrmProviders_Load(object sender, EventArgs e)
        {
            dgvProviders.DataSource = Index();
        }
        #endregion

        #region Database
        public DataTable Index()
        {
            ClsConnection.StartConnection();
            DataTable dataTable = new DataTable();
            string sql = "SELECT * FROM TBL_providers";
            SqlCommand cmd = new SqlCommand(sql, ClsConnection.StartConnection());
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);

            return dataTable;
        }
        private void btnConfirm_Click_1(object sender, EventArgs e)
        {
            string sql_insert = "INSERT INTO TBL_providers (provider_name,nit,phone,mobile,email) VALUES (@provider_name,@nit,@phone,@mobile,@email)";
            SqlCommand cmd = new SqlCommand(sql_insert, ClsConnection.StartConnection());
            //recuperamos todos los datos
            cmd.Parameters.AddWithValue("@provider_name", tbProviderName.Text);
            cmd.Parameters.AddWithValue("@nit", tbNit.Text);
            cmd.Parameters.AddWithValue("@phone", tbPhone.Text);
            cmd.Parameters.AddWithValue("@mobile", tbMobile.Text);
            cmd.Parameters.AddWithValue("@email", tbEmail.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Datos agregados correctamente");
            panelAdd.Visible = false;
            dgvProviders.DataSource = Index();
            clear();
        }
        
        #endregion

        #region Main
        private void btnAdd_Click(object sender, EventArgs e)
        {
            panelAdd.Visible = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Dialog
        private void btnCancel_Click(object sender, EventArgs e)
        {
            panelAdd.Visible = false;
            clear();
        }
        #endregion

        #region Functions
        private void clear()
        {
            tbProviderName.Text = string.Empty;
            tbNit.Text = string.Empty;
            tbPhone.Text = string.Empty;
            tbMobile.Text = string.Empty;
            tbEmail.Text = string.Empty;
        }
        #endregion

    }
}
