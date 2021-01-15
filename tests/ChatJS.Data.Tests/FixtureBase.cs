using AutoFixture;

namespace ChatJS.Data.Tests
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
