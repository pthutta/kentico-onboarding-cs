using Unity;

namespace TodoApp.Contracts.Configs
{
    public interface IDatabaseConfig
    {
        void Register(IUnityContainer container);
    }
}
