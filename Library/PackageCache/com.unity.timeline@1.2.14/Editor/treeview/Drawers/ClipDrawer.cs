                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                ons != null)
                    DrawClipIcons(rightIcons, IconData.Side.Right, neededTextWidth, availableRect);
            }

            if (neededTextWidth > availableRect.width)
                s_TitleContent.text = DirectorStyles.Elipsify(title, availableRect.width, neededTextWidth);

            s_TitleContent.tooltip = tooltipMessage;
            DrawClipName(availableRect, s_TitleContent, textColor);
        }

        static void DrawClipIcons(IconData[] icons, IconData.Side side, float labelWidth, Rect availableRect)
        {
            var halfText = labelWidth / 2.0f;
            var offset = halfText + k_IconsPadding;

            foreach (var iconData in icons)
            {
                offset += iconData.width / 2.0f + k_IconsPadding;

                var iconRect =
                    new Rect(0.0f, 0.0f, iconData.width, iconData.height)
                {
                    center = new Vector2(availableRect.center.x + offset * (int)side, availableRect.center.y)
                };

                DrawIcon(iconRect, iconData.tint, iconData.icon);

                offset += iconData.width / 2.0f;
            }
        }

        static void DrawClipName(Rect rect, GUIContent content, Color textColor)
        {
            Graphics.ShadowLabel(rect, content, DirectorStyles.Instance.fontClip, textColor, Color.black);
        }

        static void DrawIcon(Rect imageRect, Color color, Texture2D icon)
        {
            GUI.DrawTexture(imageRect, icon, ScaleMode.ScaleAndCrop, true, 0, color, 0, 0);
        }

        static void DrawClipBackground(Rect clipCenterSection, bool selected)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            var color = selected ? DirectorStyles.Instance.customSkin.clipSelectedBckg : DirectorStyles.Instance.customSkin.clipBckg;
            EditorGUI.DrawRect(clipCenterSection, color);
        }

        static Vector3[] s_BlendVertices = new Vector3[3];
        static void DrawClipBlends(ClipBlends blends, Color inColor, Color outColor, Color backgroundColor)
        {
            switch (blends.inKind)
            {
                case BlendKind.Ease:
                    //     2
                    //   / |
                    //  /  |
                    // 0---1
                    EditorGUI.DrawRect(blends.inRect, backgroundColor);
                    s_BlendVertices[0] = new Vector3(blends.inRect.xMin, blends.inRect.yMax);
                    s_BlendVertices[1] = new Vector3(blends.inRect.xMax, blends.inRect.yMax);
                    s_BlendVertices[2] = new Vector3(blends.inRect.xMax, blends.inRect.yMin);
                    Graphics.DrawPolygonAA(inColor, s_BlendVertices);
                    break;
                case BlendKind.Mix:
                    // 0---2
                    //  \  |
                    //   \ |
                    //     1
                    s_BlendVertices[0] = new Vector3(blends.inRect.xMin, blends.inRect.yMin);
                    s_BlendVertices[1] = new Vector3(blends.inRect.xMax, blends.inRect.yMax);
                    s_BlendVertices[2] = new Vector3(blends.inRect.xMax, blends.inRect.yMin);
                    Graphics.DrawPolygonAA(inColor, s_BlendVertices);
                    break;
            }

            if (blends.outKind != BlendKind.None)
            {
                if (blends.outKind == BlendKind.Ease)
                    EditorGUI.DrawRect(blends.outRect, backgroundColor);
                // 0
                // | \
                // |  \
                // 1---2
                s_BlendVertices[0] = new Vector3(blends.outRect.xMin, blends.outRect.yMin);
                s_BlendVertices[1] = new Vector3(blends.outRect.xMin, blends.outRect.yMax);
                s_BlendVertices[2] = new Vector3(blends.outRect.xMax, blends.outRect.yMax);
                Graphics.DrawPolygonAA(outColor, s_BlendVertices);
            }
        }

        static void DrawClipSwatch(Rect targetRect, Color swatchColor)
        {
            // Draw Colored Line at the bottom.
            var colorRect = targetRect;
            colorRect.yMin = colorRect.yMax - k_ClipSwatchLineThickness;
            EditorGUI.DrawRect(colorRect, swatchColor);
        }

        public static void DrawSimpleClip(TimelineClip clip, Rect targetRect, ClipBorder border, Color overlay, ClipDrawOptions drawOptions)
        {
            GUI.BeginClip(targetRect);
            var clipRect = new Rect(0.0f, 0.0f, targetRect.width, targetRect.height);

            var orgColor = GUI.color;
            GUI.color = overlay;

            DrawClipBackground(clipRect, false);
            GUI.color = orgColor;

            if (clipRect.width <= k_MinClipWidth)
            {
                clipRect.width = k_MinClipWidth;
            }

            DrawClipSwatch(targetRect, drawOptions.highlightColor * overlay);

            if (targetRect.width >= k_ClipInOutMinWidth)
                DrawClipInOut(clipRect, clip);

            var textRect = clipRect;

            textRect.xMin += k_ClipLabelPadding;
            textRect.xMax -= k_ClipLabelPadding;

            if (textRect.width > k_ClipLabelMinWidth)
                DrawClipLabel(clip.displayName, textRect, Color.white, drawOptions.errorText);

            if (border != null)
                DrawClipSelectionBorder(clipRect, border, ClipBlends.kNone);

            GUI.EndClip();
        }

        public static void DrawDefaultClip(ClipDrawData drawData)
        {
            var customSkin = DirectorStyles.Instance.customSkin;
            var blendInColor = drawData.selected ? customSkin.clipBlendInSelected : customSkin.clipBlendIn;
            var blendOutColor = drawData.selected ? customSkin.clipBlendOutSelected : customSkin.clipBlendOut;
            var easeBackgroundColor = customSkin.clipEaseBckgColor;

            DrawClipBlends(drawData.clipBlends, blendInColor, blendOutColor, easeBackgroundColor);
            DrawClipBackground(drawData.clipCenterSection, drawData.selected);

            if (drawData.targetRect.width > k_MinClipWidth)
            {
                DrawClipEditorBackground(drawData);
            }
            else
            {
                drawData.targetRect.width = k_MinClipWidth;
                drawData.clipCenterSection.width = k_MinClipWidth;
            }

            DrawClipTimescale(drawData.targetRect, drawData.clippedRect, drawData.clip.timeScale);

            if (drawData.targetRect.width >= k_ClipInOutMinWidth)
                DrawClipInOut(drawData.targetRect, drawData.clip);

            var labelRect = drawData.clipCenterSection;

            if (drawData.targetRect.width >= k_ClipLoopsMinWidth)
            {
                bool selected = drawData.selected || drawData.inlineCurvesSelected;

                if (selected)
                {
                    if (drawData.loopRects != null && drawData.loopRects.Any())
                    {
                        DrawLoops(drawData);

                        var l = drawData.loopRects[0];
                        labelRect.xMax = Math.Min(labelRect.xMax, l.x - drawData.unclippedRect.x);
                    }
                }
            }

            labelRect.xMin += k_ClipLabelPadding;
            labelRect.xMax -= k_ClipLabelPadding;

            if (labelRect.width > k_ClipLabelMinWidth)
            {
                DrawClipLabel(drawData, labelRect, Color.white);
            }

            DrawClipSwatch(drawData.targetRect, drawData.ClipDrawOptions.highlightColor);
            DrawClipBorder(drawData);
        }

        static void DrawClipEditorBackground(ClipDrawData drawData)
        {
            var isRepaint = (Event.current.type == EventType.Repaint);
            if (isRepaint && drawData.clipEditor != null)
            {
                var customBodyRect = drawData.clippedRect;
                customBodyRect.yMin += k_ClipInlineWidth;
                customBodyRect.yMax -= k_ClipSwatchLineThickness;
                var region = new ClipBackgroundRegion(customBodyRect, drawData.localVisibleStartTime, drawData.localVisibleEndTime);
                try
                {
                    drawData.clipEditor.DrawBackground(drawData.clip, region);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        public static void DrawAnimationRecordBorder(ClipDrawData drawData)
        {
            if (!drawData.clip.parentTrack.IsRecordingToClip(drawData.clip))
                return;

            var time = new DiscreteTime(TimelineWindow.instance.state.editSequence.time);
            var start = new DiscreteTime(drawData.clip.start + drawData.clip.mixInDuration);
            var end = new DiscreteTime(drawData.clip.end - drawData.clip.mixOutDuration);

            if (time < start || time >= end)
                return;

            DrawClipSelectionBorder(drawData.clipCenterSection, ClipBorder.Recording(), ClipBlends.kNone);
        }

        public static void DrawRecordProhibited(ClipDrawData drawData)
        {
            DrawRecordInvalidClip(drawData);
            DrawRecordOnBlend(drawData);
        }

        public static void DrawRecordOnBlend(ClipDrawData drawData)
        {
            double time = TimelineWindow.instance.state.editSequence.time;
            if (time >= drawData.clip.start && time < drawData.clip.start + drawData.clip.mixInDuration)
            {
                Rect r = Rect.MinMaxRect(drawData.clippedRect.xMin, drawData.clippedRect.yMin, drawData.clipCenterSection.xMin, drawData.clippedRect.yMax);
                DrawInvalidRecordIcon(r, Styles.s_ClipNoRecordInBlend);
            }

            if (time <= drawData.clip.end && time > drawData.clip.end - drawData.clip.mixOutDuration)
            {
                Rect r = Rect.MinMaxRect(drawData.clipCenterSection.xMax, drawData.clippedRect.yMin, drawData.clippedRect.xMax, drawData.clippedRect.yMax);
                DrawInvalidRecordIcon(r, Styles.s_ClipNoRecordInBlend);
            }
        }

        public static void DrawRecordInvalidClip(ClipDrawData drawData)
        {
            if (drawData.clip.recordable)
                return;

            double time = TimelineWindow.instance.state.editSequence.time;
            if (time < drawData.clip.start + drawData.clip.mixInDuration || time > drawData.clip.end - drawData.clip.mixOutDuration)
                return;

            DrawInvalidRecordIcon(drawData.clipCenterSection, Styles.s_ClipNotRecorable);
        }

        public static void DrawInvalidRecordIcon(Rect rect, GUIContent helpText)
        {
            EditorGUI.DrawRect(rect, new Color(0, 0, 0, 0.30f));

            var icon = Styles.s_IconNoRecord;
            if (rect.width < icon.width || rect.height < icon.height)
                return;

            float x = rect.x + (rect.width - icon.width) * 0.5f;
            float y = rect.y + (rect.height - icon.height) * 0.5f;
            Rect r = new Rect(x, y, icon.width, icon.height);
            GUI.Label(r, helpText);
            GUI.DrawTexture(r, icon, ScaleMode.ScaleAndCrop, true, 0, Color.white, 0, 0);
        }
    }
}
