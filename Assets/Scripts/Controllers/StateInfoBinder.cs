using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Controllers
{
    public class StateInfoBinder : MonoInstaller
    {
        private Dictionary<string, CharacterStateInfo> _characterInfos;

        public override void InstallBindings()
        {
            var jsonSerializer = new JsonSerializer();

            var jsonPath = Application.streamingAssetsPath + "\\CharInfo.json";

#if UNITY_EDITOR

            // load character infos from json file
            using (var fileStream = new FileStream(Application.streamingAssetsPath + "\\CharInfo.json", FileMode.Open))
            {
                using (var textReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    using (var jsonTextReader = new JsonTextReader(textReader))
                    {
                        while (jsonTextReader.Read())
                        {
                            if (jsonTextReader.TokenType == JsonToken.StartObject)
                                _characterInfos = jsonSerializer.Deserialize<Dictionary<string, CharacterStateInfo>>(jsonTextReader);
                        }
                    }
                }
            }
#endif
#if UNITY_ANDROID
            WWW reader = new WWW(jsonPath);

            while (!reader.isDone) { }

            var jsonString = reader.text;
            _characterInfos = JsonConvert.DeserializeObject<Dictionary<string, CharacterStateInfo>>(jsonString);
#endif

            // bind character infos
            foreach (var characterInfo in _characterInfos)
            {
                Container.Bind<CharacterStateInfo>().WithId(characterInfo.Key).FromInstance(characterInfo.Value);
                Debug.Log("hahha");
            }
        }
    }
}
