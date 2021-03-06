using UnityEngine;
using System.Collections;

public class CloudCam : MonoBehaviour {
	
	public GameObject target;
	
	// Use this for initialization
	void Start () 
	{
		transform.position = new Vector3(10, 20, 15);
		transform.Rotate(75.0f, 45.0f, 0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (World.me.m_currentMode == World.Mode.CLOUD)
		{
			if (target != null)
			{
				transform.position = target.transform.position;
				
				if( Input.GetKeyDown( "r" ) )
				{
					if( World.me.m_energy.current >= World.me.m_godRainEnergy )
					{
						Ray ray = World.me.GetActiveCamera().ScreenPointToRay(Input.mousePosition);
					
						RaycastHit hit;
					
		            	if( Physics.Raycast( ray, out hit ) )
						{
							Instantiate( World.me.m_foodDef, hit.point, new Quaternion() );
							World.me.m_energy.add ( -World.me.m_godRainEnergy );
						}
					}
				}
				
				if( Input.GetKeyDown(KeyCode.Escape))
				{
					World.me.m_currentMode = World.Mode.GOD;
				}
			}
		}
	}
	
	
}
