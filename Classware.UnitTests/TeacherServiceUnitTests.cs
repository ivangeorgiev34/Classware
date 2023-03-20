using Classware.Core.Contracts;
using Classware.Core.Services;
using Classware.Infrastructure.Common;
using Classware.Infrastructure.Data;
using Classware.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.UnitTests
{
    [TestFixture]
    public class TeacherServiceUnitTests
    {
        private IRepository repo;
        private ApplicationDbContext dbContext;
        private UserManager<ApplicationUser> userManager;
        private ITeacherService teacherService;

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
        public async Task Test_MethodAddTeacherShouldWorkProperly()
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

            repo = new Repository(dbContext);
            teacherService = new TeacherService(repo, userManager);

            var teacher = new Teacher()
            {
                Id = 1,
                UserId = "3b1a9553-091d-4972-83a2-dc2280c57c3e"
            };

            await teacherService.AddTeacher(teacher);

            var result = (await repo.All<Teacher>()
                .Where(t => t.IsActive)
                .ToListAsync()).Count;

            var expectedTeachersCount = 1;

            Assert.That(result, Is.EqualTo(expectedTeachersCount));
        }

        [Test]
        public async Task Test_MethodAssignSubjectToTeacherAsyncShouldWorkProperly()
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

            repo = new Repository(dbContext);
            teacherService = new TeacherService(repo, userManager);

            var teacher = new Teacher()
            {
                Id = 1,
                UserId = "3b1a9553-091d-4972-83a2-dc2280c57c3e"
            };

            var teacherId = teacher.Id;

            var subjectId = 1;

            await teacherService.AddTeacher(teacher);

            await teacherService.AssignSubjectToTeacherAsync(teacherId, subjectId);

            var result = (await repo.All<Teacher>().FirstOrDefaultAsync(t => t.Id == teacherId))?.SubjectId;

            Assert.That(result, Is.EqualTo(subjectId));
        }

        [Test]
        public async Task Test_MethodAssignSubjectToTeacherAsyncShouldThrowExceptionWhenCannotFindTeacher()
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

            repo = new Repository(dbContext);
            teacherService = new TeacherService(repo, userManager);

            var teacher = new Teacher()
            {
                Id = 1,
                UserId = "3b1a9553-091d-4972-83a2-dc2280c57c3e",
                IsActive=false
            };

            var teacherId = teacher.Id;

            var subjectId = 1;

            await teacherService.AddTeacher(teacher);

            Assert.ThrowsAsync<InvalidOperationException>(async () => await teacherService.AssignSubjectToTeacherAsync(teacherId, subjectId));

        }

        [Test]
        public async Task Test_MethodDeleteTeacherByIdAsyncShouldWorkProperly()
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

            repo = new Repository(dbContext);
            teacherService = new TeacherService(repo, userManager);

            var teacher = new Teacher()
            {
                Id = 1,
                UserId = "3b1a9553-091d-4972-83a2-dc2280c57c3e"
            };

            var teacherId = teacher.Id;

            await teacherService.AddTeacher(teacher);

            await teacherService.DeleteTeacherByIdAsync(teacherId);

            var result = (await repo.All<Teacher>().FirstOrDefaultAsync(t => t.Id == teacherId))?.IsActive;

            Assert.That(result, Is.False);    
        }

        [Test]
        public async Task Test_MethodGetAllTeachersAsyncShouldWorkProperly()
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

            repo = new Repository(dbContext);
            teacherService = new TeacherService(repo, userManager);

            var teacher = new Teacher()
            {
                Id = 1,
                UserId = "3b1a9553-091d-4972-83a2-dc2280c57c3e"
            };

            await teacherService.AddTeacher(teacher);

            var result = (await teacherService.GetAllTeachersAsync()).Count();

            var expectedResult = 1;

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task Test_MethodGetTeacherByIdAsyncShouldWorkProperly()
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

            repo = new Repository(dbContext);
            teacherService = new TeacherService(repo, userManager);

            var teacher = new Teacher()
            {
                Id = 1,
                UserId = "3b1a9553-091d-4972-83a2-dc2280c57c3e"
            };

            await teacherService.AddTeacher(teacher);

            var teacherId = teacher.Id;

            var result = (await teacherService.GetTeacherByIdAsync(teacherId))?.UserId;

            var expectedResult = "3b1a9553-091d-4972-83a2-dc2280c57c3e";

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task Test_MethodGetTeacherByIdAsyncShouldThrowExceptionWhenCannotFindTeacher()
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

            repo = new Repository(dbContext);
            teacherService = new TeacherService(repo, userManager);

            var teacher = new Teacher()
            {
                Id = 1,
                UserId = "3b1a9553-091d-4972-83a2-dc2280c57c3e"
            };

            await teacherService.AddTeacher(teacher);

            await teacherService.DeleteTeacherByIdAsync(teacher.Id);

            Assert.ThrowsAsync<InvalidOperationException>(async () => await teacherService.GetTeacherByIdAsync(teacher.Id));
        }

        [Test]
        public async Task Test_MethodGetTeacherByUserIdAsyncShouldWorkProperly()
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

            repo = new Repository(dbContext);
            teacherService = new TeacherService(repo, userManager);

            var teacher = new Teacher()
            {
                Id = 1,
                UserId = "3b1a9553-091d-4972-83a2-dc2280c57c3e"
            };

            await teacherService.AddTeacher(teacher);

            var teacherUserId = teacher.UserId;

            var result = (await teacherService.GetTeacherByUserIdAsync(teacherUserId)).Id;

            var expectedResult = 1;

            Assert.That(result,Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task Test_MethodTeacherHasASubjectAsyncShouldReturnTrueWhenTeacherHasSubject()
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

            repo = new Repository(dbContext);
            teacherService = new TeacherService(repo, userManager);

            var teacher = new Teacher()
            {
                Id = 1,
                UserId = "3b1a9553-091d-4972-83a2-dc2280c57c3e"
            };

            await teacherService.AddTeacher(teacher);

            await teacherService.AssignSubjectToTeacherAsync(teacher.Id, 1);

            var teacherUserId = teacher.UserId;

            var result = await teacherService.TeacherHasASubjectAsync(teacherUserId);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task Test_MethodTeacherHasASubjectAsyncShouldReturnFalseWhenTeacherDoesntHaveSubject()
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

            repo = new Repository(dbContext);
            teacherService = new TeacherService(repo, userManager);

            var teacher = new Teacher()
            {
                Id = 1,
                UserId = "3b1a9553-091d-4972-83a2-dc2280c57c3e"
            };

            await teacherService.AddTeacher(teacher);

            var teacherUserId = teacher.UserId;

            var result = await teacherService.TeacherHasASubjectAsync(teacherUserId);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task Test_MethodGetTeacherByUserIdAsyncShouldThrowExceptionWhenCannotFindTeacher()
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

            repo = new Repository(dbContext);
            teacherService = new TeacherService(repo, userManager);

            var teacher = new Teacher()
            {
                Id = 1,
                UserId = "3b1a9553-091d-4972-83a2-dc2280c57c3e"
            };

            await teacherService.AddTeacher(teacher);

            await teacherService.DeleteTeacherByIdAsync(teacher.Id);

            Assert.ThrowsAsync<NullReferenceException>(async () => await teacherService.GetTeacherByUserIdAsync(teacher.UserId));
        }
        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }
    }
}
