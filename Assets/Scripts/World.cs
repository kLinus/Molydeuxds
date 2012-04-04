using UnityEngine;
using System.Collections;
using System.Collections.Generic;



// TODO: Make Cameras through code
public enum Side
{
	Invalid,
	Red,
	Blue,
}

public class Chunk : MonoBehaviour
{
	static public float s_chunkHalf = 0.5f;
	static public int s_chunkSize = 16;
	
	short[] m_types = new short[s_chunkSize * s_chunkSize * s_chunkSize];
	
	static Vector2[] s_texOff = 
	{
		new Vector2( 0.0f, 0.5f ), //Blank
		new Vector2( 0.5f, 0.5f ), //Grass
		new Vector2( 0.0f, 0.0f ), //Mud
		new Vector2( 0.5f, 0.0f ), //Stone
	};
	
	World m_world;
	
	//World basis
	int m_x;
	int m_y;
	int m_z;
	
	public void MakeNewChunk( World world, int x, int y, int z )
	{
		m_world = world;
		m_x = x;
		m_y = y;
		m_z = z;
		
		//m_mesh = new Mesh();
	}
	
	public int localToIndex( int x, int y, int z )
	{
		return y * s_chunkSize * s_chunkSize + z * s_chunkSize + x;
	}
	
	public short getType_l( int x, int y, int z )
	{
		int index = localToIndex( x, y, z );
		
		return m_types[index];
	}
	
	public void remBlock_l( int x, int y, int z )
	{
		if( y == 0 ) return;
		
		int index = localToIndex( x, y, z );
		
		m_types[index] = 0;
		
		createGeo();
	}
	
	public void addBlock_l( short type, int x, int y, int z )
	{
		if( y == 15 ) return;
		
		int index = localToIndex( x, y, z );
		
		m_types[index] = type;
		
		createGeo();
	}
	
	public float getWorldHeight( int x, int z )
	{
		short dontCare;
		return getWorldHeight( x, z, out dontCare );
	}
	
	public float getWorldHeight( int x, int z, out short o_type )
	{
		for( int y = 15; y >= 0; --y )
		{
			short type = getType_l( x, y, z );
			if( type != 0 )
			{
				o_type = type;
				return m_y + y + 1.0f;
			}
		}
		
		o_type = 0;
		return m_y;
	}
	
	public short getType( int x, int y, int z )
	{
		bool isOutside = (x < 0) | (y < 0) | (z < 0) | (x >= s_chunkSize) | (y >= s_chunkSize) | (z >= s_chunkSize);

		if( isOutside ) return m_world.getType( x + m_x, y + m_y, z + m_z );
		else return getType_l( x, y, z );
	}
	
	public void createRandomCubes( float noiseZ )
	{
		Noise.Triple fnFBNLargeSmooth = ( x, y, z ) => Mathf.Sin( ( 1.0f / Mathf.PI * 2.0f ) * 0.7f * ( 0.44f + (float)Noise.Perlin.fBm( x, y, z + 57.0f, ( x1, y1, z1 ) => 3.0f, ( x1, y1, z1 ) => 0.3f, ( x1, y1, z1 ) => 0.7f ) ) );
        Noise.Triple fnFBNPlanety = ( x, y, z ) =>  0.79 * ( 0.62 + Noise.Perlin.fBm( x, y, z + 37, ( x1, y1, z1 ) => 10, ( x1, y1, z1 ) => 1.9, ( x1, y1, z1 ) => 0.6 ) );
        Noise.Triple fnFBNStrings = ( x, y, z ) => Mathf.Max( -0.2f, Mathf.Min( 1.2f, 20.0f * ( -0.1f + (float)Noise.Perlin.fBm( x, y, z + 79.0f, ( x1, y1, z1 ) => 6.0f, ( x1, y1, z1 ) => 2.0f, ( x1, y1, z1 ) => 0.1f ) ) ) );
        Noise.Triple fnRidged = ( x, y, z ) => 0.8 * ( -0.24 + Noise.Perlin.RidgedMF( x, y, z, (int)12, (float)1.8, (float)0.65f, 1.0f ) );
        Noise.Triple fnChunkPlanety = ( x, y, z ) => 0.0 + ( 0.3 * (1 - fnFBNStrings( x, y, z )) * fnFBNPlanety( x, y, z ) );
        Noise.Triple fnStringRidged = ( x, y, z ) => 0.0 + ( 0.3 * fnFBNStrings( x, y, z ) * fnRidged( x, y, z ) );
        Noise.Triple fnFinal = ( x, y, z ) => fnFBNLargeSmooth( x, y, z ) + fnStringRidged(x,y,z) + fnChunkPlanety(x,y,z);
		
		for( int z = 0; z < 16; ++z )
		{
			float fz = (float)z;
			for( int x = 0; x < 16; ++x )
			{
				float fx = (float)x;
			
				float wx = fx + m_x;
				float wz = fz + m_z;
				
				float heightUnit = (float)fnFinal( wx / 50.0f, wz / 50.0f, noiseZ );
				
				float hut0X = wx - 8.0f;
				float hut0Z = wz - 8.0f;				
				bool nearHut0 = hut0X * hut0X + hut0Z * hut0Z < (16.0f*16.0f);

				float hut1X = wx - 152.0f;
				float hut1Z = wz - 152.0f;				
				bool nearHut1 = hut1X * hut1X + hut1Z * hut1Z < (16.0f*16.0f);
				
				float height = ( heightUnit ) * 12.0f + 4.0f;
				
				if( nearHut0 || nearHut1 )
				{
					height = Mathf.Max( height, 9 );
				}
						
				
				int index0 = localToIndex( x, 0, z );
				m_types[ index0 ] = 1;
				
				for( int y = 1; y < 15; ++y )
				{
					float fy = (float)y;
				
					int index = localToIndex( x, y, z );
					
					float heightDiff = height - fy;
					
					short val = 0;
					
					if( heightDiff < 0.0f )
					{
						val = 0;
					}
					else if( heightDiff > 3.0f )
					{
						val = 3;
					}
					else if( heightDiff > 2.0f )
					{
						//val = 2;
						val = fy > 6.5f ? (short)1 : (short)2;
					}
										
					m_types[ index ] = val;
				}
			}
		}
	}
	
	int getVertIndex( Vector3 v, Vector2 u, List<Vector3> verts, List<Vector2> uvs, Dictionary<Vector3, int> map )
	{
		/*
		if( map.ContainsKey( v ) )
		{
			return map[v];
		}
		*/
		
		int index = verts.Count;
		
		map[v] = index;
		verts.Add( v );
		uvs.Add ( u );
		
		return index;
	}
	
	void createTriangle( Vector3 v0, Vector3 v1, Vector3 v2, Vector2 u0, Vector2 u1, Vector2 u2, List<Vector3> verts, List<Vector2> uvs, Dictionary<Vector3, int> map, List<int> indices )
	{
		int i0 = getVertIndex( v0, u0, verts, uvs, map );
		int i1 = getVertIndex( v1, u1, verts, uvs, map );
		int i2 = getVertIndex( v2, u2, verts, uvs, map );
		
		indices.Add( i0 );
		indices.Add( i1 );
		indices.Add( i2 );
	}
	
	public void createGeo()
	{
		List<Vector3> verts = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		Dictionary<Vector3, int> map = new Dictionary<Vector3, int>();
		List<int> indices = new List<int>();
		List<Color> colors = new List<Color>();


		for( int y = 0; y < 16; ++y )
		{
			float fy = (float)y;
			float yhp = fy + 0.5f;
			float yhn = fy - 0.5f;
			
			float fColor = Mathf.Clamp( (float)y / 6.0f, 0.0f, 1.0f );
			
			Color color = new Color( fColor, fColor, fColor );

			for( int z = 0; z < 16; ++z )
			{
				float fz = (float)z;
				float zhp = fz + 0.5f;
				float zhn = fz - 0.5f;

				for( int x = 0; x < 16; ++x )
				{
					float fx = (float)x;
					float xhp = fx + 0.5f;
					float xhn = fx - 0.5f;

					short myType = getType( x, y, z );
					
					if( myType == 0 ) continue;
					
					short xp = getType( x + 1, y + 0, z + 0 );
					short yp = getType( x + 0, y + 1, z + 0 );
					short zp = getType( x + 0, y + 0, z + 1 );
					
					short xn = getType( x - 1, y + 0, z + 0 );
					short yn = getType( x + 0, y - 1, z + 0 );
					short zn = getType( x + 0, y + 0, z - 1 );
					
					if( xp == 0 )
					{
						Vector3 v0 = new Vector3( xhp, yhp, zhn );
						Vector3 v1 = new Vector3( xhp, yhp, zhp );
						Vector3 v2 = new Vector3( xhp, yhn, zhp );
						Vector3 v3 = new Vector3( xhp, yhn, zhn );
						
						Vector2 u0 = s_texOff[myType] + new Vector2( 0.0f, 0.0f );
						Vector2 u1 = s_texOff[myType] + new Vector2( 0.5f, 0.0f );
						Vector2 u2 = s_texOff[myType] + new Vector2( 0.5f, 0.5f );
						Vector2 u3 = s_texOff[myType] + new Vector2( 0.0f, 0.5f );
						
						createTriangle ( v0, v1, v3, u0, u1, u3, verts, uvs, map, indices );
						createTriangle ( v1, v2, v3, u1, u2, u3, verts, uvs, map, indices );
						
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
					}
					
					if( yp == 0 )
					{
						Vector3 v0 = new Vector3( xhn, yhp, zhp );
						Vector3 v1 = new Vector3( xhp, yhp, zhp );
						Vector3 v2 = new Vector3( xhp, yhp, zhn );
						Vector3 v3 = new Vector3( xhn, yhp, zhn );
						
						Vector2 u0 = s_texOff[myType] + new Vector2( 0.0f, 0.0f );
						Vector2 u1 = s_texOff[myType] + new Vector2( 0.5f, 0.0f );
						Vector2 u2 = s_texOff[myType] + new Vector2( 0.5f, 0.5f );
						Vector2 u3 = s_texOff[myType] + new Vector2( 0.0f, 0.5f );
						
						createTriangle ( v0, v1, v3, u0, u1, u3, verts, uvs, map, indices );
						createTriangle ( v1, v2, v3, u1, u2, u3, verts, uvs, map, indices );						

						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
					}
					
					if( zp == 0 )
					{
						Vector3 v0 = new Vector3( xhp, yhp, zhp );
						Vector3 v1 = new Vector3( xhn, yhp, zhp );
						Vector3 v2 = new Vector3( xhn, yhn, zhp );
						Vector3 v3 = new Vector3( xhp, yhn, zhp );
						
						Vector2 u0 = s_texOff[myType] + new Vector2( 0.0f, 0.0f );
						Vector2 u1 = s_texOff[myType] + new Vector2( 0.5f, 0.0f );
						Vector2 u2 = s_texOff[myType] + new Vector2( 0.5f, 0.5f );
						Vector2 u3 = s_texOff[myType] + new Vector2( 0.0f, 0.5f );
						
						createTriangle ( v0, v1, v3, u0, u1, u3, verts, uvs, map, indices );
						createTriangle ( v1, v2, v3, u1, u2, u3, verts, uvs, map, indices );						

						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
					}
					
					if( xn == 0 )
					{
						Vector3 v0 = new Vector3( xhn, yhp, zhp );
						Vector3 v1 = new Vector3( xhn, yhp, zhn );
						Vector3 v2 = new Vector3( xhn, yhn, zhn );
						Vector3 v3 = new Vector3( xhn, yhn, zhp );
						
						Vector2 u0 = s_texOff[myType] + new Vector2( 0.0f, 0.0f );
						Vector2 u1 = s_texOff[myType] + new Vector2( 0.5f, 0.0f );
						Vector2 u2 = s_texOff[myType] + new Vector2( 0.5f, 0.5f );
						Vector2 u3 = s_texOff[myType] + new Vector2( 0.0f, 0.5f );
						
						createTriangle ( v0, v1, v3, u0, u1, u3, verts, uvs, map, indices );
						createTriangle ( v1, v2, v3, u1, u2, u3, verts, uvs, map, indices );						

						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
					}
					
					if( yn == 0 )
					{
						Vector3 v0 = new Vector3( xhp, yhn, zhp );
						Vector3 v1 = new Vector3( xhn, yhn, zhp );
						Vector3 v2 = new Vector3( xhn, yhn, zhn );
						Vector3 v3 = new Vector3( xhp, yhn, zhn );
						
						Vector2 u0 = s_texOff[myType] + new Vector2( 0.0f, 0.0f );
						Vector2 u1 = s_texOff[myType] + new Vector2( 0.5f, 0.0f );
						Vector2 u2 = s_texOff[myType] + new Vector2( 0.5f, 0.5f );
						Vector2 u3 = s_texOff[myType] + new Vector2( 0.0f, 0.5f );
						
						createTriangle ( v0, v1, v3, u0, u1, u3, verts, uvs, map, indices );
						createTriangle ( v1, v2, v3, u1, u2, u3, verts, uvs, map, indices );	
						
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						
					}
					
					if( zn == 0 )
					{
						Vector3 v0 = new Vector3( xhn, yhp, zhn );
						Vector3 v1 = new Vector3( xhp, yhp, zhn );
						Vector3 v2 = new Vector3( xhp, yhn, zhn );
						Vector3 v3 = new Vector3( xhn, yhn, zhn );
						
						Vector2 u0 = s_texOff[myType] + new Vector2( 0.0f, 0.0f );
						Vector2 u1 = s_texOff[myType] + new Vector2( 0.5f, 0.0f );
						Vector2 u2 = s_texOff[myType] + new Vector2( 0.5f, 0.5f );
						Vector2 u3 = s_texOff[myType] + new Vector2( 0.0f, 0.5f );
						
						createTriangle ( v0, v1, v3, u0, u1, u3, verts, uvs, map, indices );
						createTriangle ( v1, v2, v3, u1, u2, u3, verts, uvs, map, indices );
						
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
						colors.Add( color );
					}

				}
			}
		}
		
		//GetCompon
		
		//GetComponent<MeshRenderer>().mesh = 
		
		Mesh mesh = new Mesh();
		
		mesh.vertices = verts.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.triangles = indices.ToArray();
		mesh.colors = colors.ToArray();
		
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();
		
		GetComponent<MeshFilter>().mesh = mesh;
		//GetComponent<MeshFilter>().sharedMesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;
		//GetComponent<MeshRenderer>().enabled = true;
	}
}


public class World : MonoBehaviour 
{
	static public bool isOutside( Vector3 pos )
	{
		if( pos.x < 0 || pos.z < 0 ) return true;
		if( pos.x > (float)s_chunkSide * s_chunkWorldSize || pos.y > (float)s_chunkSide * s_chunkWorldSize ) return true;
		
		return false;
	}
	
	
	
	#region Variables
	
	public LayerMask s_layerHut;
	public LayerMask s_layerWalker;
	public LayerMask s_layerResource;
	
	static public int s_chunkSide = 10;
	static public float s_chunkWorldSize = (float)Chunk.s_chunkSize;
	public enum Mode {BEAR, CLOUD, GOD};
	public Mode m_currentMode = Mode.GOD;
	
	private Camera camBEAR = null;
	private Camera camGOD = null;
	private Camera camCLOUD = null;
	
	public Rect statsGUI = new Rect(0, 0, 200, 100);
		
	[System.SerializableAttribute]
	public class EnergyProperties
	{
		public float max;
		public float decayAmount;
		public float decayTime;
		public float lastDecay;
		public float increasePerWorker = 0.01f;
		public float current{ get; private set; }
		
		public void add( float val )
		{
			current = Mathf.Clamp( current + val, 0, max );
		}
		
		public void decay()
		{
			current -= decayAmount;
			lastDecay = Time.realtimeSinceStartup;
		}
	}
	
	public EnergyProperties m_energy;
	
	public static World me;

	public Side m_player = Side.Invalid;

	GameObject[] m_chunks = new GameObject[ s_chunkSide * s_chunkSide ];
	
	public GameObject m_chunkObjectDef;

	public GameObject m_bearDef;
	public GameObject m_cloudDef;

	public GameObject m_rockDef;
	public GameObject m_treeDef;
	public GameObject m_foodDef;

	public GameObject m_hutRedDef;
	public GameObject m_hutBlueDef;

	public float m_godRainEnergy	= 25.0f;
	public float m_godAddLandEnergy	= 1.0f;
	public float m_godRemLandEnergy	= 1.0f;
	public float m_godFloodEnergy	= 100.0f;
	public float m_godDroughtEnergy	= 100.0f;
	public float m_godVolcanoEnergy	= 100.0f;
	public float m_cooldownGrowFood = 10;
	public float m_cooldownGrowBear = 20;
	public float m_cooldownGrowTree = 10;
	
	
	private int m_numWalkerRed;
	private int m_numWalkerBlue;
	private int m_numHutRed;
	private int m_numHutBlue;
	private int m_numBears;
	private int m_numFoodLeft;
	
	
	
	public GameObject m_guiEnergy;

	public GameObject m_waterObj;


	public int m_volcanoProbes = 1000;
	public float m_volcanoRadius = 8.0f;
	
	
	
	#endregion
		
	void Start () 
	{
		m_energy.add(m_energy.max);
		createWorld();

		//hutColor = Color.red;
		//m_hutRedDef.renderer.material.color = hutColor;
		
		{
			float bX = 8.0f; 
			float bZ = 8.0f; 
			
			float bY = getWorldHeight( bX, bZ ) - 0.5f;
	
			Vector3 bPos = new Vector3( bX, bY, bZ );
			GameObject bHut = Instantiate( m_hutBlueDef, bPos, new Quaternion() ) as GameObject;
			bHut.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
		}		
		
		{
			float maxSize = (float)s_chunkSide * s_chunkWorldSize;
			
			float rX = maxSize - 8.0f;
			float rZ = maxSize - 8.0f;
			
			float rY = getWorldHeight( rX, rZ ) - 0.5f;
	
			Vector3 rPos = new Vector3( rX, rY, rZ );
	
			GameObject rHut = Instantiate( m_hutRedDef, rPos, new Quaternion() ) as GameObject;
			rHut.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
		}		

		
		scatterClouds( m_cloudDef, 20 );

		scatterResource( m_foodDef, 200, 1 );
		scatterResource( m_rockDef, 50, -1 );
		scatterResource( m_treeDef, 500, 1 );
		
		StartCoroutine( growBear() );
		StartCoroutine( growFood() );
		StartCoroutine( growTrees() );
	}
	
	void Update()
	{	
		//CheaterKeys();
		//Determine which camera is which
		if ( camBEAR == null || camCLOUD == null || camGOD == null) // if they aren't initialized
		{
			Camera[] cameras = Camera.allCameras;
			
			//Initialize them
			foreach(Camera cam in cameras)
			{
				if(cam.name == "CameraBear")
				{
					camBEAR = cam;
				}
				
				else if(cam.name == "CameraGod")
				{
					camGOD = cam;
				}
				
				else if(cam.name == "CameraCloud")
				{
					camCLOUD = cam;
				}
				
				cam.enabled = false;
			}
		}
		
		m_numWalkerRed  = GameObject.FindGameObjectsWithTag("WalkerRed").Length;
		m_numWalkerBlue = GameObject.FindGameObjectsWithTag("WalkerBlue").Length;
		m_numFoodLeft   = GameObject.FindGameObjectsWithTag("Food").Length;
		m_numHutBlue    = GameObject.FindGameObjectsWithTag("HutBlue").Length;
		m_numHutRed     = GameObject.FindGameObjectsWithTag("HutRed").Length;
		m_numBears      = GameObject.FindGameObjectsWithTag("Bear").Length;
		RefreshCameras();

	}
	
	void OnGUI()
	{
		GUI.Box(new Rect(0,0, 200, 165), "Energy");
		GUI.color = new Color(133, 0 ,0);
		GUI.HorizontalScrollbar( new Rect (10, 20, 180, 20), 0, World.me.m_energy.current, 0, World.me.m_energy.max);
		GUI.color = Color.white;
		GUI.Label( new Rect(10, 40, 200, 20), "Red Workers: " + m_numWalkerRed.ToString() );
		GUI.Label( new Rect(10, 60, 200, 20), "Red Huts: " + m_numHutRed.ToString() );
		GUI.Label( new Rect(10, 80, 200, 20), "Blue Workers: " + m_numWalkerBlue.ToString() );
		GUI.Label( new Rect(10, 100, 200, 20), "Blue Hut: " + m_numHutBlue.ToString() );
		GUI.Label( new Rect(10, 120, 200, 20), "Food Left: " + m_numFoodLeft.ToString() );
		GUI.Label( new Rect(10, 140, 200, 20), "Bears: " + m_numBears.ToString() );
		GUI.Label( new Rect(10, 160, 200, 20), "Energy: " + World.me.m_energy.current.ToString() );

	}
	
	public void RefreshCameras()
	{
		//Switch Cameras depending on state
		if( camBEAR != null && m_currentMode == Mode.BEAR && camBEAR.enabled == false)
		{
			ClearCameras();
			camBEAR.enabled = true;
			camBEAR.GetComponent<BearCam>().enabled = true;
			camBEAR.GetComponentInChildren<AudioListener>().enabled = true;
		}
		else if ( camCLOUD != null && m_currentMode == Mode.CLOUD && camCLOUD.enabled == false)
		{
			ClearCameras();
			camCLOUD.enabled = true;
			camCLOUD.GetComponent<CloudCam>().enabled = true;
			camCLOUD.GetComponentInChildren<AudioListener>().enabled = true;
		}		
		else if ( camGOD != null && m_currentMode == Mode.GOD && camGOD.enabled == false)
		{
			ClearCameras();
			camGOD.enabled = true;
			camGOD.GetComponent<RTSCam>().enabled = true;
			camGOD.GetComponentInChildren<AudioListener>().enabled = true;
		}
		
	}
	
	public Camera GetActiveCamera()
	{
		if(camBEAR.enabled == true)
		{
			return camBEAR;
		}
		if (camCLOUD.enabled == true)
		{
			return camCLOUD;
		}
		if (camGOD.enabled == true)
		{
			return camGOD;
		}
		return Camera.current;
	}
	
	public void ClearCameras()
	{
		Camera[] cameras = Camera.allCameras;
		
		foreach(Camera cam in cameras)
		{
			cam.enabled = false;
			cam.GetComponentInChildren<AudioListener>().enabled = false;
		}

		camBEAR.GetComponent<BearCam>().enabled = false;
		camCLOUD.GetComponent<CloudCam>().enabled = false;
		camGOD.GetComponent<RTSCam>().enabled = true;

	}
	
	int chunkIndex( int chX, int chY, int chZ )
	{
		return chZ * s_chunkSide + chX;
	}
	
	public void createWorld()
	{
		me = this;

		UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks;
		
		float noiseZ = UnityEngine.Random.Range( 0.0f, 10.0f );		
		
		print ( "Start world create" );
		for( int z = 0; z < s_chunkSide; ++z )
		{
			for( int x = 0; x < s_chunkSide; ++x )
			{
				int index = z * s_chunkSide + x;
				
				Vector3 chunkPos = new Vector3( (float)x * s_chunkWorldSize, 0, (float)z * s_chunkWorldSize );
				
				GameObject chunk = Instantiate( m_chunkObjectDef, chunkPos, new Quaternion() ) as GameObject;
				
				Chunk chunkScript = chunk.AddComponent<Chunk>();
				
				m_chunks[index] = chunk;
			}
		}
		for( int z = 0; z < s_chunkSide; ++z )
		{
			for( int x = 0; x < s_chunkSide; ++x )
			{
				int index = z * s_chunkSide + x;
				
				Chunk chunkScript = m_chunks[index].GetComponent<Chunk>();
				
				chunkScript.MakeNewChunk( this, x * (int)s_chunkWorldSize, 0, z * (int)s_chunkWorldSize );
				
				chunkScript.createRandomCubes( noiseZ );
				chunkScript.createGeo();
			}
		}
		print ( "End world create" );
	}
	
	public int worldToChunkIndex( float x, float y, float z )
	{
		if( x < 0 ) x += -s_chunkWorldSize;
		if( y < 0 ) y += -s_chunkWorldSize;
		if( z < 0 ) z += -s_chunkWorldSize;
		
		int chunkX = ((int)(x+0.5f)) / Chunk.s_chunkSize;
		int chunkY = ((int)(y+0.5f)) / Chunk.s_chunkSize;
		int chunkZ = ((int)(z+0.5f)) / Chunk.s_chunkSize;
		
		if( chunkX < 0 || chunkX >= s_chunkSide ) return 0;
		if( chunkZ < 0 || chunkZ >= s_chunkSide ) return 0;
		if( chunkY < 0 || chunkY >= 2 ) return 0;

		return chunkIndex( chunkX, chunkY, chunkZ );
	}
			
	public short getType( int x, int y, int z )
	{
		if( x < 0 ) x += -16;
		if( y < 0 ) y += -16;
		if( z < 0 ) z += -16;
		
		int chunkX = x / Chunk.s_chunkSize;
		int chunkY = y / Chunk.s_chunkSize;
		int chunkZ = z / Chunk.s_chunkSize;
		
		if( chunkX < 0 || chunkX >= s_chunkSide ) return 0;
		if( chunkZ < 0 || chunkZ >= s_chunkSide ) return 0;
		if( chunkY < 0 || chunkY >= 2 ) return 0;

		int index = chunkIndex( chunkX, chunkY, chunkZ );
		
		Chunk chScr = m_chunks[index].GetComponent<Chunk>();
		
		return chScr.getType_l( x % Chunk.s_chunkSize, y % Chunk.s_chunkSize, z % Chunk.s_chunkSize );
	}
			
	public void remBlock( int x, int y, int z )
	{
		if( x < 0 ) x += -16;
		if( y < 0 ) y += -16;
		if( z < 0 ) z += -16;
		
		int chunkX = x / Chunk.s_chunkSize;
		int chunkY = y / Chunk.s_chunkSize;
		int chunkZ = z / Chunk.s_chunkSize;
		
		if( chunkX < 0 || chunkX >= s_chunkSide ) return;
		if( chunkZ < 0 || chunkZ >= s_chunkSide ) return;
		if( chunkY < 0 || chunkY >= 2 ) return;

		int index = chunkIndex( chunkX, chunkY, chunkZ );
		
		Chunk chScr = m_chunks[index].GetComponent<Chunk>();
		
		chScr.remBlock_l( x % Chunk.s_chunkSize, y % Chunk.s_chunkSize, z % Chunk.s_chunkSize );
	}
			
	public void addBlock( short type, int x, int y, int z )
	{
		if( x < 0 ) x += -16;
		if( y < 0 ) y += -16;
		if( z < 0 ) z += -16;
		
		int chunkX = x / Chunk.s_chunkSize;
		int chunkY = y / Chunk.s_chunkSize;
		int chunkZ = z / Chunk.s_chunkSize;
		
		if( chunkX < 0 || chunkX >= s_chunkSide ) return;
		if( chunkZ < 0 || chunkZ >= s_chunkSide ) return;
		if( chunkY < 0 || chunkY >= 2 ) return;

		int index = chunkIndex( chunkX, chunkY, chunkZ );
		
		Chunk chScr = m_chunks[index].GetComponent<Chunk>();
		
		chScr.addBlock_l( type, x % Chunk.s_chunkSize, y % Chunk.s_chunkSize, z % Chunk.s_chunkSize );
	}

	public float getWorldHeight( float x, float z )
	{
		short dontCare;
		return getWorldHeight( x, z, out dontCare );
	}

	public float getWorldHeight( float x, float z, out short o_type )
	{	
		int index = worldToChunkIndex( x, 0, z );
		
		if( index < 0 || index > (s_chunkSide * s_chunkSide) ) 
		{
			o_type = 0;
			return 0.0f;
		}
		
		Chunk chScr = m_chunks[index].GetComponent<Chunk>();
		
		return chScr.getWorldHeight( (int)(x+0.5f) % Chunk.s_chunkSize, (int)(z+0.5f) % Chunk.s_chunkSize, out o_type );
	}
	
	
	
	public void scatterClouds( GameObject def, int count )
	{
		for( int i = 0; i < count; ++i )
		{
			float fx = UnityEngine.Random.Range( 0, s_chunkSide * s_chunkWorldSize );
			float fz = UnityEngine.Random.Range( 0, s_chunkSide * s_chunkWorldSize );
			
			float fy = UnityEngine.Random.Range( 18, 24 );
			
			Vector3 pos = new Vector3( fx, fy, fz );
			
			GameObject resource = Instantiate( def, pos, new Quaternion() ) as GameObject;
			
			
		}

	}
	
	public void scatterResource( GameObject def, int count, short type )
	{
		for( int i = 0; i < count; ++i )
		{
			float fx = UnityEngine.Random.Range( 0, s_chunkSide * s_chunkWorldSize );
			float fz = UnityEngine.Random.Range( 0, s_chunkSide * s_chunkWorldSize );
			
			short groundType;
			float fy = getWorldHeight( fx, fz, out groundType ) - 0.5f;
			
			if( type != -1 && type != groundType ) continue;
			
			if( fy < World.me.m_waterObj.transform.position.y ) continue;
			
			Vector3 pos = new Vector3( fx, fy, fz );
			
			GameObject resource = Instantiate( def, pos, new Quaternion() ) as GameObject;
			
			
		}

	}
	
	public void CheaterKeys()
	{
		if(Input.GetKey(KeyCode.Alpha1))
		{
			m_currentMode = Mode.BEAR;
		}
		
		if(Input.GetKey(KeyCode.Alpha2))
		{
			m_currentMode = Mode.GOD;
		}
		
		if(Input.GetKey(KeyCode.Alpha3))
		{
			m_currentMode = Mode.CLOUD;
		}
		
		if(Input.GetKey(KeyCode.Alpha9))
		{
			World.me.m_energy.add(500f);
		}
	}
	
	IEnumerator growGrass()
	{
		//yield return new WaitForSeconds(10);
		
		while( true )
		{
			
			
			yield return new WaitForSeconds( 0.1f );
		}
	}
	
	IEnumerator growBear()
	{
		//yield return new WaitForSeconds(10);
		
		while( true )
		{
			scatterResource( m_bearDef, 1, -1 );

			yield return new WaitForSeconds( 20 );
		}
	}
	
	IEnumerator growTrees()
	{
		while( true )
		{
			yield return new WaitForSeconds(1);

			scatterResource( m_treeDef, 1, 1 );
		}
	}
	
	IEnumerator growFood()
	{
		while( true )
		{
			yield return new WaitForSeconds(1);

			scatterResource( m_foodDef, 1, 1 );
		}
	}
	
}
