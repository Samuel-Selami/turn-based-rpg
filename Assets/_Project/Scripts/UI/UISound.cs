using UnityEngine;

public class UISound : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip navigateClip;
    [SerializeField] float navigateClipVolume = 0.1f;
    [SerializeField] AudioClip confirmClip;
    [SerializeField] AudioClip errorClip;
    [SerializeField] float clipVolume = 0.2f;

    public void PlayNavigate()
    {
        audioSource.PlayOneShot(navigateClip, navigateClipVolume);
    }

    public void PlayConfirm()
    {
        audioSource.PlayOneShot(confirmClip, clipVolume);
    }

    public void PlayError()
    {
        audioSource.PlayOneShot(errorClip, clipVolume);
    }

}
