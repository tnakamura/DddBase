namespace DddBase
{
    // TODO: API Design
    internal interface IAggregate<TKey>
    {
        TKey Id { get; }
    }
}
