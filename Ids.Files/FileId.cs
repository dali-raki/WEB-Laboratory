namespace Ids.Files;

public class FileId
{
    public Guid Value { get; set; }

    public FileId(Guid value) => Value = value;

    public FileId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            Value = Guid.NewGuid();
            return;
        }

        try
        {
            Value = Guid.Parse(value);
        }
        catch
        {
            throw new Exception($"Bad format: {value}");
        }
    }

    public FileId() => Value = Guid.NewGuid();

    public static bool operator ==(FileId a, FileId b) => a.Value.Equals(b.Value);

    public static bool operator !=(FileId a, FileId b) => !(a == b);
}