# Classware ![GitHub last commit](https://img.shields.io/github/last-commit/ivangeorgiev34/Classware?color=success&style=plastic) ![GitHub repo file count](https://img.shields.io/github/directory-file-count/ivangeorgiev34/Classware?color=informational&logo=files&style=plastic)
Classware is a simple school management system built on ASP.NET MVC that helps teachers and students manage grades, remarks, and compliments for a specific subject, through their accounts, which are made by the admins of the application.
# Table of contents
* [Users and Roles](#users-and-roles)
* [Roles actions](#roles-actions)
* [Chat system](#chat-system)
* [Technologies and Tools](#technologies-and-tools)
* [Database design](#database-design)
## <a id="users-and-roles" name="users-and-roles"></a>Users and roles
There are three roles in the application, each with their separate area:
* Admin role has its admin area
* Teacher role has its teacher area
* Student role has its student area
## <a id="roles-actions" name="roles-actions"></a>Roles actions
### Admin can do the following actions:
* Add a class
* View information about class(see how many students are in the class)
* Delete a class
* Add a teacher
* See all teachers along with information about them
* Delete a teacher
* Assign a subject to a teacher
* Add a student
* View all student along with information about them
* Delete a student
* Assign subjects to a student
* Only admins can create profiles in the system
<!-- end of the list -->
### Teacher can do the following actions:
* View all classes
* View all the student which have the teacher's subject
* View all the grades a student has along with information about it
* Add a grade
* Delete a grade
* Edit a grade
* View all remarks of a student for the teacher's subject
* Add a remark
* Delete a remark
* Edit a remark
* View all compliments of a student for the teacher's subject
* Add a compliment
* Delete a compliment
* Edit a compliment
* View profile information
* Upload or update a profile picture
* Edit his profile information
<!-- end of the list -->
### Student can do the following actions:
* View all subjects along with its grades
* View grade information
* View all remarks
* View remark information
* View all compliments
* View compliment information
* View profile information
* Upload or update profile picture
* Edit his profile information
<!-- end of the list -->
## <a id="chat-system" name="chat-system"></a>Chat system
### How the chat system works:
* Messages can be written to all administrators, by users that are not logged in.
* Messages are stored in a database and all messages that are not answered are displayed when admin accesses the all messages page.
* When someone sends a message it is saved in the database and added to the all messages page, which is only accessed by admins.
* A message can be flagged as answered by the admin answering it.
* Users submit their full name and email for contact information so that administrators can contact them later via email (contacting the users is done outside the application).
<!-- end of the list -->
## <a id="technologies-and-tools" name="technologies-and-tools"></a>Technologies and Tools
- ASP.NET Core MVC 6.0
- Entity Framework Core 6.0
- Bootstrap
- Javascript
- HTML 5
- CSS
- SignalR
- NUnit
- Toastr library
## <a id="database-design" name="database-design">Database Diagram</a>
![ClasswareDatabaseDiagram](Images/classware-database-diagram.JPG?raw=true "Database Diagram")
