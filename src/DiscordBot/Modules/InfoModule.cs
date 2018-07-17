using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task HelpAsync([Optional] string command)
        {
            await Context.Client.SetGameAsync("Gulag Simulator 1943");

            EmbedBuilder builder = new EmbedBuilder();
            builder.AddField("Commands", "help, ping, roll, say, saytts, spam, gulag, bourgeoisie, ungulag, ascend, descend, rachit, pfp, L, leaderboard tag")
                .AddInlineField("Tags", "blyat, cyka, cb, L,  mic, delet, DropMark, gang, smug, drone, c-ops, BoylesLaw, SunGod, darn, tims, deeps, same, LeftToRight, SlavSquad, discrimination, vectors, amigos, accord, linksis, cummies, OurCity, confused, late, dox (or doxx), swiggity, hackerman, hackerman2, knowledge, plug, Dabola")
                .WithColor(Color.DarkRed);

            if (command == null)
                await ReplyAsync($"Привет Комраде, I am {Context.Client.CurrentUser.Username} and here to spread communism.\nTyping `%help <command name>` will give you more information on how to use the specific command.\nHere is a list of my commands:", true, builder.Build());
            else
            {
                command = command.ToLower();
                if (command == "ping")
                    await ReplyAsync("Calculates your latency to discord servers. Correct usage: `%ping`");
                else if (command == "roll")
                    await ReplyAsync("Rolls an n-sided die. Correct usage: `%roll <number of faces on die>`");
                else if (command == "say")
                    await ReplyAsync("I repeat what you say. Correct usage: `%say <text>`");
                else if (command == "saytts")
                    await ReplyAsync("I repeat what you say using text-to-speech. Correct usage: `%saytts <text>`");
                else if (command == "spam")
                    await ReplyAsync("Repeatedly pings a user. Correct usage: `%spam <user> <number of mentions>`");
                else if (command == "gulag")
                    await ReplyAsync("Sends the specified user to the gulag and removes all other ranked roles. Do not ever try to gulag me, a Lorde, or a True Slav. Correct usage: `%gulag <user>`");
                else if (command == "bourgeoisie")
                    await ReplyAsync("Gives the specified user the True Bourgeoisie role and limits their speech to #filthy-propaganda. Correct usage: `%bourgeoisie <user>`");
                else if (command == "ungulag")
                    await ReplyAsync("Removes the specified user from the gulag and gives them back a certain role. Correct usage: `%ungulag <user> <role name>`");
                else if (command == "ascend")
                    await ReplyAsync("Ascends or ultra ascends the specified user. Correct usage: `%ascend <user>`");
                else if (command == "descend")
                    await ReplyAsync("Descends the specified user. Correct usage: `%descend <user>`");
                else if (command == "rachit")
                    await ReplyAsync("Quotes Mr.Don Drone himself. Correct usage: `%rachit`");
                else if (command == "pfp")
                    await ReplyAsync("Gets the profile picture of a specified user. Correct usage: `%pfp <user>`");
                else if (command == "tag")
                    await ReplyAsync("Tags the relevant memes, videos, copypastas, etc. A full list of tags are available with `%help`. Correct usage: `%tag <tag>`");
                else await ReplyAsync("This command either does not exist or is being used as a tag.");
            }  
        }

        #region Commands

        [Command("ping")]
        public async Task PingAsync([Optional] [Remainder] string useless)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var msg = await ReplyAsync("Calculating...");
            watch.Stop();
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle(":ping_pong: Pong!")
                .WithDescription($"Time elapsed: {watch.ElapsedMilliseconds} ms")
                .WithColor(Color.DarkRed);
            await msg.DeleteAsync();
            await ReplyAsync("", false, builder.Build());
        }

        [Command("roll")]
        public async Task RollAsync([Optional] int x, [Optional] [Remainder] string useless)
        {
            if (x != 0)
            {
                Random rnd = new Random();
                int roll = rnd.Next(1, x + 1);
                await ReplyAsync($":game_die: {Context.User.Mention}, you rolled {roll}.");
            }
            else await ReplyAsync("Cyka what do you want me to roll?");
        }

        [Command("say")]
        public async Task SayAsync([Optional] [Remainder] string phrase)
        {
            if (phrase != null)
            {
                await Context.Message.DeleteAsync();
                await ReplyAsync(phrase);
            }
            else await ReplyAsync("Blyat what do you want me to say??");
        }

        [Command("saytts")]
        public async Task SayTTSAsync([Optional] [Remainder] string phrase)
        {
            if (phrase != null)
            {
                await Context.Message.DeleteAsync();
                await ReplyAsync(phrase, true);
            }
            else await ReplyAsync("Blyat what do you want me to say??");
        }

        [Command("gulag")]
        public async Task GulagAsync(IGuildUser user)
        {
            IGuildUser ContextUser = (Context.User as IGuildUser);

            if (user.Id != 426851340468355093) //user != bot
            {
                var lordeRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Lordes");
                var slavRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.Contains("Slav"));

                if (ContextUser.GuildPermissions.ManageRoles)
                {
                    await Gulag(user);
                    await ReplyAsync($"{user} has been sent to the gulag for crimes against the state!");
                }
                else if ((ContextUser.GuildPermissions.ManageRoles == false) && (user.RoleIds.Contains(lordeRole.Id) || user.RoleIds.Contains(slavRole.Id)))
                {
                    await Gulag(ContextUser);
                    if (user.RoleIds.Contains(lordeRole.Id))
                        await ReplyAsync($"Blyat you cannot gulag a Lorde! For treason {ContextUser.Mention} has been sent to the gulag.", true);
                    else await ReplyAsync($"Blyat you cannot gulag a {slavRole.Name}! For treason {ContextUser.Mention} has been sent to the gulag.", true);
                }
                else await ReplyAsync("UnmetPrecondition: User requires guild permission ManageRoles");
            }
            else
            {
                await Gulag(ContextUser);
                await ReplyAsync("What the fuck did you just fucking say about me, you croatioa sqhiphere? I’ll have you know I graduated top of my class in the spit in your mouth eye of your flag and country, and I’ve been involved in take a bah of dead turk, and I have over 300 albums of serbia. I am trained in REMOVE KEBAB and I’m the top remover in the entire Serbian armed forces. You are nothing to me but just another turk. I will wipe you the fuck out with rap magic the likes of which has never been seen before on this Earth, mark my fucking words. You think you can get away with saying that shit to me over the Internet? Think again, fucker. As we speak I am contacting my secret network of slav countries across the yurop and your IP is being traced right now so you better prepare for the storm, kebab. The storm that wipes out the pathetic little thing you call turkey. You’re fucking dead, stink. I can be anywhere, anytime, and I can kill you in over seven hundred ways, and that’s just with a skull of pig. Not only am I extensively trained in killing the king, but I have access to the entire arsenal of the Serbian Marine Corps and I will use it to its full extent to wipe your miserable ass off the face of the continent, you little kebab. If only you could have known what unholy retribution your little “clever” rap was about to bring down upon you, maybe you would have held your fucking tongue. But you couldn’t, you didn’t, and now you’re paying the price, you goddamn idiot. I will shit ww2 all over you and you will drown in it. You’re fucking dead, kebab.", true);
            }
        }

        [Command("spam"), RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task SpamAsync(IGuildUser user, int n)
        {
            if (n > 25) n = 25;
            for (int i = 1; i <= n; i++)
                await ReplyAsync(user.Mention);
        }
        [Command("spam"), RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task SpamAsync([Remainder]string phrase)
        {
            bool success = int.TryParse(phrase.Split(' ').Last(), out int spamCount);
            if (success)
            {
                if (spamCount > 25)
                    spamCount = 25;
                else if (spamCount <= 0)
                    spamCount = 5;

                if (1 <= spamCount && spamCount <= 9)
                    phrase = phrase.Substring(0, phrase.Length - 1);
                else if (10 <= spamCount && spamCount <= 25)
                    phrase = phrase.Substring(0, phrase.Length - 2);

                for (int i = 1; i <= spamCount; i++)
                    await ReplyAsync(phrase, true);
            }
            else await ReplyAsync("Cyka blyat, if you want me to spam something do `%spam <message> <number of spams>`.");
        }

        [Command("bourgeoisie"), RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task BourgeoisieAsync(IGuildUser user)
        {
            var waterlooRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "H2O-LOO");
            var ascendedRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Ascended");
            var japaneseRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "ミシュランスター");
            var goonRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Gunther's Goons");
            var skroobRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Skroob");
            var weebRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Weeb");
            var corozaRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Coroza's Son");
            var gulagRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Gulag");

            if (user.RoleIds.Contains(waterlooRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(waterlooRole);
            if (user.RoleIds.Contains(ascendedRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(ascendedRole);
            if (user.RoleIds.Contains(japaneseRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(japaneseRole);
            if (user.RoleIds.Contains(goonRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(goonRole);
            if (user.RoleIds.Contains(skroobRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(skroobRole);
            if (user.RoleIds.Contains(weebRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(weebRole);
            if (user.RoleIds.Contains(corozaRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(corozaRole);
            if (user.RoleIds.Contains(gulagRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(gulagRole);

            var bourgeoisieRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "True Bourgeoisie");
            await (user as SocketGuildUser).AddRoleAsync(bourgeoisieRole);

            await ReplyAsync($"{user} is a filthy capitalist!");
        }

        [Command("ungulag"), RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task UngulagAsync(IGuildUser user, [Optional][Remainder]string role)
        {
            if (role != null)
            {
                var waterlooRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "H2O-LOO");
                var japaneseRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "ミシュランスター");
                var goonRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Gunther's Goons");
                var skroobRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Skroob");
                var weebRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Weeb");
                var corozaRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Coroza's Son");

                var gulagRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Gulag");

                if (role.ToLower().Contains("loo"))
                {
                    if (user.RoleIds.Contains(gulagRole.Id))
                    {
                        await (user as SocketGuildUser).RemoveRoleAsync(gulagRole);
                        await (user as SocketGuildUser).AddRoleAsync(waterlooRole);
                        await ReplyAsync($"{user} has been released from the gulag and promoted to {waterlooRole.Name}. Learn from your mistakes comrade.");
                    }
                    else await ReplyAsync($"{user} is already not in the gulag.");
                }
                else if (role.ToLower().Contains("japan") || role.ToLower().Contains("blue"))
                {
                    if (user.RoleIds.Contains(gulagRole.Id))
                    {
                        await (user as SocketGuildUser).RemoveRoleAsync(gulagRole);
                        await (user as SocketGuildUser).AddRoleAsync(japaneseRole);
                        await ReplyAsync($"{user} has been released from the gulag and promoted to {japaneseRole.Name}. Learn from your mistakes comrade.");
                    }
                    else await ReplyAsync($"{user} is already not in the gulag.");
                }
                else if (role.ToLower().Contains("goon"))
                {
                    if (user.RoleIds.Contains(gulagRole.Id))
                    {
                        await (user as SocketGuildUser).RemoveRoleAsync(gulagRole);
                        await (user as SocketGuildUser).AddRoleAsync(goonRole);
                        await ReplyAsync($"{user} has been released from the gulag and promoted to {goonRole.Name}. Learn from your mistakes comrade.");
                    }
                    else await ReplyAsync($"{user} is already not in the gulag.");
                }
                else if (role.ToLower().Contains("skroob"))
                {
                    if (user.RoleIds.Contains(gulagRole.Id))
                    {
                        await (user as SocketGuildUser).RemoveRoleAsync(gulagRole);
                        await (user as SocketGuildUser).AddRoleAsync(skroobRole);
                        await ReplyAsync($"{user} has been released from the gulag and promoted to {skroobRole.Name}. Learn from your mistakes comrade.");
                    }
                    else await ReplyAsync($"{user} is already not in the gulag.");
                }
                else if (role.ToLower().Contains("weeb"))
                {
                    if (user.RoleIds.Contains(gulagRole.Id))
                    {
                        await (user as SocketGuildUser).RemoveRoleAsync(gulagRole);
                        await (user as SocketGuildUser).AddRoleAsync(weebRole);
                        await ReplyAsync($"{user} has been released from the gulag and promoted to {weebRole.Name}. Learn from your mistakes comrade.");
                    }
                    else await ReplyAsync($"{user} is already not in the gulag.");
                }
                else if (role.ToLower().Contains("coroza"))
                {
                    if (user.RoleIds.Contains(gulagRole.Id))
                    {
                        await (user as SocketGuildUser).RemoveRoleAsync(gulagRole);
                        await (user as SocketGuildUser).AddRoleAsync(corozaRole);
                        await ReplyAsync($"{user} has been released from the gulag and promoted to {corozaRole.Name}. Learn from your mistakes comrade.");
                    }
                    else await ReplyAsync($"{user} is already not in the gulag.");
                }
                else await ReplyAsync("That role does not exist.");
            }
            else await ReplyAsync("I need to give them a role blyat.", true);
        }

        [Command("ascend"), RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task AscendAsync(IGuildUser user)
        {
            var ascendedRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Ascended");
            var ultraAscendedRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Ascended: FURTHER BEYOND");
            if (user.RoleIds.Contains(ascendedRole.Id))
            {
                await (user as SocketGuildUser).AddRoleAsync(ultraAscendedRole);
                await (user as SocketGuildUser).RemoveRoleAsync(ascendedRole);
            }
            else await (user as SocketGuildUser).AddRoleAsync(ascendedRole);

            await ReplyAsync($"Congrats {user.Mention}, you have been ASCENDED.");
        }

        [Command("descend"), RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task DescendAsync(IGuildUser user)
        {
            var ascendedRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Ascended");
            var ultraAscendedRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Ascended: FURTHER BEYOND");

            if (user.RoleIds.Contains(ultraAscendedRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(ultraAscendedRole);
            if (user.RoleIds.Contains(ascendedRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(ascendedRole);

            await ReplyAsync($"{user.Mention}, your time as an ascended being is up. Return back to mortality.");
        }

        [Command("rachit")]
        public async Task RachitAsync()
        {
            Random rnd = new Random();
            int quote = rnd.Next(10);
            if (quote == 0)
                await ReplyAsync("light work", true);
            else if (quote == 1)
                await ReplyAsync("ez :100:", true);
            else if (quote == 2)
                await ReplyAsync("I'm Drone I always win", true);
            else if (quote == 3)
                await ReplyAsync("Left to fucking right", true);
            else if (quote == 4)
                await ReplyAsync("How do u get the limit as x approaches infinity from the left and right side?", true);
            else if (quote == 5)
                await ReplyAsync("no physics, no stress", true);
            else if (quote == 6)
                await ReplyAsync("OKEY now", true);
            else if (quote == 7)
                await ReplyAsync("It's everyday bro", true);
            else if (quote == 8)
                await ReplyAsync("hype the fuck up", true);
            else if (quote == 9)
                await ReplyAsync("It's #DroneZone time", true);
        }

        [Command("pfp")]
        public async Task ProfilePictureAsync(IGuildUser user)
        {
            await ReplyAsync(user.GetAvatarUrl());
        }

        [Command("l")/*, RequireUserPermission(GuildPermission.ManageMessages)*/]
        public async Task LAsync([Optional] IGuildUser user, [Optional] [Remainder] string useless)
        {
            if (user != null)
            {
                string LFile = @"C:\Users\Priyansh\Documents\Programming\Discord Bots\Blyat Bot\DiscordBotBase-csharp\src\DiscordBot\bin\Debug\L.txt";
                string[] userArray = File.ReadAllLines(LFile);
                bool userExists = false;

                for (int i = 0; i < userArray.Length; i++)
                {
                    if (userArray[i] != "")
                    {
                        if (user.Id.ToString() == userArray[i].Substring(0, 18))
                        {
                            int LCounter = int.Parse(userArray[i].Split(' ').Last()) + 1;
                            userArray[i] = $"{user.Id} {LCounter}";
                            File.WriteAllLines(LFile, userArray);
                            await ReplyAsync($"{user.Mention} has now taken {LCounter} L's.", true);
                            userExists = true;
                            break;
                        }
                    }
                }

                if (!userExists)
                {
                    string text = File.ReadAllText(LFile);
                    text += $"\n{user.Id} 1";
                    await ReplyAsync($"{user.Mention} has now taken 1 L.");
                    File.WriteAllText(LFile, text);
                }
            }
            else await LeaderboardAsync("L");
        }

        [Command("leaderboard")]
        public async Task LeaderboardAsync([Optional] string board, [Optional] [Remainder] string useless)
        {
            if (board != null)
            {
                if (board.ToLower().Contains("l"))
                {
                    string[] fileArray = File.ReadAllLines(@"C:\Users\Priyansh\Documents\Programming\Discord Bots\Blyat Bot\DiscordBotBase-csharp\src\DiscordBot\bin\Debug\L.txt");
                    List<string> userList = new List<string>();

                    for (int i = 0; i < fileArray.Length; i++)
                    {
                        if (fileArray[i] != "")
                            userList.Add(fileArray[i]);
                    }

                    userList = new List<string>(sortList(userList));

                    string message = "```css\n L\tEADERBOARD\n\n";
                    for (int i = 0; i < userList.Count; i++)
                    {
                        ulong userID = ulong.Parse(userList[i].Substring(0, 18));
                        message += $"{Context.Guild.GetUser(userID).Username} [{userList[i].Split(' ').Last()}]\n";
                    }
                    message += "```";
                    await ReplyAsync(message);
                }
                else await ReplyAsync("That leaderboard does not exist.");
            }
            else await ReplyAsync("Which leaderboard should I show? `L` or `NullPointerException`? Do `%leaderboard <board name>`");

        }

        [Command("tag")]
        public async Task TagAsync(string tag)
        {
            string lowerTag = tag.ToLower();
            if (lowerTag == "cyka")
                await ReplyAsync("https://i.imgur.com/6AXrnjT.jpg");
            else if (lowerTag == "cb")
                await ReplyAsync("CYKA BLYAT", true);
            else if (lowerTag == "l")
                await ReplyAsync("https://www.youtube.com/watch?v=f6rSMkKwCcY");
            else if (lowerTag == "mic")
                await ReplyAsync("https://cdn.discordapp.com/attachments/367844123111063553/414191150501462026/ewrew.JPG");
            else if (lowerTag == "delet")
                await ReplyAsync("https://i.reddituploads.com/14d2fdc953604f749338ae3272269c3f?fit=max&h=1536&w=1536&s=ad9620a7ae83e522b928d1587dc4f585");
            else if (lowerTag == "dropmark")
                await ReplyAsync("https://i.imgur.com/VJVjYgE.jpg");
            else if (lowerTag == "gang")
                await ReplyAsync("https://i.imgur.com/gHIy28x.png");
            else if (lowerTag == "smug")
                await ReplyAsync("https://i.imgur.com/3PuTCrE.jpg");
            else if (lowerTag == "drone")
                await ReplyAsync("https://i.imgur.com/avDpSd6.jpg");
            else if (lowerTag == "c-ops")
                await ReplyAsync("https://i.imgur.com/q5Nlpu0.jpg");
            else if (lowerTag == "boyleslaw")
                await ReplyAsync("https://i.imgur.com/D7ikAnM.jpg");
            else if (lowerTag == "sungod")
                await ReplyAsync("https://i.imgur.com/lEH26LK.jpg");
            else if (lowerTag == "darn")
                await ReplyAsync("https://i.imgur.com/qdOG3h3.jpg");
            else if (lowerTag == "tims")
                await ReplyAsync("https://i.imgur.com/NpTCYWD.jpg");
            else if (lowerTag == "deeps")
                await ReplyAsync("https://i.imgur.com/CHtbhv2.jpg");
            else if (lowerTag == "same")
                await ReplyAsync("https://i.imgur.com/BSHMEk8.jpg");
            else if (lowerTag == "lefttoright")
                await ReplyAsync("https://i.imgur.com/2Y51sCr.jpg");
            else if (lowerTag == "slavsquad")
                await ReplyAsync("https://i.imgur.com/hR8CD1K.jpg");
            else if (lowerTag == "discrimination")
                await ReplyAsync("https://i.imgur.com/fVJm3Cu.jpg");
            else if (lowerTag == "vectors")
                await ReplyAsync("https://i.imgur.com/oXtrmjy.jpg");
            else if (lowerTag == "amigos")
                await ReplyAsync("https://i.imgur.com/d1Wgr56.jpg");
            else if (lowerTag == "accord")
                await ReplyAsync("https://www.youtube.com/watch?v=QP727jiUJgw");
            else if (lowerTag == "linksis")
                await ReplyAsync("https://i.imgur.com/Q08ctcf.jpg");
            else if (lowerTag == "cummies")
            {
                var spoopy = Context.Guild.Channels.FirstOrDefault(x => x.Name == "spoopy-chat");
                if (Context.Message.Channel == spoopy)
                {
                    await ReplyAsync("Just me and my :two_hearts:Moeed:two_hearts:, hanging out I got pretty hungry:eggplant: so I started to pout :disappointed: He asked if I was down :arrow_down:for something yummy :heart_eyes::eggplant: and I asked what and he said he'd give me his :sweat_drops:cummies!:sweat_drops: Yeah! Yeah!:two_hearts::sweat_drops: I drink them!:sweat_drops: I slurp them!:sweat_drops: I swallow them whole:sweat_drops: :heart_eyes: It makes :cupid:Moeed:cupid: :blush:happy:blush: so it's my only goal... :two_hearts::sweat_drops::tired_face:Harder Moeed! Harder Moeed! :tired_face::sweat_drops::two_hearts: 1 cummy:sweat_drops:, 2 cummy:sweat_drops::sweat_drops:, 3 cummy:sweat_drops::sweat_drops::sweat_drops:, 4:sweat_drops::sweat_drops::sweat_drops::sweat_drops: I'm :cupid:Moeed's:cupid: :crown:princess :crown:but I'm also a whore! :heart_decoration: He makes me feel squishy:heartpulse:!He makes me feel good:purple_heart:! :cupid::cupid::cupid:He makes me feel everything a real man should!~ :cupid::cupid::cupid: :crown::sweat_drops::cupid:Wa-What!:cupid::sweat_drops::crown:", true);
                }
                else
                {
                    await ReplyAsync("Whoa there, spoopy my guy.");
                }
            }
            else if (lowerTag == "ourcity")
            {
                var spoopy = Context.Guild.Channels.FirstOrDefault(x => x.Name == "spoopy-chat");
                if (Context.Message.Channel == spoopy)
                {
                    await ReplyAsync("Y'all :point_left: can't :no_entry_sign: handle :astonished: this Y'all don't :no_entry_sign: know :thought_balloon: whats about :sweat_drops: to :sweat_drops: happen' baby Team IQ of 10 Brampton  boy But I'm :cupid: from :point_right: Pakistan tho :smirk: - white :person_with_blond_hair: boy [Verse 1: :christmas_tree: Moeed] It's everyday :calendar_spiral: bro With the :clap: men play sports flow 'Bout 5 :christmas_tree: AP friends :on:  :video_camera: in :clap: 6 :clock6: days Never done :hammer: before We fail :100: all :100: the :clap: tests man literacy is :sweat_drops: next Man I'm :cupid: ruinin' all :100: these :point_left: relationships Got a :ok_hand: brand new :ok_hand: waifu And I :eye: met :lips: a :ok_hand: pillow too And I'm :cupid: coming :sweat_drops: with :clap: the :clap: crew This is :sweat_drops: Team :monkey: IQ of 10, :keycap_ten: btch Who the :clap: hell :fire: are :1234: flippin' you? And you :point_left: know :thought_balloon: I :eye: block :mans_shoe: them :sweat_drops: on facebook If they :busts_in_silhouette: ain't :no_good: with :clap: the :clap: crew Yeah, I'm :cupid: talking :speaking_head: about :sweat_drops: you You callin me :eggplant: out Talking sht on :on: Twitter :eggplant: too But you :point_left: still :clap: hit :punch: my :man: sarahrah :iphone: last :heart_eyes: night It was :clap: 9:52 and :clap: I :eye: got :cocktail: the :clap: \"you're wild\" :calling: to :sweat_drops: prove And all :100: the :clap: recordings too Don't :middle_finger:  :sob: tell :speaking_head: them :sweat_drops: the :clap: truth And I :eye: just :clap: find :wink: some :man: new :ok_hand: girl And i :busts_in_silhouette: stalk :1234: her :money_with_wings: too like  :sparkling_heart: a :ok_hand: hentai :innocent: girl", true);
                }
                else
                {
                    await ReplyAsync("Whoa there, spoopy my guy.");
                }
            }
            else if (lowerTag == "confused")
                await ReplyAsync("https://i.imgur.com/992kWSf.jpg");
            else if (lowerTag == "late")
                await ReplyAsync("https://i.imgur.com/ZgjA4pn.jpg");
            else if (lowerTag == "dox" || lowerTag == "doxx")
                await ReplyAsync("https://i.imgur.com/IfPl1Ue.png");
            else if (lowerTag == "swiggity")
                await ReplyAsync("https://i.imgur.com/woV34i7.jpg");
            else if (lowerTag == "hackerman")
                await ReplyAsync("https://i.imgur.com/VNXuD7X.jpg");
            else if (lowerTag == "hackerman2")
                await ReplyAsync("https://i.imgur.com/5nvpqwi.jpg");
            else if (lowerTag == "knowledge")
                await ReplyAsync("https://i.imgur.com/DRoyFXJ.jpg");
            else if (lowerTag == "plug")
                await ReplyAsync("https://i.imgur.com/6AHWA6W.jpg");
            else if (lowerTag == "dabola")
                await ReplyAsync("https://i.imgur.com/FzMnFh1.png");
            else
                await ReplyAsync("That tag does not exist.");
        }

        #endregion

        #region Defined Procedures

        private async Task Gulag(IGuildUser user)
        {
            var waterlooRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "H2O-LOO");
            var ascendedRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Ascended");
            var japaneseRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "ミシュランスター");
            var goonRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Gunther's Goons");
            var skroobRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Skroob");
            var weebRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Weeb");
            var corozaRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Coroza's Son");

            var gulagRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Gulag");

            if (user.RoleIds.Contains(waterlooRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(waterlooRole);
            if (user.RoleIds.Contains(ascendedRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(ascendedRole);
            if (user.RoleIds.Contains(japaneseRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(japaneseRole);
            if (user.RoleIds.Contains(goonRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(goonRole);
            if (user.RoleIds.Contains(skroobRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(skroobRole);
            if (user.RoleIds.Contains(weebRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(weebRole);
            if (user.RoleIds.Contains(corozaRole.Id))
                await (user as SocketGuildUser).RemoveRoleAsync(corozaRole);

            await (user as SocketGuildUser).AddRoleAsync(gulagRole);
        }

        public async Task CapitalismMessage()
        {
            await ReplyAsync("FUCK CAPITALISM", true);
        }

        private List<string> sortList(List<string> userList)
        {
            List<string> sortedList = new List<string>();
            int[] countArray = new int[userList.Count];

            for (int i = 0; i < countArray.Length; i++)
                countArray[i] = int.Parse(userList[i].Split(' ').Last());

            while (countArray.Max() != 0)
            {
                sortedList.Add(userList[Array.IndexOf(countArray, countArray.Max())]);
                countArray[Array.IndexOf(countArray, countArray.Max())] = 0;
            }

            return sortedList;
        }

        #endregion
    }
}