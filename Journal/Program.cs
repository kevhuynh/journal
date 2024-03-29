using System;
using System.Collections.Generic;
using System.CommandLine;
using Journal.Data;
using Microsoft.EntityFrameworkCore.Design;

namespace Journal
{
    #region Enums
    public enum FilterField
    {
        All,
        Title,
        Body
    }

    public enum SortField
    {
        Date,
        Title,
        Body
    }

    public enum SortOrder
    {
        Asc,
        Desc
    }

    public enum DateGetOptions
    {
        DayHourMinute,
        Before,
        After
    }
    #endregion

    class Program
    {
        private static void Main(string[] args)
        {
            Option<string> newUserOption = new Option<string>(
                name: "--username",
                description: "New username")
                { IsRequired = true };
            Option<string> titleOption = new Option<string>(
                name: "--title",
                description: "The title of the journal entry",
                getDefaultValue: () => "");

            Option<string> bodyOption = new Option<string>(
                name: "--body",
                description: "The body of the journal entry",
                getDefaultValue: () => "");

            Option<string> usernameOption = new Option<string>(
                name: "--username",
                description: "The username associated with the journal entry")
                { IsRequired = true };

            Option<FilterField> filterFieldOption = new Option<FilterField>(
                name: "--field",
                description: "Filter based on this field. Default is filter on both Title and Body");

            Option<string> filterPhraseOption = new Option<string>(
                name: "--phrase",
                description: "Search for this phrase");

            Option<SortField> sortFieldOption = new Option<SortField>(
                name: "--sortField",
                description: "Sort based on this field. Default field is the date");

            Option<SortOrder> sortOrderOption = new Option<SortOrder>(
                name: "--sortOrder",
                description: "Sort the entries by this order. Default order is ascending");

            Option<DateGetOptions> dateOption = new Option<DateGetOptions>(
                name: "--dateGetOption",
                description: "Get dates according to this date option. Default option is getting by the date, hour, and minute.");

            Option<DateTime> dateTimeOption = new Option<DateTime>(
                name: "--dateTime",
                description: "Get dates based on this date.",
                getDefaultValue: () => DateTime.Today);

            RootCommand rootCommand = new RootCommand("Command line journal app");
            Command createCommand = new Command("create", "Create a new journal entry");
            createCommand.AddOption(bodyOption);
            createCommand.AddOption(titleOption);
            createCommand.AddOption(usernameOption);
            rootCommand.AddCommand(createCommand);

            Command createUserCommand = new Command("createUser", "Create a new user");
            createUserCommand.AddOption(usernameOption);
            rootCommand.AddCommand(createUserCommand);

            Command listCommand = new Command("list", "List the existing journal entries");
            rootCommand.AddCommand(listCommand);

            Command filterCommand = new Command("filter", "Filter the existing journal entries");
            filterCommand.AddOption(filterFieldOption);
            filterCommand.AddOption(filterPhraseOption);
            rootCommand.AddCommand(filterCommand);

            Command sortCommand = new Command("sort", "Sort the existing journal entries");
            sortCommand.AddOption(sortFieldOption);
            sortCommand.AddOption(sortOrderOption);
            rootCommand.AddCommand(sortCommand);

            Command getByDateTime = new Command("getByDateTime", "Get journal entries according to date");
            getByDateTime.AddOption(dateOption);
            getByDateTime.AddOption(dateTimeOption);
            rootCommand.AddCommand(getByDateTime);

            JournalContext? context = SetupDbContext();
            if (context == null) return;

            JournalDb journalDb = new JournalDb(context);
            Application app = new Application(journalDb);

            createCommand.SetHandler((title, body, username) =>
            {
                app.CreateJournalEntry(title, body, username);
            }, titleOption, bodyOption, usernameOption);

            createUserCommand.SetHandler((username) =>
            {
                app.CreateUser(username);
            }, usernameOption);

            listCommand.SetHandler(() =>
            {
                app.ListEntries();
            });

            filterCommand.SetHandler((field, phrase) =>
            {
                app.FilterEntries(field, phrase);
            }, filterFieldOption, filterPhraseOption);

            sortCommand.SetHandler((field, order) =>
            {
                app.SortEntries(field, order);
            }, sortFieldOption, sortOrderOption);

            getByDateTime.SetHandler((getByDateOption, getByDateTime) =>
            {
                app.GetEntriesByDateTime(getByDateOption, getByDateTime);
            }, dateOption, dateTimeOption);

            rootCommand.Invoke(args);
        }

        private static JournalContext? SetupDbContext()
        {
            JournalContext context = new JournalContext();

            if (!context.Database.CanConnect())
            {
                Console.WriteLine("Cannot connect to DB");
                return null;
            }

            context.Database.EnsureCreated();

            return context;
        }
    }
}