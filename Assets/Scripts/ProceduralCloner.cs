using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralCloner : Object
{
    public enum Cubeside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };
    static string objName;
    static GameObject obj;
    static GameObject cube;
    static GameObject sphere;

    public static GameObject Clone(Vector3 pos, string cloneType)
    {
        if (obj == null)
        {
            switch (cloneType)
            {
                case "Cube":
                    cube = new GameObject();
                    obj = cube;
                    CreateCube(Vector3.zero);
                    break;
                case "Sphere":
                    sphere = new GameObject();
                    obj = sphere;
                    CreateSphere(Vector3.zero);
                    break;
            }
            obj.SetActive(false);
            objName = obj.name.ToString();

        }

        GameObject objClone = new GameObject();
        objClone.AddComponent<MeshFilter>();
        objClone.AddComponent<MeshRenderer>();
        objClone.GetComponent<MeshFilter>().mesh = obj.GetComponent<MeshFilter>().mesh;

        MeshRenderer rend = objClone.GetComponent<MeshRenderer>();
        rend.material = obj.GetComponent<MeshRenderer>().material;
        objClone.AddComponent<Rigidbody>();
        objClone.AddComponent<BoxCollider>();
        objClone.name = objName + "(Clone)";
        objClone.gameObject.SetActive(true);
        objClone.transform.position = pos;
        return objClone;
    }

    public static void CreateQuad(Cubeside side, GameObject parent)
    {
        Mesh mesh = new Mesh();
        mesh.name = "ScriptedMesh" + side.ToString();

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangles = new int[6];

        //all possible UVs
        Vector2 uv00 = new Vector2(0f, 0f);
        Vector2 uv10 = new Vector2(1f, 0f);
        Vector2 uv01 = new Vector2(0f, 1f);
        Vector2 uv11 = new Vector2(1f, 1f);

        //all possible vertices 
        Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);

        switch (side) //**
        {
            case Cubeside.BOTTOM:
                vertices = new Vector3[] { p0, p1, p2, p3 };
                normals = new Vector3[] {Vector3.down, Vector3.down,
                                            Vector3.down, Vector3.down};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.TOP:
                vertices = new Vector3[] { p7, p6, p5, p4 };
                normals = new Vector3[] {Vector3.up, Vector3.up,
                                            Vector3.up, Vector3.up};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.LEFT:
                vertices = new Vector3[] { p7, p4, p0, p3 };
                normals = new Vector3[] {Vector3.left, Vector3.left,
                                            Vector3.left, Vector3.left};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.RIGHT:
                vertices = new Vector3[] { p5, p6, p2, p1 };
                normals = new Vector3[] {Vector3.right, Vector3.right,
                                            Vector3.right, Vector3.right};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.FRONT:
                vertices = new Vector3[] { p4, p5, p1, p0 };
                normals = new Vector3[] {Vector3.forward, Vector3.forward,
                                            Vector3.forward, Vector3.forward};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case Cubeside.BACK:
                vertices = new Vector3[] { p6, p7, p3, p2 };
                normals = new Vector3[] {Vector3.back, Vector3.back,
                                            Vector3.back, Vector3.back};
                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

        GameObject quad = new GameObject("Quad");
        quad.transform.parent = parent.transform;
        MeshFilter meshFilter = quad.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }
    public static void CreateCube(Vector3 pos)
    {
        cube.AddComponent<MeshFilter>();
        cube.AddComponent<MeshRenderer>();
        CreateQuad(Cubeside.FRONT, cube);
        CreateQuad(Cubeside.BACK, cube);
        CreateQuad(Cubeside.TOP, cube);
        CreateQuad(Cubeside.BOTTOM, cube);
        CreateQuad(Cubeside.LEFT, cube);
        CreateQuad(Cubeside.RIGHT, cube);

        MeshFilter[] meshFilters = cube.GetComponentsInChildren<MeshFilter>();


        //remove any nulls
        List<MeshFilter> filters = new List<MeshFilter>();
        int i = 0;
        while (i < meshFilters.Length)
        {
            if (meshFilters[i].sharedMesh != null)
            {
                filters.Add(meshFilters[i]);
            }
            i++;
        }

        CombineInstance[] combine = new CombineInstance[filters.Count];
        i = 0;
        foreach (MeshFilter m in filters)
        {
            combine[i].mesh = m.mesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            m.gameObject.SetActive(false);
            i++;
        }

        cube.GetComponent<MeshFilter>().mesh = new Mesh();
        cube.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        cube.GetComponent<MeshFilter>().mesh.name = "CreatedCube";

        MeshRenderer rend = cube.GetComponent<MeshRenderer>();
        rend.material = new Material(Shader.Find("OD/Plasma"));

        cube.AddComponent<Rigidbody>();
        cube.AddComponent<BoxCollider>();
        cube.name = "Cube";
        cube.gameObject.SetActive(true);
        cube.transform.position = pos;
    }
    public static void CreateSphere(Vector3 pos)
    {
        sphere.AddComponent<MeshFilter>();
        sphere.AddComponent<MeshRenderer>();

        //Construct Sphere Mesh =======================
        Mesh mesh = new Mesh();
        mesh.name = "Sphere_" + Time.realtimeSinceStartup.ToString();
        mesh.Clear();

        float radius = 1f;
        int LONGITUDE = 20;
        int LATITUDE = 20;

        Vector3[] vertices = new Vector3[(LONGITUDE + 1) * LATITUDE + 2 * LONGITUDE];
        float PI2 = Mathf.PI * 2f;

        //NORTH POLE
        for (int v = 0; v <= LONGITUDE; v++)
        {
            vertices[v] = Vector3.up * radius;
        }

        for (int lat = 0; lat < LATITUDE; lat++)
        {
            float a1 = Mathf.PI * (float)(lat + 1) / (LATITUDE + 1);
            float sin1 = Mathf.Sin(a1);
            float cos1 = Mathf.Cos(a1);

            for (int lon = 0; lon <= LONGITUDE; lon++)
            {
                float a2 = PI2 * (float)(lon == LONGITUDE ? 0 : lon) / LONGITUDE;
                float sin2 = Mathf.Sin(a2);
                float cos2 = Mathf.Cos(a2);

                vertices[lon + lat * (LONGITUDE + 1) + LONGITUDE] = new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
            }
        }

        //SOUTH POLE
        for (int v = 1; v <= LONGITUDE; v++)
        {
            vertices[vertices.Length - v] = Vector3.up * -radius;   //last vertex - bottom most
        }

        Vector3[] normals = new Vector3[vertices.Length];
        for (int n = 0; n < vertices.Length; n++)
        {
            normals[n] = vertices[n].normalized;
        }

        Vector2[] uvs = new Vector2[vertices.Length];
        uvs[0] = Vector2.up;
        uvs[uvs.Length - 1] = Vector2.zero;
        for (int lat = 0; lat < LATITUDE; lat++)
            for (int lon = 0; lon <= LONGITUDE; lon++)
            {
                int uvpos = lon + lat * (LONGITUDE + 1) + LONGITUDE;
                uvs[uvpos] =
                        new Vector2((float)lon / LONGITUDE,
                            1f - (float)(lat + 1) / (LATITUDE + 1));

            }

        //top cap uvs
        float uvOffset = 1 / (float)LONGITUDE * 0.5f;
        for (int v = 0; v < LONGITUDE; v++)
        {
            uvs[v] = new Vector2(1 / (float)LONGITUDE * v + uvOffset, 1);
        }

        //bottom cap uvs
        int u = 0;
        for (int v = vertices.Length - LONGITUDE; v < vertices.Length; v++)
        {
            uvs[v] = new Vector2(1 / (float)LONGITUDE * u + uvOffset, 0);
            u++;
        }

        int totalFaces = vertices.Length;
        int totalTris = totalFaces * 2;
        int triIndexes = totalTris * 3;
        int[] triangles = new int[triIndexes];

        //Top Cap
        int i = 0;
        for (int lon = 0; lon <= LONGITUDE; lon++)
        {
            triangles[i++] = LONGITUDE + lon + 1;
            triangles[i++] = LONGITUDE + lon;
            triangles[i++] = lon;
        }

        //Middle
        for (int lat = 0; lat < LATITUDE - 1; lat++)
        {
            for (int lon = 0; lon < LONGITUDE; lon++)
            {
                int current = lon + lat * (LONGITUDE + 1) + LONGITUDE;
                int next = current + LONGITUDE + 1;

                triangles[i++] = current;
                triangles[i++] = current + 1;
                triangles[i++] = next + 1;

                triangles[i++] = current;
                triangles[i++] = next + 1;
                triangles[i++] = next;
            }
        }

        //Bottom Cap
        for (int lon = 0; lon <= LONGITUDE; lon++)
        {
            triangles[i++] = vertices.Length - 1 - lon;
            triangles[i++] = vertices.Length - LONGITUDE - (lon + 2);
            triangles[i++] = vertices.Length - LONGITUDE - (lon + 1);
        }


        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        //=========================================================

        sphere.GetComponent<MeshFilter>().mesh = mesh;

        MeshRenderer rend = sphere.GetComponent<MeshRenderer>();
        rend.material = new Material(Shader.Find("OD/Plasma"));
        sphere.AddComponent<Rigidbody>();
        sphere.AddComponent<SphereCollider>();
        sphere.name = "Sphere";
        sphere.gameObject.SetActive(true);
        sphere.transform.position = pos;
    }
}
