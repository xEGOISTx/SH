using SH.Core;

namespace SH.DataRepository
{
    internal class OperationResult : IOperationResult
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}
