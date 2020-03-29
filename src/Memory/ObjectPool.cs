using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Memory
{
    /*
     * A global ObjectPool for storing objects of one specific type. Used for temporary objects only for short durations. 
     */
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
