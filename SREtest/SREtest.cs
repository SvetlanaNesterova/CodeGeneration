using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace SREtest
{
    class SREtest
    {
        public static AssemblyBuilder RunAndGenerateAssembly()
        {
            // создаем сборку (точнее AssemblyBuilder)
            var name = new AssemblyName("MyAssemblySRE");
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                name, AssemblyBuilderAccess.RunAndSave);

            // создаем модуль (билдер модуля)
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(
                name.Name, name.Name + ".dll");

            // создаем класс (билдер типа)
            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                "MyClass", TypeAttributes.Public);

            // добавляем к классу методы
            CreateAddIntToStrMethod(typeBuilder);
            CreateAddIntToIntMethod(typeBuilder);

            // наконец, создаем сам тип и объект данного типа
            Type myType = typeBuilder.CreateType();
            var myTypeObject = Activator.CreateInstance(myType);

            // используем его методы
            var addToIntMethod = myType.GetMethod("AddIntToInt");
            var resultInt = addToIntMethod.Invoke(myTypeObject, new Object[] { 90, 10 });
            Console.WriteLine(resultInt); // 100

            var addToStrMethod = myType.GetMethod("AddIntToStr");
            int c = 10;
            ref int r = ref c;
            var resultStr = addToStrMethod.Invoke(
                myTypeObject, new Object[] { "abc", r });
            Console.WriteLine(resultStr); // abc10

            return assemblyBuilder;
        }

        private static void CreateAddIntToIntMethod(TypeBuilder typeBuilder)
        {
            MethodBuilder addIntMethodBuilder = typeBuilder.DefineMethod(
                "AddIntToInt",
                MethodAttributes.Public,
                typeof(int), // тип возвращаемого значения
                new[] { typeof(int), typeof(int) }); // типы аргументов
            GenerateAddToIntBody(addIntMethodBuilder);
        }

        private static void GenerateAddToIntBody(MethodBuilder addMethodBuilder)
        {
            ILGenerator generator = addMethodBuilder.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldarg_2);
            generator.Emit(OpCodes.Add);
            generator.Emit(OpCodes.Ret);
        }

        private static void CreateAddIntToStrMethod(TypeBuilder typeBuilder)
        {
            MethodBuilder addMethodBuilder = typeBuilder.DefineMethod(
                "AddIntToStr",
                MethodAttributes.Public,
                // Тип возвращаемого значения
                typeof(string),
                // Типы аргументов
                new[] { typeof(string), typeof(int).MakeByRefType() });
            GenerateAddToStrBody(addMethodBuilder);
        }

        private static void GenerateAddToStrBody(MethodBuilder addMethodBuilder)
        {
            ILGenerator generator = addMethodBuilder.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldarg_2);

            var toStringMethod = typeof(int).GetMethod("ToString", Type.EmptyTypes);
            // во время исполнения неправильно написанной программы кидало NullRefException
            generator.EmitCall(OpCodes.Call, toStringMethod, null);

            var concatMethod = typeof(String).GetMethod(
                "Concat",
                new Type[] { typeof(String), typeof(String) });

            generator.EmitCall(OpCodes.Call, concatMethod, null);
            generator.Emit(OpCodes.Ret);
        }
    }
}
