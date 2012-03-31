using UnityEngine;
using System.Collections;

public class PersonScript : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnCollisionEnter( Collision col )
	{
		Debug.Log("Collider Tag: " + col.gameObject.tag);
		if (col.gameObject.tag == "Player")
		{
			Destroy(this.gameObject);
			col.gameObject.GetComponent<BearScript>().IncreaseEnergy(30);
			//Turn to ghost()
		}
	}
}
