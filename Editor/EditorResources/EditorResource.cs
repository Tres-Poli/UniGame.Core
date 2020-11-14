﻿namespace UniModules.UniGame.Core.EditorTools.Editor.EditorResources
{
    using System;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Object = UnityEngine.Object;

    [Serializable]
    public class EditorResource : ResourceHandle
    {
        public string AssetPath => assetPath;

        public bool IsInstance { get; protected set; }

        public bool IsSceneAsset { get; protected set; }

        public Scene  Scene  { get; protected set; }
        
        public Object Target => asset;

        public PrefabInstanceStatus InstanceStatus { get; protected set; } = PrefabInstanceStatus.NotAPrefab;

        public PrefabAssetType PrefabAssetType { get; protected set; } = PrefabAssetType.NotAPrefab;

        public bool IsVariant { get; protected set; }

        protected override ResourceHandle OnUpdateAsset(Object targetAsset)
        {
            var resultAsset = targetAsset;
            var resultPath  = string.Empty;

            if (targetAsset is Component componentAsset)
                resultAsset = componentAsset.gameObject;
            if (resultAsset is GameObject targetGameObject)
            {
                InstanceStatus  = PrefabUtility.GetPrefabInstanceStatus(targetGameObject);
                PrefabAssetType = PrefabUtility.GetPrefabAssetType(targetGameObject);

                resultAsset = PrefabUtility.GetOutermostPrefabInstanceRoot(targetGameObject);

                if (resultAsset != null)
                {
                    resultPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(resultAsset);
                }

                IsVariant    = PrefabUtility.IsPartOfVariantPrefab(targetGameObject);
                Scene        = targetGameObject.scene;
                IsSceneAsset = !string.IsNullOrEmpty(targetGameObject.scene.name);
            }

            IsInstance = resultAsset != null;

            if (resultAsset == null)
            {
                resultAsset = targetAsset;
                resultPath  = AssetDatabase.GetAssetPath(targetAsset);
            }

            assetPath = string.IsNullOrEmpty(resultPath) ? AssetDatabase.GetAssetPath(resultAsset) : resultPath;
            guid      = string.IsNullOrEmpty(assetPath) ? string.Empty : AssetDatabase.AssetPathToGUID(assetPath);
            asset     = resultAsset;

            return this;
        }

        protected override TResult LoadAsset<TResult>()
        {
            if (string.IsNullOrEmpty(AssetPath))
                return null;

            var loadedAsset = AssetDatabase.LoadAssetAtPath<Object>(AssetPath);

            return GetTargetFromSource<TResult>(loadedAsset);
        }
    }
}