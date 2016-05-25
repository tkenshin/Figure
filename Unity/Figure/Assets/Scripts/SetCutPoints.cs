using UnityEngine;
using System.Collections.Generic;

public class SetCutPoints : MonoBehaviour
{

    [SerializeField]
    private GameObject cube;
    [SerializeField]
    private Material material;
    [SerializeField]
    private Camera mainCamera;

    private static MeshFilter mf;
    private static Vector3 tapPoint;
    private static Vector3[] cubeVertices;
    private static List<Vector3> baseVertices = new List<Vector3>();

    public static List<Vector3> cutPoints = new List<Vector3>();

    void Start()
    {
        var CubeVertices = new GameObject("CubeVertices");
        mf = cube.GetComponent<MeshFilter>();
        cubeVertices = mf.mesh.vertices;

        foreach (var v in cubeVertices)
        {
            if (!baseVertices.Contains(v))
            {
                baseVertices.Add(v);

            }

        }

        // Debug ===============================================================================
        //for (var i = 0; i < baseVertices.Count; i++)
        //{
        //    var verSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    verSphere.transform.position = baseVertices[i];
        //    verSphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        //    verSphere.name = "CubeVertice[" + i + "]";
        //    verSphere.transform.parent = CubeVertices.transform;
        //    Debug.Log(baseVertices[i]);
        //}
        // ======================================================================================
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {

            var mousePos = Input.mousePosition;
            var ray = Camera.main.ScreenPointToRay(mousePos);
            var hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, 100000) && cutPoints.Count < 3)
            {
                tapPoint = hit.point;

                var cutPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                cutPoint.transform.position = tapPoint;
                cutPoint.transform.parent = hit.transform;
                cutPoint.transform.localRotation = new Quaternion(0, 0, 0, 0);
                cutPoint.transform.localPosition = new Vector3(0, cutPoint.transform.localPosition.y, 0);
                cutPoint.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                cutPoints.Add(cutPoint.transform.position);
                Destroy(hit.collider);

                if (cutPoints.Count == 3)
                {
                    if (cutPoints[0].x == cutPoints[1].x && cutPoints[0].x == cutPoints[2].x)
                    {
                        Debug.Log("無効なカットラインです。");

                    } else if (cutPoints[0].z == cutPoints[1].z && cutPoints[0].z == cutPoints[2].z)
                    {

                        Debug.Log("無効なカットラインです。");

                    } else if(cutPoints[0].y == cutPoints[1].y && cutPoints[0].y == cutPoints[2].y)
                    {
                        if(cutPoints[0].x != cutPoints[1].x && cutPoints[0].x != cutPoints[2].x)
                        {
                            GameObject[] cutObjects = CutObject.Cut(cube);

                        } else if (cutPoints[0].z != cutPoints[1].z && cutPoints[0].z != cutPoints[2].z)
                        {
                            GameObject[] cutObjects = CutObject.Cut(cube);

                        } else
                        {
                            Debug.Log("無効なカットラインです。");

                        }


                    } else
                    {
                        GameObject[] cutObjects = CutObject.Cut(cube);
                    }

                }
            }
        }
    }
}
