﻿using System;
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

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(value);
        }

        public int CompareTo(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return Comparer<T>.Default.Compare(value, ((Identifier<T>)obj).value);
        }
    }
}
