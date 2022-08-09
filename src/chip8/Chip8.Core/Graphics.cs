using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8.Core
{
    internal sealed class Graphics
    {
        public const int WIDTH = 64;
        public const int HEIGHT = 32;

        private readonly Bit[,] _buffer = new Bit[WIDTH, HEIGHT];

        public void Clear()
        {
            for (var x = 0; x < WIDTH; x++)
                for (var y = 0; y < HEIGHT; y++)
                    _buffer[x, y] = 0;
        }

        public Bit GetXY(int x, int y) => _buffer[x, y];
        public void SetXY(int x, int y, Bit value)
        {
            if (_buffer[x, y] != value)
            {
                _buffer[x, y] = value;
            }
        }


    }
}
