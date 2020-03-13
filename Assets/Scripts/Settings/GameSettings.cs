using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class GameSettings
    {
        public enum BonusAlgorithmType
        {
            Random,
            Sequence,
        }

        public GameDifficulty difficulty = GameDifficulty.Easy;
        [Range(1f, 10f)]
        public float ballSpeed = 5f;
        public float animationTime = 0.2f;
        public BonusAlgorithmType bonusAlgorithm = BonusAlgorithmType.Random;
        [Tooltip("Процент вероятности появления бонуса на плитке. Чем выше процент, тем выше вероятность")]
        public int bonusRandomAlgorithmPercent = 5;
        [Tooltip("Размер блока, в ктором выщитывается позиция для появления бонуса")]
        public int bonusSequenceAlgorithmBlockSize = 5;
    }
}
