using UnityEngine;
using System.Collections;

public class CalciumBehavior : MonoBehaviour {

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

		generateNoise ();

		mesh.RecalculateBounds();
		mesh.RecalculateNormals ();
		mesh.Optimize();

		//update the physics collider
		GetComponent<MeshCollider>().sharedMesh = null;
		GetComponent<MeshCollider>().sharedMesh = mesh;

	}

	// Update is called once per frame
	void Update () {
//		generateNoise ();

		if (Input.GetKey (KeyCode.Alpha8)) {
			generateNoise();
			mesh.RecalculateBounds();
			mesh.RecalculateNormals ();
			mesh.Optimize();
			
			//update the physics collider
			GetComponent<MeshCollider>().sharedMesh = null;
			GetComponent<MeshCollider>().sharedMesh = mesh;

		}
	}


	void generateNoise () {

		//deforming mesh with perlin noise to simpulate calcium buildup

		Debug.Log ("Generating noise!");

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
			i++;
		}
		
		mesh.vertices = newVerts;



//		noiseTex = new Texture2D (resZ,resX);
//		pix = new Color[resZ * resX];
//		Debug.Log ("pix length:");
//		Debug.Log (pix.Length);
//
//
//		Material vesselMat = gameObject.renderer.materials[1];
//		vesselTexture = vesselMat.mainTexture as Texture2D;
//		newVesselTexture = new Texture2D (1024, 512, TextureFormat.ARGB32, false);
//		vesselPixels = vesselTexture.GetPixels32 ();
//		Debug.Log ("Current Pixels Length:");
//		Debug.Log (vesselPixels.Length);
//		Color32[] newPixels = vesselPixels.Clone() as Color32[];
//		Color32 currentPixel;
//
//		for (float x = 0.0F; x < 1024; x++) {
//			for (float y = 0.0F; y < 512; y++) {
//				// Get a sample from the corresponding position in the noise plane
//				// and create a greyscale pixel from it.
//				var xCoord = xOrg + x / 1024.0F * scale2;
//				var yCoord = yOrg + y / 512.0F * scale2;
//				float sample = Mathf.PerlinNoise(xCoord, yCoord);
//
//				if( sample > .5F) {
//					sample = 0.0F;
//				} else {
//					sample = 1.0F;
//				}
//
//				var index = ((int) x * 512) + (int) y;
//
//				currentPixel = vesselPixels[index]; 
//				int alpha = Mathf.RoundToInt(sample * 255);
//				byte alphabit = (byte)alpha;
//				currentPixel.a = alphabit;
//		
//		//			newPixel = new Color( currentPixel.r, currentPixel.g, currentPixel.b, sample);
//		//			newPixels[i] = newPixel;
//		//			Debug.Log(string.Format("X: {0} Z: {1} NOISE: {2}", vert.x, vert.y, sample));
//				newPixels[index] = currentPixel;
//
//			}
//		}
//
//		Debug.Log("New Pixels Length:");
//		Debug.Log (newPixels.Length);
//		newVesselTexture.SetPixels32 (newPixels);
//		newVesselTexture.Apply ();
//		
//		gameObject.renderer.materials[1].mainTexture = newVesselTexture;

//
//		gameObject.renderer.material.SetTexture ("_MainTex", noiseTex);
//		noiseTex.SetPixels(pix);
//		noiseTex.Apply();

	}



}
