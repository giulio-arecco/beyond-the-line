namespace Architecture.Interfaces {
    public interface ILateUpdateObserver {
        public int LateUpdatePriority { get; set; }

        public void ObservedLateUpdate();
    }
}