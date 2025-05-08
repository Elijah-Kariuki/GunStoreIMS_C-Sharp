using System.ComponentModel.DataAnnotations;

namespace GunStoreIMS.Domain.Models
{
    public abstract class Handgun : Firearm
    {
        [Range(1, 20)]
        public decimal BarrelLengthInches { get; set; }
    }
}
