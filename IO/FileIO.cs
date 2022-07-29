using System;
using System.Collections.Generic;
using System.IO;
using PPF.WPF.Extensions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace PPF.WPF.IO
{
    /// <summary>
    /// Static class that provides methods for JSON and Binary data processing.
    /// </summary>
    public static class FileIO
    {
        /// <summary>
        /// Static method to check if a file exists.
        /// </summary>
        /// <param name="fileName">Name of file to check</param>
        /// <returns></returns>
        public static bool FileExists(string fileName)
        {
            return File.Exists(fileName);
        }

        /// <summary>
        /// Static method to check if folder exists.
        /// </summary>
        /// <param name="folder">Folder/directory to check</param>
        /// <returns></returns>
        public static bool DirectoryExists(string folder)
        {
            return Directory.Exists(folder);
        }

        /// <summary>
        /// Static method to delete a file. Checks for existance before attempting.
        /// </summary>
        /// <param name="fileName">File name to delete</param>
        public static void DeleteFile(string fileName)
        {
            if (FileExists(fileName))
            {
                File.Delete(fileName);
            }
        }

        /// <summary>
        /// Static method to delete a directory
        /// </summary>
        /// <param name="folder">Folder or path to delete</param>
        public static void DeleteDirectory(string folder)
        {
            if (DirectoryExists(folder))
            {
                ClearDirectory(folder);
                Directory.Delete(folder);
            }
        }

        /// <summary>
        /// Platform-agnostic directory creation
        /// </summary>
        /// <param name="folderPath">Path for new directory</param>
        public static void CreateDirectory(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        /// <summary>
        /// Platform-agnostic directory
        /// </summary>
        /// <param name="folderPath">Path for new directory</param>
        public static void ClearDirectory(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                foreach (var file in Directory.GetFiles(folderPath))
                {
                    File.Delete(file);
                }
            }
        }

        /// <summary>
        /// Static cloning method for objects
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="myObject">Object to clone</param>
        /// <returns>Returns the clone of the object</returns>
        public static T Clone<T>(this T myObject)
        {
            var serialized = myObject.WriteToJson();
            return serialized.ReadFromJson<T>();
        }

        /// <summary>
        /// Copies a stream to another output.
        /// </summary>
        /// <param name="input">Stream to copu</param>
        /// <param name="output">Where to copy stream to</param>
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        /// <summary>
        /// Platform-agnostic call to create an empty directory (deleting files if they exist)
        /// </summary>
        /// <param name="folderPath">Path for new directory</param>
        public static void CreateEmptyDirectory(string folderPath)
        {
            FileIO.ClearDirectory(folderPath);
            FileIO.CreateDirectory(folderPath);
        }

        /// <summary>
        /// Static method to write an object to a stream
        /// </summary>
        /// <typeparam name="T">Type of the object to be written</typeparam>
        /// <param name="myObject">Object to be written</param>
        /// <param name="stream">Stream to which to write the object</param>
        public static void WriteToXMLStream<T>(this T myObject, Stream stream)
        {
            var dcs = new DataContractSerializer(typeof(T), KnownTypes.CurrentKnownTypes.Values);
            dcs.WriteObject(stream, myObject);
        }

        /// <summary>
        /// Static method to write an object to a stream
        /// </summary>
        /// <typeparam name="T">Type of the object to be written</typeparam>
        /// <param name="myObject">Object to be written</param>
        /// <param name="stream">Stream to which to write the object</param>
        public static void WriteJsonToStream<T>(this T myObject, Stream stream)
        {
            var serializedJson = PPFlowJsonSerializer.WriteToJson(myObject);
            WriteTextToStream(serializedJson, stream);
        }

        /// <summary>
        /// Static method to read a specified type from a stream
        /// </summary>
        /// <typeparam name="T">Type of the data to be read</typeparam>
        /// <param name="stream">Stream from which to read</param>
        /// <returns>The data as the specified type</returns>
        public static T ReadFromStream<T>(Stream stream)
        {
            var dcs = new DataContractSerializer(typeof(T), KnownTypes.CurrentKnownTypes.Values);
            return (T)dcs.ReadObject(stream);
        }

        /// <summary>
        /// Static method to read a specified type from a stream
        /// </summary>
        /// <typeparam name="T">Type of the data to be read</typeparam>
        /// <param name="stream">Stream from which to read</param>
        /// <returns>The data as the specified type</returns>
        public static T ReadFromJsonStream<T>(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                var serializedJson = sr.ReadToEnd();
                var myObject = PPFlowJsonSerializer.ReadFromJson<T>(serializedJson);
                return myObject;
            }
        }

        /// <summary>
        /// Writes the string to a stream
        /// </summary>
        /// <param name="text">Text to write to the file</param>
        /// <param name="stream">Name of the stream</param>
        public static void WriteTextToStream(string text, Stream stream)
        {
            using (StreamWriter outstream = new StreamWriter(stream))
            {
                outstream.Write(text);
            }
        }

        /// <summary>
        /// Writes the string to a text file
        /// </summary>
        /// <param name="text">Text to write to the file</param>
        /// <param name="filename">Name of the text file to write </param>
        public static void WriteToTextFile(string text, string filename)
        {
            Stream stream = StreamFinder.GetFileStream(filename, FileMode.Create);
            using (StreamWriter outfile = new StreamWriter(stream))
            {
                outfile.Write(text);
            }
        }

        /// <summary>
        /// Writes data of a specified type to an XML file
        /// </summary>
        /// <typeparam name="T">Type of the data to be written</typeparam>
        /// <param name="myObject">Object to be written</param>
        /// <param name="filename">Name of the XML file to write</param>
        public static void WriteToXML<T>(this T myObject, string filename)
        {
            using (Stream stream = StreamFinder.GetFileStream(filename, FileMode.Create))
            {
                myObject.WriteToXMLStream(stream);
            }
        }

        /// <summary>
        /// Writes data of a specified type to an JSON file
        /// </summary>
        /// <typeparam name="T">Type of the data to be written</typeparam>
        /// <param name="myObject">Object to be written</param>
        /// <param name="filename">Name of the JSON file to write</param>
        public static void WriteToJson<T>(this T myObject, string filename)
        {
            using (Stream stream = StreamFinder.GetFileStream(filename, FileMode.Create))
            {
                myObject.WriteJsonToStream(stream);
            }
        }

        /// <summary>
        /// Reads data of a specified type from an XML file
        /// </summary>
        /// <typeparam name="T">Type of the data</typeparam>
        /// <param name="filename">Name of the XML file to be read</param>
        /// <returns>The data as the specified type</returns>
        public static T ReadFromXML<T>(string filename)
        {
            using (Stream stream = StreamFinder.GetFileStream(filename, FileMode.Open))
            {
                return ReadFromStream<T>(stream);
            }
        }

        /// <summary>
        /// Reads data of a specified type from a JSON file
        /// </summary>
        /// <typeparam name="T">Type of the data</typeparam>
        /// <param name="filename">Name of the JSON file to be read</param>
        /// <returns>The data as the specified type</returns>
        public static T ReadFromJson<T>(string filename)
        {
            using (Stream stream = StreamFinder.GetFileStream(filename, FileMode.Open))
            {
                return ReadFromJsonStream<T>(stream);
            }
        }

        /// <summary>
        /// Copy a file from resources to an external location
        /// </summary>
        /// <example>FileIO.CopyFileFromResources("Resources/resourcesfile.txt", Path.Combine(resultsFolder, "resourcefile.txt"), "Vts.Desktop.Test");</example>
        /// <param name="sourceFileName">Path and filename of the file in resources</param>
        /// <param name="destinationFileName">Path and filename of the destination location</param>
        /// <param name="projectName">The name of the project where the file in resources is located</param>
        public static void CopyFileFromResources(string sourceFileName, string destinationFileName, string projectName)
        {
            using (var stream = StreamFinder.GetFileStreamFromResources(sourceFileName, projectName))
            {
                var emptyStream = StreamFinder.GetFileStream(destinationFileName, FileMode.Create);
                stream.CopyTo(emptyStream);
                emptyStream.Close();
            }
        }

        public static T ReadFromXMLInResources<T>(string fileName, string projectName)
        {
            using (Stream stream = StreamFinder.GetFileStreamFromResources(fileName, projectName))
            {
                return ReadFromStream<T>(stream);
            }
        }

        /// <summary>
        /// Reads data of a specified type from an JSON file in resources
        /// </summary>
        /// <typeparam name="T">Type of the data</typeparam>
        /// <param name="fileName">Name of the JSON file to be read</param>
        /// <param name="projectName">Project name for the location of resources</param>
        /// <returns>The data as the specified type</returns>
        public static T ReadFromJsonInResources<T>(string fileName, string projectName)
        {
            using (var stream = StreamFinder.GetFileStreamFromResources(fileName, projectName))
            {
                return ReadFromJsonStream<T>(stream);
            }
        }

        /// <summary>
        /// Writes a scalar value to a binary file
        /// </summary>
        /// <typeparam name="T">Type of the data to be written</typeparam>
        /// <param name="dataIN">Data to be written</param>
        /// <param name="filename">Name of the binary file to write</param>
        /// <param name="writeMap"></param>
        public static void WriteScalarValueToBinary<T>(T dataIN, string filename, Action<BinaryWriter, T> writeMap)
        {
            // Create a file to write binary data 
            using (Stream s = StreamFinder.GetFileStream(filename, FileMode.OpenOrCreate))
            {
                using (BinaryWriter bw = new BinaryWriter(s))
                {
                    writeMap(bw, dataIN);
                }
            }
        }

        /// <summary>
        /// Reads a scalar value from a binary file
        /// </summary>
        /// <typeparam name="T">Type of data to be read</typeparam>
        /// <param name="filename">Name of the binary file</param>
        /// <param name="readMap"></param>
        /// <returns></returns>
        public static T ReadScalarValueFromBinary<T>(string filename, Func<BinaryReader, T> readMap)
        {
            // Create a file to write binary data 
            using (Stream s = StreamFinder.GetFileStream(filename, FileMode.OpenOrCreate))
            {
                using (BinaryReader br = new BinaryReader(s))
                {
                    return readMap(br);
                }
            }
        }

        /// <summary>
        /// Writes an array to a binary file and optionally accompanying .xml file 
        /// (to store array dimensions) if includeMetaData = true
        /// </summary>
        /// <param name="dataIN">Array to be written</param>
        /// <param name="filename">Name of the file where the data is written</param>
        /// <param name="includeMetaData">Boolean to determine whether to include meta data, if set to true, an accompanying XML file will be created with the same name</param>
        public static void WriteArrayToBinary(Array dataIN, string filename, bool includeMetaData)
        {
            // Write XML file to describe the contents of the binary file
            if (includeMetaData)
            {
                new MetaData(dataIN).WriteToJson(filename + ".txt");
            }
            // Create a file to write binary data 
            using (Stream s = StreamFinder.GetFileStream(filename, FileMode.OpenOrCreate))
            {
                using (BinaryWriter bw = new BinaryWriter(s))
                {
                    new ArrayCustomBinaryWriter().WriteToBinary(bw, dataIN);
                }
            }
        }

        /// <summary>
        /// Writes an array to a binary file, as well as an accompanying .txt (JSON) file to store array dimensions
        /// </summary>
        /// <param name="dataIN">Array to be written</param>
        /// <param name="filename">Name of the file to which the array is written</param>
        public static void WriteArrayToBinary(Array dataIN, string filename)
        {
            WriteArrayToBinary(dataIN, filename, true);
        }

        /// <summary>
        /// Reads array from a binary file, using the accompanying .txt file to specify dimensions
        /// </summary>
        /// <typeparam name="T">Type of the array being read</typeparam>
        /// <param name="filename">Name of the file from which to read the array</param>
        /// <returns>Array from the file</returns>
        public static Array ReadArrayFromBinary<T>(string filename) where T : struct
        {
            MetaData dataInfo = ReadFromJson<MetaData>(filename + ".txt");

            return ReadArrayFromBinary<T>(filename, dataInfo.dims);
        }

        /// <summary>
        /// Reads array from a binary file using explicitly-set dimensions
        /// </summary>
        /// <typeparam name="T">Type of the array being read</typeparam>
        /// <param name="filename">Name of the file from which to read the array</param>
        /// <param name="dims">Dimensions of the array</param>
        /// <returns>Array from the file</returns>
        public static Array ReadArrayFromBinary<T>(string filename, params int[] dims) where T : struct
        {
            using (Stream s = StreamFinder.GetFileStream(filename, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(s))
                {
                    return new ArrayCustomBinaryReader<T>(dims).ReadFromBinary(br);
                }
            }
        }

        /// <summary>
        /// Reads array from a binary file in resources, using the accompanying .txt (JSON) file to specify dimensions
        /// </summary>
        /// <typeparam name="T">Type of the array being read</typeparam>
        /// <param name="filename">Name of the JSON file containing the meta data</param>
        /// <param name="projectname">Project name for the location of resources</param>
        /// <returns>Array from the file</returns>
        public static Array ReadArrayFromBinaryInResources<T>(string filename, string projectname) where T : struct
        {
            // Read JSON text file that describes the contents of the binary file
            MetaData dataInfo = ReadFromJsonInResources<MetaData>(filename + ".txt", projectname);

            // call the overload (below) which explicitly specifies the array dimensions
            return ReadArrayFromBinaryInResources<T>(filename, projectname, dataInfo.dims);
        }

        /// <summary>
        /// Reads array from a binary file in resources using explicitly-set dimensions
        /// </summary>
        /// <typeparam name="T">Type of the array being read</typeparam>
        /// <param name="filename">Name of the JSON (text) file containing the meta data</param>
        /// <param name="projectname">Project name for the location of resources</param>
        /// <param name="dims">Dimensions of the array</param>
        /// <returns>Array from the file</returns>
        public static Array ReadArrayFromBinaryInResources<T>(string filename, string projectname, params int[] dims) where T : struct
        {
            using (Stream stream = StreamFinder.GetFileStreamFromResources(filename, projectname))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    return new ArrayCustomBinaryReader<T>(dims).ReadFromBinary(br);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="fileName">Name of the binary file to write</param>
        /// <param name="writerMap"></param>
        public static void WriteToBinaryCustom<T>(this IEnumerable<T> data, string fileName, Action<BinaryWriter, T> writerMap)
        {
            // convert to "push" method with System.Observable in Rx Extensions (write upon appearance of new datum)?
            using (Stream s = StreamFinder.GetFileStream(fileName, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(s))
                {
                    data.ForEach(d => writerMap(bw, d));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">Name of the binary file to read</param>
        /// <param name="readerMap"></param>
        /// <returns></returns>
        public static IEnumerable<T> ReadFromBinaryCustom<T>(string fileName, Func<BinaryReader, T> readerMap)
        {
            using (Stream s = StreamFinder.GetFileStream(fileName, FileMode.Open))
            {
                foreach (var item in ReadStreamFromBinaryCustom<T>(s, readerMap))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">Name of the binary file to read</param>
        /// <param name="projectName">Project name where resources is located</param>
        /// <param name="readerMap"></param>
        /// <returns></returns>
        public static IEnumerable<T> ReadFromBinaryInResourcesCustom<T>(string fileName, string projectName, Func<BinaryReader, T> readerMap)
        {
            using (Stream s = StreamFinder.GetFileStreamFromResources(fileName, projectName))
            {
                foreach (var item in ReadStreamFromBinaryCustom<T>(s, readerMap))
                {
                    yield return item;
                }
            }
        }

        // both versions of ReadArrayFromBinary<T> call this method to actually read the data - is this still true?
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="readerMap"></param>
        /// <returns></returns>
        private static IEnumerable<T> ReadStreamFromBinaryCustom<T>(Stream s, Func<BinaryReader, T> readerMap)
        {
            using (BinaryReader br = new BinaryReader(s))
            {
                while (s.Position < s.Length)
                {
                    yield return readerMap(br);
                }
            }
        }

        #region Platform-Specific Methods

        /// <summary>
        /// Read from a binary stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static T ReadFromBinary<T>(string filename)
        {
            using (Stream stream = StreamFinder.GetFileStream(filename, FileMode.Open))
            {
                return ReadFromBinaryStream<T>(stream);
            }
        }

        /// <summary>
        /// Read an object of type T from a binary file in resources
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="filename">Name of the binary file in resources</param>
        /// <param name="projectName">Name of the project where the resources are located</param>
        /// <returns>The object of type T</returns>
        public static T ReadFromBinaryInResources<T>(string filename, string projectName)
        {
            using (Stream stream = StreamFinder.GetFileStreamFromResources(filename, projectName))
            {
                return ReadFromBinaryStream<T>(stream);
            }
        }

        /// <summary>
        /// Write an object of type T to a binary file
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="myObject">Object</param>
        /// <param name="filename">Name of the binary file</param>
        public static void WriteToBinary<T>(this T myObject, string filename)
        {
            using (Stream stream = StreamFinder.GetFileStream(filename, FileMode.Create))
            {
                WriteToBinaryStream<T>(myObject, stream);
            }
        }

        /// <summary>
        /// Deserializes a stream into an object
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="s">Stream to deserialize</param>
        /// <returns>The object of type T</returns>
        public static T ReadFromBinaryStream<T>(Stream s)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                return (T)formatter.Deserialize(s);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
        }

        /// <summary>
        /// Serializes an object of type T to the given stream
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="myObject">Object</param>
        /// <param name="s">Stream to which to write the object</param>
        public static void WriteToBinaryStream<T>(T myObject, Stream s)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(s, myObject);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
        }
        #endregion
    }
}
