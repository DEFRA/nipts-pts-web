using Defra.PTS.Web.UI.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Xunit;

namespace Defra.PTS.Web.UI.UnitTests.Extensions;

public class ViewDataExtensionsTests
{
    [Fact]
    public void SetKeyValue_ViewData()
    {
        // Arrange
        var viewData = GetViewData();

        // Act
        viewData.SetKeyValue("TestKey", "TestValue");
        var value = viewData["TestKey"] as string;
        
        // Assert
        value.Should().Be("TestValue");
    }


    [Fact]
    public void SetKeyValue_ViewDataEx()
    {
        // Arrange
        var viewData = GetViewDataEx();

        // Act
        viewData.SetKeyValue("TestKey", "TestValue");
        var value = viewData.GetKeyValue("TestKey");

        // Assert
        value.Should().Be("TestValue");
    }


    private static ViewDataDictionary GetViewData()
    {
        var modelState = new ModelStateDictionary();
        var modelMetadataProvider = new EmptyModelMetadataProvider();
        return new ViewDataDictionary(modelMetadataProvider, modelState);
    }

    private static ViewDataDictionary<dynamic> GetViewDataEx()
    {
        var modelState = new ModelStateDictionary();
        var modelMetadataProvider = new EmptyModelMetadataProvider();
        return new ViewDataDictionary<dynamic>(modelMetadataProvider, modelState);
    }

}
