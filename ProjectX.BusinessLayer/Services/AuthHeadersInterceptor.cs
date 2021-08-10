using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;

namespace ProjectX.BusinessLayer.Services
{

    public class AuthHeadersInterceptor : Interceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthHeadersInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var metadata = new Metadata
            {
                {"Authorization", $"Bearer <JWT_TOKEN>"}
            };
            var userIdentity = _httpContextAccessor.HttpContext.User.Identity;
            if (userIdentity.IsAuthenticated)
            {
                metadata.Add("User", userIdentity.Name);
            }

            var callOption = context.Options.WithHeaders(metadata);
            context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, callOption);

            return base.AsyncUnaryCall(request, context, continuation);
        }
    }
}
