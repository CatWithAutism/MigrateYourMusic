using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebHandlers.Utils
{
    public class TaskQueue<T>
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly ConcurrentQueue<QueueElement> _taskQueue;

        private volatile bool _isWorking;

        /// <summary>
        ///     The default constructor.
        /// </summary>
        /// <param name="maxRunningTasks">Maximum of running tasks at the same time.</param>
        public TaskQueue(int maxRunningTasks)
        {
            _semaphore = new SemaphoreSlim(maxRunningTasks);
            _taskQueue = new ConcurrentQueue<QueueElement>();
        }

        public bool IsWorking => _isWorking;

        public event Action<int> OnQueueUpdated;


        public void EnqueueTask(Func<Task<T>> taskGenerator, Action onStart, Action onCrash, Action<T> onEnd,
            Action<int> onPositionUpdated)
        {
            Guarantee.IsArgumentNotNull(taskGenerator, nameof(taskGenerator));
            _taskQueue.Enqueue(new QueueElement
            {
                TaskGenerator = taskGenerator,
                OnStarted = onStart,
                OnEnded = onEnd,
                OnCrashed = onCrash,
                OnPositionUpdated = onPositionUpdated
            });
        }

        public async Task Start()
        {
            if (_taskQueue.Count > 0)
                _isWorking = true;

            await _semaphore.WaitAsync();

            while (_taskQueue.Count > 0)
                if (_taskQueue.TryDequeue(out var queueElement))
                {
                    for (var j = 0; j < _taskQueue.Count; j++)
                    {
                        var tmp = _taskQueue.ElementAt(j);
                        tmp.OnPositionUpdated(j);
                    }

                    var task = queueElement.TaskGenerator();
                    try
                    {
                        queueElement.OnStarted();
                        await task;
                    }
                    finally
                    {
                        if (task.IsCompleted)
                            queueElement.OnEnded(task.Result);
                        else
                            queueElement.OnCrashed();
                        _semaphore.Release();
                    }
                }

            if (_taskQueue.Count == 0)
                _isWorking = false;
        }

        private class QueueElement
        {
            public Action OnCrashed;
            public Action<T> OnEnded;
            public Action<int> OnPositionUpdated;
            public Action OnStarted;
            public Func<Task<T>> TaskGenerator;
        }
    }
}