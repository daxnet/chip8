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
        private readonly ushort[] _stack = new ushort[16];
        private readonly byte[] _v = new byte[0xf];
        private byte _delayTimer;
        private byte _soundTimer;

        private readonly Machine _machine;

        public Cpu(Machine machine) => _machine = machine;

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
            if (_delayTimer > 0)
            {
                _delayTimer--;
            }

            if (_soundTimer > 0)
            {
                _soundTimer--;
            }
        }

    }
}
