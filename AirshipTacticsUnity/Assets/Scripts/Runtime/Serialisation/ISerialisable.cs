public interface ISerialisable
{
    public AbstractSerialisationModel GetModel();
    public void Load(AbstractSerialisationModel model);
}
