using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Parser
{
    public enum StaticOrDynamic { Static, Dynamic };

    public class ITypeDescription
    {
        public static Dictionary<string, ITypeDescription> allTypes;
        public Dictionary<string, ITypeDescription> AllTypes;
        public Type realType;

        static ITypeDescription()
        {
            allTypes = new Dictionary<string, ITypeDescription>();
        }

        public ITypeDescription()
        {
            AllTypes = allTypes;
        }
    }
}
