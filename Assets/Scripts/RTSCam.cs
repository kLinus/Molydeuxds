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
		float speed = 15.0f;
		
		if( Input.GetKey("up") )
		{
			Camera.main.transform.position += ( new Vector3( 0, 0, 1 ) * Time.deltaTime * speed );
		}
		if( Input.GetKey("down") )
		{
			Camera.main.transform.position += ( new Vector3( 0, 0,-1 ) * Time.deltaTime * speed );
		}
		if( Input.GetKey("left") )
		{
			Camera.main.transform.position += ( new Vector3(-1, 0, 0 ) * Time.deltaTime * speed );
		}
		if( Input.GetKey("right") )
		{
			Camera.main.transform.position += ( new Vector3( 1, 0, 0 ) * Time.deltaTime * speed );
		}
		
		if( Input.GetKeyDown( "r" ) )
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			RaycastHit hit;
			
            if( Physics.Raycast( ray, out hit ) )
			{
				Instantiate( World.me.m_treeDef, hit.point, new Quaternion() );
			}
		}
		
		if( Input.GetMouseButtonDown( 0 ) )
		{
			/*
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
			*/            		
		}
		
	}
}
