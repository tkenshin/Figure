using UnityEngine;
using System.Collections.Generic;

public class CutObject
{
    private static CutMeshFace leftFace = new CutMeshFace();
    private static CutMeshFace rightFace = new CutMeshFace();

    private static Plane cutPlane;
    private static Mesh targetMesh;

    private static List<Vector3> newVertices = new List<Vector3>();

	private class CutMeshFace
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


        public void AddTriangle(int point01, int point02, int point03)
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

        public void AddTriangle(Vector3[] three_points, Vector3[] three_normals, Vector2[] three_uvs, Vector3 faceNormal)
        {
            Vector3 CrossProductNormal = Vector3.Cross((three_points[1] - three_points[0]).normalized, (three_points[2] - three_points[0]).normalized);

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
		var vertices = new GameObject("Vertices");

        Vector3[] cutPoints = SetCutPoints.cutPoints.ToArray();

        cutPlane = new Plane(cutPoints[0], cutPoints[1], cutPoints[2]);

        targetMesh = target.GetComponent<MeshFilter>().mesh;

        newVertices.Clear();
        leftFace.InitializeAll();
        rightFace.InitializeAll();

        bool[] sides = new bool[3];
        int point01, point02, point03;
        int[] indices;

        for (var r = 0; r < targetMesh.vertices.Length; r++)
        {
			var sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sp.transform.position = targetMesh.vertices[r];
            sp.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            sp.name = "Sphere[" + r + "]";
			sp.transform.parent = vertices.transform;

        }

        for (var v = 0; v < targetMesh.subMeshCount; v++)
        {
            // indices.Length = 36(0 ~ 35);
            // indicesにはCube(対象)の三角メッシュのIndex情報が入る。
            indices = targetMesh.GetIndices(v);

            // 36 / 3  = 12回繰り返す
            for (var i = 0; i < indices.Length; i += 3)
            {
                // 三角メッシュ(triangles)のパターン(0, 1, 2 ...)を各point01 ~ 03に入れる。
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
                        leftFace.AddTriangle(point01, point02, point03);

                    }
                    else
                    {
                        rightFace.AddTriangle(point01, point02, point03);

                    }

                }
                else
                {
                    CutFace(sides, point01, point02, point03);

                }
            }
        }

        Mesh leftMesh = new Mesh();
        leftMesh.vertices = leftFace.vertices.ToArray();
        leftMesh.triangles = leftFace.triangles.ToArray();
        leftMesh.normals = leftFace.normals.ToArray();
        leftMesh.uv = leftFace.uvs.ToArray();

        Mesh rightMesh = new Mesh();
        rightMesh.vertices = rightFace.vertices.ToArray();
        rightMesh.triangles = rightFace.triangles.ToArray();
        rightMesh.normals = rightFace.normals.ToArray();
        rightMesh.uv = rightFace.uvs.ToArray();


        target.GetComponent<MeshFilter>().mesh = leftMesh;
		Material[] materials = target.GetComponent<MeshRenderer>().sharedMaterials;

		Material[] newMaterials = new Material[materials.Length + 1];
		materials.CopyTo(newMaterials, 0);
		newMaterials[materials.Length] = material;
		materials = newMaterials;


		GameObject leftOBJ = target;

        GameObject rightOBJ = new GameObject("RightOBJ", typeof(MeshFilter), typeof(MeshRenderer));
        rightOBJ.transform.position = target.transform.position;
        rightOBJ.transform.rotation = target.transform.rotation;
        rightOBJ.GetComponent<MeshFilter>().mesh = rightMesh;

		leftOBJ.GetComponent<MeshRenderer>().materials = materials;
		rightOBJ.GetComponent<MeshRenderer>().materials = materials;

        return new GameObject[] { leftOBJ, rightOBJ };
    }

    private static void CutFace(bool[] sides, int index01, int index02, int index03)
    {
		var newVert = new GameObject("NewVertices");

        Vector3[] leftPoints = new Vector3[2];
        Vector3[] leftNormals = new Vector3[2];
        Vector2[] leftUvs = new Vector2[2];
        Vector3[] rightPoints = new Vector3[2];
        Vector3[] rightNormals = new Vector3[2];
        Vector2[] rightUvs = new Vector2[2];

        var didset_left = false;
        var didset_right = false;

        var p = index01;

        for (var i = 0; i < 3; i++)
        {

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

            if (sides[i])
            {
                if (!didset_left)
                {

                    didset_left = true;
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
                if (!didset_right)
                {

                    didset_right = true;

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

        var normalizedDistance = 0.0f;
        var distance = 0.0f;

        cutPlane.Raycast(new Ray(leftPoints[0], (rightPoints[0] - leftPoints[0]).normalized), out distance);

        normalizedDistance = distance / (rightPoints[0] - leftPoints[0]).magnitude;

        Vector3 newVertex1 = Vector3.Lerp(leftPoints[0], rightPoints[0], normalizedDistance);

        Vector2 newUv1 = Vector2.Lerp(leftUvs[0], rightUvs[0], normalizedDistance);

        Vector3 newNormal1 = Vector3.Lerp(leftNormals[0], rightNormals[0], normalizedDistance);

        newVertices.Add(newVertex1);

        cutPlane.Raycast(new Ray(leftPoints[1], (rightPoints[1] - leftPoints[1]).normalized), out distance);

        normalizedDistance = distance / (rightPoints[1] - leftPoints[1]).magnitude;
        Vector3 newVertex2 = Vector3.Lerp(leftPoints[1], rightPoints[1], normalizedDistance);

        Vector2 newUv2 = Vector2.Lerp(leftUvs[1], rightUvs[1], normalizedDistance);
        Vector3 newNormal2 = Vector3.Lerp(leftNormals[1], rightNormals[1], normalizedDistance);

        newVertices.Add(newVertex2);



		var ray = new Ray(leftPoints[0], (rightPoints[0] - leftPoints[0]).normalized);
		Debug.DrawRay(ray.origin, ray.direction, Color.green, 2000.0f);

		var ray02 = new Ray(leftPoints[1], (rightPoints[1] - leftPoints[1]).normalized);
		Debug.DrawRay(ray02.origin, ray02.direction, Color.red, 2000.0f);

		GameObject newVerSp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		newVerSp.transform.position = newVertex1;
		newVerSp.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
		newVerSp.name = "NEW_VERTEX1";
		newVerSp.transform.parent = newVert.transform;

		GameObject newVerSp2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		newVerSp2.transform.position = newVertex2;
		newVerSp2.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
		newVerSp2.name = "NEW_VERTEX2";
		newVerSp2.transform.parent = newVert.transform;



        leftFace.AddTriangle(new Vector3[] { leftPoints[0], newVertex1, newVertex2 },
            new Vector3[] { leftNormals[0], newNormal1, newNormal2 },
            new Vector2[] { leftUvs[0], newUv1, newUv2 }, newNormal1);

        leftFace.AddTriangle(new Vector3[] { leftPoints[0], leftPoints[1], newVertex2 },
            new Vector3[] { leftNormals[0], leftNormals[1], newNormal2 },
            new Vector2[] { leftUvs[0], leftUvs[1], newUv2 }, newNormal2);

        rightFace.AddTriangle(new Vector3[] { rightPoints[0], newVertex1, newVertex2 },
            new Vector3[] { rightNormals[0], newNormal1, newNormal2 },
            new Vector2[] { rightUvs[0], newUv1, newUv2 }, newNormal1);

        rightFace.AddTriangle(new Vector3[] { rightPoints[0], rightPoints[1], newVertex2 },
            new Vector3[] { rightNormals[0], rightNormals[1], newNormal2 },
            new Vector2[] { rightUvs[0], rightUvs[1], newUv2 }, newNormal2);

    }

    void Start()
    {

    }

    void Update()
    {
		

    }
}
