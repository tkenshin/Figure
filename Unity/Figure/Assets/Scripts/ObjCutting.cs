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
	private List<Vector3> screenPoint = new List<Vector3>();
    private List<GameObject> screenPointOBJ = new List<GameObject>();

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
				Destroy(hit.collider);

				if (cutPointArray.Count == 3)
				{
                    Debug.Log(cutPointArray[0]);
                    Debug.Log(cutPointArray[1]);
                    Debug.Log(cutPointArray[2]);

                    Vector3 A = cutPointArray[0];
                    Vector3 B = cutPointArray[1];
                    Vector3 C = cutPointArray[2];

                    Vector3 AB = new Vector3(B.x - A.x, B.y - A.y, B.z - A.z);
                    Vector3 AC = new Vector3(C.x - A.x, C.y - A.y, C.z - A.z);

                    float a = (B.y - A.y) * (C.z - A.z) - (C.y - A.y) * (B.z - A.z);
                    float b = (B.z - A.z) * (C.x - A.x) - (C.z - A.z) * (B.x - A.x);
                    //Debug.Log(" (B.z - A.z) = " + (B.z - A.z));
                    //Debug.Log("C.x = " + C.x + "A.x = " + A.x);
                    //Debug.Log(" (C.x - A.x) = " + (C.x - (A.x)));
                    //Debug.Log(" (B.z - A.z) * (C.x - A.x) = " + (B.z - A.z) * (C.x - A.x));

                    //Debug.Log(" (C.z - A.z) = " + (C.z - A.z));
                    //Debug.Log(" (B.x - A.x) = " + (B.x - A.x));
                    //Debug.Log(" (C.z - A.z) * (B.x - A.x) = " + (C.z - A.z) * (B.x - A.x));


                    float c = (B.x - A.x) * (C.y - A.y) - (C.x - A.x) * (B.y - A.y);

                    float d = -(a * A.x + b * A.y + c * A.z);

                    Vector3 ABxAC = new Vector3(a, b, c);

                    Debug.Log("a = " + a);
                    Debug.Log("b = " + b);
                    Debug.Log("c = " + c);
                    Debug.Log("d = " + d);

                    Debug.Log("Q = " + (a + b + c + d));


                    //screenPoint.Add(mainCamera.WorldToScreenPoint(cutPointArray[0]));
                    //screenPoint.Add(mainCamera.WorldToScreenPoint(cutPointArray[1]));

                    //isCut = true;

                }

			}
		}
        if (isCut)
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Linecast(screenPoint[0], screenPoint[1], out hit))
            {
                Debug.Log(hit.collider);

            }

        }

    }
}



