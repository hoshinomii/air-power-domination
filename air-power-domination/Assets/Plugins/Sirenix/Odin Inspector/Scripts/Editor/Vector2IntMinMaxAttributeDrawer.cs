﻿//-----------------------------------------------------------------------
// <copyright file="Vector2IntMinMaxAttributeDrawer.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR && UNITY_2017_2_OR_NEWER

namespace Plugins.Sirenix.Odin_Inspector.Scripts.Editor
{
    /// <summary>
    /// Draws Vector2Int properties marked with <see cref="MinMaxSliderAttribute"/>.
    /// </summary>
    public class Vector2IntMinMaxAttributeDrawer : OdinAttributeDrawer<MinMaxSliderAttribute, Vector2Int>
    {
        private ValueResolver<float> minGetter;
        private ValueResolver<float> maxGetter;
        private ValueResolver<Vector2Int> vector2IntMinMaxGetter;

        /// <summary>
        /// Initializes the drawer by resolving any optional references to members for min/max value.
        /// </summary>
        protected override void Initialize()
        {
            // Min member reference.
            this.minGetter = ValueResolver.Get<float>(this.Property, this.Attribute.MinValueGetter, this.Attribute.MinValue);
            this.maxGetter = ValueResolver.Get<float>(this.Property, this.Attribute.MaxValueGetter, this.Attribute.MaxValue);

            // Min max member reference.
            if (this.Attribute.MinMaxValueGetter != null)
            {
                this.vector2IntMinMaxGetter = ValueResolver.Get<Vector2Int>(this.Property, this.Attribute.MinMaxValueGetter);
            }
        }

        /// <summary>
        /// Draws the property.
        /// </summary>
        protected override void DrawPropertyLayout(GUIContent label)
        {
            ValueResolver.DrawErrors(this.minGetter, this.maxGetter, this.vector2IntMinMaxGetter);

            // Get the range of the slider from the attribute or from member references.
            Vector2 range;
            if (this.vector2IntMinMaxGetter != null && !this.vector2IntMinMaxGetter.HasError)
            {
                range = (Vector2)this.vector2IntMinMaxGetter.GetValue();
            }
            else
            {
                range.x = this.minGetter.GetValue();
                range.y = this.maxGetter.GetValue();
            }

            EditorGUI.BeginChangeCheck();
            Vector2 value = SirenixEditorFields.MinMaxSlider(label, (Vector2)this.ValueEntry.SmartValue, range, this.Attribute.ShowFields);
            if (EditorGUI.EndChangeCheck())
            {
                this.ValueEntry.SmartValue = new Vector2Int((int)value.x, (int)value.y);
            }
        }
    }
}
#endif // UNITY_EDITOR && UNITY_2017_2_OR_NEWER