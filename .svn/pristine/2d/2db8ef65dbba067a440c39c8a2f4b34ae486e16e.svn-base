using UnityEngine;
using System.Collections;

public class AsteroidScript : MonoBehaviour {
	
	public int maxMins = 3500;
	public int minMins = 1000;
	public int startingMins;
	public int currentMins;
    public Texture2D[] Textures;

    private Texture2D texClone;
    private Renderer render;
    private int[] grayInterval;
    private bool[] intervalDone;

	// Use this for initialization
	void Start () {
        grayInterval = new int[10];
        intervalDone = new bool[10];
        grayInterval[0] = startingMins / 10;

        render = GetComponent<Renderer>();
		currentMins = Random.Range( minMins, maxMins );
		startingMins = currentMins;
		float size = (float)( (float)( (float)( currentMins - 1000 ) / 2500 ) * 0.30f ) + 0.2f;
		transform.localScale = new Vector3( size, size, size );

        for( int i = 1; i <= 9; i++ )
        {
            grayInterval[i] = grayInterval[i-1] + ( startingMins / 10 );
        }

        texClone = (Texture2D)Instantiate( render.material.mainTexture );
        render.material.mainTexture = texClone;
	}
	
	// Update is called once per frame
    void Update()
    {
        for( int i = 9; i >= 0; i-- )
        {
            if( ( currentMins <= grayInterval[i] ) && ( !intervalDone[i] ) )
            {
                SetGrayness( texClone, i );
                intervalDone[i] = true;
                break;
            }
        }
    }

    void SetGrayness( Texture2D texture, int interval )
    {
        texture = (Texture2D)Instantiate( Textures[interval] );
        render.material.mainTexture = texture;
    }
}
