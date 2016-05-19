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

        //// Debug ===============================================================================
        //for (var i = 0; i < baseVerPos.Count; i++)
        //{
        //    var verSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    verSphere.transform.position = new Vector3(baseVerPos[i].x, baseVerPos[i].y, baseVerPos[i].z);
        //    verSphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        //    verSphere.name = "CubeVertice[" + i + "]";
        //    verSphere.transform.parent = CubeVertices.transform;
        //    Debug.Log(baseVerPos[i]);

        //}
        //// ======================================================================================

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
                    //Debug.Log(cutPointArray[0]);
                    //Debug.Log(cutPointArray[1]);
                    //Debug.Log(cutPointArray[2]);

                    var cutPoint_01 = cutPointArray[0];
                    var cutPoint_02 = cutPointArray[1];
                    var cutPoint_03 = cutPointArray[2];

                    var cutPlaneNV = GetCrossProduct(cutPoint_01, cutPoint_02, cutPoint_03);

                    var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    obj.transform.position = cutPlaneNV;
                    obj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    obj.name = "CutPlaneNormalVector";

                    // 6,4,5, 0,1,2, 3,2,1, 7,5,2, 4,1,2, 0,6,7

                    int[,] verticesNo = new int[,]
                    {
                        { 6,0,3,7,4,0 },
                        { 4,1,2,5,1,6 },
                        { 5,4,1,2,2,7 }

                    };

                    for(var i = 0; i < verticesNo.Length / 3; i++)
                    {
                        var cubeVertices_01 = baseVerPos[verticesNo[0, i]];
                        var cubeVertices_02 = baseVerPos[verticesNo[1, i]];
                        var cubeVertices_03 = baseVerPos[verticesNo[2, i]];

                        var cubePlaneNV = GetCrossProduct(cubeVertices_01, cubeVertices_02, cubeVertices_03);

                        var obj02 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        obj02.transform.position = cubePlaneNV;
                        obj02.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                        obj02.name = "CubePlaneNormalVector";

                    }

                }

			}

		}

    }

}