using AutoFixture;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FakeItEasy;
using GameStore.BL.Enums;
using GameStore.BL.Services;
using GameStore.WEB.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GameStore.UnitTests.BL.Services
{
    public class CloudinaryServiceTests
    {

        [Fact]
        public async Task ShouldReturnUploadResultFromUploadAsync()
        {
            //Arrange
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior()}
            };


            var settings = fixture.Create<AppSettings>();
            var cloudinaryService = new CloudinaryService(settings);

            var filename = fixture.Create<string>();
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(filename);
            writer.Flush();
            stream.Position = 0;

            var file = new FormFile(stream, 0, 0, filename, filename + ".jpg");

            //Act
            var result = await cloudinaryService.UploadAsync(file);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);
        }

        [Fact]
        public async Task ShouldReturnInternalErrorOnWrongExtensionFromUploadAsync()
        {
            //Arrange
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };


            var settings = fixture.Create<AppSettings>();
            var cloudinaryService = new CloudinaryService(settings);

            var filename = fixture.Create<string>();
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(filename);
            writer.Flush();
            stream.Position = 0;

            var file = new FormFile(stream, 0, 0, filename, filename + ".");

            //Act
            var result = await cloudinaryService.UploadAsync(file);

            //Assert
            Assert.Equal(ServiceResultType.InvalidData, result.Result);
        }
    }
}
