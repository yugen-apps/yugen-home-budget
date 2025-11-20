using Blazorise;

namespace Yugen.HomeBudget.Server.Models
{
    public class ForwardRef
    {
        private Bar? _current;

        public Bar? Current
        {
            get => _current;
            set => Set(value);
        }

        public void Set(Bar? value)
        {
            _current = value;
        }
    }
}
