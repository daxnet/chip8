using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8.Core
{
    public sealed class Machine
    {
        public static readonly byte[] DefaultFontArray = new byte[]
        {
            0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
            0x20, 0x60, 0x20, 0x20, 0x70, // 1
            0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
            0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
            0x90, 0x90, 0xF0, 0x10, 0x10, // 4
            0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
            0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
            0xF0, 0x10, 0x20, 0x40, 0x40, // 7
            0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
            0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
            0xF0, 0x90, 0xF0, 0x90, 0x90, // A
            0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
            0xF0, 0x80, 0x80, 0x80, 0xF0, // C
            0xE0, 0x90, 0x90, 0x90, 0xE0, // D
            0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
            0xF0, 0x80, 0xF0, 0x80, 0x80  // F
        };

        public const ushort FontTableLoadAddress = 0x50;
        public const ushort ProgramLoadAddress = 0x200;

        private readonly Memory _memory;
        private readonly Cpu _cpu;
        private readonly Graphics _graphics;
        private readonly byte[] _fontArray;

        public Machine()
            : this(DefaultFontArray)
        {

        }

        public Machine(byte[] fontArray)
        {
            _fontArray = fontArray;
            _memory = new Memory();
            _cpu = new Cpu(this);
            _graphics = new Graphics();
        }

        public void Initialize()
        {
            Array.Copy(_fontArray, 0, _memory.Buffer, FontTableLoadAddress, _fontArray.Length);
            _cpu.Reset();
        }

        public void LoadRom(string romFile)
        {
            using var fileStream = File.OpenRead(romFile);
            fileStream.Read(_memory.Buffer, ProgramLoadAddress, (int)fileStream.Length);
        }

        public void Run()
        {
            while(true)
            {
                _cpu.Cycle();
            }
        }

        internal Memory Memory => _memory;
    }
}
