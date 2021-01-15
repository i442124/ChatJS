using AutoFixture;

namespace ChatJS.Domain.Tests
{
    public class FixtureBase
    {
        public Fixture Fixture { get; set; }

        public FixtureBase()
        {
            Fixture = new Fixture();
        }
    }
}
