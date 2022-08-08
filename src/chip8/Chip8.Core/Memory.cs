namespace Chip8.Core
{
    internal sealed class Memory
    {
        public const int Size = 4096; // 4KB memory
        private readonly byte[] _buffer = new byte[Size];

        //public byte Get(int position) => _buffer[position];

        //public void Set(int position, byte value) => _buffer[position] = value;

        //public void BlockSet(byte[] sourceValues, int start, int length)
        //    => Array.Copy(sourceValues, 0, _buffer, start, length);

        public byte[] Buffer => _buffer;
    }
}