using UnityEngine;
using System.Collections;

public class CrownScript : MonoBehaviour {



	public float centerX = 10.0F;
	public float centerY = 10.0F;

	public float rotateRadius = 40.0F;
	
	private float radians = 0.0F;

	public float radianIncrement = 0.05F;

	private float twoPi = Mathf.PI * 2.0F;

	private GameObject player;

	private GameObject wireFront;
	private Mesh wireFrontMesh;
	private Vector3[] wireFrontOrigVerts;

	private GameObject wireRear;
	private Mesh wireRearMesh;
	private Vector3[] wireRearOrigVerts;

	private float origCapsuleZ;

	public float zOffset = 0.0F;
	public float zOffsetIncrement = 0.1F;

	// Use this for initialization
	void Start () {

		wireFront = GameObject.Find ("WireFront");
		wireRear = GameObject.Find ("WireRear");

		player = GameObject.Find ("OVRPlayerController");

//		wireFront.renderer.material.SetColor ("_Color", Color.red);
//		wireRear.renderer.material.SetColor ("_Color", Color.green);

		wireFrontMesh = wireFront.GetComponent<MeshFilter> ().mesh;
		wireFrontOrigVerts = wireFrontMesh.vertices.Clone () as Vector3[];

		wireRearMesh = wireRear.GetComponent<MeshFilter> ().mesh;
		wireRearOrigVerts = wireRearMesh.vertices.Clone () as Vector3[];

		origCapsuleZ = gameObject.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {

		//capture key input to move device
		if( Input.GetKey(KeyCode.Alpha1)) {
			zOffset += zOffsetIncrement;
		}
		if( Input.GetKey(KeyCode.Alpha2)) {
			zOffset -= zOffsetIncrement;
		}

		if( Input.GetKey(KeyCode.Alpha3)) {
			rotateRadius+=.2F;
		}
		if( Input.GetKey(KeyCode.Alpha4)) {
			rotateRadius-=.2F;
		}

		if( Input.GetKey(KeyCode.Alpha5)) {
			radianIncrement+= .01F;
		}
		if( Input.GetKey(KeyCode.Alpha6)) {
			radianIncrement-= .01F;
		}

		if( Input.GetKey(KeyCode.Alpha7)) {
			player.transform.position = new Vector3(0.0F, 0.0F, 0.0F);				
		}

		//move crown
		float newX = Mathf.Cos (radians);
		float newY = Mathf.Sin (radians);

		Vector3 currentPosition = gameObject.transform.position;
		currentPosition.x = (newX * rotateRadius) + centerX;
		currentPosition.y = (newY * rotateRadius) + centerY;
		currentPosition.z = origCapsuleZ + (zOffset*56.0F);
		gameObject.transform.position = currentPosition;



		//move wire front
		Vector3[] newVerts = wireFrontOrigVerts.Clone () as Vector3[];
		float percentMotion;
		float newWireX;
		float newWireY;
		int count = 0;
		int vertLength = newVerts.Length;
		Vector3 vert;
		while( count < vertLength ) {

			vert = newVerts[count];
			percentMotion = (vert.z + 6.0F) / 8.0F;
			newWireX = centerX + vert.x + (newX * rotateRadius * percentMotion);
			newWireY = centerY + vert.y + (newY * rotateRadius * percentMotion);

			newVerts[count].x = newWireX;
			newVerts[count].y = newWireY;
			newVerts[count].z = vert.z + zOffset;
			count++;
		}

		wireFrontMesh.vertices = newVerts;


		//move wire rear
		newVerts = wireRearOrigVerts.Clone () as Vector3[];
		count = 0;
		vertLength = newVerts.Length;

		while( count < vertLength ) {			
			vert = newVerts[count];
			percentMotion = (vert.z + 6.0F) / 8.0F;
			newWireX = -centerX + vert.x + (-newX * rotateRadius * percentMotion);
			newWireY = centerY + vert.y + (newY * rotateRadius * percentMotion);
			
			newVerts[count].x = newWireX;
			newVerts[count].y = newWireY;
			newVerts[count].z = vert.z - (zOffset/3.0F);
			count++;
		}
		
		wireRearMesh.vertices = newVerts;


		//increase radians!
		if (radians < twoPi) {

			radians += radianIncrement;

		} else {
			radians=0.0F;
		}

	}

//	void onTriggerEnter(Collider other) {
//
//		Debug.Log ("ON Trigger Enter crown");
//	}
}
