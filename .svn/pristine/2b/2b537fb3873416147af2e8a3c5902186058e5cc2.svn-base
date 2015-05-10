using UnityEngine;
using System.Collections;

public class SolarCollectorScript : GenericStructureScript {
	
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
                    //				powerManager.UpdateGenerator( powerGeneration, maxStoredPower, gameObject );
                }
            }

            if( health <= 0 )
            {
                StartCoroutine( Die( false, 0 ) );
            }
        }
	}
}
