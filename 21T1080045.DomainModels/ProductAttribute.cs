using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21T1080045.DomainModels
{
    public class ProductAttribute
    {
        public int AttributeID { get; set; }
        public int ProductID { get; set; }
        public string AttributeName {  get; set; } = string.Empty;
        public string AttributeValue { get; set; }= string.Empty;
        public int DisplayOrder {  get; set; }
    }
}
