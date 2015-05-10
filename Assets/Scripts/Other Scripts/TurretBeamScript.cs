using UnityEngine;
using System.Collections;

public class TurretBeamScript : MonoBehaviour {

    public GameObject target;
    public Transform sourceTrans;

    CircleCollider2D targetCol;

    private bool targetAcquired = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if( targetAcquired )
        {
            if( target != null )
            {
                Vector3 posInsideAsteroid = new Vector3( targetCol.transform.position.x + Random.Range( -0.02f, 0.02f ) * targetCol.transform.localScale.x, targetCol.transform.position.y + Random.Range( -0.02f, 0.02f ) * targetCol.transform.localScale.y, 4f );
                Vector3 pos = new Vector3( ( posInsideAsteroid.x + sourceTrans.transform.position.x ) / 2, ( posInsideAsteroid.y + sourceTrans.transform.position.y ) / 2, 4f );
                Vector3 temp = posInsideAsteroid - sourceTrans.transform.position;
                temp.z = 0;

                transform.position = pos;
                transform.rotation = Quaternion.FromToRotation( new Vector3( Vector3.up.x, Vector3.up.y, Vector3.up.z ), temp );
                transform.localScale = new Vector3( transform.localScale.x, temp.magnitude, transform.localScale.z );
            }
        }
	}

    public void SetTargSource( GameObject tar, Transform sour )
    {
        target = tar;
        sourceTrans = sour;
        targetAcquired = true;
        targetCol = target.GetComponent<CircleCollider2D>();
    }

}
