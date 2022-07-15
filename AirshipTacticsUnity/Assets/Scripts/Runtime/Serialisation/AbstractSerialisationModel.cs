using System;

[Serializable]
public abstract class AbstractSerialisationModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string IdString => Id.ToString();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
}
