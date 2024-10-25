using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.Build;

namespace StinkySteak.BuildEnvironmentConfig.Editor
{
    /// <summary>
    /// Editor Helper for Settings: <br/>
    /// 1. Script Define Symbols <br/>
    /// 2. Environment build
    /// </summary>
    public class BaseBuildEnvironmentConfigWindow : EditorWindow
    {
        private BuildTargetGroup _platformTarget;
        private bool _isServer;
        private int _environmentIndex;

        public virtual string[] SYMBOLS => new string[] { };

        protected virtual string ENVIRONMENT_DATA_CONTAINER_PATH => string.Empty;
        public const string MENU_ITEM_PATH = BaseBuildEnvironmentDataContainer.MENU_ITEM_PATH;


        private void OnGUI()
        {
            var container = GetPlatformEnvirontmentDataContainer();

            List<string> nameList = new();
            List<int> intList = new();

            int i = 0;

            foreach (var data in container.Datas)
            {
                intList.Add(i++);
                nameList.Add(data.Title);
            }

            _environmentIndex = EditorGUILayout.IntPopup("Environment", _environmentIndex, nameList.ToArray(), intList.ToArray());
            _platformTarget = (BuildTargetGroup)EditorGUILayout.EnumPopup("Platform Target", _platformTarget);

            if (_platformTarget == BuildTargetGroup.Standalone)
                _isServer = EditorGUILayout.Toggle("Is Server?", _isServer);

            if (GUILayout.Button("Set Release Target"))
            {
                if (!IsPlatformValid()) return;

                UpdateScriptingSymbols();
            }
        }

        private bool IsPlatformValid()
        {
            if (_platformTarget == BuildTargetGroup.Unknown)
            {
                Debug.LogError($"[GameBuildTargeting]: Unknown Platform");
                return false;
            }

            return true;
        }

        private BaseBuildEnvironmentDataContainer GetPlatformEnvirontmentDataContainer()
        {
            if (string.IsNullOrEmpty(ENVIRONMENT_DATA_CONTAINER_PATH))
            {
                Debug.LogError($"[GameBuildTargeting]: {nameof(ENVIRONMENT_DATA_CONTAINER_PATH)} is null!");
                return null;
            }

            string[] paths = new string[] { ENVIRONMENT_DATA_CONTAINER_PATH };

            string[] guids = AssetDatabase.FindAssets("", paths);

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                object obj = AssetDatabase.LoadAssetAtPath(path, typeof(BaseBuildEnvironmentDataContainer));

                if (obj is BaseBuildEnvironmentDataContainer container)
                {
                    return container;
                }
            }

            Debug.LogError($"[GameBuildTargeting]: No Platform Environment Data Container found in {ENVIRONMENT_DATA_CONTAINER_PATH}");
            return null;
        }

        protected BaseBuildEnvironment GetPlatformEnvironment(int environmentIndex)
        {
            BaseBuildEnvironmentDataContainer container = GetPlatformEnvirontmentDataContainer();

            return container.GetEnvironmentData(environmentIndex);
        }

        protected virtual void UpdateList(BaseBuildEnvironment activeEnvironment, List<string> symbolList)
        {

        }

        private void UpdateScriptingSymbols()
        {
            BaseBuildEnvironment environment = GetPlatformEnvironment(_environmentIndex);

            NamedBuildTarget buildTarget = NamedBuildTarget.FromBuildTargetGroup(_platformTarget);
            PlayerSettings.GetScriptingDefineSymbols(buildTarget, out string[] symbols);

            bool isStandalone = _platformTarget == BuildTargetGroup.Standalone;

            if (isStandalone && _isServer)
            {
                buildTarget = NamedBuildTarget.Server;
            }

            EditorUserBuildSettings.standaloneBuildSubtarget = StandaloneBuildSubtarget.Player;

            List<string> symbolList = symbols.ToList();

            foreach (string symbol in SYMBOLS)
                TryRemove(symbolList, symbol);

            UpdateList(environment, symbolList);

            PlayerSettings.SetScriptingDefineSymbols(buildTarget, symbolList.ToArray());
            Unity.CodeEditor.CodeEditor.Editor.CurrentCodeEditor.SyncAll();

            if (!_isServer)
            {
                Debug.Log($"[GameBuildTargeting]: Release Target has been set to: {environment.Title} for platform: {_platformTarget}");
                return;
            }

            Debug.Log($"[GameBuildTargeting]: Release Target has been set to: {environment.Title} for platform: {_platformTarget} with server subtarget");
        }

        private void TryRemove(List<string> symbolList, string defineSymbol)
        {
            if (symbolList.Contains(defineSymbol))
                symbolList.Remove(defineSymbol);
        }
    }
}
