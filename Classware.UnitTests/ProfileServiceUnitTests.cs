using Classware.Core.Contracts;
using Classware.Core.Services;
using Classware.Infrastructure.Common;
using Classware.Infrastructure.Data;
using Classware.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.UnitTests
{
    [TestFixture]
    public class ProfileServiceUnitTests
    {
        private IRepository repo;
        private UserManager<ApplicationUser> userManager;
        private ApplicationDbContext dbContext;
        private IProfileService profileService;

        [SetUp]
        public void SetUp()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ClasswareDb")
                .Options;

            dbContext = new ApplicationDbContext(contextOptions);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        [Test]
        public async Task Test_EditProfileInformationAsyncShouldThrowExceptionWhenCannotFindUser()
        {
            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Id = "3b1a9553-091d-4972-83a2-dc2280c57c3e",
                    Email  ="user1@abv.bg",
                    UserName = "User1",
                    FirstName = "TestUser1"
                },
                new ApplicationUser()
                {
                    Id = "49529761-1248-4739-a965-5a25cc410967",
                    Email  ="user2@abv.bg",
                    UserName = "User2",
                    FirstName = "TestUser2"

                }
            };

            var userStore = new UserStore<ApplicationUser>(dbContext, null);
            userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);

            foreach (var user in users)
            {
                await userManager.CreateAsync(user);
            }

            var user2Id = "49529761-1248-4739-a965-5a25cc410967";

            repo = new Repository(dbContext);
            profileService = new ProfileService(userManager,repo);

            Assert.ThrowsAsync<NullReferenceException>(async () => await profileService.EditProfileInformationAsync("", new byte[1], null, null, null, null, null));
        }

        [Test]
        public async Task Test_EditProfileInformationAsyncShouldEditProperly()
        {
            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Id = "3b1a9553-091d-4972-83a2-dc2280c57c3e",
                    Email  ="user1@abv.bg",
                    UserName = "User1",
                    FirstName = "TestUser1"
                },
                new ApplicationUser()
                {
                    Id = "49529761-1248-4739-a965-5a25cc410967",
                    Email  ="user2@abv.bg",
                    UserName = "User2",
                    FirstName = "TestUser2"
                   
                }
            };

            var userStore = new UserStore<ApplicationUser>(dbContext,null);
            userManager = new UserManager<ApplicationUser>(userStore, null,null,null,null,null,null,null,null);

            foreach (var user in users)
            {
                await userManager.CreateAsync(user);
            }

            var user2Id = "49529761-1248-4739-a965-5a25cc410967";

            repo = new Repository(dbContext);
            profileService = new ProfileService(userManager, repo);

            await profileService.EditProfileInformationAsync(user2Id, new byte[1], "EditedUser2", null, null, null, null);

            Assert.That((await userManager.FindByIdAsync(user2Id)).FirstName, Is.EqualTo("EditedUser2"));
        }

        [Test]
        public async Task TestUploadPictureAsyncShouldUploadProperly()
        {

            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Id = "3b1a9553-091d-4972-83a2-dc2280c57c3e",
                    Email  ="user1@abv.bg",
                    UserName = "User1",
                    FirstName = "TestUser1"
                },
                new ApplicationUser()
                {
                    Id = "49529761-1248-4739-a965-5a25cc410967",
                    Email  ="user2@abv.bg",
                    UserName = "User2",
                    FirstName = "TestUser2"

                }
            };

            var userStore = new UserStore<ApplicationUser>(dbContext, null);
            userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);

            foreach (var user in users)
            {
                await userManager.CreateAsync(user);
            }

            var user2Id = "49529761-1248-4739-a965-5a25cc410967";

            repo = new Repository(dbContext);
            profileService = new ProfileService(userManager, repo);

            Random rnd = new Random();
            byte[] data = new byte[5 * 1024];
            rnd.NextBytes(data);

            await profileService.UploadPictureAsync(data, user2Id);

            Assert.That((await userManager.FindByIdAsync(user2Id)).ProfilePicture, Is.EqualTo(data));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
