using Unity;

namespace TodoApp.Contracts.Configs
{
    public interface IConfig
    {
        void Register(IUnityContainer container);
    }
}
