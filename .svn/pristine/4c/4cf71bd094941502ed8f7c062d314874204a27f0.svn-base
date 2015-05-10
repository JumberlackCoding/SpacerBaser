using UnityEngine;
using System.Collections;

public class Turret_Missile : GenericStructureScript {

	private bool needScript = true;
	private bool fired = false;

	// Use this for initialization
	void Start () {
		Initialize();
		StartCoroutine( PulseAction() );
	}
	
	// Update is called once per frame
	void Update () {
		if( !built )
		{
			Build();
		}
		else
		{
			if( target == null )
			{
				inRange = false;
				needScript = true;
				target = AcquireTarget();
			}
			CheckInRange();
			if( target != null )
			{
				targetLocation = target.transform.position;
				if( needScript )
				{
					targetScript = target.GetComponent<GenericEnemyScript>();
					needScript = false;
				}
			}
		}
		if( health <= 0 )
		{
			Die( false, 0 );
		}
	}

	protected override IEnumerator PulseAction()
	{
		while( true )
		{
			if( built )
			{
				currentPower = powerManager.GetTurretPower( gameObject );
				
				if( inRange && ( target != null ) && !fired )
				{
					if( currentPower >= powerConsumption )
					{
						powerManager.ConsumeTurretPower( gameObject, powerConsumption );
						// Spawn Missle Sprite
						fired = true;
					}
				}
				
				if(fired)
				{
					fired = false;
				}
				
				yield return new WaitForSeconds( attackSpeed );
			}
			else
			{
				yield return new WaitForSeconds( 0.5f );
			}
		}
	}

	public bool AbleToAttack()
	{
		bool result = false;
		if( inRange && ( target != null ) )
		{
			result = true;
		}
		
		return result;
	}
	
	public override void StructureFunction()
	{
		// wait for enemies to be in range and then launch missiles when they are
	}
}
