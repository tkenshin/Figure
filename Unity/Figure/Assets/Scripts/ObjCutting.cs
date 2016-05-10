using UnityEngine;
using System.Collections.Generic;

public class ObjCutting : MonoBehaviour {

	public GameObject simpleCube;
	public Camera mainCamera;

	private MeshFilter mf;
	private bool isCut = false;

	private Vector3 tapPoint;
	private Vector3[] cubeVerPos;
	private List<Vector3> baseVerPos = new List<Vector3>();
	private List<Vector3> cutPointArray = new List<Vector3>();
	private List<Vector3> screenPointArray = new List<Vector3>();

	private float rot2Dir(float radian)
	{
		return radian * 180 / Mathf.PI;

	}

	private float AngleCalculation(List<Vector3> sp)
	{
		float b = Vector2.Distance(sp[1], sp[2]);

		float a = Vector2.Distance(sp[0], sp[2]);

		return Mathf.Atan(a / b);

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

				if (cutPointArray.Count == 2)
				{
					isCut = true;
				}

			}
		}

		if (cutPointArray.Count == 2 && isCut)
		{
			screenPointArray.Add(mainCamera.WorldToScreenPoint(cutPointArray[0]));
			screenPointArray.Add(mainCamera.WorldToScreenPoint(cutPointArray[1]));
			screenPointArray.Add(new Vector3(screenPointArray[0].x, screenPointArray[1].y, screenPointArray[0].z));

			for (var i = 0; i < screenPointArray.Count; i++)
			{
				Debug.Log("screenPointArray [" + i + "] = " + screenPointArray[i]);


			}

			float a = rot2Dir(AngleCalculation(screenPointArray));
			Debug.Log("a = " + a);
			Debug.Log("ac = " + AngleCalculation(screenPointArray));

			isCut = false;
		}
	}
}



