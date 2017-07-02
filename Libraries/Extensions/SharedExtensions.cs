using System;
using System.IO;
using System.Text;

namespace EverStore
{

    public static class SharedExtensions
    {
        public static readonly UTF8Encoding ENCODING = new UTF8Encoding(false);

        public static bool IsFile(this string fullPath)
        {
            return File.Exists(fullPath);
        }
        public static string GetCurrentPath(this string fileName)
        {
            return Path.Combine(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), fileName);
        }

        public static string ReadAllText(this string fullPath)
        {
            return File.ReadAllText(fullPath, ENCODING);
        }
        public static string[] ReadAllLines(this string fullPath)
        {
            return File.ReadAllLines(fullPath, ENCODING);
        }

        public static void WriteAllText(this string fullPath, string content)
        {
            File.WriteAllText(fullPath, content, ENCODING);

        }
        public static string[] Slice(this string value, char separator, int count = 2)
        {
            if (value == null) return null;
            return value.Split(new char[] { separator }, count);
        }
        public static string TakeHead(this string value, char separator)
        {
            if (value == null) return null;
            return value.Split(new char[] { separator }, 2)[0];
        }
    }
}