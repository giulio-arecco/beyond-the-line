public interface IUpdateObserver {
    public int UpdatePriority { get; set; }

    public void ObservedUpdate();
}
