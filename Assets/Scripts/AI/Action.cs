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

		Vector3 moved = new Vector3();

		while( moved.sqrMagnitude < totalDistSqr )
		{
			float distThisFrame = speed * Time.deltaTime;
			
			Vector3 p = m_walker.transform.position + dir * distThisFrame;
			
			float y = World.me.getWorldHeight( p.x, p.z );
			
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

}

class ActWorker : Action
{
	public ActWorker( Walker walker ): base( walker )
	{
	}
	
	public IEnumerator act()
	{
		while( true )
		{
			float x = Random.Range( 0.0f, World.s_chunkWorldSize * World.s_chunkSide );
			float z = Random.Range( 0.0f, World.s_chunkWorldSize * World.s_chunkSide );
			
			float y = World.me.getWorldHeight( x, z );
			
			
			yield return m_walker.StartCoroutine( gotoSquare( new Vector3( x, y, z ), 4.0f, 0.01f ) );
		}
	}
}

