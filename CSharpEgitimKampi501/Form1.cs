using CSharpEgitimKampi501.Dtos;
using Dapper;
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

namespace CSharpEgitimKampi501
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection("Server=YIGITATAMANPC;Initial Catalog=EgitimKampi501Db;Integrated Security=True");

        private async void btnList_Click(object sender, EventArgs e)
        {
            string query = "Select * From TblProduct";
            var values = await connection.QueryAsync<ResultProductDto>(query);
            dataGridView1.DataSource = values;
        }
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            string query= "insert into TblProduct(ProductName,ProductCategory,ProductStock,ProductPrice) values(@ProductName,@ProductCategory,@ProductStock,@ProductPrice)";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductName", txtProductName.Text);
            parameters.Add("@ProductCategory", txtProductCategory.Text);
            parameters.Add("@ProductStock", txtProductStock.Text);
            parameters.Add("@ProductPrice", txtProductPrice.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Yeni Kitap Ekleme İşlemi Başarılı!");
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            string query = "delete from TblProduct where ProductId=@ProductId";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", txtProductId.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Kitap Silme İşlemi Başarılı!");
        }

        private async void btnUpdate_Click_1(object sender, EventArgs e)
        {

            string query = "update TblProduct set ProductName=@ProductName,ProductCategory=@ProductCategory,ProductStock=@ProductStock,ProductPrice=@ProductPrice where ProductId=@ProductId";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductName", txtProductName.Text);
            parameters.Add("@ProductCategory", txtProductCategory.Text);
            parameters.Add("@ProductStock", txtProductStock.Text);
            parameters.Add("@ProductPrice", txtProductPrice.Text);
            parameters.Add("@ProductId", txtProductId.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Kitap Güncelleme İşlemi Başarılı!", "Güncelleme", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string query1 = "Select count(*) From TblProduct";
            var productTotalCount = await connection.QueryAsync<int>(query1);
            lblTotalProductCount.Text = productTotalCount.FirstOrDefault().ToString();

            string query2 = "Select ProductName From TblProduct Where ProductPrice =(Select Max(ProductPrice) From TblProduct)";
            var maxPriceProductName = await connection.QueryFirstOrDefaultAsync<string>(query2);
            lblmaxPriceProductName.Text = maxPriceProductName.ToString();

            string query3 = "Select Count(Distinct(ProductCategory)) From TblProduct";
            var DistinctProductCount = await connection.QueryFirstOrDefaultAsync<int>(query3);
            lblDistinctProductCount.Text = DistinctProductCount.ToString();
        }
    }
}
