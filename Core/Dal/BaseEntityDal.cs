namespace Core.Dal
{
    public class BaseEntityDal<T>
    {
        public required T Id { get; init; }
    }
}
