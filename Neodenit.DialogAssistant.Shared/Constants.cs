using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Shared
{
    public static class Constants
    {
        public const double ApproximateTokenLength = 4;

        public const string MessageSeparator = "\n";

        public const string LengthFinishReason = "length";

        public const string Ellipsis = "...";

        public const string FakeUser = "User3";

        public static readonly string[] StopSequences1 = { "\n", ".", "?", "!" };

        public static readonly string[] StopSequences2 = { $"{nameof(Dialog.User1)}:", $"{nameof(Dialog.User2)}:", FakeUser };

        public static readonly string[] SentenceEndings = { ".", "?", "!" };
    }
}
