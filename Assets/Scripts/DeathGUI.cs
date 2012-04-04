using UnityEngine;
using System.Collections;

public class DeathGUI : MonoBehaviour
{

	bool showingDeath = true;
	
	public GUIStyle background;
	public GUIStyle header1;
	public GUIStyle paragraph;
	public GUIStyle exitButton;
	public GUIStyle skipButton;
	
	//public string name;
	
	string[] paragraphs;
	string[] headers;
	
	Vector2 scrollposition;
	
	// Use this for initialization
	void Start ()
	{
		headers = new string[] {
			"Early Childhood",
			"Teen Years",
			"Adulthood",
			"Late Adulthood",
			"Death"
		};
		paragraphs = new string[headers.Length];
		
		MadLib ml = new MadLib();
		Debug.Log(paragraphs.Length + " " + headers.Length);
		for(int i = 0; i < paragraphs.Length; i++)
		{
			Debug.Log("Get Paragraph: " + i);
			paragraphs[i] = ml.GetParagraph(i, name);
		}
		
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		
	}
	
	void OnGUI()
	{
		if(showingDeath && World.me.m_displayDeathGUI)
		{
			GUI.depth = 5;
			
			if(Input.GetButtonDown("Back"))
				hide();
			
			float scrollwidth = background.fixedWidth - background.padding.left - background.padding.right - 20;
			
			Rect windowRect = new Rect((Screen.width - background.fixedWidth) / 2, (Screen.height - background.fixedHeight)/2, background.fixedWidth, background.fixedHeight);
			GUILayout.BeginArea(windowRect, background);
			GUILayout.BeginVertical();
			
			scrollposition = GUILayout.BeginScrollView(scrollposition);
			GUILayout.BeginVertical(GUILayout.Width(scrollwidth));
	/******* I'm going to create a custom style here becuase I can't figure out where the rest of the GUI is */
	/******* Make sure to take these lines out when the right place is found */		
			
			GUIStyle nameStyle = new GUIStyle(header1);
			nameStyle.alignment = TextAnchor.MiddleCenter;
			GUILayout.Label(gameObject.name, nameStyle);
	/******* End of shotty shit */		
			
			for(int i = 0; i < paragraphs.Length; i++)
			{
				GUILayout.Label(headers[i], header1);
				GUILayout.Label(paragraphs[i], paragraph);	
			}
			
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if(GUILayout.Button(" ", exitButton))
				hide();
			if(GUILayout.Button(" ", skipButton))
				World.me.m_displayDeathGUI = false;
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.EndArea();
			
		}
	}
	
	public void show()
	{
		
		scrollposition = Vector2.zero;
		showingDeath = true;
	}
	
	public void hide()
	{
		showingDeath = false;
		GetComponent<DeathGUI>().enabled = false;
	}
	
	public void setName(string newName)
	{
		name = newName;
	}
	
}

