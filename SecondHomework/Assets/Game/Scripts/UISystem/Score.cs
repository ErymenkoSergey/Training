using UnityEngine;
using Game.Interfaces;
using Modules.UI;

namespace Game.Core
{
    public sealed class Score : MonoBehaviour, IScore
    {
        [SerializeField] private ScoreView view;
        
        public void ChangeScore(int value) => view.SetValue(value);
    }
}