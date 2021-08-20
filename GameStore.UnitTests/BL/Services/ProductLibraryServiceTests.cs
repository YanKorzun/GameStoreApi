using FakeItEasy;
using GameStore.BL.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GameStore.UnitTests.BL.Services
{
    public class ProductLibraryServiceTests
    {

        [Fact]
        public async Task ShouldBeCalledOnce()
        {
            //Arrange
            var libraryRepository = A.Fake<IProductLibraryRepository>();

            var libService = new ProductLibraryService(libraryRepository);

            var library = new List<ProductLibraries> { new ProductLibraries(1, 2) };

            A.CallTo(() => libraryRepository.CreateItemsAsync(A<IEnumerable<ProductLibraries>>._));

            //Act
            await libService.AddItemsToLibraryAsync(library);

            //Assert
            A.CallTo(() => libraryRepository.CreateItemsAsync(A<IEnumerable<ProductLibraries>>._)).MustHaveHappenedOnceExactly();
        }
    }
}
