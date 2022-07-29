using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPF.WPF.IO
{
    /// <summary>
    /// Mainly for mono framework. Work-around for data serialization.
    /// </summary>
    public static class KnownTypes
    {
        private static readonly IDictionary<string, Type> _types;
        static KnownTypes()
        {
            var knownTypesArray = new Type[]
            {
                // add types if XML serializer is used...
            };
            _types = knownTypesArray.ToDictionary(type => type.ToString());
        }

        /// <summary>
        /// Gets a dictionary of the current known types
        /// </summary>
        public static IDictionary<string, Type> CurrentKnownTypes { get { return _types; } }

        /// <summary>
        /// Adds a new type to the list of known types
        /// </summary>
        /// <param name="t">The type to add</param>
        public static void Add(Type t)
        {
            if (!_types.ContainsKey(t.ToString()))
            {
                _types.Add(t.ToString(), t);
            }
        }
    }
}
