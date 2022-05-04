using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaMeetingsManagerApp.Interfaces;
using VismaMeetingsManagerApp.Models;
using VismaMeetingsManagerApp.Services;

namespace VismaMeetingsManagerApp.UI
{
    public class UI
    {
        private readonly IMeetingsService _meetingsService;
        public UI()
        {
            _meetingsService = new MeetingsService();
        }

        public void Run()
        {
            Console.WriteLine("Welcome to Visma's meetings management app!");
            Console.WriteLine();
            bool loop = true;
            while (loop)
            {
                Console.WriteLine("Choose one of the following functions by typing in the corresponding number:");
                Console.WriteLine("1 - Create new meeting");
                Console.WriteLine("2 - Delete a meeting");
                Console.WriteLine("3 - Add an attendee to a meeting");
                Console.WriteLine("4 - Remove an attendee from a meeting");
                Console.WriteLine("5 - Get a list of all meetings");
                Console.WriteLine("6 - Exit");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Type in the name for the meeting: ");
                        string name = Console.ReadLine();
                        if ( name == "")
                        {
                            Console.WriteLine("Meeting name cannot be empty. Try again.");
                            break;
                        }
                        Console.WriteLine("Type in the name of a person who will be responsible for the meeting:");
                        string personName = Console.ReadLine();
                        if (personName == "" || personName.Any(char.IsDigit))
                        {
                            Console.WriteLine("Person's name cannot be empty or contain numbers. Try again.");
                            break;
                        }
                        Console.WriteLine("Type in the surname of a person who will be responsible for the meeting:");
                        string personSurname = Console.ReadLine();
                        if (personSurname == "" || personSurname.Any(char.IsDigit))
                        {
                            Console.WriteLine("Person's surname cannot be empty or contain numbers. Try again.");
                            break;
                        }
                        Console.WriteLine("Type in the description of this meeting:");
                        string description = Console.ReadLine();
                        if (description == "")
                        {
                            Console.WriteLine("Meeting description cannot be empty. Try again.");
                            break;
                        }
                        Console.WriteLine("Choose one of the categories below by typing in the corresponding number:");
                        Console.WriteLine("1 - CodeMonkey");
                        Console.WriteLine("2 - Hub");
                        Console.WriteLine("3 - Short");
                        Console.WriteLine("4 - TeamBuilding");
                        string category = Console.ReadLine();
                        if (!Enum.TryParse(category, out Category categoryValue))
                        {
                            Console.WriteLine("Non existant category chosen. Try again.");
                            break;
                        }
                        Console.WriteLine("Choose one of the meeting types below by typing in the corresponding number:");
                        Console.WriteLine("1 - Live");
                        Console.WriteLine("2 - In person");
                        string type = Console.ReadLine();
                        if (!Enum.TryParse(type, out MeetingType typeValue))
                        {
                            Console.WriteLine("Non existant type chosen. Try again.");
                            break;
                        }
                        Console.WriteLine("Type in the start date of the meeting (use format year-month-day hours:minutes):");
                        string startDateInpt = Console.ReadLine();
                        if (!DateTime.TryParse(startDateInpt, out DateTime startDate))
                        {
                            Console.WriteLine("Start date was entered incorrectly. Try again.");
                            break;
                        }
                        Console.WriteLine("Type in the end date of the meeting (use format year-month-day hours:minutes):");
                        string endDateInpt = Console.ReadLine();
                        if (!DateTime.TryParse(endDateInpt, out DateTime endDate))
                        {
                            Console.WriteLine("End date was entered incorrectly. Try again.");
                            break;
                        }
                        Person person = new Person(personName, personSurname);
                        Meeting meeting = new Meeting(name, person, description, categoryValue, typeValue, startDate, endDate);
                        Console.WriteLine(_meetingsService.CreateMeeting(meeting));
                        break;
                    case "2":
                        Console.WriteLine("Type in your name:");
                        string userName = Console.ReadLine();
                        if(userName == "" || userName.Any(char.IsDigit))
                        {
                            Console.WriteLine("Name cannot be empty or with numbers. Try again.");
                            break;
                        }
                        Console.WriteLine("Type in your surname:");
                        string userSurname = Console.ReadLine();
                        if (userSurname == "" || userSurname.Any(char.IsDigit))
                        {
                            Console.WriteLine("Surname cannot be empty or with numbers. Try again.");
                            break;
                        }
                        if(_meetingsService.GetAllMeetings().Count == 0)
                        {
                            Console.WriteLine("There are no meetings.");
                            break;
                        }
                        PrintAllMeetings(_meetingsService.GetAllMeetings());
                        Console.WriteLine("Type in the name of the meeting which you want to delete:");
                        string meetingName = Console.ReadLine();
                        Person user = new Person(userName, userSurname);
                        Console.WriteLine(_meetingsService.DeleteMeeting(user, meetingName));
                        break;
                    case "3":
                        Console.WriteLine("Type in attendee's name:");
                        string attendeeName = Console.ReadLine();
                        if (attendeeName == "" || attendeeName.Any(char.IsDigit))
                        {
                            Console.WriteLine("Attendee name cannot be empty or with numbers. Try again.");
                            break;
                        }
                        Console.WriteLine("Type in attendee's surname:");
                        string attendeeSurname = Console.ReadLine();
                        if (attendeeSurname == "" || attendeeSurname.Any(char.IsDigit))
                        {
                            Console.WriteLine("Attendee surname cannot be empty or with numbers. Try again.");
                            break;
                        }
                        if (_meetingsService.GetAllMeetings().Count == 0)
                        {
                            Console.WriteLine("There are no meetings.");
                            break;
                        }
                        PrintAllMeetings(_meetingsService.GetAllMeetings());
                        Console.WriteLine("Type in the name of the meeting to which you want to add an attendee:");
                        string meetName = Console.ReadLine();
                        Person attendee = new Person(attendeeName, attendeeSurname);
                        Console.WriteLine(_meetingsService.AddAttendee(attendee, meetName));
                        break;
                    case "4":
                        if (_meetingsService.GetAllMeetings().Count == 0)
                        {
                            Console.WriteLine("There are no meetings.");
                            break;
                        }
                        PrintAllMeetings(_meetingsService.GetAllMeetings());
                        Console.WriteLine("Type in the name of the meeting from which you want to remove an attendee:");
                        string meet = Console.ReadLine();
                        List<Person> attendees = _meetingsService.GetAttendees(meet);
                        if(attendees == null)
                        {
                            Console.WriteLine("There is no meeting by given name. Try again");
                            break;
                        }
                        if (attendees.Count == 0)
                        {
                            Console.WriteLine("There are no attendees in this meeting.");
                            break;
                        }
                        PrintAllAttendees(attendees);
                        Console.WriteLine("Type in the name of an attendee you want to remove:");
                        string attendeeNameRmv = Console.ReadLine();
                        Console.WriteLine("Type in the surname of an attendee you want to remove:");
                        string attendeeSurnameRmv = Console.ReadLine();
                        Person attendeeRmv = new Person(attendeeNameRmv, attendeeSurnameRmv);
                        Console.WriteLine(_meetingsService.RemoveAttendee(attendeeRmv, meet));
                        break;
                    case "5":
                        if (_meetingsService.GetAllMeetings().Count == 0)
                        {
                            Console.WriteLine("There are no meetings.");
                            break;
                        }
                        PrintAllMeetings(_meetingsService.GetAllMeetings());
                        Console.WriteLine("Choose one of the following filters by typing in the corresponding number:");
                        Console.WriteLine("1 - Filter by description");
                        Console.WriteLine("2 - Filter by responsible person");
                        Console.WriteLine("3 - Filter by category");
                        Console.WriteLine("4 - Filter by type");
                        Console.WriteLine("5 - Filter by dates");
                        Console.WriteLine("6 - Filter by number of attendees");
                        Console.WriteLine("Press any other key to return to the menu");
                        string filter = Console.ReadLine();
                        switch (filter)
                        {
                            case "1":
                                Console.WriteLine("Type in keyword for meeting description:");
                                string filterDescription = Console.ReadLine();
                                if(_meetingsService.GetMeetingsByFilter(x => x.Description.Contains(filterDescription)).Count == 0)
                                {
                                    Console.WriteLine("No meetings found using this filter.");
                                    break;
                                }
                                PrintAllMeetings(_meetingsService.GetMeetingsByFilter(x => x.Description.Contains(filterDescription)));
                                break;
                            case "2":
                                Console.WriteLine("Type in responsible person's name:");
                                string filterName = Console.ReadLine();
                                if(filterName == "" || filterName.Any(char.IsDigit))
                                {
                                    Console.WriteLine("Filter name cannot be empty or with numbers. Try again.");
                                }
                                Console.WriteLine("Type in responsible person's surname:");
                                string filterSurname = Console.ReadLine();
                                if (filterSurname == "" || filterSurname.Any(char.IsDigit))
                                {
                                    Console.WriteLine("Filter surname cannot be empty or with numbers. Try again.");
                                }
                                Person filterPerson = new Person(filterName, filterSurname);
                                if (_meetingsService.GetMeetingsByFilter(x => x.ResponsiblePerson.Equals(filterPerson)).Count == 0)
                                {
                                    Console.WriteLine("No meetings found using this filter.");
                                    break;
                                }
                                PrintAllMeetings(_meetingsService.GetMeetingsByFilter(x => x.ResponsiblePerson.Equals(filterPerson)));
                                break;
                            case "3":
                                Console.WriteLine("Choose one of the categories below by typing in the corresponding number:");
                                Console.WriteLine("1 - CodeMonkey");
                                Console.WriteLine("2 - Hub");
                                Console.WriteLine("3 - Short");
                                Console.WriteLine("4 - TeamBuilding");
                                string filterCategory = Console.ReadLine();
                                if (Enum.TryParse(filterCategory, out Category filteredCategory))
                                {
                                    if (_meetingsService.GetMeetingsByFilter(x => x.Category.ToString().Contains(filteredCategory.ToString())).Count == 0)
                                    {
                                        Console.WriteLine("No meetings found using this filter.");
                                        break;
                                    }
                                    PrintAllMeetings(_meetingsService.GetMeetingsByFilter(x => x.Category.ToString().Contains(filteredCategory.ToString())));
                                }
                                else
                                {
                                    Console.WriteLine("Wrong category chosen. Try again.");
                                }
                                break;
                            case "4":
                                Console.WriteLine("Choose one of the meeting types below by typing in the corresponding number:");
                                Console.WriteLine("1 - Live");
                                Console.WriteLine("2 - In person");
                                string filterType = Console.ReadLine();
                                if (Enum.TryParse(filterType, out MeetingType filteredType))
                                {
                                    if (_meetingsService.GetMeetingsByFilter(x => x.Type.ToString().Contains(filteredType.ToString())).Count == 0)
                                    {
                                        Console.WriteLine("No meetings found using this filter.");
                                        break;
                                    }
                                    PrintAllMeetings(_meetingsService.GetMeetingsByFilter(x => x.Type.ToString().Contains(filteredType.ToString())));
                                }
                                else
                                {
                                    Console.WriteLine("Wrong type chosen. Try again.");
                                }
                                break;
                            case "5":
                                Console.WriteLine("Type in the date from which you would like to start to filter (use format year-month-day hours:minutes):");
                                string fromDateInpt = Console.ReadLine();
                                if (!DateTime.TryParse(fromDateInpt, out DateTime fromDate))
                                {
                                    Console.WriteLine("Start date was entered incorrectly. Try again.");
                                    break;
                                }
                                Console.WriteLine("Type in the date to which you would like to filter (use format year-month-day hours:minutes), leave empty if you don't want to select an end date:");
                                string toDateInpt = Console.ReadLine();
                                if (toDateInpt == "")
                                {
                                    _meetingsService.GetMeetingsByFilter(x => x.StartDate >= fromDate);
                                    break;
                                }
                                if (!DateTime.TryParse(toDateInpt, out DateTime toDate))
                                {
                                    Console.WriteLine("End date was entered incorrectly. Try again.");
                                    break;
                                }
                                if (_meetingsService.GetMeetingsByFilter(x => x.StartDate >= fromDate && x.StartDate <= toDate).Count == 0)
                                {
                                    Console.WriteLine("No meetings found using this filter.");
                                    break;
                                }
                                PrintAllMeetings(_meetingsService.GetMeetingsByFilter(x => x.StartDate >= fromDate && x.StartDate <= toDate));
                                break;
                            case "6":
                                Console.WriteLine("Type in the number of attendees by which you want to filter:");
                                string numberOfAttendees = Console.ReadLine();
                                if (!int.TryParse(numberOfAttendees, out int number) || number < 0)
                                {
                                    Console.WriteLine("You may have made a mistake when entering the number. Try again.");
                                    break;
                                }
                                Console.WriteLine("Type in whether to filter over or under by the number given (type either over or under):");
                                string overUnder = Console.ReadLine();
                                if (overUnder.ToUpper() == "OVER")
                                {
                                    if (_meetingsService.GetMeetingsByFilter(x => x.Attendees.Count >= number).Count == 0)
                                    {
                                        Console.WriteLine("No meetings found using this filter.");
                                        break;
                                    }
                                    PrintAllMeetings(_meetingsService.GetMeetingsByFilter(x => x.Attendees.Count >= number));
                                    break;
                                }
                                else if (overUnder.ToUpper() == "UNDER")
                                {
                                    if (_meetingsService.GetMeetingsByFilter(x => x.Attendees.Count <= number).Count == 0)
                                    {
                                        Console.WriteLine("No meetings found using this filter.");
                                        break;
                                    }
                                    PrintAllMeetings(_meetingsService.GetMeetingsByFilter(x => x.Attendees.Count <= number));
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("There was a mistake when entering filtering condition. Try again.");
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    case "6":
                        loop = false;
                        break;
                    default:
                        Console.WriteLine("You may have made a mistake. Please try again.");
                        break;
                }
            }
            Console.WriteLine("Program exited successfully.");
        }

        public void PrintAllMeetings(List<Meeting> meetings)
        {
            Console.WriteLine(new string('-', 189));
            Console.WriteLine(string.Format(
                "| {0, 15} | {1, 25} | {2, 29} | {3, 20} | {4, 12} | {5, 8} | {6, 16} | {7, 16} | {8, 20} |",
                "Name", "Responsible person's name", "Responsible person's surname", "Description", "Category",
                "Type", "Start date", "End date", "Atendees count"));
            Console.WriteLine(new string('-', 189));
            foreach (var meeting in meetings)
            {
                Console.WriteLine(meeting.ToString());
            }
            Console.WriteLine(new string('-', 189));
        }

        public void PrintAllAttendees(List<Person> attendees)
        {
            Console.WriteLine(new string('-', 37));
            Console.WriteLine(string.Format("| {0, -15} | {1, -15} |", "Name", "Surname"));
            Console.WriteLine(new string('-', 37));
            foreach (var attendee in attendees)
            {
                Console.WriteLine(attendee.ToString());
            }
            Console.WriteLine(new string('-', 37));
        }
    }
}
