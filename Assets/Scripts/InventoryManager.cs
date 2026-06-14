using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Головна панель інтерфейсу")]
    public GameObject inventoryWindow;

    [Header("Вкладки (Панелі)")]
    public GameObject equipmentPanel;
    public GameObject statsPanel;
    public GameObject craftPanel;
    public GameObject shopPanel;

    private bool isWindowOpen = false;

    void Start()
    {
        if (inventoryWindow != null) inventoryWindow.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isWindowOpen = !isWindowOpen;
        inventoryWindow.SetActive(isWindowOpen);
        Time.timeScale = isWindowOpen ? 0f : 1f;

        if (isWindowOpen)
        {
            OpenEquipment();
        }
    }

    public void OpenEquipment()
    {
        SetAllPanelsActive(false);
        if (equipmentPanel != null) equipmentPanel.SetActive(true);
    }

    public void OpenStats()
    {
        SetAllPanelsActive(false);
        if (statsPanel != null) statsPanel.SetActive(true);
    }

    public void OpenCraft()
    {
        SetAllPanelsActive(false);
        if (craftPanel != null) craftPanel.SetActive(true);
    }

    public void OpenShop()
    {
        SetAllPanelsActive(false);
        if (shopPanel != null) shopPanel.SetActive(true);
    }

    private void SetAllPanelsActive(bool state)
    {
        if (equipmentPanel != null) equipmentPanel.SetActive(state);
        if (statsPanel != null) statsPanel.SetActive(state);
        if (craftPanel != null) craftPanel.SetActive(state);
        if (shopPanel != null) shopPanel.SetActive(state);
    }


}