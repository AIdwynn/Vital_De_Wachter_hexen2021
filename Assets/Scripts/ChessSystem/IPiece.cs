using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAE.ChessSystem
{
    public interface IPiece
    {
        int PlayerID { get; }
        bool Moved { get; set; }
        PieceType PieceType { get; }

    }
}
