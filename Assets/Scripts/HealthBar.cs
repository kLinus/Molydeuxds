using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GUITexture) )]
public class HealthBar : MonoBehaviour 
{
	public float currentHealth = 0.0f;
	public float maxHealth = 0.0f;
	public float barHeightOffset;
	public GameObject pinnedObject;
	private bool isInitialized = false;
	private GameObject healthBar;
	public GameObject healthBarPrefab;
				
	// Use this for initialization
	void Start () 
	{
		if(pinnedObject != null)
		{
			AttachToGameObject(pinnedObject);
		}
		
		if( currentHealth != 0.0f && maxHealth != 0.0f)
		{
			isInitialized = true;
		}
	}
	
	void Update () 
	{
		if (isInitialized && healthBar != null)
		{
			healthBar.transform.position = OffsetVector;
			healthBar.transform.rotation = pinnedObject.transform.rotation;
			Debug.Log("It's moving!");
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
		if (healthBarPrefab != null)
		{
			healthBar = (GameObject)GameObject.Instantiate(this.healthBarPrefab, pinnedObject.transform.position, pinnedObject.transform.rotation);
	
		}
		else
		{
			Debug.Log("Set up the healthBarPrefab plx");
		}
	}
	
	public Vector3 OffsetVector
	{
		get{ return new Vector3(pinnedObject.transform.position.x, pinnedObject.transform.position.y + barHeightOffset, pinnedObject.transform.position.z); }
	}
}
