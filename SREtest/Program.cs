using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using GrEmit;

namespace SREtest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (false)
            {
                var ab = SREtest.RunAndGenerateAssembly();
                CreateEntryPoint(ab, "MyAssemblySRE");
                ab.Save("MyAssemblySRE.dll");

                ab = GrEmitTest.RunAndGenerateAssembly();
                CreateEntryPoint(ab, "MyAssemblyGrEmit");
                ab.Save("MyAssemblyGrEmit.dll");
            }

            var myyyyy = MyClass.AddIntToInt(123, 777);
        }


        static void CreateEntryPoint(AssemblyBuilder assemblyBuilder, string moduleName)
        {
            ModuleBuilder module = assemblyBuilder.GetDynamicModule(moduleName);
            TypeBuilder typeProgram = module.DefineType("Program", TypeAttributes.Public);

            MethodBuilder mainBuilder = typeProgram.DefineMethod("Main",
                                MethodAttributes.Public |
                                MethodAttributes.Static,
                                typeof(void),
                                null);
            GroboIL generator = new GroboIL(mainBuilder);
            generator.Ret();

            Type programType = typeProgram.CreateType();
            assemblyBuilder.SetEntryPoint(mainBuilder);
        }
    }
}