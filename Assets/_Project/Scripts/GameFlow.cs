using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{

    [SerializeField] PlayerController playerController;
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject titleScreenMenu;
    [SerializeField] CinemachineCamera titleScreenCamera;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Light day;
    [SerializeField] Light night;
    [SerializeField] Image fadeScreen;
    [SerializeField] float fadeDuration = 2;
    [SerializeField] float standUpDuration = 3f;

    private void Awake()
    {
        night.gameObject.SetActive(true);
        day.gameObject.SetActive(false);
    }

    void Start()
    {
        
    }
    public void StartGame()
    {
        StartCoroutine(StartSequence());
    }
    IEnumerator StartSequence()
    {
        titleScreenMenu.SetActive(false);
        titleScreenCamera.Priority = 0;

        playerAnimator.SetTrigger("StandUp");
        yield return new WaitForSeconds(standUpDuration);

        yield return Fade(0f, 1f);
        night.gameObject.SetActive(false);
        day.gameObject.SetActive(true);
        yield return Fade(1f, 0f);

        playerController.enabled = true;
        characterController.enabled = true;

    }

    IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;


            Color c = fadeScreen.color;
            c.a = Mathf.Lerp(from, to, t);
            fadeScreen.color = c;

            yield return null;
        }
    }

    public void StartBattle()
    {
        StartCoroutine(BattleTransition());
    }
    IEnumerator BattleTransition()
    {
        fadeDuration = 1f;
        playerController.enabled = false;
        yield return Fade(0f, 1f);
        SceneManager.LoadScene("Battle");
    }

    void Update()
    {
        
    }
}
