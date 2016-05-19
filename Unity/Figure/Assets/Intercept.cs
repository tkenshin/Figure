using UnityEngine;
using System.Collections.Generic;

public class Intercept : MonoBehaviour {

    [SerializeField]
    private GameObject plane01;
    [SerializeField]
    private GameObject plane02;

    private MeshFilter mf01;
    private MeshFilter mf02;

    private List<Vector3> plane01Vertices = new List<Vector3>();
    private List<Vector3> plane02Vertices = new List<Vector3>();

	void Start () {
        mf01 = plane01.GetComponent<MeshFilter>();
        mf02 = plane02.GetComponent<MeshFilter>();

        Vector3[] basePlane01Vertices = mf01.mesh.vertices;
        Vector3[] basePlane02Vertices = mf02.mesh.vertices;

        for (var i = 0; i < basePlane01Vertices.Length; i++)
        {
            plane01Vertices.Add(basePlane01Vertices[i]);
            plane02Vertices.Add(basePlane02Vertices[i]);

        }

        for (var i = 0; i < plane01Vertices.Count - 1; i++)
        {
            GameObject verSp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            verSp.transform.position = plane01Vertices[i];
            verSp.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            verSp.name = "Plane01Vertex[" + i + "]";

            Debug.Log("Plane01Vertex[" + i + "]" + plane01Vertices[i]);

        }

		for (var i = 0; i < plane02Vertices.Count - 1; i++)
		{
			GameObject verSp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			verSp.transform.position = plane02Vertices[i];
			verSp.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

			verSp.name = "Plane02Vertex[" + i + "]";

			Debug.Log("Plane02Vertex[" + i + "]" + plane02Vertices[i]);

		}

    }


	void Update () {
        // Plane01
        if(Input.GetKeyDown(KeyCode.Return))
        {
            // PlaneVertex_00 = A
            // PlaneVertex_01 = B
            // PlaneVertex_02 = C

            Vector3 A = plane01Vertices[0];
            Vector3 B = plane01Vertices[1];
            Vector3 C = plane01Vertices[2];

            Vector3 AB = new Vector3(B.x - A.x, B.y - A.y, B.z - A.z);
            Vector3 AC = new Vector3(C.x - A.x, C.y - A.y, C.z - A.z);

            //Vector3 n = new Vector3(AB.x * AC.x, AB.y * AC.y, AB.z * AC.z);
            float a = (B.y - A.y) * (C.z - A.z) - (C.y - A.y) * (B.z - A.z);
            float b = (B.z - A.z) * (C.x - A.x) - (C.z - A.z) * (B.x - A.x);
            float c = (B.x - A.x) * (C.y - A.y) - (C.x - A.x) * (B.y - A.y);

            float d = -(a * A.x + b * A.y + c * A.z);

            Vector3 n = new Vector3(a, b, c);


            GameObject verSp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            verSp.transform.position = n;
            verSp.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            // ax + by + cz + d = 0;

            Debug.Log(a + "x + " + b + "y + " + c + "z");

            Debug.Log("n = " + n);
            Debug.Log("d = " + d);

        }

        if(Input.GetKeyDown(KeyCode.Space))
		{
            // PlaneVertex_00 = A
            // PlaneVertex_01 = B
            // PlaneVertex_02 = C

            Vector3 A = plane02Vertices[0];
            Vector3 B = plane02Vertices[1];
            Vector3 C = plane02Vertices[2];

            Vector3 AB = new Vector3(B.x - A.x, B.y - A.y, B.z - A.z);
            Vector3 AC = new Vector3(C.x - A.x, C.y - A.y, C.z - A.z);

            float a = (B.y - A.y) * (C.z - A.z) - (C.y - A.y) * (B.z - A.z);
            float b = (B.z - A.z) * (C.x - A.x) - (C.z - A.z) * (B.x - A.x);
            float c = (B.x - A.x) * (C.y - A.y) - (C.x - A.x) * (B.y - A.y);

            float d = -(a * A.x + b * A.y + c * A.z);

			Vector3 n = new Vector3(a, b, c);

            GameObject verSp02 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            verSp02.transform.position = n;
            verSp02.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            Debug.Log("n = " + n);
            Debug.Log("d = " + d);


        }

    }
}
