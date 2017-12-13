using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Parser
{
    public enum StaticOrDynamic { Static, Dynamic };

    public class ITypeDescriber
    {
        public static Dictionary<string, ITypeDescriber> allTypes;
        public Dictionary<string, ITypeDescriber> AllTypes;
        public Type realType;

        static ITypeDescriber()
        {
            allTypes = new Dictionary<string, ITypeDescriber>();
        }

        public ITypeDescriber()
        {
            AllTypes = allTypes;
        }
    }
}
