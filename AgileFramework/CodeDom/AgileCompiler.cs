using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;

namespace AgileFramework.CodeDom
{
    public static class AgileCompiler
    {
        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="classCode">代码</param>
        /// <param name="referencedAssemblies">引用</param>
        /// <param name="compilerOptions">参数</param>
        /// <returns>结果</returns>
        public static Assembly Compile(string classCode, string[] referencedAssemblies = null, string compilerOptions = "/t:library /optimize+ /platform:AnyCPU")
        {
            Dictionary<string, string> providerOptions = new Dictionary<string, string>();
            providerOptions.Add("CompilerVersion", "v4.0");

            referencedAssemblies = referencedAssemblies ?? new[] { "system.dll" };
            using (var csharpCodeProvider = new CSharpCodeProvider(providerOptions))
            {
                var compilerParameters = new CompilerParameters();
                compilerParameters.ReferencedAssemblies.AddRange(referencedAssemblies);
                compilerParameters.CompilerOptions = compilerOptions;
                compilerParameters.GenerateInMemory = true;
                compilerParameters.IncludeDebugInformation = false;
                compilerParameters.TreatWarningsAsErrors = false;
                compilerParameters.OutputAssembly = string.Format("{0}.dll", AgileGuid.LowerNewGuid());
                var compilerResults = csharpCodeProvider.CompileAssemblyFromSource(compilerParameters, classCode);
                return compilerResults.CompiledAssembly;
            }
        }

        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="classCode">代码</param>
        /// <param name="referencedAssemblies">引用</param>
        /// <param name="outputAssembly">输出集</param>
        /// <param name="tempFileCollection">临时文件集合</param>
        /// <param name="compilerOptions">参数</param>
        /// <returns>结果</returns>
        public static string CompileToFile(string classCode, string[] referencedAssemblies = null, string outputAssembly = "", TempFileCollection tempFileCollection = null, string compilerOptions = "/t:library /optimize+ /platform:AnyCPU")
        {
            Dictionary<string, string> providerOptions = new Dictionary<string, string>();
            providerOptions.Add("CompilerVersion", "v4.0");

            referencedAssemblies = referencedAssemblies ?? new[] { "system.dll" };
            using (var csharpCodeProvider = new CSharpCodeProvider(providerOptions))
            {
                var compilerParameters = new CompilerParameters();
                compilerParameters.ReferencedAssemblies.AddRange(referencedAssemblies);
                compilerParameters.CompilerOptions = compilerOptions;
                compilerParameters.GenerateInMemory = false;
                compilerParameters.IncludeDebugInformation = false;
                compilerParameters.TreatWarningsAsErrors = false;
                if (string.IsNullOrWhiteSpace(outputAssembly))
                {
                    compilerParameters.OutputAssembly = string.Format("{0}.dll", AgileGuid.LowerNewGuid());
                }
                else
                {
                    compilerParameters.OutputAssembly = outputAssembly;
                }
                if (tempFileCollection != null)
                {
                    compilerParameters.TempFiles = tempFileCollection;
                }
                var compilerResults = csharpCodeProvider.CompileAssemblyFromSource(compilerParameters, classCode);
                return compilerResults.PathToAssembly;
            }
        }
    }
}
