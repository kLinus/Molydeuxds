using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GUITexture) )]
public class HealthBar : MonoBehaviour 
{
	public float currentHealth;
	public float maxHealth;
	public GameObject pinnedObject;
	private bool isInitialized = false;
	private Texture healthBar;
	private Camera cam;
				
	// Use this for initialization
	void Start () 
	{
	
	}
	
	void OnGUI () 
	{
		if (isInitialized && cam != null)
		{
			Vector3 pos = cam.WorldToScreenPoint(pinnedObject.transform.position);
			GUI.color = new Color(255, 0, 0);
			GUI.HorizontalScrollbar(new Rect(pos.x - maxHealth/2, pos.y + 20, 100, 20), 0, currentHealth, 0, maxHealth);
			
		}
		else if (cam == null)
		{
			cam = Camera.mainCamera;
		}
		else
		{
			if ( pinnedObject == null)
			{
				Debug.Log ("Healthbar is not initialized nor pinned to an object!");
			}
			else
			{
				Debug.Log ("Healthbar for object '" + pinnedObject + "' is not initialized!");
			}
		}
	}
	
	public void Initialize(float max, float current)
	{
		currentHealth = current;
		maxHealth = max;
		isInitialized = true;
	}
	public void UpdateHealth(float current)
	{
		this.currentHealth = current;
	}
	
	public void AttachToGameObject(GameObject gObject)
	{
		pinnedObject = gObject;
	}
}
