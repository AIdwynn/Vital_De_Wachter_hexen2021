using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAE.BoardSystem;
using DAE.HexSystem;
using UnityEngine;

namespace DAE.GameSystem
{
    [CreateAssetMenu(menuName = "DAE/Position Helper")]
    class PositionHelper : ScriptableObject
    {
       
        [SerializeField]
        private float _tileDimensions;

        public (int x, int y) ToGridPosition(Grid<Position> grid, Transform parent, Vector3 worldPosition)
        {
            double qCalc = (Math.Sqrt(3) / 3 * worldPosition.x - 1 / 3f * worldPosition.z) / (_tileDimensions);
            double rCalc = (2 / 3f * worldPosition.z) / (_tileDimensions);

            int q = (int)Math.Round(qCalc);
            int r = (int)Math.Round(rCalc);

            return (q, r);
            
        }

        public Vector3 ToWorldPosition(Grid<Position> grid, Transform parent, (int q, int r) coordinate)
        {
            var x = _tileDimensions * (Math.Sqrt(3f) * coordinate.q + Math.Sqrt(3f) / 2f * coordinate.r);
            var y = _tileDimensions * (3f / 2f * coordinate.r);
            var worldPosition = new Vector3((float)x, 0, y);

            return worldPosition;
        }
    }
}
