using Xunit;
using FluentAssertions;

namespace SampleTests
{
    public class SampleTest
    {
        [Fact]
        public void Example_Test_ShouldPass()
        {
            int result = 2 + 2;
            result.Should().Be(4);
        }
    }
}
