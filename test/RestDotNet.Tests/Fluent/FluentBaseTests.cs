using Moq;

namespace RestDotNet.Tests.FluentTests
{
    public abstract class FluentBaseTests
    {
        protected FluentBaseTests()
        {
            HandlerMock = new Mock<IRestHandler>();
            UntypedMock = new Mock<IRestRequest>();
            UntypedMock.Setup(request => request.Handler)
                .Returns(HandlerMock.Object);

            TypedMock = new Mock<IRestRequest<object>>();
            TypedMock.Setup(request => request.Handler)
                .Returns(HandlerMock.Object);
        }

        public Mock<IRestHandler> HandlerMock { get; }

        public Mock<IRestRequest> UntypedMock { get; }

        public Mock<IRestRequest<object>> TypedMock { get; }

        public abstract void Typed_Response_With_Content_Register_Callback();

        public abstract void Typed_Response_With_Content_Return_The_Same();

        public abstract void Typed_Response_Without_Content_Register_Callback();

        public abstract void Typed_Response_Without_Content_Return_The_Same();

        public abstract void Untyped_Response_With_Content_Register_Callback();

        public abstract void Untyped_Response_With_Content_Return_The_Same();

        public abstract void Untyped_Response_Without_Content_Register_Callback();

        public abstract void Untyped_Response_Without_Content_Return_The_Same();
    }
}