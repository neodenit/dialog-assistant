using Neodenit.DialogAssistant.Shared.Models;

namespace Neodenit.DialogAssistant.Shared
{
    public static class Constants
    {
        public const double ApproximateTokenLength = 4;

        public const string MessageSeparator = "\n";

        public const string SenderPlaceholder = "Me";

        public const string ReceiverPlaceholder = "You";

        public const string LengthFinishReason = "length";

        public const string Ellipsis = "...";

        public static readonly string[] StopSequences1 = { "\n", ".", "?", "!" };

        public static readonly string[] StopSequences2 = { $"{SenderPlaceholder}:", $"{ReceiverPlaceholder}:" };

        public static readonly string[] SentenceEndings = { ".", "?", "!" };
    }
}
