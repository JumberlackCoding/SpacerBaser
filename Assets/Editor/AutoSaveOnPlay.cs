using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


[InitializeOnLoad]
public class OnUnityLoad
{
    static OnUnityLoad()
    {
        EditorApplication.playmodeStateChanged = () =>
        {
            if( EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying )
            {
                Debug.Log( "Autosaving scene before entering play mode: " + EditorApplication.currentScene );

                EditorApplication.SaveScene();
                EditorApplication.SaveAssets();
            }
        };
    }
}
