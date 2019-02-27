using System;
using System.Collections.Generic;
using System.Text;

namespace DddBase
{
    public abstract class Entity<TKey>
    {
        protected Entity(TKey id)
        {
            Id = id;
        }

        public TKey Id { get; private set; }
    }
}
