namespace ModuleLibrary
{
    public interface IModule<T>
    {
        void Initialize(T events);
    }
}
