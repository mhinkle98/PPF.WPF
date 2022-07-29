using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PPF.WPF.IO
{
    /// <summary>
    /// Static class for returning streams from different locations. 
    /// </summary>
    public static class StreamFinder
    {
        private static IDictionary<string, string> _lazyLoadLibraries;

        static StreamFinder()
        {
            _lazyLoadLibraries = new Dictionary<string, string>
            {
                // { "Database", "Database.dll" } ????
            };
        }

        /// <summary>
        /// Returns a stream from resources,
        /// given a file name and an assembly (project) name
        /// </summary>
        /// <param name="fileName">Name of the file in resources</param>
        /// <param name="projectName">Project name where the resources are located</param>
        /// <returns>Stream of data</returns>
        public static Stream GetFileStreamFromResources(string fileName, string projectName)
        {
            if (_lazyLoadLibraries.ContainsKey(projectName))
            {
                LibraryIO.EnsureDllIsLoaded(_lazyLoadLibraries[projectName]);
            }

            string currentAssemblyDirectoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fullPath = Path.Combine(currentAssemblyDirectoryName, projectName);

            Assembly assembly = null;

            if (File.Exists(fullPath + ".dll"))
            {
                assembly = Assembly.LoadFrom(fullPath + ".dll");
            }
            else if (File.Exists(fullPath + ".exe"))
            {
                assembly = Assembly.LoadFrom(fullPath + ".exe");
            }
            else if (FileIO.FileExists(projectName + ".dll"))
            {
                assembly = Assembly.LoadFrom(projectName + ".dll");
            }
            else
            {
                throw new FileNotFoundException("Can't locate specified assembly.");
            }

            var newFileName = fileName.Replace("/", ".").Replace(@"\", ".");
            var stream = assembly.GetManifestResourceStream(projectName + "." + newFileName);
            return stream;
        }

        /// <summary>
        /// Returns a stream from the local file system.
        /// </summary>
        /// <param name="filename">Name of the file</param>
        /// <param name="fileMode">The FileMode to use when accessing the file</param>
        /// <returns>Stream of data</returns>
        public static Stream GetFileStream(string filename, FileMode fileMode)
        {
            FileStream fs = null;

            try
            {
                fs = File.Open(filename, fileMode);
            }
            catch
            {
                // problem locating file
            }

            if (fs != null)
            {
                return fs;
            }

            try
            {
                string currentAssemblyDirectoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string fullPath = currentAssemblyDirectoryName + "\\" + filename;
                fs = File.Open(fullPath, fileMode);
            }
            catch
            {
                // problem locating file
            }

            if (fs != null)
            {
                return fs;
            }

            return null;
        }
    }
}
