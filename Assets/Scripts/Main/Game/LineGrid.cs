using UnityEngine;

[ExecuteInEditMode]
public class LineGrid : MonoBehaviour
{
    [SerializeField] private Vector2 scale = new Vector2(10f, 10f);
    [SerializeField] private Vector2Int division = new Vector2Int(10, 10);
    [SerializeField] private Material material;

    private void OnRenderObject()
    {
        Vector2 stepSize = scale / division;
        Vector2 halfScale = scale * 0.5f;
        material.SetPass(0);
        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        for (int x = 0; x <= division.x; x++)
        {
            GL.Begin(GL.LINES);
            for (int z = 0; z < division.y; z++)
            {
                GL.Vertex(new Vector3(x * stepSize.x - halfScale.x, 0f, z * stepSize.y - halfScale.y));
                GL.Vertex(new Vector3(x * stepSize.x - halfScale.x, 0f, (z + 1) * stepSize.y - halfScale.y));
            }
            GL.End();
        }
        for (int z = 0; z <= division.y; z++)
        {
            GL.Begin(GL.LINES);
            for (int x = 0; x < division.x; x++)
            {
                GL.Vertex(new Vector3(x * stepSize.x - halfScale.x, 0f, z * stepSize.y - halfScale.y));
                GL.Vertex(new Vector3((x + 1) * stepSize.x - halfScale.x, 0f, z * stepSize.y - halfScale.y));
            }
            GL.End();
        }
        GL.PopMatrix();
    }
}
