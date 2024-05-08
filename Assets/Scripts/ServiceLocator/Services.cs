using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace.ServiceLocator
{
    public class MockLocalization : ILocalization
    {
        private readonly List<string> words = new List<string>() { "köpek", "kedi" ,"balık", "boğa", "kuş"};
        private readonly System.Random random = new Random();

        public string GetLocalizedWord(string key)
        {
            return words[random.Next(words.Count)];
        }
    }
    
    public class MockSerializer : ISerializer
    {
        public void Serialize()
        {
            Debug.Log("MockSerialized.Serialize");
        }
    }
    
    public class MockAudioService : IAudioService
    {
        public void Play()
        {
            Debug.Log("MockAudioService.Play");
        }
    }

    public class MockGameService : IGameService
    {
        public void StartGame()
        {
            Debug.Log("MockGameService.StartGame");
        }
    }
}