using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;

using UKParliament.CodeTest.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UKParliament.CodeTest.Tests
{
    public class PeopleControllerTests
    {
        [Fact]
        public async Task GetAll_Person_Test()
        {
            await RunInContextAsync(async peopleController =>
            {
                var person1 = GetTestPersonModel();
                var person2 = GetTestPersonModel();

                //Act  
                await peopleController.AddAsync(person1);
                await peopleController.AddAsync(person2);

                var result = await peopleController.GetAllAsync(null, null, null);

                //Assert  
                Assert.Equal(2, ((IEnumerable<PersonModel>)result.Value).Count());
            });
        }

        [Fact]
        public async Task GetById_Person_Test()
        {
            await RunInContextAsync(async peopleController =>
            {
                var personId = 1;
                var person = GetTestPersonModel();

                //Act  
                await peopleController.AddAsync(person);

                var result = await peopleController.GetAsync(personId);

                //Assert  
                Assert.IsType<OkObjectResult>(result);
            });
        }

        [Fact]
        public async Task GetById_Person_Not_Found_Test()
        {
            await RunInContextAsync(async peopleController =>
            {
                var personId = 2;
                var person = GetTestPersonModel();

                //Act  
                await peopleController.AddAsync(person);

                var result = await peopleController.GetAsync(personId);

                //Assert  
                Assert.IsType<NotFoundObjectResult>(result);
            });
        }

        [Fact]
        public async Task Add_Person_Test()
        {
            await RunInContextAsync(async peopleController =>
            {
                var person = GetTestPersonModel();

                //Act  
                var result = await peopleController.AddAsync(person);

                //Assert  
                Assert.IsType<OkObjectResult>(result);
            });
        }

        [Fact]
        public async Task Add_Person_FirstName_Is_Required_Test()
        {
            await RunInContextAsync(async peopleController =>
            {
                var person = GetTestPersonModel();
                person.Name = null;

                //Act  
                var result = await peopleController.AddAsync(person);

                //Assert  
                Assert.IsType<BadRequestObjectResult>(result);
            });
        }

        [Fact]
        public async Task Update_Person_Test()
        {
            await RunInContextAsync(async peopleController =>
            {
                var personId = 1;
                var person = GetTestPersonModel();
                await peopleController.AddAsync(person);

                person.Name = "Updated First Name";

                // Act
                var result = await peopleController.UpdateAsync(personId, person);

                //Assert  
                Assert.IsType<OkObjectResult>(result);
            });
        }

        [Fact]
        public async Task Update_Person_Last_Name_Is_Required_Test()
        {
            await RunInContextAsync(async peopleController =>
            {
                var personId = 1;
                var person = GetTestPersonModel();
                await peopleController.AddAsync(person);

                person.Name = null;

                // Act
                var result = await peopleController.UpdateAsync(personId, person);

                //Assert  
                Assert.IsType<BadRequestObjectResult>(result);
            });
        }

        [Fact]
        public async Task Remove_Person_Test()
        {
            await RunInContextAsync(async peopleController =>
            {
                var personId = 1;
                var person = GetTestPersonModel();
                await peopleController.AddAsync(person);

                // Act
                var result = await peopleController.RemoveAsync(personId);

                //Assert  
                Assert.IsType<OkObjectResult>(result);
            });
        }

        [Fact]
        public async Task Remove_Person_Not_Found_Test()
        {
            await RunInContextAsync(async peopleController =>
            {
                var personId = 2;
                var person = GetTestPersonModel();
                await peopleController.AddAsync(person);

                // Act
                var result = await peopleController.RemoveAsync(personId);

                //Assert  
                Assert.IsType<NotFoundObjectResult>(result);
            });
        }

        // Helpers

        private async Task RunInContextAsync(Func<PeopleController, Task> func)
        {
            var dbOptions = new DbContextOptionsBuilder<RoomBookingsContext>().UseInMemoryDatabase(databaseName: "RoomBookings" + new Random().Next(1, 100000))
                                                                              .Options;

            using (var context = new RoomBookingsContext(dbOptions))
            {
                var _mockPersonRepository = new Repository<Person>(context);
                var _mockPersonService = new PersonService(_mockPersonRepository);
                var _mockPeopleController = new PeopleController(_mockPersonService);

                await func(_mockPeopleController);
            }
        }

        private PersonModel GetTestPersonModel()
        {
            var person = new PersonModel()
            {
                Name = "Test First Name",
                Email = "test@test.com",
                DateOfBirth = new DateTime(1996, 10, 10)
            };

            return person;
        }
    }
}
