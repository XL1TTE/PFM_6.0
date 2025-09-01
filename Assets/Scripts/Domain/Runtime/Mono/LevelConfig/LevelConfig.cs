using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace Domain.Levels.Mono
{
    public sealed class LevelConfig : MonoBehaviour
    {
        void Awake()
        {
            if(_instance == null){
                _instance = this;
            }
        }
        private static LevelConfig _instance;

        [SerializeField] private GameObject _startLevel;
        
        public static string StartLevelID {
            get {
                if(_instance != null){
                    return _instance._startLevel.name;
                }
                return null;
            }
        }
    }
}
