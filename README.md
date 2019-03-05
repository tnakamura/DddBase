# DddBase

DDD base class library for .NET application.


## Install

```
PM> Install-Package DddBase
```


## Usage

### Identifier

```cs
public class EntryId : Identifier<string>
{
    public EntryId(string id)
        : base(id)
    {
    }
}
```


### ValueObject

```cs
public class Address : ValueObject<Address>
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }
    public string ZipCode { get; private set; }

    private Address() { }

    public Address(string street, string city, string state, string country, string zipcode)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipcode;
    }
}
```


## Author

- [tnakamura](https://github.com/tnakamura)


## License

MIT

