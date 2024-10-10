
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.Data.Entities;
using School.Helpers;

namespace School.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Student");
            await _userHelper.CheckRoleAsync("Employee");
            await _userHelper.CheckRoleAsync("Teacher");

            var user = await _userHelper.GetUserByEmailAsync("adm.projectotorres@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Admin",
                    LastName = "Principal",
                    DateBirth = new DateTime(1993, 01, 05),
                    Email = "adm.projectotorres@gmail.com",
                    UserName = "adm.projectotorres@gmail.com",
                    PhoneNumber = "123456789",
                    Address = "Rua Jau 33",
                    PostalCode = "2700-700",
                    Nif = "123321213"
                };
                var result = await _userHelper.AddUserWithPasswordAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }


            var result1 = await AddTeachersAsync("mariajoaquina@yopmail.com", "Maria", "Joaquina");
            var result2 = await AddTeachersAsync("josefernandes@yopmail.com", "Jose", "Fernandes");
            var result3 = await AddTeachersAsync("paulofreire@yopmail.com", "Paulo", "Freire");
            var result4 = await AddTeachersAsync("minerva@yopmail.com", "Minerva", "McGonagall");
            var result5 = await AddStudentsAsync("anakinsky@yopmail.com", "Anakin", "Skywalker");            
            var result6 = await AddStudentsAsync("brucewayne@yopmail.com", "Bruce", "Wayne");
            var result7 = await AddStudentsAsync("dianaprince@yopmail.com", "Diana", "Prince");
            var result8 = await AddStudentsAsync("leiaprincess@yopmail.com", "Leia", "Organa");
            var result9 = await AddStudentsAsync("peterparker@yopmail.com", "Peter", "Parker");


            if (!_context.Subjects.Any())
            {
                AddSubject("Português", "UFDC 5062", 50);
                AddSubject("Matemática", "UFDC 5064", 50);
                AddSubject("Língua Inglesa", "UFDC 5063", 50);
                AddSubject("Física e Química", "UFDC 8886", 25);

                await _context.SaveChangesAsync();
            }
        }

        private void AddSubject(string name, string description, int workload)
        {
            _context.Subjects.Add(new Subject
            {
                Name = name,
                Description = description,
                Workload = workload
            });
        }

        private async Task<bool> AddTeachersAsync(string userName, string firstName, string lastName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DateBirth = new DateTime(1990, 03, 10),
                    Email = userName,
                    UserName = userName,
                    PhoneNumber = "123456789",
                    Address = "Rua Jau 33",
                    PostalCode = "2700-700",
                    Nif = "123321213"
                };
                var result = await _userHelper.AddUserWithPasswordAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Teacher");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
                return true;
            }
            return false;
        }

        private async Task<bool> AddStudentsAsync(string userName, string firstName, string lastName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DateBirth = new DateTime(1990, 03, 10),
                    Email = userName,
                    UserName = userName,
                    PhoneNumber = "123456789",
                    Address = "Rua Jau 33",
                    PostalCode = "2700-700",
                    Nif = "123321213"
                };
                var result = await _userHelper.AddUserWithPasswordAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Student");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
                return true;
            }
            return false;
        }
    }
}
