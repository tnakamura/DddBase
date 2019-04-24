namespace DddBase
{
    // TODO: API Design
    internal interface IAggregateRoot<TKey>
    {
        TKey Id { get; }
    }
}
