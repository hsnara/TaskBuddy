using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBuddyClassLibrary.Exceptions
{
    public class TaskBuddyException : Exception
    {
        public TaskBuddyException(string message)
            : base(message)
        {
        }

        public TaskBuddyException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
