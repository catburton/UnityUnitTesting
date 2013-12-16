using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework.Api;
using NUnit.Framework.Internal;
using System.Collections;
using UnityEngine;
using NUnitLite.Runner;

namespace UnityUnitTesting
{
    public static class UnityUnitTester
    {
        private static HashSet<string> _assemblyNamesToTest = new HashSet<string> { "Assembly-CSharp", "Assembly-CSharp-Editor" };
        private static HashSet<Assembly> _assembliesToTest;
        private static HashSet<AssemblyUnitTestResult> _testResults = new HashSet<AssemblyUnitTestResult>();

        public static HashSet<AssemblyUnitTestResult> TestResults
        {
            get
            {
                return _testResults;
            }
        }

        private static void Initialise()
        {
            _assembliesToTest = new HashSet<Assembly>();

            Assembly[] allAssemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            for (int i = 0; i < allAssemblies.Length; i++)
            {
                if (_assemblyNamesToTest.Contains(allAssemblies[i].GetName().Name))
                {
                    _assembliesToTest.Add(allAssemblies[i]);
                }
            }
        }

        public static HashSet<AssemblyUnitTestResult> RunAllTests()
        {
            _testResults.Clear();

            if (_assembliesToTest == null)
            {
                Initialise();
            }

            foreach (Assembly assembly in _assembliesToTest)
            {
                RunTestsForAssembly(assembly);
            }

            return _testResults;
        }

        private static void RunTestsForAssembly(Assembly assembly)
        {
            ITestAssemblyRunner testRunner = new NUnitLiteTestAssemblyRunner(new NUnitLiteTestAssemblyBuilder());
            IDictionary loadOptions = new Hashtable();
            
            if (!testRunner.Load(assembly, loadOptions))
            {
                Debug.LogWarning("No unit tests found for assembly: " + assembly.GetName().Name);
                return;
            }
            
            ITestResult result = testRunner.Run(TestListener.NULL, TestFilter.Empty);
            ResultSummary summary = new ResultSummary(result);
            AssemblyUnitTestResult unitTestResult = new AssemblyUnitTestResult(assembly, result, summary);

            unitTestResult.DebugResult();

            _testResults.Add(unitTestResult);
        }
    }
}