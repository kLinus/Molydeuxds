using UnityEngine;
using System.Collections;

public class RabidBear : Walker 
{
	string bearName;
	public float m_bearEnergy = 100;
	public float m_bearDecay = 5;
	
	ActBear m_act;
	
	// Use this for initialization
	void Start () 
	{
		m_act = new ActBear( this );
		
		StartCoroutine( m_act.act() );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (gameObject.transform.position.y <= World.me.m_waterObj.transform.position.y)
		{
			Destroy (gameObject);
		}
		if (m_bearEnergy <= 0)
		{
			Destroy (gameObject);
		}
		m_bearEnergy -= m_bearDecay * Time.deltaTime;

	}
		
	void OnGUI()
	{
	}
	
	public void GetPossessed()
	{
		StopAllCoroutines();
	}
	
	public void setName(string name)
	{
		bearName = name;
	}
}
