using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.WebServer.Hubs
{
    public class HubSubscriptionMapper<T> : IHubSubscriptionMapper<T>
    {
        private readonly ConcurrentDictionary<T, HashSet<string>> _subscriptions =
        new ConcurrentDictionary<T, HashSet<string>>();

        private readonly ConcurrentDictionary<string, HashSet<T>> _subscribers =
        new ConcurrentDictionary<string, HashSet<T>>();

        public int SubscriberCount
        {
            get { return _subscribers.Count; }
        }

        public int SubscriptionCount
        {
            get { return _subscriptions.Count; }
        }

        public void AddSubscriber(string subscriberId)
        {
            if (!_subscribers.TryAdd(subscriberId, new HashSet<T>()))
            {
                throw new ArgumentException("Subscriber already exists.");
            }
        }

        public Task AddSubscriberAsync(string subscriberId)
        {
            return Task.Run(() => AddSubscriber(subscriberId));
        }

        public void RemoveSubscriber(string subscriberId)
        {
            if (!_subscribers.TryRemove(subscriberId, out HashSet<T> subscriptions))
            {
                throw new ArgumentException("Subscriber does not exists.");
            }

            foreach (var subscription in subscriptions)
            {
                var hashSet = _subscriptions[subscription];
                hashSet.Remove(subscriberId);

                if (hashSet.Count == 0)
                {
                    _subscriptions.TryRemove(subscription, out _);
                }
            }
        }

        public Task RemoveSubscriberAsync(string subscriberId)
        {
            return Task.Run(() => RemoveSubscriber(subscriberId));
        }

        public void Subscribe(string subscriberId, T subscription)
        {
            if (!_subscribers.TryGetValue(subscriberId, out HashSet<T> subscriptions))
            {
                throw new ArgumentException("Subscriber does not exists.");
            }
            else if (!subscriptions.Add(subscription))
            {
                throw new ArgumentException("Subscriber is already subscribed to the subscription.");
            }

            _subscriptions.TryAdd(subscription, new HashSet<string>());
            _subscriptions[subscription].Add(subscriberId);
        }

        public Task SubscribeAsync(string subscriberId, T subscription)
        {
            return Task.Run(() => Subscribe(subscriberId, subscription));
        }

        public void Unsubscribe(string subscriberId, T subscription)
        {
            if (!_subscribers.TryGetValue(subscriberId, out HashSet<T> subscriptions))
            {
                throw new ArgumentException("Subscriber does not exists.");
            }
            else if (!subscriptions.Remove(subscription))
            {
                throw new ArgumentException("Subscriber is not subscribed to the subscription.");
            }

            _subscriptions[subscription].Remove(subscriberId);
        }

        public Task UnsubscribeAsync(string subscriberId, T subscription)
        {
            return Task.Run(() => Unsubscribe(subscriberId, subscription));
        }

        public IEnumerable<T> GetSubscriptions(string subscriberId)
        {
            if (!_subscribers.TryGetValue(subscriberId, out HashSet<T> subscriptions))
            {
                throw new ArgumentException("Subscriber does not exists.");
            }

            foreach (var subscription in subscriptions)
            {
                yield return subscription;
            }
        }

        public Task<IEnumerable<T>> GetSubscriptionsAsync(string subscriberId)
        {
            return Task.Run(() => GetSubscriptions(subscriberId));
        }

        public IEnumerable<string> GetSubscribers(T subscription)
        {
            if (!_subscriptions.TryGetValue(subscription, out HashSet<string> subscribers))
            {
                throw new ArgumentException("Subscription does not exists.");
            }

            foreach (var subscriber in subscribers)
            {
                yield return subscriber;
            }
        }

        public Task<IEnumerable<string>> GetSubscribersAsync(T subscription)
        {
            return Task.Run(() => GetSubscribers(subscription));
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var subscription in _subscriptions.Keys)
            {
                yield return subscription;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var subscription in _subscriptions.Keys)
            {
                yield return subscription;
            }
        }
    }
}
