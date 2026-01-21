namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ItShould_string_compare_correctly()
        {
            // Arrange
            string input = "aaabbcccdde";
            string expectedCompressed = "a3b2c3d2e";
            var sut = new Compression();
            var actualCompressed = sut.Compress(input);
            // Act
            // Assert
            Assert.Equal(expectedCompressed, actualCompressed);
        }
    }
}
