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

	public float radiusX = 50.0F;
	public float radiusY = 50.0F;

	//noise vars
	public float xOrg;
	public float yOrg;
	public float perlinHeightX = 40.0F;
	public float perlinHeightZ = 40.0F;
	public float perlinScaleX = 40.0F;
	public float perlinScaleZ = 80.0F;

	public float pheightModifier = 0.03F;
	public float pheightModifier2 = 0.1F;
	public float pheightModifier3 = 0.005F;

	public float minPHeight = 32.0F;


	public float rheightModifier = 0.1F;
	public float rheightModifier2 = 0.04F;
	public float rheightModifier3 = 0.01F;

	public float crownRadiusOffset = -14.0F;

	public float maxRHeight = 57.0F;

	public float xCosDiv = 2.0F;


	private Vector3[] origVerts;


	public CrownBehavior crown;

	public float[] perlinHeights;
	public float[] radiiHeights;

	private int crownZIndex=64;

	void Start () {
	
		int i;
		// You can change that line to provide another MeshFilter
		filter = gameObject.GetComponent<MeshFilter> ();
		mesh = filter.mesh;
		vertices = mesh.vertices;
		origVerts = vertices.Clone () as Vector3[];

		//initialize perlin height array
		perlinHeights = new float[resX];
		float startPerlinHeight = minPHeight;
		for(i =0; i< resX; i++) {
			perlinHeights[i] = startPerlinHeight;
			if( i > 27 && startPerlinHeight < perlinHeightX ) {
				startPerlinHeight+=1.0F;
			}
		}

		//initialize radius height array
		radiiHeights = new float[resX];
		float startRadiiHeight = maxRHeight;
		for (i=0; i < resX; i++) {
			radiiHeights[i] = startRadiiHeight;

			if( i > 27  && startRadiiHeight > radiusX) {
				startRadiiHeight -=1.0F;
			}
		}

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


		crownZIndex = Mathf.FloorToInt((crown.zOffset + 25.0F) / 0.39F );
		if (crownZIndex < 0) crownZIndex = 0;
		if (crownZIndex > 127) crownZIndex = 127;

		generateNoise();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals ();
//		mesh.Optimize();
		
		//update the physics collider
//		GetComponent<MeshCollider>().sharedMesh = null;
//		GetComponent<MeshCollider>().sharedMesh = mesh;
	}


	void generateNoise () {

		//modify perlin heights based on crown z position
		//also modify radius height
		float pheightCenter = perlinHeights [crownZIndex];
		int j;
		float perlinProgress;
		float radiusProgress;
		float totalRadiusMovement = maxRHeight - radiusX;
		float beforeRadiusProg = 1.0F;
		float afterRadiusProg = 1.0F;

		for (j =0; j< resX; j++) {
			if( j == crownZIndex ) {


				if( perlinHeights[j] > minPHeight) {
					perlinHeights[j]-= pheightModifier;
				}

				if( radiiHeights[j] < maxRHeight ) {
					radiiHeights[j] += rheightModifier;
				}
				crown.rotateRadius = radiiHeights[j] + crownRadiusOffset;


			}

			if( j == crownZIndex - 1 ) {
				if( perlinHeights[j] > minPHeight) {
					perlinHeights[j]-= pheightModifier2;
				}

				if( radiiHeights[j] < maxRHeight ) {
					radiiHeights[j] += rheightModifier2;
				}

				beforeRadiusProg = (radiiHeights[j] - radiusX) / totalRadiusMovement;
				

			}

//			if( j == crownZIndex - 2 ) {
//				if( perlinHeights[j] > minPHeight) {
//					perlinHeights[j]-= pheightModifier3;
//				}
//				if( radiiHeights[j] < maxRHeight ) {
//					radiiHeights[j] += rheightModifier3;
//				}
//			}

			if( j == crownZIndex + 1 ) {
				if( perlinHeights[j] > minPHeight) {
					perlinHeights[j]-= pheightModifier2;
				}
				if( radiiHeights[j] < maxRHeight ) {
					radiiHeights[j] += rheightModifier2;
				}
				afterRadiusProg = (radiiHeights[j] - radiusX) / totalRadiusMovement;
			}

//			if( j == crownZIndex + 2 ) {
//				if( perlinHeights[j] > minPHeight) {
//					perlinHeights[j]-= pheightModifier3;
//				}
//				if( radiiHeights[j] < maxRHeight ) {
//					radiiHeights[j] += rheightModifier3;
//				}
//			}

			if( beforeRadiusProg < 1.0F || afterRadiusProg < 1.0F ) {
				crown.pHolder.renderer.enabled=true;
			} else {
				crown.pHolder.renderer.enabled=false;
			}
		}



		//deforming mesh with perlin noise to simpulate calcium buildup

		Vector3[] newVerts = origVerts.Clone () as Vector3[];
		int i = 0;

		float lasty = 0.0F;
		int yIndex = 0;
		bool start = true;

		Vector3 vert;
		float ratio;
		float ex;
		float zee;

		float xCoord;
		float zCoord;

		float sample;
		float newPerlinHeight;
		float newRadiusHeight;

		while (i < newVerts.Length) {

			vert = newVerts[i];

			if( start ){
				start=false;
				lasty = vert.y;
			}

			ratio = (vert.x + 10.0F)/20.0F;
			
			ex = Mathf.Cos( ratio *  2.0F * Mathf.PI);
			zee = Mathf.Sin( ratio * 2.0F *  Mathf.PI );


			xCoord = xOrg + vert.x / width * perlinScaleX;
			zCoord = yOrg + vert.y / length * perlinScaleZ;

			sample = Mathf.PerlinNoise(xCoord, zCoord) - 0.5F;

			newPerlinHeight = perlinHeights[yIndex];
			newRadiusHeight = radiiHeights[yIndex];

			newVerts[i].x = (ex * newRadiusHeight) - (sample * newPerlinHeight);
			newVerts[i].z = (zee * newRadiusHeight) - (sample * newPerlinHeight);
			i++;

			if( vert.y != lasty ) {
				lasty = vert.y;
				yIndex++;
			}

		}
		mesh.vertices = newVerts;

	}



}
