using UnityEngine;
using System.Collections.Generic;

public class ObjCutting : MonoBehaviour
{

    [SerializeField]
    private GameObject cube;
    private Camera mainCamera;

	private MeshFilter mf;
	private static float constant_02;
    private static float constant_01;

    private Vector3 tapPoint;
	private Vector3 pt;
    private Vector3[] cubeVerPos;

    private List<Vector3> baseVerPos = new List<Vector3>();
    private List<Vector3> cutPointArray = new List<Vector3>();

    private Vector3 GetCrossProduct(Vector3 A, Vector3 B, Vector3 C)
    {
        var AB = B - A;
        var AC = C - A;

        return Vector3.Cross(AB, AC);
    }

	private bool IntersectionLine(Vector3 e, Vector3 cubePlaneNV, Vector3 cutPlaneNV, float d01, float d02)
	{
		if (0 != e.z)
		{
			pt.x = (d01 * cubePlaneNV.y - d02 * cutPlaneNV.y) / e.z;
			pt.y = (d01 * cubePlaneNV.x - d02 * cutPlaneNV.x) / (-e.z);
			pt.z = 0;

			return true;

		}

		if (0 != e.y)
		{
			pt.x = (d01 * cubePlaneNV.z - d02 * cutPlaneNV.z) / (-e.y);
			pt.y = 0;
			pt.z = (d01 * cubePlaneNV.x - d02 * cutPlaneNV.x) / e.y;

			return true;

		}

		if (0 != e.x)
		{
			pt.x = 0;
			pt.y = (d01 * cubePlaneNV.z - d02 * cutPlaneNV.z) / e.x;
			pt.z = (d01 * cubePlaneNV.y - d02 * cutPlaneNV.y) / (-e.x);

			return true;

		}

		return false;


	}

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;

        }

        if (cube == null)
        {
            cube = GameObject.Find("cube");


        }

    }

    void Start()
    {
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

    void Update()
    {
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
                cutPoint.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);

                cutPointArray.Add(cutPoint.transform.position);
                Destroy(hit.collider);

				if (cutPointArray.Count == 3)
				{
					


				}

                
            }

        }

    }

}

//if (cutPointArray.Count == 3)
//                {
//                    var directionalVectors = new GameObject("DirectionalVectors");
//var cubePlaneNormalVectors = new GameObject("CubePlaneNormalVectors");
//var intesectPoint = new GameObject("IntesectPoints");

////Debug.Log(cutPointArray[0]);
////Debug.Log(cutPointArray[1]);
////Debug.Log(cutPointArray[2]);

//var cutPoint_01 = cutPointArray[0];
//var cutPoint_02 = cutPointArray[1];
//var cutPoint_03 = cutPointArray[2];

//var cutPlaneNV = GetCrossProduct(cutPoint_01, cutPoint_02, cutPoint_03);

//var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//obj.transform.position = cutPlaneNV;
//                    obj.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
//obj.name = "CutPlaneNormalVector";

//                    constant_01 = -(cutPlaneNV.x* cutPoint_01.x + cutPlaneNV.y* cutPoint_01.y + cutPlaneNV.z* cutPoint_01.z);
//Debug.Log(cutPlaneNV.x + cutPlaneNV.y + cutPlaneNV.z + constant_01);

//                    // 6,4,5, 0,1,2, 3,2,1, 7,5,2, 4,1,2, 0,6,7

//                    int[,] verticesNo = new int[,]
//					{
//						{ 6,0,3,7,4,0 },
//						{ 4,1,2,5,1,6 },
//						{ 5,4,1,2,2,7 }

//					};

//					for (var i = 0; i<verticesNo.Length / 3; i++)
//					{
//						var cubeVertices_01 = baseVerPos[verticesNo[0, i]];
//var cubeVertices_02 = baseVerPos[verticesNo[1, i]];
//var cubeVertices_03 = baseVerPos[verticesNo[2, i]];

////var cubeVertices_01 = baseVerPos[3];
////var cubeVertices_02 = baseVerPos[2];
////var cubeVertices_03 = baseVerPos[1];

//var cubePlaneNV = GetCrossProduct(cubeVertices_01, cubeVertices_02, cubeVertices_03);

//var obj02 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//obj02.transform.position = -(cubePlaneNV);
//					    obj02.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
//obj02.name = "CubePlaneNormalVector";
//					    obj02.transform.parent = cubePlaneNormalVectors.transform;

//					    constant_02 = -(cubePlaneNV.x* cubeVertices_01.x +
//					                    cubePlaneNV.y* cubeVertices_01.y +

//										cubePlaneNV.z* cubeVertices_01.z);

//Debug.Log(cubePlaneNV.x + cubePlaneNV.y + cubePlaneNV.z + constant_02);

//						var e = Vector3.Cross(cutPlaneNV, cubePlaneNV);

//var obj03 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//obj03.transform.position = e;
//						obj03.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
//obj03.name = "DirectionalVector";
//						obj03.transform.parent = directionalVectors.transform;
					  	
//						if (IntersectionLine(e, cubePlaneNV, cutPlaneNV, constant_01, constant_02))
//						{
//							var obj04 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//obj04.transform.position = pt;
//							obj04.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
//obj04.name = "IntesectPoint";
//						 	obj04.transform.parent = intesectPoint.transform;
					  	

//						}

//					}

//				}
