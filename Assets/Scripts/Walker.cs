using UnityEngine;
using System.Collections;

public class Walker : MonoBehaviour 
{
	public Side m_side = Side.Invalid;
	
	public GameObject m_buildingDef;
	
	public GameObject ghostSpawn;
	
	public float s_ageToMakeBuilding = 60.0f;
	public float s_chanceToMakeBuilding = 0.5f;
	
	public float m_maxAge = 600.0f;
	public GUIStyle nameLabel;
	
	public float m_addEnergy = 0.25f;
	
	float m_age = 0.0f;
	
	string m_name;
	
	float m_chanceOfBuilding = Random.Range( 0.0f, 1.0f );
	
	ActWorker m_act;
	
	public ResourceDef m_resCarrying;
	
	// Use this for initialization
	void Start () 
	{
		m_act = new ActWorker( this );
		
		m_name = Walker.getName();
		
		StartCoroutine( m_act.act() );
	}
	
	// Update is called once per frame
	void Update() 
	{
		m_age += Time.deltaTime;
		
		World.me.m_energy.add( m_addEnergy * Time.deltaTime );
		
		if( m_chanceOfBuilding < s_chanceToMakeBuilding )
		{
			if( m_age > s_ageToMakeBuilding )
			{
				m_chanceOfBuilding = 1.0f;
				
				Instantiate( m_buildingDef, transform.position, new Quaternion() );
			}
		}
		
		if( m_age > m_maxAge )
		{
			oldDeath();
		}
		
		float waterY = World.me.m_waterObj.transform.position.y;
		
		if( transform.position.y < waterY )
		{
			oldDeath();
		}
	}
	
	void OnGUI()
	{
		if(renderer.isVisible)
		{
			GUI.depth = 10;
			Vector3 inScreen = Camera.main.WorldToScreenPoint(transform.position);
			Rect labelRect = new Rect(inScreen.x - 100, Screen.height - inScreen.y - 50, 200, 100); 
			GUI.Label(labelRect, m_name, nameLabel);
		}
	}
	
	void oldDeath()
	{
		GameObject temp = (GameObject)Instantiate(ghostSpawn, transform.position, Quaternion.identity);
		temp.GetComponent<Ghost>().setName(m_name);
		temp.GetComponent<DeathGUI>().setName(m_name);
		
		Object.Destroy( gameObject );
	}
	
	static string getName()
	{
		string[] names = {
			"Eliot Lashdeux",
			"Tim Kimdeux",
			"Chuck Jordandeux",
			"Charles Pattersondeux",
			"Mirfak Qazideux",
			"John Parkdeux",
			"Romain Killiandeux",
			"Mark Cookedeux",
			"Jacob Schmuggedeux",
			"David Burkedeux",
			"Anthony Antonarosdeux",
			"Marc Hernandezdeux",
			"Kristen Salvatoredeux",
			"Rahil Pateldeux",
			"Bailie Dusseaudeux",
			"Ryan Loebsdeux",
			"Jacob Federicodeux",
			"Carlo Delallanadeux",
			"Karen Chudeux",
			"Tiffany Le Brundeux",
			"William Gahrdeux",
			"Brennan Andersondeux",
			"Joseph Natolideux",
			"Sean Thompsondeux",
			"Jeffrey Fernandezdeux",
			"Carlos Romerodeux",
			"Meggan Scaviodeux",
			"Josh Diazdeux",
			"Jakob Medlindeux",
			"Peter Mohrbacherdeux",
			"Juan Rubiodeux",
			"Damian Soldeux",
			"Michael Levinedeux",
			"Kris Velezdeux",
			"Andrew Langleydeux",
			"Lee Pettydeux",
			"Paul Du Boisdeux",
			"Tulley Raffertydeux",
			"Ben Pollackdeux",
			"Mike Halesdeux",
			"Jean-Paul Lebretondeux",
			"Drew Skillmandeux",
			"Chris Jurneydeux",
			"Michael Kenterdeux",
			"Doug Tabaccodeux",
			"Kelsey Martindeux",
			"James Hofmanndeux",
			"Marcus Boguedeux",
			"Kevin Gadddeux",
			"Luke Rewegadeux",
			"Trevor Adamsdeux",
			"Andrew Crowelldeux",
			"Shannon Meissnerdeux",
			"Jett Zoomdeux",
			"Tram (Emma) Sulmdeux",
			"Nate Allendeux",
			"Andrew Coggeshalldeux",
			"Michael Stevensondeux",
			"Laura Michetdeux",
			"Rich Vreelanddeux",
			"Christopher Andersondeux",
			"Ben Mcgrawdeux",
			"Patrick Connordeux",
			"Harrison Pinkdeux",
			"Eric Nicholasdeux",
			"Dmitriy Golovinovdeux",
			"Brent Jentzschdeux",
			"Stephanie Tangdeux",
			"Kirk Georgedeux",
			"Eric Parsonsdeux",
			"Celeste Masinterdeux",
			"Justin Buzondeux",
			"Sheiva Yazdanideux",
			"Colin Bayerdeux",
			"Jeff Hutchinsdeux",
			"Shelley Monahandeux",
			"Havilah Farnsworthdeux",
			"Joseph Millerdeux",
			"Michael Molinarideux",
			"Chelsea Howedeux",
			"Tim Azadeux",
			"Antonio Aiellodeux",
			"Regina Farnsworthdeux",
			"Joseph Maliksideux",
			"Seth Roblesdeux",
			"John Driscolldeux",
			"John Eternaldeux",
			"Jonathan Ghazariandeux",
			"Jolie Menzeldeux",
			"Sean Vanamandeux",
			"Jeffrey Schwinghammerdeux",
			"Jonathon Banksdeux",
			"Mauricio Balvaneradeux",
			"Pietro Righi Rivadeux",
			"Joshua Belldeux",
			"Richard Shemakadeux",
			"John Seggersondeux",
			"Charlie Hugueanrddeux",
			"Will Longdeux",
			"Daniel Plemmonsdeux"
		};
		
		return names[Random.Range(0,names.Length)];
	}
}
