using ContainerShip;
using System.Diagnostics;

namespace TestShip
{
    [TestClass]
    public class ProgramTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            // Run the app once for visualisation
            Ship ship = RandomValidShip();
            Debug.WriteLine($"https://i872272.luna.fhict.nl/ContainerVisualizer/index.html?length={ship.Length}&width={ship.Width}&stacks={string.Join("/", ship.Rows.Select(r => string.Join(",", r.Stacks.Select(s => string.Join("-", s.Containers.Select(c => 1 + (c.IsValuable?1:0) + 2*(c.IsCooled?1:0)))))))}&weights={string.Join("/", ship.Rows.Select(r => string.Join(",", r.Stacks.Select(s => string.Join("-", s.Containers.Select(c => c.Weight))))))}");
        }

        [TestMethod]
        public void TestMaxWeightOnContainer()
        {
            Ship ship = RandomValidShip();
            Assert.IsTrue(ship.Rows.All(
                r => r.Stacks.All(
                    s => s.Containers.Skip(1).Sum(c => c.Weight) <= Stack.MAX_WEIGHT_ON_CONTAINER
                )
            ));
        }

        [TestMethod]
        public void TestUnderweightContainer()
        {
            Assert.ThrowsException<ArgumentException>(() => new Container(Container.MIN_WEIGHT - 1, false, false));
        }

        [TestMethod]
        public void TestOverweightContainer()
        {
            Assert.ThrowsException<ArgumentException>(() => new Container(Container.MAX_WEIGHT + 1, false, false));
        }

        [TestMethod]
        public void TestValuablesOnTop()
        {
            Ship ship = RandomValidShip();
            Assert.IsTrue(ship.Rows.All(
                r => r.Stacks.Where(s => s.Containers.LastOrDefault()?.IsValuable ?? false).All(
                    s => s.Containers.Take(s.Containers.Count - 1).All(c => !c.IsValuable)
                )
            ));
        }

        [TestMethod]
        public void TestAccessibleValuables()
        {
            Ship ship = RandomValidShip();
            Assert.IsFalse(ship.Rows.Any(
                r => r.Stacks.Index().Any(
                    s => s.item.Containers.Index().Any(
                        c => c.item.IsValuable
                        && r.Previous?.Stacks.ElementAtOrDefault(s.index)?.Containers.ElementAtOrDefault(c.index) != null
                        && r.Next?.Stacks.ElementAtOrDefault(s.index)?.Containers.ElementAtOrDefault(c.index) != null
                    )
                )
            ));
        }

        [TestMethod]
        public void TestNoCooledAfterFirst()
        {
            Ship ship = RandomValidShip();
            Assert.IsFalse(ship.Rows.Skip(1).Any(r => r.Stacks.Any(s => s.Containers.Any(c => c.IsCooled))));
        }

        [TestMethod]
        public void TestMoreThanHalfWeight()
        {
            Ship ship = new(5, 10);
            Assert.ThrowsException<ArgumentException>(() => ship.AddContainers([new Container(Container.MIN_WEIGHT, false, false)]));
        }

        [TestMethod]
        public void TestBalance20Percent()
        {
            Ship ship = RandomValidShip();
            var weightLeft = ship.Rows.Sum(r => r.Stacks.Take(r.Stacks.Count / 2).Sum(s => s.Weight()));
            var weightRight = ship.Rows.Sum(r => r.Stacks.TakeLast(r.Stacks.Count / 2).Sum(s => s.Weight()));
            Assert.IsTrue(weightLeft - weightRight <= ship.Weight() * 0.2);
        }

        Ship RandomValidShip()
        {
            Random random = new();
            List<Container> containers = Enumerable.Repeat(0, 250).Select(_ => new Container(random.Next(Container.MIN_WEIGHT, Container.MAX_WEIGHT), random.Next(0, 1) > 0, random.Next(0, 1) > 0)).ToList();
            Ship ship = new(5, 10);
            ship.AddContainers(containers);
            return ship;
        }
    }
}