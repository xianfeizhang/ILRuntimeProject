using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Mono.Cecil.Pdb;
using ILRuntime.Runtime.Enviorment;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GF
{
    public class ILRuntimeEntry : MonoSingleton<ILRuntimeEntry>
    {
        public AppDomain appDomain;

        FileStream scriptFS;
        FileStream pdbFS;

        private void Start()
        {
            appDomain = new AppDomain();

            RegisterDelegates();

            string filePath = "GFScripts/bin/Debug/GFScripts.dll";
            if (!FileHelper.Exists(filePath))
            {
                Debug.LogError("[ILRuntimeEntry] Start File not found: " + filePath);
                return;
            }

            string pdbPath = Path.ChangeExtension(filePath, ".pdb");

            scriptFS = new FileStream(filePath, FileMode.Open);

#if UNITY_EDITOR
            pdbFS = new FileStream(pdbPath, FileMode.Open);
            appDomain.LoadAssembly(scriptFS, pdbFS, new PdbReaderProvider());
            appDomain.DebugService.StartDebugService(56000);
#else
            appDomain.LoadAssembly(scriptFS);
#endif
            //调用热更入口
            appDomain.Invoke("GFScripts.CustomEntry", "HelloWorld", null);

            //TestILRuntime();
        }

        void TestILRuntime()
        {
            Debug.Log("1、调用无参数静态方法===========");
            appDomain.Invoke("GFScripts.TestILRuntime", "StaticFunTest", null, null);

            Debug.Log("2、调用有参数静态方法===========");
            appDomain.Invoke("GFScripts.TestILRuntime", "StaticFunTest2", null, 123);

            Debug.Log("3、通过IMethod调用方法===========");
            //预先获得Imethod，可以减低每次调用查找方法耗用的时间
            IType type = appDomain.LoadedTypes["GFScripts.TestILRuntime"];
            //根据方法名称和参数个数获取方法
            IMethod method = type.GetMethod("StaticFunTest2", 1);
            appDomain.Invoke(method, null, 456);

            Debug.Log("4、通过无GC Alloc方法调用方法===========");
            using (var ctx = appDomain.BeginInvoke(method))
            {
                ctx.PushInteger(789);
                ctx.Invoke();
            }

            Debug.Log("5、指定参数类型来获得IMethod===========");
            IType intType = appDomain.GetType(typeof(int));
            //参数类型列表
            List<IType> paramList = new List<IType>();
            paramList.Add(intType);
            //根据方法名称和参数类型列表获取方法
            method = type.GetMethod("StaticFunTest2", paramList, null);
            appDomain.Invoke(method, null, 123);

            Debug.Log("6、实例化热更工程的类===========");
            //法1
            object obj1 = appDomain.Instantiate("GFScripts.TestILRuntime", new object[] { 123 });
            //法2
            object obj2 = ((ILType)type).Instantiate();

            Debug.Log("7、调用成员方法===========");
            method = type.GetMethod("get_ID", 0);
            using (var ctx = appDomain.BeginInvoke(method))
            {
                ctx.PushObject(obj1);
                ctx.Invoke();
                int id = ctx.ReadInteger();
                Debug.Log("[ILRuntimeEntry]GFScripts.TestILRuntime.ID = " + id);
            }

            using (var ctx = appDomain.BeginInvoke(method))
            {
                ctx.PushObject(obj2);
                ctx.Invoke();
                int id = ctx.ReadInteger();
                Debug.Log("[ILRuntimeEntry]GFScripts.TestILRuntime.ID = " + id);
            }

            Debug.Log("8、获取泛型方法===========");
            IType stringType = appDomain.GetType(typeof(string));
            IType[] genericArgs = new IType[] { stringType };
            appDomain.InvokeGenericMethod("GFScripts.TestILRuntime", "GenericMethod", genericArgs, null, "TestString");
        }

        private void OnDestroy()
        {
            if (scriptFS != null)
            {
                scriptFS.Dispose();
            }

            if (pdbFS != null)
            {
                pdbFS.Dispose();
            }
        }

        void RegisterDelegates()
        {
            appDomain.DelegateManager.RegisterMethodDelegate<int>();
            appDomain.DelegateManager.RegisterMethodDelegate<float>();
            appDomain.DelegateManager.RegisterMethodDelegate<bool>();
            appDomain.DelegateManager.RegisterMethodDelegate<Vector2>();
            appDomain.DelegateManager.RegisterMethodDelegate<Vector3>();
            appDomain.DelegateManager.RegisterMethodDelegate<Vector4>();
            appDomain.DelegateManager.RegisterMethodDelegate<object>();
            appDomain.DelegateManager.RegisterMethodDelegate<GameObject>();
            appDomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Object>();
        }
    }

}