using UnityEngine;
using UnityEngine.SceneManagement;

namespace ContradictiveGames
{
    public class MenuScripts : MonoBehaviour
    {
        public void LoadLevelByName(string levelName){
            if(SceneManager.GetSceneByName(levelName) != null){
                SceneManager.LoadScene(levelName);
            }
            else{
                Debug.LogError($"Scene name: {levelName} does not exist!");
            }
        }


        // DEBUG ONLY
        public void OpenPanel(GameObject panelToOpen){
            panelToOpen.SetActive(true);
        }
        public void ClosePanel(GameObject panelToClose){
            panelToClose.SetActive(false);
        }

        public void QuitGame(){
            Application.Quit();
        }
    }
}
