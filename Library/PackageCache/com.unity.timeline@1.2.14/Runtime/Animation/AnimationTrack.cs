                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                t<AnimationTrack> flattenTracks = new List<AnimationTrack>();
            if (CanCompileClips())
                flattenTracks.Add(this);


            bool animatesRootTransform = AnimatesRootTransform();
            foreach (var subTrack in GetChildTracks())
            {
                var child = subTrack as AnimationTrack;
                if (child != null && child.CanCompileClips())
                {
                    animatesRootTransform |= child.AnimatesRootTransform();
                    flattenTracks.Add(child);
                }
            }

            // figure out which mode to apply
            AppliedOffsetMode mode = GetOffsetMode(go, animatesRootTransform);
            var layerMixer = CreateGroupMixer(graph, go, flattenTracks.Count);
            for (int c = 0; c < flattenTracks.Count; c++)
            {
                var compiledTrackPlayable = flattenTracks[c].inClipMode ?
                    CompileTrackPlayable(graph, flattenTracks[c], go, tree, mode) :
                    flattenTracks[c].CreateInfiniteTrackPlayable(graph, go, tree, mode);
                graph.Connect(compiledTrackPlayable, 0, layerMixer, c);
                layerMixer.SetInputWeight(c, flattenTracks[c].inClipMode ? 0 : 1);
                if (flattenTracks[c].applyAvatarMask && flattenTracks[c].avatarMask != null)
                {
                    layerMixer.SetLayerMaskFromAvatarMask((uint)c, flattenTracks[c].avatarMask);
                }
            }

            bool requiresMotionXPlayable = RequiresMotionXPlayable(mode, go);

            Playable mixer = layerMixer;
            mixer = CreateDefaultBlend(graph, go, mixer, requiresMotionXPlayable);

            // motionX playable not required in scene offset mode, or root transform mode
            if (requiresMotionXPlayable)
            {
                // If we are animating a root transform, add the motionX to delta playable as the root node
                var motionXToDelta = AnimationMotionXToDeltaPlayable.Create(graph);
                graph.Connect(mixer, 0, motionXToDelta, 0);
                motionXToDelta.SetInputWeight(0, 1.0f);
                motionXToDelta.SetAbsoluteMotion(UsesAbsoluteMotion(mode));
                mixer = (Playable)motionXToDelta;
            }


#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                var animator = GetBinding(go != null ? go.GetComponent<PlayableDirector>() : null);
                if (animator != null)
                {
                    GameObject targetGO = animator.gameObject;
                    IAnimationWindowPreview[] previewComponents = targetGO.GetComponents<IAnimationWindowPreview>();

                    m_HasPreviewComponents = previewComponents.Length > 0;
                    if (m_HasPreviewComponents)
                    {
                        foreach (var component in previewComponents)
                        {
                            mixer = component.BuildPreviewGraph(graph, mixer);
                        }
                    }
                }
            }
#endif

            return mixer;
        }

        // Creates a layer mixer containing default blends
        // the base layer is a default clip of all driven properties
        // the next layer is optionally the desired default pose (in the case of humanoid, the tpose
        private Playable CreateDefaultBlend(PlayableGraph graph, GameObject go, Playable mixer, bool requireOffset)
        {
#if  UNITY_EDITOR
            if (Application.isPlaying)
                return mixer;

            int inputs = 1 + ((m_CachedPropertiesClip != null) ? 1 : 0) + ((m_DefaultPoseClip != null) ? 1 : 0);
            if (inputs == 1)
                return mixer;

            var defaultPoseMixer = AnimationLayerMixerPlayable.Create(graph, inputs);

            int mixerInput = 0;
            if (m_CachedPropertiesClip)
            {
                var cachedPropertiesClip = AnimationClipPlayable.Create(graph, m_CachedPropertiesClip);
                cachedPropertiesClip.SetApplyFootIK(false);
                var defaults = (Playable) cachedPropertiesClip;
                if (requireOffset)
                    defaults = AttachOffsetPlayable(graph, defaults, m_SceneOffsetPosition, Quaternion.Euler(m_SceneOffsetRotation));
                graph.Connect(defaults, 0, defaultPoseMixer, mixerInput);
                defaultPoseMixer.SetInputWeight(mixerInput, 1.0f);
                mixerInput++;
            }

            if (m_DefaultPoseClip)
            {
                var defaultPose = AnimationClipPlayable.Create(graph, m_DefaultPoseClip);
                defaultPose.SetApplyFootIK(false);
                var blendDefault = (Playable) defaultPose;
                if (requireOffset)
                    blendDefault = AttachOffsetPlayable(graph, blendDefault, m_SceneOffsetPosition, Quaternion.Euler(m_SceneOffsetRotation));

                graph.Connect(blendDefault, 0, defaultPoseMixer, mixerInput);
                defaultPoseMixer.SetInputWeight(mixerInput, 1.0f);
                mixerInput++;
            }


            graph.Connect(mixer, 0, defaultPoseMixer, mixerInput);
            defaultPoseMixer.SetInputWeight(mixerInput, 1.0f);

            return defaultPoseMixer;
#else
            return mixer;
#endif
        }

        private Playable AttachOffsetPlayable(PlayableGraph graph, Playable playable, Vector3 pos, Quaternion rot)
        {
            var offsetPlayable = AnimationOffsetPlayable.Create(graph, pos, rot, 1);
            offsetPlayable.SetInputWeight(0, 1.0f);
            graph.Connect(playable, 0, offsetPlayable, 0);
            return offsetPlayable;
        }

#if UNITY_EDITOR
        private static string k_DefaultHumanoidClipPath = "Packages/com.unity.timeline/Editor/StyleSheets/res/HumanoidDefault.anim";
        private static AnimationClip s_DefaultHumanoidClip = null;

        AnimationClip GetDefaultHumanoidClip()
        {
            if (s_DefaultHumanoidClip == null)
            {
                s_DefaultHumanoidClip = EditorGUIUtility.LoadRequired(k_DefaultHumanoidClipPath) as AnimationClip;
                if (s_DefaultHumanoidClip == null)
                    Debug.LogError("Could not load default humanoid animation clip for Timeline");
            }

            return s_DefaultHumanoidClip;
        }

#endif

        bool RequiresMotionXPlayable(AppliedOffsetMode mode, GameObject gameObject)
        {
            if (mode == AppliedOffsetMode.NoRootTransform)
                return false;
            if (mode == AppliedOffsetMode.SceneOffsetLegacy)
            {
                var animator = GetBinding(gameObject != null ? gameObject.GetComponent<PlayableDirector>() : null);
                return animator != null && animator.hasRootMotion;
            }
            return true;
        }

        static bool UsesAbsoluteMotion(AppliedOffsetMode mode)
        {
#if UNITY_EDITOR
            // in editor, previewing is always done in absolute motion
            if (!Application.isPlaying)
                return true;
#endif
            return mode != AppliedOffsetMode.SceneOffset &&
                mode != AppliedOffsetMode.SceneOffsetLegacy;
        }

        bool HasController(GameObject gameObject)
        {
            var animator = GetBinding(gameObject != null ? gameObject.GetComponent<PlayableDirector>() : null);

            return animator != null && animator.runtimeAnimatorController != null;
        }

        internal Animator GetBinding(PlayableDirector director)
        {
            if (director == null)
                return null;

            UnityEngine.Object key = this;
            if (isSubTrack)
                key = parent;

            UnityEngine.Object binding = null;
            if (director != null)
                binding = director.GetGenericBinding(key);

            Animator animator = null;
            if (binding != null) // the binding can be an animator or game object
            {
                animator = binding as Animator;
                var gameObject = binding as GameObject;
                if (animator == null && gameObject != null)
                    animator = gameObject.GetComponent<Animator>();
            }

            return animator;
        }

        static AnimationLayerMixerPlayable CreateGroupMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return AnimationLayerMixerPlayable.Create(graph, inputCount);
        }

        Playable CreateInfiniteTrackPlayable(PlayableGraph graph, GameObject go, IntervalTree<RuntimeElement> tree, AppliedOffsetMode mode)
        {
            if (m_InfiniteClip == null)
                return Playable.Null;

            var mixer = AnimationMixerPlayable.Create(graph, 1);

            // In infinite mode, we always force the loop mode of the clip off because the clip keys are offset in infinite mode
            //  which causes loop to behave different.
            // The inline curve editor never shows loops in infinite mode.
            var playable = AnimationPlayableAsset.CreatePlayable(graph, m_InfiniteClip, m_InfiniteClipOffsetPosition, m_InfiniteClipOffsetEulerAngles, false, mode, infiniteClipApplyFootIK, AnimationPlayableAsset.LoopMode.Off);
            if (playable.IsValid())
            {
                tree.Add(new InfiniteRuntimeClip(playable));
                graph.Connect(playable, 0, mixer, 0);
                mixer.SetInputWeight(0, 1.0f);
            }

            return ApplyTrackOffset(graph, mixer, go, mode);
        }

        Playable ApplyTrackOffset(PlayableGraph graph, Playable root, GameObject go, AppliedOffsetMode mode)
        {
#if UNITY_EDITOR
            m_ClipOffset = AnimationOffsetPlayable.Null;
#endif

            // offsets don't apply in scene offset, or if there is no root transform (globally or on this track)
            if (mode == AppliedOffsetMode.SceneOffsetLegacy ||
                mode == AppliedOffsetMode.SceneOffset     ||
                mode == AppliedOffsetMode.NoRootTransform ||
                !AnimatesRootTransform()
            )
                return root;


            var pos = position;
            var rot = rotation;

#if UNITY_EDITOR
            // in the editor use the preview position to playback from if available
            if (mode == AppliedOffsetMode.SceneOffsetEditor)
            {
                pos = m_SceneOffsetPosition;
                rot = Quaternion.Euler(m_SceneOffsetRotation);
            }
#endif

            var offsetPlayable = AnimationOffsetPlayable.Create(graph, pos, rot, 1);
#if UNITY_EDITOR
            m_ClipOffset = offsetPlayable;
#endif
            graph.Connect(root, 0, offsetPlayable, 0);
            offsetPlayable.SetInputWeight(0, 1);

            return offsetPlayable;
        }

        // the evaluation time is large so that the properties always get evaluated
        internal override void GetEvaluationTime(out double outStart, out double outDuration)
        {
            if (inClipMode)
            {
                base.GetEvaluationTime(out outStart, out outDuration);
            }
            else
            {
                outStart = 0;
                outDuration = TimelineClip.kMaxTimeValue;
            }
        }

        internal override void GetSequenceTime(out double outStart, out double outDuration)
        {
            if (inClipMode)
            {
                base.GetSequenceTime(out outStart, out outDuration);
            }
            else
            {
                outStart = 0;
                outDuration = Math.Max(GetNotificationDuration(), TimeUtility.GetAnimationClipLength(m_InfiniteClip));
            }
        }

        void AssignAnimationClip(TimelineClip clip, AnimationClip animClip)
        {
            if (clip == null || animClip == null)
                return;

            if (animClip.legacy)
                throw new InvalidOperationException("Legacy Animation Clips are not supported");

            AnimationPlayableAsset asset = clip.asset as AnimationPlayableAsset;
            if (asset != null)
            {
                asset.clip = animClip;
                asset.name = animClip.name;
                var duration = asset.duration;
                if (!double.IsInfinity(duration) && duration >= TimelineClip.kMinDuration && duration < TimelineClip.kMaxTimeValue)
                    clip.duration = duration;
            }
            clip.displayName = animClip.name;
        }

        /// <summary>
        /// Called by the Timeline Editor to gather properties requiring preview.
        /// </summary>
        /// <param name="director">The PlayableDirector invoking the preview</param>
        /// <param name="driver">PropertyCollector used to gather previewable properties</param>
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
#if UNITY_EDITOR
            m_SceneOffsetPosition = Vector3.zero;
            m_SceneOffsetRotation = Vector3.zero;

            var animator = GetBinding(director);
            if (animator == null)
                return;

            var animClips = new List<AnimationClip>(this.clips.Length + 2);
            GetAnimationClips(animClips);

            var hasHumanMotion = animClips.Exists(clip => clip.humanMotion);

            m_SceneOffsetPosition = animator.transform.localPosition;
            m_SceneOffsetRotation = animator.transform.localEulerAngles;

            // Create default pose clip from collected properties
            if (hasHumanMotion)
                animClips.Add(GetDefaultHumanoidClip());

            var bindings = AnimationPreviewUtilities.GetBindings(animator.gameObject, animClips);

            m_CachedPropertiesClip = AnimationPreviewUtilities.CreateDefaultClip(animator.gameObject, bindings);
            AnimationPreviewUtilities.PreviewFromCurves(animator.gameObject, bindings); // faster to preview from curves then an animation clip
            m_DefaultPoseClip = hasHumanMotion ? GetDefaultHumanoidClip() : null;
#endif
        }

        /// <summary>
        /// Gather all the animation clips for this track
        /// </summary>
        /// <param name="animClips"></param>
        private void GetAnimationClips(List<AnimationClip> animClips)
        {
            foreach (var c in clips)
            {
                var a = c.asset as AnimationPlayableAsset;
                if (a != null && a.clip != null)
                    animClips.Add(a.clip);
            }

            if (m_InfiniteClip != null)
                animClips.Add(m_InfiniteClip);

            foreach (var childTrack in GetChildTracks())
            {
                var animChildTrack = childTrack as AnimationTrack;
                if (animChildTrack != null)
                    animChildTrack.GetAnimationClips(animClips);
            }
        }

        // calculate which offset mode to apply
        AppliedOffsetMode GetOffsetMode(GameObject go, bool animatesRootTransform)
        {
            if (!animatesRootTransform)
                return AppliedOffsetMode.NoRootTransform;

            if (m_TrackOffset == TrackOffset.ApplyTransformOffsets)
                return AppliedOffsetMode.TransformOffset;

            if (m_TrackOffset == TrackOffset.ApplySceneOffsets)
                return (Application.isPlaying) ? AppliedOffsetMode.SceneOffset : AppliedOffsetMode.SceneOffsetEditor;

            if (HasController(go))
            {
                if (!Application.isPlaying)
                    return AppliedOffsetMode.SceneOffsetLegacyEditor;
                return AppliedOffsetMode.SceneOffsetLegacy;
            }

            return AppliedOffsetMode.TransformOffsetLegacy;
        }

        internal bool AnimatesRootTransform()
        {
            // infinite mode
            if (AnimationPlayableAsset.HasRootTransforms(m_InfiniteClip))
                return true;

            // clip mode
            foreach (var c in GetClips())
            {
                var apa = c.asset as AnimationPlayableAsset;
                if (apa != null && apa.hasRootTransforms)
                    return true;
            }

            return false;
        }
    }
}
