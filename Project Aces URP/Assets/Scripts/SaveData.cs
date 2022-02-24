using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveData
{
    public static void SaveProfile(ProfileHandler profile){
        string path = Application.persistentDataPath + "/profile.ace";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerProfileData data = new PlayerProfileData(profile); //creates the data
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("SaveData: SaveProfile(), profile named '" + data.profileName + "' saved to " + path);
    }

    public static PlayerProfileData LoadProfile(){
        string path = Application.persistentDataPath + "/profile.ace";

        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerProfileData data = formatter.Deserialize(stream) as PlayerProfileData;

            stream.Close();
            return data;
        }
        else{
            Debug.LogWarning("SaveData: LoadProfile(), Save file not found in " + path);
            return null;
        }
    }

}
