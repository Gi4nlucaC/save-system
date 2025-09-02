public interface ISavable
{
    string PersistentId { get; }
    public string SaveData();

    public ISavableData LoadData(string data);

    public void DeleteData();
}