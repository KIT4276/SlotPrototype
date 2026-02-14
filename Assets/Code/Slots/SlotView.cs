using UnityEngine;
using UnityEngine.UI;

namespace SlotPrototype.UI
{
    public class SlotView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;

        public RectTransform RectTransform { get => _rectTransform; }

        public void SetImage(Sprite sprite) => 
            _image.sprite = sprite;


#if UNITY_EDITOR
        [ContextMenu("Assign _image & _rectTransform")]
        private void AssignFromChildren()
        {
            _image = transform.GetChild(0).GetComponent<Image>();
            _rectTransform = GetComponent<RectTransform>();
        }
#endif
    }
}

