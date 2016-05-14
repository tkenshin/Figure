using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Vector3Comparer : IComparer<Vector3>
{
	public int Compare(Vector3 a, Vector3 b)
	{
		if (a.y > b.y)
		{
			return -1;

		}
		else if (Mathf.Approximately(a.y, b.y))
		{
			if (a.x > b.x)
			{
				return 0;

			}
			else if (Mathf.Approximately(a.x, b.x))
			{
				if (a.z > b.z)
				{
					return 0;

				}
				else if (Mathf.Approximately(a.z, b.z))
				{
					return 0;

				}
				else
				{
					return 0;

				}

			}
			else
			{
				return 0;

			}

		}
		else
		{
			return 0;

		}

	}

}

public class ObjCutting : MonoBehaviour {

	[SerializeField]
	private GameObject simpleCube;
	[SerializeField]
	private Camera mainCamera;

	private MeshFilter mf;
	private bool isCut = false;

	private Vector3 tapPoint;
	private Vector3[] cubeVerPos;
	private List<Vector3> baseVerPos = new List<Vector3>();
	private List<Vector3> cutPointArray = new List<Vector3>();
	private List<Vector3> screenPoint = new List<Vector3>();

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
		if (Input.GetMouseButtonUp(0))
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
				Destroy(hit.collider);

				if (cutPointArray.Count == 2)
				{
					isCut = true;
				}

			}
		}

		if (cutPointArray.Count == 2 && isCut)
		{
			screenPoint.Add(mainCamera.WorldToScreenPoint(cutPointArray[0]));
			screenPoint.Add(mainCamera.WorldToScreenPoint(cutPointArray[1]));
			//screenPoint.Sort(new Vector3Comparer());

			for (var i = 0; i < screenPoint.Count; i++)
			{
				Debug.Log("screePoint[" + i + "] = " + screenPoint[i]);

			}

			if (Physics.Linecast(cutPointArray[0], cutPointArray[1])) 
			{
				

			}

			//float a = rot2Dir(AngleCalculation(screenPoint));
			//Debug.Log("a = " + a);
			//Debug.Log("ac = " + AngleCalculation(screenPoint));


			isCut = false;
		}

	}
}



