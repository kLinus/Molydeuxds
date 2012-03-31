using UnityEngine;
using System.Collections;


class Action
{
	protected Walker m_walker; 
	
	public Action( Walker walker )
	{
		m_walker = walker;
	}
	
	public virtual void Update()
	{
	}
}

class ActWorker : Action
{
	public ActWorker( Walker walker ): base( walker )
	{
	}

	public override void Update()
	{
	}
}

