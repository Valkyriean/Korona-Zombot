using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GeneratedBulletHole : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public int xSize = 100;
    public int zSize = 100;
    private float descendWidth;
    private float descendSteepness;
    private float floorHeight;
    private float noise_freq;
    private float noise_ampl;

    Texture2D texture;
    Vector2[] uvs;
    // Start is called before the first frame update
    void Start()
    {
        // Randomly generate parameters for making the mesh
        descendWidth = Random.Range(3f, 6f);
        descendSteepness = Random.Range(0.5f, 2f);
        floorHeight = Random.Range(0.01f, 2f);
        noise_freq = Random.Range(0.09f, 0.11f);
        noise_ampl = Random.Range(2.5f, 3.5f);
        mesh = new Mesh();
        texture = new Texture2D(xSize+1, zSize +1);
        GetComponent<Renderer>().material.mainTexture = texture;
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
        Material material = GetComponent<Renderer>().material;
        ToFadeMode(material);
        texture.Apply();
    }


    // Trun rendering mode into fade, code from https://forum.unity.com/threads/change-rendering-mode-via-script.476437/
    public void ToFadeMode(Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
    }


    void CreateShape(){
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        float maxY = 0f;

        // Determain the y value at every given x and z coordinate
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                // Calculate the radius from point to central
                float r = Mathf.Sqrt ( Mathf.Pow((x - (xSize/2)), 2) + Mathf.Pow((z - (zSize/2)), 2));
                float noise = (Mathf.PerlinNoise(x * noise_freq, z * noise_freq)-0.5f) * noise_ampl ;

                float y = AssemblyShape(r*0.1f) + noise;

                if ( r > xSize/2)
                {
                    y = 0.01f;
                }
                if (y < 0.01f)
                {
                    y = 0.01f;
                }
                if (y > maxY)
                {
                    maxY = y;
                }
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }
        
        // Build the texture for mesh
        uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 point = vertices[i];
            float ratio = Mathf.Clamp((point.y/maxY), 0, 1);
            float invRatio = Mathf.Clamp((1-ratio)-0.7f,0,1);
            ratio = Mathf.Clamp(ratio+0.5f,0f,1f);
            // Make corners outside circle transparent
            if (point.y < 0.02f) ratio = 0f;
            // Set color based on height
            Color c = new Color(invRatio,invRatio,invRatio, ratio);
            texture.SetPixel((int)point.x, (int)point.z, c);
            uvs[i] = new Vector2(vertices[i].x/xSize, vertices[i].z/zSize);
        }


        // Group the vertices into triangles.
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
            triangles[tris + 0] = vert + 0;
            triangles[tris + 1] = vert + xSize + 1;
            triangles[tris + 2] = vert + 1;
            triangles[tris + 3] = vert + 1;
            triangles[tris + 4] = vert + xSize + 1;
            triangles[tris + 5] = vert + xSize + 2;
            vert++;
            tris += 6;
            }
            vert++;
        }
    }

    
    void UpdateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.uv = uvs;
    }

    // The floor nearest to the central point
    float CentralFloorShape() {
        return floorHeight;
    }

    // Ascend from central floor
    float AscendShape(float r) {
        return r * r -1;
    }

    // Descend after Ascend back to 0 height
    float DescendShape(float r) {
        float x = Mathf.Abs(r) - 1 -  descendWidth;
        return  descendSteepness * x * x;
    }

    // Assembly three shape into one
    float AssemblyShape(float r) {
        // Ignore the corners of circle in square mesh
        if (r > (descendWidth + 1))
        {
            return 0f;
        }
        return Mathf.Max(Mathf.Min(AscendShape(r), DescendShape(r)), CentralFloorShape());
    }
}
