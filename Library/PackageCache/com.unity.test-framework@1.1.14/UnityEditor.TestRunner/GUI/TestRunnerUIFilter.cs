using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools.TestRunner.GUI;

namespace UnityEditor.TestTools.TestRunner.GUI
{
    [Serializable]
    internal class TestRunnerUIFilter
    {
        private int m_PassedCount;
        private int m_FailedCount;
        private int m_NotRunCount;
        private int m_InconclusiveCount;
        private int m_SkippedCount;

        public int PassedCount { get { return m_PassedCount; } }
        public int FailedCount { get { return m_FailedCount + m_InconclusiveCount; } }
        public int NotRunCount { get { return m_NotRunCount + m_SkippedCount; } }

        [SerializeField]
        public bool PassedHidden;
        [SerializeField]
        public bool FailedHidden;
        [SerializeField]
        public bool NotRunHidden;

        [SerializeField]
        private string m_SearchString;
        [SerializeField]
        private int selectedCategoryMask;

        public string[] availableCategories = new string[0];


        private GUIContent m_SucceededBtn;
        private GUIContent m_FailedBtn;
        private GUIContent m_NotRunBtn;

        public Action RebuildTestList;
        public Action<string> SearchStringChanged;
        public Action SearchStringCleared;
        public bool IsFiltering
        {
            get
            {
                return !string.IsNullOrEmpty(m_SearchString) || PassedHidden || FailedHidden || NotRunHidden ||
                    selectedCategoryMask != 0;
            }
        }

        public string[] CategoryFilter
        {
            get
            {
                var list = new List<string>();
                for (int i = 0; i < availableCategories.Length; i++)
                {
                    if ((selectedCategoryMask & (1 << i)) != 0)
                    {
                        list.Add(availableCategories[i]);
                    }
                }
                return list.ToArray();
            }
        }

        public void UpdateCounters(List<TestRunnerResult> resultList)
        {
            m_PassedCount = m_FailedCount = m_NotRunCount =  m_InconclusiveCount = m_SkippedCount = 0;
            foreach (var result in resultList)
            {
                if (result.isSuite)
                    continue;
                switch (result.resultStatus)
                {
                    case TestRunnerResult.ResultStatus.Passed:
                        m_PassedCount++;
                        break;
                    case TestRunnerResult.ResultStatus.Failed:
                        m_FailedCount++;
                        break;
                    case TestRunnerResult.ResultStatus.Inconclusive:
                        m_InconclusiveCount++;
                        break;
                    case TestRunnerResult.ResultStatus.Skipped:
                        m_SkippedCount++;
                        break;
                    case TestRunnerResult.ResultStatus.NotRun:
                    default:
                        m_NotRunCount++;
                        break;
                }
            }

            var succeededTooltip = string.Format("Show tests that succeeded\n{0} succeeded", m_PassedCount);
            m_SucceededBtn = new GUIContent(PassedCount.ToString(), Icons.s_SuccessImg, succeededTooltip);
            var failedTooltip = string.Format("Show tests that failed\n{0} failed\n{1} inconclusive", m_FailedCount, m_InconclusiveCount);
            m_FailedBtn = new GUIContent(FailedCount.ToString(), Icons.s_FailImg, failedTooltip);
            var notRunTooltip = string.Format("Show tests that didn't run\n{0} didn't run\n{1} skipped or ignored", m_NotRunCount, m_SkippedCount);
            m_NotRunBtn = new GUIContent(NotRunCount.ToString(), Icons.s_UnknownImg, notRunTooltip);
        }

        public void Draw()
        {
            EditorGUI.BeginChangeCheck();
            if (m_SearchString == null)
            {
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       