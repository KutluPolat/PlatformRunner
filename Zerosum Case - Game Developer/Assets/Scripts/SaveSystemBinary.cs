using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystemBinary<T>
{
#pragma warning disable CS0162 // Unreachable code detected warning disabled

    private const string EXTENSION_NAME = "platformrunner";
    private const bool IS_SAVE_SYSTEM_ON = true;

    public static void Save(string name, T value)
    {
        if (IS_SAVE_SYSTEM_ON == false)
            return;


        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + name + "." + EXTENSION_NAME;
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, value);
        stream.Close();
    }

    public static T Load(string name, T nullVal)
    {
        if (IS_SAVE_SYSTEM_ON == false)
            return nullVal;

        string path = Application.persistentDataPath + "/" + name + "." + EXTENSION_NAME;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            T loadedGeneric = (T)formatter.Deserialize(stream);
            stream.Close();

            return loadedGeneric;
        }
        else
        {
            return nullVal;
        }
    }

#pragma warning restore CS0162 // Unreachable code detected enabled again
}
