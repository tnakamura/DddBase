# DddBase

DDD base class library for .NET application.


## Features

This library provide basic DDD base classes.

- Entity : Entity is domain concept that have a unique identity
- Identifier : Identifier is unique identifier for an Entity
- ValueObject : Value Object is an entity's state, describing something about the entity
- (ToDo) Repository : Repository is used to manage aggregate persistence

## Install

```
PM> Install-Package DddBase
```


## Usage

### Identifier

```cs
public class CustomerId : Identifier<string>
{
    public CustomerId(string id)
        : base(id)
    {
    }

    public static CustomerId NewCustomerId()
    {
        return new CustomerId(Guid.NewGuid().ToString());
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

## Entity

```cs
public class Customer : Entity<CustomerId>
{
    public Customer(string name, Address address)
        : base(CustomerId.NewCustomerId())
    {
        Name = name;
        Address = address;
    }

    public string Name { get; private set; } 

    public Address Address { get; private set; }
}
```


## Author

- [tnakamura](https://github.com/tnakamura)


## License

MIT

