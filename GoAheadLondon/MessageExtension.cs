using System;
using System.Linq;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;

namespace Microsoft.Teams.Samples.HelloWorld.Web
{
    public class MessageExtension
    {
        public static ComposeExtensionResponse HandleMessageExtensionQuery(ConnectorClient connector, Activity activity)
        {
            var query = activity.GetComposeExtensionQueryData();
            if (query == null || query.CommandId != "getRandomText")
            {
                // We only process the 'getRandomText' queries with this message extension
                return null;
            }

            var title = "";
            var titleParam = query.Parameters?.FirstOrDefault(p => p.Name == "cardTitle");
            if (titleParam != null)
            {
                title = titleParam.Value.ToString();
            }

            var attachments = new ComposeExtensionAttachment[5];
            for (int i = 0; i < 5; i++)
            {
                attachments[i] = GetAttachment(title);
            }

            var response = new ComposeExtensionResponse(new ComposeExtensionResult("list", "result"));
            response.ComposeExtension.Attachments = attachments.ToList();

            return response;
        }

        private static ComposeExtensionAttachment GetAttachment(string title = null)
        {
            var card = new ThumbnailCard
            {
                Title = !string.IsNullOrWhiteSpace(title) ? title : Faker.Lorem.Sentence(),
                Text = Faker.Lorem.Paragraph(),
               Images = new System.Collections.Generic.List<CardImage> { new CardImage("http://lorempixel.com/640/480?rand=" + DateTime.Now.Ticks.ToString()) }
            };
            
            return card
                .ToAttachment()
                .ToComposeExtensionAttachment();
        }
    }
}
