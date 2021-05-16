namespace TelegramBote.Model
{
    public enum UserAction
    {
        Color,
        Picture,
        ValueGenerator,
        None,
    }

    public class ConversationData
    {
        public UserAction LastQuestionAsked { get; set; } = UserAction.None;
    }
}
