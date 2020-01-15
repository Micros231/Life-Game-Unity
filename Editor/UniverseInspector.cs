using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LifeGame
{
    [CustomEditor(typeof(Universe))]
    public class UniverseInspector : Editor
    {
        private Universe UniverseTarget => target as Universe;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (EditorApplication.isPlaying)
            {
                if (!serializedObject.FindProperty("_isRunGeneration").boolValue)
                {
                    if (GUILayout.Button("Start Generation"))
                    {
                        UniverseTarget.StartGeneration();
                    }
                }
                else
                {
                    if (GUILayout.Button("Stop Generation"))
                    {
                        UniverseTarget.StopGeneration();
                    }

                }
            }
            
        }
    }
}

