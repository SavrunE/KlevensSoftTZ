using Core.Server;
using NUnit.Framework;

[TestFixture]
public class ServerTests
{
    [Fact]
    public void ConcurrentReaders_ShouldReadCorrectCount()
    {
        // Arrange
        Server.AddToCount(10);
        int totalReaders = 10;
        int expectedCount = Server.GetCount();
        int actualCount = 0;

        Parallel.For(0, totalReaders, _ =>
        {
            actualCount += Server.GetCount();
        });

        // Act & Assert
        Xunit.Assert.Equal(expectedCount * totalReaders, actualCount);
        Server.ResetCount();
    }

    [Fact]
    public void ItShould_writes_not_interfere_with_each_other()
    {
        // Arrange
        int writerCount = 5;
        int valueToAdd = 10;

        Parallel.For(0, writerCount, _ =>
        {
            Server.AddToCount(valueToAdd);
        });

        // Act
        var finalCount = Server.GetCount();

        // Assert
        Xunit.Assert.Equal(valueToAdd * writerCount, finalCount);
        Server.ResetCount();
    }

    [Fact]
    public void ItShould_wait_writers()
    {
        // Arrange
        var taskCompletionSource = new TaskCompletionSource<bool>();
        var readerTask = Task.Run(() =>
        {
            // Reader should wait until writer is done
            int count = Server.GetCount();
            taskCompletionSource.SetResult(true);
            return count;
        });

        // Simulate a writer that takes some time
        Task.Run(() =>
        {
            Thread.Sleep(100); // Simulating work
            Server.AddToCount(5);
        });

        // Act
        var readerCompleted = taskCompletionSource.Task.Wait(200); // Wait for reader to complete

        // Assert
        Xunit.Assert.True(readerCompleted);
        Server.ResetCount();
    }


    [Fact]
    public void ItShould_getCount_be_zero()
    {
        // Arrange
        var initialValue = Server.GetCount();

        // Act & Assert
        Xunit.Assert.Equal(0, initialValue);
    }
}