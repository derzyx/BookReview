using Autofac.Extras.Moq;
using BookReview.ClassLibrary;
using BookReview.Controlllers;
using BookReview.Data;
using BookReview.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Xunit;

namespace BookReview.Tests
{
    public class AccountTests
    {


        DataContext dataContext = MoqUtilities.MockDbContext(users: Users(), reviews: null).DbContext.Object;

        [Fact]
        public void Login_CorrectValues()
        {
            var accountController = new AccountController(dataContext)
            var expected = 

        }

        private static List<User> Users()
        {
            List<User> users = new List<User>
            {
                new User
                {
                    Email = "tom23@gmail.com",
                    Password = "qwertyuiop"
                },
                new User
                {
                    Email = "adm@gmail.com",
                    Password = "zielony1"
                }
            };
            return users;
        }

        
    }
}