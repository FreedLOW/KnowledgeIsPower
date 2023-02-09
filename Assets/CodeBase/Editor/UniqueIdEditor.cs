using System;
using System.Linq;
using CodeBase.Logic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(UniqueId))]
    public class UniqueIdEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            var uniqueId = (UniqueId) target;

            if (IsPrefab(uniqueId)) return;
            
            if (string.IsNullOrEmpty(uniqueId.uniqueId))
            {
                Generate(uniqueId);
            }
            else
            {
                UniqueId[] uniqueIds = FindObjectsOfType<UniqueId>();
                if (uniqueIds.Any(other => other != uniqueId && other.uniqueId == uniqueId.uniqueId))
                    Generate(uniqueId);
            }
        }

        private bool IsPrefab(UniqueId uniqueId) => 
            uniqueId.gameObject.scene.rootCount == 0;

        private void Generate(UniqueId uniqueId)
        {
            uniqueId.uniqueId = $"{uniqueId.gameObject.scene.name}_{Guid.NewGuid().ToString()}";

            EditorUtility.SetDirty(uniqueId);
            if (Application.isPlaying) return;
            EditorSceneManager.MarkSceneDirty(uniqueId.gameObject.scene);
        }
    }
}