using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Memory
{
    public static class ObjectPool<T> where T : new ()
    {
        private static readonly Stack<T> pool = new Stack<T> (8);

        public static T GetObject ()
        {
            lock (pool)
            {
                if (pool.Count > 0)
                    return pool.Pop ();
                else
                    return new T ();
            }
        }

        public static void ReturnObject (T obj)
        {
            lock (pool)
            {
                pool.Push (obj);
            }
        }
    }
}
