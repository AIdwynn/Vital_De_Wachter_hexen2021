using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class GridGeneration : MonoBehaviour
{
    [SerializeField]
    private GameObject HexPrefab;
    [SerializeField]
    private int _boardRadius;
    [SerializeField]
    private bool _generate = false;
    [SerializeField]
    private bool _destroy = false;

    private void Update()
    {
        if (_generate)
        {
            GenerateBoard();
        }
        if(_destroy)
        {
            DestroyBoard();
        }
    }

    public void GenerateBoard()
    {
        for (int q = -_boardRadius; q <= _boardRadius; q++)
        {
            int r1 = Mathf.Max(-_boardRadius, -q -_boardRadius);
            int r2 = Mathf.Min(_boardRadius, -q + _boardRadius);
            for (int r = r1; r <= r2; r++)
            {
                Vector2 point = HexToPixel(q, r);
                var hex = Instantiate(HexPrefab, new Vector3(point.x, 0, point.y), Quaternion.identity);
                hex.transform.parent = gameObject.transform;
            }
        }
        _generate = false;
    }
    private void DestroyBoard()
    {
        for (int i = gameObject.transform.childCount -1; i >= 0; i--)
        {
            if(gameObject.transform.GetChild(i) != null) 
            {
                DestroyImmediate(gameObject.transform.GetChild(i).gameObject);
            }
        }
        _destroy = false;
    }

    private Vector2 HexToPixel(int q, int r)
    {
        var x = Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f)/2f * r;
        var y = 3f/2f  * r;
        return new Vector2(x, y);
    }
}
