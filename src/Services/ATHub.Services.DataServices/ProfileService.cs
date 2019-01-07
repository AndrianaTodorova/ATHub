using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ATHub.Data.Common;
using ATHub.Data.Models;
using ATHub.Services.Data.Models;
using Microsoft.AspNetCore.Http;

namespace ATHub.Services.DataServices
{
    public class ProfileService : IProfileService
    {
        private readonly IRepository<ATHubUser> userRepository;
        private readonly IRepository<Image> imageRepository;
        private readonly IRepository<UserProfile> userProfileRepository;

        public ProfileService(IRepository<ATHubUser> userRepository,
            IRepository<Image> imageRepository,
            IRepository<UserProfile> userProfileRepository)
        {
            this.userRepository = userRepository;
            this.imageRepository = imageRepository;
            this.userProfileRepository = userProfileRepository;
        }

        public MyProfileViewModel GetProfile(ATHubUser user)
        {
            var profile = new MyProfileViewModel()
            {
                Username = user.UserName,
                Email = user.Email
            };
            if(user.UserProfileId != null)
            {
                profile.FacebookLink = user.UserProfile.FacebookLink;
                profile.InstagramLink = user.UserProfile.InstagramLink;
                profile.Birthdate = user.UserProfile.Birthdate;
                profile.Country = user.UserProfile.Country;
                profile.Phone = user.UserProfile.Phone;
                profile.ImageUrl = user.UserProfile.Image.Url;
            }
            return profile;
        }

        public async Task<int> UploadImg(IFormFile file, ATHubUser user)
        {
            string baseImagePath = Directory.GetCurrentDirectory() + "/StaticFiles";
            string baseImageFile = baseImagePath + "/images/" + file.FileName;
            using (var stream = new FileStream( baseImageFile, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            var thumbDirectory = baseImagePath + "/images/thumbs/" + file.FileName;

            var image = System.Drawing.Image.FromFile(baseImageFile);
            var thumb = image.GetThumbnailImage(120, 120, () => false, IntPtr.Zero);
             thumb.Save(Path.ChangeExtension(thumbDirectory, "png"));
            var img = new Image()
            {
                Url = "/images/" + file.FileName,
                Thumbnail = Path.ChangeExtension("images/thumbs/" + file.FileName, "png")
            };
            await this.imageRepository.AddAsync(img);
            await this.imageRepository.SaveChangesAsync();
            var userProfile = new UserProfile()
            {
                Image = img,
                ImageId = img.Id
            };
            await this.userProfileRepository.AddAsync(userProfile);
            await this.userProfileRepository.SaveChangesAsync();
            user.UserProfile = userProfile;
            user.UserProfileId = userProfile.Id;
            await this.userRepository.SaveChangesAsync();



            return userProfile.Id;
        }
    }
}
