using Microsoft.AspNetCore.Identity;
using System;

namespace GUI.Models.Identity
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public bool FirstLogin { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Clinic { get; set; }
        public string Specialization { get; set; }
        public string Qualification { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageExtention { get; set; }
        public int Age
        {
            get { return new DateTime(DateTime.Now.Subtract(DateOfBirth).Ticks).Year - 1; }
        }

    }
}
