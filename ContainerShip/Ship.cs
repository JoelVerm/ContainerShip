using System.Diagnostics;

namespace ContainerShip
{
    public class Ship
    {
        public readonly int Width;
        public readonly int Length;

        readonly List<Row> rows = [];
        public IReadOnlyList<Row> Rows => rows;

        public Ship(int width, int length)
        {
            if (width < 1 || length < 1)
                throw new ArgumentException("Width and length must be at least 1");
            Width = width;
            Length = length;
            for (int i = 0; i < length; i++)
                rows.Add(new Row(width));

            for (int i = 0; i < length - 1; i++)
            {
                rows[i].Next = rows[i + 1];
                rows[i + 1].Previous = rows[i];
            }
        }

        public int Weight() => rows.Sum(r => r.Weight());

        public bool TryAdd(Container container)
        {
            if (container.IsCooled)
                return rows.First().TryAdd(container);
            return rows.Skip(1).OrderBy(r => r.Weight()).Any(r => r.TryAdd(container));
        }

        public void AddContainers(IEnumerable<Container> containers)
        {
            var containerWeight = containers.Sum(c => c.Weight);
            var shipMaxWeight = rows.Sum(r => r.MaxWeight());
            Debug.WriteLine($"Container weight: {containerWeight}, ship max weight: {shipMaxWeight}");
            if (containerWeight > shipMaxWeight)
                throw new ArgumentException("Containers are too heavy for the ship");
            if (containerWeight < shipMaxWeight / 2)
                throw new ArgumentException("Containers are too light for the ship");

            // Order by weight to make sure the heaviest containers are placed on the bottom
            foreach (var container in containers.OrderBy(c => c.IsValuable).OrderByDescending(c => c.Weight))
                if (!TryAdd(container))
                    throw new InvalidOperationException("No more space for containers");
        }
    }
}
