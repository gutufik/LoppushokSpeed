using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoppushokSpeed.DataBase
{
    public partial class LopushokSpeendRunEntities
    {
        private static LopushokSpeendRunEntities context;
        public static LopushokSpeendRunEntities GetContext()
        {
            if (context == null)
                context = new LopushokSpeendRunEntities();
            return context;
        }
    }
}
