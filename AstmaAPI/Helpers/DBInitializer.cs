using AstmaAPI.EF;
using AstmaAPI.Models.DBO;
using System;
using System.Linq;

namespace AstmaAPI.Helpers
{
    public class DbInitializer
    {
        public static void Initialize(MainContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            User user = GetDefaultUser();

            context.Users.Add(user);

            context.SaveChanges();
        }

        private static User GetDefaultUser()
        {
            return new User
            {
                Login = "test@test.test",
                Name = "Test User",
                Password = "Test123",
                BirthDate = DateTime.Now,
                Height = 180,
                Sex = Models.API.Common.SexEnum.Man,
                Surname = "Kizchkova",
                Weight = 120
            };
        }
    }
}
