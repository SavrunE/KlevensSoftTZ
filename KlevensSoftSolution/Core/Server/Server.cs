namespace Core.Server
{
    public interface ICounter
    {
        public void ResetCount();
        public int GetCount();
        public void AddToCount(int value);
    }

    public static class Server
    {
        public static ICounter Instance { get; set; } = new ServerInstance();
        public static int GetCount() => Instance.GetCount();
        public static void AddToCount(int value) => Instance.AddToCount(value);

        public static void ResetCount() => Instance.ResetCount();

    }

    public class ServerInstance : ICounter
    {
        private static int count;
        private static readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

        public void ResetCount()
        {
            count = 0;
        }

        public int GetCount()
        {
            rwLock.EnterReadLock();
            try
            {
                return count;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public void AddToCount(int value)
        {
            if (value > 0)
            {
                rwLock.EnterWriteLock();
                try
                {
                    count += value;
                }
                finally
                {
                    rwLock.ExitWriteLock();
                }
            }
        }
    }
}