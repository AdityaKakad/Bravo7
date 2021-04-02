using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsMenu : MonoBehaviour
{
    public GameObject canvasObject_1;
    public GameObject canvasObject_2;
    public GameObject canvasObject_3;
    public GameObject canvasObject_4;
    public GameObject canvasObject_5;
    public void BackToMain()
    {
        //Instructions.gameObject.SetActive(true);
        canvasObject_1.SetActive(false);
        canvasObject_2.SetActive(false);
        canvasObject_3.SetActive(false);
        canvasObject_4.SetActive(false);
        canvasObject_5.SetActive(false);

        SceneManager.LoadScene("Menu");
    }
    public void MakeActive_1()
    {
        canvasObject_1.SetActive(false);
        canvasObject_2.SetActive(true);
        canvasObject_3.SetActive(false);
        canvasObject_4.SetActive(false);
        canvasObject_5.SetActive(false);


    }


    public void MakeActive_2()
    {
        canvasObject_1.SetActive(false);
        canvasObject_2.SetActive(false);
        canvasObject_3.SetActive(true);
        canvasObject_4.SetActive(false);
        canvasObject_5.SetActive(false);


    }
    public void MakeActive_3()
    {
        canvasObject_1.SetActive(false);
        canvasObject_2.SetActive(false);
        canvasObject_3.SetActive(false);
        canvasObject_4.SetActive(true);
        canvasObject_5.SetActive(false);


    }
    public void MakeActive_4()
    {
        canvasObject_1.SetActive(false);
        canvasObject_2.SetActive(false);
        canvasObject_3.SetActive(false);
        canvasObject_4.SetActive(false);
        canvasObject_5.SetActive(true);


    }
   
}
