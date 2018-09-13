using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace TodoApp.Contracts.Configs
{
    public interface IDatabaseConfig
    {
        void Register(IUnityContainer container);
    }
}
