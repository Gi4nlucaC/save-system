public interface ISavable
{
    public string SaveData();

    public ISavableData LoadData(string data);

    public void DeleteData();
}