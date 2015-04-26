namespace Core.Entidades
{
    public interface IEntidade<T>
    {
        T Id { get; set; }
    }
}