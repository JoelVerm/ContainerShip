using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerShip
{
    public class Stack
    {
        public const int MAX_WEIGHT_ON_CONTAINER = 120;
        public const int MAX_WEIGHT = MAX_WEIGHT_ON_CONTAINER + Container.MAX_WEIGHT;

        private readonly List<Container> containers = [];
        public IReadOnlyList<Container> Containers => containers;

        public int Weight() => containers.Sum(c => c.Weight);

        public bool IsOccupied(int index) => containers.ElementAtOrDefault(index) != null;

        public bool CanPlaceOnBottom(Container container)
        {
            var underWeight = Weight() < MAX_WEIGHT_ON_CONTAINER;
            return underWeight && !container.IsValuable;
        }

        public bool CanPlaceOnTop(Container container)
        {
            var underWeight = containers.Skip(1).Sum(c => c.Weight) + container.Weight < MAX_WEIGHT_ON_CONTAINER;
            var onValuable = containers.LastOrDefault()?.IsValuable ?? false;
            return underWeight && !onValuable;
        }

        public bool TryAdd(Container container, Func<int, bool> CanAddValuable)
        {
            if (container.IsValuable && CanAddValuable(containers.Count))
            {
                containers.Add(container);
                return true;
            }
            else if (CanPlaceOnBottom(container))
            {
                containers.Insert(0, container);
                return true;
            }
            else if (CanPlaceOnTop(container))
            {
                containers.Add(container);
                return true;
            }
            return false;
        }
    }
}
