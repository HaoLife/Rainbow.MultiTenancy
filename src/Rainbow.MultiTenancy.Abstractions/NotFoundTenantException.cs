using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Abstractions
{
    public class NotFoundTenantException : Exception
    {
        public NotFoundTenantException()
        {
        }

        public NotFoundTenantException(string message) : base(message)
        {
        }

        public NotFoundTenantException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotFoundTenantException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
