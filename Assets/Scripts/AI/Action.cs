using UnityEngine;
using System.Collections;


class Action 
{
	protected Walker m_walker;
	
	public Action( Walker walker )
	{
		m_walker = walker;
	}
	
	public virtual bool isDone() 
	{
		return false;
	}
	
	public IEnumerator gotoSquare( Vector3 smallMove, float speed, float minDist )
	{
		float minDistSqr = minDist * minDist;
		
		Vector3 startPos = m_walker.transform.position;
		
		Vector3 dir = (smallMove - startPos);
		
		float totalDistSqr = dir.sqrMagnitude;

		dir.Normalize();
		
		m_walker.transform.LookAt(smallMove);

		Vector3 moved = new Vector3();

		while( moved.sqrMagnitude < totalDistSqr )
		{
			float distThisFrame = speed * Time.deltaTime;
			
			Vector3 p = m_walker.transform.position + dir * distThisFrame;
			
			float y = World.me.getWorldHeight( p.x, p.z ) - 0.5f;
			
			if( m_walker.gameObject.GetComponent<Ghost>() == null )
			{
				if( y < World.me.m_waterObj.transform.position.y )
				{
					//I would drown!
					
					float curY = World.me.getWorldHeight( m_walker.transform.position.x, m_walker.transform.position.z ) - 0.5f;
					Vector3 pos = m_walker.transform.position;
					pos.y = curY;
					m_walker.transform.position = pos;
					
					yield break;
				}
				
				if( Mathf.Abs( m_walker.transform.position.y - y ) > 1.5f )
				{
					//Too damn high!

					float curY = World.me.getWorldHeight( m_walker.transform.position.x, m_walker.transform.position.z ) - 0.5f;
					Vector3 pos = m_walker.transform.position;
					pos.y = curY;
					m_walker.transform.position = pos;

					yield break;
				}
			}
			
			m_walker.transform.position = new Vector3( p.x, y, p.z );

			yield return 0;

			moved = m_walker.transform.position - startPos;
		}
		
		//Snap to pos.
		m_walker.transform.position = smallMove;
		
		yield return 0;
	}
	
	Vector3 getNextBlock( Vector3 pos )
	{
		Vector3 curPos = m_walker.transform.position;
		
		Vector3 dir = pos - curPos;
		
		if( Mathf.Abs( dir.x ) > Mathf.Abs( dir.z ) )
		{
			Vector3 off = new Vector3( 1.0f, 0, 0 ) * Mathf.Sign( dir.x );
			
			
		}
		else
		{
			Vector3 off = new Vector3( 0, 0, -1.0f ) * Mathf.Sign( dir.z );
		}
		
		//Temp stupid shit
		return new Vector3();
	}

	public IEnumerator returnHome(  )
	{
		int count = 10;
		while( count > 0 )
		{
			Collider[] collides = Physics.OverlapSphere( m_walker.transform.position, 256.0f, World.me.s_layerHut );
			
			
			if( collides.Length > 0 )
			{
				int i = Random.Range( 0, collides.Length );
				
				GameObject go = collides[i].gameObject;
				
				Building building = go.GetComponent<Building>();
				
				if( building != null && building.m_side == m_walker.m_side )
				{
					MonoBehaviour.print( "Heading home to:"+building.gameObject.name );
					yield return m_walker.StartCoroutine( gotoSquare( building.transform.position, 1.0f, 0.01f ) );
					
					Vector3 dist = building.transform.position - m_walker.transform.position;
					
					if( dist.sqrMagnitude < 1.1 )
					{
						building.addResource( m_walker.m_resCarrying );
						
						m_walker.m_resCarrying = null;
						
						yield break;
					}
				}
			}
	
			MonoBehaviour.print( "Finding home!" );		
			float xOff = Random.Range( -16.0f, 16.0f );
			float zOff = Random.Range( -16.0f, 16.0f );
			
			float x = Mathf.Clamp( m_walker.transform.position.x + xOff, 0, World.s_chunkWorldSize * World.s_chunkSide );
			float z = Mathf.Clamp( m_walker.transform.position.z + zOff, 0, World.s_chunkWorldSize * World.s_chunkSide );
			
			float y = World.me.getWorldHeight( x, z );
					
			yield return m_walker.StartCoroutine( gotoSquare( new Vector3( x, y, z ), 2.0f, 0.01f ) );
						
			--count;
		}
	}
	
	
	public IEnumerator extractResource( string type )
	{
		Collider[] collides = Physics.OverlapSphere( m_walker.transform.position, 32.0f, World.me.s_layerResource );
		
		if( collides.Length > 0 )
		{
			int i = Random.Range( 0, collides.Length - 1 );
			{
				GameObject go = collides[i].gameObject;
				
				Resource res = go.GetComponent<Resource>();
				
				if( res != null && type == res.def.type )
				{
					MonoBehaviour.print( "Heading towards:"+res.gameObject.name+" for res:"+type );
					
					yield return m_walker.StartCoroutine( gotoSquare( res.transform.position, 4.0f, 0.01f ) );

					
					if( go != null )
					{
						Vector3 dist = go.transform.position - m_walker.transform.position;
						
						if( dist.sqrMagnitude < 1.1 )
						{
							m_walker.m_resCarrying = res.def;
							Object.Destroy( go );
							
							yield return m_walker.StartCoroutine( returnHome() );
							
							yield break;
						}
					}
				}
			}
		}
		
		float xOff = Random.Range( -16.0f, 16.0f );
		float zOff = Random.Range( -16.0f, 16.0f );
		
		float x = Mathf.Clamp( m_walker.transform.position.x + xOff, 0, World.s_chunkWorldSize * World.s_chunkSide );
		float z = Mathf.Clamp( m_walker.transform.position.z + zOff, 0, World.s_chunkWorldSize * World.s_chunkSide );
		
		float y = World.me.getWorldHeight( x, z );
				
		yield return m_walker.StartCoroutine( gotoSquare( new Vector3( x, y, z ), 2.0f, 0.01f ) );
	}
	
	public bool shouldAttack( out Walker who )
	{
		Collider[] collides = Physics.OverlapSphere( m_walker.transform.position, 32.0f, World.me.s_layerWalker );
		
		if( collides.Length > 0 )
		{
			int count = 6;
			while( count > 0 )
			{
				int i = Random.Range( 0, collides.Length );
				{
					GameObject go = collides[i].gameObject;
					
					Walker otherWalker = go.GetComponent<Walker>();
					
					if( otherWalker != null && m_walker.m_side != otherWalker.m_side )
					{
						who = otherWalker;
						return true;
					}
				}
				
				--count;
			}
		}
		
		who = null;
		return false;
	}


	public IEnumerator attack( Walker otherWalker )
	{
		int count = 6;
		while( count > 0 )
		{
			if( otherWalker != null && m_walker.m_side != otherWalker.m_side )
			{
				MonoBehaviour.print( "Attacking:"+otherWalker.gameObject.name );
				
				yield return m_walker.StartCoroutine( gotoSquare( otherWalker.transform.position, 4.0f, 1.0f ) );
				
				if( otherWalker != null )
				{
					Vector3 dist = otherWalker.transform.position - m_walker.transform.position;
					
					if( dist.sqrMagnitude < 1.1 )
					{
						World.me.m_currentMode = World.Mode.GOD;
						GameObject temp = Object.Instantiate( otherWalker.bloodSpawn ) as GameObject;
						temp.transform.parent = otherWalker.transform;
						temp.transform.localPosition = Vector3.zero;
						World.me.RefreshCameras();
						
						Object.Destroy( otherWalker.gameObject, 1.0f );
						
						yield return m_walker.StartCoroutine( returnHome() );
						
						yield break;
					}
				}
			}
			
			float xOff = Random.Range( -2.0f, 2.0f );
			float zOff = Random.Range( -2.0f, 2.0f );
			
			float x = Mathf.Clamp( m_walker.transform.position.x + xOff, 0, World.s_chunkWorldSize * World.s_chunkSide );
			float z = Mathf.Clamp( m_walker.transform.position.z + zOff, 0, World.s_chunkWorldSize * World.s_chunkSide );
			
			float y = World.me.getWorldHeight( x, z );
					
			yield return m_walker.StartCoroutine( gotoSquare( new Vector3( x, y, z ), 3.0f, 0.01f ) );
			
			--count;
		}
	}
	
	
}

class ActWorker : Action
{
	public enum Task
	{
		Normal,
		Attack
	}

	Task m_task;
	
	public ActWorker( Walker walker, Task task ): base( walker )
	{
		m_task = task;
	}
	
	public IEnumerator act()
	{
		while( true )
		{
			switch( m_task )
			{
			case Task.Normal:
				if( m_walker.m_resCarrying != null )
				{
					yield return m_walker.StartCoroutine( returnHome() );
				}
				else
				{
					MonoBehaviour.print( "Finding resource" );
					yield return m_walker.StartCoroutine( extractResource( "Food" ) );
					
					Walker otherWalker;
					if( shouldAttack( out otherWalker ) )
					{
						m_task = Task.Attack;
					}
				}
				break;
				
			case Task.Attack:
			{
				MonoBehaviour.print( "Attack!" );
				Walker otherWalker;
				if( shouldAttack( out otherWalker ) )
				{
					yield return m_walker.StartCoroutine( attack(otherWalker) );
				}
				
				m_task = Task.Normal;
			}
				
				break;
			}
		}
	}
}

class ActGhost : Action
{
	public ActGhost( Ghost ghost ): base( ghost )
	{
	}
	
	public IEnumerator act()
	{
		while( true )
		{
			MonoBehaviour.print( "Wandering the spirit realm" );
			float xOff = Random.Range( -4.0f, 4.0f );
			float zOff = Random.Range( -4.0f, 4.0f );
			
			float x = Mathf.Clamp( m_walker.transform.position.x + xOff, 0, World.s_chunkWorldSize * World.s_chunkSide );
			float z = Mathf.Clamp( m_walker.transform.position.z + zOff, 0, World.s_chunkWorldSize * World.s_chunkSide );
			
			float y = World.me.getWorldHeight( x, z );
					
			yield return m_walker.StartCoroutine( gotoSquare( new Vector3( x, y, z ), 2.0f, 0.01f ) );
		}
	}
}

class ActBear : Action
{
	
	
	public IEnumerator extractHumanResource()
	{
		Collider[] collides = Physics.OverlapSphere( m_walker.transform.position, 32.0f, World.me.s_layerWalker );
		
		if( collides.Length > 0 )
		{
			int i = Random.Range( 0, collides.Length - 1 );
			{
				GameObject go = collides[i].gameObject;

				MonoBehaviour.print( "Attack towards:"+go.name );
					
				yield return m_walker.StartCoroutine( gotoSquare( go.transform.position, 6.0f, 0.01f ) );
					
				if( go != null )
				{
					Vector3 dist = go.transform.position - m_walker.transform.position;
					
					if( dist.sqrMagnitude < 1.1 )
					{
						Object.Destroy( go, 5.0f );
						
						yield break;
					}
				}
			}
		}
		
		float xOff = Random.Range( -32.0f, 32.0f );
		float zOff = Random.Range( -32.0f, 32.0f );
		
		float x = Mathf.Clamp( m_walker.transform.position.x + xOff, 0, World.s_chunkWorldSize * World.s_chunkSide );
		float z = Mathf.Clamp( m_walker.transform.position.z + zOff, 0, World.s_chunkWorldSize * World.s_chunkSide );
		
		float y = World.me.getWorldHeight( x, z );
				
		yield return m_walker.StartCoroutine( gotoSquare( new Vector3( x, y, z ), 2.0f, 0.01f ) );
	}
	
	
	public ActBear( RabidBear bear ): base( bear )
	{
	}
	
	public IEnumerator act()
	{
		while( true )
		{
				yield return m_walker.StartCoroutine( extractHumanResource() );
		}
	}
}
