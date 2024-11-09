using System.Linq;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string InitialPointTag = "InitialPoint";
        private const string LevelTransfer = "LevelTransfer";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelStaticData levelData = (LevelStaticData)target;

            if (GUILayout.Button("Collect"))
            {
                levelData.EnemySpawners = FindObjectsOfType<SpawnMarker>()
                    .Select(t => new EnemySpawnerData(t.GetComponent<UniqueId>().Id, t.MonsterTypeId, t.transform.position))
                    .ToList();

                levelData.LevelKey = SceneManager.GetActiveScene().name;
                levelData.InitialPoint = GameObject.FindWithTag(InitialPointTag).transform.position;
                levelData.LevelTransferPoint = GameObject.FindWithTag(LevelTransfer).transform.position;
            }
            
            EditorUtility.SetDirty(target);
        }
    }
}