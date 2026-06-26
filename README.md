Energy Core Library
===================

**Energy.Core** is a **.NET** class library which boosts your program with functionality
covering various type conversions, utility classes, database abstraction layer,
object mapping and simple application framework.

The library is designed to run wherever .NET runs. A single consistent API spans
**.NET 2.0** through **.NET Standard 2.0**, so the same code compiles for a legacy
Windows CE device and a modern cloud microservice without any changes.

Whether you need safe number parsing, color-coded console output, structured logging,
SQL query construction, HTTP requests, binary compression, or a thread-safe database
connection, Energy.Core covers it with minimal friction and no unnecessary dependencies.

☢️ Filled with radioactive rays ☢️

Versioning
----------

**Energy.Core** uses calendar-style version numbers in the form ``YY.M.R``. ``YY`` is the two-digit year,
``M`` is the month number, and ``R`` is the zero-based release counter for that month.
For example, version ``22.6.1`` denotes the second release (counting from 0) published in June 2022.

Installation
------------

The easiest way is to install using **NuGet** either by finding the **Energy.Core** package
in the official gallery or by executing one of the following commands.

Package Manager:

```
Install-Package Energy.Core
```

.NET CLI:

```
dotnet add package Energy.Core
```

The installation package contains versions for **.NET 4**, **.NET Standard** / **.NET Core**
and legacy **.NET 2**. NuGet will choose the appropriate version automatically.

For Windows CE development the package needs to be downloaded manually from [download section](download/).

Documentation
-------------

http://energy-core.readthedocs.io/

For developers working on the library, see [Development Guidelines](docs/GUIDELINES.md) for coding standards,
compilation requirements, and platform-specific considerations.

For information about the project structure and build files, see [Project Structure](docs/STRUCTURE.md).

The development process is documented in [Workflow](docs/WORKFLOW.md), [Testing](docs/TESTING.md),
and [Versioning](docs/VERSIONING.md). Templates for change requests and implementation plans live
under [docs/template](docs/template).

Examples
--------

### Safely converting value types ###

Conversion functions are in **Energy.Base.Cast**. They try to convert a value to the desired type
and return a default when conversion is impossible, so there are no exceptions to catch.

```cs
int numberInt = Energy.Base.Cast.StringToInteger("123");
long numberLong = Energy.Base.Cast.StringToLong("1234567890");
double numberDouble = Energy.Base.Cast.StringToDouble("3.1415"); // also accepts "3,1415"
Console.WriteLine(Energy.Base.Cast.DoubleToString(numberDouble));
```

The last line always produces the culture-invariant form `3.1415`.

Both parsing directions work: string to numeric, numeric to string, and between numeric types,
all with graceful handling of nulls, empty strings, and out-of-range inputs.

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

### Colorful console output ###

**Energy.Core.Tilde** is a color-text engine for console applications. Embed color markers
using the tilde character (`~`) to switch between the sixteen standard console colors inline.

```cs
Energy.Core.Tilde.WriteLine("~green~Success:~white~ all tests passed.");
Energy.Core.Tilde.WriteLine("~red~Error:~0~ connection refused.");
Energy.Core.Tilde.WriteLine("~yellow~Warning:~0~ deprecated API in use.");
Energy.Core.Tilde.WriteLine("~red~Breaking: ~white~Hell, ~yellow~yea.");
```

Colors can be referenced by full name (`~green~`, `~white~`, `~red~`) or by short alias
(`~g~`, `~w~`, `~r~`). Use `~0~` or `~default~` to restore the terminal's original color.

The engine has no dependencies on platform-specific APIs beyond `System.Console`, so it
works identically on Windows, Linux, and macOS.

### Logging events ###

**Energy.Core.Log** provides structured, multi-destination logging. Set up the global
singleton once and it writes to the console, a file, or both simultaneously.

```cs
Energy.Core.Log.Default.Setup("app.log", true, null);

Energy.Core.Log.Default.Write("Application started");
Energy.Core.Log.Default.Write("Connection failed", Energy.Enumeration.LogLevel.Error);
Energy.Core.Log.Default.Write(exception);
```

Log entries carry a message, a severity level, a source name, an error code, a timestamp,
and an optional exception, so structured processing and filtering are straightforward.

### Waiting for user input on console ###

When you call **Console.ReadLine()** the program stops until the user presses Enter.
**Energy.Core.Tilde.ReadLine()** returns `null` while nothing has been entered yet,
letting your program continue running and poll for input when convenient.

### Compress and decompress data ###

**Energy.Base.Compression** provides several algorithms out of the box.

```cs
byte[] data = System.Text.Encoding.UTF8.GetBytes("Hello, Energy.Core!");

// Deflate
byte[] compressed     = Energy.Base.Compression.Deflate.Compress(data);
byte[] decompressed   = Energy.Base.Compression.Deflate.Decompress(compressed);

// GZip
byte[] gzipped        = Energy.Base.Compression.GZip.Compress(data);
byte[] ungzipped      = Energy.Base.Compression.GZip.Decompress(gzipped);

// ZX0 - optimal sliding-window compression for small binary payloads
byte[] zx0Packed      = Energy.Base.Compression.ZX0.Compress(data);
byte[] zx0Unpacked    = Energy.Base.Compression.ZX0.Decompress(zx0Packed);

// LZ4 - fast block-format compression
byte[] lz4Packed      = Energy.Base.Compression.LZ4.Compress(data);
byte[] lz4Unpacked    = Energy.Base.Compression.LZ4.Decompress(lz4Packed);

// LZSS - sliding-window algorithm suited for repetitive binary data
byte[] lzssPacked     = Energy.Base.Compression.LZSS.Compress(data);
byte[] lzssUnpacked   = Energy.Base.Compression.LZSS.Decompress(lzssPacked);
```

### Naming convention helpers ###

**Energy.Base.Naming** converts a word list to any common identifier style.

```cs
string[] words = new string[] { "user", "profile", "name" };

string camel    = Energy.Base.Naming.CamelCase(words);    // userProfileName
string pascal   = Energy.Base.Naming.PascalCase(words);   // UserProfileName
string snake    = Energy.Base.Naming.SnakeCase(words);    // user_profile_name
string kebab    = Energy.Base.Naming.KebabCase(words);    // user-profile-name
string constant = Energy.Base.Naming.ConstantCase(words); // USER_PROFILE_NAME
string train    = Energy.Base.Naming.TrainCase(words);    // User-Profile-Name
```

Each style also has a corresponding `Is*` predicate for validation,
such as `Energy.Base.Naming.IsCamelCase(text)`.

### Working with INI configuration files ###

```cs
Energy.Base.Ini.IniFile ini = new Energy.Base.Ini.IniFile();
ini.Load("settings.ini");

string host = ini["database"]["host"];
string port = ini["database"]["port"];
```

The parser supports configurable comment characters, key-value separators, and section brackets,
so it adapts to most INI dialects without preprocessing the file.

### Easily make REST requests ###

```cs
string url = "https://www.google.com/search?q=Energy";
Console.WriteLine(Energy.Core.Web.Get(url).Body);
```

Easy to use, built upon standard **System.Net.WebRequest** class REST functions available
for common methods like GET, POST, PUT, PATCH, DELETE, HEAD or OPTIONS.

### Generic SQL database connection ###

Here database connection is made using a general connection class cooperating with
each **ADO.NET** provider of the database connection.

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

Connections are thread-safe, may be cloned or even set to be persistent if you want
to limit connections to your SQL database.

Feature Overview
----------------

The table below gives a quick map of what Energy.Core covers.

| Area               | Primary class                                | What you get                                                               |
| ------------------ | -------------------------------------------- | -------------------------------------------------------------------------- |
| Type conversion    | `Energy.Base.Cast`                           | Safe string-to-number and cross-type conversions with sensible defaults    |
| Text manipulation  | `Energy.Base.Text`                           | Trim, pad, capitalize, replace, split with cross-platform newline handling |
| Naming conventions | `Energy.Base.Naming`                         | camelCase, PascalCase, snake_case, kebab-case, CONSTANT_CASE, Train-Case   |
| Hex encoding       | `Energy.Base.Hex`                            | Encode, decode, pretty-print hex dumps with ASCII column                   |
| Hashing            | `Energy.Base.Hash`                           | MD5, SHA1, SHA256, SHA384, SHA512 and CRC32                                |
| Compression        | `Energy.Base.Compression`                    | Deflate, GZip, LZ4, LZSS sliding-window, ZX0                               |
| Color values       | `Energy.Base.Color`                          | RGB/ARGB struct, HSL conversion, HTML and hex color parsing                |
| Binary data        | `Energy.Base.Binary`, `Energy.Base.Bit`      | Bit readers/writers, byte utilities, BCD encoding, nibble handling         |
| Collections        | `Energy.Base.Collection`                     | StringDictionary, StringArray comparison, typed list helpers               |
| Date and time      | `Energy.Base.Date`, `Energy.Base.Clock`      | Date arithmetic, elapsed time, clock helpers, cross-culture formatting     |
| File system        | `Energy.Base.File`, `Energy.Base.Path`       | Cross-platform read/write text and binary, path normalization              |
| Network helpers    | `Energy.Base.Network`, `Energy.Core.Network` | IP and MAC address parsing, host resolution utilities                      |
| HTTP / REST        | `Energy.Core.Web`, `Energy.Base.Http`        | GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS                               |
| INI files          | `Energy.Base.Ini`                            | Parse and write INI configuration files                                    |
| CSV                | `Energy.Base.Csv`                            | Parse and produce comma-separated values                                   |
| JSON               | `Energy.Base.Json`                           | Lightweight JSON tokenizer and value extractor                             |
| XML                | `Energy.Base.Xml`                            | XML parsing and generation helpers                                         |
| SQL connection     | `Energy.Source.Connection<T>`                | Thread-safe ADO.NET wrapper with pooling, clone, scalar, and execute       |
| SQL queries        | `Energy.Query`                               | Dialect-aware query builder for ANSI, MySQL, SQLite, and SQL Server        |
| Object mapping     | `Energy.Source.Map`                          | Map DataRow columns to object properties                                   |
| Logging            | `Energy.Core.Log`                            | Structured logging to console, file, or custom event handler               |
| Console coloring   | `Energy.Core.Tilde`                          | Inline color markup engine for console programs                            |
| Application frame  | `Energy.Core.Application`                    | Structured application host with config, log, connection, and locale       |
| Threading          | `Energy.Core.Worker<T>`                      | Generic thread-worker base class with start/stop lifecycle                 |
| Random data        | `Energy.Base.Random`                         | Random byte arrays, integers, and strings                                  |
| Benchmarking       | `Energy.Core.Benchmark`                      | Elapsed-time measurement for performance profiling                         |
| Monetary values    | `Energy.Base.Money`                          | Currency-typed monetary values with arithmetic and equality                |
| Geolocation        | `Energy.Base.Geo`                            | Latitude and longitude coordinate type                                     |
| Pattern matching   | `Energy.Base.Pattern`                        | Wildcard and glob pattern matching                                         |
| Encryption         | `Energy.Base.Cipher`                         | Symmetric cipher helpers                                                   |

Content
-------

Library has been divided into several different namespaces. Following table briefly describes
the purpose of each of them.

 - *Energy.Base* - Core utility classes covering conversions, text, binary data, hashing, compression,
   file I/O, collections, network helpers, and format parsers for INI, CSV, JSON, XML, and YAML
 - *Energy.Core* - Application framework functions: logging, colorful console output, HTTP client,
   application host, configuration, threading workers, and benchmarking
 - *Energy.Query* - SQL query building with dialect support for ANSI SQL, MySQL, SQLite, and SQL Server
 - *Energy.Source* - Database abstraction layer: thread-safe ADO.NET connection, object-to-row mapping,
   schema inspection, and connection factory
 - *Energy.Attribute* - Custom attributes for metadata, code annotations, and data binding
 - *Energy.Enumeration* - Shared enumerations used across the library
 - *Energy.Interface* - Interfaces for common lifecycle patterns: start/stop, socket client and server,
   file load/save, logger, and worker

History
-------

Working for many years on different development projects, from simple applications, web applications,
to a rich and monolithic production environment with plenty of small software programs that act as interfaces
and all kinds of small services, as most of you have probably noticed that some part of the functionality
is repeated to a greater or lesser extent regardless of the project type.

This library was created completely independently from professional work as an attempt to build a "base",
which can be quickly used in almost any project in order not to repeat again the same codes to achieve
functionality like safe (international) type conversion or generic database connection which is easy
and most importantly safe to use.

Greetings
---------

To be continued...

❤️ Made with love for you ❤️
