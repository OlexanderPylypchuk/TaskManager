# TaskManager
<h2>How to run?</h2>
<p>To run this project, you need to:</p>
<ol>
  <li>
    Install project on your machine
  </li>
  <li>
    Change server name in application.json to that one your machine has
  </li>
  <li>
    Run the app, all migrations will be applied automatically :)
  </li>
</ol>
<h2>Endpoints</h2>
<p>API runs on https://localhost:7145 (changable) and has 2 controllers: UserController and TaskController. Lets walk over their endpoints:</p>
<h3>UserController</h3>
<ul>
  <li>
    /users/login - POST, allows users to log in using LoginRequestDto, which consists of Username and Password fields. If login was successful, user gets back responce with token inside, which they can use to authorize and access TaskController endpoints.
  </li>
  <li>
    /users/register - POST, allows user to register using RegistrationRequestDto, which consists of Email, Password and Username fields. If success, user is created and saved in db, with hashed password.
  </li>
</ul>
<h3>TaskController</h3>
<ul>
  <li>
    /tasks - GET, returns authorized user list of their tasks. Can be sorted by Priority, DueDate, Status, and ordered by Priority and/or DueDate.
  </li>
  <li>
    /tasks/{id} - GET, returns user task based on id of that task if this task is assigned to that user. Else, returns exception.
  </li>
  <li>
    /task - POST, creates task based on TaskOfUserDto, which contains Id, Name, Description, Status, Priority, DueDate and UserId as fields. Id and UserId are not requiered inputs, as they are changed authomatically, in case of UserId based on user that sends data. 
  </li>
  <li>
    /task - PUT, updates task based on same TaskOfUserDto. Does NOT update such fields as Id, UserId and CreatedTime.
  </li>
  <li>
    /task/{id} - DELETE, removes task with specified id from db, if it is assign to the user making request.
  </li>
</ul>
<h2>Authentication</h2>
<p>For authentication I made simple JwtTokenGenerator, that has claims for user Id, email, username. I also created PasswordHasher, that uses Pbkdf2 and HMACSHA256 algorithms. Finally, I created AuthService, that allows user to login and register, using PasswordHasher and JwtTokenGenerator to do it. It also tells user if their password is too easy (like special characters, number, upper/lowercase letters, etc.)</p>

<h1>For now, thats it, be happy to use it or copy my code if you want it for some reason XD</h1>
