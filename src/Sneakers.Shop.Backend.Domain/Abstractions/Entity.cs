namespace Sneakers.Shop.Backend.Domain.Abstractions
{
    public abstract class Entity : IEquatable<Entity>
    {
        public Guid Id { get; protected set; }

        protected Entity(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            Id = id;
        }

        protected Entity() { }

        public bool Equals(Entity? other) =>
            other != null && GetType() == other.GetType() && Id.Equals(other.Id);
        public override bool Equals(object? obj) => Equals(obj as Entity);
        public override int GetHashCode() => HashCode.Combine(GetType(), Id);

        public static bool operator ==(Entity? left, Entity? right) => Equals(left, right);
        public static bool operator !=(Entity? left, Entity? right) => !Equals(left, right);
    }
}
