using System;
using System.Collections.Generic;
using System.Linq;

using InputReader;
using Core.Services.Updater;
using Fight;
using NPC.Controller;
using StatsSystem;
using StatsSystem.Enum;
using UnityEngine;

namespace Player
{
    class PlayerEntity : Entity, IDisposable
    {
        private readonly PlayerEntityBehaviour _playerEntityBehaviour;

        private readonly List<IEntityInputSource> _inputSources;

        private bool _isAttacking;
        private bool _canAttack = true;

        public PlayerEntity(PlayerEntityBehaviour playerEntityBehaviour, List<IEntityInputSource> inputSources, StatsController statValueGiver )
        : base(playerEntityBehaviour, statValueGiver)
        {
            _playerEntityBehaviour = playerEntityBehaviour;
            _playerEntityBehaviour.AttackEnded += OnAttackEnded;
            _playerEntityBehaviour.Attacked += OnAttacked;
            _inputSources = inputSources;

            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdate;

            VisualiseHP(StatsController.GetStatValue(StatType.Health));
        }

        private void OnAttacked(IDamageable target)
        {
            target.TakeDamage(StatsController.GetStatValue(StatType.Damage));
        }

        private void OnAttackEnded()
        {
            _isAttacking = false;
            ProjectUpdater.Instance.Invoke(() =>
                _canAttack = true, StatsController.GetStatValue((StatType.AfterAttackDelay)));
        }

        public void Dispose()
        {
            base.Dispose();
            ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdate;
            _playerEntityBehaviour.AttackEnded -= OnAttackEnded;
            _playerEntityBehaviour.Attacked -= OnAttacked;
        }

        private void OnFixedUpdate()
        {

            _playerEntityBehaviour.Move(GetMoveDirection() * StatsController.GetStatValue(StatType.Speed));

            if (IsAttack && _canAttack)
            {
                if (_playerEntityBehaviour.TryStartAttack())
                {
                    _isAttacking = true;
                    _canAttack = false;
                }
            }
                

            if (IsJump)
                _playerEntityBehaviour.Jump(StatsController.GetStatValue(StatType.JumpForce));

            if (IsRoll)
                _playerEntityBehaviour.Roll();

            _playerEntityBehaviour.Block(IsBlock());

            foreach (var inputSource in _inputSources)
            {
                inputSource.ResetOneTimeActions();
            }
        }

        private float GetMoveDirection()
        {
            foreach(var inputSource in _inputSources)
            {
                if (inputSource.HorizontalDirection == 0)
                    continue;

                return inputSource.HorizontalDirection;
            }

            return 0;
        }

        private bool IsAttack => _inputSources.Any(sources => sources.Attack);
        private bool IsJump => _inputSources.Any(sources => sources.Jump);
        private bool IsRoll => _inputSources.Any(sources => sources.Roll);

        private bool IsBlock()
        {
            foreach (var inputSource in _inputSources)
            {
                if (inputSource.Block == true)
                    return true;
            }

            return false;
        }

        protected sealed override void VisualiseHP(float currentHp)
        {
            if (_playerEntityBehaviour.statsUIView.HPBar.maxValue < currentHp)
                _playerEntityBehaviour.statsUIView.HPBar.maxValue = currentHp;

            _playerEntityBehaviour.statsUIView.HPBar.value = currentHp;
        }
    }
}
