public interface ISaveStorage
{
    void Write(string slotId, string content);
    string Read(string slotId);
    bool Exists(string slotId);
    void Delete(string slotId);
}
