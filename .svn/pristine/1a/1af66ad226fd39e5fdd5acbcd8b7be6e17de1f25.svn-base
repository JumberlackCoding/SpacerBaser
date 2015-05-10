using UnityEngine;
using System.Collections;

public class MineralBeamScript : MonoBehaviour {

    public GameObject ParticleEmitter;
    public LayerMask asteroidMask;
    public GameObject rayStart;
    public GameObject rayEnd;

    void Awake()
    {

    }

	// Use this for initialization
	void Start () {
        Vector3 newPos = ParticleEmitter.transform.localPosition;

        RaycastHit2D tempHit = Physics2D.Linecast( rayStart.transform.position, rayEnd.transform.position, asteroidMask );
        newPos = transform.InverseTransformPoint( tempHit.point );
        newPos.z = 0f;
        newPos.x = 0f;
        ParticleEmitter.transform.localPosition = newPos;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
