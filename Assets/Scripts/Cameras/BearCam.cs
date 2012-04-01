using UnityEngine;
using System.Collections;

public class BearCam : MonoBehaviour {
	
	public GameObject target;
	public Vector3 offset = new Vector3(-5, 7, -5);
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 tarPos = target.transform.position;
		World.me.GetActiveCamera().transform.position = new Vector3(tarPos.x + offset.x, tarPos.y + offset.y, tarPos.z + offset.z); 
		World.me.GetActiveCamera().transform.LookAt(target.transform);
	
	}
}
