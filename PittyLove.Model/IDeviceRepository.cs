namespace PittyLove.Model
{
    public interface IDeviceRepository : IRepository<Device>
    {
        Device GetByPublicKey(string publicKey);
    }
}