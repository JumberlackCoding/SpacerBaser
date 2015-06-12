using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game 
{
    public static Game current;
    public GameSave save1;
    public GameSave save2;
    public GameSave save3;

    public Game()
    {
        save1 = new GameSave();
        save2 = new GameSave();
        save3 = new GameSave();
    }
}

[System.Serializable]
public class GameSave
{
    public string name;

    public GameSave()
    {
        this.name = "";
    }
}
