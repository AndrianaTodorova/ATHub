using ATHub.Data.Models;
using ATHub.Services.Data.Models;
using ATHub.Services.DataServices;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATHub.Tets
{
    public class ProfileServiceTests : BaseServiceTests
    {

        private const string FacebookLink = "https://www.facebook.com/";
        private const string InstagramLink = "https://www.instagram.com/?hl=en";
        private const string Country = "Bulgaria";
        private const string Phone = "0893222063";
        private IProfileService ProfileServiceMock => this.ServiceProvider.GetRequiredService<IProfileService>();

        [Test]
        public async Task GetProfile()
        {
            var user = this.AddTestingUserToDb();
            var userProfile = new UserProfile()
            {
                FacebookLink = FacebookLink,
                InstagramLink = InstagramLink,
                Birthdate = DateTime.UtcNow,
                Country = Country,
                Phone = Phone,
                Image = new Image()
                {
                    Url = "some url",
                    Thumbnail = "some tumbnail"
                }
            };
            await this.DbContext.UsersProfiles.AddAsync(userProfile);
            await this.DbContext.SaveChangesAsync();
            user.UserProfile = userProfile;
            await this.DbContext.SaveChangesAsync();
            var profile = new MyProfileViewModel()
            {
                Username = user.UserName,
                Email = user.Email
            };

            profile.FacebookLink = user.UserProfile.FacebookLink;
            profile.InstagramLink = user.UserProfile.InstagramLink;
            profile.Birthdate = user.UserProfile.Birthdate;
            profile.Country = user.UserProfile.Country;
            profile.Phone = user.UserProfile.Phone;
            profile.ImageUrl = user.UserProfile.Image.Url;
           

            var actual = this.ProfileServiceMock.GetProfile(user.Id);
            var asd = profile.InstagramLink;
            Assert.That(profile.InstagramLink.Equals(actual.InstagramLink));
            Assert.That(profile.FacebookLink.Equals(actual.FacebookLink));
            Assert.That(profile.Country.Equals(actual.Country));

        }
        private ATHubUser AddTestingUserToDb()
        {
            var result = new ATHubUser
            {
                Id = "1",
                UserName = "testName",
                Email = "test@mail.bg",
            };
            this.DbContext.Users.Add(result);
            this.DbContext.SaveChangesAsync();
            return result;
        }
    }
}
