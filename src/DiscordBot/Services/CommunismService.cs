using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Discord.Commands;
using DiscordBot.Modules;

namespace DiscordBot.Services
{
    public class CommunismService
    {
        private readonly DiscordSocketClient _client;

        public CommunismService(DiscordSocketClient client)
        {
            _client = client;
            _client.MessageReceived += CommunismMessage;
            _client.MessageReceived += CapitalistMessage;
        }

        private async Task CommunismMessage(SocketMessage msg)
        {
            if (msg.Content.ToLower().Contains("communis"))
            {
                var emote = (msg.Author as SocketGuildUser).Guild.Emotes.FirstOrDefault(x => x.Name.Contains("theslavsmark"));
                await (msg as SocketUserMessage).AddReactionAsync(emote);
            }
        }

        private async Task CapitalistMessage(SocketMessage arg)
        {
            if (arg.Content.ToLower().Contains("capitalis"))
            {
                if (arg.Content.ToLower().Contains("fuck"))
                {
                    InfoModule n = new InfoModule();
                    await n.SayTTSAsync("FUCK CAPITALISM");
                }
                else
                {
                    Emoji gEmote = new Emoji("🇬");
                    Emoji uEmote = new Emoji("🇺");
                    Emoji lEmote = new Emoji("🇱");
                    Emoji aEmote = new Emoji("🇦");
                    Emoji g2Emote = new Emoji("🆖");

                    await (arg as SocketUserMessage).AddReactionAsync(gEmote);
                    await (arg as SocketUserMessage).AddReactionAsync(uEmote);
                    await (arg as SocketUserMessage).AddReactionAsync(lEmote);
                    await (arg as SocketUserMessage).AddReactionAsync(aEmote);
                    await (arg as SocketUserMessage).AddReactionAsync(g2Emote);
                }
            }
        }
    }
}
