using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Api
{
    public interface IDal
    {
        public ISupplier Supplier { get; }  
        public IProduct Product { get; }  
        public IOrder Order { get; }
        public IStatus Status { get; }
        public IUser User { get; }
        public IManager Manager { get; }

    }
}
