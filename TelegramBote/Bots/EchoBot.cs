using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TelegramBote.Model;

namespace TelegramBote.Bots
{
    public class EchoBot : ActivityHandler
    {
        private BotState _conversationState;
        private BotState _userState;

        public EchoBot(ConversationState conversationState, UserState userState)
        {
            _conversationState = conversationState;
            _userState = userState;
        }

        /// <summary>
        /// вызывается каждый раз, когда принимаются данные от нового пользователя
        /// </summary>
        /// <param name="turnContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task OnMessageActivityAsync(
            ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var conversationStateAccessors = _conversationState.CreateProperty<ConversationData>(nameof(ConversationData));
            ConversationData conversationData = await conversationStateAccessors.GetAsync(turnContext, () => new ConversationData());

            var userStateAccessors = _userState.CreateProperty<UserProfile>(nameof(UserProfile));
            UserProfile userProfile = await userStateAccessors.GetAsync(turnContext, () => new UserProfile());

            // генерация ответа для пользователя
            await ProcessInput(turnContext, userProfile, conversationData, cancellationToken);

            await _conversationState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
            await _userState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// вызывается каждый раз, когда к боту подключается новый пользователь
        /// </summary>
        /// <param name="membersAdded"></param>
        /// <param name="turnContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task OnMembersAddedAsync(
            IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var userStateAccessors = _userState.CreateProperty<UserProfile>(nameof(UserProfile));
            UserProfile userProfile = await userStateAccessors.GetAsync(turnContext, () => new UserProfile());

            if (!userProfile.DidBotWelcomeUser)
            {
                userProfile.DidBotWelcomeUser = true;
                await SendWelcomeMessageAsync(turnContext, cancellationToken);
            }

            await _userState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

        private static async Task SendWelcomeMessageAsync(
            ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    #region old generated

                    //var welcomeText = $"Hello and welcome, {member.Name}";
                    //IMessageActivity reply = MessageFactory.Text(welcomeText);
                    //await turnContext.SendActivityAsync(reply, cancellationToken);
                    //await SendSuggestedActionsAsync(turnContext, cancellationToken);

                    #endregion

                    await SendIntroCardAsync(turnContext, cancellationToken);
                }
            }
        }

        /// <summary>
        /// обработка сообщений от пользователя
        /// </summary>
        /// <param name="turnContext"></param>
        /// <param name="conversationData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static async Task ProcessInput(
            ITurnContext turnContext, UserProfile userProfile, ConversationData conversationData, CancellationToken cancellationToken)
        {
            IMessageActivity reply = null;
            bool sendButton = true;
            var text = turnContext.Activity.Text?.ToLowerInvariant() ?? "none";

            if (!userProfile.DidBotWelcomeUser && text.Equals("/start"))
            {
                sendButton = false;
                userProfile.DidBotWelcomeUser = true;
                await SendIntroCardAsync(turnContext, cancellationToken);
            }
            else
            {
                if (conversationData.LastQuestionAsked == UserAction.Picture)
                {
                    conversationData.LastQuestionAsked = UserAction.None;
                    await SendInternetAttachment(text, turnContext, cancellationToken);
                }
                else
                {
                    switch (text)
                    {
                        case "сгенерировать число":
                            {
                                conversationData.LastQuestionAsked = UserAction.None;
                                var rand = new Random();
                                reply = MessageFactory.Text($"{rand.Next(1, 100)}");
                                break;
                            }
                        case "дай картинку":
                            {
                                sendButton = false;
                                conversationData.LastQuestionAsked = UserAction.Picture;
                                reply = MessageFactory.Text($"Введите описание для картинки");
                                break;
                            }
                        case "help":
                            {
                                sendButton = false;
                                conversationData.LastQuestionAsked = UserAction.None;
                                await SendIntroCardAsync(turnContext, cancellationToken);
                                break;
                            }
                        case "красный":
                            {
                                conversationData.LastQuestionAsked = UserAction.None;
                                reply = MessageFactory.Text($"Красный - прекрасный цвет");
                                break;
                            }
                        //case "/start":
                        //    {
                        //        sendButton = false;
                        //        userProfile.DidBotWelcomeUser = true;
                        //        await SendIntroCardAsync(turnContext, cancellationToken);
                        //        break;
                        //    }
                        default:
                            {
                                conversationData.LastQuestionAsked = UserAction.None;
                                reply = MessageFactory.Text("Некорректная команда");
                                break;
                            }
                    }
                }
            }

            if (reply != null)
                await turnContext.SendActivityAsync(reply, cancellationToken);
            if (sendButton)
                await SendSuggestedActionsAsync(turnContext, cancellationToken);
        }

        /// <summary>
        /// отправка пользователю кнопок действий
        /// </summary>
        /// <param name="turnContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static async Task SendSuggestedActionsAsync(
            ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var reply = MessageFactory.Text("Что прикажешь, хозяин?");

            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction() {
                        Title = "Сгенерировать число",
                        Type = ActionTypes.ImBack,
                        Value = "Сгенерировать число",
                    },
                     new CardAction() {
                        Title = "Дай картинку",
                        Type = ActionTypes.ImBack,
                        Value = "Дай картинку",
                    },
                    new CardAction() {
                        Title = "Красный",
                        Type = ActionTypes.ImBack,
                        Value = "Красный",
                        Image = "https://via.placeholder.com/20/FF0000?text=R",
                        ImageAltText = "R"
                    },
                     new CardAction() {
                        Title = "help",
                        Type = ActionTypes.ImBack,
                        Value = "help",
                    },

                },
            };
            await turnContext.SendActivityAsync(reply, cancellationToken);
        }

        /// <summary>
        /// получение вложения для сообщения с картинкой из интернета
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        private static async Task SendInternetAttachment(
            string searchTerm, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var subscriptionKey = "1d516d8adb0f483cb367e1cc87683a81";

            var uriQuery = "https://api.bing.microsoft.com/v7.0/images/search" + "?q=" + Uri.EscapeDataString(searchTerm);

            WebRequest request = WebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = subscriptionKey;
            HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            var firstJsonObj = jsonObj["value"][1];
            var urlToViewObj = firstJsonObj["contentUrl"];
            var urlToView = urlToViewObj.ToString();

            var imageAttachment = new Attachment
            {
                Name = @"architecture-resize.png",
                ContentType = "image/jpeg",
                ContentUrl = urlToView,
            };

            IMessageActivity reply = MessageFactory.Attachment(imageAttachment);
            await turnContext.SendActivityAsync(reply, cancellationToken);

            //IMessageActivity reply = MessageFactory.Text("Картинка с интернета:");
            //reply.Attachments = new List<Attachment>() { imageAttachment };
            //await turnContext.SendActivityAsync(reply, cancellationToken);
        }

        /// <summary>
        /// отправка пользователю приветственного сообщения
        /// </summary>
        /// <param name="turnContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static async Task SendIntroCardAsync(
            ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var imagePath = Path.Combine(Environment.CurrentDirectory, @"Resources", "ChatBot-BotFramework.png");
            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            var card = new HeroCard
            {
                Title = "Вас приветствует бот",
                Text = @"Эксперементальный бот, напсианный на bot fraemwork от microsoft. Что умею делать:",
                Images = new List<CardImage>() {
                   // new CardImage($"data:image/png;base64,{imageData}")
                   new CardImage("https://dvlup.tech/wp-content/uploads/2019/03/ChatBot-BotFramework.png")
                },
                Buttons = new List<CardAction>()
                {
                     new CardAction() {
                        Title = "Сгенерировать число",
                        Type = ActionTypes.ImBack,
                        Value = "Сгенерировать число",
                    },
                     new CardAction() {
                        Title = "Дай картинку",
                        Type = ActionTypes.ImBack,
                        Value = "Дай картинку",
                    },
                    new CardAction() {
                        Title = "Красный",
                        Type = ActionTypes.ImBack,
                        Value = "Красный",
                        Image = "https://via.placeholder.com/20/FF0000?text=R",
                        ImageAltText = "R"
                    },
                    new CardAction()
                    {
                        Type = ActionTypes.OpenUrl,
                        Title = "Автор",
                        Value = "https://github.com/PKkDev"
                    }
                }
            };

            Attachment helpAttachment = card.ToAttachment();
            IMessageActivity reply = MessageFactory.Attachment(helpAttachment);
            await turnContext.SendActivityAsync(reply, cancellationToken);
        }
    }
}
