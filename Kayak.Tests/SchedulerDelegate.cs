namespace Kayak.Tests
{
    using System;

    public class SchedulerDelegate : IScheduler
    {
        #region Implementation of IDisposable

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of IScheduler

        public void Post(Action action)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}