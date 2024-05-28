using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerShip
{
    public static class Helpers
    {
        public static IEnumerable<(T item, int index)> Index<T>(this IEnumerable<T> source) => source.Select((item, index) => (item, index));
    }
}
