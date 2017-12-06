using GrEmit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SREtest
{
    class GrEmitTest
    {
        public static AssemblyBuilder RunAndGenerateAssembly()
        {
            // создаем сборку (точнее AssemblyBuilder)
            var name = new AssemblyName("MyAssemblyGrEmit");
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
                myTypeObject, new Object[] { "abc", c });
            Console.WriteLine(resultStr); // abc10

            return assemblyBuilder;
        }

        private static void CreateAddIntToIntMethod(TypeBuilder typeBuilder)
        {
            MethodBuilder addIntMethodBuilder = typeBuilder.DefineMethod(
                "AddIntToInt",
                MethodAttributes.Public | MethodAttributes.Static,
                typeof(int), // тип возвращаемого значения
                new[] { typeof(int), typeof(int) }); // типы аргументов
            GenerateAddToIntBody(addIntMethodBuilder);
        }

        private static void GenerateAddToIntBody(MethodBuilder addMethodBuilder)
        {
            GroboIL generator = new GroboIL(addMethodBuilder);
            generator.Ldarg(0); // кладем в стек 1-ый аргумент
            generator.Ldarg(1); // кладем в стек 2-ой аргумент
            generator.Add(); // операция сложения
            generator.Ret(); // возвращение верхнего значения стека
                             // Стек обязан быть пуст
        }

        private static void CreateAddIntToStrMethod(TypeBuilder typeBuilder)
        {
            MethodBuilder addMethodBuilder = typeBuilder.DefineMethod(
                "AddIntToStr",
                MethodAttributes.Public | MethodAttributes.Static,
                // Тип возвращаемого значения
                typeof(string),
                // Типы аргументов.
                // Хотелось бы написать так:
                // new[] { typeof(string), typeof(int) });
                new[] { typeof(string), typeof(int).MakeByRefType() });
            GenerateAddToStrBody(addMethodBuilder);
        }

        private static void GenerateAddToStrBody(MethodBuilder addMethodBuilder)
        {
            GroboIL generator = new GroboIL(addMethodBuilder);
            generator.Ldarg(0);                                // [String]    (Типы на стеке)
            generator.Ldarg(1);                                // [String, Int32&]

            // получаем метод ToString() у int'а и добавляем вызов этого метода
            var toStringMethod = typeof(int).GetMethod(
                "ToString",
                Type.EmptyTypes);
            // Вызов метода у типа значения возможен,
            // только если он упакован или представлен ссылкой
            generator.Call(toStringMethod, typeof(Int32));     // [String, String]

            // получаем метод Concat
            var concatMethod = typeof(String).GetMethod(
                "Concat",
                new Type[] { typeof(String), typeof(String) });
            
            generator.Call(concatMethod, typeof(String));      // [String]
            generator.Ret();                                   // []
        }
    }
}