namespace GunStoreIMS.Domain.Models
{
    public sealed class Revolver : Handgun
    {
        public Revolver() => Type = FirearmType.Revolver;
    }
}
