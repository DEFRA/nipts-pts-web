using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.ViewModels;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using static Defra.PTS.Web.UI.Constants.WebAppConstants;

namespace Defra.PTS.Web.UI.UnitTests.Extensions;

public class TempDataExtensionsTests
{
    private readonly ITempDataDictionary _tempData;
    public TempDataExtensionsTests()
    {
        var mockTempData = new Mock<ITempDataDictionary>();

        mockTempData.Setup(t => t.ContainsKey("key1"))
            .Returns(true);

        object value1 = "value1";

        mockTempData.Setup(t => t["key1"])
            .Returns(value1);

        mockTempData.Setup(t => t.TryGetValue("key1", out value1))
            .Returns(true);



        _tempData = mockTempData.Object;
    }

    [Fact]
    public void TempData_GetTravelDocument_ReturnsNull()
    {
        // Arrange
        var tempData = TempData();

        // Act
        var result = tempData.GetTravelDocument(createIfNull: false);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TempData_GetTravelDocument_ReturnsObject()
    {
        // Arrange
        var tempData = TempData();

        // Act
        var result = tempData.GetTravelDocument(createIfNull: true);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void TempData_SetTravelDocument()
    {
        // Arrange
        var model = new TravelDocumentViewModel
        {
            RequestId = Guid.NewGuid()
        };

        var tempData = TempData();

        // Act
        tempData.SetTravelDocument(model);
        var result = tempData.GetTravelDocument();

        // Assert
        result.RequestId.Should().Be(model.RequestId);
    }

    [Fact]
    public void TempData_RemoveTravelDocument_ReturnsNull()
    {
        // Arrange
        var tempData = TempData();

        // Act
        tempData.RemoveTravelDocument();
        var result = tempData.GetTravelDocument(createIfNull: false);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TempData_GetFromFormSubmissionQueue_ReturnsObject()
    {
        // Arrange
        var tempData = TempData();

        // Act
        var result = tempData.GetFormSubmissionQueue();

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void TempData_ClearFormSubmissionQueue_ReturnsNull()
    {
        // Arrange
        var tempData = TempData();
        var id = Guid.NewGuid();
        // Act
        tempData.AddToFormSubmissionQueue(id);
        tempData.ClearFormSubmissionQueue();
        var result = tempData.GetFormSubmissionQueue();

        // Assert
        result.Should().BeNullOrEmpty();
    }

    [Fact]
    public void TempData_AddToFormSubmissionQueue_ReturnsObject()
    {
        // Arrange
        var tempData = TempData();
        var id  = Guid.NewGuid();
        // Act
        tempData.AddToFormSubmissionQueue(id);
        var result = tempData.GetFormSubmissionQueue();

        // Assert
        result.Count.Should().Be(1);
    }

    [Fact]
    public void TempData_IsInFormSubmissionQueue_ReturnsObject()
    {
        // Arrange
        var tempData = TempData();
        var id = Guid.NewGuid();
        // Act
        tempData.AddToFormSubmissionQueue(id);
        var result = tempData.IsInFormSubmissionQueue(id);
        var resultX = tempData.IsInFormSubmissionQueue(Guid.NewGuid());

        // Assert
        result.Should().Be(true);
        resultX.Should().Be(false);
    }

    [Fact]
    public void TempData_RemoveFromFormSubmissionQueue_ReturnsObject()
    {
        // Arrange
        var tempData = TempData();
        var id = Guid.NewGuid();
        // Act
        tempData.AddToFormSubmissionQueue(id);
        tempData.RemoveFromFormSubmissionQueue(id);
        var result = tempData.IsInFormSubmissionQueue(id);

        // Assert
        result.Should().Be(false);
    }

    [Fact]
    public void TempData_GetMagicWordViewModel_ReturnsNull()
    {
        // Arrange
        var tempData = TempData();

        // Act
        var result = tempData.GetHasUserUsedMagicWord(createIfNull: false);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TempData_GetMagicWordViewModel_ReturnsObject()
    {
        // Arrange
        var tempData = TempData();

        // Act
        var result = tempData.GetHasUserUsedMagicWord(createIfNull: true);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void TempData_SetMagicWordViewModel()
    {
        // Arrange
        var model = new MagicWordViewModel
        {
             HasUserPassedPasswordCheck = true,
        };

        var tempData = TempData();

        // Act
        tempData.SetHasUserUsedMagicWord(model);
        var result = tempData.GetHasUserUsedMagicWord();

        // Assert
        result.HasUserPassedPasswordCheck.Should().Be(model.HasUserPassedPasswordCheck);
    }

    [Fact]
    public void TempData_RemoveMagicWordViewModel_ReturnsNull()
    {
        // Arrange
        var tempData = TempData();

        // Act
        tempData.RemoveHasUserUsedMagicWord();
        var result = tempData.GetHasUserUsedMagicWord(createIfNull: false);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void TempData_Get()
    {
        // Arrange
        var model = new TravelDocumentViewModel
        {
            RequestId = Guid.NewGuid()
        };

        var tempData = TempData();

        // Act
        tempData.SetTravelDocument(model);
        var result = tempData.Get<TravelDocumentViewModel>(TempDataKey.TravelDocumentViewModel);

        // Assert
        result.RequestId.Should().Be(model.RequestId);
    }


    private static ITempDataDictionary TempData()
    {
        var tempDataProvider = Mock.Of<ITempDataProvider>();
        var tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
        var tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

        return tempData;
    }
}
