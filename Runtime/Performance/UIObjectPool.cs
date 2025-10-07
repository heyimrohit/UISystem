using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aetheriaum.UISystem.Performance
{
    public class UIObjectPool<T> where T : class, new()
    {
        private readonly Stack<T> _pool = new();
        private readonly Func<T> _createFunction;
        private readonly Action<T> _resetAction;

        public UIObjectPool(Func<T> createFunction, Action<T> resetAction, int initialSize = 10)
        {
            _createFunction = createFunction;
            _resetAction = resetAction;

            // Pre-populate pool
            for (int i = 0; i < initialSize; i++)
            {
                _pool.Push(_createFunction());
            }
        }

        public T Get()
        {
            if (_pool.Count > 0)
            {
                var item = _pool.Pop();
                return item;
            }

            return _createFunction();
        }

        public void Return(T item)
        {
            _resetAction?.Invoke(item);
            _pool.Push(item);
        }
    }

    public class UIUpdateBatcher
    {
        private readonly List<Action> _pendingUpdates = new();
        private readonly List<Action> _frameUpdates = new();
        private float _updateInterval = 1f / 60f; // 60 FPS
        private float _lastUpdateTime;

        public void QueueUpdate(Action updateAction)
        {
            _pendingUpdates.Add(updateAction);
        }

        public void ProcessUpdates()
        {
            if (Time.time - _lastUpdateTime >= _updateInterval)
            {
                _frameUpdates.Clear();
                _frameUpdates.AddRange(_pendingUpdates);
                _pendingUpdates.Clear();

                foreach (var update in _frameUpdates)
                {
                    update?.Invoke();
                }

                _lastUpdateTime = Time.time;
            }
        }
    }
}