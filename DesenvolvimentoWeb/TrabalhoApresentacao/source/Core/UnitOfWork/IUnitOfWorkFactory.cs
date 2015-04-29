namespace Core.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Criar();
    }
}