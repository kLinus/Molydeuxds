using UnityEngine;
using System.Collections;

public class BearCam : MonoBehaviour {
	
	public GameObject target;
	public Vector3 offset = new Vector3(-5, 7, -5);
	private float originalOffsetY;
	
	// Use this for initialization
	void Start () 
	{
		originalOffsetY = offset.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(target.transform.position.y > offset.y)
		{
			offset.y = offset.y + offset.y + 1;
		}
		else
		{
			offset.y = originalOffsetY;
		}
		Vector3 tarPos = target.transform.position;
		World.me.GetActiveCamera().transform.position = new Vector3(tarPos.x + offset.x, offset.y, tarPos.z + offset.z); 
		World.me.GetActiveCamera().transform.LookAt(target.transform);
	
	}
}
