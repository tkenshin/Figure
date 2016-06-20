using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetCutPoints : MonoBehaviour
{
	[SerializeField]
	private GameObject cube;
	[SerializeField]
	private Camera mainCamera;
	[SerializeField]
	private Material material;

	private static MeshFilter mf;
	private static Vector3 tapPoint;
	private static Vector3[] cubeVertices;
	private static List<Vector3> baseVertices = new List<Vector3>();
	private static List<GameObject> cutPointObjects = new List<GameObject>();

	public static List<Vector3> cutPoints = new List<Vector3>();

	private void LocateCuttingPoints(Vector3[] cutPoint)
	{
		var mousePos = Input.mousePosition;
		var ray = Camera.main.ScreenPointToRay(mousePos);
		var hit = new RaycastHit();

		if (Physics.Raycast(ray, out hit, 100000) && cutPoints.Count < 3)
		{
			tapPoint = hit.point;

			var cutPointOBJ = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			cutPointOBJ.transform.position = tapPoint;
			cutPointOBJ.transform.parent = hit.transform;
			cutPointOBJ.transform.localRotation = new Quaternion(0, 0, 0, 0);
			cutPointOBJ.transform.localPosition = new Vector3(0, cutPointOBJ.transform.localPosition.y, 0);
			cutPointOBJ.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
			Destroy(cutPointOBJ.GetComponent<SphereCollider>());

			cutPoints.Add(cutPointOBJ.transform.position);
			cutPointObjects.Add(cutPointOBJ);
			Destroy(hit.collider);

			if (cutPoints.Count == 3)
			{
				if (cutPoints[0].x == cutPoints[1].x && cutPoints[0].x == cutPoints[2].x)
				{
					CutObject.meshState = MeshState.Invalid;

				}
				else if (cutPoints[0].z == cutPoints[1].z && cutPoints[0].z == cutPoints[2].z)
				{
					CutObject.meshState = MeshState.Invalid;

				}
				else if (cutPoints[0].y == cutPoints[1].y && cutPoints[0].y == cutPoints[2].y)
				{
					if (cutPoints[0].x != cutPoints[1].x && cutPoints[0].x != cutPoints[2].x)
					{
						GameObject[] cutObjects = CutObject.Cut(cube, material);

					}
					else if (cutPoints[0].z != cutPoints[1].z && cutPoints[0].z != cutPoints[2].z)
					{
						GameObject[] cutObjects = CutObject.Cut(cube, material);

					}
					else
					{
						CutObject.meshState = MeshState.Invalid;

					}

				}
				else
				{
					GameObject[] cutObjects = CutObject.Cut(cube, material);
				}

				Destroy(cutPointObjects[0]);
				Destroy(cutPointObjects[1]);
				Destroy(cutPointObjects[2]);

			}
		}

	}

	void Start()
	{
		mf = cube.GetComponent<MeshFilter>();
		cubeVertices = mf.mesh.vertices;

		foreach (var v in cubeVertices)
		{
			if (!baseVertices.Contains(v))
			{
				baseVertices.Add(v);

			}

		}

	}

	void Update()
	{
		if (Input.GetMouseButtonUp(0))
		{

			if (CutObject.meshState == MeshState.Isolation)
			{

				var mousePos = Input.mousePosition;
				var ray = Camera.main.ScreenPointToRay(mousePos);
				var hit = new RaycastHit();

				if (Physics.Raycast(ray, out hit, 100000) && hit.collider.gameObject != null)
				{

					Destroy(hit.collider.gameObject);

					CutObject.meshState = MeshState.Cut;
				}

			}

			LocateCuttingPoints(cutPoints.ToArray());

		}
	}
}
