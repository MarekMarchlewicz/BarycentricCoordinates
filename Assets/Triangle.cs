using UnityEngine;

[ExecuteInEditMode]
public class Triangle : MonoBehaviour
{
    [SerializeField]
    private Transform vertex1, vertex2, vertex3;

    [SerializeField, Range(0f, 1f)]
    private float vertex1Value, vertex2Value, vertex3Value;

    private float vertex1PreviousValue, vertex2PreviousValue, vertex3PreviousValue;

    [SerializeField]
    private Transform marker;

    [SerializeField]
    private Color color;

    private bool HasVertices { get { return vertex1 && vertex2 && vertex3; } }
    
    private void OnEnable()
    {
        if(vertex1Value + vertex2Value + vertex3Value != 1f)
        {
            vertex1Value = vertex2Value = vertex3Value = 1f / 3f;
        }

        vertex1PreviousValue = vertex1Value;
        vertex2PreviousValue = vertex2Value;
        vertex3PreviousValue = vertex3Value;
    }

    private void Update()
    {
        if(vertex1Value != vertex1PreviousValue)
        {
            float sum = 1f - vertex1Value;

            if (sum > 0f)
            {
                float remainingSum = vertex2Value + vertex3Value;

                if (remainingSum > 0f)
                {
                    vertex2Value *= sum / remainingSum;
                    vertex3Value *= sum / remainingSum;
                }
                else
                {
                    vertex2Value = sum / 2f;
                    vertex3Value = sum / 2f;
                }
            }
            else
            {
                vertex2Value = 0f;
                vertex3Value = 0f;
            }
        }
        else if (vertex2Value != vertex2PreviousValue)
        {
            float sum = 1f - vertex2Value;

            if (sum > 0f)
            {
                float remainingSum = vertex1Value + vertex3Value;

                if (remainingSum > 0f)
                {
                    vertex1Value *= sum / remainingSum;
                    vertex3Value *= sum / remainingSum;
                }
                else
                {
                    vertex1Value = sum / 2f;
                    vertex3Value = sum / 2f;
                }
            }
            else
            {
                vertex1Value = 0f;
                vertex3Value = 0f;
            }
        }
        else if (vertex3Value != vertex3PreviousValue)
        {
            float sum = 1f - vertex3Value;

            if (sum > 0f)
            {
                float remainingSum = vertex1Value + vertex2Value;
                
                if (remainingSum > 0f)
                {
                    vertex1Value *= sum / remainingSum;
                    vertex2Value *= sum / remainingSum;
                }
                else
                {
                    vertex1Value = sum / 2f;
                    vertex2Value = sum / 2f;
                }
            }
            else
            {
                vertex1Value = 0f;
                vertex2Value = 0f;
            }
        }

        UpdateMarker();

        vertex1PreviousValue = vertex1Value;
        vertex2PreviousValue = vertex2Value;
        vertex3PreviousValue = vertex3Value;
    }

    private void UpdateMarker()
    {
        if (!marker)
            return;

        Vector3 markerPosition = vertex1Value * vertex1.position
            + vertex2Value * vertex2.position
            + vertex3Value * vertex3.position;

        marker.position = markerPosition;
    }

    private void OnDrawGizmos()
    {
        if (!HasVertices)
            return;

        Mesh mesh = new Mesh();

        Vector3 normal = Vector3.Cross((vertex2.position - vertex1.position).normalized, (vertex3.position - vertex1.position).normalized);

        mesh.vertices = new Vector3[3] { transform.InverseTransformPoint(vertex1.position),
            transform.InverseTransformPoint(vertex2.position),
            transform.InverseTransformPoint(vertex3.position) };
        mesh.triangles = new int[3] { 0, 1, 2 };
        mesh.normals = new Vector3[3] { normal, normal, normal };

        Gizmos.color = color;

        Gizmos.DrawMesh(mesh, transform.position, transform.rotation, transform.localScale);
    }
}
