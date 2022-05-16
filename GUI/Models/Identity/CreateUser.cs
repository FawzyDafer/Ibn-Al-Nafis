using GUI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GUI.Models.Identity
{
    public class PublicUser
    {
        [Required(ErrorMessage = "please add name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please select the sex")]
        [Display(Name = "Sex")]
        public string Sex { get; set; }
        [Required(ErrorMessage = "Plaease add date of birth")]
        [Display(Name = "Date of Birth")]
        [DateValidation(ErrorMessage = "You can not add an employee under 18 years of age.", Year = 18)]
        public DateTime DateofBirth { get; set; }
        [Required(ErrorMessage = "Please select a jop title")]
        [Display(Name = "Jop")]
        public string Staff { get; set; }
        [SpecializationValidation("Staff")]
        public string Specialization { get; set; }
        public string Qualification { get; set; }
        public string Image { get; set; }
    }
    public class CreateUser : PublicUser
    {
        [Required(ErrorMessage = "Please add user name")]
        [Remote(action: "VerifyUserName", controller: "Staff")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
    }
    public class UserandRole
    {
        public User User { get; set; }
        public IList<string> Role { get; set; }
    }
    public class UsersPaging
    {
        public List<UserandRole> UserandRoles { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string Search { get; set; }
    }
    public class EditUser : PublicUser
    {
        [Required(ErrorMessage = "Can not remove id")]
        public string Id { get; set; }
        public string Search { get; set; }
        [RegularExpression(@"(01)(0|1|2|5)[0-9]{8}", ErrorMessage = "It is not a correct phone")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please add user name")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
    }
    public class ChangePassword
    {
        [Required(ErrorMessage = "Please enter username")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter new password")]
        [Display(Name = "Password")]
        [RegularExpression(@"^((?=.*[a-z])|(?=.*[A-Z])|(?=.*\d)).{8,15}$",
            ErrorMessage = "Password must be at least of 8 character")]
        [Remote(action: "VerifyPassword", controller: "Account", AdditionalFields = "UserName")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please confirm the password")]
        [Compare("Password", ErrorMessage = "The password does not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Please enter new password")]
        [Display(Name = "Old Password")]
        [RegularExpression(@"^((?=.*[a-z])|(?=.*[A-Z])|(?=.*\d)).{8,15}$",
            ErrorMessage = "In correct Password")]
        public string OldPassword { get; set; }
        public string Search { get; set; }
    }
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter username")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please Enter your Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
