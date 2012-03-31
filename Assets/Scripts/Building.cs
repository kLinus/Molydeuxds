using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ResourceDef
{
	public string type;
	public float  amount;
}

public class Building : MonoBehaviour 
{
	public GameObject m_walkerDef;
	public GameObject m_upgradesToDef;
	

	public ResourceDef[] m_resNeededForSpawn;
	public ResourceDef[] m_resNeededForUpgrade;
	
	public Dictionary<string, ResourceDef> m_res;


	float m_nextAgentSpawn = 10.0f;
		
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_nextAgentSpawn -= Time.deltaTime;
		
		if( m_nextAgentSpawn < 0 )
		{
			Instantiate( m_walkerDef, transform.position, new Quaternion() );
			
			m_nextAgentSpawn = Random.Range( 10.0f, 20.0f );
		}
	}
}
