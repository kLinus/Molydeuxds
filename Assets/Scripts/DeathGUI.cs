using UnityEngine;
using System.Collections;

public class DeathGUI : MonoBehaviour
{

	bool showingDeath = true;
	
	public GUIStyle background;
	public GUIStyle header1;
	public GUIStyle paragraph;
	public GUIStyle exitButton;
	
	public string name;
	
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
		for(int i = 0; i < paragraphs.Length; i++)
			paragraphs[i] = ml.GetParagraph(i, name);
		
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		
	}
	
	void OnGUI()
	{
		if(showingDeath)
		{
			GUI.depth = 5;
			
			if(Input.GetButtonDown("Back"))
				hide();
			
			Rect windowRect = new Rect((Screen.width - background.fixedWidth) / 2, (Screen.height - background.fixedHeight)/2, background.fixedWidth, background.fixedHeight);
			GUILayout.BeginArea(windowRect, background);
			GUILayout.BeginVertical();
			
			scrollposition = GUILayout.BeginScrollView(scrollposition);
			GUILayout.BeginVertical();
			for(int i = 0; i < paragraphs.Length; i++)
			{
				GUILayout.Label(headers[i], header1);
				GUILayout.Label(paragraphs[i], paragraph);	
			}
			
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
			if(GUILayout.Button("Skip for 5 credits", exitButton))
				hide();
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

