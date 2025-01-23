
namespace MPath.SharedKernel.Primitive
{
    public abstract class Entity<T>
    {

        private bool Equals(Entity<T>? other)
        {
            return other != null && EqualityComparer<T>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Id);
        }

        public T Id { get; }
        
    
        public static bool operator ==(Entity<T>? a, Entity<T>? b)
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
        
        public static bool operator !=(Entity<T>? a, Entity<T>? b)
        {
            return !(a == b);
        }
    }
}