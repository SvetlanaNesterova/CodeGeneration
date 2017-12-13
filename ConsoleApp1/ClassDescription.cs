using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class ClassDescription : ITypeDescription
    {
        public string ClassName;
        public StaticOrDynamic StatOrDyn;

        public ClassDescription(string name)
        {
            ClassName = name;
            allTypes[name] = this;
            Constructors = new List<ConstructorDescription>();
            Methods = new List<MethodDescription>();
            InnerClasses = new List<ClassDescription>();
        }

        public List<ConstructorDescription> Constructors;
        public List<MethodDescription> Methods;

        public List<ClassDescription> InnerClasses;

        public override string ToString()
        {
            return ClassName;
        }
    }
}