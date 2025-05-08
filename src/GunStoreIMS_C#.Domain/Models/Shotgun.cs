namespace GunStoreIMS.Domain.Models
{
    public sealed class Shotgun : LongGun
    {
        public Shotgun() => Type = FirearmType.Shotgun;
    }
}
