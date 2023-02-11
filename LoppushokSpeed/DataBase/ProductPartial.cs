using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoppushokSpeed.DataBase
{
    public partial class Product
    {
        public string Materials { get => string.Join(", ", ProductMaterials.Select(x => x.Material.Name)); }
    }
}
