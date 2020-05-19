using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LINA.Core.Infrastructure.Lock
{
    public static class ReaderWriterLockSlimExtensions
    {
        private sealed class ReadLockToken : IDisposable
        {
            private ReaderWriterLockSlim _sync;

            public ReadLockToken(ReaderWriterLockSlim sync)
            {
                _sync = sync;
                sync.EnterReadLock();
            }

            public void Dispose()
            {
                if (_sync.IsReadLockHeld)
                {
                    _sync.ExitReadLock();
                }
            }
        }

        private sealed class WriteLockToken : IDisposable
        {
            private ReaderWriterLockSlim _sync;

            public WriteLockToken(ReaderWriterLockSlim sync)
            {
                _sync = sync;
                sync.EnterWriteLock();
            }

            public void Dispose()
            {
                if (_sync.IsWriteLockHeld)
                {
                    _sync.ExitWriteLock();
                }
            }
        }

        private sealed class TryWriteLockToken : IDisposable
        {
            private ReaderWriterLockSlim _sync;

            public TryWriteLockToken(ReaderWriterLockSlim sync, int waitingTime)
            {
                _sync = sync;
                sync.TryEnterWriteLock(waitingTime);
            }

            public void Dispose()
            {
                if (_sync.IsWriteLockHeld)
                {
                    _sync.ExitWriteLock();
                }
            }
        }

        public static IDisposable Read(this ReaderWriterLockSlim obj)
        {
            return new ReadLockToken(obj);
        }

        public static IDisposable Write(this ReaderWriterLockSlim obj)
        {
            return new WriteLockToken(obj);
        }

        /// <summary>
        /// if any thread already wrote on implemented code block;
        /// new threads waits to completed the thread that already doing own job,
        /// or you can set waitingTime. after expiring this time;
        /// this thread will not waiting the inner thread, it start the doing job too like thread like already doing job
        /// so, deadlock will never happen, just give the some time to inner thread,
        /// if it cannot be giving response w/ any reason or get unhandled exception,
        /// (after expiring time) then new thread going the job.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="waitingTime"></param>
        /// <returns></returns>
        public static IDisposable TryWrite(this ReaderWriterLockSlim obj, int waitingTime)
        {
            return new TryWriteLockToken(obj, waitingTime);
        }
    }

}
