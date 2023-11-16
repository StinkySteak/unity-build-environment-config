using UnityEngine;

namespace StinkySteak.BuildEnvironmentConfig
{
    /// <summary>
    /// Data Container for storing multiple <see cref="BaseBuildEnvironment"/> to EditorWindow to scan
    /// </summary>
    public class BaseBuildEnvironmentDataContainer : ScriptableObject
    {
        [SerializeField] protected BaseBuildEnvironment[] _environments;

        public const string MENU_ITEM_PATH = "Tools/Set Release Target";

        public BaseBuildEnvironment GetEnvironmentData(int data)
        {
            return _environments[data];
        }

        public BaseBuildEnvironment[] Datas => _environments;

        /// <summary>
        /// Override this and add a your custom EditorButton to open up <see cref="MENU_ITEM_PATH"/> window quickly
        /// </summary>
        protected virtual void OpenMenu()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExecuteMenuItem(MENU_ITEM_PATH);
#endif
        }

    }
}
