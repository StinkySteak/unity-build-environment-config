using UnityEngine;

namespace StinkySteak.BuildEnvironmentConfig
{
    /// <summary>
    /// Environment target metadata
    /// </summary>
    public class BaseBuildEnvironment : ScriptableObject
    {
        [SerializeField] private string _title = "DEVELOPMENT";
        public string Title => _title;

        /// <summary>
        /// Override this and add a your custom EditorButton to open up <see cref="BaseBuildEnvironmentDataContainer.MENU_ITEM_PATH"/> window quickly
        /// </summary>
        protected virtual void OpenMenu()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExecuteMenuItem(BaseBuildEnvironmentDataContainer.MENU_ITEM_PATH);
#endif
        }
    }
}
