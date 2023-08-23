using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSShop
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            customizeDesing();
        }

        private void customizeDesing()
        {
            panelAdmin.Visible = false;
        }

        private void hideSubMenu()
        {
            if(panelAdmin.Visible)
            {
                panelAdmin.Visible = false;
            }
        }

        private void showSubMenu(Panel subMenu)
        {
            if (!subMenu.Visible)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            showSubMenu(panelAdmin);
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            openChildForm(new FrmUsers());
            hideSubMenu();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            openChildForm(new FrmProducts());
            hideSubMenu();
        }

        private Form activeForm = null;
        private void openChildForm(Form children)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = children;
            children.TopLevel = false;
            children.FormBorderStyle = FormBorderStyle.None;
            children.Dock   = DockStyle.Fill;
            panelView.Controls.Add(children);
            panelView.Tag = children;
            children.BringToFront();
            children.Show();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            openChildForm(new FrmSales());
            hideSubMenu();
        }

        private void btnProvider_Click(object sender, EventArgs e)
        {
            openChildForm(new FrmProviders());
            hideSubMenu();
        }
    }
}
