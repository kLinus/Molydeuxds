using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour 
{
	public GameObject m_walkerDef;
	public GameObject m_upgradesToDef;
	
	public float m_foodNeeded = 10.0f;
	
	public float m_lumberNeeded = 10.0f;
	public float m_rocksNeeded = 10.0f;

	float m_nextAgentSpawn = 10.0f;
	
	float m_lumber	= 0.0f;
	float m_rocks	= 0.0f;
	float m_food	= 0.0f;
	
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
