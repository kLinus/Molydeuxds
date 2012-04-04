using UnityEngine;
using System.Collections;

// TODO: Collider around bear to trigger 'Roar' sound
// TODO: Fix jump and movement physics


public class BearScript : MonoBehaviour 
{
	// Attributes
	public  float velocity = 10;
	public  float rotationSpeed = 90;
	
	public  float jumpSpeed = 1000;
	private bool  canJump=true;
	
	public GameObject bloodPrefab;
	
	public  float magicNumber;
	public  float roarCheckRadius = 15;
	private SphereCollider roarColliderCheck;
	
	private bool roaring;
	
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
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (World.me.m_currentMode == World.Mode.BEAR)
		{
			if ( transform.gameObject.GetComponent<RabidBear>().enabled )
			{
				gameObject.GetComponent<RabidBear>().enabled = false;
				Debug.Log("Rabid Bear Script disabled");
			}
			
			UpdateMovement();
			UpdateEnergy();
			Roar();
			CheckforKill();
			
			
		}
	}
	
	
	public void Roar()
	{
		Collider[] collides = Physics.OverlapSphere(gameObject.transform.position, roarCheckRadius, World.me.s_layerWalker);
		foreach( Collider col in collides)
		{
			
			AudioSource[] audios = GetComponents<AudioSource>();
			if(!roaring)
				audios[Random.Range(0,audios.Length-1)].Play();
			roaring = true;
				
		}
		if(collides.Length == 0)
			roaring = false;

	}
	
	void CheckforKill()
	{
		Collider[] collides = Physics.OverlapSphere(gameObject.transform.position, .5f, World.me.s_layerWalker);
		foreach( Collider col in collides)
		{
			col.gameObject.GetComponent<Walker>().bearDeath();
				
		}

	}
	
	public void UpdateMovement()
	{
		
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
		if( World.me.m_energy.current <= 2)
		{
			Debug.Log("That's a dead bear");
			World.me.m_currentMode = World.Mode.GOD;
			GameObject temp = Instantiate(bloodPrefab) as GameObject;
			temp.transform.parent = transform;
			temp.transform.localPosition = Vector3.zero;
			World.me.RefreshCameras();
			Destroy(this.gameObject, 3.0f);
		}
		
		if( Time.realtimeSinceStartup - World.me.m_energy.lastDecay > World.me.m_energy.decayTime)
		{
			World.me.m_energy.decay();
		}
	}
	
	public void IncreaseEnergy(float amount)
	{
		World.me.m_energy.add ( amount );
		GetComponentInChildren<HealthBar>().UpdateHealth(World.me.m_energy.current);
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
