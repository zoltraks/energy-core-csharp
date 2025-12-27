Energy Core Library
===================

**Energy.Core** is a **.NET** class library which boosts your program with functionality
covering various type conversions, utility classes, database abstraction layer,
object mapping and simple application framework.

To be used by wide range of applications, services, web or console programs.

☢️ Filled with radioactive rays ☢️

Installation
------------

The easiest way is install using **nuget** either by finding **Energy.Core** package in official gallery or by executing command.

```
Install-Package Energy.Core
```

Installation package contains versions for **.NET 4**, **.NET Standard** / **.NET Core** and legacy **.NET 2**. Nuget should choose apropriate version automatically.

For Windows CE development package needs to be downloaded manually from [download section](download/).

Documentation
-------------

http://energy-core.readthedocs.io/

For developers working on the library, see [Development Guidelines](docs/GUIDELINES.md) for coding standards, compilation requirements, and platform-specific considerations.

For information about the project structure and build files, see [Project Structure](docs/STRUCTURE.md).

Examples
--------

### Safely converting value types ###

Conversion functions are located in **Energy.Base.Cast** class. These functions will try to convert value to desired type or use defaults if value cannot be converted.

```cs
int numberInt = Energy.Base.Cast.StringToInteger("123");
long numberLong = Energy.Base.Cast.StringToLong("1234567890");
double numberDouble = Energy.Base.Cast.StringToDouble("3.1415"); // or "3,1415"
Console.WriteLine(Energy.Base.Cast.DoubleToString(numberDouble));
```

Last will always result in culture invariant version "3.1415".

### Displaying bytes in pretty format ###

```cs
byte[] array = Energy.Base.Random.GetRandomByteArray(40);
Console.WriteLine(Energy.Base.Hex.Print(array));
```

Which may result in example output.

```
2c c5 31 be  de 96 fb 5a  76 53 b7 84  2c 09 8d 16   ,.1....ZvS..,...
88 0f c5 6c  50 c3 69 51  48 99 4b 9f  53 00 79 89   ...lP.iQH.K.S y.
1d c9 de c6  4a c9 dc e2                             ....J...
```

This was very basic usage but you may extend it with different formatting options, offsets and even coloring.

### Waiting for user input on console ##

When you call **Console.ReadLine()** program will stop and wait for user input. If you want to stop only when user enters data, use **Energy.Core.Tilde.ReadLine()** which will result in *null* as long as user has not accepted its input by pressing Enter key allowing your program to do its job.

### Easily make REST requests ###

How about:

```cs
string url = "https://www.google.com/search?q=Energy";
Console.WriteLine(Energy.Core.Web.Get(url).Body);
```

Easy to use, build upon standard **System.Net.WebRequest** class REST functions available for common methods like GET, POST, PUT, PATCH, DELETE, HEAD or OPTIONS.

### Generic SQL database connection ###

Here database connection is made using  general connection class cooperating with each **ADO.NET** provider of the database connection.

```cs
Energy.Source.Connection<MySql.Data.MySqlClient.MySqlConnection> db
    = new Energy.Source.Connection<MySql.Data.MySqlClient.MySqlConnection>();
db.ConnectionString = @"Server=127.0.0.1;Database=test;Uid=test;Pwd=test;";
if (!db.Test())
{
    Console.WriteLine("Connection test error");
}
else
{
    string result = db.Scalar<string>("SELECT CURRENT_TIMESTAMP()");
    Console.WriteLine("Current server time is: {0}", result);
}
```

Connections are thread safe, may be cloned or even set to be persistent if you want to limit connections to your SQL database.


Content
-------

Library has been divided into several different namespaces. Following table briefly describes the purpose of each of them.

 - *Energy.Base* - Contains base classes
 - *Energy.Core* - Library functions
 - *Energy.Attribute* - Attributes
 - *Energy.Enumeration* - Enumerations
 - *Energy.Interface* - Interfaces
 - *Energy.Source* - Database connection

History
-------

Working for many years on different development projects, from simple applications, web applicatons, to a rich and monolithic production environment with plenty of small software programs that act as interfaces and all kinds of small services, as most of you have probably noticed that some part of the functionality is repeated to a greater or lesser extent regardless of the project type.

This library was created completely independently from my professional work as an attempt to build a "base", which can be quickly used in almost any project in order not to repeat again the same codes to achieve functionality like safe (international) type conversion or generic database connection which is easy and most importantly safe to use.

Greetings
---------

To be continued...

❤️ Made with love for you ❤️
