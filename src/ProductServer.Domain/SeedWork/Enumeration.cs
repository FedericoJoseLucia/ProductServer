using System.Reflection;

// Microsoft SeedWork
// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/seedwork-domain-model-base-classes-interfaces

namespace ProductServer.Domain.SeedWork
{
    public abstract class Enumeration : IComparable, IEquatable<Enumeration>
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public |
                                             BindingFlags.Static |
                                             BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Enumeration otherValue)
                return false;

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public int CompareTo(object? other) => Id.CompareTo(((Enumeration)other!).Id);

        public bool Equals(Enumeration? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Id);
        }

        public static bool operator ==(Enumeration? left, Enumeration? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Enumeration? left, Enumeration? right)
        {
            return !Equals(left, right);
        }
    }
}
