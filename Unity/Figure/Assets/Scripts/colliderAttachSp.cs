using UnityEngine;
using System.Collections.Generic;

public class colliderAttachSp : MonoBehaviour
{

    [SerializeField]
    private GameObject cube;
	private float collider_size = 0.05f;

    private MeshFilter mf;

	private List<GameObject> collider_objects = new List<GameObject>();
	private List<Vector3> base_vertices = new List<Vector3>();
	private List<BoxCollider> box_colliders = new List<BoxCollider>();

    private Vector3[] GetColliderSize(float size)
    {
        return new Vector3[]
        {
            new Vector3(size, Vector3.Distance(base_vertices[4], base_vertices[6]), size), new Vector3(size, Vector3.Distance(base_vertices[5], base_vertices[7]), size),
            new Vector3(size, Vector3.Distance(base_vertices[4], base_vertices[5]), size), new Vector3(size, Vector3.Distance(base_vertices[6], base_vertices[7]), size),
            new Vector3(size, Vector3.Distance(base_vertices[0], base_vertices[6]), size), new Vector3(size, Vector3.Distance(base_vertices[1], base_vertices[4]), size),
            new Vector3(size, Vector3.Distance(base_vertices[3], base_vertices[7]), size), new Vector3(size, Vector3.Distance(base_vertices[2], base_vertices[5]), size),
            new Vector3(size, Vector3.Distance(base_vertices[2], base_vertices[3]), size), new Vector3(size, Vector3.Distance(base_vertices[0], base_vertices[1]), size),
            new Vector3(size, Vector3.Distance(base_vertices[1], base_vertices[2]), size), new Vector3(size, Vector3.Distance(base_vertices[0], base_vertices[3]), size)

        };
    }

    private Vector3[] GetColliderCenter(Transform cubeTF)
    {
        return new Vector3[]
        {
            new Vector3(base_vertices[4].x, cubeTF.position.y, base_vertices[4].z), new Vector3(base_vertices[5].x, cubeTF.position.y, base_vertices[5].z),
            new Vector3(cubeTF.position.x, base_vertices[4].y, base_vertices[4].z), new Vector3(cubeTF.position.x, base_vertices[6].y, base_vertices[6].z),
            new Vector3(base_vertices[0].x, base_vertices[0].y, cubeTF.position.z), new Vector3(base_vertices[1].x, base_vertices[1].y, cubeTF.position.z),
            new Vector3(base_vertices[3].x, base_vertices[3].y, cubeTF.position.z), new Vector3(base_vertices[2].x, base_vertices[2].y, cubeTF.position.z),
            new Vector3(base_vertices[2].x, cubeTF.position.y, base_vertices[2].z), new Vector3(base_vertices[0].x, cubeTF.position.y, base_vertices[0].z),
            new Vector3(cubeTF.position.x, base_vertices[1].y, base_vertices[1].z), new Vector3(cubeTF.position.x, base_vertices[0].y, base_vertices[0].z)

        };
    }

    private Quaternion[] GetColliderAngle()
    {
        return new Quaternion[]
        {
            Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 0),
            Quaternion.Euler(0, 0, 90), Quaternion.Euler(0, 0, 90),
            Quaternion.Euler(90, 0, 0), Quaternion.Euler(90, 0, 0),
            Quaternion.Euler(90, 0, 0), Quaternion.Euler(90, 0, 0),
            Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 0),
            Quaternion.Euler(0, 0, 90), Quaternion.Euler(0, 0, 90)

        };

    }

    private void CreateColliders(Vector3[] size, Vector3[] center, Quaternion[] angle, List<BoxCollider> col)
    {
        for (var i = 0; i < size.Length; i++)
        {
            collider_objects.Add(new GameObject("BoxCollider[" + i + "]"));
            col.Add(collider_objects[i].AddComponent<BoxCollider>());
            col[i].size = size[i];
            collider_objects[i].transform.position = center[i];
            collider_objects[i].transform.rotation = angle[i];

            collider_objects[i].transform.parent = gameObject.transform;

        }

    }

    void Start()
    {
        mf = cube.GetComponent<MeshFilter>();
        Vector3[] vertices = mf.mesh.vertices;


        foreach (var v in vertices)
        {
            if (!base_vertices.Contains(v))
            {
                base_vertices.Add(v);

            }

        }

        // AddToList (col.size, col.center, target collider);
        CreateColliders(GetColliderSize(collider_size), GetColliderCenter(cube.transform), GetColliderAngle(), box_colliders);   // target collider


    }

    void Update()
    {

    }

}
