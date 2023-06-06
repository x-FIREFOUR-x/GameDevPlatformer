using System;
using UnityEngine;
using UnityEngine.UI;

using Movement.Controller;
using Movement.Data;
using Movement.Enums;
using Core.Animation;
using Fight;

namespace NPC.Behaviour
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class MeleeEntityBehaviour : BaseEntityBehaviour
    {
        private Collider2D _entityCollider;
        private RectTransform _HPBarTransform;
        
        [field: SerializeField] public Vector2 TargetSearchBox { get; private set; }
        [field: SerializeField] public Slider HPBar { get; private set; }

        public event Action AttackSequenceEnded;

        public Vector2 Size => _entityCollider.bounds.size;

        public override void Initialize()
        {
            base.Initialize();
            _entityCollider = GetComponent<BoxCollider2D>();
            Mover = new PositionMover(Rigidbody);
            _HPBarTransform = HPBar.GetComponent<RectTransform>();
        }

        private void Update()
        {
            UpdateAnimations();
            UpdateHPBarRotation();
        }
        
        public override void Move(float direction)
        {
            Mover.Move(direction, true);
        }

        public void Move(float direction, float finalDirection)
        {
            ((PositionMover)Mover).Move(direction, finalDirection);
        }

        public void Attack() => Animator.SetAnimationState(AnimationType.Attack, true, StartAttack, EndAttack);
        
        public override void EndAttack() => AttackSequenceEnded?.Invoke();

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, TargetSearchBox);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(AttackPoint.position, AttackRadius);
        }

        private void UpdateHPBarRotation()
        {
            _HPBarTransform.rotation = Quaternion.Euler(HPBar.GetComponent<RectTransform>().rotation.eulerAngles.x, 0 , HPBar.GetComponent<RectTransform>().rotation.eulerAngles.z);
        }

        public void SetDirection(Direction direction) => Mover.SetDirection(direction);
    }
}
