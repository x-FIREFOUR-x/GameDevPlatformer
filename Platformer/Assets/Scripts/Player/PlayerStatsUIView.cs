using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerStatsUIView : MonoBehaviour
    {
        [field: SerializeField] public Slider HPBar { get; private set; }
        [field: SerializeField] public TMP_Text CurrentHPText { get; private set; }
    }
}
