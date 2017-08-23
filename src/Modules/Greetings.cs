using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheShooterBot.Modules
{

    // Module Class.  Kicks, bans, mutes, deafens, etc.
    public class ModeratorModule : ModuleBase<SocketCommandContext>
    {
        // Kick users.

        [Command("kick")]
        [Remarks("Kicks the user in question")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task Kick([Remainder]SocketGuildUser user)
        {
            await ReplyAsync($"Take care, {user.Mention} :wave:");
            await user.KickAsync();
        }

        // Ban users.
        [Command("ban")]
        [Remarks("Bans the specified user in question")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Ban(SocketGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null)
                await ReplyAsync($"Mention their name, fam.");
            if (string.IsNullOrWhiteSpace(reason))
                await ReplyAsync($"Provide a reason for ban, thanks.");
            await ReplyAsync($"Get the fuck out, {user.Mention}!");
            var gld = Context.Guild as SocketGuild;
            await gld.AddBanAsync(user);
            string nameOfBanned = ($"{user.Mention}");
            System.IO.File.WriteAllText(@"C:\Users\Daavid M\Desktop\Coding for School and Future\BotforDiscord\TheShooterBot\TheShooterBot\BanList.txt", nameOfBanned);
        }

        // Purge comments.
        [Command("purge")]
        [Remarks("Purges entire chat history.")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Purge([Remainder] int x = 0)
        {
            var purgedMessages = await Context.Channel.GetMessagesAsync(x + 1).Flatten();
            var user = Context.User as SocketGuildUser;
            if (x < 100)
            {
                await Context.Channel.DeleteMessagesAsync(purgedMessages);

                await ReplyAsync($"We purged {x} messages for you, {user.Mention}.  Clean slate, baby.");
            }
            else if (x >= 100 || x < 0)
                await ReplyAsync($"We can't purge that many messages, {user.Mention}.  Try the !spurge command.");

        }
        // Super Purge
        [Command("spurge")]
        [Remarks("Purges entire chat history.")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task sPurge([Remainder] int x = 100)
        {
            var purgedMessages = await Context.Channel.GetMessagesAsync(x).Flatten();
            var user = Context.User as SocketGuildUser;
            await Context.Channel.DeleteMessagesAsync(purgedMessages);
            await ReplyAsync($"Wiped all recent messages out, {user.Mention}.  Clean slate.");
        }

        //Creates a role.
        [Command("role")]
        [Remarks("The bot creates a role.")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task AddRole(string role)
        {
            await Context.Guild.CreateRoleAsync(role);
            var user = Context.User as SocketGuildUser;
            await ReplyAsync($"{user.Mention} has created **{role}**.");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Daavid M\Desktop\Coding for School and Future\BotforDiscord\TheShooterBot\TheShooterBot\RoleList.txt", true))
            {
              file.WriteLine(role);
            }
          //  using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Daavid M\Desktop\Coding for School and Future\BotforDiscord\TheShooterBot\TheShooterBot\DeletedRoles.txt", true))
           // {
             //   file.WriteLine(role);
         //   }

        }

        // Deletes a role.
        [Command("delrole")]
        [Remarks("Deletes role from user.")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task Mute(SocketRole role)
        {
            var user = Context.User as SocketGuildUser;
            await role.DeleteAsync();
            await ReplyAsync($"{user.Mention} has deleted the role **{role}**.");
            string s = ($"{role}");
            string line = null;

            using (System.IO.StreamReader reader = new System.IO.StreamReader(@"C:\Users\Daavid M\Desktop\Coding for School and Future\BotforDiscord\TheShooterBot\TheShooterBot\DeletedRoles.txt"))
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(@"C:\Users\Daavid M\Desktop\Coding for School and Future\BotforDiscord\TheShooterBot\TheShooterBot\RoleList.txt"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (String.Compare(line, s) == 0)
                            continue;
                        writer.WriteLine(line);
                    }
                }
            }

        }

        [Command("rolelist")]
        [Remarks("Shows the current role list.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ShowRoleList()
        {
            string text = System.IO.File.ReadAllText(@"C:\Users\Daavid M\Desktop\Coding for School and Future\BotforDiscord\TheShooterBot\TheShooterBot\RoleList.txt");

            await ReplyAsync($"```{text}```");
        }

        //Creates a text channel
        [Command("create")]
        [Remarks("Creates a text channel.")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task CreateChannel(string channelName)
        {
            await Context.Guild.CreateTextChannelAsync(channelName);
            var user = Context.User as SocketGuildUser;
            await ReplyAsync($"{user.Mention} has created Channel **{channelName}**.");
        }
        //Creates a Voice Channel
        [Command("createv")]
        [Remarks("Creates a voice channel.")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task CreateVChannel(string vChannelName)
        {
            await Context.Guild.CreateVoiceChannelAsync(vChannelName);
            var user = Context.User as SocketGuildUser;
            await ReplyAsync($"{user.Mention} has created Voice Channel **{vChannelName}**.");
        }

        //Deletes Text and Voice Channels.
        [Command("delete")]
        [Remarks("Deletes voice/test channel.")]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task DeleteChannel(SocketGuildChannel channelName)
        {
            await channelName.DeleteAsync();
            var user = Context.User as SocketGuildUser;
            await ReplyAsync($"{user.Mention} has deleted the Channel **{channelName}**.");
        }

        //Gives user a role. Make sure the role has been created!
        [Command("give")]
        [Remarks("Gives user a role")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task GibsMeDat(SocketGuildUser recipient, IRole role)
        {
            var user = Context.User as SocketGuildUser;
            await recipient.AddRoleAsync(role);
            await ReplyAsync($"{user.Mention} has given role **{role}** to {recipient}.");
        }

        [Command("remove")]
        [Remarks("Removes a role from user.")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task RemuvsMeDat(SocketGuildUser recipient, IRole role)
        {
            var user = Context.User as SocketGuildUser;
            await recipient.RemoveRoleAsync(role);
            await ReplyAsync($"{user.Mention} has removed role **{role}** to {recipient}.");
        }

        [Command("mute")]
        [Remarks("Mutes the user in question.")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task UserJoined(SocketGuildUser recipient)
        {
            var role = recipient.Guild.Roles.Where(has => has.Name.ToUpper() == "muted".ToUpper());
            var user = Context.User as SocketGuildUser;
            await recipient.AddRolesAsync(role);
            await ReplyAsync($"{user.Mention} has muted you. Shut the fuck up, {recipient}.");
        }

        [Command("name")]
        [Remarks("Changes the name of the bot.")]
        [RequireUserPermission(GuildPermission.ChangeNickname)]
        public async Task Username(string name)
        {
            await Context.Client.CurrentUser.ModifyAsync(x => x.Username = name);
            await ReplyAsync("Bot's name has been updated.");
        }

        // Changes channel name to whatever.
        //Copied this code a bit, too.  Boooooooo.  I need to really learn C#!!!
        [Command("chan")]
        [Remarks("Changes the name of channel.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task chanName(string chan_name)
        {
            var user = Context.User as SocketGuildUser;
            await (Context.Channel as ITextChannel)?.ModifyAsync(x =>
            {
                x.Name = chan_name;
            });
            await ReplyAsync($"{user.Mention} has changed the channel name to ``{chan_name}``");
        }

        // Gets user information.
        //Code pretty much stolen from CodingWithStorm.  Credit to forum.

        [Command("userinfo")]
        [Name("userinfo `<user>`")]
        public async Task UserInfo(IGuildUser user)
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            var thumbnailurl = user.GetAvatarUrl();
            var date = $"{user.CreatedAt.Month}/{user.CreatedAt.Day}/{user.CreatedAt.Year}";
            var auth = new EmbedAuthorBuilder()

            {
                Name = user.Username,
                IconUrl = thumbnailurl,
            };

            var embed = new EmbedBuilder()

            {
                Color = new Color(29, 140, 209),
                Author = auth
            };

            var us = user as SocketGuildUser;
            var D = us.Username;
            var S = date;
            var C = us.Status;
            var CC = us.JoinedAt;
            //embed.Title = $"**{us.Username}** Information";
            embed.Description = $"Username: **{D}**\nCreated at: **{S}**\nCurrent Status: **{C}**\nJoined server at: **{CC}**";

            await ReplyAsync("", false, embed.Build());
        }

        //Lets user see the profile.
        // Minor changes.  Most credit goes to CodingWithStorm.
        [Command("profile")]
        [Name("Lets the user see their own profile.")]
        public async Task UserInfo()
        {
            var user = Context.User as SocketGuildUser;
            var application = await Context.Client.GetApplicationInfoAsync();
            var thumbnailurl = user.GetAvatarUrl(ImageFormat.Auto, 256);
            var date = $"{user.CreatedAt.Month}/{user.CreatedAt.Day}/{user.CreatedAt.Year}";
            var auth = new EmbedAuthorBuilder()

            {
                Name = user.Username,
                IconUrl = thumbnailurl,
            };
            var embed = new EmbedBuilder()
            {
                Color = new Color(29, 140, 209),
                Author = auth
            };

            var us = user;
            var D = us.Username;
            var S = date;
            var C = us.Status;
            var CC = us.JoinedAt;
           // embed.Title = $"**{us.Username}** Information";
            embed.Description = $"name: **{D}**\nCreated at: **{S}**\nCurrent Status: **{C}**\nJoined server at: **{CC}**";

            await ReplyAsync("", false, embed.Build());
        }


        // Help command.
        [Command("help")]
        [Remarks("Helps the user with commands.")]
        public async Task Help()
        {
            var user = Context.User as SocketGuildUser;
            await ReplyAsync($"Hey, {user.Mention}.  Welcome to the help menu. Let's get you started!\n\n" +

                "```This bot was coded by Daavid (aka @enraged) of Acoustic Mafia, AntiSocial Network & Cactus Garden. Written in C#, using Discord.NET Wrapper.\n" +
                "PM him on discord at your leisure for the GitHub associated with the bot.\n\n" +
                "Command List as follows:\n\n" +
                "!help - Shows this prompt!\n" +
                "!commands - Shows command list prompt.\n" +
                "!hey = Bot greets you.\n" +
                "!hello = Bot greets you again, but differently.\n" +
                "!godhatesfags = Blame Alex for this one.\n" +
                "!profile - Users can view information about themselves." +
                "!userinfo @name - Users can view information about others." +
                "!dm @user msg_here or !dm user msg_here - The bot will send the user a DM with a msg.\n\n" +

                "The following are ADMINISTRATIVE and SUB-ADMINISTRATIVE COMMANDS:\n\n" +

                "!kick @user - The bot will kick user. Only for Administrators.\n" +
                "!ban @user - The bot will ban user.  Only for Administrators.\n" +
                "!name NameOfBot - Renames the bot.  Administrators only.\n" +
                "!mute @user - The bot will mute the user in question. Muting perms required.\n" +
                "!role RoleName - Creates a role.  Role management perms required.\n" +
                "!delrole RoleName - Deletes a role.  Role management perms required.\n" +
                "!give @user RoleName - Gives role to user. Make sure role exists, otherwise won't work!  Role management perms required.\n" +
                "!remove  @user RoleName - Will remove a role from a user.  Role management perms required.\n" +
                "!rolelist - Shows roles in the server. Message management perms required.\n```");
            
            await ReplyAsync($"```!create NameOfChannel - Creates text channel.Channel management perms required.\n" +
                "!createv NameOfVoiceChannel - Creates voice channel. Channel management perms required.\n" +
                "!chan NameOfChannel - Rename the channel you're in.  Channel management perms required.\n" +
                
                "!delete NameOfChannel - Deletes channel/voice channel. Channel management perms required.\n" +
                "!purge N - Purges 0<=N<100 chat msgs. Message management perms required.\n" +
                "!spurge - Purge chat history.  Message management perms required.\n\n" +

                "This bot is still a work-in-progress.  Please note that more commands will be added in time.  Requests for other commands can be directed to @enraged.```");
        }

        //Shows commands.  More or less the same as !help, just without the introduction.

        [Command("commands")]
        [Remarks("Will show commands to user.")]
        public async Task CommandShow()
        {
            await ReplyAsync("```Command List as follows:\n\n" +

                "!help - Shows this prompt!\n" +
                "!commands - Shows command list prompt.\n" +
                "!hey = Bot greets you.\n" +
                "!hello = Bot greets you again, but differently.\n" +
                "!godhatesfags = Blame Alex for this one.\n" +
                "!profile - Users can view information about themselves." +
                "!userinfo @name - Users can view information about others." +
                "!dm @user msg_here or !dm user msg_here - The bot will send the user a DM with a msg.\n\n" +

                "The following are ADMINISTRATIVE and SUB-ADMINISTRATIVE COMMANDS:\n\n" +

                "!kick @user - The bot will kick user. Only for Administrators.\n" +
                "!ban @user - The bot will ban user.  Only for Administrators.\n" +
                "!name NameOfBot - Renames the bot.  Administrators only.\n" +
                "!mute @user - The bot will mute the user in question. Muting perms required.  Work in Progress!\n" +
                "!role RoleName - Creates a role.  Role management perms required.\n" +
                "!delrole RoleName - Deletes a role.  Role management perms required.\n" +
                "!give @user RoleName - Gives role to user. Make sure role exists, otherwise won't work!  Role management perms required.\n" +
                "!remove  @user RoleName - Will remove a role from a user.  Role management perms required.\n" +
                "!rolelist - Shows roles in the server. Message management perms required.\n" +
                "!create NameOfChannel - Creates text channel.Channel management perms required.\n" +
                "!createv NameOfVoiceChannel - Creates voice channel. Channel management perms required.\n" +
                "!chan NameOfChannel - Rename the channel you're in.  Channel management perms required.\n" +
                "!delete NameOfChannel - Deletes channel/voice channel. Channel management perms required.\n" +
                "!purge N - Purges 0<=N<100 chat msgs. Message management perms required.\n" +
                "!spurge - Purge chat history.  Message management perms required.\n\n" +

                "This bot is still a work-in-progress.  Please note that more commands will be added in time.  Requests for other commands can be directed to @enraged.```");
        }
    }

    // Greetings Class.  Hello, goodbye, what'sup, etc.

    public class Greetings : ModuleBase<SocketCommandContext>
        {
            [Command("hey")]
            [Remarks("Replies hey back to user.")]
            public async Task greetings1()
            {
                var user = Context.User as SocketGuildUser;
                await ReplyAsync($"Hey, {user.Mention}!  What's up?");
            }

            [Command("hello")]
            [Remarks("Replies hello back to user.")]
            public async Task greeting2()
            {
                var user = Context.User as SocketGuildUser;
                await ReplyAsync($"Hello, {user.Mention}!  What's up?");
            }

            [Command("godhatesfags")]
            [Remarks("Joke command for Alex. Haha")]
            public async Task GodHatesGayPeople()
            {
                await Context.Channel.SendMessageAsync("You god damn fucking right he does!");
            }

            // DM Command.  Pretty useless, but kind of cool. Interaction between bot and users.
            [Command("dm")]
            [Remarks("The bot DMs the user.")]
            public async Task DMUser(IUser recipient, [Remainder] string message)
            {
                    var prsnlmsg = await recipient.GetOrCreateDMChannelAsync();
                    await prsnlmsg.SendMessageAsync($"{message}");
                    await ReplyAsync($"{recipient.Mention}, check ya DMs fam?");

        }
    }
}
