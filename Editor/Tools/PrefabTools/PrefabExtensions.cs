﻿namespace UniModules.Editor
{
    using System.Collections.Generic;
    using UnityEngine;
    
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public static class PrefabExtensions
    {
        public static void CreatePrefabs(this IEnumerable<GameObject> source, string path)
        {
#if UNITY_EDITOR
            
            foreach (var gameObject in source)
            {
                if (!gameObject)
                {
                    Debug.LogError($"NULL Object Asset with path {path}");
                }

                gameObject.SaveAsset(gameObject.name, path, false);
            }
            
            AssetDatabase.Refresh();
#endif
        }

        public static bool TryApplyPrefab(this GameObject source, string path, out GameObject prefab)
        {
#if UNITY_EDITOR
            if (source && PrefabUtility.IsPartOfAnyPrefab(source) && AssetDatabaseHelper.TryGetAsset<GameObject>(path, out var result)) {
                PrefabUtility.ApplyPrefabInstance(source, InteractionMode.AutomatedAction);
                prefab = result;
                return true;
            }
#endif
            prefab = null;
            return false;
        }

        public static bool TryApplyPrefab(this GameObject source)
        {
            if (!source)
                return false;
#if UNITY_EDITOR
            if (PrefabUtility.IsPartOfAnyPrefab(source)) {
                PrefabUtility.ApplyPrefabInstance(source, InteractionMode.AutomatedAction);
                return true;
            }
#endif
            return false;
        }

        public static bool TryApplyPrefab(this Component source)
        {
            return source.gameObject.TryApplyPrefab();
        }
    }
}