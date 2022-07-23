using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using static AbstractEnumSpecifiedClassConverter<AbstractSerialisationModel, SerialisationModelType>;

public class SerialisationController
{
    private static SemaphoreSlim FileAccessSemaphore { get; } = new SemaphoreSlim(1, 1);

    public List<AbstractSerialisationModel> LoadedModels { get; private set; } = new List<AbstractSerialisationModel>();

    /// <summary>
    /// The default location for saving anything using the SaveSystem
    /// </summary>
    /// <returns></returns>
    private string SaveDirectory => Path.Combine("C:\\Users\\jbrew\\Desktop", "Save Files");

    /// <summary>
    /// A hash set containing all observers to the saving controller
    /// </summary>
    /// <value></value>
    private HashSet<ISerialisable> Observers { get; } = new HashSet<ISerialisable>();

    public void Subscribe(ISerialisable serialisable)
    {
        Observers.Add(serialisable);
    }

    /// <summary>
    /// Saves all registered observes to specified file name
    /// </summary>
    /// <param name="fileName"></param>
    public async Task SaveAll(string fileName)
    {
        foreach (ISerialisable serialisable in Observers)
        {
            await Save(serialisable, fileName);
        }
    }

    /// <summary>
    /// Saves given serialisable to given file name
    /// </summary>
    /// <param name="serialisable"></param>
    /// <param name="fileName"></param>
    public async Task Save(ISerialisable serialisable, string fileName)
    {
        string jsonObject = JsonConvert.SerializeObject(serialisable.GetModel(), new SerialisationModelConverter());
        await WriteToFile(jsonObject, Path.Combine(SaveDirectory, fileName));
    }

    /// <summary>
    /// Loads an abstract saveables Model data.
    /// </summary>
    /// <param name="saveable"></param>
    /// <returns>True if successfully loaded, False otherwise</returns>
    public bool Load(string fileName)
    {
        if (!TryReadFromFile(Path.Combine(SaveDirectory, fileName), out string json))
        {
            Debug.LogWarning($"Failed To Read File: {fileName}");
            return false;
        }

        LoadedModels = JsonConvert.DeserializeObject<List<AbstractSerialisationModel>>(json, new SerialisationModelConverter());

        return true;
    }

    #region IO

    /// <summary>
    /// Writes json data to the supplied filepath.
    /// </summary>
    /// <param name="json"></param>
    /// <param name="filepath"></param>
    private async Task WriteToFile(string json, string filepath)
    {
        await FileAccessSemaphore.WaitAsync();
        if (!Directory.Exists(Path.GetDirectoryName(filepath)))
            Directory.CreateDirectory(Path.GetDirectoryName(filepath));

        if (File.Exists(filepath))
            File.Create(filepath).Dispose();

        File.WriteAllText(filepath, json);
        FileAccessSemaphore.Release();
    }

    /// <summary>
    /// Reads json data from the supplied file path.
    /// </summary>
    /// <param name="filepath"></param>
    /// <param name="json"></param>
    /// <returns>True if successful, False otherwise.</returns>
    private bool TryReadFromFile(string filepath, out string json)
    {
        FileAccessSemaphore.Wait();
        if (!File.Exists(filepath))
        {
            json = null;
            FileAccessSemaphore.Release();
            return false;
        }
        json = File.ReadAllText(filepath);
        FileAccessSemaphore.Release();
        return true;
    }

    #endregion
}
