using System;
using System.Collections.Generic;
using GameNet.KriptoFX.Realistic_Effects_Pack_3.Effects.Common.Editor;
using UnityEditor;

namespace GameNet.KriptoFX.Realistic_Effects_Pack_3.Effects.Bloom.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Bloom))]
    public class BloomEditor : UnityEditor.Editor
    {
        [NonSerialized]
        private List<SerializedProperty> m_Properties = new List<SerializedProperty>();

        BloomGraphDrawer _graph;

        bool CheckHdr(Bloom target)
        {
            var camera = target.GetComponent<UnityEngine.Camera>();
            return camera != null && camera.allowHDR;
        }

        void OnEnable()
        {
            var settings = FieldFinder<Bloom>.GetField(x => x.settings);
            foreach (var setting in settings.FieldType.GetFields())
            {
                var prop = settings.Name + "." + setting.Name;
                m_Properties.Add(serializedObject.FindProperty(prop));
            }

            _graph = new BloomGraphDrawer();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (!serializedObject.isEditingMultipleObjects)
            {
                EditorGUILayout.Space();
                var bloom = (Bloom)target;
                _graph.Prepare(bloom.settings, CheckHdr(bloom));
                _graph.DrawGraph();
                EditorGUILayout.Space();
            }

            foreach (var property in m_Properties)
                EditorGUILayout.PropertyField(property);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
