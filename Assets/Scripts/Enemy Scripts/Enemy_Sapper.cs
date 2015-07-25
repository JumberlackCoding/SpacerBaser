using UnityEngine;
using System.Collections;

public class Enemy_Sapper : GenericEnemyScript {

	// Use this for initialization
	void Start () {
        StartCoroutine( CheckCloserTarget() );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
