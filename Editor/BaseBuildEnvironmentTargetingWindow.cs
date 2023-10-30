using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

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
        private int _environmentIndex;

        public virtual string[] SYMBOLS => new string[] { };

        protected virtual string ENVIRONMENT_DATA_CONTAINER_PATH => string.Empty;
        public const string MENU_ITEM_PATH = "Tools/Set Release Target";


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

            _environmentIndex = EditorGUILayout.IntPopup("Environtment", _environmentIndex, nameList.ToArray(), intList.ToArray());
            _platformTarget = (BuildTargetGroup)EditorGUILayout.EnumPopup("Platform Target", _platformTarget);

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

        private BuildEnvironmentDataContainer GetPlatformEnvirontmentDataContainer()
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
                object obj = AssetDatabase.LoadAssetAtPath(path, typeof(BuildEnvironmentDataContainer));

                if (obj is BuildEnvironmentDataContainer container)
                {
                    return container;
                }
            }

            Debug.LogError($"[GameBuildTargeting]: No Platform Environment Data Container found in {ENVIRONMENT_DATA_CONTAINER_PATH}");
            return null;
        }

        protected BaseBuildEnvironment GetPlatformEnvironment(int environmentIndex)
        {
            BuildEnvironmentDataContainer container = GetPlatformEnvirontmentDataContainer();

            return container.GetEnvironmentData(environmentIndex);
        }

        protected virtual void UpdateList(BaseBuildEnvironment activeEnvironment, List<string> symbolList)
        {

        }

        private void UpdateScriptingSymbols()
        {
            BaseBuildEnvironment environment = GetPlatformEnvironment(_environmentIndex);

            PlayerSettings.GetScriptingDefineSymbolsForGroup(_platformTarget, out string[] symbols);

            List<string> symbolList = symbols.ToList();

            foreach (string symbol in SYMBOLS)
                TryRemove(symbolList, symbol);

            UpdateList(environment, symbolList);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(_platformTarget, symbolList.ToArray());
            Unity.CodeEditor.CodeEditor.Editor.CurrentCodeEditor.SyncAll();

            Debug.Log($"[GameBuildTargeting]: Release Target has been set to: {environment.Title} for platform: {_platformTarget}");
        }

        private void TryRemove(List<string> symbolList, string defineSymbol)
        {
            if (symbolList.Contains(defineSymbol))
                symbolList.Remove(defineSymbol);
        }
    }
}