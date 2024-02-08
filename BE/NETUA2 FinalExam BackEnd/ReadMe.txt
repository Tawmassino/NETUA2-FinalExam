HUMAN LOGGING API

Entities: 
User:
*Username
*Password
*Salt
*Role
*Human Information reference

Person:
*Name
*Surname
*Personal code
*Telephone number
*email post office
*Profile picture
*LivingLocation reference

LivingLocation:
*City
*Street
*House number
*Flat number

===========================================================================
FEATURES:
User must be able to register.

After registration, a User is created with the default role 'User'.

The user must be able to generate information about himself, in which ALL fields are mandatory (Personal information)
the user must not be able to generate information about more than one person.

There must be different endpoints to update EACH of the fields, e.g.: Name, social security code, phone number, city (cannot be updated to an empty field or whitespace)

When filling out a person's information, a person must upload a profile photo, its size must be reduced to 200x200 (if the photo is too small, it will be stretched to 200x200).

It must be possible to get all the information about the uploaded person according to his ID (the photo is returned as a byte array).

The user must not be able to update information other than his own, 
for the sake of convenience, let's say that with each request "from the frontend" the User ID will come, 
but the recommendation is to accept the User ID from JWT token claims (authorized user).

There must also be an 'Admin' role, which will be determined through the database and it will have an endpoint through which it can delete the user by ID 
(deleting the user also deletes the person's info)

Before loggging in, user is only able to register and log in.

Authentication and Authorization is done with Json Web Tokens.
Mssql database is used.
Entity Framework is used.

===========================================================================

Veikianti programa - 5 balai
Panaudoti interfeisų ir klasių registravimai - 1 taškas
Veikiančios migracijos - 1 taškas
Kodas lengvai suprantamas - 1 taškas
Kodas vadovaujasi single-responsibility principu - 1
Unit testai - 1 taškas
