using UnityEngine;
using System.Collections;

public class RTSCam : MonoBehaviour 
{

	public GameObject m_walker;
	
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( Input.GetKeyDown("up") )
		{
			Camera.main.transform.position += ( new Vector3( 0, 0, 1 ) );
		}
		if( Input.GetKeyDown("down") )
		{
			Camera.main.transform.position += ( new Vector3( 0, 0,-1 ) );
		}
		if( Input.GetKeyDown("left") )
		{
			Camera.main.transform.position += ( new Vector3(-1, 0, 0 ) );
		}
		if( Input.GetKeyDown("right") )
		{
			Camera.main.transform.position += ( new Vector3( 1, 0, 0 ) );
		}
		
		
		if( Input.GetMouseButtonDown( 0 ) )
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			RaycastHit hit;
			
            if( Physics.Raycast( ray, out hit ) )
			{
				if( m_walker == null )
				{
					print( "No walker" );
				}
				
				Instantiate( m_walker, hit.point, new Quaternion() );
                //Instantiate( Walker ); //, this.transform.position, this.transform.rotation );
			}
            		
		}
		
	}
}
