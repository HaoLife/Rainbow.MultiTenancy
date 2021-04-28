using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Abstractions
{
    public interface ICurrentTenant
    {
        bool IsAvailable { get; }

        Guid? Id { get; }
        string Name { get; }

        IDisposable Change(Guid? id, string name = null);
    }
}
