using UnityEngine;
using System.Collections;

public class IntroScreen : MonoBehaviour
{
	public GameObject m_titleScreen;
	public GameObject m_howToPlayScreen;
	public GameObject m_controlsScreen;
	
	private int m_screenNumber=0;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.anyKeyDown)
		{
			Destroy(m_titleScreen);
			if(m_screenNumber >= 1)
			{
				Destroy(m_howToPlayScreen);
			}
			
			if(m_screenNumber == 2)
			{
				Destroy(m_controlsScreen);
				Application.LoadLevel("Final");
			}
			m_screenNumber++;
		}
	}

}

