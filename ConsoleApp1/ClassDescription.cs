using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class ClassDescription : ITypeDescription
    {
        public StaticOrDynamic StatOrDyn;

        public ClassDescription()
        {
        }

        public ClassDescription(string name)
        {
            TypeName = name;
            allTypes[TypeName] = this;
            Constructors = new List<ConstructorDescription>();
            Methods = new List<MethodDescription>();
            InnerClasses = new List<ClassDescription>();
        }

        public List<ConstructorDescription> Constructors;
        public List<MethodDescription> Methods;

        public List<ClassDescription> InnerClasses;
    }
}