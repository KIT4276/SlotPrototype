using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;
using UnityEngine.UI;

namespace SlotPrototype.UI
{
    [RequireComponent(typeof(Button))]
    public class UIButtonDataBind : MonoBehaviourExtBind
    {
        [Header("Model")]
        [SerializeField] private bool _globalModel = true;

        [Tooltip("Ключ в модели, например: UI_CanStart или UI_CanStop")]
        [SerializeField] private string _fieldName = "UI_CanStart";

        [Header("Click")]
        [Tooltip("Событие, которое отправляем в FSM, например: UI.StartClick")]
        [SerializeField] private string _clickEventName = "UI.StartClick";

        [Space]
        [SerializeField] private Button _button;

        [OnAwake]
        private void Init()
        {
            //_button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        [OnStart]
        private void ApplyInitial()
        {
            //_button.interactable = Settings.Model.GetBool(_fieldName, false);
        }

        private void OnClick()
        {
            if (_button == null || !_button.interactable) return;
            Settings.Invoke(_clickEventName);
        }

        // слушаем событие вида On{FieldName}Changed, где FieldName берётся из инспектора.
        [Bind("On{_fieldName}Changed")]
        private void OnInteractableChanged(bool value)
        {
            if (_button != null) _button.interactable = value;
        }

        [OnDestroy]
        private void Cleanup()
        {
            if (_button != null)
                _button.onClick.RemoveListener(OnClick);
        }
    }
}

