using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection;
using GrEmit;
using System.Threading;

namespace Parser
{
    static class Generator
    {
        public static Type GenerateClass(ClassDescription description)
        {
            var name = new AssemblyName("MyAssembly");
            AssemblyBuilder assemblyBuilder = 
                Thread.GetDomain().DefineDynamicAssembly(
                    name, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(
                name.Name, name.Name + ".dll");
            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                description.TypeName, TypeAttributes.Class | TypeAttributes.Public);

            foreach (var constructorDescr in description.Constructors)
            {
                var staticOrDynamicAttr =
                    constructorDescr.StaticOrDynamic == StaticOrDynamic.Static ?
                    MethodAttributes.Static : MethodAttributes.Public;
                // второй вариант по сути ничего не добавляет, но атрибута Dynamic нет

                var inTypes = constructorDescr.InputTypes
                    .Select(typeName => description.AllTypes[typeName].RealType)
                    .ToArray();

                typeBuilder.DefineConstructor(
                    MethodAttributes.Public | staticOrDynamicAttr,
                    CallingConventions.Standard,
                    inTypes);
            }


            foreach (var methodDescr in description.Methods)
            {
                var staticOrDynamicAttr =
                    methodDescr.StaticOrDynamic == StaticOrDynamic.Static ?
                    MethodAttributes.Static : MethodAttributes.Public;

                var outType = description.AllTypes[methodDescr.OutputType].RealType;
                var inTypes = methodDescr.InputTypes
                    .Select(typeName => description.AllTypes[typeName].RealType)
                    .ToArray();

                var method = typeBuilder.DefineMethod(
                    methodDescr.MethodName,
                    MethodAttributes.Public | staticOrDynamicAttr,
                    outType,
                    inTypes);


                GroboIL iLgenerator = new GroboIL(method);
                iLgenerator.Newobj(new NotImplementedException()
                    .GetType()
                    .GetConstructors()[0]);
                iLgenerator.Throw();
            }

            var type =  typeBuilder.CreateType();
            assemblyBuilder.Save(name.Name + ".dll");
            return type;
        }
    }
}