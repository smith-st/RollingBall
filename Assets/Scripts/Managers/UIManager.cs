using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textPoints;
        [SerializeField] private TextMeshProUGUI textFalls;
        [SerializeField] private CanvasGroup panelTapToPlay;

        public void ShowPoints(int value)
        {
            textPoints.text = $"Points: {value}";
        }

        public void ShowFalls(int value)
        {
            textFalls.text = $"Falls: {value}";
        }

        public void ShowTapToPlay(bool show = true)
        {
            panelTapToPlay.DOFade(show ? 1f : 0f, 0.3f);
        }

        private void Awake()
        {
            panelTapToPlay.alpha = 1f;
            ShowPoints(0);
            ShowFalls(0);
        }
    }
}
