using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject
{
    public class CatalogViewModel
    {
        public ObservableCollection<Category> Categories { get; set; } 
        public ObservableCollection<Product> Products { get; set; }
        public Category SelectedCategory { get; set; } 
        public CatalogViewModel()
        {
            Categories = new ObservableCollection<Category>();
            Products = new ObservableCollection<Product>();
            LoadCategories();
        }
        private void LoadCategories()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Name FROM Categories";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Categories.Add(new Category
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
            }
        }
        public void LoadProducts()
        {
            if (SelectedCategory == null) return;

            Products.Clear();

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Name, ImagePath, Price, Weight, Description FROM Products WHERE CategoryId = @CategoryId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryId", SelectedCategory.Id);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Products.Add(new Product
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        ImagePath = reader.GetString(2),
                        Price = reader.GetDecimal(3),
                        Weight = reader.GetDouble(4),
                        Description = reader.GetString(5)
                    });
                }
            }
        }
    }
}
