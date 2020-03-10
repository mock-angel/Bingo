using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    public static void SavePlayer(Player player){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerData.fun";
        
//        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        Debug.LogError("SavePlayer : opening stream");
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData(player);
//        
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.LogError("SavePlayer : Closed stream");
//        using (var stream = File.Open(path, FileMode.Create))
//        {
//            PlayerData data = new PlayerData(player);
//        
//            formatter.Serialize(stream, data);
//        }
        

    }
    
    public static PlayerData LoadPlayer(){
    
        string path = Application.persistentDataPath + "/playerData.fun";
        if (File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            Debug.LogError("LoadPlayer : opening stream");
            FileStream stream = new FileStream(path, FileMode.Open);
            
            stream.Position = 0;
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            Debug.LogError("LoadPlayer : Closed stream");
//            using (var stream = File.Open(path, FileMode.Open))
//            {
//                data = formatter.Deserialize(stream) as PlayerData;
//            }
            
            return data;
            
        } else {
            Debug.LogError("Save file not found in " + path);
            
            return null;
        }
    }
    
}
