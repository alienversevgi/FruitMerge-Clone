using Google.Protobuf;

namespace BaseX
{
    public interface IProtoData<T> where T : IMessage<T>
    {
        T GetProtoData();
        void SetProtoData(T data);
    }
}