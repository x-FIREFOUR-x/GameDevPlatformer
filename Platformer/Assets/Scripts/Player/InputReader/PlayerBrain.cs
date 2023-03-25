﻿using System.Collections.Generic;
using System.Linq;

namespace Player.InputReader
{
    class PlayerBrain
    {
        private readonly PlayerEntity _playerEntity;

        List<IEentityInputSource> _inputSources;


        public PlayerBrain(PlayerEntity playerEntity, List<IEentityInputSource> inputSources)
        {
            _playerEntity = playerEntity;
            _inputSources = inputSources;
        }

        public void OnFixedUpdate()
        {
            _playerEntity.Move(GetHorizontalDirection());

            if (IsAttack)
                _playerEntity.Attack();

            if (IsJump)
                _playerEntity.Jump();

            if (IsRoll)
                _playerEntity.Roll();

            _playerEntity.Block(IsBlock());

            foreach (var inputSource in _inputSources)
            {
                inputSource.ResetOneTimeActions();
            }
        }

        private float GetHorizontalDirection()
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
