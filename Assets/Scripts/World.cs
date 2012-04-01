using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Chunk : MonoBehaviour
{
	static public float s_chunkHalf = 0.5f;
	static public int s_chunkSize = 16;
	
	short[] m_types = new short[s_chunkSize * s_chunkSize * s_chunkSize];
	
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
	
	public float getWorldHeight( int x, int z )
	{
		for( int y = 15; y >= 0; --y )
		{
			short type = getType_l( x, y, z );
			if( type != 0 ) return m_y + y + 1.0f;
		}
		
		return m_y;
	}
	
	public short getType( int x, int y, int z )
	{
		bool isOutside = (x < 0) | (y < 0) | (z < 0) | (x >= s_chunkSize) | (y >= s_chunkSize) | (z >= s_chunkSize);

		if( isOutside ) return m_world.getType( x + m_x, y + m_y, z + m_z );
		else return getType_l( x, y, z );
	}
	
	public void createRandomCubes()
	{
		for( int y = 0; y < 16; ++y )
		{
			float fy = (float)y;
			for( int z = 0; z < 16; ++z )
			{
				float fz = (float)z;
				for( int x = 0; x < 16; ++x )
				{
					float fx = (float)x;
					
					int index = localToIndex( x, y, z );
					
					float wx = fx + m_x;
					float wz = fz + m_z;
					
					float height = ( Mathf.Sin( wz / 17.0f ) + Mathf.Sin( wx / 23.0f ) ) * 2.0f + 4.0f;
					
					short val = (short)((height > fy) ? 1 : 0 );
					
					m_types[ index ] = val;
				}
			}
		}
	}
	
	int getVertIndex( Vector3 v, Vector2 u, List<Vector3> verts, List<Vector2> uvs, Dictionary<Vector3, int> map )
	{
		if( map.ContainsKey( v ) )
		{
			return map[v];
		}
		
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

		for( int y = 0; y < 16; ++y )
		{
			float fy = (float)y;
			float yhp = fy + 0.5f;
			float yhn = fy - 0.5f;

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
						
						Vector2 u0 = new Vector2( fx+0, fz+0 );
						Vector2 u1 = new Vector2( fx+1, fz+0 );
						Vector2 u2 = new Vector2( fx+1, fz+1 );
						Vector2 u3 = new Vector2( fx+0, fz+1 );
						
						createTriangle ( v0, v1, v3, u0, u1, u3, verts, uvs, map, indices );
						createTriangle ( v1, v2, v3, u1, u2, u3, verts, uvs, map, indices );						
					}
					
					if( yp == 0 )
					{
						Vector3 v0 = new Vector3( xhn, yhp, zhp );
						Vector3 v1 = new Vector3( xhp, yhp, zhp );
						Vector3 v2 = new Vector3( xhp, yhp, zhn );
						Vector3 v3 = new Vector3( xhn, yhp, zhn );
						
						Vector2 u0 = new Vector2( fx+0, fz+0 );
						Vector2 u1 = new Vector2( fx+1, fz+0 );
						Vector2 u2 = new Vector2( fx+1, fz+1 );
						Vector2 u3 = new Vector2( fx+0, fz+1 );
						
						createTriangle ( v0, v1, v3, u0, u1, u3, verts, uvs, map, indices );
						createTriangle ( v1, v2, v3, u1, u2, u3, verts, uvs, map, indices );						
					}
					
					if( zp == 0 )
					{
						Vector3 v0 = new Vector3( xhp, yhp, zhp );
						Vector3 v1 = new Vector3( xhn, yhp, zhp );
						Vector3 v2 = new Vector3( xhn, yhn, zhp );
						Vector3 v3 = new Vector3( xhp, yhn, zhp );
						
						Vector2 u0 = new Vector2( fx+0, fz+0 );
						Vector2 u1 = new Vector2( fx+1, fz+0 );
						Vector2 u2 = new Vector2( fx+1, fz+1 );
						Vector2 u3 = new Vector2( fx+0, fz+1 );
						
						createTriangle ( v0, v1, v3, u0, u1, u3, verts, uvs, map, indices );
						createTriangle ( v1, v2, v3, u1, u2, u3, verts, uvs, map, indices );						
					}
					
					if( xn == 0 )
					{
						Vector3 v0 = new Vector3( xhn, yhp, zhp );
						Vector3 v1 = new Vector3( xhn, yhp, zhn );
						Vector3 v2 = new Vector3( xhn, yhn, zhn );
						Vector3 v3 = new Vector3( xhn, yhn, zhp );
						
						Vector2 u0 = new Vector2( fx+0, fz+0 );
						Vector2 u1 = new Vector2( fx+1, fz+0 );
						Vector2 u2 = new Vector2( fx+1, fz+1 );
						Vector2 u3 = new Vector2( fx+0, fz+1 );
						
						createTriangle ( v0, v1, v3, u0, u1, u3, verts, uvs, map, indices );
						createTriangle ( v1, v2, v3, u1, u2, u3, verts, uvs, map, indices );						
					}
					
					if( yn == 0 )
					{
						Vector3 v0 = new Vector3( xhp, yhn, zhp );
						Vector3 v1 = new Vector3( xhn, yhn, zhp );
						Vector3 v2 = new Vector3( xhn, yhn, zhn );
						Vector3 v3 = new Vector3( xhp, yhn, zhn );
						
						Vector2 u0 = new Vector2( fx+0, fz+0 );
						Vector2 u1 = new Vector2( fx+1, fz+0 );
						Vector2 u2 = new Vector2( fx+1, fz+1 );
						Vector2 u3 = new Vector2( fx+0, fz+1 );
						
						createTriangle ( v0, v1, v3, u0, u1, u3, verts, uvs, map, indices );
						createTriangle ( v1, v2, v3, u1, u2, u3, verts, uvs, map, indices );						
					}
					
					if( zn == 0 )
					{
						Vector3 v0 = new Vector3( xhn, yhp, zhn );
						Vector3 v1 = new Vector3( xhp, yhp, zhn );
						Vector3 v2 = new Vector3( xhp, yhn, zhn );
						Vector3 v3 = new Vector3( xhn, yhn, zhn );
						
						Vector2 u0 = new Vector2( fx+0, fz+0 );
						Vector2 u1 = new Vector2( fx+1, fz+0 );
						Vector2 u2 = new Vector2( fx+1, fz+1 );
						Vector2 u3 = new Vector2( fx+0, fz+1 );
						
						createTriangle ( v0, v1, v3, u0, u1, u3, verts, uvs, map, indices );
						createTriangle ( v1, v2, v3, u1, u2, u3, verts, uvs, map, indices );						
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
		
	[System.SerializableAttribute]
	public class EnergyProperties
	{
		public float max;
		public float decayAmount;
		public float decayTime;
		public float lastDecay;

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
	

	GameObject[] m_chunks = new GameObject[ s_chunkSide * s_chunkSide ];
	
	public GameObject m_chunkObjectDef;
	public GameObject m_hutDef;
	public GameObject m_rockDef;
	public GameObject m_treeDef;
<<<<<<< HEAD
	#endregion
=======

	public float m_godRainEnergy = 25.0f;
	
	public GameObject m_guiEnergy;
	
>>>>>>> b77a772e33fa8543e1daf5a8cc672f67083c3bfd
	
	void Start () 
	{
		createWorld();
		
		float bX = 8.0f; 
		float bZ = 8.0f; 
		
		float bY = getWorldHeight( bX, bZ );
		
		Vector3 bPos = new Vector3( bX, bY, bZ );
		
		GameObject building = Instantiate( m_hutDef, bPos, new Quaternion() ) as GameObject;
		
		scatterResource( m_treeDef, 100 );
		scatterResource( m_rockDef, 100 );
	}
	
	void Update()
	{
<<<<<<< HEAD
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
					camCLOUD = cam;
				}
				
				else if(cam.name == "CameraCloud")
				{
					camGOD = cam;
				}
			}
		}
		
		//Switch Cameras depending on state
		if (m_currentMode == Mode.BEAR)
		{
			
		}
		
		else if (m_currentMode == Mode.CLOUD)
		{
		}
		
		else if (m_currentMode == Mode.GOD)
		{
		}
=======
		int energy = (int)m_energy.current;
		m_guiEnergy.GetComponent<GUIText>().text = energy.ToString();
>>>>>>> b77a772e33fa8543e1daf5a8cc672f67083c3bfd
	}
	
	int chunkIndex( int chX, int chY, int chZ )
	{
		return chZ * s_chunkSide + chX;
	}
	
	public void createWorld()
	{
		me = this;
		
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
				
				chunkScript.createRandomCubes();
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
		
		int chunkX = ((int)x) / Chunk.s_chunkSize;
		int chunkY = ((int)y) / Chunk.s_chunkSize;
		int chunkZ = ((int)z) / Chunk.s_chunkSize;
		
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

	public float getWorldHeight( float x, float z )
	{
		int index = worldToChunkIndex( x, 0, z );
		
		if( index < 0 || index > (s_chunkSide * s_chunkSide) ) return 0.0f;
		
		Chunk chScr = m_chunks[index].GetComponent<Chunk>();
		
		return chScr.getWorldHeight( (int)x % Chunk.s_chunkSize, (int)z % Chunk.s_chunkSize );
	}
	
	public void scatterResource( GameObject def, int count )
	{
		for( int i = 0; i < count; ++i )
		{
			float fx = Random.Range( 0, s_chunkSide * s_chunkWorldSize );
			float fz = Random.Range( 0, s_chunkSide * s_chunkWorldSize );
			
			float fy = getWorldHeight( fx, fz );
			
			Vector3 pos = new Vector3( fx, fy, fz );
			
			GameObject resource = Instantiate( def, pos, new Quaternion() ) as GameObject;
			
			
		}

	}
	
}
