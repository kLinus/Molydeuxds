using UnityEngine;
using System.Collections;

public class IntroScreen : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.anyKeyDown)
			Application.LoadLevel("Final");
	}
}

