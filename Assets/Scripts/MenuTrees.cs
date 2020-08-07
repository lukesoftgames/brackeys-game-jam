using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrees : MonoBehaviour
{

    public GameObject tree;
    ShapeCreator shapeCreator;
    public float radius = 1;
    public Vector3 regionSize = Vector2.one;
    public int rejectionSamples = 30;
    public float displayRadius = 1;

    List<Vector2> points;
    float top = -Mathf.Infinity, left = -Mathf.Infinity, bottom = Mathf.Infinity, right = Mathf.Infinity;

    // Start is called before the first frame update
    void Start()
    {
        shapeCreator = GetComponent<ShapeCreator>();
        foreach (Shape shape in shapeCreator.shapes)
        {
            top = -Mathf.Infinity;
            left = -Mathf.Infinity;
            bottom = Mathf.Infinity;
            right = Mathf.Infinity;
            // find the edges of the polygon
            foreach (Vector3 v in shape.points)
            {
                if (v.z > top)
                {
                    top = v.z;
                }
                if (v.z < bottom)
                {
                    bottom = v.z;
                }
                if (v.x > left)
                {
                    left = v.x;
                }
                if (v.x < right)
                {
                    right = v.x;
                }
            }
            points = PoissonDiscSampling.GeneratePoints(radius, new Vector2(left, top), new Vector2(right, bottom), shape, rejectionSamples);
            foreach (Vector2 point in points)
            {
                GameObject go = Instantiate(tree, new Vector3(point.x, 10f, point.y), Quaternion.identity);
                go.transform.parent = transform.parent;
            }

        }
    }


}
