using System.Collections.Generic;

namespace DddBase
{
    public abstract class Entity<TKey>
    {
        protected Entity(TKey id)
        {
            Id = id;
        }

        public TKey Id { get; private set; }

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
            return EqualityComparer<TKey>.Default.Equals(Id, ((Entity<TKey>)obj).Id);
        }
        public override int GetHashCode()
        {
            return EqualityComparer<TKey>.Default.GetHashCode(Id);
        }
    }
}
