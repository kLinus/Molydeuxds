using UnityEngine;
using System.Collections;

public class RTSCam : MonoBehaviour 
{
	
	// Use this for initialization
	void Start () 
	{
		transform.position = new Vector3(5, 15, 5);
		transform.Rotate(30.0f, 45.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		float speed = 15.0f;
		
		if( Input.GetKey(KeyCode.UpArrow) )
		{
			World.me.GetActiveCamera().transform.position += ( new Vector3( 0, 0, 1 ) * Time.deltaTime * speed );
		}
		if( Input.GetKey(KeyCode.DownArrow) )
		{
			World.me.GetActiveCamera().transform.position += ( new Vector3( 0, 0,-1 ) * Time.deltaTime * speed );
		}
		if( Input.GetKey(KeyCode.LeftArrow) )
		{
			World.me.GetActiveCamera().transform.position += ( new Vector3(-1, 0, 0 ) * Time.deltaTime * speed );
		}
		if( Input.GetKey(KeyCode.RightArrow) )
		{
			World.me.GetActiveCamera().transform.position += ( new Vector3( 1, 0, 0 ) * Time.deltaTime * speed );
		}
		
		if( Input.GetKeyDown( "r" ) )
		{
			if( World.me.m_energy.current >= World.me.m_godRainEnergy )
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				RaycastHit hit;
				
	            if( Physics.Raycast( ray, out hit ) )
				{
					Instantiate( World.me.m_foodDef, hit.point, new Quaternion() );
					World.me.m_energy.add ( -World.me.m_godRainEnergy );
				}
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
