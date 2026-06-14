using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public CharacterClass warriorClass;
    public CharacterClass mageClass;
    public CharacterClass archerClass;

    public static CharacterClass SelectedClass { get; private set; }

    public void SelectWarrior()
    {
        SelectedClass = warriorClass;
        StartGame();
    }

    public void SelectMage()
    {
        SelectedClass = mageClass;
        StartGame();
    }

    public void SelectArcher()
    {
        SelectedClass = archerClass;
        StartGame();
    }

    private void StartGame()
    {
        if (SelectedClass == null) SelectedClass = warriorClass;

        SceneManager.LoadScene("Scene_Lab2");
    }
}