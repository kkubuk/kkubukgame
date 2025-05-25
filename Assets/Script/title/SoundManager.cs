using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour
{
    public AudioMixer audioSet;
    public AudioSource bgSound;
    public AudioClip[] bglist;
    public static SoundManager instance;

    public AudioClip buttonSound;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void VolumeSet(float volumesetting)
    {
        audioSet.SetFloat("VolumeSet", Mathf.Log10(volumesetting)*20);
    }
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bglist.Length; i++)
        {
            if (arg0.name == bglist[i].name)
                BgSoundPlay(bglist[i]);
            else
            {
                bgSound.Stop();
            }
        }
    }
    public void UIPlay(string uiName, AudioClip clip)
    {
        GameObject go = new GameObject(uiName + "UISound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = clip;
        audiosource.outputAudioMixerGroup = audioSet.FindMatchingGroups("sfx")[0];
        audiosource.Play();

        Destroy(go, clip.length);
    }

    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.outputAudioMixerGroup = audioSet.FindMatchingGroups("background")[0];
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.1f;
        
        bgSound.Play();
    }

    public void OnClickSound(AudioClip clip)
    {
        UIPlay("UiClik", clip);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
