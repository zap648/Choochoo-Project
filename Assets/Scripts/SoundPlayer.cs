using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    AudioSource audioSource;
    float[] audioData;
    [SerializeField]
    float maxDistance = 10.0f;

    private void Awake()
    {
        string audioFilePath = ReadAudioFilePathFromFile();
        audioSource = GetComponent<AudioSource>();
        // 3D settings
        audioSource.spatialize = true;
        audioSource.spatialBlend = 1.0f; // 1.0 means full 3D spatialization
        audioSource.spatializePostEffects = true;
        audioSource.spatialize = true;
        audioSource.maxDistance = 10.0f;

        if (!string.IsNullOrEmpty(audioFilePath))
        {
            audioData = LoadAudioData(audioFilePath);
            PlayAudio();
        }
        else
        {
            Debug.LogError("Failed to read audio file path.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // LateUpdate is called once per frame after Update
    void LateUpdate()
    {
        Vector3 playerPosition = Camera.main.transform.position; // player position
        Vector3 soundPosition = transform.position; // source of audio // locomotive
        float distance = Vector3.Distance(playerPosition, soundPosition);
        // Debug.Log(soundPosition.ToString());
        //audioSource.transform.position = soundPosition
        audioSource.volume = Mathf.Clamp01(1.0f - distance / maxDistance);   //  adjust volume based on distance

    }

    float[] LoadAudioData(string path)
    {
        try
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            int headerOffset = 44;
            float[] audioData = new float[(fileBytes.Length - headerOffset) / 2];

            for (int i = 0; i < audioData.Length; i++)
            {
                audioData[i] = (short)(fileBytes[headerOffset + i * 2] | (fileBytes[headerOffset + i * 2 + 1] << 8)) / 32768.0f;
            }
            return audioData;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load audio data: {e.Message}");
            return null;
        }
    }

    string ReadAudioFilePathFromFile()
    {
        string p1 = Application.dataPath;
        p1 = p1.Replace("/Assets", "/Assets/Sounds/train-travel-sound-27813.wav");
        Debug.Log(p1.ToString());
        return p1.ToString();
    }

    void PlayAudio()
    {
        if (audioData != null && audioData.Length > 0)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = AudioClip.Create("LoadedClip", audioData.Length, 1, 44100, false);
            audioSource.clip.SetData(audioData, 0);
            audioSource.Play(0);
        }
        else
        {
            Debug.LogError("Failed to load audio data.");
        }
    }
}
