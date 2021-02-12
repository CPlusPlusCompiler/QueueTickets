# QueueTickets

Virtual tickets giving web app. Built with .NET Core and Entity Framework.

#### How to run
First, a running SQL database is needed. Here is used connection string:
``` "DefaultConnection": "Server=localhost;Database=database.db;Trusted_Connection=False;MultipleActiveResultSets=true;user id=sa;password=SQLPassword-2" ```

with CLI:
```sh
dotnet run
```
Or use your preffered IDE

#### How to use
If running on your local machine, the base url is this:
```sh
localhost:5001/
```

##### Reserve a time for a meeting
You can reserve a meeting for current day only. The employees may have varying schedules, so that is taken into account as well. This method tries to distribute work loads equally between employees.

```sh
POST
api/Meetings/BookMeeting
[Body] string Name
[Returns] Ticket VisitInformation
```

##### Cancel a meeting
```sh
POST
api/Meetings/CancelMeeting
[Body] string ticketId
[Returns] Ok
```


##### Register an employee
We can have more than one employee working at the same time. But first, the database needs to be filled with users and passwords. Passwords are encrypted and use salt so it would be inconvinient to do it by hand.
```sh
api/SpecialistAuth/register
POST
[Body]
{
    string Name
    string Surname
    string Username
    string Password
}
[Returns] long id
[Error] string (username already exists)
```

##### Login to get a Jwt token
To do any operations as an employee, a valid Jwt token is needed. To generate one:
```sh
POST
api/SpecialistAuth/login
[Body]
{
    string Username
    string Password
}
[Returns]
{
    string name
    string surname
    string username
    string token
}
[Error] string (bad login data)
```

##### Getting current and future visits
Requires a Jwt token in the header. It already contains user id so no need to supply any additional data.
```sh
GET 
api/SpecialistConsole/getcurrentvisits
[Returns]
{
    Ticket currentVisit
    List<Ticket> upcomingVisits
}
```

##### Cancel, mark visit as started or ended
All three endpoints are POST and have the same body.
```sh
api/SpecialistConsoleController/CancelVisit
api/SpecialistConsoleController/SetVisitStarted
api/SpecialistConsoleController/SetVisitEnded
[Body] string ticketId
[Returns] Ok
[Error] string (Incorrect ticket id)
```
