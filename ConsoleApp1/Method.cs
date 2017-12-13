using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class ConstructorDescription
    {
        public string MethodName = "Constructor";
        public StaticOrDynamic StaticOrDynamic = StaticOrDynamic.Dynamic;

        public string[] InputTypes;
    }

    public class MethodDescription
    {
        public MethodDescription(string name)
        {
            MethodName = name;
        }

        public string MethodName;
        public StaticOrDynamic StaticOrDynamic;

        public string OutputType;
        public string[] InputTypes;
    }
}
