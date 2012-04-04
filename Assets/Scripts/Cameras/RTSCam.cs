using UnityEngine;
using System.Collections;

public class RTSCam : MonoBehaviour 
{
	public float m_speed = 15.0f;
	public float m_resetCamXOffset = -20;
	public float m_resetCamZOffset = -20;
	public GameObject m_selection;
	
	
	
	IEnumerator Volcano( Vector3 pos )
	{
		int count = 0;
		while( count < World.me.m_volcanoProbes )
		{
			float xOff = Random.Range( -World.me.m_volcanoRadius, World.me.m_volcanoRadius );
			float zOff = Random.Range( -World.me.m_volcanoRadius, World.me.m_volcanoRadius );
			
			float linChance = Mathf.Clamp( 1.0f - Mathf.Sqrt( xOff * xOff + zOff * zOff ) / World.me.m_volcanoRadius, 0, 1 );
			
			float sqrChance = linChance * linChance* linChance* linChance;
			
			if( sqrChance <= Random.Range( 0, 1 ) )
			{
				continue;
			}
			
			float x = pos.x + xOff;
			float z = pos.z + zOff;
			
			float y = World.me.getWorldHeight( x, z );
			
			World.me.addBlock( (short)1, (int)(x+0.5f), (int)(y+0.5f), (int)(z+0.5f) );
			
			yield return 0;
			++count;
		}

		yield return 0;
	}
	
	
	
	// Use this for initialization
	void Start () 
	{
		transform.position = new Vector3(6,50,0);
		transform.Rotate(30f, 45f, 0f);	
		camera.orthographic = true;
		camera.orthographicSize = 10;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (World.me.m_currentMode == World.Mode.GOD)
		{
			UpdateMovement();
			UpdateAbilities();
		}
		else
		{
			Vector3 camPos = World.me.GetActiveCamera().transform.position;
			transform.position = new Vector3(camPos.x + m_resetCamXOffset, 50, camPos.z + m_resetCamZOffset);
		}
	}
	
	private void UpdateMovement()
	{
		if( Input.GetKey(KeyCode.W) )
		{
			World.me.GetActiveCamera().transform.position += ( new Vector3( 0, 0, 1 ) * Time.deltaTime * m_speed );
		}
		if( Input.GetKey(KeyCode.S) )
		{
			World.me.GetActiveCamera().transform.position += ( new Vector3( 0, 0,-1 ) * Time.deltaTime * m_speed );
		}
		if( Input.GetKey(KeyCode.A) )
		{
			World.me.GetActiveCamera().transform.position += ( new Vector3(-1, 0, 0 ) * Time.deltaTime * m_speed );
		}
		if( Input.GetKey(KeyCode.D) )
		{
			World.me.GetActiveCamera().transform.position += ( new Vector3( 1, 0, 0 ) * Time.deltaTime * m_speed );
		}	
	}
		
	private void UpdateAbilities()
	{
		Ray raySel = World.me.GetActiveCamera().ScreenPointToRay(Input.mousePosition);
		RaycastHit hitSel;
		
        if( Physics.Raycast( raySel, out hitSel ) )
		{
			m_selection.transform.position = hitSel.point;
		}
		
		if( Input.GetKeyDown( "f" ) )
		{
			Vector3 waterPos = World.me.m_waterObj.transform.position;
			if( World.me.m_energy.current >= World.me.m_godFloodEnergy && waterPos.y <= 10)
			{
				World.me.m_waterObj.transform.position += new Vector3( 0, 1, 0 );
				
				World.me.m_energy.add( -World.me.m_godFloodEnergy );
			}
		}

		if( Input.GetKeyDown( "o" ) )
		{
			Vector3 waterPos = World.me.m_waterObj.transform.position;
			if( World.me.m_energy.current >= World.me.m_godDroughtEnergy && waterPos.y >= 0 )
			{
				World.me.m_waterObj.transform.position -= new Vector3( 0, 1, 0 );
				
				World.me.m_energy.add( -World.me.m_godDroughtEnergy );
			}
		}

		//if( Input.GetKeyDown( "v" ) )
		//{
		//	if( World.me.m_energy.current >= World.me.m_godVolcanoEnergy )
		//	{	
	    //      if( Physics.Raycast( raySel, out hitSel ) )
		//		{
		//			World.me.m_energy.add( -World.me.m_godVolcanoEnergy );
		//			
		//			StartCoroutine( Volcano( hitSel.point ) );
		//		}
		//	}
		//}

		if( Input.GetKeyDown( KeyCode.Q ) || Input.GetMouseButton( 0 ) )
		{
			if( World.me.m_energy.current >= World.me.m_godAddLandEnergy )
			{	
	            if( Physics.Raycast( raySel, out hitSel ) )
				{
					World.me.m_energy.add( -World.me.m_godAddLandEnergy );
					
					Vector3 blockPoint = hitSel.point + hitSel.normal * 0.25f;
					
					World.me.addBlock ( 1, (int)(blockPoint.x+0.5f), (int)(blockPoint.y+0.5f), (int)(blockPoint.z+0.5f) );
				}
			}
		}
		
		if( Input.GetKeyDown(KeyCode.E ) || Input.GetMouseButton( 1 ) )
		{
			if( World.me.m_energy.current >= World.me.m_godRemLandEnergy )
			{	
	            if( Physics.Raycast( raySel, out hitSel ) )
				{
					World.me.m_energy.add( -World.me.m_godRemLandEnergy );
					
					Vector3 blockPoint = hitSel.point + raySel.direction * 0.25f;
					
					World.me.remBlock ( (int)(blockPoint.x+0.5f), (int)(blockPoint.y+0.5f), (int)(blockPoint.z+0.5f) );
				}
			}
		}
		
		if( Input.GetKeyDown(KeyCode.B) )
		{
			if(Physics.Raycast(raySel, out hitSel))
			{
				if(hitSel.transform.gameObject.name == "Bear(Clone)")
				{
					World.me.m_currentMode = World.Mode.BEAR;
					World.me.RefreshCameras();
					World.me.GetActiveCamera().GetComponentInChildren<BearCam>().target = hitSel.transform.gameObject;
					hitSel.transform.gameObject.GetComponentInChildren<RabidBear>().GetPossessed();
					hitSel.transform.gameObject.GetComponentInChildren<RabidBear>().enabled = false;
					hitSel.transform.gameObject.GetComponentInChildren<BearScript>().enabled = true;
				}
			}
		}
		
		if( Input.GetKey(KeyCode.C) )
		{
			if(Physics.Raycast(raySel, out hitSel))
			{
				if(hitSel.transform.gameObject.name == "CloudPrefab(Clone)")
				{
					Debug.Log("Switching to Cloud Mode");
					World.me.m_currentMode = World.Mode.CLOUD;
					World.me.RefreshCameras();
					World.me.GetActiveCamera().GetComponent<CloudCam>().target = hitSel.transform.gameObject;
					hitSel.transform.gameObject.AddComponent<CloudScript>();
				}
				Debug.Log("Raycast hit: " + hitSel.transform.gameObject);
			}
		}
		
	//	if( Input.GetKeyDown(KeyCode.Alpha0) )
	//	{
    //        if( Physics.Raycast( raySel, out hitSel ) )
	//		{
	//			if( World.me.m_bearDef == null )
	//			{
	//				print( "No walker" );
	//			}
	//			GameObject bear;
	//			bear = (GameObject)Instantiate( World.me.m_bearDef, hitSel.point, new Quaternion() );
	//			bear.AddComponent<RabidBear>();
	//			bear.AddComponent<RabidBear>().enabled = true;
	//		}
	//	}
	}
	
}



/*
*/