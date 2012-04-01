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
	public Side m_side = Side.Invalid;
	
	public GameObject m_walkerDef;
	public GameObject m_upgradesToDef;
	
	public float m_minSpawn = 10.0f;
	public float m_maxSpawn = 15.0f;

	public ResourceDef[] m_resStart;
	public ResourceDef[] m_resNeededForSpawn;
	public ResourceDef[] m_resNeededForUpgrade;
	
	public Dictionary<string, ResourceDef> m_res = new Dictionary<string, ResourceDef>();


	float m_nextAgentSpawn = 0.0f;
		
	// Use this for initialization
	void Start () 
	{
		for( int i = 0; i < m_resStart.Length; ++i )
		{
			addResource( m_resStart[i] );
		}
	}
	
	public void addResource( ResourceDef def )
	{
		ResourceDef curDef;
		if( m_res.TryGetValue( def.type, out curDef ) )
		{
			curDef.amount += def.amount;
		}
		else
		{
			curDef = def;
			m_res[def.type] = def;
		}
		
		print( "Have "+curDef.amount+" of "+def.type );
	}

	bool hasEnough( ResourceDef[] res )
	{
		for( int i = 0; i < res.Length; ++i )
		{
			ResourceDef curDef = new ResourceDef();
			if( m_res.TryGetValue( res[i].type, out curDef ) )
			{
				if( curDef.amount < res[i].amount ) return false;
			}
			else
			{
				return false;
			}
		}
		
		return true;
	}

	void useResources( ResourceDef[] res )
	{
		for( int i = 0; i < res.Length; ++i )
		{
			ResourceDef curDef = new ResourceDef();
			if( m_res.TryGetValue( res[i].type, out curDef ) )
			{
				curDef.amount -= res[i].amount;
			}
		}
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		m_nextAgentSpawn -= Time.deltaTime;
		
		if( m_nextAgentSpawn < 0 )
		{
			if( hasEnough( m_resNeededForSpawn ) )
			{
				print( "Making new villager" );
				
				useResources( m_resNeededForSpawn );
				
				Instantiate( m_walkerDef, transform.position, new Quaternion() );
			}
			
			m_nextAgentSpawn = Random.Range( m_minSpawn, m_minSpawn );
		}
	}
}
