

using MPath.Domain.Core.Primitive;

namespace MPath.Domain.Entities
{
    public class Role : Entity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();   
        private Role()
        {
        }
        private Role(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
         public static Role CreateRole(string name, string description)
        {
            return new Role
            {
                Name = name,
                Description = description
            };
        }
        public static Role GetRole(string name)
        {
            return new Role
            {
                Name = name,
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is Role role)
            {
                return Name == role.Name;
            }
            return false;
        }

        public new int GetHashCode()
        {
            return Name.GetHashCode();
        }


    }
}