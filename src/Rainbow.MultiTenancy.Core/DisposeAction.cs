using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Core
{
    public class DisposeAction : IDisposable
    {
        private readonly Action _action;

        public DisposeAction(Action action)
        {

            _action = action;
        }

        public void Dispose()
        {
            _action();
        }
    }
}
