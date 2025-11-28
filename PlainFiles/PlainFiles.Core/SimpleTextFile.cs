using System;
using System.IO;

namespace PlainFiles.Core
{
    public class SimpleTextFile
    {
        private readonly string _path;
        public SimpleTextFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path must not be null or empty.", nameof(path));
            }
                

            _path = path;

            // Ensure directory exists
            var directory = Path.GetDirectoryName(_path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
                

            // Create the file if it does not exist
            if (!File.Exists(_path))
            {
                using (File.Create(_path))
                {
                  
                }

            }
        }

        public void WriteAllLines(string[] lines)
        {
            // Ensure directory exists before writing (defensive)
            var directory = Path.GetDirectoryName(_path);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllLines(_path, lines);
        }

        public string[] ReadAllLines()
        {
            // If the file somehow doesn't exist, return an empty array
            if (!File.Exists(_path))
                return Array.Empty<string>();

                return File.ReadAllLines(_path);
        }

    }
}
