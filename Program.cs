using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args[0].Contains(".txt"))
            {
                Rebuild(args[0]);
            }
            else
            {
                Extract(args[0]);
            }
        }
        public static void Extract(string text)
        {
            var reader = new BinaryReader(File.OpenRead(text));
            List<string> strings = new List<string>();
            List<int> size = new List<int>();
            reader.BaseStream.Position = 0x8;
            int j = 0;
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                size.Add(reader.ReadInt32());
                strings.Add(Encoding.Unicode.GetString(reader.ReadBytes(size[j] * 2)));
                strings[j] = strings[j].Replace("\n", "<lf>").Replace("\r", "<br>");
                j++;
            }
            File.WriteAllLines(text.Replace(".str", ".txt"), strings);
        }
        public static void Rebuild(string text)
        {
            string[] strings = File.ReadAllLines(text);
            using (BinaryWriter writer = new BinaryWriter(File.Create(text.Replace(".txt", ".str"))))
            {
                writer.Write(3);
                writer.Write(2);
                for (int i = 0; i < strings.Length; i++)
                {
                    int size = strings[i].Length;
                    strings[i] = strings[i].Replace("<lf>", "\n").Replace("<br>", "\r");
                    writer.Write(size);
                    writer.Write(Encoding.Unicode.GetBytes(strings[i]));
                }
            }
        }
    }
}
