using System.Text;

namespace Chip8.Core
{
    internal sealed class Graphics
    {
        #region Public Fields

        public const int HEIGHT = 32;
        public const int WIDTH = 64;

        #endregion Public Fields

        #region Private Fields

        private readonly Bit[,] _buffer = new Bit[WIDTH, HEIGHT];

        #endregion Private Fields

        #region Public Properties

        public string DebugString
        {
            get
            {
                var sb = new StringBuilder();
                for (var x = 0; x < WIDTH; x++)
                    for (var y = 0; y < HEIGHT; y++)
                    {
                        sb.Append(_buffer[x, y].ToString());
                        if (y == HEIGHT - 1)
                            sb.Append(Environment.NewLine);
                    }
                return sb.ToString();
            }
        }

        #endregion Public Properties

        #region Internal Properties

        internal Bit[,] Buffer => _buffer;

        #endregion Internal Properties

        #region Public Methods

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

        #endregion Public Methods
    }
}