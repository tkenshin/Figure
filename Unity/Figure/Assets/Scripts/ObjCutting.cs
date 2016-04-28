using UnityEngine;
using System.Collections.Generic;

public class ObjCutting : MonoBehaviour {

	public GameObject simpleCube;
	public Camera mainCamera;

	private MeshFilter mf;
	private bool isCut = false;
	private List<float> thetaArray = new List<float>();

	private Vector3 tapPoint;
	private Vector3[] cubeVerPos;
	private List<Vector3> baseVerPos = new List<Vector3>();
	private List<Vector3> cutPointArray = new List<Vector3>();
	private List<Vector3> horizontalPosArray = new List<Vector3>();


	float CalculationLeg(Vector3 cutPoint, Vector3 horizontalPoint, float th)
	{
		float dis = Vector3.Distance(cutPoint, horizontalPoint);

		float leg = dis * Mathf.Tan(th);

		return leg;
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
		for (var i = 0; i < baseVerPos.Count; i++)
		{
			GameObject verSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			verSphere.transform.position = new Vector3(baseVerPos[i].x, baseVerPos[i].y, baseVerPos[i].z);
			verSphere.transform.localScale = new Vector3(10, 10, 10);
			verSphere.name = "CubeVertice[" + i + "]";
			verSphere.transform.parent = CubeVertices.transform;
			Debug.Log(baseVerPos[i]);

		}
		// ======================================================================================

	}

	void Update () {
		if (Input.GetMouseButtonDown(0))
		{

			Vector3 mousePos = Input.mousePosition;
			Ray ray = Camera.main.ScreenPointToRay(mousePos);
			RaycastHit hit = new RaycastHit();

			if (Physics.Raycast(ray, out hit, 100000) && cutPointArray.Count < 2)
			{
				tapPoint = hit.point;

				Debug.Log("HIT!");

				GameObject cutPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				cutPoint.transform.position = tapPoint;
				cutPoint.transform.parent = hit.transform;
				cutPoint.transform.localRotation = new Quaternion(0, 0, 0, 0);
				cutPoint.transform.localPosition = new Vector3(0, cutPoint.transform.localPosition.y, 0);
				cutPoint.transform.localScale = new Vector3(10, 10, 10);

				cutPointArray.Add(cutPoint.transform.position);


				GameObject cameraPos = new GameObject("CameraPosOBJ");
				cameraPos.transform.position = mainCamera.transform.position;

				GameObject horizontalOBJ = new GameObject("CameraHorizontalOBJ");
				horizontalOBJ.transform.position = new Vector3(cameraPos.transform.position.x, simpleCube.transform.position.y, cameraPos.transform.position.z);


				float b = Vector3.Distance(cameraPos.transform.position, horizontalOBJ.transform.position);

				float a = Vector3.Distance(horizontalOBJ.transform.position, simpleCube.transform.position);

				float theta = Mathf.Atan(b / a) * (cameraPos.transform.position.y < 0 ? 1 : -1);

				thetaArray.Add(theta);

				Destroy(horizontalOBJ);
				Destroy(cameraPos);

				if (cutPointArray.Count == 2)
				{
					isCut = true;
				}

			}
		}

		if (cutPointArray.Count == 2 && isCut)
		{
			if (cutPointArray[0].z == cutPointArray[1].z)
			{
				if (cutPointArray[0].z < 0) // 正面
				{
					Debug.Log("正面");
					for (var i = 0; i < cutPointArray.Count; i++) 
					{
						GameObject horizontalOBJ = new GameObject("HorizontalOBJ");
						horizontalOBJ.transform.position = new Vector3(cutPointArray[i].x, cutPointArray[i].y, -cutPointArray[i].z);

						float leg = CalculationLeg(cutPointArray[i], horizontalOBJ.transform.position, thetaArray[i]);

						GameObject obj = new GameObject("HiddenPoint[" + i + "]");
						obj.transform.position = new Vector3(horizontalOBJ.transform.position.x, horizontalOBJ.transform.position.y + leg, horizontalOBJ.transform.position.z);

						Destroy(horizontalOBJ);

					}

				}
				else // 裏面
				{
					Debug.Log("裏面");
					for (var i = 0; i < cutPointArray.Count; i++)
					{
						//GameObject horizontalOBJ = new GameObject("HorizontalPos");
						//horizontalOBJ.transform.position = new Vector3(cutPointArray[i].x, cutPointArray[i].y, -cutPointArray[i].z);

						//horizontalPosArray.Add(horizontalOBJ.transform.position￥５a);


					}


				}

			}
			else if (cutPointArray[0].x == cutPointArray[1].x)
			{
				if (cutPointArray[0].x > 0) // 右面
				{
					Debug.Log("右面");

				}
				else
				{
					Debug.Log("左面");


				}


			}

			isCut = false;

		}
	}
}



