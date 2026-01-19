using System.Collections.Generic;
using Architecture.Interfaces;

namespace Architecture {
    public class UpdateManager : PersistentSingleton<UpdateManager> {
        private readonly List<IUpdateObserver> _observers = new();
        private readonly List<IUpdateObserver> _pendingObservers = new();
        private int _currentIndex; 
        private bool _hasPendingObservers;

        public void Update() {
            if(_hasPendingObservers) {
                _observers.AddRange(_pendingObservers);
                _pendingObservers.Clear();
                _hasPendingObservers = false;
            
                // Sort in descending order (lowest priority at the end of the list)
                _observers.Sort((a, b) => b.UpdatePriority.CompareTo(a.UpdatePriority));
            }

            for (_currentIndex = _observers.Count - 1; _currentIndex >= 0; _currentIndex--) {
                _observers[_currentIndex].ObservedUpdate();
            }
        }

        private void OnDestroy() {
            _observers.Clear();
            _pendingObservers.Clear();
        }
    
        /// <summary>
        /// Registers a script implementing the IUpdateObserver interface to the UpdateManager's queue.
        /// It's advised calling this method in the OnEnable method of the script implementing the IUpdateObserver interface.
        /// </summary>
        /// <param name="observer"> The observer to be registered </param>
        public void Register(IUpdateObserver observer) {
            _pendingObservers.Add(observer);
            _hasPendingObservers = true;
        }

        /// <summary>
        /// Unregisters a script implementing the IUpdateObserver interface from the UpdateManager's queue.
        /// It's advised calling this method in the OnDisable method of the script implementing the IUpdateObserver interface.
        /// </summary>
        /// <param name="observer"> The observer to be unregistered </param>
        public bool Unregister(IUpdateObserver observer) {
            var pendingRemoved = _pendingObservers.Remove(observer);
            _hasPendingObservers = _pendingObservers.Count > 0;
        
            var index = _observers.IndexOf(observer);
            var observerRemoved = false;
        
            if (index >= 0) {
                _observers.RemoveAt(index);
                if (index < _currentIndex) _currentIndex--;
                observerRemoved = true;
            }

            return observerRemoved || pendingRemoved;
        }
    }
}
