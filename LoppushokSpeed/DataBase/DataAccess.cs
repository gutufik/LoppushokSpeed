using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoppushokSpeed.DataBase
{
    public class DataAccess
    {
        public delegate void RefreshListDelegate();
        public static event RefreshListDelegate RefreshList;

        public static List<Product> GetProducts() => LopushokSpeendRunEntities.GetContext().Products.ToList();

        internal static List<ProductType> GetProductTypes() => LopushokSpeendRunEntities.GetContext().ProductTypes.ToList();

        internal static void SaveProduct(Product product)
        {
            if (product.Id == 0)
                LopushokSpeendRunEntities.GetContext().Products.Add(product);
            LopushokSpeendRunEntities.GetContext().SaveChanges();
            RefreshList?.Invoke();
        }

        internal static void DeleteProduct(Product product)
        {
            LopushokSpeendRunEntities.GetContext().Products.Remove(product);
            LopushokSpeendRunEntities.GetContext().SaveChanges();
            RefreshList?.Invoke();
        }
    }
}
