using System;
using DAE.Commons;


namespace DAE.BoardSystem
{
    public class Grid<TPosition>
    {
        public int R { get; }
        public int Q { get; }

        public TPosition PlayerPos { get; set; }

        public Grid(int rows, int columns)
        {
            R = rows;
            Q = columns;

        }

        private BidirectionalDictionary<(int q, int r), TPosition> _positions = new BidirectionalDictionary<(int, int), TPosition>();

        public bool TryGetPositionAt(int q, int r, out TPosition position)
           => _positions.TryGetValue((q, r), out position);

        public bool TryGetCoordinateOf(TPosition position, out (int q, int r) coordinate)
            => _positions.TryGetKey(position, out coordinate);
        public void Register(int rank, int file, TPosition position)
        {
            if (rank <= -Q || rank >= Q)
                throw new ArgumentException($"{nameof(rank)}: {rank}");

            if (file <= -R || file >= R)
                throw new ArgumentException($"{nameof(file)}: {file}");

            _positions.Add((rank, file), position);
        }
    }
}
