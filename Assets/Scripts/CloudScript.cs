using UnityEngine;
using System.Collections;

public class CloudScript : MonoBehaviour {
	
	public  float velocity = 10;
	public  float rotationSpeed = 90;
	
	public  float jumpSpeed = 100;
	private bool  canJump=true;
	
	public  float magicNumber = 0;
	public  float banterTimeInterval = 10;
	private SphereCollider roarColliderCheck;
	private float originalY;
	
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
		originalY = transform.position.y; // Used for magic
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (World.me.m_currentMode == World.Mode.CLOUD)
		{
			UpdateMovement();
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
			Move (-1 * gameObject.transform.right);
		}
		
		if( button.MoveRight )
		{
			Move (gameObject.transform.right);
		}
			
	}
	
	public void Move(Vector3 direction)
	{
		float distThisFrame = velocity * Time.deltaTime;
		Vector3 p = gameObject.transform.position + direction * distThisFrame;
		float y = World.me.getWorldHeight( p.x, p.z ) + originalY;       //These are magic numbers, don't touch!
		
		gameObject.transform.position = new Vector3( p.x, y - 2, p.z );
	}
}