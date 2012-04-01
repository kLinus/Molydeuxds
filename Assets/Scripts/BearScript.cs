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
	
	public  float magicNumber;
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
<<<<<<< HEAD
		World.me.m_bearEnergy.current = World.me.m_bearEnergy.max;
		GetComponentInChildren<HealthBar>().Initialize(World.me.m_bearEnergy.max, World.me.m_bearEnergy.current);
=======
		World.me.m_energy.lastDecay = Time.realtimeSinceStartup;
		World.me.m_energy.add( World.me.m_energy.max );
		GetComponentInChildren<HealthBar>().Initialize(World.me.m_energy.max, World.me.m_energy.current);
>>>>>>> b77a772e33fa8543e1daf5a8cc672f67083c3bfd
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (World.me.m_currentMode == World.Mode.BEAR)
		{
			UpdateMovement();
			UpdateEnergy();
			Roar();
		}
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
			Move (gameObject.transform.forward);
		}
		if( button.MoveDown )
		{
			Move (-1 * gameObject.transform.forward);
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
	
	public void Move(Vector3 direction)
	{
		float distThisFrame = velocity * Time.deltaTime;
		Vector3 p = gameObject.transform.position + direction * distThisFrame;
		float y = World.me.getWorldHeight( p.x, p.z );
		y += magicNumber;
		
		gameObject.transform.position = new Vector3( p.x, y, p.z );
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
			World.me.m_energy.decay();
		}
		GetComponentInChildren<HealthBar>().UpdateHealth(World.me.m_energy.current);
	}
	
	public void IncreaseEnergy(float amount)
	{
		World.me.m_energy.add ( amount );
		GetComponentInChildren<HealthBar>().UpdateHealth(World.me.m_energy.current);
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
