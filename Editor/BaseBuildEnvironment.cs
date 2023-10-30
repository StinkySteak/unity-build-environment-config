using UnityEngine;

namespace StinkySteak.BuildEnvironmentConfig.Editor
{
    /// <summary>
    /// Environment target metadata
    /// </summary>
    public class BaseBuildEnvironment : ScriptableObject
    {
        [SerializeField] private string _title = "DEVELOPMENT";
        public string Title => _title;
    }
}