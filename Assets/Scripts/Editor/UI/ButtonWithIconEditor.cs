using UI;
using UnityEditor;
using UnityEditor.UI;

namespace Editor.UI
{
    [CustomEditor(typeof(ButtonWithIcon), true)]
    [CanEditMultipleObjects]
    public class ButtonWithIconEditor : ButtonEditor
    {
        private SerializedProperty iconProperty;
        private SerializedProperty iconColorProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            iconProperty = serializedObject.FindProperty("icon");
            iconColorProperty = serializedObject.FindProperty("iconColorBlock");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(iconProperty);
            if (iconProperty.objectReferenceValue != null)
                EditorGUILayout.PropertyField(iconColorProperty);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }
    }
}