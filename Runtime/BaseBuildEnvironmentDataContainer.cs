using UnityEngine;

namespace StinkySteak.BuildEnvironmentConfig.Editor
{
    /// <summary>
    /// Data Container for storing multiple <see cref="BaseBuildEnvironment"/> to EditorWindow to scan
    /// </summary>
    public class BaseBuildEnvironmentDataContainer : ScriptableObject
    {
        [SerializeField] private BaseBuildEnvironment[] _environments;

        public const string MENU_ITEM_PATH = "Tools/Set Release Target";

        public BaseBuildEnvironment GetEnvironmentData(int data)
        {
            return _environments[data];
        }

        public BaseBuildEnvironment[] Datas => _environments;

#if UNITY_EDITOR
        protected void OpenMenu()
        {
            UnityEditor.EditorApplication.ExecuteMenuItem(MENU_ITEM_PATH);
        }
#endif
    }
}
