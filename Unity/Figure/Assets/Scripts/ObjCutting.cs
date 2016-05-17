using System.Collections.Generic;
using UnityEngine;

public class ObjCutting : MonoBehaviour {

	[SerializeField]
	private GameObject simpleCube;
	private Camera mainCamera;

	private MeshFilter mf;
	private bool isCut = false;

	private Vector3 tapPoint;
	private Vector3[] cubeVerPos;
	private List<Vector3> baseVerPos = new List<Vector3>();
	private List<Vector3> cutPointArray = new List<Vector3>();
	private List<Transform> cutTransArray = new List<Transform>();
    private List<GameObject> screenPointOBJ = new List<GameObject>();

	private Plane plane;

    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;

        }

        if(simpleCube == null)
        {
            simpleCube = GameObject.Find("simpleCube");


        }

    }

	void Start () {
		var CubeVertices = new GameObject("CubeVertices");
		mf = simpleCube.GetComponent<MeshFilter>();
		cubeVerPos = mf.mesh.vertices;

		foreach (var v in cubeVerPos)
		{
			if (!baseVerPos.Contains(v))
			{
				baseVerPos.Add(v);

			}

		}

		// Debug ===============================================================================
		//for (var i = 0; i < baseVerPos.Count; i++)
		//{
		//	GameObject verSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		//	verSphere.transform.position = new Vector3(baseVerPos[i].x, baseVerPos[i].y, baseVerPos[i].z);
		//	verSphere.transform.localScale = new Vector3(10, 10, 10);
		//	verSphere.name = "CubeVertice[" + i + "]";
		//	verSphere.transform.parent = CubeVertices.transform;
		//	Debug.Log(baseVerPos[i]);

		//}
		// ======================================================================================

	}

	void Update () {
		if (Input.GetMouseButtonUp(0))
		{

			Vector3 mousePos = Input.mousePosition;
			Ray ray = Camera.main.ScreenPointToRay(mousePos);
			RaycastHit hit = new RaycastHit();

			if (Physics.Raycast(ray, out hit, 100000) && cutPointArray.Count < 3)
			{
				tapPoint = hit.point;

				GameObject cutPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				cutPoint.transform.position = tapPoint;
				cutPoint.transform.parent = hit.transform;
				cutPoint.transform.localRotation = new Quaternion(0, 0, 0, 0);
				cutPoint.transform.localPosition = new Vector3(0, cutPoint.transform.localPosition.y, 0);
				cutPoint.transform.localScale = new Vector3(10, 10, 10);

                cutPointArray.Add(cutPoint.transform.position);
				cutTransArray.Add(cutPoint.transform);
				Destroy(hit.collider);

				if (cutPointArray.Count == 3)
				{
					//screenPoint.Add(mainCamera.WorldToScreenPoint(cutPointArray[0]));
					//screenPoint.Add(mainCamera.WorldToScreenPoint(cutPointArray[1]));	


					isCut = true;

					Vector3 A = cutPointArray[0];
					Vector3 B = cutPointArray[1];
					Vector3 C = cutPointArray[2];

					Vector3 AB = new Vector3(B.x - A.x, B.y - A.y, B.z - A.z);
					Vector3 AC = new Vector3(C.x - A.x, C.y - A.y, C.z - A.z);

					Vector3 ABxAC = new Vector3(AB.y * AC.z - AC.y * AB.z,
												AB.z * AC.x - AC.z * AB.x,
												AB.x * AC.y - AC.x * AB.y);


					Vector3 P = new Vector3(ABxAC.x, ABxAC.y, ABxAC.z);



					Debug.Log(P.x + "x " + P.y + "y " + P.z + "z = " + (P.x + P.y + P.z));



					// ax + by + cz + d = 0

					//float f = a * cutPointArray[0].x + b * cutPointArray[0].y + c * cutPointArray[0].z;

					//Debug.Log("f = " + f);

					//Debug.Log("a+b+c = " + (a + b + c));
					//Debug.Log("a+b+c+d = " + (a + b + c + d));


                }

			}
		}
        if (isCut)
        {
            //RaycastHit hit = new RaycastHit();
            //if (Physics.Linecast(screenPoint[0], screenPoint[1], out hit))
            //{
            //    Debug.Log(hit.collider);

            //}

        }

    }

}



