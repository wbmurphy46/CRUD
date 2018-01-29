using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace supplierTableCreateReadUpdateDelete
{
    class Program
    {
        static void Main(string[] args)
        {
            char userInput;

            try
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("This is the menu to CREATE, READ, UPDATE, or DELETE rows\nfrom the Supplier table.\n" +
                    "\n Press 1 to CREATE" +
                    "\n Press 2 to READ all rows" +
                    "\n Press 3 to READ A SPECIFIC ROW" +
                    "\n Press 4 to UPDATE" +
                    "\n Press 5 to DELETE" +
                    "\n Press 0 to exit");

                    userInput = Console.ReadKey(true).KeyChar;
                    Console.Clear();

                    switch (userInput)
                    {
                        case '1':
                            Create();
                            break;
                        case '2':
                            Read();
                            break;
                        case '3':
                            ReadRowBySupplierID();
                            break;
                        case '4':
                            Update();
                            break;
                        case '5':
                            Delete();
                            break;
                    }
                    if (userInput != '0')
                    {
                        Console.WriteLine("\n Press enter to continue:");
                        Console.ReadKey(true);
                    }
                }
                while (userInput != '0');
            }

            catch (Exception ex)
            {
                StreamWriter writeToLog = null;
                try
                {
                    writeToLog = new StreamWriter("errorlog.txt", true);
                    writeToLog.WriteLine(DateTime.Now + " Warning: " + ex.Source + " " + ex.Message);
                }
                catch (Exception ex2)
                {
                    throw ex;
                }
                finally
                {
                    writeToLog.Close();
                    writeToLog.Dispose();
                }
            }

            finally
            {
            }
        }

        public static void Create()
        {
            SqlConnection sqlConnection = null;
            string sqlNorthwind = System.Configuration.ConfigurationManager.ConnectionStrings["sqlNorthwind"].ConnectionString;
            SqlCommand CreateRowCommand = null;
            string CompanyName;
            string ContactName;
            string ContactTitle;
            string Country;
            string Phone;
            Console.WriteLine("CREATE ROW\nType in the company name: ");
            CompanyName = Console.ReadLine();
            Console.WriteLine("Type in the contact name: ");
            ContactName = Console.ReadLine();
            Console.WriteLine("Type in the contact title: ");
            ContactTitle = Console.ReadLine();
            Console.WriteLine("Type in the country: ");
            Country = Console.ReadLine();
            Console.WriteLine("Type in the phone number: ");
            Phone = Console.ReadLine();

            try
            {
                sqlConnection = new SqlConnection(sqlNorthwind);
                CreateRowCommand = new SqlCommand("ADD_ROW", sqlConnection);

                CreateRowCommand.CommandType = CommandType.StoredProcedure;

                CreateRowCommand.Parameters.Add(new SqlParameter("@CompanyName", CompanyName));
                CreateRowCommand.Parameters.Add(new SqlParameter("@ContactName", ContactName));
                CreateRowCommand.Parameters.Add(new SqlParameter("@ContactTitle", ContactTitle));
                CreateRowCommand.Parameters.Add(new SqlParameter("@Country", Country));
                CreateRowCommand.Parameters.Add(new SqlParameter("@Phone", Phone));

                sqlConnection.Open();

                CreateRowCommand.ExecuteNonQuery();

                Console.WriteLine("\nRow was added.\n");
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                sqlConnection.Close();
            }
        }

        public static void ReadRowBySupplierID()
        {
            SqlConnection sqlConnection = null;
            string sqlNorthwind = System.Configuration.ConfigurationManager.ConnectionStrings["sqlNorthwind"].ConnectionString;
            SqlCommand ReadRowCommand = null;
            int SupplierID;

            try
            {
                sqlConnection = new SqlConnection(sqlNorthwind);
                do
                {
                    Console.WriteLine("READ ROW\nType in the Supplier ID: ");
                } while (!(int.TryParse(Console.ReadLine(), out SupplierID)));
                if (CheckRowExists(SupplierID))
                {
                    ReadRowCommand = new SqlCommand("READ_ROW_BY_SUPPLIER_ID", sqlConnection);
                    ReadRowCommand.Parameters.Add(new SqlParameter("@SupplierID", SupplierID));
                    ReadRowCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    ReadRowCommand.ExecuteNonQuery();
                    SqlDataReader rowReader = ReadRowCommand.ExecuteReader();
                    rowReader.Read();

                    for (int i = 1; i < rowReader.FieldCount; i++)
                    {
                        if (rowReader.IsDBNull(i) == false)
                        {
                            Console.WriteLine(rowReader.GetString(i));
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The SupplierID was not found.");
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                sqlConnection.Close();
            }
        }

        public static void Read()
        {
            SqlConnection sqlConnection = null;
            string sqlNorthwind = System.Configuration.ConfigurationManager.ConnectionStrings["sqlNorthwind"].ConnectionString;
            DataTable suppliers = new DataTable();
            SqlDataAdapter dataAdapter = null;
            SqlCommand ObtainTable = null;
            SqlDataAdapter adapter = null;

            try
            {
                sqlConnection = new SqlConnection(sqlNorthwind);
                dataAdapter = new SqlDataAdapter();
                ObtainTable = new SqlCommand("OBTAIN_SUPPLIERS", sqlConnection);
                ObtainTable.CommandType = CommandType.StoredProcedure;
                adapter = new SqlDataAdapter(ObtainTable);
                adapter.Fill(suppliers);

                foreach (DataRow row in suppliers.Rows)
                {
                    foreach (DataColumn col in suppliers.Columns)
                    {
                        Console.WriteLine("{0}", row[col]);
                    }
                    Console.WriteLine();
                }
            }


            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                sqlConnection.Close();
            }
        }

        public static void Update()
        {
            SqlConnection sqlConnection = null;
            string sqlNorthwind = System.Configuration.ConfigurationManager.ConnectionStrings["sqlNorthwind"].ConnectionString;
            SqlCommand UpdateRowCommand = null;
            int SupplierID;
            string CompanyName;
            string ContactName;
            string ContactTitle;
            string Country;
            string Phone;

            try
            {
                sqlConnection = new SqlConnection(sqlNorthwind);
                do
                {
                    Console.WriteLine("UPDATE ROW\nType in the Supplier ID: ");
                } while (!(int.TryParse(Console.ReadLine(), out SupplierID)));

                if (CheckRowExists(SupplierID))
                {

                    Console.WriteLine("Type in the company name: ");
                    CompanyName = Console.ReadLine();
                    Console.WriteLine("Type in the contact name: ");
                    ContactName = Console.ReadLine();
                    Console.WriteLine("Type in the contact title: ");
                    ContactTitle = Console.ReadLine();
                    Console.WriteLine("Type in the country: ");
                    Country = Console.ReadLine();
                    Console.WriteLine("Type in the phone number: ");
                    Phone = Console.ReadLine();

                    UpdateRowCommand = new SqlCommand("UPDATE_ROW", sqlConnection);
                    UpdateRowCommand.CommandType = CommandType.StoredProcedure;
                    UpdateRowCommand.Parameters.Add(new SqlParameter("@SupplierID", SupplierID));
                    UpdateRowCommand.Parameters.Add(new SqlParameter("@CompanyName", CompanyName));
                    UpdateRowCommand.Parameters.Add(new SqlParameter("@ContactName", ContactName));
                    UpdateRowCommand.Parameters.Add(new SqlParameter("@ContactTitle", ContactTitle));
                    UpdateRowCommand.Parameters.Add(new SqlParameter("@Country", Country));
                    UpdateRowCommand.Parameters.Add(new SqlParameter("@Phone", Phone));

                    sqlConnection.Open();

                    UpdateRowCommand.ExecuteNonQuery();

                    Console.WriteLine("\nRow was updated.\n");
                }
                else
                {
                    Console.WriteLine("SupplierID not found.");
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                sqlConnection.Close();

            }
        }

        public static void Delete()
        {
            SqlConnection sqlConnection = null;
            string sqlNorthwind = System.Configuration.ConfigurationManager.ConnectionStrings["sqlNorthwind"].ConnectionString;
            SqlCommand DeleteRowCommand = null;
            int SupplierID;
            char userDeleteConfirmation;

            try
            {
                sqlConnection = new SqlConnection(sqlNorthwind);
                Console.WriteLine("DELETE ROW\nType in the supplier ID: ");
                int.TryParse(Console.ReadLine(), out SupplierID);

                if (CheckRowExists(SupplierID))
                {
                    Console.WriteLine("Are you sure you want to delete this row? Type Y for 'yes' or N for 'no.'");
                    userDeleteConfirmation = Console.ReadKey(true).KeyChar;
                    if (char.ToLower(userDeleteConfirmation) == 'y')
                    {
                        DeleteRowCommand = new SqlCommand("DELETE_ROW", sqlConnection);
                        DeleteRowCommand.Parameters.Add(new SqlParameter("@SupplierID", SupplierID));

                        DeleteRowCommand.CommandType = CommandType.StoredProcedure;

                        sqlConnection.Open();

                        DeleteRowCommand.ExecuteNonQuery();

                        Console.WriteLine("\nRow deleted. \n");
                    }
                    else
                    {
                        Console.WriteLine("Okay, no deletion.");
                    }
                }
                else
                {
                    Console.WriteLine("SupplierID not found.");
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                sqlConnection.Close();
            }
        }
        public static bool CheckRowExists(int SupplierID)
        {
            SqlConnection sqlConnection = null;
            string sqlNorthwind = System.Configuration.ConfigurationManager.ConnectionStrings["sqlNorthwind"].ConnectionString;
            bool rowExists = false;
            System.Collections.Generic.List<int> supplierIDList = new System.Collections.Generic.List<int>();
            SqlCommand CheckRowCommand = null;

            try
            {
                sqlConnection = new SqlConnection(sqlNorthwind);
                CheckRowCommand = new SqlCommand("READ_ROW_BY_SUPPLIER_ID", sqlConnection);
                CheckRowCommand.Parameters.Add(new SqlParameter("@SupplierID", SupplierID));
                CheckRowCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                CheckRowCommand.ExecuteNonQuery();
                SqlDataReader checkRow = CheckRowCommand.ExecuteReader();

                while (checkRow.Read())
                {
                    supplierIDList.Add(checkRow.GetInt32(0));
                }

                for (int i = 0; i < supplierIDList.Count; i++)
                {
                    if (SupplierID == supplierIDList[i])
                    {
                        rowExists = true;
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                sqlConnection.Close();
            }
            return rowExists;
        }
    }
}

