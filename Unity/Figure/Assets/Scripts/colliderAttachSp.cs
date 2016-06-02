﻿using UnityEngine;
using System.Linq;

public class colliderAttachSp : MonoBehaviour
{
    [SerializeField]
    private GameObject cube;
	private float collider_size = 0.05f;

    private static Vector3[] GetColliderSize(float size, Vector3[] vertices)
    {
        return new Vector3[]
        {
            new Vector3(size, Vector3.Distance(vertices[4], vertices[6]), size), new Vector3(size, Vector3.Distance(vertices[5], vertices[7]), size),
            new Vector3(size, Vector3.Distance(vertices[4], vertices[5]), size), new Vector3(size, Vector3.Distance(vertices[6], vertices[7]), size),
            new Vector3(size, Vector3.Distance(vertices[0], vertices[6]), size), new Vector3(size, Vector3.Distance(vertices[1], vertices[4]), size),
            new Vector3(size, Vector3.Distance(vertices[3], vertices[7]), size), new Vector3(size, Vector3.Distance(vertices[2], vertices[5]), size),
            new Vector3(size, Vector3.Distance(vertices[2], vertices[3]), size), new Vector3(size, Vector3.Distance(vertices[0], vertices[1]), size),
            new Vector3(size, Vector3.Distance(vertices[1], vertices[2]), size), new Vector3(size, Vector3.Distance(vertices[0], vertices[3]), size)
        };
    }

    private static Vector3[] GetColliderCenter(Transform cubeTF, Vector3[] vertices)
    {
        return new Vector3[]
        {
            new Vector3(vertices[4].x, cubeTF.position.y, vertices[4].z), new Vector3(vertices[5].x, cubeTF.position.y, vertices[5].z),
            new Vector3(cubeTF.position.x, vertices[4].y, vertices[4].z), new Vector3(cubeTF.position.x, vertices[6].y, vertices[6].z),
            new Vector3(vertices[0].x, vertices[0].y, cubeTF.position.z), new Vector3(vertices[1].x, vertices[1].y, cubeTF.position.z),
            new Vector3(vertices[3].x, vertices[3].y, cubeTF.position.z), new Vector3(vertices[2].x, vertices[2].y, cubeTF.position.z),
            new Vector3(vertices[2].x, cubeTF.position.y, vertices[2].z), new Vector3(vertices[0].x, cubeTF.position.y, vertices[0].z),
            new Vector3(cubeTF.position.x, vertices[1].y, vertices[1].z), new Vector3(cubeTF.position.x, vertices[0].y, vertices[0].z)
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

    private void CreateColliders(Vector3[] size, Vector3[] center, Quaternion[] angle)
    {
        for (var i = 0; i < size.Length; i++)
        {
            var obj = new GameObject("BoxCollider[" + i + "]");
            obj.transform.position = center[i];
            obj.transform.rotation = angle[i];
            obj.transform.parent = gameObject.transform;

            var col = obj.AddComponent<BoxCollider>();
            col.size = size[i];
        }
    }

    void Start()
    {
        var mf = cube.GetComponent<MeshFilter>();
        var vertices = mf.mesh.vertices.Distinct().ToArray();

        // AddToList (col.size, col.center, target collider);
        CreateColliders(GetColliderSize(collider_size, vertices), GetColliderCenter(cube.transform, vertices), GetColliderAngle());   // target collider
    }

    void Update()
    {

    }

}
