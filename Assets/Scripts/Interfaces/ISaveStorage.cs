using System.Threading.Tasks;

public interface ISaveStorage<T>
{
    void Write(string slotId, T content);
    Task WriteAsync(string slotId, T content);
    T Read(string slotId);
    Task<T> ReadAsync(string slotId);
    bool Exists(string slotId);
    void Delete(string slotId);
}
