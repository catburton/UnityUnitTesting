using NUnit.Framework.Api;
using NUnitLite.Runner;
using System.Reflection;
using UnityEngine;

namespace UnityUnitTesting
{
    public class AssemblyUnitTestResult
    {
        private Assembly _assembly;
        private ResultSummary _resultSummary;
        private ITestResult _testResult;

        public Assembly Assembly
        {
            get
            {
                return _assembly;
            }
        }

        public ITestResult TestResult
        {
            get
            {
                return _testResult;
            }
        }

        public AssemblyUnitTestResult(Assembly assembly, ITestResult testResult, ResultSummary resultSummary)
        {
            _assembly = assembly;
            _testResult = testResult;
            _resultSummary = resultSummary;
        }

        public void DebugResult()
        {
            string summary = ToString();

            if (_resultSummary.ErrorCount > 0 || _resultSummary.FailureCount > 0)
            {
                Debug.LogError(summary);
            }
            else if (_resultSummary.IgnoreCount > 0 || _resultSummary.SkipCount > 0)
            {
                Debug.LogWarning(summary);
            }
            else
            {
                Debug.Log(summary);
            }
        }

        public override string ToString()
        {
            return string.Format("AssemblyUnitTestResult for {0}: Pass={1}, Skip={2}, Ignore={3}, Fail={4}", _assembly.GetName().Name, _resultSummary.PassCount, _resultSummary.SkipCount, _resultSummary.IgnoreCount, _resultSummary.FailureCount);
        }

        public string DetailedResultsString()
        {
            string result = "";

            if (_testResult.HasChildren)
            {
                foreach (ITestResult testFixture in _testResult.Children)
                {
                    result += string.Format("UnitTestResult for fixture {0}: Pass={1}, Skip={2}, Ignore={3}, Fail={4}\n", testFixture.Name, testFixture.PassCount, testFixture.SkipCount, _resultSummary.IgnoreCount, testFixture.FailCount);
                }
            }
            return result;
        }
    }
}
