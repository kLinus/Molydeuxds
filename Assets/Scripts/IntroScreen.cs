using UnityEngine;
using System.Collections;

public class IntroScreen : MonoBehaviour
{
	public GameObject m_titleScreen;
	public GameObject m_howToPlayScreen;
	public GameObject m_controlsScreen;
	public GameObject m_loadingScreen;
	
	private float m_lastCheck = 0;
	private bool  m_waitOver = false;
	
	private int m_screenNumber=0;

	// Use this for initialization
	void Start ()
	{
		m_titleScreen.active = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Time.realtimeSinceStartup > 2.0f)
			m_waitOver = true;

		if(Input.anyKeyDown && m_waitOver)
		{
			if (m_screenNumber == 0)
			{
				Destroy(m_titleScreen);
				m_howToPlayScreen.active = true;
			}
			if(m_screenNumber >= 1)
			{
				m_controlsScreen.active = true;
				Destroy(m_howToPlayScreen);
			}
			
			if(m_screenNumber == 2)
			{
				Destroy(m_controlsScreen);
				m_loadingScreen.active = true;
				Application.LoadLevel("Final");
			}
			m_screenNumber++;
		}
	}
	
	public void WaitForUnityLogo(float wait)
	{
		while( Time.realtimeSinceStartup - m_lastCheck > wait)
		{
			m_lastCheck = Time.realtimeSinceStartup;
		}
		m_waitOver = true;
	}

}

