using ExamProject.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ExamProject.Models
{
    public class AspNetUserExtendedModel
    {
        public string Id { get; set; }

        [Display(Name = "Kullanıcı Tipi")]
        [Required]
        public string UserType { get; set; }

        //public string UserTypeStr { get { return (byte)UserType == 0 ? "" : Enum.Parse(typeof(EUserType), ((byte)UserType).ToString()).ToString(); } }

        [Display(Name = "İsim")]
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }

        public string UserRole { get; set; }

        [Display(Name = "Soyisim")]
        [MaxLength(30)]
        [Required]
        public string Surname { get; set; }

        //[Display(Name = "Hesap Aktif")]
        //public bool IsActive { get; set; }
        //public string IsActiveStr { get; set; }

        [Required]
        [Display(Name = "E-Posta")]
        [EmailAddress]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }



        //[Display(Name = "Yöneticisi")]
        //public string MasterUserId { get; set; }
        //[Display(Name = "Yöneticisi")]
        //public string MasterUserName { get; set; }


        //[Display(Name = "Doğum Tarihi")]
        //public DateTime? Birthdate { get; set; }

        //internal static List<SelectListItem> GetMasterUserList(int authId)
        //{
        //    List<SelectListItem> _masters = _masters = new List<SelectListItem>();
        //    using (INOCRMEntities _database = new INOCRMEntities())
        //    {
        //        AuthorizationRoles _role = _database.AuthorizationRoles.Where(x => x.AuthorizationId == authId).FirstOrDefault();
        //        if (_role != null)
        //            _masters = _database.AspNetUsersExtended.Where(x => x.AuthorizationRoleId == _role.MainId)
        //                .Select(i => new SelectListItem() { Text = i.Name + " " + i.Surname + " " + i.Title, Value = i.Id })
        //                .ToList();
        //    }

        //    return _masters;
        //}

        //[Display(Name = "Cinsiyet")]
        //public string Gender { get; set; }


        //[Display(Name = "İşe Giriş Tarihi")]
        //public DateTime? EmploymentDate { get; set; }

        //[Display(Name = "Resim")]
        //public string ImagePath { get; set; }
        //[Display(Name = "Resim")]
        //public HttpPostedFileBase EmployeePicture { get; set; }

        [StringLength(20, ErrorMessage = "{0} uzunluğu en az {2} en çok 20 karakter olmalı ve en az 1'er adet küçük harf, büyük harf, ve sayı içermelidir !", MinimumLength = 6)]
        //[RequiredIf("CreateWebUserStr", "on", ErrorMessage = "Sistem kullanıcısı için, şifre belirleyiniz")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d$@$!%*?&]{6,}", ErrorMessage = "Şifre en az 6 karakter uzunluğunda olmalı ve en az 1'er adet küçük harf, büyük harf, ve sayı içermelidir !")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }


        //[Display(Name = "Yetki Grubu")]
        //public string AuthorizationRoleName { get; set; }

        //internal static List<SelectListItem> GetAllUsersSelectList(int CorporateId)
        //{
        //    List<SelectListItem> _list;
        //    using (var context = new INOCRMEntities())
        //    {
        //        _list = context.AspNetUsersExtended.Where(x => x.CorporateId == CorporateId).OrderBy(x => x.Name).Select(x => new SelectListItem() { Value = x.Id, Text = x.Name + " " + x.Surname }).ToList();
        //    }

        //    return _list;
        //}

        //[Display(Name = "Oluşturma Tarihi")]
        //public System.DateTime CreateDate { get; set; }
        //[Display(Name = "Güncelleme Tarihi")]
        //public DateTime? UpdateDate { get; set; }
        public string ProfilImageFileName { get; set; }
        public string ImageGuid { get; set; }
        public string ImageGuidPath { get; set; }
        internal static AspNetUserExtendedModel LoadModel(string userId)
        {
            AspNetUserExtendedModel _model;

            using (var context = new ExamProjectDbEntities())
            {
                //_model = context.AspNetUsersExtended.Where(x => x.Id.Equals(userId))
                //    .Select(u => new AspNetUsersExtendedModel()
                //    {
                //        Id = u.Id,
                //        Name = u.Name,
                //        Surname = u.Surname,

                //    })
                //    .FirstOrDefault();
                _model = (from UserExtended in context.AspNetUserExtended
                             .Where(x => x.Id == userId).DefaultIfEmpty()
                          from RoleExtended in context.AspNetRoles
                          .Where(x => x.Id == UserExtended.UserType).DefaultIfEmpty()
                          select new AspNetUserExtendedModel
                          {
                              UserRole = RoleExtended.Name,
                              Id = UserExtended.Id,
                              Name = UserExtended.Name,
                              Surname = UserExtended.SurName,
                              UserType = UserExtended.UserType,

                          }).FirstOrDefault();

                //var _type = (from UserExtended in context.AspNetUsersExtended.Where(x => x.Id.Equals(userId))
                //             join RoleExtended in context.AspNetRoles on UserExtended.UserType equals RoleExtended.Id select RoleExtended).FirstOrDefault();


                var _usr = context.AspNetUsers.Where(x => x.Id.Equals(userId)).FirstOrDefault();
                _model.Email = _usr.Email;
                _model.EmailConfirmed = _usr.EmailConfirmed;

            }

            return _model;
        }
        internal static List<AspNetUserExtendedModel> GetList()
        {
            List<AspNetUserExtendedModel> _list = null;
            using (var context = new ExamProjectDbEntities())
            {
                _list = (from UserExtended in context.AspNetUserExtended
                         join RoleExtended in context.AspNetRoles on UserExtended.UserType equals RoleExtended.Id
                         join User in context.AspNetUsers on UserExtended.Id equals User.Id
                         select new AspNetUserExtendedModel()
                         {
                             UserRole = RoleExtended.Name,
                             Email = User.Email,
                             EmailConfirmed = User.EmailConfirmed,
                             UserType = RoleExtended.Id,
                             Id = UserExtended.Id,
                             Name = UserExtended.Name,
                             Surname = UserExtended.SurName,
                         }).ToList();
            }
            return _list;
        }
        internal void Create(string userId)
        {
            using (var context = new ExamProjectDbEntities())
            {
                AspNetUserExtended _ext = new AspNetUserExtended()
                {
                    Id = userId,
                    Name = Name,
                    SurName = Surname,
                    UserType = UserType,

                };
                context.Entry(_ext).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
        }
        

        internal static string DeleteUser(string id)
        {
            string _email;
            using (var context = new ExamProjectDbEntities())
            {
                var _user = context.AspNetUsers.Where(u => u.Id == id).FirstOrDefault();
                _email = _user.Email;
                var _exUser = context.AspNetUserExtended.Where(u => u.Id == id).FirstOrDefault();
                context.Entry(_user).State = System.Data.Entity.EntityState.Deleted;
                context.Entry(_exUser).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();


            }
            return _email;
        }

    }
}