namespace Chip8.Core
{
    internal sealed class Cpu
    {
        #region Private Fields

        private static readonly Random _rnd = new(DateTime.Now.Millisecond);
        private readonly Machine _machine;
        private readonly List<Action<int>> _opcodeExecCallbacks;
        private readonly ushort[] _stack = new ushort[0x10];
        private readonly byte[] _v = new byte[0x10];
        private byte _delayTimer;
        private ushort _i;
        private ushort _pc;
        private byte _soundTimer;
        private ushort _sp;

        #endregion Private Fields

        #region Public Constructors

        public Cpu(Machine machine)
        {
            _machine = machine;
            _opcodeExecCallbacks = new()
            {
                OpCode_0, OpCode_1, OpCode_2, OpCode_3, OpCode_4, OpCode_5, OpCode_6, OpCode_7,
                OpCode_8, OpCode_9, OpCode_A, OpCode_B, OpCode_C, OpCode_D, OpCode_E, OpCode_F,
            };
        }

        #endregion Public Constructors

        #region Public Methods

        public void Reset()
        {
            _pc = Machine.ProgramLoadAddress;
            _i = 0;
            _sp = 0;
            _delayTimer = 0;
            _soundTimer = 0;
        }

        public void Tick()
        {
            var instruction = (_machine.Memory.Buffer[_pc] << 8) | (_machine.Memory.Buffer[_pc + 1]);
            var opcode = (instruction & 0xf000) >> 12;

            _pc += 2;

            var opcodeProc = _opcodeExecCallbacks[opcode];
            opcodeProc(instruction);
        }

        public void UpdateTimer()
        {
            if (_delayTimer > 0)
                _delayTimer--;
            if (_soundTimer > 0)
                _soundTimer--;
        }

        #endregion Public Methods

        #region Private Methods

        private void OpCode_0(int instruction)
        {
            switch (instruction)
            {
                case 0xe0:
                    _machine.Graphics.Clear();
                    _machine.UpdateGraphics();
                    break;

                case 0xee:
                    --_sp;
                    _pc = _stack[_sp];
                    break;
            }
        }

        private void OpCode_1(int instruction)
        {
            var address = (ushort)(instruction & 0x0fff);
            _pc = address;
        }

        private void OpCode_2(int instruction)
        {
            var address = (ushort)(instruction & 0x0fff);
            _stack[_sp] = _pc;
            ++_sp;
            _pc = address;
        }

        private void OpCode_3(int instruction)
        {
            var r = (instruction & 0x0f00) >> 8;
            var n = instruction & 0x00ff;
            if (_v[r] == n)
            {
                _pc += 2;
            }
        }

        private void OpCode_4(int instruction)
        {
            var r = (instruction & 0x0f00) >> 8;
            var n = instruction & 0x00ff;
            if (_v[r] != n)
            {
                _pc += 2;
            }
        }

        private void OpCode_5(int instruction)
        {
            var rx = (instruction & 0x0f00) >> 8;
            var ry = (instruction & 0x00f0) >> 4;
            if (_v[rx] == _v[ry])
            {
                _pc += 2;
            }
        }

        private void OpCode_6(int instruction)
        {
            var r = (instruction & 0x0f00) >> 8;
            var n = (byte)(instruction & 0x00ff);
            _v[r] = n;
        }

        private void OpCode_7(int instruction)
        {
            var r = (instruction & 0x0f00) >> 8;
            var n = (byte)(instruction & 0x00ff);
            _v[r] += n;
        }

        private void OpCode_8(int instruction)
        {
            var op = instruction & 0x000f;
            var rx = (instruction & 0x0f00) >> 8;
            var ry = (instruction & 0x00f0) >> 4;

            switch (op)
            {
                case 0:
                    _v[rx] = _v[ry];
                    break;

                case 1:
                    _v[rx] |= _v[ry];
                    break;

                case 2:
                    _v[rx] &= _v[ry];
                    break;

                case 3:
                    _v[rx] ^= _v[ry];
                    break;

                case 4:
                    var sum = _v[rx] + _v[ry];
                    _v[0xf] = sum > 0xff ? (byte)1 : (byte)0;
                    _v[rx] = (byte)(sum & 0xff);
                    break;

                case 5:
                    _v[0xf] = _v[rx] > _v[ry] ? (byte)1 : (byte)0;
                    _v[rx] -= _v[ry];
                    break;

                case 6:
                    _v[0xf] = (byte)(_v[rx] & 0x1);
                    _v[rx] >>= 1;
                    break;

                case 7:
                    _v[0xf] = _v[rx] > _v[ry] ? (byte)0 : (byte)1;
                    _v[rx] = (byte)(_v[ry] - _v[rx]);
                    break;

                case 0x0e:
                    _v[0xf] = (byte)((_v[rx] & 0x80) >> 7);
                    _v[rx] <<= 1;
                    break;

                default: break;
            }
        }

        private void OpCode_9(int instruction)
        {
            var rx = (instruction & 0x0f00) >> 8;
            var ry = (instruction & 0x00f0) >> 4;
            if (_v[rx] != _v[ry])
            {
                _pc += 2;
            }
        }

        private void OpCode_A(int instruction)
        {
            var address = instruction & 0x0fff;
            _i = (ushort)address;
        }

        private void OpCode_B(int instruction)
        {
            var address = instruction & 0x0fff;
            _pc = (ushort)(_v[0] + address);
        }

        private void OpCode_C(int instruction)
        {
            var rand = _rnd.Next(256);
            var rx = (instruction & 0x0f00) >> 8;
            var n = instruction & 0x00ff;
            _v[rx] = (byte)(rand & n);
        }

        private void OpCode_D(int instruction)
        {
            var rx = (instruction & 0x0f00) >> 8;
            var ry = (instruction & 0x00f0) >> 4;
            var height = instruction & 0x000f;
            byte pixel;

            _v[0xf] = 0;
            for (var row = 0; row < height; row++)
            {
                var y = (_v[ry] + row) % Graphics.HEIGHT;
                pixel = _machine.Memory.Buffer[_i + row];
                for (var col = 0; col < 8; col++)
                {
                    var x = (_v[rx] + col) % Graphics.WIDTH;
                    Bit bit = pixel & (0x80 >> col);
                    var valXY = _machine.Graphics.GetXY(x, y);
                    if (bit)
                    {
                        if (valXY)
                        {
                            _v[0xf] = 1;
                        }
                    }
                    _machine.Graphics.SetXY(x, y, valXY ^ bit);
                    _machine.UpdateGraphics();
                }
            }
        }

        private void OpCode_E(int instruction)
        {
            var r = (instruction & 0x0f00) >> 8;
            var c = instruction & 0x00ff;
            switch (c)
            {
                case 0x9e:
                    if (_machine.Keys[_v[r]])
                    {
                        _pc += 2;
                    }
                    break;

                case 0xa1:
                    if (!_machine.Keys[_v[r]])
                    {
                        _pc += 2;
                    }
                    break;

                default: break;
            }
        }

        private void OpCode_F(int instruction)
        {
            var r = (instruction & 0x0f00) >> 8;
            var c = instruction & 0x00ff;
            switch (c)
            {
                case 0x7:
                    _v[r] = _delayTimer;
                    break;

                case 0xa:
                    var pressedKeysCount = _machine.Keys.Count(k => k);
                    switch (pressedKeysCount)
                    {
                        case 0:
                            _pc -= 2;
                            break;

                        case 1:
                            var idx = Array.IndexOf(_machine.Keys, true);
                            _v[r] = (byte)idx;
                            break;
                    }
                    break;

                case 0x15:
                    _delayTimer = _v[r];
                    break;

                case 0x18:
                    _soundTimer = _v[r];
                    break;

                case 0x1e:
                    _i += _v[r];
                    break;

                case 0x29:
                    var d = _v[r];
                    _i = (ushort)(Machine.FontTableLoadAddress + (5 * d));
                    break;

                case 0x33:
                    var val = _v[r];
                    _machine.Memory.Buffer[_i + 2] = (byte)(val % 10);
                    val /= 10;
                    _machine.Memory.Buffer[_i + 1] = (byte)(val % 10);
                    val /= 10;
                    _machine.Memory.Buffer[_i] = (byte)(val % 10);
                    break;

                case 0x55:
                    for (var i = 0; i <= r; i++)
                    {
                        _machine.Memory.Buffer[_i + i] = _v[i];
                    }
                    break;

                case 0x65:
                    for (var i = 0; i <= r; i++)
                    {
                        _v[i] = _machine.Memory.Buffer[_i + i];
                    }
                    break;
            }
        }

        #endregion Private Methods
    }
}