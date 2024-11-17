using System.IO;
using Google.Protobuf;
using UnityEngine;

namespace BaseX.Scripts
{
    public static class DataHandler
    {
        public static string GeneralPath => $"{Application.persistentDataPath}/Data/";
        public static string Extention => ".data";

        public static bool LoadData<T>(IProtoData<T> source, string fileName, IProtoData<T> defaultValue)
            where T : IMessage<T>, new()
        {
            bool isSuccess = LoadData(source, fileName);
            if (!isSuccess)
            {
                source.SetProtoData(defaultValue.GetProtoData());
                SaveData(source, fileName);
            }

            return true;
        }

        public static bool LoadData<T>(IProtoData<T> source, string fileName) where T : IMessage<T>, new()
        {
            if (!IsFileExists(fileName))
            {
                return false;
            }

            byte[] bytes = LoadFromFile(fileName);
            var baseData = Deserialize<T>(bytes);
            source.SetProtoData(baseData);

            return true;
        }

        public static void SaveData<T>(IProtoData<T> source, string fileName) where T : IMessage<T>, new()
        {
            var protoData = source.GetProtoData();
            var bytes = Serialize(protoData);
            SaveToFile(fileName, bytes);
        }

        private static byte[] Serialize<T>(T player) where T : IMessage<T>
        {
            using MemoryStream stream = new MemoryStream();
            player.WriteTo(stream);
            return stream.ToArray();
        }

        private static T Deserialize<T>(byte[] data) where T : IMessage<T>, new()
        {
            using MemoryStream stream = new MemoryStream(data);
            MessageParser<T> parser = new MessageParser<T>(() => new T());

            return parser.ParseFrom(data);
        }

        private static bool IsFileExists(string fileName)
        {
            var path = Path.Combine(GeneralPath, $"{fileName}{Extention}");
            return File.Exists(path);
        }

        private static void SaveToFile(string fileName, byte[] data)
        {
            if (!Directory.Exists(GeneralPath))
            {
                Directory.CreateDirectory(GeneralPath);
            }

            var path = Path.Combine(GeneralPath, $"{fileName}{Extention}");
            File.WriteAllBytes(path, data);
        }

        private static byte[] LoadFromFile(string fileName)
        {
            var path = Path.Combine(GeneralPath, $"{fileName}{Extention}");
            return File.ReadAllBytes(path);
        }
    }
}