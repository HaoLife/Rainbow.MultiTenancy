using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Abstractions
{
    [Serializable]
    public class ConnectionStrings : NameValueCollection
    {
        public const string DefaultConnectionStringName = "Default";

        public string Default
        {
            get => this[DefaultConnectionStringName];
            set => this[DefaultConnectionStringName] = value;
        }
    }
}
