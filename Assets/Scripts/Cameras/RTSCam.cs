using UnityEngine;
using System.Collections;

public class RTSCam : MonoBehaviour 
{
	public float m_speed = 15.0f;
	
	// Use this for initialization
	void Start () 
	{
		//transform.position = new Vector3(5, 15, 5);
		//transform.Rotate(45.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if( Input.GetKey(KeyCode.UpArrow) )
		{
			World.me.GetActiveCamera().transform.position += ( new Vector3( 0, 0, 1 ) * Time.deltaTime * m_speed );
		}
		if( Input.GetKey(KeyCode.DownArrow) )
		{
			World.me.GetActiveCamera().transform.position += ( new Vector3( 0, 0,-1 ) * Time.deltaTime * m_speed );
		}
		if( Input.GetKey(KeyCode.LeftArrow) )
		{
			World.me.GetActiveCamera().transform.position += ( new Vector3(-1, 0, 0 ) * Time.deltaTime * m_speed );
		}
		if( Input.GetKey(KeyCode.RightArrow) )
		{
			World.me.GetActiveCamera().transform.position += ( new Vector3( 1, 0, 0 ) * Time.deltaTime * m_speed );
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



/*
		//TODO: Remove the other raycasts.
		Ray raySel = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		RaycastHit hitSel;
		
        if( Physics.Raycast( raySel, out hitSel ) )
		{
			m_selection.transform.position = hitSel.point;
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
		
		if( Input.GetKeyDown( "a" ) || Input.GetMouseButtonDown( 0 ) )
		{
			if( World.me.m_energy.current >= World.me.m_godAddLandEnergy )
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				RaycastHit hit;
				
	            if( Physics.Raycast( ray, out hit ) )
				{
					World.me.m_energy.add( -World.me.m_godAddLandEnergy );
					
					Vector3 blockPoint = hit.point + hit.normal * 0.25f;
					
					World.me.addBlock ( 1, (int)(blockPoint.x+0.5f), (int)(blockPoint.y+0.5f), (int)(blockPoint.z+0.5f) );
				}
			}
		}
		
		if( Input.GetKeyDown( "d" ) || Input.GetMouseButtonDown( 1 ) )
		{
			if( World.me.m_energy.current >= World.me.m_godRemLandEnergy )
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				RaycastHit hit;
				
	            if( Physics.Raycast( ray, out hit ) )
				{
					World.me.m_energy.add( -World.me.m_godRemLandEnergy );
					
					Vector3 blockPoint = hit.point + ray.direction * 0.25f;
					
					World.me.remBlock ( (int)(blockPoint.x+0.5f), (int)(blockPoint.y+0.5f), (int)(blockPoint.z+0.5f) );
				}
			}
		}
*/