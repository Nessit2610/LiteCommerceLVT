using _21T1080045.DataLayers;
using _21T1080045.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21T1080045.BusinessLayers
{
    public static class ProductDataService
    {
        private static readonly IProductDAL productDB;

        static ProductDataService()
        {
            productDB = new DataLayers.SQLServer.ProductDAL(Configuration.ConnectionString);
        }
        public static List<Product> ListProducts(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "",int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            rowCount = productDB.Count(searchValue,categoryID,supplierID,minPrice,maxPrice);
            return productDB.List(page, pageSize, searchValue, categoryID, supplierID, minPrice, maxPrice);
        }
        public static Product? GetProduct(int productID)
        {
            return productDB.Get(productID);
        }
        public static int AddProduct(Product product)
        {
            return productDB.Add(product);
        }
        public static bool UpdateProduct(Product product)
        {
            return productDB.Update(product);
        }
        public static bool DeleteProduct(int productID)
        {
            return productDB.Delete(productID);
        }
        public static long AddProductPhoto(ProductPhoto productPhoto)
        {
            return productDB.AddPhoto(productPhoto);
        }
        public static IList<ProductPhoto> ListOfProductPhoto(int productPhotoID)
        {
            return productDB.ListPhotos(productPhotoID);
        }
        public static ProductPhoto? GetProductPhoto(long productID, long photoID) 
        {
            return productDB.GetPhoto(productID, photoID);
        }
        public static bool UpdateProductPhoto(ProductPhoto productPhoto)
        {
            return productDB.UpdatePhoto(productPhoto);
        }
        public static bool DeleteProductPhoto(long productID, long photoID)
        {
            return productDB.DeletePhoto(productID, photoID);
        }
        public static IList<ProductAttribute> ListProductAttribute(int productID)
        {
            return productDB.ListAttributes(productID);
        }
        public static long AddProductAttribute(ProductAttribute data)
        {
            return productDB.AddAttribute(data);
        }
        public static ProductAttribute? GetProductAttribute(int attributeID)
        {
            return productDB.GetAttribute(attributeID);
        }
        public static bool UppdateAttribute(ProductAttribute productAttribute)
        {
            return productDB.UpdateAttribute(productAttribute);
        }
        public static bool DeleteAttribute(long attributeID)
        {
            return productDB.DeleteAttribute(attributeID);
        }

    }

    
}
