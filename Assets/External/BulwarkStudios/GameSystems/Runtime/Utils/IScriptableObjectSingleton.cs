using UnityEngine;

namespace BulwarkStudios.GameSystems.Utils {

    public interface IScriptableObjectSingleton {

        /// <summary>
        /// Set the instance
        /// </summary>
        /// <param name="so"></param>
        void SetInstance(ScriptableObject so);

    }

}