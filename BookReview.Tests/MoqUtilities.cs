using BookReview.ClassLibrary;
using BookReview.Data;
using Coderful.EntityFramework.Testing.Mock;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReview.Tests
{
    internal class MoqUtilities
    {
        public static MockedDbContext<DbContext> MockDbContext(
        IList<Review> reviews = null,
        IList<User> users = null)
        {
            var mockContext = new Mock<DbContext>();

            // Create the DbSet objects.
            var dbSets = new object[]
            {
            mockContext.MockDbSet(reviews, (objects, review) => review.ReviewId == (int)objects[0]),
            mockContext.MockDbSet(users, (objects, user) => user.UserId == (int)objects[0])
            };

            return new MockedDbContext<DbContext>(mockContext, dbSets);
        }
    }
}
