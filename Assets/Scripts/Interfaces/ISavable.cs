namespace PeraphsPizza.SaveSystem
{
    public interface ISavable
    {
        string PersistentId { get; }
        public PureRawData SaveData();
        public  void LoadData();
        public void DeleteData();
        public void SnapshotData();
    }
}