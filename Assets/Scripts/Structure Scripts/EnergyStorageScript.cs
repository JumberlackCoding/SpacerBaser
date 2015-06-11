using UnityEngine;
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

            // handle health bars
            if( health < maxHealth )
            {
                if( !healthBarBackMade )
                {
                    healthBarBackObj = (GameObject)Instantiate( healthBarBackPreFab, new Vector3( transform.position.x, transform.position.y + 0.2f, transform.position.z - 0.5f ), Quaternion.identity );

                    healthBarBackMade = true;
                }
                if( !healthBarFrontMade )
                {
                    healthBarFrontObj = (GameObject)Instantiate( healthBarFrontPreFab, new Vector3( transform.position.x, transform.position.y + 0.2f, transform.position.z - 1.0f ), Quaternion.identity );
                    healthBarFrontMade = true;
                }

                if( healthBarBackObj )
                {
                    healthBarBackObj.transform.position = new Vector3( transform.position.x, transform.position.y + 0.2f, transform.position.z - 0.5f );
                }

                if( healthBarFrontObj )
                {
                    float healthPercent = (float)health / maxHealth;
                    healthBarFrontObj.transform.localScale = new Vector3( 0.2f * healthPercent, healthBarFrontObj.transform.localScale.y, healthBarFrontObj.transform.localScale.z );
                    healthBarFrontObj.transform.position = new Vector3( transform.position.x - 0.1f + ( 0.1f * healthPercent ), transform.position.y + 0.2f, transform.position.z - 1.0f );
                }
            }
            else
            {
                if( healthBarBackObj )
                {
                    Destroy( healthBarBackObj );
                    healthBarBackMade = false;
                }
                if( healthBarFrontObj )
                {
                    Destroy( healthBarFrontObj );
                    healthBarFrontMade = false;
                }
            }
        }
	}
	
	public override void StructureFunction()
	{
		// take in power and store it and release power when needed
	}
}
