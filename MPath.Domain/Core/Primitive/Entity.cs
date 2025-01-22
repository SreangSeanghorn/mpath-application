
namespace MPath.Domain.Core.Primitive
{
    public abstract class Entity<T>
    {
        public T Id { get; set; }

        public override bool Equals(object? obj)
        {
           if (obj == null || !(obj is Entity<T> other))
           {
               return false;
           }
           var entity = (Entity<T>) obj;
           return Id.Equals(entity.Id);
        }
        public int GetHashCode()
        {
            return Id.GetHashCode();
        }
        public static bool operator ==(Entity<T> a, Entity<T> b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            if (a is null || b is null)
            {
                return false;
            }
            return a.Equals(b);
        }
        
        public static bool operator !=(Entity<T> a, Entity<T> b)
        {
            return !(a == b);
        }
    }
}