using System;
using System.Collections.Generic;
using System.Text;

namespace piine.Memory
{
    /*
     * GlobalObjects provides a global registry for storing named objects. 
     */
    public static class GlobalObjects
    {
        private static Dictionary<string, object> objects = new Dictionary<string, object> ();

        public static void Set (string name, object obj)
        {
            lock (objects)
                objects[name] = obj;
        }

        public static T Get<T> (string name)
        {
            lock (objects)
            {
                if (objects.TryGetValue (name, out object obj))
                    return (T)obj;
                else
                    return default;
            }
        }

        public static bool Remove (string name)
        {
            lock (objects)
                return objects.Remove (name);
        }

        public static bool HasValue (string name)
        {
            lock (objects)
                return objects.ContainsKey (name);
        }
    }
}
