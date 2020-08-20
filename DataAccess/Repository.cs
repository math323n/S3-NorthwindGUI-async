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
        /// Initializes a new instance of Repository. Attempts to establish a consnection, and will throw an exception on connection error.
        /// </summary>
        public Repository()
        {
            
        }

        /// <summary>
        /// Initialize the Repository, try get Connection, else catch Exception
        /// </summary>
        /// <returns></returns>
        public virtual async Task InitRepository()
        {
            try
            {
                SqlConnection connection = await Task.Run(() => GetConnection(connectionString) as SqlConnection);
                (bool, Exception) connectionAttemptResult = await TryConnectUsingAsync(connection);
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
        public virtual async Task<DataSet> ExecuteAsync(string query)
        {
            if(string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("Null or whitespace.");
            }
            DataSet resultSet = new DataSet();
            try
            {
                SqlConnection connection = await Task.Factory.StartNew(() => GetConnection(connectionString)) as SqlConnection;
                using(SqlDataAdapter adapter = new SqlDataAdapter(new SqlCommand(query, connection)))
                {
                    await Task.Factory.StartNew(() => adapter.Fill(resultSet));
                }
                return resultSet;
            }
            catch(Exception e)
            {
                throw new Exception("Data access error. See inner exception for details", e);
            }
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
        public async Task<(bool, Exception)> TryConnectUsingAsync(DbConnection connection)
        {
            try
            {
                using(connection)
                {
                    await connection.OpenAsync();
                    await connection.CloseAsync();
                }
                return (true, null);
            }
            catch(Exception e)
            {
                return (false, e);
            }
        }
        #endregion

        #region Repository Methods
        /// <summary>
        /// Extract all data relevant to an order from a data row object, and return an order object.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private static async Task<Order> ExtractOrderFromAsync(DataRow dataRow)
        {
            // Repository object for querying
            Repository repository = new Repository();
            // List for OrderDetails related to the order
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            // Assign DataRows to variables
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

            // Query for getting Order details related to the order
            string query = $"SELECT * FROM [Order Details] WHERE OrderID = {orderID};";
            // Execute the query
            DataSet details = await repository.ExecuteAsync(query);

            await Task.Run(async () => {
                if(details.Tables.Count > 0 && details.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow resultDataRow in details.Tables[0].Rows)
                    {
                        OrderDetail detail = await Task.Factory.StartNew(() => ExtractOrderDetailsFrom(resultDataRow));
                        orderDetails.Add(detail);
                    }
                }
            } );
            // If the query returned any results
           

            // Create the order object
            Order order = new Order(orderID, customerID, employeeID, orderDate, requiredDate, shippedDate,
                shipVia, freight, shipName, shipAddress, shipCity, shipRegion, shipPostalCode, shipCountry, orderDetails);

            // Return the created order object
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

        // <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>A list of all orders</returns>
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            List<Order> orders = new List<Order>();
            string query = "SELECT * FROM Orders";
            DataSet resultSet;
            try
            {
                resultSet = await ExecuteAsync(query);
            }
            catch(Exception)
            {
                throw;
            }
            if(resultSet.Tables.Count > 0 && resultSet.Tables[0].Rows.Count > 0)
            {
                await Task.Run(async () =>
                { 

                foreach(DataRow dataRow in resultSet.Tables[0].Rows)
                {
                    Order order = await ExtractOrderFromAsync(dataRow);
                    orders.Add(order);
                }
                
                });

            }
            return orders;
        }


        public async void AddOrder(Order order)
        {
            string sql = $"INSERT INTO Orders(CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry)  VALUES('{order.CustomerID}', '{order.EmployeeID}', " +
               $"'{order.OrderDate}', '{order.RequiredDate}', '{order.ShippedDate}', '{order.ShipVia}'," +
               $" '{order.Freight}', '{order.ShipName}', '{order.ShipAddress}', '{order.ShipCity}', " +
               $" '{order.ShipRegion}', '{order.ShipPostalCode}', '{order.ShipCountry}')";
            DataSet resultSet;
            try
            {
                resultSet = await ExecuteAsync(sql);
            }
            catch(Exception)
            {
                throw;
            }
        }
        #endregion

    }
}