using UnityEngine;
using System.Collections;

// TODO: Collider around bear to trigger 'Roar' sound
// TODO: Collision when eating people
// TODO: Energy Bar
// TODO: Fix jump and movement physics

// Required Components
[RequireComponent( typeof(Rigidbody) )]
public class BearScript : MonoBehaviour 
{
	// Attributes
	public  float velocity;
	public  float rotationSpeed;
	
	public  float jumpSpeed;
	private bool  canJump=true;
	
	[System.SerializableAttribute]
	public class EnergyProperties
	{
		public float max;
		public float decayAmount;
		public float decayTime;
		public float current;
		public float lastDecay;
	} public EnergyProperties energy;
	
	// Bear Actions
	public class ButtonProperties
	{
		public bool Jump      { get { return Input.GetKey( KeyCode.Space);} }
		public bool MoveUp    { get { return Input.GetKey( KeyCode.W);} }
		public bool MoveDown  { get { return Input.GetKey( KeyCode.S);} }
		public bool MoveLeft  { get { return Input.GetKey( KeyCode.A);} }
		public bool MoveRight { get { return Input.GetKey( KeyCode.D);} }
		
	} public ButtonProperties button = new ButtonProperties();
	
	// Use this for initialization
	void Start () 
	{
		energy.lastDecay = Time.realtimeSinceStartup;
		energy.current = energy.max;
		GetComponentInChildren<HealthBar>().Initialize(energy.max, energy.current);
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateMovement();
		UpdateEnergy();
	}
	
	public void UpdateMovement()
	{
		if( button.MoveUp )
		{
			transform.Translate(new Vector3(0,0, velocity * Time.deltaTime) );
		}
		
		if( button.MoveDown )
		{
			transform.Translate(new Vector3(0,0, -velocity * Time.deltaTime) );
		}
		
		if( button.MoveLeft )
		{
			transform.Rotate(new Vector3(0, -rotationSpeed * Time.deltaTime, 0));
		}
		
		if( button.MoveRight )
		{
			transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
		}
		
		if( button.Jump && canJump )
		{
			rigidbody.AddForce( new Vector3(0, jumpSpeed * Time.deltaTime, 0));
		}
			
	}
	
	public void UpdateEnergy()
	{
		if( energy.current == 0)
		{
			Debug.Log("That's a dead bear");
			Destroy(this.gameObject);
		}
		
		if( Time.realtimeSinceStartup - energy.lastDecay > energy.decayTime)
		{
			energy.current -= energy.decayAmount;
			energy.lastDecay = Time.realtimeSinceStartup;
		}
		GetComponentInChildren<HealthBar>().UpdateHealth(energy.current);
	}
	
	public void IncreaseEnergy(float amount)
	{
		if (energy.current + amount < energy.max)
		{
			energy.current += amount;
			GetComponentInChildren<HealthBar>().UpdateHealth(energy.current);
		}
		else
		{
			energy.current = energy.max;
			GetComponentInChildren<HealthBar>().UpdateHealth(energy.current);
		}
	}
	
//	void OnGUI()
//	{
//		GUI.color = new Color(133, 0 ,0);
//		Vector3 gameObjPosition = this.gameObject.transform.position;
//		GUI.HorizontalScrollbar( new Rect (gameObjPosition.x, gameObjPosition.y + 10, 50, 20), 0, energy.current, 0, energy.max);
//	}
	
	public void TriggerJump()
	{
		if(canJump)
		{
			
		}
	}
	
	public void UpdateJump()
	{
	}
}
