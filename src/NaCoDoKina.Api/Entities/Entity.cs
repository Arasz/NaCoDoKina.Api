using System.Collections.Generic;

namespace NaCoDoKina.Api.Entities
{
    /// <summary>
    /// Base class for all persisted model classes 
    /// </summary>
    public abstract partial class Entity
    {
        public long Id { get; set; }
    }

    public abstract partial class Entity
    {
        protected bool Equals(Entity other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Entity)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static IEqualityComparer<Entity> IdComparer { get; } = new IdEqualityComparer();

        private sealed class IdEqualityComparer : IEqualityComparer<Entity>
        {
            public bool Equals(Entity x, Entity y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id == y.Id;
            }

            public int GetHashCode(Entity obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}