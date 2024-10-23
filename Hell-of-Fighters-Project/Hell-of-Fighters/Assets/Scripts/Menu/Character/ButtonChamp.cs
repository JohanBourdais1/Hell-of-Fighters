using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Character
{
    public class SelectChamp : MonoBehaviour
    {
        public void OnClick()
        {
            ChampSelect.PlayerSelected=GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text;
            ChampSelect.Selected = true;
        }
    }
}