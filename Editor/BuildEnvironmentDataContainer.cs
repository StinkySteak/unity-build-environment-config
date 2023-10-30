using UnityEngine;

namespace StinkySteak.BuildEnvironmentConfig.Editor
{
    [CreateAssetMenu(fileName = nameof(BuildEnvironmentDataContainer), menuName = "Data/Platform Environment/Container")]
    public class BuildEnvironmentDataContainer : ScriptableObject
    {
        [SerializeField] private BaseBuildEnvironment[] _environments;

        public const string MENU_ITEM_PATH = "Tools/Set Release Target";

        public BaseBuildEnvironment GetEnvironmentData(int data)
        {
            return _environments[data];
        }

        public BaseBuildEnvironment[] Datas => _environments;

#if UNITY_EDITOR
        private void OpenMenu()
        {
            UnityEditor.EditorApplication.ExecuteMenuItem(MENU_ITEM_PATH);
        }
#endif
    }
}
