using UnityEngine;
using System.Collections;

// TODO: Collider around bear to trigger 'Roar' sound
// TODO: Fix jump and movement physics


public class BearScript : MonoBehaviour 
{
	// Attributes
	public  float velocity;
	public  float rotationSpeed;
	
	public  float jumpSpeed;
	private bool  canJump=true;
	
	public  float roarCheckRadius;
	private SphereCollider roarColliderCheck;
	
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
		//Energy Setup
		World.me.m_energy.lastDecay = Time.realtimeSinceStartup;
		World.me.m_energy.current = World.me.m_energy.max;
		GetComponentInChildren<HealthBar>().Initialize(World.me.m_energy.max, World.me.m_energy.current);
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateMovement();
		UpdateEnergy();
		Roar();
	}
	
	
	public void Roar()
	{
		Collider[] collides = Physics.OverlapSphere(gameObject.transform.position, roarCheckRadius);
		foreach( Collider col in collides)
		{
			if(col.tag == "Person")
			{
				print("Roar!!");
			}
		}
	}
	
	public void UpdateMovement()
	{
		float posY = World.me.getWorldHeight(transform.position.x, transform.position.z);
		if( button.MoveUp )
		{
			transform.Translate(new Vector3(0, posY, velocity * Time.deltaTime) );
		}
		
		if( button.MoveDown )
		{
			transform.Translate(new Vector3(0, posY, -velocity * Time.deltaTime) );
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
			
		}
			
	}
	
	public void UpdateEnergy()
	{
		if( World.me.m_energy.current == 0)
		{
			Debug.Log("That's a dead bear");
			Destroy(this.gameObject);
		}
		
		if( Time.realtimeSinceStartup - World.me.m_energy.lastDecay > World.me.m_energy.decayTime)
		{
			World.me.m_energy.current -= World.me.m_energy.decayAmount;
			World.me.m_energy.lastDecay = Time.realtimeSinceStartup;
		}
		GetComponentInChildren<HealthBar>().UpdateHealth(World.me.m_energy.current);
	}
	
	public void IncreaseEnergy(float amount)
	{
		if ( World.me.m_energy.current + amount < World.me.m_energy.max)
		{
			World.me.m_energy.current += amount;
			GetComponentInChildren<HealthBar>().UpdateHealth(World.me.m_energy.current);
		}
		else
		{
			World.me.m_energy.current = World.me.m_energy.max;
			GetComponentInChildren<HealthBar>().UpdateHealth(World.me.m_energy.current);
		}
	}
	
	void OnGUI()
	{
		GUI.color = new Color(133, 0 ,0);
		Vector3 gameObjPosition = this.gameObject.transform.position;
		GUI.HorizontalScrollbar( new Rect (gameObjPosition.x, gameObjPosition.y + 10, 50, 20), 0, World.me.m_energy.current, 0, World.me.m_energy.max);
	}
	
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
