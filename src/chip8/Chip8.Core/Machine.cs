using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8.Core
{
    public sealed class Machine
    {
        public const ushort FontTableLoadAddress = 0x50;

        public const ushort ProgramLoadAddress = 0x200;

        private static readonly byte[] DefaultFontArray = new byte[]
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

        private static readonly List<ConsoleKey> DefaultKeyMapping = new()
        {
            ConsoleKey.X,
            ConsoleKey.D1,
            ConsoleKey.D2,
            ConsoleKey.D3,
            ConsoleKey.Q,
            ConsoleKey.W,
            ConsoleKey.E,
            ConsoleKey.A,
            ConsoleKey.S,
            ConsoleKey.D,
            ConsoleKey.Z,
            ConsoleKey.C,
            ConsoleKey.D4,
            ConsoleKey.R,
            ConsoleKey.F,
            ConsoleKey.V
        };

        private readonly Bit[] _keys = new Bit[0x10];
        private readonly List<ConsoleKey> _keyMapping;
        private readonly Cpu _cpu;
        private readonly byte[] _fontArray;
        private readonly Graphics _graphics;
        private readonly Memory _memory;
        private readonly int _screenUpdateFrequency;
        private int _updateCounter;

        public Machine()
            : this(DefaultFontArray, DefaultKeyMapping, 600)
        {

        }

        public Machine(byte[] fontArray, List<ConsoleKey> keyMapping, int screenUpdateFrequency)
        {
            _fontArray = fontArray;
            _keyMapping = keyMapping;
            _memory = new Memory();
            _cpu = new Cpu(this);
            _graphics = new Graphics();
            _screenUpdateFrequency = screenUpdateFrequency;
        }

        public event EventHandler<GraphicsUpdatedEventArgs>? GraphicsUpdated;

        internal Graphics Graphics => _graphics;

        internal Memory Memory => _memory;

        internal Bit[] Keys => _keys;

        public void EmulateCycle()
        {
            _cpu.Tick();
            if ((_updateCounter % (_screenUpdateFrequency / 60)) == 0)
            {
                _cpu.UpdateTimer();
            }

            _updateCounter++;
        }

        public void Initialize()
        {
            Array.Copy(_fontArray, 0, _memory.Buffer, FontTableLoadAddress, _fontArray.Length);
            Array.Clear(_keys);
            _cpu.Reset();
        }

        public void LoadRom(string romFile)
        {
            using var fileStream = File.OpenRead(romFile);
            fileStream.Read(_memory.Buffer, ProgramLoadAddress, (int)fileStream.Length);
        }

        public void KeyDown(ConsoleKey consoleKey)
        {
            var keyIdx = _keyMapping.IndexOf(consoleKey);
            if (keyIdx > 0)
            {
                _keys[keyIdx] = 1;
            }
        }

        public void KeyUp(ConsoleKey consoleKey)
        {
            var keyIdx = _keyMapping.IndexOf(consoleKey);
            if (keyIdx > 0)
            {
                _keys[keyIdx] = 0;
            }
        }

        internal void UpdateGraphics()
        {
            GraphicsUpdated?.Invoke(this, new GraphicsUpdatedEventArgs(_graphics.Buffer));
        }

        private void Graphics_BufferChanged(object? sender, EventArgs e)
        {
            this.GraphicsUpdated?.Invoke(this, new GraphicsUpdatedEventArgs(_graphics.Buffer));
        }
    }
}
