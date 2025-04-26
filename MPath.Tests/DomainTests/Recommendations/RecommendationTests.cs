using MPath.Domain.Entities;

namespace MPath.Tests.DomainTests.Recommendations;

public class RecommendationTests
{
    [Fact]
    public void Create_Should_CreateRecommendationSuccessfully()
    {
        // Arrange
        var title = "Exercise Regularly";
        var content = "Perform at least 30 minutes of exercise daily.";
        var isCompleted = false;
        var createdByUserId = Guid.NewGuid();
        
        // Act
        var recommendation = Recommendation.Create(title, content, isCompleted, createdByUserId);
        
        // Assert
        Assert.NotNull(recommendation);
        Assert.Equal(title, recommendation.Title);
        Assert.Equal(content, recommendation.Content);
        Assert.Equal(isCompleted, recommendation.IsCompleted);
        Assert.Equal(createdByUserId, recommendation.CreatedByUserId);
    }
    
    [Fact]
    public void MarkCompleted_Should_SetIsCompletedToTrue()
    {
        // Arrange
        var recommendation = Recommendation.Create("Exercise", "Do daily workouts", false, Guid.NewGuid());
        
        // Act
        recommendation.MarkCompleted();
        
        // Assert
        Assert.True(recommendation.IsCompleted);
    }
}