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


	private LineRenderer wireFrontLine;
	private LineRenderer wireRearLine;

	private int wireNumSegments = 40;
	private float wireSegmentLength = 40.0F;

	private float wireFrontStart = 30.0F;
	private float wireRearStart = 40.0F;



	private float origCapsuleZ;

	public float zOffset = 0.0F;
	public float zOffsetIncrement = 0.1F;

	// Use this for initialization
	void Start () {



		wireFrontLine = GameObject.Find ("WireFrontLine").GetComponent<LineRenderer>();

		wireRearLine = GameObject.Find ("WireRearLine").GetComponent<LineRenderer>();

		initWireLines();

		player = GameObject.Find ("OVRPlayerController");

//		wireFront.renderer.material.SetColor ("_Color", Color.red);
//		wireRear.renderer.material.SetColor ("_Color", Color.green);

//		wireFrontMesh = wireFront.GetComponent<MeshFilter> ().mesh;
//		wireFrontOrigVerts = wireFrontMesh.vertices.Clone () as Vector3[];
//
//		wireRearMesh = wireRear.GetComponent<MeshFilter> ().mesh;
//		wireRearOrigVerts = wireRearMesh.vertices.Clone () as Vector3[];

		origCapsuleZ = gameObject.transform.position.z;
	}

	void initWireLines() {

		//front line
		int i = 0;
		float z = wireFrontStart;
		wireFrontLine.SetVertexCount (wireNumSegments);

		while (i < wireNumSegments) {

			Vector3 newPos = new Vector3(0.0F,0.0F, z);
			wireFrontLine.SetPosition(i, newPos);
			z -= wireSegmentLength;
			i++;
		}

		//rear line
		i = 0;
		z = wireRearStart;
		wireRearLine.SetVertexCount (wireNumSegments);

		while (i < wireNumSegments) {
			
			Vector3 newPos = new Vector3(0.0F,0.0F, z);
			wireRearLine.SetPosition(i, newPos);
			z += wireSegmentLength;
			i++;
		}

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
		int i = 0;
		float z = wireFrontStart;
		float percentDone = 1.0F;
		float percentDecrement = 1.0F / wireNumSegments;
		float newWireX;
		float newWireY;
		while (i < wireNumSegments) {

			newWireX = centerX + (newX * rotateRadius * percentDone);
			newWireY = centerY + (newY * rotateRadius * percentDone);
			Vector3 newPos = new Vector3(newWireX, newWireY, z);
			wireFrontLine.SetPosition(i, newPos);
			z -= wireSegmentLength;
			percentDone -= percentDecrement;
			i++;
		}

		//move wire rear
		i = 0;
		z = wireRearStart;
		percentDone = 1.0F;
		while (i < wireNumSegments) {
			
			newWireX = centerX + (newX * rotateRadius * percentDone);
			newWireY = centerY + (newY * rotateRadius * percentDone);
			Vector3 newPos = new Vector3(newWireX, newWireY, z);
			wireRearLine.SetPosition(i, newPos);
			z += wireSegmentLength;
			percentDone -= percentDecrement;
			i++;
		}


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
