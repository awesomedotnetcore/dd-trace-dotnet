using Datadog.Trace.Logging;

namespace Datadog.Trace
{
    internal class AsyncLocalScopeManager
    {
        private static readonly ILog Log = LogProvider.For<AsyncLocalScopeManager>();

        private readonly AsyncLocalCompat<Scope> _activeScope = new AsyncLocalCompat<Scope>();

        public Scope Active => _activeScope.Get();

        public Scope Activate(Span span, bool finishOnClose = true)
        {
            var activeScope = _activeScope.Get();
            var scope = new Scope(activeScope, span, this, finishOnClose);
            _activeScope.Set(scope);
            return scope;
        }

        public void Close(Scope scope)
        {
            var current = _activeScope.Get();

            if (scope == current)
            {
                _activeScope.Set(current.Parent);
            }
        }
    }
}
