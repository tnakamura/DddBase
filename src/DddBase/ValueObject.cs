using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DddBase
{
    public abstract class ValueObject<TSelf> : IEquatable<TSelf>
        where TSelf : ValueObject<TSelf>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ValueObject{TValueObject}"/>.
        /// </summary>
        protected ValueObject()
        {
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        /// <returns>
        /// true if the current object is equal to the other parameter; otherwise, false.
        /// </returns>
        public bool Equals(TSelf other)
        {
            if (other == null || GetType() != other.GetType())
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return DelegateCache.EqualsDelegate((TSelf)this, other);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">
        /// The object to compare with the current object.
        /// </param>
        /// <returns>
        /// true if the specified object is equal to the current object; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as TSelf);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return DelegateCache.GetHashCodeDelegate((TSelf)this);
        }

        public static bool operator ==(ValueObject<TSelf> obj1, ValueObject<TSelf> obj2)
        {
            if (ReferenceEquals(obj1, null) ^ ReferenceEquals(obj2, null))
            {
                return false;
            }
            return ReferenceEquals(obj1, null) || obj1.Equals(obj2);
        }

        public static bool operator !=(ValueObject<TSelf> obj1, ValueObject<TSelf> obj2)
        {
            return !(obj1 == obj2);
        }

        static class DelegateCache
        {
            public static readonly Func<TSelf, TSelf, bool> EqualsDelegate;

            public static readonly Func<TSelf, int> GetHashCodeDelegate;

            static DelegateCache()
            {
                EqualsDelegate = BuildEqualsDelegate();
                GetHashCodeDelegate = BuildGetHashCodeDelegate();
            }

            static MemberInfo[] GetTargetMembers()
            {
                return typeof(TSelf).GetMembers(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.GetCustomAttribute<IgnoreMemberAttribute>(true) == null)
                    .Where(x => x is FieldInfo || x is PropertyInfo)
                    .ToArray();
            }

            static Func<TSelf, TSelf, bool> BuildEqualsDelegate()
            {
                var members = GetTargetMembers();
                if (members.Length == 0)
                {
                    return new Func<TSelf, TSelf, bool>((x, y) => true);
                }

                var obj1 = Expression.Parameter(typeof(TSelf), "obj1");
                var obj2 = Expression.Parameter(typeof(TSelf), "obj2");
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
                return Expression.Lambda<Func<TSelf, TSelf, bool>>(bodyExpr, obj1, obj2).Compile();
            }

            static Func<TSelf, int> BuildGetHashCodeDelegate()
            {
                var members = GetTargetMembers();
                if (members.Length == 0)
                {
                    return new Func<TSelf, int>(x => 0);
                }

                var obj = Expression.Parameter(typeof(TSelf), "obj");
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
                return Expression.Lambda<Func<TSelf, int>>(bodyExpr, obj)
                    .Compile();
            }
        }
    }
}
