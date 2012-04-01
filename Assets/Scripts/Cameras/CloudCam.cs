using UnityEngine;
using System.Collections;

public class CloudCam : MonoBehaviour {
	
	public float velocity = 15.0f;
	
	// Use this for initialization
	void Start () 
	{
		transform.position = new Vector3(10, 20, 15);
		transform.Rotate(75.0f, 45.0f, 0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.W))
		{
			World.me.GetActiveCamera().transform.position += Vector3.forward * Time.deltaTime * velocity;
		}
		
		if(Input.GetKey(KeyCode.S))
		{
			World.me.GetActiveCamera().transform.position += Vector3.back * Time.deltaTime * velocity;
		}
		
		if(Input.GetKey(KeyCode.D))
		{
			World.me.GetActiveCamera().transform.position += Vector3.right * Time.deltaTime * velocity;
		}
	
		if(Input.GetKey(KeyCode.A))
		{
			World.me.GetActiveCamera().transform.position += Vector3.left * Time.deltaTime * velocity;
		}
	}
}
