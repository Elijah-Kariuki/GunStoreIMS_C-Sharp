namespace GunStoreIMS.Domain.Models
{
    public sealed class Rifle : LongGun
    {
        public Rifle() => Type = FirearmType.Rifle;
    }
}
