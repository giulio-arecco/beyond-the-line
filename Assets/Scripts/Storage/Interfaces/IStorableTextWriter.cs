using Storage.Storables;

namespace Storage.Interfaces {
    public interface IStorableTextWriter {
        public void SetNameText(Storable storable);  
        public void SetDescriptionText(Storable storable);  
        public void SetOtherText(Storable storable);
        public void ClearAllText();
    }
}
