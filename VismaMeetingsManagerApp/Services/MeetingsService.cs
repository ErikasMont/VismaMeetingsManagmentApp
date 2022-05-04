using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaMeetingsManagerApp.Interfaces;
using VismaMeetingsManagerApp.Models;
using VismaMeetingsManagerApp.Repositories;

namespace VismaMeetingsManagerApp.Services
{
    public class MeetingsService : IMeetingsService
    {
        private readonly IMeetingsRepository _meetingsRepository;

        public MeetingsService()
        {
            _meetingsRepository = new MeetingsRepository();
        }
        public List<Meeting> GetAllMeetings()
        {
            List<Meeting> meetings = _meetingsRepository.Read();
            return meetings;
        }
        public string CreateMeeting(Meeting meeting)
        {
            List<Meeting> meetings = _meetingsRepository.Read();

            Meeting foundMeeting = meetings.Find(x => x.Name == meeting.Name);

            if(foundMeeting != null)
            {
                return "The meeting is already created.";
            }

            meeting.Attendees.Add(meeting.ResponsiblePerson);
            _meetingsRepository.Create(meeting);
            return "Meeting created successfully.";
        }
        public string DeleteMeeting(Person person, string name)
        {
            List<Meeting> meetings = _meetingsRepository.Read();

            Meeting meeting = meetings.Find(x => x.Name == name);

            if(meeting == null)
            {
                return "There is no such meeting. Try again.";
            }
            if(meeting.ResponsiblePerson.Name != person.Name && meeting.ResponsiblePerson.Surname != person.Surname)
            {
                return "You are not the responsible person for this meeting, therefore you cannot delete this meeting.";
            }

            _meetingsRepository.Delete(meeting);
            return "Meeting deleted successfully.";
        }
        public string AddAttendee(Person person, string name)
        {
            List<Meeting> meetings = _meetingsRepository.Read();

            Meeting meeting = meetings.Find(x => x.Name == name);

            if (meeting == null)
            {
                return "There is no such meeting. Try again.";
            }
            if (meeting.ResponsiblePerson.Equals(person))
            {
                return "Attendee cannot be added, because he is already responsible for this meeting.";
            }
            if (meeting.Attendees.Contains(person))
            {
                return "Attendee is already added to this meeting.";
            }

            List<Meeting> meetingsSameTime = meetings.Where(x => x.StartDate.Equals(meeting.StartDate)).ToList();
            foreach(var meet in meetingsSameTime)
            {
                if (meet.Attendees.Contains(person))
                {
                    Console.WriteLine("Warning! Attendee you are trying to add has another meeting at the same time.");
                    break;
                }
            }

            _meetingsRepository.AddAttendee(meeting, person);
            return "Attendee was added to the meeting successfully.";
        }

        public List<Person> GetAttendees(string name)
        {
            List<Meeting> meetings = _meetingsRepository.Read();

            Meeting meeting = meetings.Find(x => x.Name == name);

            if(meeting == null)
            {
                return null;
            }

            return meeting.Attendees;
        }

        public string RemoveAttendee(Person person, string name)
        {
            List<Meeting> meetings = _meetingsRepository.Read();

            Meeting meeting = meetings.Find(x => x.Name == name);

            if (meeting.ResponsiblePerson.Equals(person))
            {
                return "Attendee cannot be removed, because he is responsible for this meeting.";
            }
            if (!meeting.Attendees.Contains(person))
            {
                return "There is no such attendee at this meeting. Try again.";
            }

            _meetingsRepository.RemoveAttendee(meeting, person);
            return "Attendee was removed successfully.";
        }

        public List<Meeting> GetMeetingsByFilter(Predicate<Meeting> lambda)
        {
            List<Meeting> meetings = _meetingsRepository.Read().FindAll(lambda).ToList();
            return meetings;
        }
    }
}
