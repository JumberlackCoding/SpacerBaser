using UnityEngine;
using System.Collections;

public class Enemy_Mothership : GenericEnemyScript {

    public GameObject SapperPreFab;
    public Sprite[] sprites;
    public GameObject[] DockingPoints;
    public GameObject SpawnPoint1;
    public GameObject SpawnPoint2;

    public int maxPowerStored;
    public int maxSappers;

    public float sapperChargeDelay;

    private SpriteRenderer spriteManager;

    private int powerStored;
    private int sapperCount;
    private int sapperCharges;
    private int sappersLaunched;

    void Awake()
    {
        spriteManager = GetComponent<SpriteRenderer>();
    }

	// Use this for initialization
	void Start () {
        powerStored = 0;
        sapperCount = 0;
        sapperCharges = 0;
        sappersLaunched = 0;
        StartCoroutine( GetCharges() );
        StartCoroutine( CheckCloserTarget() );
	}
	
	// Update is called once per frame
	void Update () {

        if( powerStored > maxPowerStored )
        {
            powerStored = maxPowerStored;
        }
        if( powerStored < 0 )
        {
            powerStored = 0;
        }

        // change sprite based on power stored
	    if( powerStored == maxPowerStored )
        {
            spriteManager.sprite = sprites[5];
        }
        else if( powerStored >= maxPowerStored * 4 / 5 )
        {
            spriteManager.sprite = sprites[4];
        }
        else if( powerStored >= maxPowerStored * 3 / 5 )
        {
            spriteManager.sprite = sprites[3];
        }
        else if( powerStored >= maxPowerStored * 2 / 5 )
        {
            spriteManager.sprite = sprites[2];
        }
        else if( powerStored >= maxPowerStored * 1 / 5 )
        {
            spriteManager.sprite = sprites[1];
        }
        else if( powerStored >= 0 )
        {
            spriteManager.sprite = sprites[0];
        }


        if( ( sapperCount < maxSappers ) && ( sapperCharges > 0 ) )
        {
            sapperCharges--;
            sapperCount++;

            Vector3 spawnpos;
            
            // alternate sides that launch sappers
            if( sappersLaunched % 2 == 0 )
            {
                spawnpos = SpawnPoint1.transform.position;
            }
            else
            {
                spawnpos = SpawnPoint2.transform.position;
            }
            Instantiate( SapperPreFab, spawnpos, transform.rotation );
            sappersLaunched++;
        }


        // usual enemy behaviour
        if( target == null )
        {
            target = AcquireTarget();

            if( target == null )
            {
                targetLocation = transform.position;
                totalDistance = 0f;
                haveTarget = false;
                inRange = false;
            }
            else
            {
                called = true;
            }
        }
        if( target != null )
        {
            if( called )
            {
                haveTarget = true;
                targetLocation = target.transform.position;
                startTime = Time.time;
                totalDistance = Vector2.Distance( transform.position, targetLocation );
                targetScript = target.GetComponent<GenericStructureScript>();
                called = false;
            }
        }
        if( haveTarget )
        {
            float distanceFromTarget = Vector2.Distance( transform.position, targetLocation );
            if( distanceFromTarget > range )
            {
                inRange = false;
                checkTimeDelay = Random.Range( 6.8f, 7.21f );
            }
            else
            {
                checkTimeDelay = Random.Range( 1.8f, 2.21f );
            }
            if( inRange == false )
            {
                MoveToTarget();
            }
            if( ( inRange == true ) && ( startedAttacking == false ) )
            {
                startedAttacking = true;
                AttackTarget();
            }
        }

        // handle health bars
        if( health < maxHealth )
        {
            if( !healthBarBackMade )
            {
                healthBarBackObj = (GameObject)Instantiate( healthBarBackPreFab, new Vector3( transform.position.x, transform.position.y + 0.1f, transform.position.z - 0.5f ), Quaternion.identity );

                healthBarBackMade = true;
            }
            if( !healthBarFrontMade )
            {
                healthBarFrontObj = (GameObject)Instantiate( healthBarFrontPreFab, new Vector3( transform.position.x, transform.position.y + 0.1f, transform.position.z - 1.0f ), Quaternion.identity );
                healthBarFrontMade = true;
            }

            if( healthBarBackObj )
            {
                healthBarBackObj.transform.position = new Vector3( transform.position.x, transform.position.y + 0.1f, transform.position.z - 0.5f );
            }

            if( healthBarFrontObj )
            {
                float healthPercent = (float)health / maxHealth;
                healthBarFrontObj.transform.localScale = new Vector3( 0.2f * healthPercent, healthBarFrontObj.transform.localScale.y, healthBarFrontObj.transform.localScale.z );
                healthBarFrontObj.transform.position = new Vector3( transform.position.x - 0.1f + ( 0.1f * healthPercent ), transform.position.y + 0.1f, transform.position.z - 1.0f );
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

    public void DropOffPower( int amount )
    {
        powerStored += amount;
    }

    IEnumerator GetCharges()
    {
        while( true )
        {
            yield return new WaitForSeconds( sapperChargeDelay );

            if( sapperCharges < maxSappers )
            {
                sapperCharges++;
            }
        }
    }
}
