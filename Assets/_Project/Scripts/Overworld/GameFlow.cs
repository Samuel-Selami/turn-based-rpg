using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Encounter;

public class GameFlow : MonoBehaviour
{

    [SerializeField] PlayerController playerController;
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject titleScreenMenu;
    [SerializeField] CinemachineCamera titleScreenCamera;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Light day;
    [SerializeField] Light night;
    [SerializeField] float standUpDuration = 3f;
    [SerializeField] float gameStartFadeDuration = 1f;
    [SerializeField] float battleFadeDuration = 1f;
    [SerializeField] Transform respawnPoint;
    [Header("Sound")]
    [SerializeField] AudioSource ambienceNight;
    [SerializeField] AudioSource ambienceDay;
    [SerializeField] AudioSource musicGame;

    private void Awake()
    {
        if (Encounter.returnReason != ReturnReason.None)
            return;
        night.gameObject.SetActive(true);
        day.gameObject.SetActive(false);
    }

    void Start()
    {
        ScreenFader.instance.FadeOut(battleFadeDuration);

        if (Encounter.returnReason == ReturnReason.None)
            return;

        Encounter.aggroResumeTime = Time.time + 3f;
        night.gameObject.SetActive(false);
        day.gameObject.SetActive(true);

        ambienceNight.Stop();
        ambienceDay.Play();
        musicGame.Play();

        titleScreenMenu.SetActive(false);
        titleScreenCamera.Priority = 0;

        if (Encounter.returnReason == ReturnReason.Defeat)
        {
            characterController.transform.position = respawnPoint.position;
            characterController.transform.rotation = respawnPoint.rotation;
        }
        else
        {
            characterController.transform.position = Encounter.playerPosition;
            characterController.transform.rotation = Encounter.playerRotation;
        }

        playerController.enabled = true;
        characterController.enabled = true;

        playerAnimator.Play("Locomotion");

        Encounter.returnReason = ReturnReason.None;
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

        yield return ScreenFader.instance.Fade(0f, 1f, gameStartFadeDuration);
        night.gameObject.SetActive(false);
        day.gameObject.SetActive(true);
        ambienceNight.Stop();
        ambienceDay.Play();
        musicGame.Play();
        yield return ScreenFader.instance.Fade(1f, 0f, gameStartFadeDuration);

        playerController.enabled = true;
        characterController.enabled = true;

    }

    public void StartBattle()
    {
        StartCoroutine(BattleTransition());
    }
    IEnumerator BattleTransition()
    {
        playerController.enabled = false;
        playerAnimator.SetFloat("Speed", 0f);
        yield return ScreenFader.instance.Fade(0f, 1f, battleFadeDuration);
        SceneManager.LoadScene("Battle");
    }

    void Update()
    {
        
    }
}
