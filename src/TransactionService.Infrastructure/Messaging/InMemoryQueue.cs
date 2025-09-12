using System;

namespace TransactionService.Infrastructure.Messaging
{
    public interface IInMemoryQueue
    {
        void Enqueue(InMemoryQueueItem item);
        bool TryDequeue(out InMemoryQueueItem? item);
    }

    public class InMemoryQueueItem { public string Id { get; set; } = ""; public string Payload { get; set; } = ""; }

    public class InMemoryQueue : IInMemoryQueue
    {
        private readonly System.Collections.Concurrent.ConcurrentQueue<InMemoryQueueItem> _q = new();
        public void Enqueue(InMemoryQueueItem item) => _q.Enqueue(item);
        public bool TryDequeue(out InMemoryQueueItem? item) => _q.TryDequeue(out item);
    }
}