using UnityEngine;
using UnityEditor;
using Utils;
using Utils.Debugger;

namespace Editors
{
    [CustomEditor(typeof(BridgeContainer))]
    public class BridgeContainerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Bridge Constructor", EditorStyles.centeredGreyMiniLabel);

            BridgeContainer bridgeContainer = (BridgeContainer)target;

            bridgeContainer.IsBreakable = GUILayout.Toggle(bridgeContainer.IsBreakable, "Is Breakable");
            if (bridgeContainer.IsBreakable)
            {
                bridgeContainer.BrakeForce = EditorGUILayout.FloatField("Brake Force", bridgeContainer.BrakeForce);
                bridgeContainer.BrakeTorque = EditorGUILayout.FloatField("Brake Torque", bridgeContainer.BrakeTorque);
            }
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add plank")) bridgeContainer.AddPlank();
            GUI.enabled = bridgeContainer.Count > 2;
            if (GUILayout.Button("Remove plank")) bridgeContainer.RemovePlank();
            GUI.enabled = true;
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Finish building")) bridgeContainer.Finish();
            EditorGUILayout.HelpBox("Do not start Play mode until the end of building", MessageType.Warning);
        }
    }
}