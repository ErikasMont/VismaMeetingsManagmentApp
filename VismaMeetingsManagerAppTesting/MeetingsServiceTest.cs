using VismaMeetingsManagerApp.Interfaces;
using Xunit;
using Moq;
using VismaMeetingsManagerApp.Models;
using System.Collections.Generic;
using VismaMeetingsManagerApp.Services;
using System;
using System.IO;

namespace VismaMeetingsManagerAppTesting
{
    public class MeetingsServiceTest
    {
        private IMeetingsService _meetingsService;

        public MeetingsServiceTest()
        {
            _meetingsService = new MeetingsService();
            if (File.Exists("Meetings.json"))
            {
                File.Delete("Meetings.json");
            }
            SeedData(_meetingsService);
        }

        [Fact]
        public void GetAllMeetings_WhenCalled_AListOfMeetingsReturned()
        {
            //Arrange
            Person person1 = new Person("test1", "test1");
            Person person2 = new Person("test2", "test2");
            List<Meeting> expected = new List<Meeting>()
            {
                new Meeting("name", person1, "desc", Category.CodeMonkey, MeetingType.InPerson, DateTime.Now, DateTime.Now.AddHours(1)),
                new Meeting("name2", person2, "test keyword .net", Category.TeamBuilding, MeetingType.InPerson, DateTime.Now, DateTime.Now.AddHours(2))
            };
            
            //Act
            var actual = _meetingsService.GetAllMeetings();

            //Assert
            Assert.IsType<List<Meeting>>(actual);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.Count, actual.Count);
        }

        [Fact]
        public void CreateMeeting_WhenExistingMeetingGiven_ReturnsErrorMessage()
        {
            //Arrange
            Person person = new Person("test1", "test2");
            Meeting meeting = new Meeting("name", person, "desc", Category.CodeMonkey, MeetingType.InPerson, DateTime.Now, DateTime.Now.AddHours(1));
            var expected = "The meeting is already created.";
            List<Meeting> meetingsBefore = _meetingsService.GetAllMeetings();

            //Act
            var actual = _meetingsService.CreateMeeting(meeting);
            List<Meeting> meetingsAfter = _meetingsService.GetAllMeetings();

            //Assert
            Assert.IsType<string>(actual);
            Assert.Equal(expected, actual);
            Assert.Equal(meetingsBefore.Count, meetingsAfter.Count);
        }

        [Fact]
        public void CreateMeeting_WhenNewMeetingGiven_ReturnsSuccessMessage()
        {
            //Arrange
            Person person = new Person("name", "surename");
            Meeting meeting = new Meeting("meeting", person, "coding academy", Category.CodeMonkey, MeetingType.InPerson, DateTime.Now, DateTime.Now.AddHours(1));
            var expected = "Meeting created successfully.";
            List<Meeting> meetingsBefore = _meetingsService.GetAllMeetings();

            //Act
            var actual = _meetingsService.CreateMeeting(meeting);
            List<Meeting> meetingsAfter = _meetingsService.GetAllMeetings();

            //Assert
            Assert.IsType<string>(actual);
            Assert.Equal(expected, actual);
            Assert.Contains(meeting, meetingsAfter);
            Assert.Equal(meetingsBefore.Count + 1, meetingsAfter.Count);
        }

        [Fact]
        public void DeleteMeeting_WhenNotExistingMeetingGiven_ReturnsErrorMessage()
        {
            //Arrange
            Person person = new Person("name", "surname");
            string name = "random";
            var expected = "There is no such meeting. Try again.";
            List<Meeting> meetingsBefore = _meetingsService.GetAllMeetings();

            //Act
            var actual = _meetingsService.DeleteMeeting(person, name);
            List<Meeting> meetingsAfter = _meetingsService.GetAllMeetings();

            //Assert
            Assert.IsType<string>(actual);
            Assert.Equal(expected, actual);
            Assert.Equal(meetingsBefore.Count, meetingsAfter.Count);
        }

        [Fact]
        public void DeleteMeeting_WhenNotResponsiblePersonGiven_ReturnsErrorMessage()
        {
            //Arrange
            Person person = new Person("name", "surname");
            string name = "name2";
            var expected = "You are not the responsible person for this meeting, therefore you cannot delete this meeting.";
            List<Meeting> meetingsBefore = _meetingsService.GetAllMeetings();

            //Act
            var actual = _meetingsService.DeleteMeeting(person, name);
            List<Meeting> meetingsAfter = _meetingsService.GetAllMeetings();

            //Assert
            Assert.IsType<string>(actual);
            Assert.Equal(expected, actual);
            Assert.Equal(meetingsBefore.Count, meetingsAfter.Count);
        }

        [Fact]
        public void DeleteMeeting_WhenExistingMeetingAndResponsiblePersonGiven_ReturnsSuccessMessage()
        {
            //Arrange
            Person person = new Person("test2", "test2");
            string name = "name2";
            var expected = "Meeting deleted successfully.";
            List<Meeting> meetingsBefore = _meetingsService.GetAllMeetings();

            //Act
            var actual = _meetingsService.DeleteMeeting(person, name);
            List<Meeting> meetingsAfter = _meetingsService.GetAllMeetings();

            //Assert
            Assert.IsType<string>(actual);
            Assert.Equal(expected, actual);
            Assert.Equal(meetingsBefore.Count - 1, meetingsAfter.Count);
        }

        [Fact]
        public void AddAttendee_WhenNotExistingMeetingGiven_ReturnsErrorMessage()
        {
            //Arrange
            Person person = new Person("name", "surname");
            string name = "random";
            var expected = "There is no such meeting. Try again.";

            //Act
            var actual = _meetingsService.AddAttendee(person, name);

            //Assert
            Assert.IsType<string>(actual);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddAttendee_WhenResponsiblePersonGivenAsAttendee_ReturnsErrorMessage()
        {
            //Arrange
            Person person = new Person("test2", "test2");
            string name = "name2";
            var expected = "Attendee cannot be added, because he is already responsible for this meeting.";

            //Act
            var actual = _meetingsService.AddAttendee(person, name);

            //Assert
            Assert.IsType<string>(actual);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddAttendee_WhenAttendeeIsAlreadyAdded_ReturnsErrorMessage()
        {
            //Arrange
            Person person = new Person("test3", "test3");
            string name = "name2";
            var expected = "Attendee is already added to this meeting.";
            var temp = _meetingsService.AddAttendee(person, name);

            //Act
            var actual = _meetingsService.AddAttendee(person, name);

            //Assert
            Assert.IsType<string>(actual);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddAttendee_WhenNewAttendeeGiven_ReturnsSuccessMessage()
        {
            //Arrange
            Person person = new Person("test3", "test3");
            string name = "name2";
            var expected = "Attendee was added to the meeting successfully.";
            List<Person> attendeesBefore = _meetingsService.GetAttendees(name);

            //Act
            var actual = _meetingsService.AddAttendee(person, name);
            List<Person> attendeesAfter = _meetingsService.GetAttendees(name);

            //Assert
            Assert.IsType<string>(actual);
            Assert.Equal(expected, actual);
            Assert.Equal(attendeesBefore.Count + 1, attendeesAfter.Count);
            Assert.Contains(person, attendeesAfter);
        }

        [Fact]
        public void GetAttendees_WhenExistingMeetingGiven_AListOfAttendeesReturned()
        {
            //Arrange
            Person person1 = new Person("test2", "test2");
            Person person2 = new Person("test3", "test3");
            List<Person> expected = new List<Person>()
            {
                person1,
                person2
            };

            //Act
            var temp = _meetingsService.AddAttendee(person2, "name2");
            var actual = _meetingsService.GetAttendees("name2");

            //Assert
            Assert.IsType<List<Person>>(actual);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.Count, actual.Count);
        }

        [Fact]
        public void RemoveAttendee_WhenResponsiblePersonGivenToRemove_ReturnsErrorMessage()
        {
            //Arrange
            Person person = new Person("test2", "test2");
            string name = "name2";
            var expected = "Attendee cannot be removed, because he is responsible for this meeting.";
            List<Person> attendeesBefore = _meetingsService.GetAttendees(name);

            //Act
            var actual = _meetingsService.RemoveAttendee(person, "name2");
            List<Person> attendeesAfter = _meetingsService.GetAttendees(name);

            //Assert
            Assert.IsType<string>(actual);
            Assert.Equal(expected, actual);
            Assert.Equal(attendeesBefore.Count, attendeesAfter.Count);
        }

        [Fact]
        public void RemoveAttendee_WhenNotExistantAttendeeGiven_ReturnsErrorMessage()
        {
            //Arrange
            Person person = new Person("test10", "test10");
            string name = "name2";
            var expected = "There is no such attendee at this meeting. Try again.";
            List<Person> attendeesBefore = _meetingsService.GetAttendees(name);

            //Act
            var actual = _meetingsService.RemoveAttendee(person, name);
            List<Person> attendeesAfter = _meetingsService.GetAttendees(name);

            //Assert
            Assert.IsType<string>(actual);
            Assert.Equal(expected, actual);
            Assert.Equal(attendeesBefore.Count, attendeesAfter.Count);
        }

        [Fact]
        public void RemoveAttendee_WhenExistantAttendeeGiven_ReturnsSuccessMessage()
        {
            //Arrange
            Person person = new Person("test3", "test3");
            string name = "name2";
            var expected = "Attendee was removed successfully.";
            var temp = _meetingsService.AddAttendee(person, name);
            List<Person> attendeesBefore = _meetingsService.GetAttendees(name);

            //Act
            var actual = _meetingsService.RemoveAttendee(person, name);
            List<Person> attendeesAfter = _meetingsService.GetAttendees(name);

            //Assert
            Assert.IsType<string>(actual);
            Assert.Equal(expected, actual);
            Assert.Equal(attendeesBefore.Count - 1, attendeesAfter.Count);
        }

        [Fact]
        public void GetMeetingsByFilter_WhenCalled_AListOfFilteredMeetingsReturned()
        {
            //Arrange
            Person person = new Person("test2", "test2");
            List<Meeting> expected = new List<Meeting>()
            {
                new Meeting("name2", person, "test keyword .net", Category.TeamBuilding, MeetingType.InPerson, DateTime.Now, DateTime.Now.AddHours(2))
            };
            string keyword = ".net";

            //Act
            var actual = _meetingsService.GetMeetingsByFilter(x => x.Description.Contains(keyword));

            //Assert
            Assert.IsType<List<Meeting>>(actual);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.Count, actual.Count);
        }

        private void SeedData(IMeetingsService meetingsService)
        {
            Person person1 = new Person("test1", "test1");
            Person person2 = new Person("test2", "test2");
            Meeting meeting1 = new Meeting("name", person1, "desc", Category.CodeMonkey, MeetingType.InPerson, DateTime.Now, DateTime.Now.AddHours(1));
            Meeting meeting2 = new Meeting("name2", person2, "test keyword .net", Category.TeamBuilding, MeetingType.InPerson, DateTime.Now, DateTime.Now.AddHours(2));
            meetingsService.CreateMeeting(meeting1);
            meetingsService.CreateMeeting(meeting2);
        }
    }
}