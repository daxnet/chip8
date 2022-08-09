using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8.Core
{
    internal sealed class Cpu
    {
        private ushort _pc;
        private ushort _i;
        private ushort _sp;
        private readonly ushort[] _stack = new ushort[0xf];
        private readonly byte[] _v = new byte[0xf];
        private byte _delayTimer;
        private byte _soundTimer;

        private readonly Machine _machine;
        private static readonly Random _rnd = new(DateTime.Now.Millisecond);

        private readonly List<Action<int>> _opcodeExecCallbacks;

        public Cpu(Machine machine)
        {
            _machine = machine;
            _opcodeExecCallbacks = new()
            {
                OpCode_0,
                OpCode_1,
                OpCode_2,
                OpCode_3,
                OpCode_4,
                OpCode_5,
                OpCode_6,
                OpCode_7,
                OpCode_8,
                OpCode_9,
                OpCode_A,
                OpCode_B,
                OpCode_C,
                OpCode_D,
                OpCode_E,
                OpCode_F,
            };
        }

        public void Reset()
        {
            _pc = Machine.ProgramLoadAddress;
            _i = 0;
            _sp = 0;
            _delayTimer = 0;
            _soundTimer = 0;
        }

        public void Cycle()
        {
            var instruction = (_machine.Memory.Buffer[_pc] << 8) | (_machine.Memory.Buffer[_pc + 1]);
            var opcode = (instruction & 0xf000) >> 12;

            _pc += 2;
            
            var opcodeProc = _opcodeExecCallbacks[opcode];
            opcodeProc(instruction);

            if (_delayTimer > 0)
            {
                _delayTimer--;
            }

            if (_soundTimer > 0)
            {
                _soundTimer--;
            }
        }

        private void OpCode_0(int instruction)
        {
            switch (instruction)
            {
                case 0xe0:
                    _machine.Graphics.Clear();
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
                    _v[0xf] = _v[rx] > _v[ry] ? (byte)1: (byte)0;
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
            _i = (byte)address;
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

            var x = _v[rx] % Graphics.WIDTH;
            var y = _v[ry] % Graphics.HEIGHT;
            _v[0xf] = 0;
            for (var row = 0; row < height; row++)
            {
                pixel = _machine.Memory.Buffer[_i + row];
                for (var col = 0; col < 8; col++)
                {
                    Bit bit = pixel & (0x80 >> col);
                    if (bit)
                    {
                        var valXY = _machine.Graphics.GetXY(x, y);
                        if (valXY)
                        {
                            _v[0xf] = 1;
                        }

                        _machine.Graphics.SetXY(x, y, valXY ^ 1);
                    }
                }
            }
        }

        private void OpCode_E(int instruction)
        {

        }

        private void OpCode_F(int instruction)
        {

        }
    }
}
