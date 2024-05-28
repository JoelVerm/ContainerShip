using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerShip
{
    public class Container
    {
        public const int MAX_WEIGHT = 30;
        public const int MIN_WEIGHT = 4;

        public readonly int Weight;
        public readonly bool IsValuable;
        public readonly bool IsCooled;

        public Container(int weight, bool isValuable, bool isCooled)
        {
            if (weight < MIN_WEIGHT || weight > MAX_WEIGHT)
            {
                throw new ArgumentException("Invalid weight");
            }

            Weight = weight;
            IsValuable = isValuable;
            IsCooled = isCooled;
        }
    }
}
