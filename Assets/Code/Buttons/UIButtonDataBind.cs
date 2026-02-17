using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SlotPrototype.UI
{
    [RequireComponent(typeof(Button))]
    public sealed class UIButtonDataBind : MonoBehaviourExtBind
    {
        [Header("Model")]
        [SerializeField] private bool _globalModel = false;

        [SerializeField] private string _fieldName = "UI_CanStart";

        [Header("Click")]
        [SerializeField] private string _clickEventName = "UI.StartClick";

        [SerializeField] private Button _button;

        private string _eventName;
        private DynamicModel _model;

        [OnAwake]
        private void Init()
        {
            if (_button == null) _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);

            _model = _globalModel ? Settings.GlobalModel : Settings.Model;
            _eventName = "On" + _fieldName + "Changed";
        }

        [OnEnable]
        private void ApplyInitialAndBind()
        {
            SetInteractable(_model.Get<bool>(_fieldName));
            _model.EventManager.AddAction(_eventName, OnChanged);
        }

        private void OnChanged(params object[] args)
        {
            var value = args.Length > 0 ? args[0] : null;
            SetInteractable(Convert.ToBoolean(value));
        }

        private void OnClick()
        {
            if (_button == null || !_button.interactable) return;
            Settings.Invoke(_clickEventName);
        }

        private void SetInteractable(bool value)
        {
            if (_button != null)
                _button.interactable = value;
        }

        [OnDisable]
        private void AxDisable()
        {
            if (_model != null && !string.IsNullOrEmpty(_eventName))
                _model.EventManager.RemoveAction(_eventName, (DEventMethod)OnChanged);
        }

        [OnDestroy]
        private void AxDestroy()
        {
            if (_button != null)
                _button.onClick.RemoveListener(OnClick);
        }
    }
}