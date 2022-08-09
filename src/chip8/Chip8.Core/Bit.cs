using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8.Core
{
    public struct Bit
    {
        private int _value;

        public Bit() : this(0)
        { }

        public Bit(int value) => _value = value == 0 ? 0 : 1;

        public Bit(bool value) => _value = value ? 1 : 0;

        public int Value => _value;

        public bool BoolValue => _value != 0;

        public static implicit operator int (Bit bit) => bit._value;

        public static implicit operator Bit(int value) => new (value);

        public static implicit operator bool (Bit bit) => bit._value == 1;

        public static implicit operator Bit(bool value) => new (value);

        public static Bit operator &(Bit a, Bit b) => a._value & b._value;

        public static Bit operator |(Bit a, Bit b) => a._value | b._value;

        public static Bit operator ^(Bit a, Bit b) => a._value ^ b._value;

        public static Bit operator ~(Bit a) => !a;

        public static bool operator ==(Bit a, Bit b) => a.Value == b.Value;

        public static bool operator !=(Bit a, Bit b) => !(a == b);

        public override bool Equals(object? obj)
        {
            return obj is Bit bit &&
                   _value == bit._value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_value);
        }
    }
}
