using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Items.Behaviour
{
    public class SceneItem: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;
        [SerializeField] private Canvas _canvas;

        [Header("DropAnimation")]
        [SerializeField] private float _dropAnimTime;
        [SerializeField] private float _dropRadius;
        [SerializeField] private float _dropRotation;
        [SerializeField] private Transform _itemTransform;

        private Sequence _sequence;

        [field: SerializeField] public float InteractionDistance;
        public Vector2 Position => _itemTransform.position;

        public event Action<SceneItem> ItemClicked;

        private bool _textEnabled = true;
        public bool TextEnabled 
        { 
            set 
            {
                if (_textEnabled != value)
                    return;

                _textEnabled = value;
                _canvas.enabled = false;
            } 
        }

        private void Awake() => _button.onClick.AddListener(() => ItemClicked?.Invoke(this));

        private void OnMouseDown() => ItemClicked?.Invoke(this);

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_itemTransform.position, InteractionDistance);
        }

        public void SetItem(Sprite sprite, string itemName, Color textColor)
        {
            _sprite.sprite = sprite;
            _text.text = itemName;
            _text.color = textColor;
            _canvas.enabled = false;
        }

        public void PlayAnimationDrop(Vector2 position)
        {
            transform.position = position;
            Vector2 movePosition = (transform.position + new Vector3(UnityEngine.Random.Range(-_dropRadius, _dropRadius), 0, 0));

            _sequence = DOTween.Sequence();
            _sequence.Join(transform.DOMove(movePosition, _dropAnimTime));
            _sequence.Join(
                _itemTransform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(-_dropRotation, _dropRotation)), _dropAnimTime));

            _sequence.OnComplete(() => _canvas.enabled = _textEnabled);
        }
    }
}
