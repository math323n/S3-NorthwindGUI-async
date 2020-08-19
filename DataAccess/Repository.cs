using Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Repository
    {

        #region Fields and constants
        const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=5;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        #endregion


        #region Constructors
        /// <summary>
        /// Initializes a new instance of Repository. Attempts to establish a connection, and will throw an exception on connection error.
        /// </summary>
        public Repository()
        {
            try
            {
                SqlConnection connection = GetConnection(connectionString) as SqlConnection;
                (bool, Exception) connectionAttemptResult = TryConnectUsing(connection);
            }
            catch(Exception e)
            {
                throw new Exception("Data access error. See inner exception for details", e);
            }
        }
        #endregion


        #region Helper Methods
        /// <summary>
        /// Executes the provided SQL statement and returns data wrapped in a data set, if any. 
        /// </summary>
        /// <param name="sql">The SQL statement to execute.</param>
        /// <returns>A <see cref="DataSet"/> wrapping any returned data.</returns>
        /// <exception cref="ArgumentException"/>
        public DataSet Execute(string query)
        {
            if(string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("Null or whitespace.");
            }
            DataSet resultSet = new DataSet();
           
                SqlConnection connection = GetConnection(connectionString) as SqlConnection;
                using(SqlDataAdapter adapter = new SqlDataAdapter(new SqlCommand(query, connection)))
                {
                    adapter.Fill(resultSet);
                }
                return resultSet;
          
        }
        /// <summary>
        /// Creates a connection based on the name of the input parameter connection string.
        /// </summary>
        /// <param name="connectionString">The name of the connection string.</param>
        /// <returns>A new connection.</returns>
        private static DbConnection GetConnection(string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        /// <summary>
        /// Attempts to connect to a data source using the provided connection.
        /// </summary>
        /// <param name="connection">The connection to use.</param>
        /// <returns>True, if the connection could be established, false otherwise.</returns>
        public (bool, Exception) TryConnectUsing(DbConnection connection)
        {
            try
            {
                using(connection)
                {
                    connection.Open();
                    connection.Close();
                }
                return (true, null);
            }
            catch(Exception e)
            {
                return (false, e);
            }
        }

        /// <summary>
        /// Extract all data relevant to an order from a data row object, and return an order object.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private static Order ExtractOrderFrom(DataRow dataRow)
        {
            int orderID = (int)dataRow["OrderID"];
            string customerID = (string)dataRow["CustomerID"];
            int employeeID = (int)dataRow["EmployeeID"];
            DateTime orderDate = (DateTime)dataRow["OrderDate"];
            DateTime requiredDate = (DateTime)dataRow["RequiredDate"];
            DateTime shippedDate = Convert.IsDBNull(dataRow["ShippedDate"]) ? DateTime.MinValue : (DateTime)dataRow["ShippedDate"];
            int shipVia = (int)dataRow["ShipVia"];
            decimal freight = (decimal)dataRow["Freight"];
            string shipName = Convert.IsDBNull(dataRow["ShipName"]) ? null : (string)dataRow["ShipName"];
            string shipAddress = Convert.IsDBNull(dataRow["ShipAddress"]) ? null : (string)dataRow["ShipAddress"];
            string shipCity = Convert.IsDBNull(dataRow["ShipCity"]) ? null : (string)dataRow["ShipCity"];
            string shipRegion = Convert.IsDBNull(dataRow["ShipRegion"]) ? null : (string)dataRow["ShipRegion"];
            string shipPostalCode = Convert.IsDBNull(dataRow["ShipPostalCode"]) ? null : (string)dataRow["ShipPostalCode"];
            string shipCountry = Convert.IsDBNull(dataRow["ShipCountry"]) ? null : (string)dataRow["ShipCountry"];


            string query = $"SELECT * FROM [Order Details] WHERE OrderID = {orderID}";
            Repository repository = new Repository();
            DataSet orderDetails = repository.Execute(query);
            List<OrderDetail> orderDetailList = new List<OrderDetail>();
            if(orderDetails.Tables.Count > 0 && orderDetails.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow orderDetailsDataRow in orderDetails.Tables[0].Rows)
                {
                    OrderDetail orderDetail = ExtractOrderDetailsFrom(orderDetailsDataRow);
                    orderDetailList.Add(orderDetail);
                }
            }
            Order order = new Order(orderID, customerID, employeeID, orderDate, requiredDate, shippedDate, shipVia, freight, shipName, shipAddress, shipCity, shipRegion, shipPostalCode, shipCountry, orderDetailList);

            return order;
        }


        /// <summary>
        /// Extract all data relevant to an order from a data row object, and return an order object.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private async Task<Order> ExtractOrderFromASync(DataRow dataRow)
        {
            int orderID = (int)dataRow["OrderID"];
            string customerID = (string)dataRow["CustomerID"];
            int employeeID = (int)dataRow["EmployeeID"];
            DateTime orderDate = (DateTime)dataRow["OrderDate"];
            DateTime requiredDate = (DateTime)dataRow["RequiredDate"];
            DateTime shippedDate = Convert.IsDBNull(dataRow["ShippedDate"]) ? DateTime.MinValue : (DateTime)dataRow["ShippedDate"];
            int shipVia = (int)dataRow["ShipVia"];
            decimal freight = (decimal)dataRow["Freight"];
            string shipName = Convert.IsDBNull(dataRow["ShipName"]) ? null : (string)dataRow["ShipName"];
            string shipAddress = Convert.IsDBNull(dataRow["ShipAddress"]) ? null : (string)dataRow["ShipAddress"];
            string shipCity = Convert.IsDBNull(dataRow["ShipCity"]) ? null : (string)dataRow["ShipCity"];
            string shipRegion = Convert.IsDBNull(dataRow["ShipRegion"]) ? null : (string)dataRow["ShipRegion"];
            string shipPostalCode = Convert.IsDBNull(dataRow["ShipPostalCode"]) ? null : (string)dataRow["ShipPostalCode"];
            string shipCountry = Convert.IsDBNull(dataRow["ShipCountry"]) ? null : (string)dataRow["ShipCountry"];


            string query = $"SELECT * FROM [Order Details] WHERE OrderID = {orderID}";
            Repository repository = new Repository();
            DataSet orderDetails =  await Task.Run(() => repository.Execute(query));
            List<OrderDetail> orderDetailList = new List<OrderDetail>();
            if(orderDetails.Tables.Count > 0 && orderDetails.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow orderDetailsDataRow in orderDetails.Tables[0].Rows)
                {
                    OrderDetail orderDetail = await Task.Run(() => ExtractOrderDetailsFrom(orderDetailsDataRow));
                    orderDetailList.Add(orderDetail);
                }
            }
            Order order = new Order(orderID, customerID, employeeID, orderDate, requiredDate, shippedDate, shipVia, freight, shipName, shipAddress, shipCity, shipRegion, shipPostalCode, shipCountry, orderDetailList);

            return order;
        }
        /// <summary>
        /// Extract all data relevant to an order detail from a dat row object, and return an order detail object.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private static OrderDetail ExtractOrderDetailsFrom(DataRow dataRow)
        {
            int orderID = (int)dataRow["OrderID"];
            int productID = (int)dataRow["ProductID"];
            decimal unitPrice = (decimal)dataRow["UnitPrice"];
            short quantity = (short)dataRow["Quantity"];
            float discount = (float)dataRow["Discount"];

            OrderDetail orderDetails = new OrderDetail(orderID, productID, unitPrice, quantity, discount);

            return orderDetails;
        }
        #endregion


        #region Repository Methods
        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>A list of all orders</returns>
        public async Task<List<Order>> GetAllOrdersASync()
        {
            List<Order> orders = new List<Order>();
            string query = "SELECT * FROM Orders";
            DataSet resultSet;
            try
            {
                resultSet = Execute(query);
            }
            catch(Exception)
            {
                throw;
            }

            if(resultSet.Tables.Count > 0 && resultSet.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow dataRow in resultSet.Tables[0].Rows)
                {
                    Order order = await ExtractOrderFromASync(dataRow);
                    orders.Add(order);
                }
            }
            return orders;
        }
        #endregion

        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>A list of all orders</returns>
        public List<OrderDetail> GetAllOrderDetails()
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            string query = "SELECT * FROM [Order Details]";
            DataSet resultSet;
            try
            {
                resultSet = Execute(query);
            }
            catch(Exception)
            {
                throw;
            }

            if(resultSet.Tables.Count > 0 && resultSet.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow dataRow in resultSet.Tables[0].Rows)
                {
                    OrderDetail orderDetail = ExtractOrderDetailsFrom(dataRow);
                    orderDetails.Add(orderDetail);
                }
            }
            return orderDetails;
        }

        public void AddOrder(Order order)
        {
            string sql = $"INSERT INTO Orders(CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry)  VALUES('{order.CustomerID}', '{order.EmployeeID}', " +
               $"'{order.OrderDate}', '{order.RequiredDate}', '{order.ShippedDate}', '{order.ShipVia}'," +
               $" '{order.Freight}', '{order.ShipName}', '{order.ShipAddress}', '{order.ShipCity}', " +
               $" '{order.ShipRegion}', '{order.ShipPostalCode}', '{order.ShipCountry}')";
            DataSet resultSet;
            try
            {
                resultSet = Execute(sql);
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}