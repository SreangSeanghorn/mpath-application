

namespace MPath.SharedKernel.Primitive
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {

        protected abstract IEnumerable<object> GetAtomicValues();
        public bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            var other = (ValueObject)obj;
            return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
        }
        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(x => x.GetHashCode())
                .Aggregate((x, y) => x ^ y);
        }
        public bool Equals(ValueObject? other)
        {
            return Equals((object)other!);
        }
        public static bool operator ==(ValueObject? a, ValueObject? b)
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
        
        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }
    }
}