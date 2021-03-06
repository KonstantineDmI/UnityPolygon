using System;
using UnityEditor.Callbacks;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEditor.TestTools.TestRunner.GUI;
using UnityEngine;

namespace UnityEditor.TestTools.TestRunner
{
    [Serializable]
    internal class TestRunnerWindow : EditorWindow, IHasCustomMenu
    {
        internal static class Styles
        {
            public static GUIStyle info;
            public static GUIStyle testList;

            static Styles()
            {
                info = new GUIStyle(EditorStyles.wordWrappedLabel);
                info.wordWrap = false;
                info.stretchHeight = true;
                info.margin.right = 15;

                testList = new GUIStyle("CN Box");
                testList.margin.top = 0;
                testList.padding.left = 3;
            }
        }

        private readonly GUIContent m_GUIHorizontalSplit = EditorGUIUtility.TrTextContent("Horizontal layout");
        private readonly GUIContent m_GUIVerticalSplit = EditorGUIUtility.TrTextContent("Vertical layout");
        private readonly GUIContent m_GUIEnableaPlaymodeTestsRunner = EditorGUIUtility.TrTextContent("Enable playmode tests for all assemblies");
        private readonly GUIContent m_GUIDisablePlaymodeTestsRunner = EditorGUIUtility.TrTextContent("Disable playmode tests for all assemblies");
        private readonly GUIContent m_GUIRunPlayModeTestAsEditModeTests = EditorGUIUtility.TrTextContent("Run playmode tests as editmode tests");

        internal static TestRunnerWindow s_Instance;
        private bool m_IsBuilding;
        [NonSerialized]
        private bool m_Enabled;
        public TestFilterSettings filterSettings;

        [SerializeField]
        private SplitterState m_Spl = new SplitterState(new float[] { 75, 25 }, new[] { 32, 32 }, null);

        private TestRunnerWindowSettings m_Settings;

        private enum TestRunnerMenuLabels
        {
            PlayMode = 0,
            EditMode = 1
        }
        [SerializeField]
        private int m_TestTypeToolbarIndex = (int)TestRunnerMenuLabels.EditMode;
        [SerializeField]
        private PlayModeTestListGUI m_PlayModeTestListGUI;
        [SerializeField]
        private EditModeTestListGUI m_EditModeTestListGUI;

        internal TestListGUI m_SelectedTestTypes;

        private ITestRunnerApi m_testRunnerApi;

        private WindowResultUpdater m_WindowResultUpdater;

        [MenuItem("Window/General/Test Runner", false, 201, false)]
        public static void ShowPlaymodeTestsRunnerWindowCodeBased()
        {
            s_Instance = GetWindow<TestRunnerWindow>("Test Runner");
            s_Instance.Show();
        }

        static TestRunnerWindow()
        {
            InitBackgroundRunners();
        }

        private static void InitBackgroundRunners()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        [DidReloadScripts]
        private static void CompilationCallback()
        {
            UpdateWindow();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (s_Instance && state == PlayModeStateChange.EnteredEditMode && s_Instance.m_SelectedTestTypes.HasTreeData())
            {
                //repaint message details after exit playmode
                s_Instance.m_SelectedTestTypes.TestSelectionCallback(s_Instance.m_SelectedTestTypes.m_TestListState.selectedIDs.ToArray());
                s_Instance.Repaint();
            }
        }

        public void OnDestroy()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnEnable()
        {
            s_Instance = this;
            SelectTestListGUI(m_TestTypeToolbarIndex);

            m_testRunnerApi = ScriptableObject.CreateInstance<TestRunnerApi>();
            m_WindowResultUpdater = new WindowRes                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         