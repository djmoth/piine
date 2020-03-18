using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Memory.Unsafe
{
    public interface IUnsafeObject
    {
        int Size { get; }

        void Alloc ();
        void Free ();
    }
}
