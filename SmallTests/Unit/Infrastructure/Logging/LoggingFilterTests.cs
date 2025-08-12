namespace Tests.Unit.Infrastructure
{
    public class LoggingFilterTests
    {
        //[Fact]
        //public async Task OnActionExecutionAsync_ShouldLogRequestAndResponse()
        //{
        //    var mockLogger = new Mock<IStructuredLogger>();
        //    var filter = new LoggingFilter(mockLogger.Object);

        //    var context = new ActionExecutingContext(
        //        new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
        //        new List<IFilterMetadata>(), new Dictionary<string, object>(), new object());

        //    var executedContext = new ActionExecutedContext(context, new List<IFilterMetadata>(), new object());

        //    await filter.OnActionExecutionAsync(context, () => Task.FromResult(executedContext));

        //    mockLogger.Verify(l => l.Info(It.IsAny<string>(), It.IsAny<object[]>()), Times.Exactly(2));
        //}
    }
}
