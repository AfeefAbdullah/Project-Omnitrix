using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public Sound[] sounds;

	public static AudioManager instance;

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
	}

	private void Start()
	{
		Play("Theme");
	}

	public void Play(string name)
	{
		Array.Find(sounds, sound => sound.name == name)?.source.Play();
	}

	public void Stop(string name)
	{
		Array.Find(sounds, sound => sound.name == name)?.source.Stop();
	}
}
