using UnityEngine.UI;

namespace Assets.Game.Scripts.UI
{
    public class ButtonWithSound : Button
    {
        protected override void Start()
        {
            base.Start();
            this.onClick.AddListener(() => UIManager.Instance.ButtonSound());
        }
    }
}