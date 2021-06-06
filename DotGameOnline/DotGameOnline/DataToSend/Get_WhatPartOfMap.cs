using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotGameOnline
{
    public class Get_WhatPartOfMap
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Get_WhatPartOfMap()
        {
            X = Y = 0;
            Width = 32;
            Height = 18;
        }
        public Get_WhatPartOfMap(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
