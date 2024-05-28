using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerShip
{
    public class Row
    {
        private readonly List<Stack> stacks = [];
        public IReadOnlyList<Stack> Stacks => stacks;

        public Row? Previous { get; set; } = null;
        public Row? Next { get; set; } = null;

        public Row(int width)
        {
            for (int i = 0; i < width; i++)
                stacks.Add(new Stack());
        }

        public int Weight() => stacks.Sum(s => s.Weight());

        public int MaxWeight() => stacks.Count * Stack.MAX_WEIGHT;

        public bool IsOccupied(int stack, int index)
        {
            return stacks.ElementAtOrDefault(stack)?.IsOccupied(index) ?? false;
        }

        public bool CanAddValuable(int stack, int index)
        {
            var previous = Previous?.IsOccupied(stack, index) ?? false;
            var next = Next?.IsOccupied(stack, index) ?? false;
            return !previous || !next;
        }

        public bool TryAdd(Container container)
        {
            return stacks
                .Index()
                .OrderBy(s => s.item.Weight())
                .Any(s => s.item.TryAdd(container, i => CanAddValuable(s.index, i)));
        }
    }
}
