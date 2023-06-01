using System;
using System.Collections.Generic;
using System.Linq;

using InputReader;
using Core.Services.Updater;
using StatsSystem;
using StatsSystem.Enum;

namespace Player
{
    class PlayerBrain : IDisposable
    {
        private readonly PlayerEntityBehaviour _playerEntityBehaviour;

        private readonly List<IEntityInputSource> _inputSources;
        
        private readonly IStatValueGiver _statValueGiver;
        
        public PlayerBrain(PlayerEntityBehaviour playerEntityBehaviour, List<IEntityInputSource> inputSources, IStatValueGiver statValueGiver)
        {
            _playerEntityBehaviour = playerEntityBehaviour;
            _inputSources = inputSources;
            _statValueGiver = statValueGiver;

            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdate;
        }

        public void Dispose() => ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdate;

        private void OnFixedUpdate()
        {
            _playerEntityBehaviour.Move(GetMoveDirection() * _statValueGiver.GetStatValue(StatType.Speed));

            if (IsAttack)
                _playerEntityBehaviour.Attack();

            if (IsJump)
                _playerEntityBehaviour.Jump(_statValueGiver.GetStatValue(StatType.JumpForce));

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
    }
}
