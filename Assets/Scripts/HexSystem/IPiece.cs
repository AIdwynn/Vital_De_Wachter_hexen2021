using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAE.HexSystem
{
    public interface IPiece
    {
        int PlayerID { get; }
        bool Moved { get; set; }

        
        CardType Type { get; }

    }
}
