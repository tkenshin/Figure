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

			cutPoints.Add(cutPointOBJ.transform.position);
			Destroy(hit.collider);

			if (cutPoints.Count == 3)
			{
				if (cutPoints[0].x == cutPoints[1].x && cutPoints[0].x == cutPoints[2].x)
				{
					Debug.Log("無効なカットラインです。");

				}
				else if (cutPoints[0].z == cutPoints[1].z && cutPoints[0].z == cutPoints[2].z)
				{
					Debug.Log("無効なカットラインです。");

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
						Debug.Log("無効なカットラインです。");

					}

				}
				else
				{
					GameObject[] cutObjects = CutObject.Cut(cube, material);
				}

			}
		}

	}

    void Start()
    {
		cube.GetComponent<Renderer>().material = material;
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
			LocateCuttingPoints(cutPoints.ToArray());

        }
    }

}
