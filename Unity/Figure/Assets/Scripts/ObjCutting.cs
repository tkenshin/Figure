using UnityEngine;
using System.Collections.Generic;

public class ObjCutting : MonoBehaviour {

	[SerializeField]
	private GameObject cube;
	private Camera mainCamera;

	private MeshFilter mf;
	private bool isCut = false;

	private Vector3 tapPoint;
	private Vector3[] cubeVerPos;
	private List<Vector3> baseVerPos = new List<Vector3>();
	private List<Vector3> cutPointArray = new List<Vector3>();
	private List<Vector3> screenPoint = new List<Vector3>();
    private List<GameObject> screenPointOBJ = new List<GameObject>();

    private Vector3 GetCrossProduct(Vector3 A, Vector3 B, Vector3 C)
    {
        var AB = B - A;
        var AC = C - A;

        return Vector3.Cross(AB, AC);
    }

    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;

        }

        if(cube == null)
        {
            cube = GameObject.Find("cube");


        }

    }

	void Start () {
		var CubeVertices = new GameObject("CubeVertices");
		mf = cube.GetComponent<MeshFilter>();
		cubeVerPos = mf.mesh.vertices;

		foreach (var v in cubeVerPos)
		{
			if (!baseVerPos.Contains(v))
			{
				baseVerPos.Add(v);

			}

		}

        // Debug ===============================================================================
        for (var i = 0; i < baseVerPos.Count; i++)
        {
            var verSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            verSphere.transform.position = new Vector3(baseVerPos[i].x, baseVerPos[i].y, baseVerPos[i].z);
            verSphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            verSphere.name = "CubeVertice[" + i + "]";
            verSphere.transform.parent = CubeVertices.transform;
            Debug.Log(baseVerPos[i]);

        }
        // ======================================================================================

    }

	void Update () {
		if (Input.GetMouseButtonUp(0))
		{

			var mousePos = Input.mousePosition;
			var ray = Camera.main.ScreenPointToRay(mousePos);
			var hit = new RaycastHit();

			if (Physics.Raycast(ray, out hit, 100000) && cutPointArray.Count < 3)
			{
				tapPoint = hit.point;

				var cutPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				cutPoint.transform.position = tapPoint;
				cutPoint.transform.parent = hit.transform;
				cutPoint.transform.localRotation = new Quaternion(0, 0, 0, 0);
				cutPoint.transform.localPosition = new Vector3(0, cutPoint.transform.localPosition.y, 0);
				cutPoint.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                cutPointArray.Add(cutPoint.transform.position);
				Destroy(hit.collider);

				if (cutPointArray.Count == 3)
				{
                    Debug.Log(cutPointArray[0]);
                    Debug.Log(cutPointArray[1]);
                    Debug.Log(cutPointArray[2]);

                    var A = cutPointArray[0];
                    var B = cutPointArray[1];
                    var C = cutPointArray[2];

                    var n = GetCrossProduct(A, B, C);

                    var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    obj.transform.position = n;
                    obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);


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