namespace DddBase
{
    internal interface IAggregate<TKey>
    {
        TKey Id { get; }
    }
}
