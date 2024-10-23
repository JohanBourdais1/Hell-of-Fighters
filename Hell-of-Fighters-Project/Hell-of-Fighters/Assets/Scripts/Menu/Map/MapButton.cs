using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Map
{
    public class MapButton : MonoBehaviour
    {
        public void OnClick()
        {
            MapSelect.MapSelected=GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text;
            MapSelect.MmapSelected = true;
        }
    }
}