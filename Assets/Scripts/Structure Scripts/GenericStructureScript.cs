﻿using UnityEngine;
using System.Collections;

public class GenericStructureScript : MonoBehaviour {
    public ParticleSystem partSys;

    public int mineralsToBuild;
    public int maxHealth;
    public int damage;
    public float range;
    public int maxBeamsAllowed;
    public int powerGeneration;
    public int powerConsumption;
    public int maxStoredPower;
    public float attackSpeed;

    public int currentPower;
    public int attachedBeamCount = 0;
    public int _health = 1;
    public int powerToBuild;
    public int powerToUpgrade = 10;
    public float buildPercent;
    public float BarOffset;

    public float rangeFromTarget;
    public GameObject target = null;
    public GameObject BeamPreFab;
    public Vector3 targetLocation;
    public GenericEnemyScript targetScript;
    public bool inRange = false;
    public bool placed = false;
    public bool built = false;
    public bool healthBarBackMade = false;
    public bool healthBarFrontMade = false;

    public GameObject BuildBackPreFab;
    public GameObject BuildForePreFab;

    [SerializeField]
    protected GameObject healthBarBackPreFab;
    protected GameObject healthBarBackObj;
    [SerializeField]
    protected GameObject healthBarFrontPreFab;
    protected GameObject healthBarFrontObj;

    protected Vector3 buildBarBackPos;
    protected Vector3 buildBarForePos;
    protected Vector3 buildBarSize;
    protected Vector3 buildBarSizeMax;

    protected bool initial = true;
    protected bool oneTimeCall = true;
    protected bool dead = false;

    protected GameObject foreBuildBar;
    protected GameObject backBuildBar;
    protected GameObject beamObj;

    protected PowerManagerScript powerManager;
    protected SpriteRenderer sprite;

    public int health
    {
        // called when retrieving the value
        get
        {
            return _health;
        }

        // called when setting the value
        set
        {
            _health = value;
            if(_health <= 0)
            {
                Die(false, 0);
            }
        }
    }

    protected virtual void Initialize()
    {
        currentPower = 0;
        powerToBuild = 10;
        BarOffset = 0.3f;
        buildBarSize = new Vector3(0f, 0f, 0f);
        buildBarSizeMax = new Vector3(0.25f, 0.25f, 0.25f);
        sprite = GetComponent<SpriteRenderer>();
        if(powerManager == null)
        {
            powerManager = GameObject.Find("PowerManager").GetComponent<PowerManagerScript>();
        }
    }
	
	protected virtual void Build()
	{
        if( !dead )
        {
            if( placed && !built )
            {
                // jump into this if statement only once to spawn the build bar
                if( initial )
                {
                    buildBarForePos = new Vector3( transform.position.x, transform.position.y + BarOffset, transform.position.z + 1f );
                    buildBarBackPos = new Vector3( transform.position.x, transform.position.y + BarOffset, transform.position.z + 5f );
                    backBuildBar = (GameObject)Instantiate( BuildBackPreFab, buildBarBackPos, Quaternion.identity );
                    foreBuildBar = (GameObject)Instantiate( BuildForePreFab, buildBarForePos, Quaternion.identity );
                    foreBuildBar.transform.localScale = buildBarSize;
                    initial = false;
                }
                if( buildPercent <= 1 )
                {
                    //				currentPower += 0.01f;		// this is for testing purposes only until the power system is up and running
                    buildBarSize = buildBarSizeMax * buildPercent;
                    foreBuildBar.transform.localScale = buildBarSize;
                }
            }
            if( buildPercent >= 1 )
            {
                built = true;
                Destroy( foreBuildBar.gameObject );
                Destroy( backBuildBar.gameObject );
                health = maxHealth;
                UpdatePowerNode();
            }
            buildPercent = (float)currentPower / powerToBuild;
        }
	}
	
	protected virtual void CheckBeamCount()
	{
		if( attachedBeamCount >= maxBeamsAllowed )
		{
			sprite.color = new Color( 1f, 0.5f, 0.5f, 1.0f );
		}
		else
		{
			sprite.color = new Color( 1f, 1f, 1f, 1f );
		}
	}

	protected virtual void CheckInRange()
	{
		rangeFromTarget = Vector2.Distance( transform.position, targetLocation );

		if(rangeFromTarget<=range)
		{
			inRange = true;
		}
		else
		{
			inRange = false;
		}
	}

	public void TakeDamage( int damage )
	{
		health -= damage;
	}
	
	public void HealHealth( int heal )
	{
		health += heal;
		if( health > maxHealth )
		{
			health = maxHealth;
		}
	}
	
	public virtual void StructureFunction()
	{
		
	}

	protected virtual IEnumerator PulseAction()
	{
		float beamLife = 0.2f;
		
		while( true )
		{
			yield return new WaitForSeconds( attackSpeed );
			
			
			
			yield return new WaitForSeconds( beamLife );
			
			
			
		}
	}

	protected virtual GameObject AcquireTarget()
	{
		// Set closest target to ~infinity
		float closest = 100000000;
		
		// Get all enemies
		GameObject[] enemies = GameObject.FindGameObjectsWithTag( "Enemy" );
        GameObject[] wurms = GameObject.FindGameObjectsWithTag( "Wurm" );

		
		// Loop through all the structures, nodes and turrets
		GameObject tempTarget = null;
		for( int t = 0; t < enemies.Length; t++ )
		{
			Vector3 tempLoc = enemies[t].transform.position;
			float distance = Vector3.Distance( transform.position, tempLoc );
			
			if ( distance < closest )
			{
				closest = distance;
				tempTarget = enemies[t];
			}
		}
        if( tempTarget == null )
        {
            for( int t = 0; t < wurms.Length; t++ )
            {
                Vector3 tempLoc = wurms[t].transform.position;
                float distance = Vector3.Distance( transform.position, tempLoc );

                if( distance < closest )
                {
                    closest = distance;
                    tempTarget = wurms[t];
                }
            }
        }
		return tempTarget;
	}
	
	protected virtual void UpdatePowerNode()
	{
		powerManager.UpdatePowerNode( gameObject, built, maxStoredPower, powerGeneration, powerConsumption );
	}
	
	public virtual void DrawBeam()
	{
		if( target )
		{
			CircleCollider2D targetCol = target.GetComponent<CircleCollider2D>();
			beamObj = null;
			Vector3 posInsideAsteroid = new Vector3( targetCol.transform.position.x + Random.Range( -0.02f, 0.02f ) * targetCol.transform.localScale.x, targetCol.transform.position.y + Random.Range( -0.02f, 0.02f ) * targetCol.transform.localScale.y, 4f );
			Vector3 pos = new Vector3( ( posInsideAsteroid.x + transform.position.x ) / 2, ( posInsideAsteroid.y + transform.position.y ) / 2, 4f );
			Vector3 temp = posInsideAsteroid - transform.position;
			temp.z = 0;
			beamObj = (GameObject)Instantiate( BeamPreFab, pos, Quaternion.identity );
			
			beamObj.transform.rotation = Quaternion.FromToRotation( new Vector3( Vector3.up.x, Vector3.up.y, Vector3.up.z ), temp );
			beamObj.transform.localScale = new Vector3( beamObj.transform.localScale.x, temp.magnitude, beamObj.transform.localScale.z );
		}
	}
	
	public virtual void DestroyBeam()
	{
		if( beamObj != null )
		{
			Destroy( beamObj.gameObject );
		}
	}

    public virtual void DestroyHealthBars()
    {
        if( healthBarBackObj )
        {
            Destroy( healthBarBackObj );
        }
        if( healthBarFrontObj )
        {
            Destroy( healthBarFrontObj );
        }
    }

    public virtual void Die( bool salvaged, int cost )
    {
        partSys.Play();
        dead = true;
        if( salvaged )
        {
            if( built )
            {
                powerManager.currentMinerals += (int)( 0.6f * (float)cost );
            }
            else
            {
                powerManager.currentMinerals += cost;
            }
        }
        powerManager.RemoveFromEverything( gameObject );
        if( foreBuildBar != null )
        {
            Destroy( foreBuildBar.gameObject );
        }
        if( backBuildBar != null )
        {
            Destroy( backBuildBar.gameObject );
        }
        DestroyBeam();
        if( healthBarBackObj )
        {
            Destroy( healthBarBackObj );
        }
        if( healthBarFrontObj )
        {
            Destroy( healthBarFrontObj );
        }
        
        Destroy( gameObject, 0.2f );
    }
}
