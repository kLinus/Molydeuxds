using UnityEngine;
using System.Collections;

public class Ghost : Walker 
{
	public GameObject m_buildingDef;
	
	string ghostName;
	
	ActGhost m_act;
	
	// Use this for initialization
	void Start () 
	{
		m_act = new ActGhost( this );
		
		StartCoroutine( m_act.act() );
	}
	
	// Update is called once per frame
	void Update () 
	{


	}
	
	void OnGUI()
	{
		if(renderer.isVisible)
		{
			Vector3 inScreen = Camera.main.WorldToScreenPoint(transform.position);
			Rect labelRect = new Rect(inScreen.x - 100, Screen.height - inScreen.y + 50, 200, 100); 
			GUI.Label(labelRect, ghostName, nameLabel);
		}
	}
	
	public void setName(string name)
	{
		ghostName = name;
	}
}
