public interface ISavable
{
    string PersistentId { get; }
    public EntityData SaveData();

    public ISavableData LoadData(string data);

    public void DeleteData();
}