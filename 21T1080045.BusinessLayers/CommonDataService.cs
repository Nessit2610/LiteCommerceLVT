using _21T1080045.DataLayers;
using _21T1080045.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21T1080045.BusinessLayers
{
    public static class CommonDataService
    {
        private static readonly ICommonDAL<Customer> customerDB;
        private static readonly ICommonDAL<Category> categoryDB;
        private static readonly ICommonDAL<Employee> employeeDB;
        private static readonly ICommonDAL<Shipper> shipperDB;
        private static readonly ICommonDAL<Supplier> supplierDB;
        private static readonly ISimpleQueryDAL<Province> provinceDB;
        private static readonly ISimpleQueryDAL<Category> spcategoryDB;
        private static readonly ISimpleQueryDAL<Supplier> spsupplierDB;
        private static readonly ISimpleQueryDAL<OrderStatus> sporderstatusDB;
       


        /// <summary>
        /// Ctor
        /// </summary>
        static CommonDataService()
        {
            String connectionString = Configuration.ConnectionString;
            customerDB = new DataLayers.SQLServer.CustomerDAL(connectionString);
            categoryDB = new DataLayers.SQLServer.CategoryDAL(connectionString);
            employeeDB = new DataLayers.SQLServer.EmployeeDAL(connectionString);
            shipperDB = new DataLayers.SQLServer.ShipperDAL(connectionString);
            supplierDB = new DataLayers.SQLServer.SupplierDAL(connectionString);
            provinceDB = new DataLayers.SQLServer.ProvinceDAL(connectionString);
            spcategoryDB = new DataLayers.SQLServer.SimpleCategoryDAL(connectionString);
            spsupplierDB = new DataLayers.SQLServer.SimpleSupplierDAL(connectionString);
            sporderstatusDB = new DataLayers.SQLServer.SimpleOrderStatusDAL(connectionString);
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách khách hàng
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize,searchValue);
        }
        public static List<Customer> ListOfCustomers(string searchValue = "")
        {
            return customerDB.List(1, 0, searchValue).ToList();
        }
        public static int AddCustomer(Customer customer)
        {
            return customerDB.Add(customer);
        }

        public static Customer? GetCustomer(int id)
        {
            return customerDB.Get(id);
        }

        public static bool UpdateCustomer(Customer customer)
        {
            return customerDB.Update(customer);
        }
        public static bool DeleteCustomer(int id)
        {
            if (customerDB.InUsed(id))
            {
                return false;
            }
            else
            {
                return customerDB.Delete(id);
            }
        }
        public static bool InUsedCustomer(int id)
        {
            return customerDB.InUsed(id);
        }


        //Tìm kiếm và lấy danh sách loại hàng
        public static List<Category> SimpleListCategory()
        {
            return spcategoryDB.List();
        }
        public static List<Category> ListOfCategories(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue);
        }
        public static int AddCategory(Category category)
        {
            return categoryDB.Add(category);
        }
        public static Category? GetCategory(int id)
        {
            return categoryDB.Get(id);
        }
        public static bool UpdateCategory(Category category)
        {
            return categoryDB.Update(category);
        }
        public static bool DeleteCategory(int id)
        {
            if (categoryDB.InUsed(id))
            {
                return false;
            }
            else
            {
                return categoryDB.Delete(id);
            }
        }
        public static bool InUsedCategory(int id)
        {
            return categoryDB.InUsed(id);
        }

        //Tìm kiếm và lấy danh sách nhân viên
        public static List<Employee> ListOfEmployees(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue);
        }
        public static int AddEmployee(Employee employee)
        {
            return employeeDB.Add(employee);
        }
        public static Employee? GetEmployee(int id)
        {
            return employeeDB.Get(id);
        }
        public static bool UpdateEmployee(Employee employee)
        {
            return employeeDB.Update(employee);
        }
        public static bool DeleteEmployee(int id)
        {
            if (employeeDB.InUsed(id))
            {
                return false;
            }
            else
            {
                return employeeDB.Delete(id);
            }
        }
        public static bool InUsedEmployee(int id)
        {
            return employeeDB.InUsed(id);
        }


        //Tìm kiếm và lấy danh sách người giao hàng
        public static List<Shipper> ListOfShippers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue);
        }
        public static List<Shipper> ListOfShippers( string searchValue = "")
        {
            return shipperDB.List(1, 0, searchValue);
        }
        public static int AddShipper(Shipper shipper)
        {
            return shipperDB.Add(shipper);
        }
        public static Shipper? GetShipper(int id)
        {
            return shipperDB.Get(id);
        }
        public static bool UpdateShipper(Shipper shipper)
        {
            return shipperDB.Update(shipper);
        }
        public static bool DeleteShipper(int id)
        {
            if (shipperDB.InUsed(id))
            {
                return false;
            }
            else
            {
                return shipperDB.Delete(id);
            }
        }
        public static bool InUsedShipper(int id)
        {
            return shipperDB.InUsed(id);
        }

        //Tìm kiếm và lấy danh sách nhà cung cấp
        public static List<Supplier> SimpleListSupplier()
        {
            return spsupplierDB.List();
        }
        public static List<Supplier> ListOfSuppliers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize, searchValue);
        }
        public static int AddSupplier(Supplier supplier) 
        {
            return supplierDB.Add(supplier);
        }
        public static Supplier? GetSupplier(int id)
        {
            return supplierDB.Get(id);
        }
        public static bool UpdateSupplier(Supplier supplier)
        {
            return supplierDB.Update(supplier);
        }
        public static bool DeleteSupplier(int id) 
        {
            if (supplierDB.InUsed(id))
            {
                return false;
            }
            else
            {
                return supplierDB.Delete(id);
            }
        }
        public static bool InUsedSupplier(int id)
        {
            return supplierDB.InUsed(id);
        }

        //Lấy ra danh sách các tỉnh thành
        public static List<Province> ListOfProvinces()
        {
            return provinceDB.List();
        }

        public static List<OrderStatus> ListOfOrderStatus() 
        {
            return sporderstatusDB.List();
        }
    }
}
