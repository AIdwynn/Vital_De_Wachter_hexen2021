using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAE.BoardSystem;
using DAE.ChessSystem;
using UnityEngine;

namespace DAE.GameSystem
{
    [CreateAssetMenu(menuName = "DAE/Position Helper")]
    class PositionHelper : ScriptableObject
    {
       
        [SerializeField]
        private int _tileDimensions;

        public (int x, int y) ToGridPosition(Grid<Position> grid, Transform parent, Vector3 worldPosition)
        {
            double qCalc = (Math.Sqrt(3) / 3 * worldPosition.x - 1 / 3 * worldPosition.y) / _tileDimensions;
            double rCalc = (2 / 3 * worldPosition.y) / _tileDimensions;

            int q = (int)Math.Round(qCalc);
            int r = (int)Math.Round(rCalc);
            //var relativePosition = worldPosition - parent.position;
            //var scaledRelativePosition = relativePosition / _tileDimensions;

            //var scaledBoardOffset = new Vector3(grid.Q / 2.0f, 0, grid.R / 2.0f);
            //var scaledHaldTileOffset = new Vector3(0.5f, 0, 0.5f);
            //scaledRelativePosition += scaledBoardOffset;
            //scaledRelativePosition -= scaledHaldTileOffset;

            //int q = (int)Math.Round(scaledRelativePosition.x);
            //int r = (int)Math.Round(scaledRelativePosition.z);

            return (q, r);
            
        }

        public Vector3 ToWorldPosition(Grid<Position> grid, Transform parent, int q, int r)
        {
            var x = _tileDimensions * (Math.Sqrt(3f) * q + Math.Sqrt(3f) / 2f * r);
            var y = _tileDimensions * (3f / 2f * r);
            var worldPosition = new Vector3((float)x, 0, y);

            //var scaledRelativePosition = new Vector3(q, 0, r);

            //var scaledHalfTileOffset = new Vector3(0.5f, 0, 0.5f);
            //scaledRelativePosition -= scaledHalfTileOffset;

            //var scaledBoardOffset = new Vector3(grid.Q / -2.0f, 0, grid.R / 2.0f);
            //scaledRelativePosition += scaledBoardOffset;

            //var relativePosition = scaledRelativePosition * _tileDimensions;
            //var worldPosition = relativePosition + parent.position;

            return worldPosition;
        }
    }
}
