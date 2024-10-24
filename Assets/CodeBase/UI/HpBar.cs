using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class HpBar : MonoBehaviour
    {
        public Image HpImage;

        public void SetValue(float current, float max) => 
            HpImage.fillAmount = current / max;
    }
}