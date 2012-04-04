using UnityEngine;
using System.Collections;

public class Walker : MonoBehaviour 
{
	public Side m_side = Side.Invalid;
	
	public GameObject m_buildingDef;
	
	public GameObject ghostSpawn;
	public GameObject bloodSpawn;
	
	public float s_ageToMakeBuilding = 60.0f;
	public float s_chanceToMakeBuilding = 0.5f;
	
	public float m_maxAge = 600.0f;
	public GUIStyle nameLabel;
	public GUIStyle shadow;
	
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
		
		float energy = World.me.m_player == m_side ? m_addEnergy : m_addEnergy * World.me.m_energy.increasePerWorker;
		
		World.me.m_energy.add( energy * Time.deltaTime );
		
		if( m_chanceOfBuilding < s_chanceToMakeBuilding )
		{
			if( m_age > s_ageToMakeBuilding )
			{
				m_chanceOfBuilding = 1.0f;
				GameObject hut = Instantiate( m_buildingDef, transform.position, new Quaternion() )as GameObject;
				if (m_side == Side.Blue)
					hut.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
				else if (m_side == Side.Red)
					hut.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
				else
					Debug.Log(m_name + " has no side....DAMNIT JAKE");
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
			Vector3 inScreen = Camera.current.WorldToScreenPoint(transform.position);
			Rect labelRect = new Rect(inScreen.x - 100, Screen.height - inScreen.y - 50, 200, 100); 
			
			GUI.Label(labelRect, m_name, shadow);
			
			labelRect.x -=1;
			labelRect.y -=1;
			shadow = new GUIStyle(nameLabel);
			shadow.normal.textColor = Color.black;
			
			GUI.Label(labelRect, m_name, nameLabel);
		}
	}
	
	public void oldDeath()
	{
		
		Object.Destroy( gameObject );
	}
	
	public void OnDestroy()
	{
		int dummy = 0;
		GameObject temp = (GameObject)Instantiate(ghostSpawn, transform.position, Quaternion.identity);
		temp.GetComponent<Ghost>().setName(m_name);
		temp.GetComponent<DeathGUI>().setName(m_name);
	}
	
	public void bearDeath()
	{
		GameObject temp = (GameObject)Instantiate(bloodSpawn);
		temp.transform.parent = transform;
		temp.transform.localPosition = Vector3.zero;
		StopAllCoroutines();
		Object.Destroy( gameObject , 2.0f);
	}

		static string[] s_firstNames = {
			"MolyEliot ",
			"MolyTim ",
			"MolyChuck ",
			"MolyCharles ",
			"MolyMirfak ",
			"MolyJohn ",
			"MolyRomain ",
			"MolyMark ",
			"MolyJacob ",
			"MolyDavid ",
			"MolyAnthony ",
			"MolyMarc ",
			"MolyKristen ",
			"MolyRahil ",
			"MolyBailie ",
			"MolyRyan ",
			"MolyJacob ",
			"MolyCarlo ",
			"MolyKaren ",
			"MolyTiffany Le ",
			"MolyWilliam ",
			"MolyBrennan ",
			"MolyJoseph ",
			"MolySean ",
			"MolyJeffrey ",
			"MolyCarlos ",
			"MolyMeggan ",
			"MolyJosh ",
			"MolyJakob ",
			"MolyPeter ",
			"MolyJuan ",
			"MolyDamian ",
			"MolyMichael ",
			"MolyKris ",
			"MolyAndrew ",
			"MolyLee ",
			"MolyPaul Du ",
			"MolyTulley ",
			"MolyBen ",
			"MolyMike ",
			"MolyJean-Paul ",
			"MolyDrew ",
			"MolyChris ",
			"MolyMichael ",
			"MolyDoug ",
			"MolyKelsey ",
			"MolyJames ",
			"MolyMarcus ",
			"MolyKevin ",
			"MolyLuke ",
			"MolyTrevor ",
			"MolyAndrew ",
			"MolyShannon ",
			"MolyJett ",
			"MolyTram (Emma) ",
			"MolyNate ",
			"MolyAndrew ",
			"MolyMichael ",
			"MolyLaura ",
			"MolyRich ",
			"MolyChristopher ",
			"MolyBen ",
			"MolyPatrick ",
			"MolyHarrison ",
			"MolyEric ",
			"MolyDmitriy ",
			"MolyBrent ",
			"MolyStephanie ",
			"MolyKirk ",
			"MolyEric ",
			"MolyCeleste ",
			"MolyJustin ",
			"MolySheiva ",
			"MolyColin ",
			"MolyJeff ",
			"MolyShelley ",
			"MolyHavilah ",
			"MolyJoseph ",
			"MolyMichael ",
			"MolyChelsea ",
			"MolyTim ",
			"MolyAntonio ",
			"MolyRegina ",
			"MolyJoseph ",
			"MolySeth ",
			"MolyJohn ",
			"MolyJohn ",
			"MolyJonathan ",
			"MolyJolie ",
			"MolySean ",
			"MolyJeffrey ",
			"MolyJonathon ",
			"MolyMauricio ",
			"MolyPietro ",
			"MolyJoshua ",
			"MolyRichard ",
			"MolyJohn ",
			"MolyCharlie ",
			"MolyWill ",
			"MolyDaniel "
		};
		
		static string[] s_lastNames = {
			"Lashdeux",
			"Kimdeux",
			"Jordandeux",
			"Pattersondeux",
			"Qazideux",
			"Parkdeux",
			"Killiandeux",
			"Cookedeux",
			"Schmuggedeux",
			"Burkedeux",
			"Antonarosdeux",
			"Hernandezdeux",
			"Salvatoredeux",
			"Pateldeux",
			"Dusseaudeux",
			"Loebsdeux",
			"Federicodeux",
			"Delallanadeux",
			"Chudeux",
			"Brundeux",
			"Gahrdeux",
			"Andersondeux",
			"Natolideux",
			"Thompsondeux",
			"Fernandezdeux",
			"Romerodeux",
			"Scaviodeux",
			"Diazdeux",
			"Medlindeux",
			"Mohrbacherdeux",
			"Rubiodeux",
			"Soldeux",
			"Levinedeux",
			"Velezdeux",
			"Langleydeux",
			"Pettydeux",
			"Boisdeux",
			"Raffertydeux",
			"Pollackdeux",
			"Halesdeux",
			"Lebretondeux",
			"Skillmandeux",
			"Jurneydeux",
			"Kenterdeux",
			"Tabaccodeux",
			"Martindeux",
			"Hofmanndeux",
			"Boguedeux",
			"Gadddeux",
			"Rewegadeux",
			"Adamsdeux",
			"Crowelldeux",
			"Meissnerdeux",
			"Zoomdeux",
			"Sulmdeux",
			"Allendeux",
			"Coggeshalldeux",
			"Stevensondeux",
			"Michetdeux",
			"Vreelanddeux",
			"Andersondeux",
			"Mcgrawdeux",
			"Connordeux",
			"Pinkdeux",
			"Nicholasdeux",
			"Golovinovdeux",
			"Jentzschdeux",
			"Tangdeux",
			"Georgedeux",
			"Parsonsdeux",
			"Masinterdeux",
			"Buzondeux",
			"Yazdanideux",
			"Bayerdeux",
			"Hutchinsdeux",
			"Monahandeux",
			"Farnsworthdeux",
			"Millerdeux",
			"Molinarideux",
			"Howedeux",
			"Azadeux",
			"Aiellodeux",
			"Farnsworthdeux",
			"Maliksideux",
			"Roblesdeux",
			"Driscolldeux",
			"Eternaldeux",
			"Ghazariandeux",
			"Menzeldeux",
			"Vanamandeux",
			"Schwinghammerdeux",
			"Banksdeux",
			"Balvaneradeux",
			"Righi Rivadeux",
			"Belldeux",
			"Shemakadeux",
			"Seggersondeux",
			"Hugueanrddeux",
			"Longdeux",
			"Plemmonsdeux"
		};
	
	
	static string getName()
	{
		//return s_firstNames[Random.Range(0,s_firstNames.Length)] + s_lastNames[Random.Range(0,s_lastNames.Length)];
		int x = Random.Range(0, s_firstNames.Length+1);
		return s_firstNames[x] + s_lastNames[x];
	}
}
