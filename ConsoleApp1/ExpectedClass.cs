using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


/*
 * Сможем ли вытащить названия аргументов и информацию для "всплывающих подсказок"?
 */
namespace Parser
{
    public class ExpectedClass1
    {
        private string index;

        public ExpectedClass1(string str)
        {
            index = str;
        }

        // кажется это сложно
        public ExpectedClass1(ExpectedClass1 source)
        {
        }

        public void DoSomething()
        {
        }

        public static InnerClass getSummator()
        {
            throw new NotImplementedException();
        }

        public double[] GetSomeNumbers()
        {
            throw new NotImplementedException();
        }

        public double[] GetSomeNumbers(int param)
        {
            throw new NotImplementedException();
        }

        public class InnerClass
        {
            public int Sum(int a, int b, ExpectedClass1 something)
            {
                throw new NotImplementedException();
            }
        }
    }
}
