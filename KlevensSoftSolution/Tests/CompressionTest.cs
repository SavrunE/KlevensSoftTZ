namespace Tests
{
    public class CompressTest
    {
        [Fact]
        public void ItShould_string_compare_correctly()
        {
            // Arrange
            string input = "aaabbcccdde";
            string expectedCompressed = "a3b2c3d2e";
            var sut = new Compression();

            // Act
            var actualCompressed = sut.Compress(input);

            // Assert
            Xunit.Assert.Equal(expectedCompressed, actualCompressed);
        }
    }
}
