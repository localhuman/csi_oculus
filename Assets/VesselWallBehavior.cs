using UnityEngine;
using System.Collections;

public class VesselWallBehavior : MonoBehaviour {
	
	//mesh vars
	
	public Vector3[] vertices;
	public MeshFilter filter;
	public Mesh mesh;
	
	//mesh width/length 
	public int length = 64;
	public int width = 128;
	
	public int resX = 128; // 2 minimum
	public int resZ = 64;
	
	public float radiusX = 10.0F;
	public float radiusY = 60.0F;
	
	//noise vars
	public int pixHeight;
	public int pixWidth;
	public float xOrg;
	public float yOrg;
	public float perlinHeightX = 30.0F;
	public float perlinHeightZ = 40.0F;

	public float perlinScaleX = 30.0F;
	public float perlinScaleZ = 60.0F;


	public float xCosDiv = 2.0F;
	
	
	private Vector3[] origVerts;
	
	void Start () {
		
		
		// You can change that line to provide another MeshFilter
		filter = gameObject.GetComponent<MeshFilter> ();
		mesh = filter.mesh;
		vertices = mesh.vertices;
		origVerts = vertices.Clone () as Vector3[];
		
		pixHeight = 64;
		pixWidth = 128;
		
		generateWall ();
		
		mesh.RecalculateBounds();
		mesh.RecalculateNormals ();
		mesh.Optimize();
		
//		renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		//		generateNoise ();
		
//		generateWall();
//		mesh.RecalculateBounds();
//		mesh.RecalculateNormals ();
	}
	
	
	void generateWall () {
		
		//deforming mesh with perlin noise to simpulate calcium buildup

		Vector3[] newVerts = origVerts.Clone () as Vector3[];
		int i = 0;
		
		while (i < newVerts.Length) {
			
			Vector3 vert = newVerts[i];
			
			float ratio = (vert.x + 10.0F)/20.0F;
			
			var ex = Mathf.Cos( ratio *  2.0F * Mathf.PI);
			var why = Mathf.Sin( ratio * 2.0F *  Mathf.PI );
			
			
			var xCoord = xOrg + vert.x / width * perlinScaleX;
			var yCoord = yOrg + vert.y / length * perlinScaleZ;
			var sample = Mathf.PerlinNoise(xCoord, yCoord) - 0.5F;
			
			newVerts[i].x = (ex * radiusX) - (sample * perlinHeightX);
			newVerts[i].z = (why * radiusY) - (sample * perlinHeightZ);
			i++;
		}
		
		mesh.vertices = newVerts;

	}
	
	
	
}
