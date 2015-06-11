using UnityEngine;
using System.Collections;

public class SolarCollectorScript : GenericStructureScript {

    private bool solarGenLevel_1 = false;
    private bool solarGenLevel_2 = false;

    [SerializeField]
    private ParticleSystem UpgradePartEmitter;
    [SerializeField]
    private int Level_1_Power_Gen;
    [SerializeField]
    private int Level_2_Power_Gen;
    

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
                    if( !GetSolarGenLevel_1() && !GetSolarGenLevel_2() ) // built to be level 1
                    {
                        oneTimeCall = false;
                        SetSolarGenLevel_1( true );
                        SetSolarGenLevel_2( false );
                        powerGeneration = Level_1_Power_Gen;
                    }

                    else if( GetSolarGenLevel_1() && !GetSolarGenLevel_2() ) // after building upgrade to level 2
                    {
                        oneTimeCall = false;
                        SetSolarGenLevel_1( false );
                        SetSolarGenLevel_2( true );
                        UpgradePartEmitter.Play();
                        powerGeneration = Level_2_Power_Gen;
                    }
                }
            }

            // handle health bars
            if( health < maxHealth )
            {
                if( !healthBarBackMade )
                {
                    healthBarBackObj = (GameObject)Instantiate( healthBarBackPreFab, new Vector3( transform.position.x, transform.position.y + 0.18f, transform.position.z - 0.5f ), Quaternion.identity );

                    healthBarBackMade = true;
                }
                if( !healthBarFrontMade )
                {
                    healthBarFrontObj = (GameObject)Instantiate( healthBarFrontPreFab, new Vector3( transform.position.x, transform.position.y + 0.18f, transform.position.z - 1.0f ), Quaternion.identity );
                    healthBarFrontMade = true;
                }

                if( healthBarBackObj )
                {
                    healthBarBackObj.transform.position = new Vector3( transform.position.x, transform.position.y + 0.18f, transform.position.z - 0.5f );
                }

                if( healthBarFrontObj )
                {
                    float healthPercent = (float)health / maxHealth;
                    healthBarFrontObj.transform.localScale = new Vector3( 0.2f * healthPercent, healthBarFrontObj.transform.localScale.y, healthBarFrontObj.transform.localScale.z );
                    healthBarFrontObj.transform.position = new Vector3( transform.position.x - 0.1f + ( 0.1f * healthPercent ), transform.position.y + 0.18f, transform.position.z - 1.0f );
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

    public void UpgradeToGenLevel_2()
    {
        Initialize();
        built = false;
        oneTimeCall = true;
        powerManager.Trees.UpdateNode( gameObject, powerManager.NodesRecord, 0, 0, false, false );
    }

    public bool GetSolarGenLevel_1()
    {
        return solarGenLevel_1;
    }

    public void SetSolarGenLevel_1( bool set )
    {
        solarGenLevel_1 = set;
    }

    public bool GetSolarGenLevel_2()
    {
        return solarGenLevel_2;
    }

    public void SetSolarGenLevel_2( bool set )
    {
        solarGenLevel_2 = set;
    }
}
