using UnityEngine;
using System.Collections;

public class CSharpPlane : MonoBehaviour {

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
	public float scale = 30.0F;
	private Texture2D noiseTex;
	private Color[] pix;
	public float xCosDiv = 2.0F;


	private Vector3[] origVerts;

	void Start () {
	
		Debug.Log ("Hello Mesh");

		// You can change that line to provide another MeshFilter
		filter = gameObject.GetComponent<MeshFilter> ();
		mesh = filter.mesh;
		vertices = mesh.vertices;
		origVerts = vertices.Clone () as Vector3[];

		pixHeight = 64;
		pixWidth = 128;


		generateNoise ();



		mesh.RecalculateBounds();
		mesh.RecalculateNormals ();
		mesh.Optimize();

		//update the physics collider
		GetComponent<MeshCollider>().sharedMesh = null;
		GetComponent<MeshCollider>().sharedMesh = mesh;

//		gameObject.renderer.material.SetColor ("_Color", Color.gray);
	}


//	void onTriggerEnter(Collider other) {
//		
//		Debug.Log ("ON Trigger Enter NSOTEUH");
//	}
//

	// Update is called once per frame
	void Update () {
//		generateNoise ();
	}


	void generateNoise () {


//		noiseTex = new Texture2D (resZ,resX);
//		pix = new Color[resZ * resX];
//
//		for (float x = 0.0F; x < 128.0; x++) {
//			for (float y = 0.0F; y < 64.0F; y++) {
//				// Get a sample from the corresponding position in the noise plane
//				// and create a greyscale pixel from it.
//				var xCoord = xOrg + x / 128 * scale;
//				var yCoord = yOrg + y / 64 * scale;
//				var sample = Mathf.PerlinNoise(xCoord, yCoord);
//				var index = ((int) x * 64) + (int) y;
//				pix[index] = new Color(sample,sample,sample);
//			}
//		}
//
//		gameObject.renderer.material.SetTexture ("_MainTex", noiseTex);
//		noiseTex.SetPixels(pix);
//		noiseTex.Apply();



//perlin noise mesh
//		vertices = mesh.vertices;
//		int i = 0;
//
//		while (i < vertices.Length) {
//
//			Vector3 vert = vertices[i];
//			var xCoord = xOrg + vert.x / width * scale;
//			var yCoord = yOrg + vert.y / length * scale;
//			var sample = perlinHeight * Mathf.PerlinNoise(xCoord, yCoord);
//			vertices[i].z = sample;
//			Debug.Log(string.Format("X: {0} Z: {1} NOISE: {2}", vert.x, vert.y, sample));
//			i++;
//		}
//
//		mesh.vertices = vertices;

//		mesh.RecalculateBounds ();
//		mesh.Optimize ();
//		mesh.RecalculateNormals ();

//		Debug.Log ("generating noise!");

		Vector3[] newVerts = origVerts.Clone () as Vector3[];
		int i = 0;
		
		while (i < newVerts.Length) {
			
			Vector3 vert = newVerts[i];

			float ratio = (vert.x + 10.0F)/20.0F;

			var ex = Mathf.Cos( ratio *  2.0F * Mathf.PI);
			var why = Mathf.Sin( ratio * 2.0F *  Mathf.PI );


			var xCoord = xOrg + vert.x / width * scale;
			var yCoord = yOrg + vert.y / length * scale;
			var sample = Mathf.PerlinNoise(xCoord, yCoord);

			newVerts[i].x = (ex * radiusX) - (sample * perlinHeightX);
			newVerts[i].z = (why * radiusY) - (sample * perlinHeightZ);
//			Debug.Log(string.Format("X: {0} Z: {1} NOISE: {2}", vert.x, vert.y, sample));
			i++;
		}
		
		mesh.vertices = newVerts;

	}



}
