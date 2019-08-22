using System.Reflection;
using Sirenix.OdinInspector;

namespace BulwarkStudios.GameSystems.Ui {

    [System.Serializable, HideLabel]
    public abstract class UiButtonEffectData {

        /// <summary>
        /// Ref to the button effect
        /// </summary>
        private object buttonEffect;

        /// <summary>
        /// Set the button effect
        /// </summary>
        /// <param name="buttonEffect"></param>
        /// <returns></returns>
        public UiButtonEffectData SetButtonEffect(object buttonEffect) {
            this.buttonEffect = buttonEffect;
            return this;
        }

        /// <summary>
        /// Check a condition to display a field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        protected bool CheckCondition(string fieldName) {

#if UNITY_EDITOR
            if (buttonEffect == null) {
                return true;
            }

            FieldInfo field = buttonEffect.GetType().GetField(fieldName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly);

            if (field == null) {
                return true;
            }

            return (bool) field.GetValue(buttonEffect);
#else
            return true;
#endif

        }

    }

}