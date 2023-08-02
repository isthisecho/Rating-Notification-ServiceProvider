namespace HomeRun.RatingService
{
    public interface IMessageProducer
    {
        void SendingMessage<T>(T message);
    }
}
