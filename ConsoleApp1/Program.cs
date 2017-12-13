using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Parser
{
    class Program
    {
        static string resultFileName =
                @"C:\Users\dns\Documents\Visual Studio 2017\SREtest\ConsoleApp1\result.txt";

        static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };


        static void Main(string[] args)
        {
            DeserializesGenerateSample();
            UseSample();
        }

        static void DeserializesGenerateSample()
        {
            DescribeSerializeSample();
            string source = File.ReadAllText(resultFileName);
            ClassDescription res = JsonConvert.DeserializeObject<ClassDescription>(source);

            Type type = Generator.GenerateClass(res);
        }

        static void DescribeSerializeSample()
        {
            ClassDescription item = DescribeSomeClass();
            string result = JsonConvert.SerializeObject(item, settings);
            result = MakeReadable(result);
            File.WriteAllText(resultFileName, result);
        }

        static void UseSample()
        {
            var myyyy = new MyClass();
            myyyy.Add("aaaa", 100);
        }

        static ClassDescription DescribeSomeClass()
        {
            ClassDescription descr = new ClassDescription("MyClass");
            descr.StatOrDyn = StaticOrDynamic.Dynamic;
            var m = new MethodDescription("Add");
            m.StaticOrDynamic = StaticOrDynamic.Dynamic;
            m.InputTypes = new[] { "string", "int" };
            m.OutputType = "string";
            descr.Methods.Add(m);
            return descr;
        }

        static string MakeReadable(string source)
        { 
           // Метод добавляет отступы и переносы строк
            var res = "";
            int depth = 0;
            foreach (var c in source)
            {

                if (c == '{' || c == '[')
                {
                    res += "\n";
                    for (int i = 0; i < depth; i++)
                        res += "\t";
                    depth++;
                    res += c;
                    res += "\n";
                    for (int i = 0; i < depth; i++)
                        res += "\t";
                }
                else if (c == '}' || c == ']')
                {
                    depth--;
                    res += "\n";
                    for (int i = 0; i < depth; i++)
                        res += "\t";
                    res += c;
                    res += "\n";
                    for (int i = 0; i < depth; i++)
                        res += "\t";
                }
                else if (c == ',')
                {
                    res += c;
                    res += "\n";
                    for (int i = 0; i < depth; i++)
                        res += "\t";
                }
                else
                {
                    res += c;
                }
            }
            return res;
        }
    }
}