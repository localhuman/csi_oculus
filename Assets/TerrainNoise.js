#pragma strict

var width:int=50;
var depth:int=50;

var xOrg:float;
var zOrg:float;
var yOrg:float;

var scale:float = 10.0;

var noiseTex:Texture2D;
var pix:Color[];

var mesh:Mesh;
var verts:Vector3[];
var newVerts:Array;

var cosXDivider:float = 2.0;
var yHeightModifier:float = 20.0;

var hasCalculatedNoise:int=0;

var customMesh:Mesh;
var threeDVerts:Vector3[];

function Start () {;
	Debug.Log('unity start terrain');
	

	
	width=50;
	depth=50;
	scale = 10.0;
	cosXDivider = 2.0;
	yHeightModifier = 20.0;
	xOrg=0.0;
	yOrg=0.0;	

	noiseTex = new Texture2D(width,depth);
	pix = new Color[width*depth];

//	mesh = GetComponent(MeshFilter).mesh;
//	verts = mesh.vertices;
	newVerts = new Array();
			
//	renderer.material.mainTexture = noiseTex;	
//	gameObject.renderer.material.mainTexture = noiseTex;		
	var s = String.Format('Game object: {0}', gameObject);
	Debug.Log(s);

	Debug.Log('hello??');
		
	createCustomMesh();
}

function createCustomMesh(){

	var vx:int = 2;
	var vy:int = 4;
	var vxstart:int=-1;
	var vystart:int=-2;
	
	var v_inc:int = 1;
	
	
    var twoDVerts:Vector2[] = new Vector2[ (vx+1) * (vy+1) ];
	threeDVerts = new Vector3[ (vx+1)* (vy+1) ];
		
//	var twoDVerts:Array = new Array();
//	var threeDVerts:Array = new Array();

	var count:int=0;	
	for( var i:int = vxstart; i <=vx+vxstart; i+=1) {
		for( var j:int=vystart; j <=vy+vystart; j+=1) {

//			twoDVerts.A
			twoDVerts[count]= new Vector2( i, j);
			threeDVerts[count] = new Vector3(i, j, 0);													
			count++;
		}
	}
	
//	var tr:Triangulator = gameObject.AddComponent( Triangulator );
//	tr.initTriangulator( twoDVerts );
//	var indices:int[] = tr.Triangulate();
	
	
//	customMesh = GetComponent(MeshFilter).mesh;
//	customMesh.Clear();
//	customMesh.vertices = threeDVerts; 
////	customMesh.uv = new Vector2[vx*vy];
//	customMesh.triangles = indices;
//	customMesh.RecalculateNormals();
//	customMesh.RecalculateBounds();
//	
//	mesh = customMesh;
//	verts = mesh.vertices;
//	gameObject.AddComponent( typeof(MeshRenderer) );
//	var filter:MeshFilter = gameObject.AddComponent( typeof( MeshFilter ) ) as MeshFilter;
//	filter.mesh = customMesh;
//	customMesh.
}

function Update () {

	if( hasCalculatedNoise < 1) {		
	
		hasCalculatedNoise=1;
		// For each pixel in the texture...
		
		for (var x = 0.0; x < width; x++) {
			for (var y = 0.0; y < depth; y++) {
				// Get a sample from the corresponding position in the noise plane
				// and create a greyscale pixel from it.
				var xCoord = xOrg + x / width * scale;
				var yCoord = yOrg + y / depth * scale;
				var sample = Mathf.PerlinNoise(xCoord, yCoord);

				pix[x * noiseTex.width + y] = new Color(sample,sample,sample);
				
			}
		}
		
					
							
	//		Debug.Log( String.Format('new verts length: {0}', newVerts.length));
	//		Debug.Log( String.Format('pix length: {0}', pix.length));

	//		Debug.Log('created noise pixels');
	// Copy the pixel data to the texture and load it into the GPU.
		gameObject.renderer.material.SetTexture('_MainTex',noiseTex);

		noiseTex.SetPixels(pix);
		noiseTex.Apply();
			
		xOrg+=0.02;
		yOrg+=0.01;		
	}			

	
//	Debug.Log(cosXDivider.ToString());
//	verts = mesh.vertices;
//	for each(vert in verts) {
//			Debug.Log( String.Format("X: {0} Y:{1} Z:{2}", vert.x, vert.y, vert.z));
//		var vertXDivided:float = vert.x / cosXDivider;
//		var newCosX = Mathf.Cos(vertXDivided);
//		var newVertY:float = newCosX * yHeightModifier;
//		newVerts.Push( new Vector3(vert.x, newVertY, vert.z));
//		vert.y = newVertY;
//	}
	
//	mesh.Clear();
//	mesh.vertices = verts;;
//	mesh.RecalculateBounds();
//	mesh.RecalculateNormals();
}

