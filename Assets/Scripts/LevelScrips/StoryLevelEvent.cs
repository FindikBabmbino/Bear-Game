using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StoryLevelEvent : MonoBehaviour
{
    [SerializeField][TextArea] string storyBeat00;
    [SerializeField][TextArea] string storyBeat01;
    [SerializeField][TextArea] string storyBeat02;
    [SerializeField][TextArea] string storyBeat03;

    [SerializeField]private GameObject storyTextObject;

    private void Start()
    {

        StartCoroutine(ShowStory());
    }


    private IEnumerator ShowStory()
    {
        yield return new WaitForSeconds(0.5f);
        storyTextObject.SetActive(true);
        storyTextObject.GetComponent<TextMeshProUGUI>().text = storyBeat00;
        yield return new WaitForSeconds(5f);
        storyTextObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        storyTextObject.SetActive(true);
        storyTextObject.GetComponent<TextMeshProUGUI>().text = storyBeat01;
        yield return new WaitForSeconds(5f);
        storyTextObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        storyTextObject.SetActive(true);
        storyTextObject.GetComponent<TextMeshProUGUI>().text = storyBeat02;
        yield return new WaitForSeconds(5f);
        storyTextObject.SetActive(false);


        yield return new WaitForSeconds(0.5f);
        storyTextObject.SetActive(true);
        storyTextObject.GetComponent<TextMeshProUGUI>().text = storyBeat03;
        yield return new WaitForSeconds(5f);
        storyTextObject.SetActive(false);

        SceneManager.LoadScene("PlayLevel");


        //Load scene here



        yield return null;
    }

}
