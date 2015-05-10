using UnityEngine;
using System.Collections;

public class EnergyNodeScript : GenericStructureScript {

	// Use this for initialization
	void Start () {
		Initialize();
	}
	
	// Update is called once per frame
	void Update () {
        if( !dead )
        {
            CheckBeamCount();
            if( !built )
            {
                Build();
            }
            else
            {
                if( oneTimeCall )
                {
                    oneTimeCall = false;
                }
            }
            if( health <= 0 )
            {
                StartCoroutine( Die( false, 0 ) );
            }
        }
	}
}
