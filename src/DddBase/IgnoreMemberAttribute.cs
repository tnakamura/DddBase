using System;

namespace DddBase
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class IgnoreMemberAttribute : Attribute
    {
    }
}
