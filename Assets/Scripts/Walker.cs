using UnityEngine;
using System.Collections;

public class Walker : MonoBehaviour 
{
	public GameObject m_buildingDef;
	
	public float s_ageToMakeBuilding = 60.0f;
	public float s_chanceToMakeBuilding = 0.1f;
	
	public float m_maxAge = 60.0f;
	float m_age = 0.0f;
	
	float m_chanceOfBuilding = Random.Range( 0.0f, 1.0f );
	
	ActWorker m_act;
	
	public ResourceDef m_resCarrying;
	
	// Use this for initialization
	void Start () 
	{
		m_act = new ActWorker( this );
		
		StartCoroutine( m_act.act() );
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_age += Time.deltaTime;
		
		if( m_chanceOfBuilding < s_chanceToMakeBuilding )
		{
			if( m_age > s_ageToMakeBuilding )
			{
				m_chanceOfBuilding = 1.0f;
				
				Instantiate( m_buildingDef, transform.position, new Quaternion() );
			}
		}
		
		if( m_age > m_maxAge )
		{
			// TODO: Generate and popup life story.  
			Object.Destroy( gameObject );
		}
	}
}
