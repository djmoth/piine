using System;
using System.Collections.Generic;
using System.Text;

namespace piine
{
    public struct Ray
    {
        public Float3 Origin { get; set; }
        public Float3 Direction { get; set; }
        
        public static bool operator == (Ray left, Ray right) => left.Origin == right.Origin && left.Direction == right.Direction;

        public static bool operator != (Ray left, Ray right) => left.Origin != right.Origin || left.Direction != right.Direction;

        public override bool Equals (object obj)
        {
            Ray? v = obj as Ray?;

            if (v != null)
                return v == this;
            else
                return false;
        }
    }
}
