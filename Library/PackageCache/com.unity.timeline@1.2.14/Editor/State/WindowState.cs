using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine.Experimental.Animations;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Animations;

namespace UnityEditor.Timeline
{
    delegate bool PendingUpdateDelegate(WindowState state, Event currentEvent);

    class WindowState
    {
        const int k_TimeCodeTextFieldId = 3790;

        readonly TimelineWindow m_Window;
        bool m_Recording;
        readonly SpacePartitioner m_SpacePartitioner = new SpacePartitioner();
        readonly List<Manipulator> m_CaptureSession = new List<Manipulator>();
        int m_DirtyStamp;
        float m_SequencerHeaderWidth = WindowConstants.defaultHeaderWidth;
        float m_BindingAreaWidth = WindowConstants.defaultBindingAreaWidth;

        bool m_MustRebuildGraph;

        float m_LastTime;

        readonly PropertyCollector m_PropertyCollector = new PropertyCollector();

        static AnimationModeDriver s_PreviewDriver;
        List<Animator> m_PreviewedAnimators;
        List<IAnimationWindowPreview> m_PreviewedComponents;

        public static double kTimeEpsilon { get { return TimeUtility.kTimeEpsilon; } }
        public static readonly float kMaxShownTime = (float)TimeUtility.k_MaxTimelineDurationInSeconds;

        static readonly ISequenceState k_NullSequenceState = new NullSequenceState();

        // which tracks are armed for record - only one allowed per 'actor'
        Dictionary<TrackAsset, TrackAsset> m_ArmedTracks = new Dictionary<TrackAsset, TrackAsset>();

        TimelineWindow.TimelineWindowPreferences m_Preferences;

        List<PendingUpdateDelegate> m_OnStartFrameUpdates;
        List<PendingUpdateDelegate> m_OnEndFrameUpdates;

        readonly SequenceHierarchy m_SequenceHierarchy;

        public event Action<WindowState, Event> windowOnGuiStarted;
        public event Action<WindowState, Event> windowOnGuiFinished;

        public event Action<bool> OnPlayStateChange;
        public event Action OnDirtyStampChange;
        public event Action OnRebuildGraphChange;
        public event Action OnTimeChange;
        public event Action OnRecordingChange;

        public event Action OnBeforeSequenceChange;
        public event Action OnAfterSequenceChange;

        public WindowState(TimelineWindow w, SequenceHierarchy hierarchy)
        {
            m_Window = w;
            m_Preferences = w.preferences;
            hierarchy.Init(this);
            m_SequenceHierarchy = hierarchy;
            TimelinePlayable.muteAudioScrubbing = muteAudioScrubbing;
        }

        public static AnimationModeDriver previewDriver
        {
            get
            {
                if (s_PreviewDriver == null)
                    s_PreviewDriver = ScriptableObject.CreateInstance<AnimationModeDriver>();
                return s_PreviewDriver;
            }
        }

        public EditorWindow editorWindow
        {
            get { return m_Window; }
        }

        public ISequenceState editSequence
        {
            get
            {
                // Using "null" ISequenceState to avoid checking against null all the time.
                // This *should* be removed in a phase 2 of refactoring, where we make sure
                // to pass around the correct state object instead of letting clients dig
                // into the WindowState for whatever they want.
                return m_SequenceHierarchy.editSequence ?? k_NullSequenceState;
            }
        }

        public ISequenceState masterSequence
        {
            get { return m_SequenceHierarchy.masterSequence ?? k_NullSequenceState; }
        }

        public ISequenceState referenceSequence
        {
            get { return timeReferenceMode == TimeReferenceMode.Local ? editSequence : masterSequence; }
        }

        public bool rebuildGraph
        {
            get { return m_MustRebuildGraph; }
            set { SyncNotifyValue(ref m_MustRebuildGraph, value, OnRebuil                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                reviewMode = true;

            if (previewMode)
            {
                if (masterSequence.time > masterSequence.duration)
                    masterSequence.time = 0;

                masterSequence.director.Play();
                masterSequence.director.ProcessPendingGraphChanges();
                PlayableDirector.ResetFrameTiming();
                InvokePlayStateChangeCallback(true);
            }
        }

        public void Pause()
        {
            if (masterSequence.director != null)
            {
                masterSequence.director.Pause();
                masterSequence.director.ProcessPendingGraphChanges();
                SynchronizeSequencesAfterPlayback();
                InvokePlayStateChangeCallback(false);
            }
        }

        public void SetPlaying(bool start)
        {
            if (start && !playing)
            {
                Play();
            }

            if (!start && playing)
            {
                Pause();
            }
        }

        public void Stop()
        {
            if (masterSequence.director != null)
            {
                masterSequence.director.Stop();
                masterSequence.director.ProcessPendingGraphChanges();
                InvokePlayStateChangeCallback(false);
            }
        }

        void InvokePlayStateChangeCallback(bool isPlaying)
        {
            if (OnPlayStateChange != null)
                OnPlayStateChange.Invoke(isPlaying);
        }

        public void RebuildPlayableGraph()
        {
            if (masterSequence.director != null)
            {
                masterSequence.director.RebuildGraph();
                // rebuild both the parent and the edit sequences. control tracks don't necessary
                // rebuild the subdirector on recreation
                if (editSequence.director != null && editSequence.director != masterSequence.director)
                {
                    editSequence.director.RebuildGraph();
                }
            }
        }

        public void Evaluate()
        {
            if (masterSequence.director != null)
            {
                if (!EditorApplication.isPlaying && !previewMode)
                    GatherProperties(masterSequence.director);

                ForceTimeOnDirector(masterSequence.director);
                masterSequence.director.DeferredEvaluate();

                if (EditorApplication.isPlaying == false)
                {
                    PlayModeView.RepaintAll();
                    SceneView.RepaintAll();
                    AudioMixerWindow.RepaintAudioMixerWindow();
                }
            }
        }

        public void EvaluateImmediate()
        {
            if (masterSequence.director != null)
            {
                if (!EditorApplication.isPlaying && !previewMode)
                    GatherProperties(masterSequence.director);

                if (previewMode)
                {
                    ForceTimeOnDirector(masterSequence.director);
                    masterSequence.director.ProcessPendingGraphChanges();
                    masterSequence.director.Evaluate();
                }
            }
        }

        public void Refresh()
        {
            CheckRecordingState();
            dirtyStamp = dirtyStamp + 1;

            rebuildGraph = true;
        }

        public void UpdateViewStateHash()
        {
            viewStateHash = timeAreaTranslation.GetHashCode()
                .CombineHash(timeAreaScale.GetHashCode())
                .CombineHash(trackScale.GetHashCode());
        }

        public bool IsEditingASubItem()
        {
            return IsCurrentEditingASequencerTextField() || !SelectionManager.IsCurveEditorFocused(null);
        }

        public bool IsEditingASubTimeline()
        {
            return editSequence != masterSequence;
        }

        public bool IsEditingAnEmptyTimeline()
        {
            return editSequence.asset == null;
        }

        public bool IsEditingAPrefabAsset()
        {
            var stage = PrefabStageUtility.GetCurrentPrefabStage();
            return stage != null && editSequence.director != null && stage.IsPartOfPrefabContents(editSequence.director.gameObject);
        }

        public bool IsCurrentEditingASequencerTextField()
        {
            if (editSequence.asset == null)
                return false;

            if (k_TimeCodeTextFieldId == GUIUtility.keyboardControl)
                return true;

            return editSequence.asset.flattenedTracks.Count(t => t.GetInstanceID() == GUIUtility.keyboardControl) != 0;
        }

        public float TimeToTimeAreaPixel(double t) // TimeToTimeAreaPixel
        {
            float pixelX = (float)t;
            pixelX *= timeAreaScale.x;
            pixelX += timeAreaTranslation.x + sequencerHeaderWidth;
            return pixelX;
        }

        public float TimeToScreenSpacePixel(double time)
        {
            float pixelX = (float)time;
            pixelX *= timeAreaScale.x;
            pixelX += timeAreaTranslation.x;
            return pixelX;
        }

        public float TimeToPixel(double time)
        {
            return m_Window.timeArea.TimeToPixel((float)time, timeAreaRect);
        }

        public float PixelToTime(float pixel)
        {
            return m_Window.timeArea.PixelToTime(pixel, timeAreaRect);
        }

        public float PixelDeltaToDeltaTime(float p)
        {
            return PixelToTime(p) - PixelToTime(0);
        }

        public float TimeAreaPixelToTime(float pixel)
        {
            return PixelToTime(pixel);
        }

        public float ScreenSpacePixelToTimeAreaTime(float p)
        {
            // transform into track space by offsetting the pixel by the screen-space offset of the time area
            p -= timeAreaRect.x;
            return TrackSpacePixelToTimeAreaTime(p);
        }

        public float TrackSpacePixelToTimeAreaTime(float p)
        {
            p -= timeAreaTranslation.x;

            if (timeAreaScale.x > 0.0f)
                return p / timeAreaScale.x;

            return p;
        }

        public void OffsetTimeArea(int pixels)
        {
            Vector3 tx = timeAreaTranslation;
            tx.x += pixels;
            SetTimeAreaTransform(tx, timeAreaScale);
        }

        public GameObject GetSceneReference(TrackAsset asset)
        {
            if (editSequence.director == null)
                return null; // no player bound

            return TimelineUtility.GetSceneGameObject(editSequence.director, asset);
        }

        public void CalculateRowRects()
        {
            // arming a track might add inline curve tracks, recalc track heights
            if (m_Window != null && m_Window.treeView != null)
                m_Window.treeView.CalculateRowRects();
        }

        // Only one track within a 'track' hierarchy can be armed
        public void ArmForRecord(TrackAsset track)
        {
            m_ArmedTracks[TimelineUtility.GetSceneReferenceTrack(track)] = track;
            if (track != null && !recording)
                recording = true;
            if (!recording)
                return;

            track.OnRecordingArmed(editSequence.director);
            CalculateRowRects();
        }

        public void UnarmForRecord(TrackAsset track)
        {
            m_ArmedTracks.Remove(TimelineUtility.GetSceneReferenceTrack(track));
            if (m_ArmedTracks.Count == 0)
                recording = false;
            track.OnRecordingUnarmed(editSequence.director);
        }

        public void UpdateRecordingState()
        {
            if (recording)
            {
                foreach (var track in m_ArmedTracks.Values)
                {
                    if (track != null)
                        track.OnRecordingTimeChanged(editSequence.director);
                }
            }
        }

        public bool IsTrackRecordable(TrackAsset track)
        {
            // A track with animated parameters can always be recorded to
            return IsArmedForRecord(track) || track.HasAnyAnimatableParameters();
        }

        public bool IsArmedForRecord(TrackAsset track)
        {
            return track == GetArmedTrack(track);
        }

        public TrackAsset GetArmedTrack(TrackAsset track)
        {
            TrackAsset outTrack;
            m_ArmedTracks.TryGetValue(TimelineUtility.GetSceneReferenceTrack(track), out outTrack);
            return outTrack;
        }

        void CheckRecordingState()
        {
            // checks for deleted tracks, and makes sure the recording state matches
            if (m_ArmedTracks.Any(t => t.Value == null))
            {
                m_ArmedTracks = m_ArmedTracks.Where(t => t.Value != null).ToDictionary(t => t.Key, t => t.Value);
                if (m_ArmedTracks.Count == 0)
                    recording = false;
            }
        }

        void OnCurrentDirectorWillChange()
        {
            SynchronizeViewModelTime(editSequence);

            if (!Application.isPlaying)
                Stop();

            rebuildGraph = true; // needed for asset previews
        }

        public void GatherProperties(PlayableDirector director)
        {
            if (director == null || Application.isPlaying)
                return;

            var asset = director.playableAsset as TimelineAsset;

            if (!previewMode)
            {
                AnimationMode.StartAnimationMode(previewDriver);

                OnStartPreview(director);

                AnimationPropertyContextualMenu.Instance.SetResponder(new TimelineRecordingContextualResponder(this));
                if (!previewMode)
                    return;
                EnsureWindowTimeConsistency();
            }

            if (asset != null)
            {
                m_PropertyCollector.Reset();
                m_PropertyCollector.PushActiveGameObject(null); // avoid overflow on unbound tracks
                asset.GatherProperties(director, m_PropertyCollector);
            }
        }

        void OnStartPreview(PlayableDirector director)
        {
            previewedDirectors = TimelineUtility.GetAllDirectorsInHierarchy(director).ToList();

            if (previewedDirectors == null)
                return;

            m_PreviewedAnimators = TimelineUtility.GetBindingsFromDirectors<Animator>(previewedDirectors).ToList();

            m_PreviewedComponents = new List<IAnimationWindowPreview>();
            foreach (var animator in m_PreviewedAnimators)
            {
                m_PreviewedComponents.AddRange(animator.GetComponents<IAnimationWindowPreview>());
            }
            foreach (var previewedComponent in m_PreviewedComponents)
            {
                previewedComponent.StartPreview();
            }
        }

        void OnStopPreview()
        {
            if (m_PreviewedComponents != null)
            {
                foreach (var previewComponent in m_PreviewedComponents)
                {
                    if (previewComponent != null)
                    {
                        previewComponent.StopPreview();
                    }
                }
                m_PreviewedComponents = null;
            }

            if (m_PreviewedAnimators != null)
            {
                foreach (var previewAnimator in m_PreviewedAnimators)
                {
                    if (previewAnimator != null)
                    {
                        previewAnimator.UnbindAllHandles();
                    }
                }
                m_PreviewedAnimators = null;
            }
        }

        internal void ProcessStartFramePendingUpdates()
        {
            if (m_OnStartFrameUpdates != null)
                m_OnStartFrameUpdates.RemoveAll(callback => callback.Invoke(this, Event.current));
        }

        internal void ProcessEndFramePendingUpdates()
        {
            if (m_OnEndFrameUpdates != null)
                m_OnEndFrameUpdates.RemoveAll(callback => callback.Invoke(this, Event.current));
        }

        public void AddStartFrameDelegate(PendingUpdateDelegate updateDelegate)
        {
            if (m_OnStartFrameUpdates == null)
                m_OnStartFrameUpdates = new List<PendingUpdateDelegate>();
            if (m_OnStartFrameUpdates.Contains(updateDelegate))
                return;
            m_OnStartFrameUpdates.Add(updateDelegate);
        }

        public void AddEndFrameDelegate(PendingUpdateDelegate updateDelegate)
        {
            if (m_OnEndFrameUpdates == null)
                m_OnEndFrameUpdates = new List<PendingUpdateDelegate>();
            if (m_OnEndFrameUpdates.Contains(updateDelegate))
                return;
            m_OnEndFrameUpdates.Add(updateDelegate);
        }

        internal void InvokeWindowOnGuiStarted(Event evt)
        {
            if (windowOnGuiStarted != null)
                windowOnGuiStarted.Invoke(this, evt);
        }

        internal void InvokeWindowOnGuiFinished(Event evt)
        {
            if (windowOnGuiFinished != null)
                windowOnGuiFinished.Invoke(this, evt);
        }

        public void UpdateRootPlayableDuration(double duration)
        {
            if (editSequence.director != null)
            {
                if (editSequence.director.playableGraph.IsValid())
                {
                    if (editSequence.director.playableGraph.GetRootPlayableCount() > 0)
                    {
                        var rootPlayable = editSequence.director.playableGraph.GetRootPlayable(0);
                        if (rootPlayable.IsValid())
                            rootPlayable.SetDuration(duration);
                    }
                }
            }
        }

        public void InvokeTimeChangeCallback()
        {
            if (OnTimeChange != null)
                OnTimeChange.Invoke();
        }

        Vector2 ValidatePlayRange(Vector2 range)
        {
            if (range == TimelineAssetViewModel.NoPlayRangeSet)
                return range;

            float minimumPlayRangeTime = 0.01f / Mathf.Max(1.0f, referenceSequence.frameRate);

            // Validate min
            if (range.y - range.x < minimumPlayRangeTime)
                range.x = range.y - minimumPlayRangeTime;

            if (range.x < 0.0f)
                range.x = 0.0f;

            // Validate max
            if (range.y > editSequence.duration)
                range.y = (float)editSequence.duration;

            if (range.y - range.x < minimumPlayRangeTime)
                range.y = Mathf.Min(range.x + minimumPlayRangeTime, (float)editSequence.duration);

            return range;
        }

        void EnsureWindowTimeConsistency()
        {
            if (Application.isPlaying || masterSequence.director == null || masterSequence.viewModel == null)
                return;

            masterSequence.time = masterSequence.viewModel.windowTime;
        }

        void SynchronizeSequencesAfterPlayback()
        {
            // Synchronizing editSequence will synchronize all view models up to the master
            SynchronizeViewModelTime(editSequence);
        }

        static void SynchronizeViewModelTime(ISequenceState state)
        {
            if (state.director == null || state.viewModel == null)
                return;

            var t = state.time;
            state.time = t;
        }

        // because we may be evaluating outside the duration of the root playable
        //  we explicitly set the time - this causes the graph to not 'advance' the time
        //  because advancing it can force it to change due to wrapping to the duration
        // This can happen if the graph is force evaluated outside it's duration
        // case 910114, 936844 and 943377
        static void ForceTimeOnDirector(PlayableDirector director)
        {
            var directorTime = director.time;
            director.time = directorTime;
        }
    }
}
