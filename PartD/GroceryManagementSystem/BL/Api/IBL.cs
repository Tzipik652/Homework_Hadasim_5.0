using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Api
{
    public interface IBL
    {
        public IBLSupplier Supplier { get; }  
        public IBLProduct Products { get; }  
        public IBLOrder Order { get; }
        public IBLAuth Auth { get; }
        public IBLManager Manager { get; }



    }
}
