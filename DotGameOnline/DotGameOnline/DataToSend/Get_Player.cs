using DotGameOnline.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotGameOnline
{
    public class Get_Player
    {
        string Name { get; set; } = "NoName";
        string PlayerID { get; set; }
        string Color { get; set; }
        public Get_Player(FinalPlayer player)
        {
            Name = PlayerID = player.ID;
            Color = player.GetColor().ToString(); 
        }
    }
}
