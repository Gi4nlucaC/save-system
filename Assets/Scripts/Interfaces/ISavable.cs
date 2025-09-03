public interface ISavable
{
    string PersistentId { get; }
    public EntityData SaveData();

    public void LoadData();

    public void DeleteData();
}