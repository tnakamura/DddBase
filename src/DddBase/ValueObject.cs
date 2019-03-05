using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

namespace DddBase
{
    public abstract class ValueObject
    {
        protected ValueObject()
        {
        }

        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            var other = (ValueObject)obj;
            var thisValues = GetAtomicValues().GetEnumerator();
            var otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^
                    ReferenceEquals(otherValues.Current, null))
                {
                    return false;
                }
                if (thisValues.Current != null &&
                    !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }
            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate(0, (x, y) => x ^ y);
        }

        public static bool operator ==(ValueObject obj1, ValueObject obj2)
        {
            if (ReferenceEquals(obj1, null) ^ ReferenceEquals(obj2, null))
            {
                return false;
            }
            return ReferenceEquals(obj1, null) || obj1.Equals(obj2);
        }

        public static bool operator !=(ValueObject obj1, ValueObject obj2)
        {
            return !(obj1 == obj2);
        }
    }

    public abstract class ValueObject<T> : IEquatable<T>
        where T : ValueObject<T>
    {
        protected ValueObject()
        {
        }

        public bool Equals(T other)
        {
            if (other == null || GetType() != other.GetType())
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return DelegateCache.EqualsDelegate((T)this, other);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as T);
        }

        public override int GetHashCode()
        {
            return DelegateCache.GetHashCodeDelegate((T)this);
        }

        public static bool operator ==(ValueObject<T> obj1, ValueObject<T> obj2)
        {
            if (ReferenceEquals(obj1, null) ^ ReferenceEquals(obj2, null))
            {
                return false;
            }
            return ReferenceEquals(obj1, null) || obj1.Equals(obj2);
        }

        public static bool operator !=(ValueObject<T> obj1, ValueObject<T> obj2)
        {
            return !(obj1 == obj2);
        }

        class DelegateCache
        {
            public static readonly Func<T, T, bool> EqualsDelegate;

            public static readonly Func<T, int> GetHashCodeDelegate;

            static DelegateCache()
            {
                EqualsDelegate = BuildEqualsDelegate();
                GetHashCodeDelegate = BuildGetHashCodeDelegate();
            }

            static Func<T, T, bool> BuildEqualsDelegate()
            {
                var members = typeof(T).GetMembers(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x is FieldInfo || x is PropertyInfo)
                    .ToArray();
                if (members.Length == 0)
                {
                    return new Func<T, T, bool>((x, y) => true);
                }

                var obj1 = Expression.Parameter(typeof(T), "obj1");
                var obj2 = Expression.Parameter(typeof(T), "obj2");
                Expression bodyExpr = null;

                foreach (var member in members)
                {
                    // obj1.Member == obj2.Member
                    var equalExpr = Expression.Equal(
                        Expression.PropertyOrField(
                            obj1,
                            member.Name),
                        Expression.PropertyOrField(
                            obj2,
                            member.Name));

                    // (obj1.Member1 == obj2.Member1) &&
                    // (obj1.Member2 == obj2.Member2) &&
                    // ...
                    // (obj1.MemberN == obj2.MemberN)
                    if (bodyExpr != null)
                    {
                        bodyExpr = Expression.And(
                            bodyExpr,
                            equalExpr);
                    }
                    else
                    {
                        bodyExpr = equalExpr;
                    }
                }

                // (obj1, obj2) => (obj1.Member1 == obj2.Member1) &&
                //     (obj1.Member2 == obj2.Member2) &&
                //     ...
                //     (obj1.MemberN == obj2.MemberN)
                return Expression.Lambda<Func<T, T, bool>>(bodyExpr, obj1, obj2).Compile();
            }

            static Func<T, int> BuildGetHashCodeDelegate()
            {
                var members = typeof(T).GetMembers(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x is FieldInfo || x is PropertyInfo)
                    .ToArray();
                if (members.Length == 0)
                {
                    return new Func<T, int>(x => 0);
                }

                var obj = Expression.Parameter(typeof(T), "obj");
                Expression bodyExpr = null;

                foreach (var member in members)
                {
                    var memberExpr = Expression.PropertyOrField(
                        obj,
                        member.Name);

                    // obj.Member == null ? 0 : obj.Member.GetHashCode();
                    var conditionExpr = Expression.Condition(
                        Expression.Equal(
                            memberExpr,
                            Expression.Constant(null)),
                        Expression.Constant(0),
                        Expression.Call(
                            memberExpr,
                            "GetHashCode",
                            new Type[0]));

                    // (obj.Member1 == null ? 0 : obj.Member1.GetHashCode()) ^
                    // (obj.Member2 == null ? 0 : obj.Member2.GetHashCode()) ^
                    // ....
                    // (obj.MemberN == null ? 0 : obj.MemberN.GetHashCode())
                    if (bodyExpr != null)
                    {
                        bodyExpr = Expression.ExclusiveOr(
                            bodyExpr,
                            conditionExpr);
                    }
                    else
                    {
                        bodyExpr = conditionExpr;
                    }
                }

                // obj => (obj.Member1 == null ? 0 : obj.Member1.GetHashCode()) ^
                //     (obj.Member2 == null ? 0 : obj.Member2.GetHashCode()) ^
                //     ....
                //     (obj.MemberN == null ? 0 : obj.MemberN.GetHashCode())
                return Expression.Lambda<Func<T, int>>(bodyExpr, obj)
                    .Compile();
            }
        }
    }
}
