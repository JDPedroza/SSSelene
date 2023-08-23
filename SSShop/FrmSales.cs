using SeleneShop.Config;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace SSShop
{
    public partial class FrmSales : Form
    {
        public FrmSales()
        {
            InitializeComponent();
            LoadUsers();
            LoadProducts();
            cbProduct.SelectedIndexChanged += UpdatePriceLabels;
            cbTax.SelectedIndexChanged += UpdatePriceLabels;
            tbQuantity.TextChanged += UpdatePriceLabels;
        }

        private void UpdatePriceLabels(object sender, EventArgs e)
        {
            // Obtiene los valores seleccionados y la cantidad ingresada
            string selectedProduct = cbProduct.SelectedItem?.ToString();
            int quantity = 0;
            decimal taxPercentage = 0;

            if (int.TryParse(tbQuantity.Text, out int parsedQuantity))
            {
                quantity = parsedQuantity;
            }

            string selectedTax = cbTax.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedTax))
            {
                // Remueve el símbolo "%" y convierte a decimal dividiendo por 100
                selectedTax = selectedTax.Replace("%", "");
                if (decimal.TryParse(selectedTax, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal parsedTax))
                {
                    taxPercentage = parsedTax / 100;
                }
            }

            // Calcula los precios
            decimal finalPrice = CalculateFinalPrice(selectedProduct, quantity, taxPercentage);
            decimal totalPrice = GetProductValueByName(selectedProduct) * quantity;

            // Actualiza los Labels con los resultados
            labelSubtotal.Text = totalPrice.ToString("C");
            labelTotal.Text = finalPrice.ToString("C");
        }

        private Dictionary<string, byte> usersDictionary = new Dictionary<string, byte>();
        //private Dictionary<string, byte> productsDictionary = new Dictionary<string, byte>();
        private Dictionary<string, ProductInfo> productsDictionary = new Dictionary<string, ProductInfo>();

        public class ProductInfo
        {
            public byte ProductId { get; set; }
            public decimal Value { get; set; }
        }

        private void LoadUsers()
        {
            string selectQuery = "SELECT user_id, username FROM TBL_users";
            using (SqlCommand command = new SqlCommand(selectQuery, ClsConnection.StartConnection()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        byte userId = Convert.ToByte(reader["user_id"]);
                        string userName = reader["username"].ToString();

                        usersDictionary.Add(userName, userId);
                        cbUser.Items.Add(userName);
                    }
                }
            }
        }

        private void LoadProducts()
        {
            string selectQuery = "SELECT product_id, product_name, value FROM TBL_products";
            using (SqlCommand command = new SqlCommand(selectQuery, ClsConnection.StartConnection()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        byte productId = Convert.ToByte(reader["product_id"]);
                        string productName = reader["product_name"].ToString();
                        decimal productValue = Convert.ToDecimal(reader["value"]);

                        ProductInfo productInfo = new ProductInfo
                        {
                            ProductId = productId,
                            Value = productValue
                        };

                        productsDictionary.Add(productName, productInfo);
                        cbProduct.Items.Add(productName);
                    }
                }
            }
        }

        private byte GetUserIdByName(string userName)
        {
            if (usersDictionary.ContainsKey(userName))
            {
                return usersDictionary[userName];
            }
            return 0; // Valor por defecto si no se encuentra
        }

        private byte GetProductIdByName(string product_name)
        {
            if (productsDictionary.ContainsKey(product_name))
            {
                return productsDictionary[product_name].ProductId;
            }
            return 0; // Valor por defecto si no se encuentra
        }

        private decimal GetProductValueByName(string productName)
        {
            if (productsDictionary.ContainsKey(productName))
            {
                return productsDictionary[productName].Value;
            }
            return 0; // Valor por defecto si no se encuentra
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string sql_insert = "INSERT INTO TBL_Sales (product_id, quantity,tax,sale_date,seller_id,final_price) VALUES (@product_id, @quantity,@tax,@sale_date,@seller_id,@final_price)";
            SqlCommand cmd = new SqlCommand(sql_insert, ClsConnection.StartConnection());
            #region ComboBox
            object productName = cbProduct.SelectedItem;
            if (productName != null)
            {
                byte selectedProductId = GetProductIdByName(productName.ToString());
                cmd.Parameters.AddWithValue("@product_id", selectedProductId);
            }
            else
            {
                MessageBox.Show("Debe seleccionar un vendedor");
            }
            object username = cbUser.SelectedItem;
            if (username != null)
            {
                byte selectedUserID = GetUserIdByName(username.ToString());
                cmd.Parameters.AddWithValue("@seller_id", selectedUserID);
            }
            else
            {
                MessageBox.Show("Debe seleccionar un vendedor");
            }

            object tax = cbTax.SelectedItem;
            decimal taxPercentage = 0;
            if (tax != null)
            {
                string taxPercentageText = tax.ToString();
                taxPercentage = Convert.ToDecimal(taxPercentageText.Trim('%')) / 100;
                cmd.Parameters.AddWithValue("@tax", taxPercentage);
            }
            else
            {
                MessageBox.Show("Debe seleccionar un impuesto");
            }
            #endregion
            int quantity = Convert.ToInt32(tbQuantity.Text);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            DateTime saleDate = dtpSaleDate.Value;
            cmd.Parameters.AddWithValue("@sale_date", saleDate);
            decimal finalPrice = CalculateFinalPrice(productName.ToString(), quantity, taxPercentage);
            cmd.Parameters.AddWithValue("@final_price", finalPrice);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Datos agregados correctamente");
            ClearForm();
        }
        
        private decimal CalculateFinalPrice(string product, int quantity, decimal taxPercentage)
        {
            // Calcula el precio final considerando la cantidad y el porcentaje de impuestos
            decimal unitPrice = GetProductValueByName(product);
            decimal totalPrice = unitPrice * quantity;
            decimal totalTax = totalPrice * taxPercentage;
            decimal finalPrice = totalPrice + totalTax;
            return finalPrice;
        }
        private void ClearForm()
        {
            // Limpia los valores de los controles
            tbQuantity.Text = string.Empty;
            dtpSaleDate.Value = DateTime.Now; // Ajusta según tu DateTimePicker

            // Limpia los resultados en los Labels
            labelSubtotal.Text = "$ 0";
            labelTotal.Text = "$ 0";
        }

    }
}
