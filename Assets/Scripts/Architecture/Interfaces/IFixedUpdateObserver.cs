namespace Architecture.Interfaces {
    public interface IFixedUpdateObserver {
        public int FixedUpdatePriority { get; set; }

        public void ObservedFixedUpdate();
    }
}