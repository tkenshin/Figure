using UnityEngine;
using System.Collections.Generic;

public class CutObject
{
	private static MeshGeneration a_side = new MeshGeneration();
	private static MeshGeneration b_side = new MeshGeneration();

	private static Plane cutPlane;
	private static Mesh targetMesh;

	private static List<Vector3> newVertices = new List<Vector3>();

	private class MeshGeneration
	{
		public List<Vector3> vertices = new List<Vector3>();
		public List<Vector3> normals = new List<Vector3>();
		public List<Vector2> uvs = new List<Vector2>();
		public List<int> triangles = new List<int>();

		public void InitializeAll()
		{
			vertices.Clear();
			normals.Clear();
			uvs.Clear();
			triangles.Clear();

		}

		public void CreateTriangle(int point01, int point02, int point03)
		{
			var index = vertices.Count;

			triangles.Add(index);
			triangles.Add(index + 1);
			triangles.Add(index + 2);

			vertices.Add(targetMesh.vertices[point01]);
			vertices.Add(targetMesh.vertices[point02]);
			vertices.Add(targetMesh.vertices[point03]);

			normals.Add(targetMesh.normals[point01]);
			normals.Add(targetMesh.normals[point02]);
			normals.Add(targetMesh.normals[point03]);

			uvs.Add(targetMesh.uv[point01]);
			uvs.Add(targetMesh.uv[point02]);
			uvs.Add(targetMesh.uv[point03]);

		}

		public void CreateTriangle(Vector3[] three_points, Vector3[] three_normals, Vector2[] three_uvs, Vector3 faceNormal)
		{
			var CrossProductNormal = Vector3.Cross((three_points[1] - three_points[0]).normalized, (three_points[2] - three_points[0]).normalized);

			var point01 = 0;
			var point02 = 1;
			var point03 = 2;

			if (Vector3.Dot(CrossProductNormal, faceNormal) < 0)
			{
				point01 = 2;
				point02 = 1;
				point03 = 0;

			}

			var index = vertices.Count;

			triangles.Add(index);
			triangles.Add(index + 1);
			triangles.Add(index + 2);

			vertices.Add(three_points[point01]);
			vertices.Add(three_points[point02]);
			vertices.Add(three_points[point03]);

			normals.Add(three_normals[point01]);
			normals.Add(three_normals[point02]);
			normals.Add(three_normals[point03]);

			uvs.Add(three_uvs[point01]);
			uvs.Add(three_uvs[point02]);
			uvs.Add(three_uvs[point03]);

		}

	}

	public static GameObject[] Cut(GameObject target, Material material)
	{
		//var vertices = new GameObject("Vertices");

		Vector3[] cutPoints = SetCutPoints.cutPoints.ToArray();

		cutPlane = new Plane(cutPoints[0], cutPoints[1], cutPoints[2]);

		targetMesh = target.GetComponent<MeshFilter>().mesh;

		newVertices.Clear();
		a_side.InitializeAll();
		b_side.InitializeAll();

		bool[] sides = new bool[3];
		int point01, point02, point03;
		int[] indices;

		//     for (var r = 0; r < targetMesh.vertices.Length; r++)
		//     {
		//var sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		//         sp.transform.position = targetMesh.vertices[r];
		//         sp.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
		//         sp.name = "Sphere[" + r + "]";
		//sp.transform.parent = vertices.transform;

		//     }

		for (var v = 0; v < targetMesh.subMeshCount; v++)
		{
			// indices.Length = 36(0 ~ 35);
			// indicesにはCube(対象)の三角メッシュのIndex情報が入る。
			indices = targetMesh.GetIndices(v);

			// 36 / 3  = 12回繰り返す
			for (var i = 0; i < indices.Length; i += 3)
			{
				// 三角メッシュ(triangles)のパターン(0, 1, 2 ...)を各point01 ~ 03に入れる。(頂点情報)
				point01 = indices[i];
				point02 = indices[i + 1];
				point03 = indices[i + 2];

				// 各sides[0 ~ 2](BOOL)にCutPlaneが各頂点座標に対して表かをtrue, falseで返す。
				sides[0] = cutPlane.GetSide(targetMesh.vertices[point01]);
				sides[1] = cutPlane.GetSide(targetMesh.vertices[point02]);
				sides[2] = cutPlane.GetSide(targetMesh.vertices[point03]);

				// sides[0]とsides[1]がtrue or false、なおかつsides[0]とsides[2]がtrue or false の場合
				// (各point01 ~ 03で構成されたtriangleの各Indexの座標がすべてcutPlaneの表または裏の場合)
				if (sides[0] == sides[1] && sides[0] == sides[2])
				{
					//Debug.Log("point01 = " + point01);
					//Debug.Log("point02 = " + point02);
					//Debug.Log("point03 = " + point03);

					//Debug.Log("sides[0] = " + sides[0]);
					//Debug.Log("sides[1] = " + sides[1]);
					//Debug.Log("sides[2] = " + sides[2]);

					if (sides[0])
					{
						a_side.CreateTriangle(point01, point02, point03);

					}
					else
					{
						b_side.CreateTriangle(point01, point02, point03);

					}

				}
				else
				{
					// Triangle Index 情報と各 Index のBOOL情報(切断面に対してその Triangle Index が表にあるか裏にあるか)
					CutFace(sides, point01, point02, point03);

				}
			}
		}

		AddMeshMissing(newVertices.ToArray());

		var a_mesh = new Mesh();
		a_mesh.vertices = a_side.vertices.ToArray();
		a_mesh.triangles = a_side.triangles.ToArray();
		a_mesh.normals = a_side.normals.ToArray();
		a_mesh.uv = a_side.uvs.ToArray();

		var b_mesh = new Mesh();
		b_mesh.vertices = b_side.vertices.ToArray();
		b_mesh.triangles = b_side.triangles.ToArray();
		b_mesh.normals = b_side.normals.ToArray();
		b_mesh.uv = b_side.uvs.ToArray();

		target.GetComponent<MeshFilter>().mesh = a_mesh;
		Material[] materials = target.GetComponent<MeshRenderer>().sharedMaterials;


		var a_object = target;
		a_object.name = "A_Object";

		var b_object = new GameObject("B_Object");
		b_object.AddComponent<MeshFilter>();
		b_object.AddComponent<MeshRenderer>();
		b_object.transform.position = target.transform.position;
		b_object.transform.rotation = target.transform.rotation;
		b_object.GetComponent<MeshFilter>().mesh = b_mesh;

		a_object.GetComponent<MeshRenderer>().materials = materials;
		b_object.GetComponent<MeshRenderer>().materials = materials;

		return new GameObject[] { a_object, b_object };
	}

	private static void CutFace(bool[] sides, int index01, int index02, int index03)
	{
		// 必要な配列宣言
		Vector3[] leftPoints = new Vector3[2];
		Vector3[] leftNormals = new Vector3[2];
		Vector2[] leftUvs = new Vector2[2];
		Vector3[] rightPoints = new Vector3[2];
		Vector3[] rightNormals = new Vector3[2];
		Vector2[] rightUvs = new Vector2[2];

		// Index01 ~ 03 は Triangle の Index が入る。
		// 入ってくる Triangle 情報は切断点から生成された面に対して、すべての Triangle Index がTrue or Falseではないもの。

		var isLeft = false;
		var isRight = false;

		// index01(int) を p に入れる。
		var p = index01;

		// 合計24回(仮)(CutFaceに何回くるかによって変動 CutFaceにくる回数 * 3 = XX回)
		for (var i = 0; i < 3; i++)
		{
			// i = 0 の時 p = index01
			// i = 1 の時 p = index02
			// i = 2 の時 p = index03
			switch (i)
			{
				case 0:
					p = index01;
					break;
				case 1:
					p = index02;
					break;
				case 2:
					p = index03;
					break;
				default:
					break;

			}

			// 切断面に対して Triangle Index の [i] が True of False か。
			// True だった場合 Left,  False だった場合 Rightとする。
			// 初めの方に宣言した配列に切断面に対して右の頂点、左の頂点を入れる。
			if (sides[i])
			{

				if (!isLeft)
				{
					isLeft = true;
					leftPoints[0] = targetMesh.vertices[p];
					leftPoints[1] = leftPoints[0];
					leftUvs[0] = targetMesh.uv[p];
					leftUvs[1] = leftUvs[0];
					leftNormals[0] = targetMesh.normals[p];
					leftNormals[1] = leftNormals[0];


				}
				else
				{

					leftPoints[1] = targetMesh.vertices[p];
					leftUvs[1] = targetMesh.uv[p];
					leftNormals[1] = targetMesh.normals[p];

				}


			}
			else
			{

				if (!isRight)
				{
					isRight = true;

					rightPoints[0] = targetMesh.vertices[p];
					rightPoints[1] = rightPoints[0];
					rightUvs[0] = targetMesh.uv[p];
					rightUvs[1] = rightUvs[0];
					rightNormals[0] = targetMesh.normals[p];
					rightNormals[1] = rightNormals[0];

				}
				else
				{

					rightPoints[1] = targetMesh.vertices[p];
					rightUvs[1] = targetMesh.uv[p];
					rightNormals[1] = targetMesh.normals[p];

				}


			}

		}


		// normalizeDistance(正規化された距離) を宣言(0.0f)
		var normalizedDistance = 0.0f;
		// distance を宣言(0.0f)
		var distance = 0.0f;


		// LeftPoints と RightPoints 間で Ray を飛ばし、切断面にその Ray が交わる場合 Ray に沿った距離を Distance に入れる。
		cutPlane.Raycast(new Ray(leftPoints[0], (rightPoints[0] - leftPoints[0]).normalized), out distance);

		normalizedDistance = distance / (rightPoints[0] - leftPoints[0]).magnitude;

		var new_vertex_01 = Vector3.Lerp(leftPoints[0], rightPoints[0], normalizedDistance); // ray.getpointでもできる。
		var new_uv_01 = Vector2.Lerp(leftUvs[0], rightUvs[0], normalizedDistance);
		var new_normal_01 = Vector3.Lerp(leftNormals[0], rightNormals[0], normalizedDistance);

		newVertices.Add(new_vertex_01);


		// LeftPoints と RightPoints 間で Ray を飛ばし、切断面にその Ray が交わる場合 Ray に沿った距離を Distance に入れる。
		cutPlane.Raycast(new Ray(leftPoints[1], (rightPoints[1] - leftPoints[1]).normalized), out distance);

		normalizedDistance = distance / (rightPoints[1] - leftPoints[1]).magnitude;

		var new_vertex_02 = Vector3.Lerp(leftPoints[1], rightPoints[1], normalizedDistance); // ray.getpointでもできる。
		var new_uv_02 = Vector2.Lerp(leftUvs[1], rightUvs[1], normalizedDistance);
		var new_normal_02 = Vector3.Lerp(leftNormals[1], rightNormals[1], normalizedDistance);

		newVertices.Add(new_vertex_02);



		a_side.CreateTriangle(new Vector3[] { leftPoints[0], new_vertex_01, new_vertex_02 },
			new Vector3[] { leftNormals[0], new_normal_01, new_normal_02 },
			new Vector2[] { leftUvs[0], new_uv_01, new_uv_02 }, new_normal_01);

		a_side.CreateTriangle(new Vector3[] { leftPoints[0], leftPoints[1], new_vertex_02 },
			new Vector3[] { leftNormals[0], leftNormals[1], new_normal_02 },
			new Vector2[] { leftUvs[0], leftUvs[1], new_uv_02 }, new_normal_02);

		b_side.CreateTriangle(new Vector3[] { rightPoints[0], new_vertex_01, new_vertex_02 },
			new Vector3[] { rightNormals[0], new_normal_01, new_normal_02 },
			new Vector2[] { rightUvs[0], new_uv_01, new_uv_02 }, new_normal_01);

		b_side.CreateTriangle(new Vector3[] { rightPoints[0], rightPoints[1], new_vertex_02 },
			new Vector3[] { rightNormals[0], rightNormals[1], new_normal_02 },
			new Vector2[] { rightUvs[0], rightUvs[1], new_uv_02 }, new_normal_02);


	}

	private static void AddMeshMissing(Vector3[] cut_points)
	{
		var newVerticesOBJ = new GameObject("Vertices");


		var new_vertices = new List<Vector3>();
		var pursuer = new List<Vector3>();


		var center = Vector3.zero;

		for (var i = 0; i < cut_points.Length; i++)
		{
			if (!new_vertices.Contains(cut_points[i]))
			{
				new_vertices.Add(cut_points[i]);
				new_vertices.Add(cut_points[i + 1]);

				pursuer.Add(cut_points[i]);
				pursuer.Add(cut_points[i + 1]);

				Debug.Log("aa");

				var isDone = false;

				while (!isDone)
				{
					isDone = true;

					for (var r = 0; r < cut_points.Length; r += 2)
					{
						if (cut_points[r] == new_vertices[new_vertices.Count - 1] && !pursuer.Contains(cut_points[r + 1]))
						{
							isDone = false;
							new_vertices.Add(cut_points[r + 1]);
							pursuer.Add(cut_points[r + 1]);

						}
						else if (cut_points[r + 1] == new_vertices[new_vertices.Count - 1] && !pursuer.Contains(cut_points[r]))
						{

							isDone = false;
							new_vertices.Add(cut_points[r]);
							pursuer.Add(cut_points[r]);

						}

					}

				}

			}

		}

		foreach (Vector3 v in new_vertices)
		{
			center += v;

		}

		// 算術平均
		center = center / new_vertices.Count;

		for (var i = 0; i < new_vertices.Count; i++)
		{
			a_side.CreateTriangle(new Vector3[] { new_vertices[i], new_vertices[(i + 1) % new_vertices.Count], center },
								  new Vector3[] { new_vertices[i], new_vertices[(i + 1) % new_vertices.Count], center },
								  new Vector2[] { new_vertices[i], new_vertices[(i + 1) % new_vertices.Count], center },
								  center);

			b_side.CreateTriangle(new Vector3[] { new_vertices[i], new_vertices[(i + 1) % new_vertices.Count], center },
								  new Vector3[] { new_vertices[i], new_vertices[(i + 1) % new_vertices.Count], center },
								  new Vector2[] { new_vertices[i], new_vertices[(i + 1) % new_vertices.Count], center },
								  center);

		}


		//for (var i = 0; i < new_vertices.Count; i++)
		//{
		//	var vertexOBJ = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		//	vertexOBJ.transform.position = new_vertices[i];
		//	vertexOBJ.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
		//	vertexOBJ.name = "Vertex Object[" + i + "]";
		//	vertexOBJ.transform.parent = newVerticesOBJ.transform;

		//}

	}

	void Start()
	{

	}

	void Update()
	{


	}


}
