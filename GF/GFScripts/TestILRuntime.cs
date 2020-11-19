using System.Collections.Generic;
using UnityEngine;

namespace GFScripts
{
    class TestILRuntime
    {
        private int id;

        public TestILRuntime()
        {
            Debug.Log("[TestILRuntime]TestILRuntime()");
            this.id = 0;
        }

        public TestILRuntime(int id)
        {
            Debug.Log("[TestILRuntime]TestILRuntime() id = " + id);
            this.id = id;
        }

        public int ID
        {
            get { return id; }
        }

        /// <summary>
        /// static method
        /// </summary>
        public static void StaticFunTest()
        {
            Debug.Log("[TestILRuntime]StaticFunTest()");
        }

        public static void StaticFunTest2(int a)
        {
            Debug.Log("[TestILRuntime]StaticFunTest(), a = " + a);
        }

        public static void GenericMethod<T>(T a)
        {
            Debug.Log("[TestILRuntime]GenericMethod(), a = " + a);
        }

        public void RefOutMethod(int addition, out List<int> list, ref int val)
        {
            val = val + addition + this.id;
            list = new List<int>();
            list.Add(id);
            Debug.Log("[TestILRuntime]RefOutMethod()");
        }
    }
}
