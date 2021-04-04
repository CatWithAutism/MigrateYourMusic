using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MusicCarriers.Utils;

namespace MusicCarriers.AsynchronousModels
{
    public class TaskQueue
    {
        public class QueueElement
        {
            public Action<Exception> OnError { get; set; }
            public Action<object> OnEnd { get; set; }
            public Action<object, int> OnPositionUpdate { get; set; }
            public Action<object> OnStart { get; set; }
            public Func<Task> TaskGenerator { get; set; }
        }

        private SemaphoreSlim _semaphore;
        
        /// <summary>
        ///     The default constructor. 
        /// </summary>
        /// <param name="numberOfTasks">Number of tasks are working at the same moment.</param>
        public TaskQueue(int numberOfTasks)
        {
            Guarantee.IsGreaterThan(numberOfTasks, 0, nameof(numberOfTasks));
            
            _semaphore = new SemaphoreSlim(numberOfTasks);
        }
        
        public async Task Enqueue(QueueElement queueElement)
        {
            Guarantee.IsArgumentNotNull(queueElement, nameof(queueElement));
            Guarantee.IsArgumentNotNull(queueElement, nameof(queueElement.TaskGenerator));
            
            await _semaphore.WaitAsync();
            try
            {
                await queueElement.TaskGenerator();
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
    }
}