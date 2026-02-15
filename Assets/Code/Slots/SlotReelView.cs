using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlotPrototype.UI
{
    public class SlotReelView : MonoBehaviourExtBind
    {
        [Header("Refs")]
        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _window;
        [SerializeField] private VerticalLayoutGroup _contentLayoutGroup;
        [SerializeField] private List<SlotView> _itemViews = new();

        [Header("Motion")]
        [SerializeField] private float _startSpeed = 50f;      
        [SerializeField] private float _accel = 50f;

        [Header("Stop")]
        [Tooltip("Время торможения до 0 (сек). После этого запускается snap.")]
        [SerializeField] private float _stopTime = 0.6f;
        [SerializeField] private float _stopEpsilon = 5f;    

        [Header("Symbols")]
        [SerializeField] private List<Sprite> _sprites = new();

        private const float SnapRemainingDown = 0.01f;

        private bool _isScrolling;
        private bool _isStopping;
        private bool _isSnapping;
        private float _snapRemainingDown;

        private float _speed;

        private float _stopTimer;
        private float _stopStartSpeed;

        private float Step => Mathf.Max(1f, GetCellHeight() + _contentLayoutGroup.spacing);

        [Bind("Reel.Start")]
        private void StartScroll()
        {
            _isStopping = false;
            _isSnapping = false;
            _snapRemainingDown = 0f;

            _stopTimer = 0f;
            _stopStartSpeed = 0f;

            _speed = _startSpeed;
            _isScrolling = true;
        }

        [Bind("Reel.Stop")]
        private void StopScroll()
        {
            _isStopping = true;
            _isSnapping = false;
            _snapRemainingDown = 0f;

            _stopTimer = 0f;
            _stopStartSpeed = _speed; // фиксируем текущую скорость в момент команды Stop
        }

        [OnStart]
        private void ValidateSetup()
        {
            if (_content == null)
                Debug.LogWarning($"{nameof(SlotReelView)}: _content is null. Assign ReelContent.");

            if (_itemViews == null || _itemViews.Count == 0)
                Debug.LogWarning($"{nameof(SlotReelView)}: _itemViews is empty. Assign Slot items.");
        }

        [OnUpdate]
        private void Tick()
        {
            if (!_isScrolling || _window == null || _content == null || _itemViews == null || _itemViews.Count == 0)
                return;

            float dt = Time.deltaTime;

            if (!_isStopping)
            {
                _speed += _accel * dt;
                MoveDown(_speed * dt);
                return;
            }

            if (!_isSnapping)
            {
                // Торможение по времени
                float stopTime = Mathf.Max(0.0001f, _stopTime);
                _stopTimer += dt;

                float t = Mathf.Clamp01(_stopTimer / stopTime);
                _speed = Mathf.Lerp(_stopStartSpeed, 0f, t);

                if (_speed > _stopEpsilon)
                {
                    MoveDown(_speed * dt);
                }
                else
                {
                    BeginSnapDown();
                }
            }
            else
            {
                float stepMove = Mathf.Min(_speed * dt, _snapRemainingDown);
                MoveDown(stepMove);
                _snapRemainingDown -= stepMove;

                if (_snapRemainingDown <= 0.01f)
                {
                    _speed = 0f;
                    _isSnapping = false;
                    _isStopping = false;
                    _isScrolling = false;

                    Settings.Invoke("Reel.Stopped");
                }
            }
        }

        private void MoveDown(float pixels)
        {
            _content.anchoredPosition += Vector2.down * pixels;
            RecycleBySiblingIfNeeded();
        }

        private void BeginSnapDown()
        {
            _isSnapping = true;

            _speed = Mathf.Max(_speed, _stopEpsilon);

            float closestAbsY = float.PositiveInfinity;
            float closestY = 0f;

            for (int i = 0; i < _itemViews.Count; i++)
            {
                float y = GetItemYInWindow(_itemViews[i].RectTransform);
                float ay = Mathf.Abs(y);
                if (ay < closestAbsY)
                {
                    closestAbsY = ay;
                    closestY = y;
                }
            }

            float delta = -closestY;
            if (delta > 0f)
                delta -= Step; 

            _snapRemainingDown = Mathf.Max(0f, -delta);

            if (_snapRemainingDown <= SnapRemainingDown)
            {
                _speed = 0f;
                _isSnapping = false;
                _isStopping = false;
                _isScrolling = false;

                Settings.Invoke("Reel.Stopped");
            }
        }

        private float GetCellHeight()
        {
            if (_itemViews == null || _itemViews.Count == 0) return 100f;
            return Mathf.Max(1f, _itemViews[0].RectTransform.rect.height);
        }

        private void RecycleBySiblingIfNeeded()
        {
            float step = Step;
            float recycleMargin = step * 0.5f;

            float windowHalfH = _window.rect.height * 0.5f;
            float bottomY = -windowHalfH - recycleMargin;

            // максимум сколько элементов можно переставить за кадр
            int guard = _itemViews.Count + 2;

            while (guard-- > 0)
            {
                var last = _itemViews[_itemViews.Count - 1];
                float y = GetItemYInWindow(last.RectTransform);

                // если последний ещё видим (не ниже границы) — выходим
                if (y >= bottomY)
                    break;

                // переносим нижний наверх
                last.RectTransform.SetAsFirstSibling();
                SetRandomSprite(last);

                // синхронизируем список с новым порядком
                _itemViews.RemoveAt(_itemViews.Count - 1);
                _itemViews.Insert(0, last);

                // компенсируем скачок из-за перестановки
                _content.anchoredPosition += Vector2.up * step;
            }
        }

        private float GetItemYInWindow(RectTransform item)
        {
            Vector3 world = item.TransformPoint(item.rect.center);
            Vector3 local = _window.InverseTransformPoint(world);
            return local.y;
        }

        private void SetRandomSprite(SlotView slot)
        {
            if (_sprites == null || _sprites.Count == 0) return;

            slot.SetImage( _sprites[Random.Range(0, _sprites.Count)]);
        }

#if UNITY_EDITOR
        [ContextMenu("Assign _itemViews from children")]
        private void AssignFromChildren()
        {
            if (_content == null) return;
            _itemViews = new List<SlotView>();
            for (int i = 0; i < _content.childCount; i++)
            {
                var sv = _content.GetChild(i).GetComponent<SlotView>();
                if (sv != null) _itemViews.Add(sv);
            }
        }
#endif
    }
}
