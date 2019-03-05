using System;
using System.Collections.Generic;

namespace DddBase
{
    public abstract class Identifier<T> : IComparable
    {
        readonly T value;

        protected Identifier(T value)
        {
            this.value = value;
        }

        public T ToValue() => value;

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
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return EqualityComparer<T>.Default.Equals(value, ((Identifier<T>)obj).value);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(value);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            return Comparer<T>.Default.Compare(value, ((Identifier<T>)obj).value);
        }

        public static bool operator ==(Identifier<T> obj1, Identifier<T> obj2)
        {
            if (ReferenceEquals(obj1, null) ^ ReferenceEquals(obj2, null))
            {
                return false;
            }
            return ReferenceEquals(obj1, null) || obj1.Equals(obj2);
        }

        public static bool operator !=(Identifier<T> obj1, Identifier<T> bbj2)
        {
            return !(obj1 == bbj2);
        }
    }
}
