using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class CrownBehavior : MonoBehaviour {



	public float centerX = 10.0F;
	public float centerY = 10.0F;

	public float rotateRadius = 40.0F;
	
	private float radians = 0.0F;

	public float radianIncrement = 0.25F;

	private float twoPi = Mathf.PI * 2.0F;

	private GameObject player;


	private LineRenderer wireFrontLine;
	private LineRenderer wireRearLine;

	private int wireNumSegments = 100;
	private float wireSegmentLength = 16.0F;

	private float wireFrontStart = 30.0F;
	private float wireRearStart = 40.0F;



	private float origCapsuleZ;
	
	public float zOffset = 0.0F;
	public float zOffsetIncrement = 0.1F;


	public float playerOffsetX = 10.0F;
	public float playerOffsetY = 12.0F;
	public float playerOffsetZ = 140.0F;

	public GameObject pHolder;


	private SerialPort stream;
	private int arduinoSpeed = 1;
	private int arduinoCrownLocation = 0;


	// Use this for initialization
	void Start () {

		stream = new SerialPort ("/dev/tty.usbmodem1421", 9600);
		stream.Open ();


		wireFrontLine = GameObject.Find ("WireFrontLine").GetComponent<LineRenderer>();

		wireRearLine = GameObject.Find ("WireRearLine").GetComponent<LineRenderer>();

		initWireLines();

		player = GameObject.Find ("OVRPlayerController");

//		pHolder = GameObject.Find ("pHolder");
		pHolder.GetComponent<Renderer> ().enabled = true;

		Debug.Log ("PHOLDER: ", pHolder);

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

		try {

			string items = stream.ReadLine ();
			int ison = int.Parse(items.Split (',')[0]);
			int speend = int.Parse(items.Split (',')[1]);
			int crownLocation = int.Parse (items.Split (',') [2]);


			if( ison < 1 ) {

				radianIncrement=0.0f;

			} else {


				if (speend != arduinoSpeed) {
					arduinoSpeed = speend;
					
					switch( arduinoSpeed ) {
					case 1:
						radianIncrement = 0.25f;
						break;
					case 2:
						radianIncrement = 0.30f;
						break;
						
					case 3:
						radianIncrement = 0.35f;
						break;
						
					default:
						radianIncrement = 0.25f;
						break;
					}
				}


			}


			arduinoCrownLocation = crownLocation;

			float nCrownZ = ( arduinoCrownLocation * 2.4F ) - 1200.0F; 
//			nCrownZ-=20.0F;
			zOffset = nCrownZ;
			Debug.Log(zOffset);

		} catch( System.Exception e) {
			Debug.Log("Exception reading stream");
		}


		//move crown
		float newX = Mathf.Cos (radians);
		float newY = Mathf.Sin (radians);

		Vector3 currentPosition = gameObject.transform.position;
		currentPosition.x = (newX * rotateRadius) + centerX;
		currentPosition.y = (newY * rotateRadius) + centerY;
		currentPosition.z = zOffset;
		gameObject.transform.position = currentPosition;


		//rotate particles
		float angle = Mathf.Atan2 (currentPosition.y, currentPosition.x);
		angle = ((angle * 180.0F) / Mathf.PI) + 360.0F;		
		Quaternion currentRot = pHolder.transform.rotation;
		currentRot.y = angle;
		pHolder.transform.rotation = currentRot;

		Vector3 playerPos = player.transform.position;
		playerPos.y = playerOffsetY;
		playerPos.x = playerOffsetX;
		float targetZ = currentPosition.z - playerOffsetZ;
		playerPos.z = (playerPos.z + targetZ)/2.0F;
		player.transform.position = playerPos;


		//move wire front
		int i = 0;
		float z = wireFrontStart;
		float percentDone = 1.0F;
		float percentDecrement = 1.0F / wireNumSegments;
		float newWireX;
		float newWireY;
		float newWireZ;

		float zRadians;
		float zCos;
		while (i < wireNumSegments) {


			zRadians = percentDone * twoPi;
			zCos = Mathf.Cos(zRadians);
			newWireX = centerX + (newX * rotateRadius * percentDone * zCos);
			newWireY = centerY + (newY * rotateRadius * percentDone * zCos);
			newWireZ = z + (zOffset * 56.0F);
			Vector3 newPos = new Vector3(newWireX, newWireY, newWireZ);
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

			zRadians = percentDone * twoPi;
			zCos = Mathf.Cos(zRadians);
			newWireX = centerX + (newX * rotateRadius * percentDone * zCos);
			newWireY = centerY + (newY * rotateRadius * percentDone * zCos);

			newWireX = centerX + (newX * rotateRadius * percentDone);
			newWireY = centerY + (newY * rotateRadius * percentDone);
			newWireZ = z + (zOffset * 56.0F);

			Vector3 newPos = new Vector3(newWireX, newWireY, newWireZ);
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


}
