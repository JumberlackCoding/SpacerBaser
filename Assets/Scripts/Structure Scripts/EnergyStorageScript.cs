﻿using UnityEngine;
using System.Collections;

public class EnergyStorageScript : GenericStructureScript {

	// Use this for initialization
	void Start () {
		Initialize();
	}
	
	// Update is called once per frame
	void Update () {
        if( !dead )
        {
            if( !built )
            {
                Build();
            }
            else
            {
                if( oneTimeCall )
                {
                    oneTimeCall = false;
                    //				powerManager.UpdateBattery( maxStoredPower, gameObject );
                }
            }
            if( health <= 0 )
            {
                StartCoroutine( Die( false, 0 ) );
            }
        }
	}
	
	public override void StructureFunction()
	{
		// take in power and store it and release power when needed
	}
}
