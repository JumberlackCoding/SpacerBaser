using UnityEngine;
using System.Collections;

public class LightScript : MonoBehaviour {
	
	void Awake()
	{
		GameObject[] temp = GameObject.FindGameObjectsWithTag( "DirectionalLight" );
		for( int i = 0; i < temp.Length; i++ )
		{
			if( temp[i] != gameObject )
			{
				Destroy( gameObject );
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad( gameObject );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
