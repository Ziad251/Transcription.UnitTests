using Xunit;
using transcription_project.WebApp.Models;
using transcription_project.WebApp.Controllers;
using transcription_project.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using System;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace transcription_project.UnitTests;
public class HomeControllerTest
{
    [Fact]
    public async Task ReturnsAViewResult_WithAnArrayOfContainerNamesOfUser()
    {
        // Arrange
        var mockRepo = new Mock<IRepositoryService>();
        var user = GetTestUser();
        mockRepo.Setup(repo => repo.GetAllContainerNames(user))
        .ReturnsAsync(GetTestContainerNames());
        var homeController = new HomeController(GetTestClaims(), mockRepo.Object);

        // Act
        var result = await homeController.GetAllContainers();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        string json = Assert.IsAssignableFrom<string>(
            viewResult.ViewData["Model"]);

            string[] arrayOfNames = JsonSerializer.Deserialize<string[]>(json);
            Assert.Equal(new string[]{"First","Second","Third"}, arrayOfNames);
        
    }

    public virtual IGetClaimsProvider GetTestClaims()
    {
        var mockAccessor = new Mock<IHttpContextAccessor>();
        GetClaimsFromUser claims = new(mockAccessor.Object)
        {
            Id = Guid.NewGuid().ToString(),
            Username = "1111111111111",
            Passcode = "aaaaaaaaaaaaa",
        };

        return claims;
    }

    public virtual UserData GetTestUser()
    {
        UserData user = new()
        {
            id = Guid.NewGuid(),
            Username = "1111111111111",
            Password = "aaaaaaaaaaaaa",
            Containers = null
        };
        return user;
    }

    public virtual string[] GetTestContainerNames()
    {
        string[] containerNames = new string[] { "First", "Second", "Third" };
        return containerNames;
    }
}