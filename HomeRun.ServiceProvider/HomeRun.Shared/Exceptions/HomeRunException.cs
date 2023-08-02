using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeRun.Shared.Exceptions
{
    public class HomeRunException : Exception
    {
        public HomeRunException(string message) : base(message)
        {

        }
    }
}
