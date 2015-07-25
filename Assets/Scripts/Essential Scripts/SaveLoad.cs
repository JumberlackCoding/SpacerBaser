using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoad
{
    public static List<Game> savedGames = new List<Game>();

    public static void Save()
    {
        savedGames.Add( Game.current );
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create( Application.persistentDataPath + "/spacer.baser" );
        bf.Serialize( file, SaveLoad.savedGames );
        file.Close();
    }

    public static void Load()
    {
        if( File.Exists( Application.persistentDataPath + "/spacer.baser" ) )
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open( Application.persistentDataPath + "/spacer.baser", FileMode.Open );
            SaveLoad.savedGames = (List<Game>)bf.Deserialize( file );
            file.Close();
        }
    }
}
