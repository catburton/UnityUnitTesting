using UnityEditor;
using UnityUnitTesting;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework.Api;

public class UnityUnitTestWindow : EditorWindow
{
    private const string _runUnitTestsAutomaticallyPrefsKey = "UnityUnitTesting.RunUnitTestsAutomatically";

    private bool RunUnitTestsAutomatically
    {
        get { return EditorPrefs.GetBool(_runUnitTestsAutomaticallyPrefsKey); }
        set { EditorPrefs.SetBool(_runUnitTestsAutomaticallyPrefsKey, value); }
    }

    private bool _wasCompilingScriptsInPreviousFrame = false;
    private HashSet<ITestResult> _expandedTestfixtures = new HashSet<ITestResult>();
    private Texture2D rowBackgroundTex = new Texture2D(1, 16);
    private Vector2 scrollAreaPosition = Vector2.zero;

    [MenuItem("Window/UnitTesting/Run All Unit Tests")]
    public static void RunAllUnitTests()
    {
        UnityUnitTester.RunAllTests();
    }

    [MenuItem ("Window/UnitTesting/Show Window")]
    static void Init()
    {
        UnityUnitTestWindow window = (UnityUnitTestWindow)EditorWindow.GetWindow(typeof(UnityUnitTestWindow));
        window.title = "Unit Testing";
    }

    void OnGUI()
    {
        RenderMenuButtons();

        if (UnityUnitTester.TestResults == null)
        {
            return;
        }

        scrollAreaPosition = EditorGUILayout.BeginScrollView(scrollAreaPosition);

        foreach (AssemblyUnitTestResult testResult in UnityUnitTester.TestResults)
        {
            RenderAssemblyResult(testResult);
        }

        EditorGUILayout.EndScrollView();
    }

    void Update()
    {
        if (EditorApplication.isCompiling)
        {
            _wasCompilingScriptsInPreviousFrame = true;
        }
        else if (_wasCompilingScriptsInPreviousFrame)
        {
            if (RunUnitTestsAutomatically)
            {
                RunAllUnitTests();
            }

            _wasCompilingScriptsInPreviousFrame = false;
        }
    }

    private void RenderMenuButtons()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Run All Tests"))
        {
            RunAllUnitTests();
        }

        if (GUILayout.Button("Collapse All"))
        {
            ToggleFixtureExpansion(false);
        }

        if (GUILayout.Button("Expand All"))
        {
            ToggleFixtureExpansion(true);
        }

        RunUnitTestsAutomatically = EditorGUILayout.Toggle("Run Tests Automatically:", RunUnitTestsAutomatically);

        GUILayout.EndHorizontal();

        DrawSeparator();
    }

    void ToggleFixtureExpansion(bool expand)
    {
        if (expand)
        {
            foreach (AssemblyUnitTestResult result in UnityUnitTester.TestResults)
            {
                if (result.TestResult.HasChildren)
                {
                    foreach (ITestResult testFixture in result.TestResult.Children)
                    {
                        _expandedTestfixtures.Add(testFixture);
                    }
                }
            }
        }
        else
        {
            _expandedTestfixtures.Clear();
        }
    }

    private void RenderAssemblyResult(AssemblyUnitTestResult result)
    {
        if (result.TestResult.HasChildren)
        {
            foreach (ITestResult testFixture in result.TestResult.Children)
            {
                RenderTestFixtureResult(testFixture);
            }
        }
    }

    void HighlightResult(int skipCount, int failCount, int indent = 0)
    {
        if (failCount > 0)
        {
            HighlightPreviousLine(Color.red, indent);
        }
        else if (skipCount > 0)
        {
            HighlightPreviousLine(Color.yellow, indent);
        }
    }

    void RenderTestFixtureResult(ITestResult testFixture)
    {
        bool fixtureExpanded = _expandedTestfixtures.Contains(testFixture);

        fixtureExpanded = EditorGUILayout.Foldout(fixtureExpanded, testFixture.Name + " [" + testFixture.ResultState + "]");
        HighlightResult(testFixture.SkipCount, testFixture.FailCount);

        if (fixtureExpanded)
        {
            _expandedTestfixtures.Add(testFixture);
            RenderAllTestsInFixture(testFixture);
        }
        else
        {
            _expandedTestfixtures.Remove(testFixture);
        }
        
        DrawSeparator();
    }

    void RenderAllTestsInFixture(ITestResult testFixture)
    {
        if (testFixture.HasChildren)
        {
            RenderTestCaseResultHeader();
            foreach (ITestResult testCase in testFixture.Children)
            {
                RenderTestCaseResult(testCase);
            }
        }
    }

    void RenderTestCaseResultHeader()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("", GUILayout.Width(6));
        GUILayout.Label("Test Case", EditorStyles.boldLabel, GUILayout.Width(200));
        GUILayout.Label("Result", EditorStyles.boldLabel, GUILayout.Width(75));
        GUILayout.Label("Message", EditorStyles.boldLabel, GUILayout.MinWidth(400));
        HighlightPreviousLine(Color.black, 15);
        GUILayout.EndHorizontal();
    }

    void RenderTestCaseResult(ITestResult testCase)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("", GUILayout.Width(6));
        GUILayout.Label(testCase.Name, GUILayout.Width(200));

        string resultLabel = "pass";

        if (testCase.FailCount > 0)
        {
            resultLabel = "fail";
        }
        else if (testCase.InconclusiveCount > 0)
        {
            resultLabel = "inconclusive";
        }
        else if (testCase.SkipCount > 0)
        {
            resultLabel = "skipped";
        }

        GUILayout.Label(resultLabel, GUILayout.Width(75));
        GUILayout.Label(testCase.Message, GUILayout.MinWidth(400));
        HighlightResult(testCase.SkipCount, testCase.FailCount, 15);

        GUILayout.EndHorizontal();
    }

    private void DrawSeparator()
    {
        GUILayout.Space(5);
        Rect rect = GUILayoutUtility.GetLastRect();
        GUI.color = new Color(0f, 0f, 0f, 1f);
        GUI.DrawTexture(new Rect(0f, rect.yMax, Screen.width, 2f), rowBackgroundTex);
        GUI.color = Color.white;
        GUILayout.Space(5);
    }

    private void HighlightPreviousLine(Color highlightColour, int indent = 0)
    {
        Rect rect = GUILayoutUtility.GetLastRect();
        rect.width = Screen.width;
        rect.x = indent;

        if (indent > 0)
        {
            highlightColour.a *= 0.2f;
        }
        else
        {
            highlightColour.a *= 0.4f;
        }

        GUI.color = highlightColour;
        GUI.DrawTexture(rect, rowBackgroundTex);
        GUI.color = Color.white;
    }
}
