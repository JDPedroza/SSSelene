using SeleneShop.Config;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace SSShop
{
    public partial class FrmUsers : Form
    {
        public FrmUsers()
        {
            InitializeComponent();
            CustomizeDesing();
        }

        private void CustomizeDesing()
        {
            panelAdd.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmUsers_Load(object sender, EventArgs e)
        {
            ClsConnection.StartConnection();
            dgvUsers.DataSource = Index();
        }

        public DataTable Index()
        {
            ClsConnection.StartConnection();
            DataTable dataTable = new DataTable();
            string sql = "SELECT * FROM TBL_users";
            SqlCommand cmd = new SqlCommand(sql, ClsConnection.StartConnection());
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);

            return dataTable;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            panelAdd.Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClsConnection.StartConnection();
            string sql_insert = "INSERT INTO TBL_users (username, password ,user_type) VALUES (@username, @password, @user_type)";
            SqlCommand cmd = new SqlCommand(sql_insert, ClsConnection.StartConnection());
            cmd.Parameters.AddWithValue("@username", tbxUsername.Text);
            cmd.Parameters.AddWithValue("@password", tbxPassword.Text);
            cmd.Parameters.AddWithValue("@user_type", tbxUsertype.Text);

            cmd.ExecuteNonQuery();

            MessageBox.Show("Datos agregados correctamente");
            panelAdd.Visible=false;
            dgvUsers.DataSource = Index();
        }

        private void btnDialogAdd_Click(object sender, EventArgs e)
        {
            panelAdd.Visible = true;
        }
    }
}
