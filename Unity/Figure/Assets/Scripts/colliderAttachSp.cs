﻿using UnityEngine;
using System.Collections.Generic;

public class colliderAttachSp : MonoBehaviour {

	public GameObject simpleCube;

	private static int boxColSize = 5;
	private MeshFilter mf;

	private Vector3[] cubeVerPos;

	private List<GameObject> obj = new List<GameObject>();
	private List<Vector3> baseVerPos = new List<Vector3>();
	private List<BoxCollider> boxColArray = new List<BoxCollider>();


	void AddToList(Vector3[] size, Vector3[] center, Quaternion[] rotation, List<BoxCollider> col)
	{
		for (var i = 0; i < size.Length; i++)
		{
			obj.Add(new GameObject("BoxCollider[" + i + "]"));
			col.Add(obj[i].AddComponent<BoxCollider>());
			col[i].size = size[i];
			obj[i].transform.position = center[i];
			obj[i].transform.rotation = rotation[i];

			obj[i].transform.parent = gameObject.transform;


		}

	}


	// Use this for initialization
	void Start () {
		mf = simpleCube.GetComponent<MeshFilter>();
		cubeVerPos = mf.mesh.vertices;


		foreach (var v in cubeVerPos)
		{
			if (!baseVerPos.Contains(v))
			{
				baseVerPos.Add(v);

			}

		}

		// AddToList (col.size, col.center, target collider);
		AddToList(new Vector3[] { // col.size 
			new Vector3(boxColSize, Vector3.Distance(baseVerPos[4], baseVerPos[6]), boxColSize), new Vector3(boxColSize, Vector3.Distance(baseVerPos[5], baseVerPos[7]), boxColSize),
			new Vector3(boxColSize, Vector3.Distance(baseVerPos[4], baseVerPos[5]), boxColSize), new Vector3(boxColSize, Vector3.Distance(baseVerPos[6], baseVerPos[7]), boxColSize),
			new Vector3(boxColSize, Vector3.Distance(baseVerPos[0], baseVerPos[6]), boxColSize), new Vector3(boxColSize, Vector3.Distance(baseVerPos[1], baseVerPos[4]), boxColSize),
			new Vector3(boxColSize, Vector3.Distance(baseVerPos[3], baseVerPos[7]), boxColSize), new Vector3(boxColSize, Vector3.Distance(baseVerPos[2], baseVerPos[5]), boxColSize),
			new Vector3(boxColSize, Vector3.Distance(baseVerPos[2], baseVerPos[3]), boxColSize), new Vector3(boxColSize, Vector3.Distance(baseVerPos[0], baseVerPos[1]), boxColSize),
			new Vector3(boxColSize, Vector3.Distance(baseVerPos[1], baseVerPos[2]), boxColSize), new Vector3(boxColSize, Vector3.Distance(baseVerPos[0], baseVerPos[3]), boxColSize)
		}, new Vector3[] {	// col.center
			new Vector3(baseVerPos[4].x, simpleCube.transform.position.y, baseVerPos[4].z), new Vector3(baseVerPos[5].x, simpleCube.transform.position.y, baseVerPos[5].z),
			new Vector3(simpleCube.transform.position.x, baseVerPos[4].y, baseVerPos[4].z), new Vector3(simpleCube.transform.position.x, baseVerPos[6].y, baseVerPos[6].z),
			new Vector3(baseVerPos[0].x, baseVerPos[0].y, simpleCube.transform.position.z), new Vector3(baseVerPos[1].x, baseVerPos[1].y, simpleCube.transform.position.z),
			new Vector3(baseVerPos[3].x, baseVerPos[3].y, simpleCube.transform.position.z), new Vector3(baseVerPos[2].x, baseVerPos[2].y, simpleCube.transform.position.z),
			new Vector3(baseVerPos[2].x, simpleCube.transform.position.y, baseVerPos[2].z), new Vector3(baseVerPos[0].x, simpleCube.transform.position.y, baseVerPos[0].z),
			new Vector3(simpleCube.transform.position.x, baseVerPos[1].y, baseVerPos[1].z), new Vector3(simpleCube.transform.position.x, baseVerPos[0].y, baseVerPos[0].z)
		}, new Quaternion[] {
			Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 0),
			Quaternion.Euler(0, 0, 90), Quaternion.Euler(0, 0, 90),
			Quaternion.Euler(90, 0, 0), Quaternion.Euler(90, 0, 0),
			Quaternion.Euler(90, 0, 0), Quaternion.Euler(90, 0, 0),
			Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 0),
			Quaternion.Euler(0, 0, 90), Quaternion.Euler(0, 0, 90)

		}, boxColArray);	// target collider


	}
	
	// Update is called once per frame
	void Update () {
	
	}

}