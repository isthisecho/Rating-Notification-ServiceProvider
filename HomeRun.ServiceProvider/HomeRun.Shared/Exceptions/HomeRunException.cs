using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeRun.Shared.Exceptions
{
    public class HomeRunException : Exception
    {
        public HomeRunException() { }
        public HomeRunException(string message) : base(message) { }
        public HomeRunException(string message, Exception innerException) : base(message, innerException) { }
    }
}
